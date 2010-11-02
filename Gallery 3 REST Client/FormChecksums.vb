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

Public Partial Class FormChecksums
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub

    Public GalleryClient As Gallery3.Client

    Public Sub LoadLocalFileList()
        ' Load list of local files.
        '   Note:  This function must be called before LoadAlbumFileList.
        
        ' Create a DirectoryInfo object to load a list of files in the
        '  specified folder.  Add each file to the listFiles object.
        Dim LocalFolder As New System.IO.DirectoryInfo(txtLocalFolder.Text)
        Dim OneFile As System.IO.FileInfo
        For Each OneFile In LocalFolder.GetFiles
            Dim NewFileItem As New ListViewItem
            NewFileItem.Text = OneFile.Name
            NewFileItem.SubItems.Add("")
            listFiles.Items.Add(NewFileItem)
            Application.DoEvents()
        Next
    End Sub ' END LoadLocalFileList

    Public Sub LoadAlbumFileList()
        ' Load contents of remote album
        
        ' Get the item details for the specified album, then loop through each
        '   of its member items.
        Dim RemoteAlbum As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(txtRemoteAlbum.Tag))
        Dim RemoteItemURL As String
        Dim OneFileItem As ListViewItem
        For Each RemoteItemURL In RemoteAlbum("members")
        	
        	' Get the details for the current member item.
        	'   Note:  at this point, all the members should be cached,
        	'   so there should be a performance hit from accessing then
        	'   individually instead of all at once with GetItems.
            Dim RemoteItem As Linq.JObject = GalleryClient.GetItem(RemoteItemURL)
            
            '  Assuming nothing went wrong, and there we're not looking at an album,
            '   check for a corresponding filename on the local folder list.
            '   If found, and this file next to it.  If not, add a new entry for it
            '   at the bottom of the list.
            If Not RemoteItem Is Nothing Then
                If (RemoteItem("entity").Item("type").ToString <> """album""") Then
                    Dim Found As Boolean = False
                    For Each OneFileItem In listFiles.Items
                        If OneFileItem.Text = RemoteItem("entity").Item("name") Then
                            OneFileItem.Tag = RemoteItem("entity").Item("id")
                            OneFileItem.SubItems(1).Text = RemoteItem("entity").Item("name")
                            Found = True
                        End If
                    Next
                    If Found = False Then
                        Dim NewFileItem As New ListViewItem
                        NewFileItem.Text = ""
                        NewFileItem.Tag = RemoteItem("entity").Item("id").ToString
                        NewFileItem.SubItems.Add(RemoteItem("entity").Item("name"))
                        listFiles.Items.Add(NewFileItem)
                    End If
                End If
            End If
            
            ' Run DoEvents to keep the app responsive before moving onto the next item.
            Application.DoEvents()
        Next
    End Sub ' END LoadAlbumFileList

    Public Sub CompareFiles()
        ' Load local and remote checksums for the contents of listFiles,
        '   and compare the checksums to look for modified files.
        
        ' Loop through each item in the list, generating local and remote
        '   checksums for everything.  Then compare the checksums to look
        '   for modified or missing files.
        For Each OneFileItem In listFiles.Items
        	
        	' Generate an SHA1 checksum for the current local item.  If the file
        	'   name is empty (the file doesn't exist locally) leave it empty.
            If OneFileItem.Text <> "" Then
                Using sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider
                    Dim FileReader As New System.IO.FileStream(txtLocalFolder.Text & "\" & OneFileItem.Text, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim hash() As Byte = sha1.ComputeHash(FileReader)
                    OneFileItem.SubItems.Add(ByteArrayToString(hash))
                End Using
            Else
                OneFileItem.SubItems.Add("")
            End If
            Application.DoEvents()
            
            '  Generate an SHA1 checksum for the current remote item.  If the file
            '   name is empty (the file doesn't exist remotely) leave this field empty.
            If OneFileItem.SubItems(1).Text <> "" Then
                Dim RemoteChecksum As String = GalleryClient.GetItemChecksum(Convert.ToInt32(OneFileItem.Tag.ToString.Replace("""", "")), "sha1")
                
                ' In the event of a server error, store ERROR instead of the checksum.
                If RemoteChecksum <> "" Then
                	OneFileItem.SubItems.Add(RemoteChecksum)
                Else
                	OneFileItem.SubItems.Add("ERROR")
                End If
            Else
                OneFileItem.SubItems.Add("")
            End If
            Application.DoEvents()
            
            ' Compare the two checksums, if they don't match
            '   bold the entry.
            If OneFileItem.SubItems(2).Text <> OneFileItem.SubItems(3).Text Then
                OneFileItem.Font = New Font(listFiles.Font, FontStyle.Bold)
            End If
            Application.DoEvents()
        Next
    End Sub ' END CompareFiles

    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
    	' Convert the checksum generated for the local file 
    	'   to a string, for comparison with the remote checksum.
    	
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function ' END ByteArrayToString
End Class ' END FormChecksums
