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
Imports System.Net
Imports System.Threading


Public Class ClassFileUpload
Public Class RequestState
  Public request As HttpWebRequest = Nothing
  Public response As HttpWebResponse = Nothing
End Class

Public Class AsyncUpload
  Shared allDone As ManualResetEvent = New ManualResetEvent(False)

  Shared Sub TimeoutCallback(ByVal state As Object, ByVal timedOut As Boolean)
    If timedOut Then
      Dim request As HttpWebRequest = CType(state, HttpWebRequest)
      If Not (request Is Nothing) Then
        request.Abort()
      End If
    End If
  End Sub

  Shared Function Upload(ByVal url As String, ByVal FileToUpload As String, ByVal Gallery3RESTKey as String) as Boolean
    Try
    	
                Dim encoding As New System.Text.UTF8Encoding
                
                ' Load the file into a byte array.
                Dim UploadedFileInfo As New System.IO.FileInfo(FileToUpload)
                Dim fStream As New System.IO.FileStream(FileToUpload, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                Dim br As New System.IO.BinaryReader(fStream)
                Dim FileData As Byte() = br.ReadBytes(Convert.ToInt32(UploadedFileInfo.Length))
                br.Close()
                
                ' Set up mime data to send with the file.
                Dim strBeforeFileUpload As String = "------------196f00b77b968397849367c61a2080" & vbNewLine & _
                									"Content-Disposition: form-data; name=""entity""" & vbNewLine & vbNewLine & _
                									"{""name"":""" & UploadedFileInfo.Name & """,""type"":""photo"",""title"":""" & UploadedFileInfo.Name.Replace(UploadedFileInfo.Extension, "") & """}" & vbNewLine & _
                									"------------196f00b77b968397849367c61a2080" & vbNewLine & _
                									"Content-Disposition: form-data; name=""file""; filename=""" & UploadedFileInfo.Name & """" & vbNewLine & _
                									"Content-Type: application/octet-stream" & vbNewLine & vbNewLine
                dim strAfterFileUpload as String = vbNewLine & "------------196f00b77b968397849367c61a2080--" & vbNewLine

    	
      Dim myRequestState As New RequestState()
      myRequestState.request = CType(WebRequest.Create(url), HttpWebRequest)
      
      
                myRequestState.request.UserAgent = "rWatcher's Gallery 3 Client"
                myRequestState.request.Method = System.Net.WebRequestMethods.Http.Post
                myRequestState.request.AllowWriteStreamBuffering = False
                myRequestState.request.ContentLength = FileData.Length + encoding.GetBytes(strBeforeFileUpload).Length + encoding.GetBytes(strAfterFileUpload).Length
                myRequestState.request.Credentials = System.Net.CredentialCache.DefaultCredentials
                myRequestState.request.Headers.Add("X-Gallery-Request-Method", "post")
                myRequestState.request.Headers.Add("X-Gallery-Request-Key", Gallery3RESTKey)
                myRequestState.request.ContentType = "multipart/form-data; boundary=----------196f00b77b968397849367c61a2080"
                myRequestState.request.Method = "POST"
                Dim postStream As System.IO.Stream = myRequestState.request.GetRequestStream()
                
          	    postStream.Write (System.Text.Encoding.UTF8.GetBytes(strBeforeFileUpload), 0, System.Text.Encoding.UTF8.GetBytes(strBeforeFileUpload).Length)
          	    postStream.Write (FileData, 0, FileData.Length)
          	    postStream.Write (System.Text.Encoding.UTF8.GetBytes(strAfterFileUpload), 0, System.Text.Encoding.UTF8.GetBytes(strAfterFileUpload).Length)

      Dim ar As IAsyncResult = myRequestState.request.BeginGetResponse(New AsyncCallback(AddressOf ResponseCallback), myRequestState)

      ' Wait for request to complete
      ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle, _
        New WaitOrTimerCallback(AddressOf TimeoutCallback), myRequestState.request, 60000, True)
      ar.AsyncWaitHandle.WaitOne()
      Console.WriteLine("Request completed.")
      ' Wait for callback to signal
      allDone.WaitOne()
      'Console.WriteLine("Response status code = {0}", myRequestState.response.StatusCode)
      'MessageBox.Show (myRequestState.response.Headers.ToString)
      'MessageBox.Show (myRequestState.response.StatusCode)
      return true
    Catch ex As WebException
      Console.WriteLine("Exception in Main:")
      Console.WriteLine("Message: {0}", ex.Message)
      Console.WriteLine("Status: {0}", ex.Status)
      return false
    End Try
  End Function

  Shared Sub ResponseCallback(ByVal ar As IAsyncResult)
    Try
      Dim myRequestState As RequestState = CType(ar.AsyncState, RequestState)
      myRequestState.response = CType(myRequestState.request.EndGetResponse(ar), HttpWebResponse)
    Catch ex As WebException
      Console.WriteLine("Exception in Main:")
      Console.WriteLine("Message: {0}", ex.Message)
      Console.WriteLine("Status: {0}", ex.Status)
    End Try
    allDone.Set()
  End Sub
End Class
	
End Class
