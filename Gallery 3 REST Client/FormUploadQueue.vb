'  Gallery 3 REST Client
'  Copyright 2010-2012 Eric Cavaliere
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

Public Partial Class FormUploadQueue
	Friend GalleryClient As Gallery3.Client
	
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
		
	Sub ButtonAddToQueueClick(sender As Object, e As EventArgs)
		'  Add files to the upload queue.
		
		' Display a file selection dialog.
		Dim FilesToUpload As New OpenFileDialog
		FilesToUpload.Multiselect = True
		FilesToUpload.Filter = "Images (*.jpg; *.jpeg; *.png; *.gif)|*.JPG;*.JPEG;*.PNG;*.GIF|Movies (*.flv; *.mp4; *.m4v)|*.FLV;*.MP4;*.M4V"
		If FilesToUpload.ShowDialog = Windows.Forms.DialogResult.OK Then
			
			' Loop through each selected file, adding them to the queue.
			'   Display just the file name, but store the full name and path
			'   in the tag.
			Dim oneFile As String
			For Each oneFile In FilesToUpload.FileNames
				Dim NewQueueItem As New ListViewItem
				Dim FileDetails As new System.IO.FileInfo (onefile)
				NewQueueItem.Text = FileDetails.Name
				NewQueueItem.Tag = FileDetails.FullName
				listUploadQueue.Items.Add (NewQueueItem)
			Next
		End If
	End Sub ' END ButtonAddToQueueClick
	
	Sub ButtonUploadClick(sender As Object, e As EventArgs)
		' Upload each file in the queue to the remote Gallery.
		
		' Retrieve the details for the album that the files are being uploaded to.
		'   On error, exit.
        Dim AlbumInfo As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(textUploadDestination.Tag))
        If AlbumInfo Is Nothing Then
        	MessageBox.Show ("Error Retriving Album Details, Upload Aborted.", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error)
        	Exit Sub
        End If
        
        ' Disable a few buttons and make the window look busy.
        '   Also, set up the progress bar to track how many files have been uploaded.
        buttonUpload.Enabled = False
        buttonAddToQueue.Enabled = False
        Me.Cursor = Cursors.WaitCursor
		progressUpload.Value = 0
		progressUpload.Maximum = listUploadQueue.Items.Count
        
        ' Loop through each item in the queue, and upload them one at a time.
		Dim OneQueueItem As ListViewItem
		For Each OneQueueItem In listUploadQueue.Items
			
			' If SubItems is more then one, then an upload attempt was already run.
			'   If the upload status is "Failed" then try and upload it again, 
			'   or else skip it.
			If OneQueueItem.SubItems.Count > 1 Then
				If OneQueueItem.SubItems(1).Text = "Failed" Then
					OneQueueItem.SubItems.RemoveAt (1) 
				End If
			End If
			If OneQueueItem.SubItems.Count = 1 Then
				
				' Set the upload status to "Uploading" and upload the file.
				'   Once the upload function exits, set the status to either
				'   Complete or Failed, accordingly.
				Dim OneQueueStatus As New ListViewItem.ListViewSubItem
				OneQUeueStatus.Text = "Uploading..."
				OneQueueItem.SubItems.Add(OneQueueStatus)
				Application.DoEvents()
				If GalleryClient.UploadFile(AlbumInfo("url"), OneQueueItem.Tag) Then
					OneQueueStatus.Text = "Complete"
				Else
					OneQueueStatus.Text = "Failed"
				End If
			End If
			
			' Update the progress bar and run DoEvents before moving onto
			'   the next item.
			progressUpload.Value = progressUpload.Value + 1
			Application.DoEvents()
		Next
		
		' Once everything's been uploaded, enable the window again.
        buttonUpload.Enabled = True
        buttonAddToQueue.Enabled = True
        Me.Cursor = Cursors.Default
	End Sub ' END ButtonUploadClick
End Class ' END FormUploadQueue
