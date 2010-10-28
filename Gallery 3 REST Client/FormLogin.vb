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
Public Partial Class FormLogin
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub ButtonLoginClick(sender As Object, e As EventArgs)
		If txtGalleryURL.Text = "" Then
            MessageBox.Show("Please specify a URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If (txtUsername.Text = "" Or txtPassword.Text = "") And (txtRESTKey.Text = "") Then
            MessageBox.Show("Please specify a User Name/Password or REST Key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim galleryClient As New Gallery3.Client(txtGalleryURL.Text)
        statusLabelLogin.Text = "Connecting to Server"
        Application.DoEvents()
        If txtRESTKey.Text <> "" Then
            If Not galleryClient.login(txtRESTKey.Text) Then
                statusLabelLogin.Text = "Login Failed"
                Exit Sub
            End If
        Else
            If Not galleryClient.login(txtUsername.Text, txtPassword.Text) Then
                statusLabelLogin.Text = "Login Failed"
                Exit Sub
            End If
        End If
        statusLabelLogin.Text = "Login Successful, Downloading Root Album Data"
        Application.DoEvents()
        Dim GalleryAlbumWindow As New formAlbumBrowser
        GalleryAlbumWindow.GalleryClient = galleryClient
        GalleryAlbumWindow.Text = "Connected to " & txtGalleryURL.Text.Replace("http://", "")
        Me.Hide()
        GalleryAlbumWindow.Show()
        'Me.Close()
        While GalleryAlbumWindow.Visible = True
        	application.DoEvents()
        End While
        Me.Close()
	End Sub
End Class
