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
Imports System.Windows.Forms

Friend Class ClassFileDownload
	Public strURL As String = ""
	Public strSavedFileName As String = ""
	Public Gallery3RESTKey as String = ""
	Public DownloadProgressBar As ProgressBar = Nothing
	Public DownloadProgressText As Label = Nothing
	Public boolReturnValue As Boolean = False
	
	Sub DownloadFile()
		' Connect to remote server and request the file.
		Dim response As System.Net.HttpWebResponse
		Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(strURL), System.Net.HttpWebRequest)
		request.UserAgent = "rWatcher's Gallery 3 Client"
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
			boolReturnValue = False
			Exit Sub
		End Try
		
		' If the server responded with "OK", download the file.
		Dim DownloadedLength As Integer = 0
		If response.StatusDescription.ToString = "OK" Then
			
			' If a progress bar was provided, set its Maximum value.
			If Not DownloadProgressBar Is Nothing Then
				If response.ContentLength <> -1 Then
					DownloadProgressBar.Maximum = Convert.ToInt32(response.ContentLength)
				End If
			End If
			Application.DoEvents()
			
			' Start downloading the file.
			Dim dataStream As System.IO.Stream
			Dim reader As System.IO.BinaryReader
			Try
				Dim downloadedFile As New System.IO.FileStream(strSavedFileName, IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write)
				Dim writeFile As New System.IO.BinaryWriter(downloadedFile)
				dataStream = response.GetResponseStream()
				reader = New System.IO.BinaryReader(dataStream)
				Dim length As Integer
				Dim buffer(4096) As Byte
				length = reader.Read(buffer, 0, 4096)
				DownloadedLength = DownloadedLength + length
				
				' If a progress bar was provided, update it with the current amount downloaded.
				If Not DownloadProgressBar Is Nothing Then
					If response.ContentLength = -1 Then
						DownloadProgressBar.Maximum = DownloadedLength
					End If
					DownloadProgressBar.Value = DownloadedLength
				End If
				
				' If a label was provided, update it with the current amount downloaded.
				If Not DownloadProgressText Is Nothing Then
					DownloadProgressText.Text = Math.Round(DownloadedLength / 1024 / 1024, 2).ToString & "MB"
				End If
				
				' Download the rest of the file in chunks so
				'   we can keep the app responsive and update
				'   any visual progress indicators.
				While (length > 0) And (Not (buffer Is Nothing))
					Application.DoEvents()
					writeFile.Write(buffer, 0, length)
					Application.DoEvents()
					length = reader.Read(buffer, 0, 4096)
					DownloadedLength = DownloadedLength + length
					
					' If a progress bar was provided, update it with the current amount downloaded.
					If Not DownloadProgressBar Is Nothing Then
						DownloadProgressBar.Value = DownloadedLength
					End If
					
					' If a label was provided, update it with thecurrent amount downloaded.
					If Not DownloadProgressText Is Nothing Then
						DownloadProgressText.Text = Math.Round(DownloadedLength / 1024 / 1024, 2).ToString & "MB"
					End If
				End While
				
				' File download complete, close out everything before exiting.
				writeFile.Flush()
				writeFile.Close()
				reader.Close()
				dataStream.Close()
				
			Catch ex As Exception
				' In the event of an error, display the message and return False.
				MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				boolReturnValue = False
				Exit Sub
			End Try
		Else
			' If the server didn't respond with "OK" return false.
			boolReturnValue = False
			Exit Sub
		End If
		' If nothing went wrong by now, return true for successful.
		boolReturnValue = True
	End Sub ' END DownloadFile.
End Class ' END ClassFileDownload
