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

Public Partial Class FormAlbumBrowser
	Public GalleryClient As Gallery3.Client

	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub FormAlbumBrowserLoad(sender As Object, e As EventArgs)
		' Download the details of Item #1 (basically the name of the root album), and load into treeAlbums.
        labelGalleryStatus.Text = "Loading Albums"
        Application.DoEvents()

        Dim rootNode As New TreeNode
        Dim RootItem As Linq.JObject = GalleryClient.GetItem(1)

        If RootItem("relationships").Item("itemchecksums") Is Nothing Then
            CompareToLocalFolderToolStripMenuItem.Enabled = False
        End If

        If Not RootItem Is Nothing Then
            rootNode.Text = RootItem("entity").Item("title").ToString.Replace("""", "")
            rootNode.Tag = RootItem("entity").Item("id").ToString.Replace("""", "")
        End If
        treeAlbums.Nodes.Add(rootNode)
        treeAlbums.ExpandAll()

        labelGalleryStatus.Text = "Ready"
	End Sub
	
	Sub TreeAlbumsAfterSelect(sender As Object, e As TreeViewEventArgs)
        ' Download the details of the currently selected album.
        '   Loop through each "member" item and load into the form.
        labelGalleryStatus.Text = "Loading Contents of " & treeAlbums.SelectedNode.Text

        Application.DoEvents()
        listPictures.Items.Clear()
        ImageListThumbs.Images.Clear()

        Dim SelectedAlbumID = treeAlbums.SelectedNode.Tag
        Dim SelectedAlbum As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(treeAlbums.SelectedNode.Tag))
        Dim ChildItems As List(Of String) = GalleryClient.GetItems(SelectedAlbum("members"))

        If Not ChildItems Is Nothing Then
            'Dim DefaultFileThumbImage As System.Drawing.Image = Image.FromFile(Application.StartupPath & "\default.png")
            'ImageListThumbs.Images.Add(0, DefaultFileThumbImage)
                        ImageListThumbs.Images.Add(0, AspectedImage(Application.StartupPath & "\default.png", 64,64))
                        'OneItemView.ImageKey = OneItemView.Tag

            Dim OneChild As String
            Dim counter As Integer = 1
            For Each OneChild In ChildItems
                labelGalleryStatus.Text = "Loading Contents of " & treeAlbums.SelectedNode.Text & " (" & counter.ToString & " of " & SelectedAlbum("members").Count.ToString & ")"
                Dim OneChildData As Linq.JObject = Linq.JObject.Parse(OneChild)
                If (OneChildData("entity").Item("type").ToString = """album""") Then
                    ' Display albums in the treeAlbums object.
                    Dim SubAlbumTree As New TreeNode
                    SubAlbumTree.Text = OneChildData("entity").Item("title").ToString.Replace("""", "")
                    SubAlbumTree.Tag = OneChildData("entity").Item("id").ToString.Replace("""", "")
                    Dim SearchTree As New TreeNode
                    Dim NodeLoaded As Boolean = False
                    For Each SearchTree In treeAlbums.SelectedNode.Nodes
                        If SearchTree.Tag = SubAlbumTree.Tag Then
                            NodeLoaded = True
                            Exit For
                        End If
                    Next
                    If treeAlbums.SelectedNode.Tag = SelectedAlbumID And NodeLoaded = False Then
                        treeAlbums.SelectedNode.Nodes.Add(SubAlbumTree)
                        treeAlbums.ExpandAll()
                    End If
                Else
                    ' Display everything that's not an album in the listPictures object.
                    Dim oneChildViewItem As New ListViewItem
                    oneChildViewItem.Text = OneChildData("entity").Item("title").ToString.Replace("""", "")
                    oneChildViewItem.Tag = OneChildData("entity").Item("id").ToString.Replace("""", "")
                    ' Unable to find / download thumb, load a default.png image instead.
                    oneChildViewItem.ImageKey = 0
                    If treeAlbums.SelectedNode.Tag = SelectedAlbumID Then
                        listPictures.Items.Add(oneChildViewItem)
                    Else
                        Exit Sub
                    End If
                End If

                counter = counter + 1
                Application.DoEvents()
            Next


            labelGalleryStatus.Text = "Loading Thumbnails"
            Dim OneItemView As ListViewItem
            For Each OneItemView In listPictures.Items
                Dim strFileThumbPath As String = Application.StartupPath & "\cache\" & OneItemView.Tag & "_thumb"
                Dim OneChildData As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(OneItemView.Tag))

                If System.IO.File.Exists(strFileThumbPath) Then
                    If treeAlbums.SelectedNode.Tag = SelectedAlbumID Then
                        'Dim FileThumbImage As System.Drawing.Image = Image.FromFile(strFileThumbPath)
                        'ImageListThumbs.Images.Add(OneItemView.Tag, FileThumbImage)
                        ImageListThumbs.Images.Add(OneItemView.Tag, AspectedImage(strFileThumbPath, 64,64))
                        OneItemView.ImageKey = OneItemView.Tag
                    Else
                        Exit Sub
                    End If
                ElseIf GalleryClient.DownloadFile(OneChildData("entity").Item("thumb_url"), strFileThumbPath) Then
                    If treeAlbums.SelectedNode.Tag = SelectedAlbumID Then
                        'Dim FileThumbImage As System.Drawing.Image = Image.FromFile(strFileThumbPath)
                        'ImageListThumbs.Images.Add(OneItemView.Tag, FileThumbImage)
                        ImageListThumbs.Images.Add(OneItemView.Tag, AspectedImage(strFileThumbPath, 64,64))
                        OneItemView.ImageKey = OneItemView.Tag
                    Else
                        Exit Sub
                    End If
                End If
                Application.DoEvents()
            Next
        End If



        labelGalleryStatus.Text = "Ready"
	End Sub
	
	Sub ListPicturesSelectedDoubleClick(sender As Object, e As EventArgs)
        If listPictures.SelectedItems.Count > 0 Then
            Dim selectedItem As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(listPictures.SelectedItems(0).Tag))

            If selectedItem("entity").Item("type").ToString = """photo""" Then
                Dim strFileResizePath As String = Application.StartupPath & "\cache\" & listPictures.SelectedItems(0).Tag & "_resize"
                If System.IO.File.Exists(strFileResizePath) Then
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(strFileResizePath)
                    WindowViewResize.Tag = listPictures.SelectedItems(0).Tag
                    'WindowViewResize.Height = WindowViewResize.PictureResize.Height
                    'WindowViewResize.Width = WindowViewResize.PictureResize.Width
                    WindowViewResize.Text = listPictures.SelectedItems(0).Text
                    WindowViewResize.Show()
                ElseIf GalleryClient.DownloadFile(selectedItem("entity").Item("resize_url"), strFileResizePath) Then
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(strFileResizePath)
                    WindowViewResize.Tag = listPictures.SelectedItems(0).Tag
                    'WindowViewResize.Height = WindowViewResize.PictureResize.Height
                    'WindowViewResize.Width = WindowViewResize.PictureResize.Width
                    WindowViewResize.Text = listPictures.SelectedItems(0).Text
                    WindowViewResize.Show()
                Else
                    ' Unable to find / download thumb, load a default.png image instead.
                    Dim WindowViewResize As New formViewPicture
                    WindowViewResize.PictureResize.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\default.png")
                    WindowViewResize.Tag = listPictures.SelectedItems(0).Tag
                    'WindowViewResize.Height = WindowViewResize.PictureResize.Height
                    'WindowViewResize.Width = WindowViewResize.PictureResize.Width
                    WindowViewResize.Text = listPictures.SelectedItems(0).Text
                    WindowViewResize.Show()
                End If
            ElseIf selectedItem("entity").Item("type").ToString = """movie""" Then
                If MessageBox.Show("Movie viewing is not available at this time, would you like to download the file instead?", "Unsupported File Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Dim SaveMovieAsDialog As New SaveFileDialog
                    SaveMovieAsDialog.FileName = selectedItem("entity").Item("name")
                    If SaveMovieAsDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim DownloadProgressWindow As New formDownload
                        DownloadProgressWindow.Text = "Saving To " & SaveMovieAsDialog.FileName
                        DownloadProgressWindow.Show()
                        Application.DoEvents()
                        If GalleryClient.DownloadFile(selectedItem("entity").Item("file_url"), SaveMovieAsDialog.FileName, DownloadProgressWindow.ProgressDownload, DownloadProgressWindow.lblDownloadProgress) Then
                            MessageBox.Show("File Saved Successfully", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        DownloadProgressWindow.Close()
                    End If
                End If
            Else
                MessageBox.Show("Viewing " & selectedItem("entity").Item("type").ToString & " is not available at this time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
	End Sub
	
	Sub CompareToLocalFolderToolStripMenuItemClick(sender As Object, e As EventArgs)
		Dim LocalFolder As New FolderBrowserDialog
        If LocalFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim checksumWindow As New formChecksums
            checksumWindow.GalleryClient = GalleryClient
            checksumWindow.txtLocalFolder.Text = LocalFolder.SelectedPath
            checksumWindow.txtRemoteAlbum.Text = treeAlbums.SelectedNode.Text
            checksumWindow.txtRemoteAlbum.Tag = treeAlbums.SelectedNode.Tag
			Dim OneAlbum As TreeNode = treeAlbums.SelectedNode
			While OneAlbum.Tag <> "1"
				OneAlbum = OneAlbum.Parent
				checksumWindow.txtRemoteAlbum.Text = OneAlbum.Text & "\" & checksumWindow.txtRemoteAlbum.Text
			End While
            checksumWindow.Show()
            Application.DoEvents()

            checksumWindow.statusCompare.Text = "Loading Local File List"
            checksumWindow.LoadLocalFileList()
            checksumWindow.statusCompare.Text = "Loading Remote File List"
            checksumWindow.LoadAlbumFileList()
            checksumWindow.statusCompare.Text = "Generating Checksums"
            checksumWindow.CompareFiles()
            checksumWindow.statusCompare.Text = "Files that didn't match have been bolded."

        End If
	End Sub
	
	Sub UploadFilesToolStripMenuItemClick(sender As Object, e As EventArgs)
		Dim OneAlbum As TreeNode = treeAlbums.SelectedNode
		Dim WindowUploadQueue As New FormUploadQueue 
		WindowUploadQueue.GalleryClient = GalleryClient
		WindowUploadQueue.textUploadDestination.Text = treeAlbums.SelectedNode.Text 
		windowuploadqueue.textUploadDestination.Tag = treeAlbums.SelectedNode.Tag
		While OneAlbum.Tag <> "1"
			OneAlbum = OneAlbum.Parent
			WindowUploadQueue.textUploadDestination.Text = OneAlbum.Text & "\" & WindowUploadQueue.textUploadDestination.Text
		End While
		windowuploadqueue.Show
	End Sub
	
Private Function AspectedImage(ByVal ImagePath As String, ByVal HWanted As Integer, ByVal WWanted As Integer) As Image
'http://www.windowsdevelop.com/windows-forms-general/vb-imagelist-control-maintaining-aspect-ratio-7247.shtml
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

    End Function

	Sub AboutToolStripMenuItemClick(sender As Object, e As EventArgs)
		Dim WindowAbout As New FormAbout
		WindowAbout.Show()
	End Sub
End Class
