'  Gallery 3 REST Client
'  Copyright 2010-2012 Eric Cavaliere
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
Public Partial Class FormErrorDialog
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub SetText(ByVal strTitle As String, ByVal strErrorMessage As String, ByVal strErrorDetails As String)
		Me.Text = strTitle
		Me.labelErrorMsg.Text = strErrorMessage
		Me.textBoxErrorDetails.Text = strErrorDetails
	End Sub
	
	Sub ButtonOKClick(sender As Object, e As EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
	End Sub
	
	Sub ButtonDetailsClick(sender As Object, e As EventArgs)
		If Me.Height = 122 Then
			Me.Height = 277
		Else
			Me.Height = 122
		End If
	End Sub
End Class
