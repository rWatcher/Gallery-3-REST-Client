'
' Created by SharpDevelop.
' User: Eric
' Date: 11/14/2010
' Time: 4:09 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class FormErrorDialog
	Inherits System.Windows.Forms.Form
	
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormErrorDialog))
		Me.buttonOK = New System.Windows.Forms.Button
		Me.labelErrorMsg = New System.Windows.Forms.Label
		Me.buttonDetails = New System.Windows.Forms.Button
		Me.textBoxErrorDetails = New System.Windows.Forms.TextBox
		Me.SuspendLayout
		'
		'buttonOK
		'
		Me.buttonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonOK.Location = New System.Drawing.Point(251, 63)
		Me.buttonOK.Name = "buttonOK"
		Me.buttonOK.Size = New System.Drawing.Size(75, 23)
		Me.buttonOK.TabIndex = 0
		Me.buttonOK.Text = "Okay"
		Me.buttonOK.UseVisualStyleBackColor = true
		AddHandler Me.buttonOK.Click, AddressOf Me.ButtonOKClick
		'
		'labelErrorMsg
		'
		Me.labelErrorMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.labelErrorMsg.Location = New System.Drawing.Point(12, 9)
		Me.labelErrorMsg.Name = "labelErrorMsg"
		Me.labelErrorMsg.Size = New System.Drawing.Size(329, 51)
		Me.labelErrorMsg.TabIndex = 1
		Me.labelErrorMsg.Text = "%ERROR%"
		'
		'buttonDetails
		'
		Me.buttonDetails.Location = New System.Drawing.Point(23, 63)
		Me.buttonDetails.Name = "buttonDetails"
		Me.buttonDetails.Size = New System.Drawing.Size(75, 23)
		Me.buttonDetails.TabIndex = 2
		Me.buttonDetails.Text = "Details..."
		Me.buttonDetails.UseVisualStyleBackColor = true
		AddHandler Me.buttonDetails.Click, AddressOf Me.ButtonDetailsClick
		'
		'textBoxErrorDetails
		'
		Me.textBoxErrorDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textBoxErrorDetails.BackColor = System.Drawing.SystemColors.InactiveBorder
		Me.textBoxErrorDetails.Location = New System.Drawing.Point(12, 102)
		Me.textBoxErrorDetails.Multiline = true
		Me.textBoxErrorDetails.Name = "textBoxErrorDetails"
		Me.textBoxErrorDetails.ReadOnly = true
		Me.textBoxErrorDetails.Size = New System.Drawing.Size(329, 135)
		Me.textBoxErrorDetails.TabIndex = 3
		'
		'FormErrorDialog
		'
		Me.AcceptButton = Me.buttonOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(353, 94)
		Me.Controls.Add(Me.textBoxErrorDetails)
		Me.Controls.Add(Me.buttonDetails)
		Me.Controls.Add(Me.labelErrorMsg)
		Me.Controls.Add(Me.buttonOK)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormErrorDialog"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "FormErrorDialog"
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private textBoxErrorDetails As System.Windows.Forms.TextBox
	Private buttonDetails As System.Windows.Forms.Button
	Private labelErrorMsg As System.Windows.Forms.Label
	Private buttonOK As System.Windows.Forms.Button
End Class
