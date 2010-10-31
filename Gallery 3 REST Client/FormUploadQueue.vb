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

Public Partial Class FormUploadQueue
	Friend GalleryClient As Gallery3.Client
	
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub FormUploadQueueLoad(sender As Object, e As EventArgs)
	End Sub
	
	Sub ButtonAddToQueueClick(sender As Object, e As EventArgs)
		Dim FilesToUpload As New OpenFileDialog
		filestoupload.Multiselect = true
		If filestoupload.ShowDialog = Windows.Forms.DialogResult.OK Then
			Dim oneFile As String
			For Each oneFile In FilesToUpload.FileNames
				Dim NewQueueItem As New ListViewItem
				Dim FileDetails As new System.IO.FileInfo (onefile)
				NewQueueItem.Text = FileDetails.Name
				NewQueueItem.Tag = FileDetails.FullName
				listUploadQueue.Items.Add (NewQueueItem)
			Next
		End If
	End Sub
	
	Sub ButtonUploadClick(sender As Object, e As EventArgs)
        Dim AlbumInfo As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(textUploadDestination.Tag))
        If AlbumInfo Is Nothing Then
        	MessageBox.Show ("Error Retriving Album Details, Upload Aborted.", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error)
        	Exit Sub
        End If
        
        buttonUpload.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        
		Dim OneQueueItem As ListViewItem
		progressUpload.Value = 0
		progressUpload.Maximum = listUploadQueue.Items.Count
		For Each OneQueueItem In listUploadQueue.Items
			If OneQueueItem.SubItems.Count > 1 Then
				If OneQueueItem.SubItems(1).Text = "Failed" Then
					OneQueueItem.SubItems.RemoveAt (1) 
				End If
			End If
			If OneQueueItem.SubItems.Count = 1 Then
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
			progressUpload.Value = progressUpload.Value + 1
			Application.DoEvents()
		Next
        buttonUpload.Enabled = True
        Me.Cursor = Cursors.Default
	End Sub
End Class
