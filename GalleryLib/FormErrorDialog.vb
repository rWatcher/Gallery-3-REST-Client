'
' Created by SharpDevelop.
' User: Eric
' Date: 11/14/2010
' Time: 4:09 PM
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
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
