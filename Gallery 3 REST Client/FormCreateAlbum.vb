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
Imports GalleryLib

Public Partial Class FormCreateAlbum
	
	' Create a global instance of GalleryClient and intParentID
	'   so the main window can link into it.
    Public GalleryClient As Gallery3.Client
    Public intParentID as Integer = 0
    
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub ButtonCreateClick(sender As Object, e As EventArgs)
		' Create the album.  If successful close the window,
		'   or else leave it open for the user to make corrections.
		
		If GalleryClient.CreateAlbum (intParentID, textDirectoryName.Text, textTitle.Text, _
		                              textDescription.Text, textInternetAddress.Text) Then
			MessageBox.Show ("Album Created Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Me.Close()
		End If
	End Sub ' END ButtonCreateClick
	
	Sub TextTitleTextChanged(sender As Object, e As EventArgs)
		' Automatically generate a directory name and internet slug based on the provided title.
		
		' Replace any special characters in the title with "-" before copying over to directory and slug.
		Dim sbCleanedTitle As New System.Text.StringBuilder
		For Each oneChar As Char In textTitle.Text
			If Char.IsLetterOrDigit(oneChar) Then
				sbCleanedTitle.Append(oneChar)
			Else
				sbCleanedTitle.Append("-")
			End If
		Next
		
		' Copy the modified title to the two other fields.
		textDirectoryName.Text = sbCleanedTitle.ToString()
		textInternetAddress.Text = sbCleanedTitle.ToString()
	End Sub ' END TextTitleTextChanged
End Class
