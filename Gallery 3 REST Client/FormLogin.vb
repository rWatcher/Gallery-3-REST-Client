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
		' Login to the remote Gallery with the provided information.
		'   If successful, open the album browser window.
		'   If not, display an error, and wait exit the sub so the 
		'   user can fix it.
		
		' Make sure a URL was provided.
		If txtGalleryURL.Text = "" Then
            MessageBox.Show("Please specify a URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        
        ' Make sure either a username/password combo is present, or a rest key is present.
        If (txtUsername.Text = "" Or txtPassword.Text = "") And (txtRESTKey.Text = "") Then
            MessageBox.Show("Please specify a User Name/Password or REST Key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        
        ' Create a new Gallery 3 Client instance and attempt to log in.
        '  Exit the sub on error.
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
        
        ' Login was successful, open up the album window and connect it to the 
        '   existing Gallery Client instance.
        statusLabelLogin.Text = "Login Successful, Downloading Root Album Data"
        Application.DoEvents()
        Dim GalleryAlbumWindow As New formAlbumBrowser
        GalleryAlbumWindow.GalleryClient = galleryClient
        GalleryAlbumWindow.Text = "Connected to " & txtGalleryURL.Text.Replace("http://", "")
        Me.Hide()
        GalleryAlbumWindow.Show()
        
        ' Wait until the album window is closed before exiting.
        While GalleryAlbumWindow.Visible = True
        	application.DoEvents()
        End While
        Me.Close()
	End Sub ' END ButtonLoginClick
End Class ' END FormLogin
