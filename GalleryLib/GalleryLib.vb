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
Imports System.Windows.Forms

Public Class Gallery3
    Public Class Cache
        ' Maintains a cache of REST responses, to prevent the client from requesting
        '   the same resource over and over.

        ' Stores and ID number, and the results as a string.
        Friend Class Item
            Friend ItemID As Integer
            Friend QueryResults As String
        End Class ' END Item

        ' Create a list to store the actual data in.
        Private CachedItems As New List(Of Cache.Item)

        '''<summary>
        '''<para>Retrieves the number of items currently in the cache.</para>
        '''</summary>
        '''<returns>The number of items in the cache.</returns>
        Public Function Count() As Integer
            Return CachedItems.Count
        End Function ' END Count

        '''<summary>
        '''<para>Retrieves a specific item from the cache.</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to return</param>
        '''<returns>Original server response or an empty string if the item isn't cached.</returns>
        Public Function GetItem(ByVal ItemID As Integer) As String
            Dim CachedResults = From g3items In CachedItems Where g3items.ItemID = ItemID
            If CachedResults.Count > 0 Then
                Return CachedResults(0).QueryResults
            Else
                Return ""
            End If
        End Function ' END GetItem

        '''<summary>
        '''<para>Removes an item from the cache (if it's in there).</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to remove.</param>
        Public Sub RemoveItem(ByVal ItemID As Integer)
            Dim counter As Integer = 0
            While counter < CachedItems.Count
                If CachedItems(counter).ItemID = ItemID Then
                    CachedItems.RemoveAt(counter)
                End If
                counter = counter + 1
            End While
        End Sub ' END RemoveItem

        '''<summary>
        '''<para>Adds a new item to the cache.</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to remove.</param>
        '''<param name="txtQueryResults">The server response string.</param>
        Public Sub AddItem(ByVal ItemID As Integer, ByVal txtQueryResults As String)
            RemoveItem(ItemID)
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
        Public ItemCache As New Cache

        '''<summary>
        '''<para>Create a new instance of Client.</para>
        '''</summary>
        '''<param name="url">The URL for the Gallery 3 web site.</param>
        Public Sub New(ByVal url As String)
            ' When creating a new client, make sure the URL ends with a "/",
            '   then store it in the global Gallery3URL variable.

            If Not url.EndsWith("/") Then
                url = url & "/"
            End If
            Gallery3URL = url
        End Sub ' END New
        
        '''<summary>
        '''<para>Returns the REST API Key currently being used.</para>
        '''</summary>
        '''<returns>REST API Key</returns>
        Public Function GetRESTKey() As String
        	'  Used by the main app to save the key to the config file.
        	
        	Return Gallery3RESTKey
        End Function ' END GetRESTKey
        
        '''<summary>
        '''<para>Converts a user name and password into a REST API key.</para>
        '''</summary>
        '''<param name="username">The login name.</param>
        '''<param name="password">The corresponding password.</param>
        '''<returns>True if successful, false if not.</returns>
        Public Function Login(ByVal username As String, ByVal password As String) As Boolean
            Try
                ' Send login info.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/"), System.Net.HttpWebRequest)
                request.UserAgent = "rWatcher's Gallery 3 Client"
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

        '''<summary>
        '''<para>Logs into gallery with a REST key to make sure its valid.</para>
        '''</summary>
        '''<param name="RESTAPIKey">REST API Key to log in with.</param>
        '''<returns>True is successful, False if not.</returns>
        Public Function Login(ByVal RESTAPIKey As String) As Boolean
            Try
                ' Send the login request.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/item/1/"), System.Net.HttpWebRequest)
                request.UserAgent = "rWatcher's Gallery 3 Client"
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

        '''<summary>
        '''<para>Retrieves the details of an item from the Gallery server.</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to retrieve.</param>
        '''<returns>The details for the specified item.  On error returns Nothing.</returns>
        Public Function GetItem(ByVal ItemID As Integer) As Linq.JObject
            Return Me.GetItem(Gallery3URL & "rest/item/" & ItemID.ToString)
        End Function

        '''<summary>
        '''<para>Retrieves the details of an item from the Gallery server.</para>
        '''</summary>
        '''<param name="ItemURL">Web URL for the item to retrieve.</param>
        '''<returns>The details for the specified item.  On error returns Nothing.</returns>
        Public Function GetItem(ByVal ItemURL As String) As Linq.JObject
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
                    request.UserAgent = "rWatcher's Gallery 3 Client"
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
                    ItemCache.AddItem(ItemID, txtServerResponse)
                    Return Linq.JObject.Parse(txtServerResponse)
                End If
            Catch ex As Exception
                ' In the event of an error, display the error and return Nothing.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function ' END GetItem
        
        '''<summary>
        '''<para>Retrieves multiple items from the server, and returns
        '''the response for each item in a list of strings.</para>
        '''</summary>
        '''<param name="ItemURLs">List of URLs to retrieve</param>
        '''<returns>Server response data ast List(Of String).  On error returns Nothing.</returns>
        Public Function GetItems(ByVal ItemURLs As Linq.JToken) As List(Of String)
            '  If no URLs were given, return Nothing.
            If ItemURLs.Count = 0 Then Return Nothing
            
            ' Store the return list in here.
            Dim ItemsArray As New List(Of String)

            ' Loop through each URL that was passed into the function.
            '   If the URL is not already in the cache, add the URL to
            '   a REST request to retrieve the details for it.
            Dim oneURL, txtServerRequest As String
            txtServerRequest = "urls=["
            For Each oneURL In ItemURLs
            	
            	' Figure out the ID# for the current URL.
            	Dim ItemID as Integer
            	If oneURL.EndsWith("/") Then
            		Dim tempURL as String = oneURL.Substring(0, oneURL.Length - 1)
            		ItemID = Convert.ToInt32(tempURL.Substring(tempURL.LastIndexOf("/") + 1))
            	Else
            		ItemID = Convert.ToInt32(oneURL.Substring(oneURL.LastIndexOf("/") + 1))
                End If
                
                ' Check and see if this item is already cached.
                '   If it is, used the cached response instead of 
                '   re-requesting it.
                Dim strCachedResults As String = ItemCache.GetItem(ItemID)
                If strCachedResults = "" Then
                	txtServerRequest = txtServerRequest & """" & oneURL & ""","
                Else
                	ItemsArray.Add(strCachedResults)
                End If
            Next
            
            ' Replace the ending "," character with a "]" to complete the string.
            txtServerRequest = txtServerRequest.Substring(0, txtServerRequest.Length - 1) & "]"
            
            ' If everything is already in the cache, just return the cached results.
            If txtServerRequest = "urls=]" Then
            	Return ItemsArray
            End If

            Try
                ' Send login info and list of URLs for the items being requested.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/items/"), System.Net.HttpWebRequest)
                request.UserAgent = "rWatcher's Gallery 3 Client"
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
                Dim NewItemObject As Linq.JObject
                While txtServerResponse.IndexOf(",{""url"":") > 0
                    ' Store each item into ItemsArray (to be returned at the end)
                    '  and then cache the item for later use.
                    Dim tempString As String = txtServerResponse.Substring(0, txtServerResponse.IndexOf(",{""url"":"))
                    ItemsArray.Add(tempString)
                    txtServerResponse = txtServerResponse.Substring(txtServerResponse.IndexOf(",{""url"":") + 1)
                    NewItemObject = Linq.JObject.Parse(tempString)
                    ItemCache.RemoveItem(Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")))
                    ItemCache.AddItem(Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")), tempString)
                End While
                ' Add the last item from the Server's response to ItemsArray and cache it.
                ItemsArray.Add(txtServerResponse)
                NewItemObject = Linq.JObject.Parse(txtServerResponse)
                ItemCache.RemoveItem(Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")))
                ItemCache.AddItem(Convert.ToInt32(NewItemObject("entity").Item("id").ToString.Replace("""", "")), txtServerResponse)

                ' Return the download item details.
                Return ItemsArray

            Catch ex As Exception
                ' In the event of an error, display the message and return Nothing.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function ' END GetItems

        '''<summary>
        '''<para>Retrieves the MD5 or SHA-1 checksum for the specified photo or video.</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to retrieve.</param>
        '''<param name="ChecksumType">Type of checksum to retrieve (md5 or sha1).</param>
        '''<returns>The checksum or "".</returns>
        Public Function GetItemChecksum(ByVal ItemID As Integer, ByVal ChecksumType As String) As String
            Return GetItemChecksum(Gallery3URL & "rest/itemchecksum_" & ChecksumType.ToLower & "/" & ItemID.ToString)
        End Function ' END GetItemChecksum

        '''<summary>
        '''<para>Retrieves the MD5 or SHA-1 checksum for the specified photo or video.</para>
        '''</summary>
        '''<param name="ItemURL">The full URL for the resource containing the desired checksum.</param>
        '''<returns>The checksum or "".</returns>
        Public Function GetItemChecksum(ByVal ItemURL As String) As String
            Try
                ' Send the login info and request the checksum.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(ItemURL), System.Net.HttpWebRequest)
                request.UserAgent = "rWatcher's Gallery 3 Client"
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

        '''<summary>
        '''<para>Downloads the specified file over a REST authenticated connection.</para>
        '''</summary>
        '''<param name="ItemID">ID number of the item to retrieve.</param>
        '''<param name="FieldName">The field containing the URL to the type of file desired (file_url, resize_url, or thumb_url).</param>
        '''<param name="SavedFileName">The location to save the downloaded file to.</param>
        '''<returns>True if successful, false otherwise.</returns>
        Function DownloadFile(ByVal ItemID As Integer, ByVal FieldName As String, ByVal SavedFileName As String) As Boolean
            Dim ItemDetails As Linq.JObject = Me.GetItem(ItemID)
            If Not ItemDetails Is Nothing Then
                Return Me.DownloadFile(ItemDetails("entity").Item(FieldName), SavedFileName)
            Else
                Return False
            End If
        End Function ' END DownloadFile

        '''<summary>
        '''<para>Downloads the specified file over a REST authenticated connection.</para>
        '''</summary>
        '''<param name="url">The URL of the file to download</param>
        '''<param name="SavedFileName">The location to save the downloaded file to.</param>
        '''<param name="ShowProgress">(Optional) If true, creates a dialog to display how much of the file has been downloaded.</param>
        '''<returns>True if successful, false otherwise.</returns>
        Function DownloadFile(ByVal url As String, ByVal SavedFileName As String, Optional ByVal ShowProgress as Boolean = False) As Boolean
        	' Create a new ClassFileDownload Object, and store the provided parameters into it.
        	Dim objDownloadFile As New ClassFileDownload
        	objDownloadFile.strURL = url
        	objDownloadFile.strSavedFileName = SavedFileName
        	objDownloadFile.Gallery3RESTKey = Gallery3RESTKey 
        	objDownloadFile.boolShowDownloadProgress = ShowProgress
        	
        	' Run the download as a seperate thread, so it won't slow down the main application.
        	Dim threadDownload As New Threading.Thread (AddressOf objDownloadFile.DownloadFile)
        	threadDownload.Start ()
        	
        	' Wait until the thread exits, then return it's return value.
        	While threadDownload.IsAlive()
        		Application.DoEvents()
        	End While
        	Return objDownloadFile.boolReturnValue
        End Function ' END DownloadFile.

        '''<summary>
        '''<para>Uploads a file to the Gallery server over a REST authenticated connection.</para>
        '''</summary>
        '''<param name="url">Web address of the album to upload the file into.</param>
        '''<param name="FileToUpload">The location of the photo or video to upload.</param>
        '''<returns>True if successful, false otherwise.</returns>
        Function UploadFile(ByVal url As String, ByVal FileToUpload As String) As Boolean
            Dim UploadObject As New ClassFileUpload
            Return UploadObject.Upload(url, FileToUpload, Gallery3RESTKey)
        End Function ' END UploadFile
        
        '''<summary>
        '''<para>Creates a new album on the Gallery server.</para>
        '''</summary>
        '''<param name="intParentID">ID number of the parent album.</param>
        '''<param name="strFolderName">Desired file name to use for the album's directory.</param>
        '''<param name="strAlbumTitle">The title for the new album.</param>
        '''<param name="strAlbumDescription">A description for the album.</param>
        '''<param name="strURLSlug">The slug to use in the album's url.</param>
        '''<returns>True if successful, false otherwise.</returns>
        Function CreateAlbum (ByVal intParentID As Integer, ByVal strFolderName As String, ByVal strAlbumTitle As String, ByVal strAlbumDescription As String, ByVal strURLSlug As String) As Boolean
        	' Convert the provided information into urlencoded form data to submit to the gallery server.
            Dim txtServerRequest As String = "{""name"":""" & strFolderName & _
                                             """,""title"":""" & strAlbumTitle.Replace("""", "\""") & _
                                             """,""type"":""album"",""description"":""" & strAlbumDescription.Replace("""", "\""").Replace(vbNewLine, "\n") & _
                                             """,""slug"":""" & strURLSlug & """}"
			txtServerRequest = "entity=" & System.Web.HttpUtility.UrlEncode(txtServerRequest)
			
            Try
            	' Open a new web connection to create the album with.
                Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(Gallery3URL & "rest/item/" & intParentID.ToString), System.Net.HttpWebRequest)
				request.UserAgent = "rWatcher's Gallery 3 Client"
                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.Headers.Add("X-Gallery-Request-Method", "post")
                request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)
                
                ' Transmit the variables for the new album.
                Dim datastream As System.IO.Stream = request.GetRequestStream
                Dim encoding As New System.Text.UTF8Encoding
                datastream.Write(encoding.GetBytes(txtServerRequest), 0, encoding.GetBytes(txtServerRequest).Length)
                datastream.Close()
                
                ' Get the server's response.  This contains the URL of the new item
                '   (which we don't do anything with).
                Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                Dim receiveStream As System.IO.Stream = response.GetResponseStream()
                Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
                Dim txtServerResponse As String = readStream.ReadToEnd
                response.Close()
                readStream.Close()
                
                ' Return true.
                return True
            Catch ex As Exception
                ' In the event of an error, display the message and return false.
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try        	
        End Function ' END CreateAlbum
    End Class ' END Client
End Class ' End Gallery3
