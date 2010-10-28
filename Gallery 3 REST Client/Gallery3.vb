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

Public Class Gallery3
    Public Class GalleryItem
        Public ItemID As Integer
        Public QueryResults As String
    End Class

    Class Client
        Dim Gallery3URL As String
        Dim Gallery3RESTKey As String
        Dim QueryCache As New List(Of GalleryItem)

        Public Sub New(ByVal url As String)
            If Not url.EndsWith("/") Then
                url = url & "/"
            End If
            Gallery3URL = url
        End Sub

        Public Function login(ByVal username As String, ByVal password As String) As Boolean
            ' Log into Gallery with USERNAME/PASSWORD
            Try
                ' Send login info.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/"), System.Net.HttpWebRequest)
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = ("user=" & username & "&password=" & password).Length.ToString
                request.Headers.Add("X-Gallery-Request-Method", "post")
                Dim datastream As System.IO.Stream = request.GetRequestStream
                Dim encoding As New System.Text.UTF8Encoding
                datastream.Write(encoding.GetBytes("user=" & username & "&password=" & password), 0, encoding.GetBytes("user=" & username & "&password=" & password).Length)
                datastream.Close()

                ' Receive rest key (or an error if something wasn't right).
                Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                Dim restKey As String = readStream.ReadToEnd.Replace("""", "")
                response.Close()
                readStream.Close()

                Gallery3RESTKey = restKey
                Return True

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Public Function login(ByVal RESTAPIKey As String) As Boolean
            ' Login to Gallery using a REST API key.
            Try
                ' Send the login request.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/item/1/"), System.Net.HttpWebRequest)
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "GET"
                request.Headers.Add("X-Gallery-Request-Method", "get")
                request.Headers.Add("X-Gallery-Request-Key", RESTAPIKey)

                ' Receive the response from the server (or an error if the key or URL wasn't right)
                Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                Dim txtServerResponse As String = readStream.ReadToEnd
                response.Close()
                readStream.Close()
                Gallery3RESTKey = RESTAPIKey

                Return True

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Public Function GetItem(ByVal ItemID As Integer) As Linq.JObject
            ' Request the details of a specific item (ItemID).
            Return Me.GetItem(Gallery3URL & "rest/item/" & ItemID.ToString & "/")
        End Function

        Public Function GetItem(ByVal ItemURL As String) As Linq.JObject
            ' Request the details of a specific item (ItemID).
            Try
                If ItemURL.EndsWith("/") Then
                    ItemURL = ItemURL.Substring(0, ItemURL.Length - 1)
                End If
                Dim ItemID As Integer = Convert.ToInt32(ItemURL.Substring(ItemURL.LastIndexOf("/") + 1))
                Dim CachedResults = From g3items In QueryCache Where g3items.ItemID = ItemID
                If CachedResults.Count > 0 Then
                    Return Linq.JObject.Parse(CachedResults(0).QueryResults)
                Else
                    ' Send the login request.
                    Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(ItemURL), System.Net.HttpWebRequest)
                    request.Credentials = System.Net.CredentialCache.DefaultCredentials
                    request.Method = "GET"
                    request.Headers.Add("X-Gallery-Request-Method", "get")
                    request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)

                    ' Receive the response from the server (or an error if the key or URL wasn't right)
                    Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                    Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                    Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                    Dim txtServerResponse As String = readStream.ReadToEnd
                    response.Close()
                    readStream.Close()

                    Dim NewCachedItem As New GalleryItem
                    NewCachedItem.ItemID = ItemID
                    NewCachedItem.QueryResults = txtServerResponse
                    QueryCache.Add(NewCachedItem)

                    'Clipboard.SetText(Linq.JObject.Parse(txtServerResponse).ToString())
                    'MessageBox.Show(Linq.JObject.Parse(txtServerResponse).ToString())

                    Return Linq.JObject.Parse(txtServerResponse)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function

        Public Function GetItems(ByVal ItemURLs As Linq.JToken) As List(Of String)
            If ItemURLs.Count = 0 Then Return Nothing

            Dim oneURL, txtServerRequest As String
            txtServerRequest = "urls=["
            For Each oneURL In ItemURLs
                txtServerRequest = txtServerRequest & """" & oneURL & ""","
            Next
            txtServerRequest = txtServerRequest.Substring(0, txtServerRequest.Length - 1) & "]"

            Try
                ' Send login info.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/items/"), System.Net.HttpWebRequest)
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.Headers.Add("X-Gallery-Request-Method", "get")
                request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)
                Dim datastream As System.IO.Stream = request.GetRequestStream
                Dim encoding As New System.Text.UTF8Encoding
                datastream.Write(encoding.GetBytes(txtServerRequest), 0, encoding.GetBytes(txtServerRequest).Length)
                datastream.Close()

                ' Receive the response from the server (or an error if the key or URL wasn't right)
                Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                Dim txtServerResponse As String = readStream.ReadToEnd
                response.Close()
                readStream.Close()

                ' Split up the server's response into individual items
                txtServerResponse = txtServerResponse.Substring(1, txtServerResponse.Length - 1)
                'Dim DownloadedItems As Linq.JObject = Linq.JObject.Parse(txtServerResponse)
                Dim ItemsArray As New List(Of String)
                Dim NewItemObject As Linq.JObject
                Dim counter As Integer

                While txtServerResponse.IndexOf(",{""url"":") > 0
                    Dim tempString As String = txtServerResponse.Substring(0, txtServerResponse.IndexOf(",{""url"":"))
                    ItemsArray.Add(tempString)
                    txtServerResponse = txtServerResponse.Substring(txtServerResponse.IndexOf(",{""url"":") + 1)

                    NewItemObject = Linq.JObject.Parse(tempString)
                    counter = 0
                    While counter < QueryCache.Count
                        'MessageBox.Show("HERE")
                        'MessageBox.Show(NewItemObject("entity").Item("id").ToString)
                        'MessageBox.Show(Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")))
                        If QueryCache(counter).ItemID = Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")) Then
                            QueryCache.RemoveAt(counter)
                        End If
                        counter = counter + 1
                    End While
                    Dim NewCachedItem As New GalleryItem
                    NewCachedItem.ItemID = Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", ""))
                    NewCachedItem.QueryResults = tempString
                    QueryCache.Add(NewCachedItem)
                End While
                ItemsArray.Add(txtServerResponse)
                NewItemObject = Linq.JObject.Parse(txtServerResponse)
                counter = 0
                While counter < QueryCache.Count
                    If QueryCache(counter).ItemID = Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")) Then
                        QueryCache.RemoveAt(counter)
                    End If
                    counter = counter + 1
                End While
                Dim NewCachedItem1 As New GalleryItem
                NewCachedItem1.ItemID = Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", ""))
                NewCachedItem1.QueryResults = txtServerResponse
                QueryCache.Add(NewCachedItem1)
                Return ItemsArray

            Catch ex As Exception
                MessageBox.Show(ex.StackTrace.ToString)
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function

        Public Function GetItemChecksum(ByVal ItemID As Integer, ByVal ChecksumType As String) As String
            Return GetItemChecksum(Gallery3URL & "rest/itemchecksum_" & ChecksumType.ToLower & "/" & ItemID.ToString)
        End Function

        Public Function GetItemChecksum(ByVal ItemURL As String) As String
            ' Request the details of a specific item (ItemID).
            Try
                ' Send the login request.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(ItemURL), System.Net.HttpWebRequest)
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "GET"
                request.Headers.Add("X-Gallery-Request-Method", "get")
                request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)

                ' Receive the response from the server (or an error if the key or URL wasn't right)
                Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                Dim txtServerResponse As String = readStream.ReadToEnd
                response.Close()
                readStream.Close()

                Dim CheckSumObject As Linq.JObject = Linq.JObject.Parse(txtServerResponse)
                Return CheckSumObject("entity").Item("checksum")
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ""
            End Try
        End Function

        Function GetSubAlbums(ByVal ItemID As Integer) As TreeNode
            Return Me.GetSubAlbums(Gallery3URL & "rest/item/" & ItemID.ToString)
        End Function

        Function GetSubAlbums(ByVal ItemURL As String) As TreeNode
            Dim AlbumNode As New TreeNode
            Dim RootItem As Linq.JObject = Me.GetItem(ItemURL)
            Dim ChildURL As String
            If Not RootItem Is Nothing Then
                If RootItem("entity").Item("type").ToString = """album""" Then
                    AlbumNode.Text = RootItem("entity").Item("title").ToString.Replace("""", "")
                    AlbumNode.Tag = RootItem("entity").Item("id").ToString.Replace("""", "")
                    Application.DoEvents()
                    For Each ChildURL In RootItem("members")
                        Dim NewAlbum As TreeNode
                        NewAlbum = Me.GetSubAlbums(ChildURL)
                        If Not NewAlbum Is Nothing Then
                            AlbumNode.Nodes.Add(Me.GetSubAlbums(ChildURL))
                        End If
                    Next
                    Return AlbumNode
                Else
                End If
            End If
            Return Nothing
        End Function

        Function DownloadFile(ByVal ItemID As Integer, ByVal FieldName As String, ByRef SavedFileName As String) As Boolean
            Dim ItemDetails As Linq.JObject = Me.GetItem(ItemID)
            If Not ItemDetails Is Nothing Then
                Return Me.DownloadFile(ItemDetails("entity").Item(FieldName), SavedFileName)
            Else
                Return False
            End If
        End Function

        Function DownloadFile(ByVal url As String, ByVal SavedFileName As String, Optional ByVal DownloadProgressBar As ProgressBar = Nothing, Optional ByVal DownloadProgressText As Label = Nothing) As Boolean
            ' Download a binary file off the internet via HTTP.
            '   Return True if successful, false otherwise.
            Dim response As System.Net.HttpWebResponse

            ' Connect to remote server.
            Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
            request.Credentials = System.Net.CredentialCache.DefaultCredentials
            request.Method = "GET"
            request.Headers.Add("X-Gallery-Request-Method", "get")
            request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)
            Try
                response = CType(request.GetResponse(), System.Net.HttpWebResponse)
                request.Timeout = 90 * 1000 ' seconds * 1000
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
            Catch ex As Exception
                MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try

            ' Download file.
            Dim DownloadedLength As Integer = 0
            If response.StatusDescription.ToString = "OK" Then
                If Not DownloadProgressBar Is Nothing Then
                    If response.ContentLength <> -1 Then
                        DownloadProgressBar.Maximum = Convert.ToInt32(response.ContentLength)
                    End If
                End If
                Application.DoEvents()
                Dim dataStream As System.IO.Stream
                Dim reader As System.IO.BinaryReader
                Try
                    Dim downloadedFile As New System.IO.FileStream(SavedFileName, IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write)
                    Dim writeFile As New System.IO.BinaryWriter(downloadedFile)
                    dataStream = response.GetResponseStream()
                    reader = New System.IO.BinaryReader(dataStream)
                    Dim length As Integer
                    Dim buffer(4096) As Byte
                    length = reader.Read(buffer, 0, 4096)
                    DownloadedLength = DownloadedLength + length
                    If Not DownloadProgressBar Is Nothing Then
                        If response.ContentLength = -1 Then
                            DownloadProgressBar.Maximum = DownloadedLength
                        End If
                        DownloadProgressBar.Value = DownloadedLength
                    End If
                    If Not DownloadProgressText Is Nothing Then
                        DownloadProgressText.Text = Math.Round(DownloadedLength / 1024 / 1024, 2).ToString & "MB"
                    End If
                    While (length > 0) And (Not (buffer Is Nothing))
                        Application.DoEvents()
                        writeFile.Write(buffer, 0, length)
                        Application.DoEvents()
                        length = reader.Read(buffer, 0, 4096)
                        DownloadedLength = DownloadedLength + length
                        If Not DownloadProgressBar Is Nothing Then
                            If response.ContentLength = -1 Then
                                DownloadProgressBar.Maximum = DownloadedLength
                            End If
                            DownloadProgressBar.Value = DownloadedLength
                        End If
                        If Not DownloadProgressText Is Nothing Then
                            DownloadProgressText.Text = Math.Round(DownloadedLength / 1024 / 1024, 2).ToString & "MB"
                        End If
                    End While
                    writeFile.Flush()
                    writeFile.Close()
                    reader.Close()
                    dataStream.Close()
                Catch ex As Exception
                    MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            Else
                Return False
            End If
            Return True
        End Function

        Function UploadFile(ByVal url As String, ByVal FileToUpload As String) As Boolean
        	Dim UploadObject As New ClassFileUpload.AsyncUpload
        	return UploadObject.Upload (url, FileToUpload, Me.Gallery3RESTKey)
        End Function
    End Class
End Class
