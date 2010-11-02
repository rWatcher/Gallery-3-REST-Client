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

Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic

Class ClassFileUpload
	Public Shared UploadFileName As String = ""
	Public Shared strServerResponse As String = ""
	
	Shared Function Upload(ByVal url As String, ByVal FileToUpload As String, ByVal Gallery3RESTKey As String) As Boolean
		' Upload the specified file.
		'   Returns true if successful, otherwise false.
		
		Try
			' Set the starting values for the global 
			'   variables that will be accessed by the thread.
			strServerResponse = ""
			UploadFileName = FileToUpload
			
			' Make sure the file exists.
			Dim UploadedFileInfo As New System.IO.FileInfo(FileToUpload)
			If Not UploadedFileInfo.Exists Then
				MessageBox.Show ("File " & UploadedFileInfo.Name & " Does Not Exist", _
								 "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return False
			End If
			
			' Create a new HttpWebRequest object to upload the file over.
			Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
			
			' Configure the request for a REST upload.
			request.UserAgent = "rWatcher's Gallery 3 Client"
			request.Method = System.Net.WebRequestMethods.Http.Post
			request.Credentials = System.Net.CredentialCache.DefaultCredentials
			request.Headers.Add("X-Gallery-Request-Method", "post")
			request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)
			request.ContentType = "multipart/form-data; boundary=----------196f00b77b968397849367c61a2080"
			
			' Start the asynchronous operation to upload the file.
			Dim result As IAsyncResult = CType(request.BeginGetRequestStream(AddressOf GetRequestStreamCallback, request), IAsyncResult)
			
			' Wait until strServerResponse has a value before exiting,
			'   that way we know the file finished uploading.
			While strServerResponse = ""
				Application.DoEvents()
			End While
			
			' If something went wrong, strServerResponse will be set to -ERROR
			'   instead of an actual response, so return FALSE if this is the
			'   case, or else return TRUE.
			If strServerResponse <> "-ERROR" Then
				Return True
			Else
				Return False
			End If
			
    	Catch ex As Exception
    		' In the event of an error, display a message, then
    		'   return false so the calling code knows something went wrong.
    		MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    		Return False
    	End Try
    End Function ' END Upload

	Private Shared Sub GetRequestStreamCallback(ByVal asynchronousResult As IAsyncResult)
		' Uploads the file to the web server.
		
		Try
			' Load the file into a byte array.
			Dim UploadedFileInfo As New System.IO.FileInfo(UploadFileName)
			Dim fStream As New System.IO.FileStream(UploadFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
			Dim fReader As New System.IO.BinaryReader(fStream)
			Dim FileData As Byte() = fReader.ReadBytes(Convert.ToInt32(UploadedFileInfo.Length))
			fReader.Close()
			
			' Set up mime data to send with the file.
			Dim strBeforeFileUpload As String = "------------196f00b77b968397849367c61a2080" & vbNewLine & _
												"Content-Disposition: form-data; name=""entity""" & _
												vbNewLine & vbNewLine & "{""name"":""" & _
												UploadedFileInfo.Name & """,""type"":""photo"",""title"":""" & _
												UploadedFileInfo.Name.Replace(UploadedFileInfo.Extension, "") & _
												"""}" & vbNewLine & "------------196f00b77b968397849367c61a2080" & _
												vbNewLine & "Content-Disposition: form-data; name=""file""; filename=""" & _
												UploadedFileInfo.Name & """" & vbNewLine & "Content-Type: application/octet-stream" & _
												vbNewLine & vbNewLine
			Dim strAfterFileUpload As String = vbNewLine & "------------196f00b77b968397849367c61a2080--" & vbNewLine
			
			' Set up web connection and upload the file + mime data.
			Dim request As HttpWebRequest = CType(asynchronousResult.AsyncState, HttpWebRequest)
			Dim postStream As Stream =  request.EndGetRequestStream(asynchronousResult)
			postStream.Write (System.Text.Encoding.UTF8.GetBytes(strBeforeFileUpload), 0, _
							  System.Text.Encoding.UTF8.GetBytes(strBeforeFileUpload).Length)
			postStream.Write (FileData, 0, FileData.Length)
			postStream.Write (System.Text.Encoding.UTF8.GetBytes(strAfterFileUpload), 0, _
							  System.Text.Encoding.UTF8.GetBytes(strAfterFileUpload).Length)
			postStream.Close()
			
			' Get response from file upload (which will contain the URL of the uploaded item.
			Dim result As IAsyncResult = CType(request.BeginGetResponse(AddressOf GetResponseCallback, request), IAsyncResult)

    	Catch ex As Exception
    		' In the event of an error, display a message, then set
    		'   strServerResponse to "-ERROR" so the calling function
    		'   knows something went wrong.
    		MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    		strServerResponse = "-ERROR"
    	End Try
    End Sub ' ReadRequestStreamCallback

    Private Shared Sub GetResponseCallback(ByVal asynchronousResult As IAsyncResult)
    	' Get's the response from the file upload and stores it in strServerResponse.

    	Try
    		' Set up the response connection.
    		Dim request As HttpWebRequest = CType(asynchronousResult.AsyncState, HttpWebRequest)
    		Dim response As HttpWebResponse = CType(request.EndGetResponse(asynchronousResult), HttpWebResponse)
    		Dim streamResponse As Stream = response.GetResponseStream()
    		Dim streamRead As New StreamReader(streamResponse)
    		
    		' Retrieve the server response and store in a global variable.
    		strServerResponse = streamRead.ReadToEnd()
    		
    		' Close out the objects and exit.
    		streamResponse.Close()
    		streamRead.Close()
    		response.Close()
    		
    	Catch ex As Exception
    		' In the event of an error, display a message, then set
    		'   strServerResponse to "-ERROR" so the calling function
    		'   knows something went wrong.
    		MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    		strServerResponse = "-ERROR"
    	End Try
    End Sub ' END ReadResponseCallback

End Class ' END ClassFileUpload
