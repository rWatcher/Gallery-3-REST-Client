'  Gallery 3 REST Client
'  Copyright 2010 Eric Cavaliere
'
'  This program is free software; you can redistribute it and/or modify
'  it under the terms of the GNU General Public License as published by
'  the Free Software Foundation; either version 2 of the License, or (at
'  your option) any later version.
'
'  This program is distributed in the hope that it will be useful, but
'  WITHOUT ANY WARRANTY; without even the implied warranty of
'  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
'  General Public License for more details.
'
'  You should have received a copy of the GNU General Public License
'  along with this program; if not, write to the Free Software
'  Foundation, Inc., 51 Franklin Street - Fifth Floor, Boston, MA  02110-1301, USA.
'
Imports Newtonsoft.Json
Imports GalleryLib


Public Partial Class FormAlbumBrowser
	
	' Set up a few global variables to be used throughout this form.
    Public GalleryClient As Gallery3.Client
    Dim strDataFolder As String = ""
    Dim strCacheFolder As String = ""
    Public boolClosing as Boolean = False

	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub FormAlbumBrowserLoad(sender As Object, e As EventArgs)
		' Handles initialing loading the album window.
		'   Download details of Item #1 (the root album)
		'   and all of its member items, also handle initial
		'   setup of the form.
		
		' Figure out where the data and cache folders are.
		If System.IO.Directory.Exists(Application.StartupPath & "\data") Then
			strDataFolder = Application.StartupPath & "\data"
			strCacheFolder = strDataFolder & "\cache"
		Else
			strDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Gallery3Client"
			strCacheFolder = strDataFolder & "\cache"
		End If
		
		' Display a status message and run DoEvents
		'  to make sure the message is displayed.
        labelGalleryStatus.Text = "Loading Albums"
        Application.DoEvents()
        
        ' Attempt to download the details of the root Gallery album.
        Dim rootNode As New TreeNode
        Dim RootItem As Linq.JObject = GalleryClient.GetItem(1)
        
        ' In the event of an error, ask the user if they want to try again or quit.
        While RootItem Is Nothing
        	If MessageBox.Show ("Unable to access root album, try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = DialogResult.No Then
        		Application.Exit()
        		Exit Sub
        	End If
        	GalleryClient.ItemCache.RemoveItem(1)
        	RootItem = GalleryClient.GetItem(1)
        End While
        
        ' If itemchecksums isn't present in the root album data,
        '   assume the module is not installed remotely and disable
        '   the menu option for the file comparison feature.
        If RootItem("relationships").Item("itemchecksums") Is Nothing Then
            CompareToLocalFolderToolStripMenuItem.Enabled = False
        End If
        
        ' Add the root album to treeAlbums.
        rootNode.Text = RootItem("entity").Item("title")
        rootNode.Tag = RootItem("entity").Item("id").ToString.Replace("""", "")
        treeAlbums.Nodes.Add(rootNode)
        treeAlbums.ExpandAll()
        
        ' Set the status to Ready.
        labelGalleryStatus.Text = "Ready"
	End Sub ' END FormAlbumBrowserLoad
	
	Sub TreeAlbumsAfterSelect(sender As Object, e As TreeViewEventArgs)
        ' Download the details of the currently selected album.
        '   Loop through each "member" item and load into the form.
        
        ' Change the status, and reset listPictures and ImageListThumbs
        '   before loading the new album.
        labelGalleryStatus.Text = "Loading Contents of " & treeAlbums.SelectedNode.Text
        Application.DoEvents()
        listPictures.Items.Clear()
        ImageListThumbs.Images.Clear()
        
        Try
        	' Get the details on the newly selected item,
        	'  In the event of an error, exit the sub.
        	Dim SelectedAlbumID = treeAlbums.SelectedNode.Tag
        	Dim SelectedAlbum As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(treeAlbums.SelectedNode.Tag))
        	If SelectedAlbum Is Nothing Then
        		Exit Sub
        	End If
        	
        	' Download detailed info on all the items in the selected album
        	Dim ChildItems As List(Of String) = GalleryClient.GetItems(SelectedAlbum("members"))
        	
        	If Not ChildItems Is Nothing Then
        		' Add a default thumb to ImageListThumbs to be displayed
        		'  while the actual thumbs are downloading, or in the event
        		'  of a download error.
        		ImageListThumbs.Images.Add(0, AspectedImage(Application.StartupPath & "\default.png", 64,64))
        		
        		' Loop through each item in the current album and add to the corresponding control.
        		Dim OneChild As String
        		Dim counter As Integer = 1
        		For Each OneChild In ChildItems
        			
        			' Get the details for the current item and set the status.
        			labelGalleryStatus.Text = "Loading Contents of " & treeAlbums.SelectedNode.Text & " (" & counter.ToString & " of " & SelectedAlbum("members").Count.ToString & ")"
        			Dim OneChildData As Linq.JObject = Linq.JObject.Parse(OneChild)
        			
        			' If it's an album, add it to treeAlbums
        			If (OneChildData("entity").Item("type").ToString = """album""") Then
        				Dim SubAlbumTree As New TreeNode
        				SubAlbumTree.Text = OneChildData("entity").Item("title")
        				SubAlbumTree.Tag = OneChildData("entity").Item("id").ToString.Replace("""", "")
        				Dim SearchTree As New TreeNode
        				
        				' Make sure the album isn't already in treeAlbums before adding it.
        				Dim NodeLoaded As Boolean = False
        				For Each SearchTree In treeAlbums.SelectedNode.Nodes
        					If SearchTree.Tag = SubAlbumTree.Tag Then
        						NodeLoaded = True
        						Exit For
        					End If
        				Next
        				
        				' If the user clicks into another album while this one is still loading, abort.
        				'  or else, load the album (if it doesn't already exist).
        				If NodeLoaded = False Then
        					If treeAlbums.SelectedNode.Tag = SelectedAlbumID And Me.boolClosing = False Then
        						treeAlbums.SelectedNode.Nodes.Add(SubAlbumTree)
        						treeAlbums.ExpandAll()
        					Else
        						Exit Sub
        					End If
        				End If
        			Else
        				
        				' Display everything that's not an album in the listPictures object.
        				Dim oneChildViewItem As New ListViewItem
        				oneChildViewItem.Text = OneChildData("entity").Item("title")
        				oneChildViewItem.Tag = OneChildData("entity").Item("id").ToString.Replace("""", "")
        				
        				' Load the item onto listPictures with the default thumbnail (for now)
        				'  unless the user clicked into another album already.
        				oneChildViewItem.ImageKey = 0
        				If treeAlbums.SelectedNode.Tag = SelectedAlbumID And Me.boolClosing = False Then
        					listPictures.Items.Add(oneChildViewItem)
        				Else
        					Exit Sub
        				End If
        			End If
        			
        			' Increase the counter and move onto the next item.
        			counter = counter + 1
        			Application.DoEvents()
        		Next
        		
        		' Once the items are all loaded, load the thumbnails.
        		'  We do this seperately, because downloading thumbnails
        		'  can take a lot longer then the rest of the album load.
        		'  this way the user can see everything in the album right
        		'  away, and click on stuff while the thumbs load.
        		
        		' Set the status, and reset the counter before looping through listPictures.
        		labelGalleryStatus.Text = "Loading Thumbnails"
        		Dim OneItemView As ListViewItem
        		counter = 1
        		For Each OneItemView In listPictures.Items
        			
        			' Update the status text.
        			labelGalleryStatus.Text = "Loading Thumbnails (" & counter.ToString & " of " & listPictures.Items.Count.ToString() & ")"
        			Application.DoEvents()
        			
        			' Figure out what the file name for the thumbnail should be, and retrieve the details for this item.
        			Dim strFileThumbPath As String = strCacheFolder & "\" & OneItemView.Tag & "_thumb"
        			Dim OneChildData As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(OneItemView.Tag))
        			
        			' If the thumbnail is already downloaded into the cache, and the album selection hasn't
        			'   changed, then load it, or else download the thumbnail and display it.
        			'   If the selected album has changed, abort.
        			If System.IO.File.Exists(strFileThumbPath) Then
        				If treeAlbums.SelectedNode.Tag = SelectedAlbumID And Me.boolClosing = False Then
        					ImageListThumbs.Images.Add(OneItemView.Tag, AspectedImage(strFileThumbPath, 64,64))
        					OneItemView.ImageKey = OneItemView.Tag
        				Else
        					Exit Sub
        				End If
        			ElseIf GalleryClient.DownloadFile(OneChildData("entity").Item("thumb_url"), strFileThumbPath) Then
        				If treeAlbums.SelectedNode.Tag = SelectedAlbumID And Me.boolClosing = False Then
        					ImageListThumbs.Images.Add(OneItemView.Tag, AspectedImage(strFileThumbPath, 64,64))
        					OneItemView.ImageKey = OneItemView.Tag
        				Else
        					Exit Sub
        				End If
        			End If
        			counter = counter + 1
        			Application.DoEvents()
        		Next
        	End If
        
        ' If an error occurs while the program is quiting, it probably means
        '   we just tried to access a form object that no longer exists, so
        '   ignore it and quit.  Or else display a message.
        Catch ex As Exception
        	If Me.boolClosing = False Then
        		MessageBox.Show (ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        	End If
        	Exit Sub
        End Try
        	
        ' Set the status back to ready before exiting.
        labelGalleryStatus.Text = "Ready"
	End Sub ' END TreeAlbumsAfterSelect
	
	Sub ListPicturesSelectedDoubleClick(sender As Object, e As EventArgs)
		' This function is called when a photo/video is double clicked.
		'   Either display the picture, or ask the user if they'd like to
		'   download the video.
		
		' Make sure something is selected, then download the details for that item.
        If listPictures.SelectedItems.Count > 0 Then
        	' In case the user selects something else before the download finishes,
        	'   we're going to store the selected item in a local variable.
        	Dim selectedListViewItem as ListViewItem = listPictures.SelectedItems(0)
        	labelGalleryStatus.Text = "Loading Item..."
            Dim selectedItemData As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(listPictures.SelectedItems(0).Tag))
            
            ' If the item is a photo, download the resize and display it.
            If selectedItemData("entity").Item("type").ToString = """photo""" Then
            	labelGalleryStatus.Text = "Loading Image..."
            	
            	' Check to see if the resize already exists in the cache,
            	'   if it does, display it, or else download and display it.
                Dim strFileResizePath As String = strCacheFolder & "\" & listPictures.SelectedItems(0).Tag & "_resize"
                If System.IO.File.Exists(strFileResizePath) Then
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(strFileResizePath)
                    WindowViewResize.Tag = selectedListViewItem.Tag
                    WindowViewResize.Text = selectedListViewItem.Text
                    WindowViewResize.Show()
                ElseIf GalleryClient.DownloadFile(selectedItemData("entity").Item("resize_url"), strFileResizePath) Then
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(strFileResizePath)
                    WindowViewResize.Tag = selectedListViewItem.Tag
                    WindowViewResize.Text = selectedListViewItem.Text
                    WindowViewResize.Show()
                Else
                    ' Unable to find / download thumb, load a default.png image instead.
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\default.png")
                    WindowViewResize.Tag = selectedListViewItem.Tag
                    WindowViewResize.Text = selectedListViewItem.Text
                    WindowViewResize.Show()
                End If

            ElseIf selectedItemData("entity").Item("type").ToString = """movie""" Then
            	' If the item is a movie, ask if the user wants to download it.
                If MessageBox.Show("Movie viewing is not available at this time, would you like to download the file instead?", "Unsupported File Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                	dim strMovieURL as String = selectedItemData("entity").Item("file_url")
                    Dim SaveMovieAsDialog As New SaveFileDialog
                    SaveMovieAsDialog.FileName = selectedItemData("entity").Item("name")
                    If SaveMovieAsDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                        If GalleryClient.DownloadFile(strMovieURL, SaveMovieAsDialog.FileName, True) Then
                            MessageBox.Show("File Saved Successfully", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            Else
            	' This should never happen, but just in case the item isn't a photo or movie, display an error.
                MessageBox.Show("Viewing " & selectedItemData("entity").Item("type").ToString & " is not available at this time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        
        ' Set the status back to ready.
        labelGalleryStatus.Text = "Ready"
	End Sub ' END ListPicturesSelectedDoubleClick
	
	Sub CompareToLocalFolderToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Compare the contents of a local folder to the contents of a remote album.
		'   Use file names to look for missing items, and checksums to look for
		'   anything that's been modified or damaged.
		
		' Ask the user for the location of the folder to compare to.
		Dim LocalFolder As New FolderBrowserDialog
        If LocalFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
        	Me.Cursor = Cursors.WaitCursor
        	
        	' Create a new Window.  Load GalleryClient, the selected folder,
        	'   and the current selected album into it.
            Dim checksumWindow As New formChecksums
            checksumWindow.GalleryClient = GalleryClient
            checksumWindow.txtLocalFolder.Text = LocalFolder.SelectedPath
            checksumWindow.txtRemoteAlbum.Text = treeAlbums.SelectedNode.Text
            checksumWindow.txtRemoteAlbum.Tag = treeAlbums.SelectedNode.Tag
			Dim OneAlbum As TreeNode = treeAlbums.SelectedNode
			
			' Generate a full path to the current album to display instead of
			'   just the album name.
			While OneAlbum.Tag <> "1"
				OneAlbum = OneAlbum.Parent
				checksumWindow.txtRemoteAlbum.Text = OneAlbum.Text & "\" & checksumWindow.txtRemoteAlbum.Text
			End While
			
			' Show the window, and run DoEvents.
            checksumWindow.Show()
            Application.DoEvents()
            
            ' Load the local and remote file lists, then generate checksums and compare.
            checksumWindow.statusCompare.Text = "Loading Local File List"
            checksumWindow.LoadLocalFileList()
            checksumWindow.statusCompare.Text = "Loading Remote File List"
            checksumWindow.LoadAlbumFileList()
            checksumWindow.statusCompare.Text = "Generating Checksums"
            checksumWindow.CompareFiles()
            
            ' Set the status text and turn control back over to the user.
            If checksumWindow.BadFilesCount = 0 Then
                checksumWindow.statusCompare.Text = "All Files Matched."
            ElseIf checksumWindow.BadFilesCount = 1 Then
                checksumWindow.statusCompare.Text = "One file did not match and has been bolded."
            Else
                checksumWindow.statusCompare.Text = checksumWindow.BadFilesCount.ToString & " files did not match and have been bolded."
            End If
            Me.Cursor = Cursors.Default
        End If
	End Sub ' END CompareToLocalFolderToolStripMenuItemClick
	
	Sub UploadFilesToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Create a new upload queue window, when it closes refresh the album contents.
		
		' Create the new window, load GalleryClient into it and
		'   Set the upload destination to the current album.
		Dim WindowUploadQueue As New FormUploadQueue
		Dim AlbumID as Integer = Convert.ToInt32(treeAlbums.SelectedNode.Tag)
		WindowUploadQueue.GalleryClient = GalleryClient
		WindowUploadQueue.textUploadDestination.Tag = AlbumID.ToString()
		
		' Use the full path to the album instead of just it's name.
		WindowUploadQueue.textUploadDestination.Text = treeAlbums.SelectedNode.Text
		Dim OneAlbum As TreeNode = treeAlbums.SelectedNode
		While OneAlbum.Tag <> "1"
			OneAlbum = OneAlbum.Parent
			WindowUploadQueue.textUploadDestination.Text = OneAlbum.Text & "\" & WindowUploadQueue.textUploadDestination.Text
		End While
		
		' Display the window.
		WindowUploadQueue.Show()
		
		' Wait until the user is finished uploading files, then
		'   remove this album from the cache so the changes will be
		'   visible.
		While WindowUploadQueue.Visible = True
			Application.DoEvents()
		End While
		GalleryClient.ItemCache.RemoveItem(AlbumID)
		
		' If the album selection hasn't changed, reload it to display the new uploads.
		If treeAlbums.SelectedNode.Tag = AlbumID.ToString() Then
		  TreeAlbumsAfterSelect(sender, new TreeViewEventArgs(TreeAlbums.SelectedNode))
		End If
	End Sub ' END UploadFilesToolStripMenuItemClick
	
	Private Function AspectedImage(ByVal ImagePath As String, ByVal HWanted As Integer, ByVal WWanted As Integer) As Image
		' This function loads the thumbnails with their correct aspect ratio.
		'   Credit: http://www.windowsdevelop.com/windows-forms-general/vb-imagelist-control-maintaining-aspect-ratio-7247.shtml
        Dim myBitmap, WhiteSpace As System.Drawing.Bitmap
        Dim myGraphics As Graphics
        Dim myDestination As Rectangle
        Dim MaxDimension As Integer

        'create an instance of bitmap based on a file
        myBitmap = New System.Drawing.Bitmap(fileName:=ImagePath)
        'create a new square blank bitmap the right size
        If myBitmap.Height >= myBitmap.Width Then MaxDimension = myBitmap.Height Else MaxDimension = myBitmap.Width
        WhiteSpace = New System.Drawing.Bitmap(MaxDimension, MaxDimension)

        'get the drawing surface of the new blank bitmap
        myGraphics = Graphics.FromImage(WhiteSpace)

        'find out if the photo is landscape or portrait
        Dim WhiteGap As Double

        If myBitmap.Height > myBitmap.Width Then 'portrait
            WhiteGap = ((myBitmap.Width - myBitmap.Height) / 2) * -1
            myDestination = New Rectangle(x:=CInt(WhiteGap), y:=0, Width:=myBitmap.Width, Height:=myBitmap.Height)
        Else 'landscape
            WhiteGap = ((myBitmap.Width - myBitmap.Height) / 2)
            'create a destination rectangle
            myDestination = New Rectangle(x:=0, y:=CInt(WhiteGap), Width:=myBitmap.Width, Height:=myBitmap.Height)
        End If

        'draw the image on the white square
        myGraphics.DrawImage(image:=myBitmap, rect:=myDestination)

        AspectedImage = WhiteSpace
    End Function ' END AspectedImage

	Sub AboutToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Display the About window.
		
		Dim WindowAbout As New FormAbout
		WindowAbout.Show()
	End Sub ' END AboutToolStripMenuItemClick
	
	Sub EmptyImageCacheToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Delete the contents of the image cache folder.
		
		Dim CacheDirectory As New System.IO.DirectoryInfo(strCacheFolder)
		Dim oneFile As System.IO.FileInfo
		Dim FailedCounter As Integer = 0
		
		' Delete files one at a time, skipping anything that causes an error.
		'   Errors usually mean the file is still in use.
		For Each oneFile In CacheDirectory.GetFiles()
			Try
				oneFile.Delete()
			Catch ex As Exception
				FailedCounter = FailedCounter + 1
			End Try
		Next
		
		' Display a success message, or tell the user how many files are left.
		If FailedCounter = 0 Then
			MessageBox.Show ("Image Cache Emptied Successfully.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
		Else
			MessageBox.Show (FailedCounter.ToString() & " files were in use and could not be deleted.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub ' END EmptyImageCacheToolStripMenuItemClick
	
	Sub FormAlbumBrowserClosing(sender As Object, e As EventArgs)
		' Empty the image cache on exit (if the user checked the necessary box).
		
		' Load preferences from the config file.
        Dim results() As DataRow
        Dim g3Prefs As New DataSet
        g3Prefs.ReadXmlSchema(Application.StartupPath & "\config.xsd")
        g3Prefs.ReadXml(strDataFolder & "\config.xml")
        
        ' If EmptyCache is checked, delete the contents of the image cache folder.
        results = g3Prefs.Tables(0).Select("ConfigName = 'EmptyCache'")
        If results(0).Item(1) = True Then
        	Dim CacheDirectory As New System.IO.DirectoryInfo(strCacheFolder)
        	Dim oneFile As System.IO.FileInfo
        	
        	' Delete files one at a time, skipping anything that causes an error.
        	'   Errors usually mean the file is still in use.
        	For Each oneFile In CacheDirectory.GetFiles()
        		Try
        			oneFile.Delete()
        		Catch ex As Exception
        		End Try
        	Next
        End If
        
        ' Signal to any running threads/windows that we're quiting.
		Me.boolClosing = True
		Application.Exit()
	End Sub ' END FormAlbumBrowserClosing
	
	Sub FullscreenToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Switch the main window between windowed and fullscreen mode.
		
		' Toggle the checked status.
		FullscreenToolStripMenuItem.Checked = (FullscreenToolStripMenuItem.Checked = False)
		
		' Switch fullscreen off or on based on if the menu option is checked.
        If FullscreenToolStripMenuItem.Checked = True Then
        	Me.WindowState = FormWindowState.Normal
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        Else
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            Me.WindowState = FormWindowState.Normal
        End If
	End Sub ' END FullscreenToolStripMenuItemClick
	
	Sub PreferencesToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Display the preferences window
		
		Dim WindowPreferences As New FormPreferences
		WindowPreferences.Show()
	End Sub ' END PreferencesToolStripMenuItemClick
	
	Sub CreateNewAlbumToolStripMenuItemClick(sender As Object, e As EventArgs)
		' Display the create new album window.
		
		' Store the ID # for the selected album, in case the user
		'   selects something else before this sub finishes.
		Dim SelectedAlbumID As String = treeAlbums.SelectedNode.Tag
		
		' Create a new window.
		Dim WindowNewAlbum As New FormCreateAlbum
		WindowNewAlbum.GalleryClient = GalleryClient
		WindowNewAlbum.intParentID = Convert.ToInt32(SelectedAlbumID)
		WindowNewAlbum.Show()
		
		' While the window is visible, pause this sub.
		While WindowNewAlbum.Visible
			Application.DoEvents()
		End While
		
		' Once the window is no longer visible, assume a new album was created.
		'   Remove the cached data for the parent album in order to force a refresh.
		GalleryClient.ItemCache.RemoveItem(Convert.ToInt32(SelectedAlbumID))
		
		' If the album selection hasn't changed, reload it to display the new uploads.
		If treeAlbums.SelectedNode.Tag = SelectedAlbumID Then
		  TreeAlbumsAfterSelect(sender, new TreeViewEventArgs(TreeAlbums.SelectedNode))
		End If
	End Sub ' END CreateNewAlbumToolStripMenuItemClick
End Class ' EndFormAlbumBrowser
