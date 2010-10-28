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
        ' Load list of local files
        Dim LocalFolder As New System.IO.DirectoryInfo(txtLocalFolder.Text)
        Dim OneFile As System.IO.FileInfo
        For Each OneFile In LocalFolder.GetFiles
            Dim NewFileItem As New ListViewItem
            NewFileItem.Text = OneFile.Name
            NewFileItem.SubItems.Add("")
            listFiles.Items.Add(NewFileItem)
            Application.DoEvents()
        Next
    End Sub

    Public Sub LoadAlbumFileList()
        ' Load contents of remote album
        Dim RemoteAlbum As Linq.JObject = GalleryClient.GetItem(Convert.ToInt32(txtRemoteAlbum.Tag))
        Dim RemoteItemURL As String
        Dim OneFileItem As ListViewItem
        For Each RemoteItemURL In RemoteAlbum("members")
            Dim RemoteItem As Linq.JObject = GalleryClient.GetItem(RemoteItemURL)
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
            Application.DoEvents()
        Next
    End Sub

    Public Sub CompareFiles()
        ' Load checksums and compare
        For Each OneFileItem In listFiles.Items
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

            If OneFileItem.SubItems(1).Text <> "" Then
                Dim RemoteChecksum As String = GalleryClient.GetItemChecksum(Convert.ToInt32(OneFileItem.Tag.ToString.Replace("""", "")), "sha1")
                OneFileItem.SubItems.Add(RemoteChecksum)
            Else
                OneFileItem.SubItems.Add("")
            End If
            Application.DoEvents()

            If OneFileItem.SubItems(2).Text <> OneFileItem.SubItems(3).Text Then
                OneFileItem.Font = New Font(listFiles.Font, FontStyle.Bold)
            End If
            Application.DoEvents()
        Next
    End Sub

    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function
End Class
