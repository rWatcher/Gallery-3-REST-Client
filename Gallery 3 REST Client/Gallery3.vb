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
	Public Class Cache
		' Maintains a cache of REST responses, to prevent the client from requesting
		'   the same resource over and over.
		
		' Stores and ID number, and the results as a string.
		Friend Class Item
			Friend Dim ItemID As Integer
			Friend Dim QueryResults as String
		End Class ' END Item
		
		' Create a list to store the actual data in.
		Private Dim CachedItems As New List (Of Cache.Item)
		
		Public Function Count() As Integer
			' Return the number of items currently in the cache.
			
			Return CachedItems.Count
		End Function ' END Count
		
		Public Function GetItem(ByVal ItemID As Integer) As String
			' Retrieve a specific item from the Cache.
			'   Either returns the cached response as a
			'   string, or returns "" if it wasn't found.
			
			Dim CachedResults = From g3items In CachedItems Where g3items.ItemID = ItemID
			If CachedResults.Count > 0 Then
				Return CachedResults(0).QueryResults
			Else
				Return ""
			End If
		End Function ' END GetItem
		
		Public Sub RemoveItem (ByVal ItemID As Integer)
			' Remove an item from the cache (if it exists).
			
			Dim counter As Integer = 0
			While counter < CachedItems.Count
				If CachedItems(counter).ItemID = ItemID Then
					CachedItems.RemoveAt(counter)
				End If
				counter = counter + 1
			End While
		End Sub ' END RemoveItem
		
		Public Sub AddItem (ByVal ItemID As Integer, ByVal txtQueryResults As String)
			'  Add a new item to the cache.
			
			RemoveItem (ItemID)
			Dim NewCachedItem As New Cache.Item
			NewCachedItem.ItemID = ItemID
			NewCachedItem.QueryResults = txtQueryResults
			CachedItems.Add(NewCachedItem)
		End Sub ' END AddItem
	End Class ' END Cache

    Class Client
    	' Responsible for connecting to and communicating with the
    	'   remote Gallery server.
    	
    	' Set up a few global variables for the server URL, REST key
    	'   and a cache of previously requested items.
        Dim Gallery3URL As String
        Dim Gallery3RESTKey As String
        Public Dim ItemCache as New Cache

        Public Sub New(ByVal url As String)
        	' When creating a new client, make sure the URL ends with a "/",
        	'   then store it in the global Gallery3URL variable.
        	
            If Not url.EndsWith("/") Then
                url = url & "/"
            End If
            Gallery3URL = url
        End Sub ' END New

        Public Function Login(ByVal username As String, ByVal password As String) As Boolean
            ' Log into Gallery with USERNAME/PASSWORD
            '   Returns True if successful, false otherwise.
            
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
                
                ' Store the REST Key in the Gallery3RESTKey global variable for later use.
                Gallery3RESTKey = restKey
                Return True

            Catch ex As Exception
            	' In the event of an error (such as a bad password, or a server error),
            	'   display the message and return false.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function ' END Login

        Public Function Login(ByVal RESTAPIKey As String) As Boolean
            ' Login to Gallery using a REST API key.
            '   Returns True if successful, otherwise returns False.
            
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
                
                ' Store the REST key in the global Gallery3RESTKey variable for later use,
                '   and return True for success.
                Gallery3RESTKey = RESTAPIKey
                Return True

            Catch ex As Exception
            	' In the event of an error (such as a bad password, or a server error),
            	'   display the message and return false
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function ' END Login

        Public Function GetItem(ByVal ItemID As Integer) As Linq.JObject
            ' Request the details of a specific item (ItemID).
            '  Returns a Linq.JObject containing the Item, or Nothing.
            
            Return Me.GetItem(Gallery3URL & "rest/item/" & ItemID.ToString)
        End Function

        Public Function GetItem(ByVal ItemURL As String) As Linq.JObject
            ' Request the details of a specific item (ItemID).
            '  Returns a Linq.JObject containing the item, or Nothing.
            
            Try
            	'  Make sure ItemURL does not end with a "/",
            	'   Fix it if it does.
                If ItemURL.EndsWith("/") Then
                    ItemURL = ItemURL.Substring(0, ItemURL.Length - 1)
                End If
                
                ' Check and see if this item was already requested.
                '   If so, return the cached results.  If not,
                '   request it from the server.
                Dim ItemID As Integer = Convert.ToInt32(ItemURL.Substring(ItemURL.LastIndexOf("/") + 1))
                Dim strCachedResults As String = ItemCache.GetItem(ItemID)
                If strCachedResults <> "" Then
                	Return Linq.JObject.Parse(strCachedResults)
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
                    
                    ' Add the response to the cache, and return it
                    '   as a Linq.JObject variable.
                    ItemCache.AddItem (ItemID, txtServerResponse)
                    Return Linq.JObject.Parse(txtServerResponse)
                End If
            Catch ex As Exception
            	' In the event of an error, display the error and return Nothing.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function ' END GetItem

        Public Function GetItems(ByVal ItemURLs As Linq.JToken) As List(Of String)
        	' Retrieves multiple items from the server, and returns the response for
        	'   each item in a list of Strings.
        	'   Returns Nothing in the event of an error.
        	
        	'  If no URLs were given, return Nothing.
            If ItemURLs.Count = 0 Then Return Nothing
            
            '@TODO:
            ' Right now, this retrieves everything in an album, even if it's already cached.
            '  I might want to re-work this to only retrieve files that aren't already cached?
            '  As it stands now, this forces an update for the contents of an album, but doesn't
            '  force an update for the album itself (which means new items won't necessarly show up).
            
            ' Convert the URL array until a properly formated form data for the REST request.
            Dim oneURL, txtServerRequest As String
            txtServerRequest = "urls=["
            For Each oneURL In ItemURLs
                txtServerRequest = txtServerRequest & """" & oneURL & ""","
            Next
            txtServerRequest = txtServerRequest.Substring(0, txtServerRequest.Length - 1) & "]"

            Try
                ' Send login info and list of URLs for the items being requested.
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
                Dim ItemsArray As New List(Of String)
                Dim NewItemObject As Linq.JObject
                While txtServerResponse.IndexOf(",{""url"":") > 0
                	' Store each item into ItemsArray (to be returned at the end)
                	'  and then cache the item for later use.
                    Dim tempString As String = txtServerResponse.Substring(0, txtServerResponse.IndexOf(",{""url"":"))
                    ItemsArray.Add(tempString)
                    txtServerResponse = txtServerResponse.Substring(txtServerResponse.IndexOf(",{""url"":") + 1)
                    NewItemObject = Linq.JObject.Parse(tempString)
                    ItemCache.RemoveItem (Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")))
                    ItemCache.AddItem (Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")), tempString)
                End While
                ' Add the last item from the Server's response to ItemsArray and cache it.
                ItemsArray.Add(txtServerResponse)
                NewItemObject = Linq.JObject.Parse(txtServerResponse)
                ItemCache.RemoveItem (Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")))
                ItemCache.AddItem (Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")), txtServerResponse)
                
                ' Return the download item details.
                Return ItemsArray
                
            Catch ex As Exception
            	' In the event of an error, display the message and return Nothing.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function ' END GetItems

        Public Function GetItemChecksum(ByVal ItemID As Integer, ByVal ChecksumType As String) As String
        	' Retrieve the checksum (either md5 of sha1) for the original photo/video on the Gallery server.
        	'   Checksum is returned as a string.
        	'   In the event of an error, an empty string is returned instead.
        	
            Return GetItemChecksum(Gallery3URL & "rest/itemchecksum_" & ChecksumType.ToLower & "/" & ItemID.ToString)
        End Function ' END GetItemChecksum

        Public Function GetItemChecksum(ByVal ItemURL As String) As String
        	' Retrieve the checksum (either md5 of sha1) for the original photo/video on the Gallery server.
        	'   Checksum is returned as a string.
        	'   In the event of an error, an empty string is returned instead.

            Try
                ' Send the login info and request the checksum.
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
                
                ' Extract the checksum from the server's response and return it as a string.
                Dim CheckSumObject As Linq.JObject = Linq.JObject.Parse(txtServerResponse)
                Return CheckSumObject("entity").Item("checksum")
                
            Catch ex As Exception
            	' In the event of an error, display the message and return "".
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ""
            End Try
        End Function ' END GetItemChecksum

        Function DownloadFile(ByVal ItemID As Integer, ByVal FieldName As String, ByRef SavedFileName As String) As Boolean
        	' Download the specified Photo or Video.
        	'  FieldName can be either file_url, resize_url, or thumb_url depending on the item desired.
        	'  Returns true if successful, false otherwise.
        	
            Dim ItemDetails As Linq.JObject = Me.GetItem(ItemID)
            If Not ItemDetails Is Nothing Then
                Return Me.DownloadFile(ItemDetails("entity").Item(FieldName), SavedFileName)
            Else
                Return False
            End If
        End Function ' END DownloadFile

        Function DownloadFile(ByVal url As String, ByVal SavedFileName As String, Optional ByVal DownloadProgressBar As ProgressBar = Nothing, Optional ByVal DownloadProgressText As Label = Nothing) As Boolean
        	' Download the specified Photo or Video.
        	'  Returns true if successful, false otherwise.

            ' Connect to remote server and request the file.
            Dim response As System.Net.HttpWebResponse
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

            ' Download the requested file.
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
                	' In the event of an error, display the message and return False.
                    MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            Else
            	' If the server didn't respond with "OK" return false.
                Return False
            End If
            ' If nothing went wrong by now, return true for successful.
            Return True
        End Function ' END DownloadFile.

        Function UploadFile(ByVal url As String, ByVal FileToUpload As String) As Boolean
        	' Upload a file to the Gallery Server.
        	'   Returns True if successful, false otherwise.
        	
        	Dim UploadObject As New ClassFileUpload
        	return UploadObject.Upload (url, FileToUpload, Me.Gallery3RESTKey)
        End Function ' END UploadFile
    End Class ' END Client
End Class ' End Gallery3
