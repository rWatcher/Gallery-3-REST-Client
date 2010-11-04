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
Partial Class FormLogin
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLogin))
		Me.pictureGalleryLogo = New System.Windows.Forms.PictureBox
		Me.lblLoginTitle = New System.Windows.Forms.Label
		Me.groupBoxURL = New System.Windows.Forms.GroupBox
		Me.lblURLExample = New System.Windows.Forms.Label
		Me.txtGalleryURL = New System.Windows.Forms.TextBox
		Me.groupBoxLogin = New System.Windows.Forms.GroupBox
		Me.lblLoginInfo = New System.Windows.Forms.Label
		Me.lblRestKey = New System.Windows.Forms.Label
		Me.lblPassword = New System.Windows.Forms.Label
		Me.lblUsername = New System.Windows.Forms.Label
		Me.txtRESTKey = New System.Windows.Forms.TextBox
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtUsername = New System.Windows.Forms.TextBox
		Me.statusStripLogin = New System.Windows.Forms.StatusStrip
		Me.statusLabelLogin = New System.Windows.Forms.ToolStripStatusLabel
		Me.buttonLogin = New System.Windows.Forms.Button
		Me.checkSaveLoginDetails = New System.Windows.Forms.CheckBox
		CType(Me.pictureGalleryLogo,System.ComponentModel.ISupportInitialize).BeginInit
		Me.groupBoxURL.SuspendLayout
		Me.groupBoxLogin.SuspendLayout
		Me.statusStripLogin.SuspendLayout
		Me.SuspendLayout
		'
		'pictureGalleryLogo
		'
		Me.pictureGalleryLogo.Image = CType(resources.GetObject("pictureGalleryLogo.Image"),System.Drawing.Image)
		Me.pictureGalleryLogo.Location = New System.Drawing.Point(12, 12)
		Me.pictureGalleryLogo.Name = "pictureGalleryLogo"
		Me.pictureGalleryLogo.Size = New System.Drawing.Size(107, 48)
		Me.pictureGalleryLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.pictureGalleryLogo.TabIndex = 0
		Me.pictureGalleryLogo.TabStop = false
		'
		'lblLoginTitle
		'
		Me.lblLoginTitle.AutoSize = true
		Me.lblLoginTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblLoginTitle.Location = New System.Drawing.Point(159, 24)
		Me.lblLoginTitle.Name = "lblLoginTitle"
		Me.lblLoginTitle.Size = New System.Drawing.Size(228, 24)
		Me.lblLoginTitle.TabIndex = 1
		Me.lblLoginTitle.Text = "Gallery 3 Remote Login"
		'
		'groupBoxURL
		'
		Me.groupBoxURL.Controls.Add(Me.lblURLExample)
		Me.groupBoxURL.Controls.Add(Me.txtGalleryURL)
		Me.groupBoxURL.Location = New System.Drawing.Point(22, 75)
		Me.groupBoxURL.Name = "groupBoxURL"
		Me.groupBoxURL.Size = New System.Drawing.Size(393, 70)
		Me.groupBoxURL.TabIndex = 2
		Me.groupBoxURL.TabStop = false
		Me.groupBoxURL.Text = "Gallery 3 URL"
		'
		'lblURLExample
		'
		Me.lblURLExample.AutoSize = true
		Me.lblURLExample.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblURLExample.Location = New System.Drawing.Point(51, 42)
		Me.lblURLExample.Name = "lblURLExample"
		Me.lblURLExample.Size = New System.Drawing.Size(268, 13)
		Me.lblURLExample.TabIndex = 1
		Me.lblURLExample.Text = "Example:  http://www.example.com/gallery3/index.php"
		'
		'txtGalleryURL
		'
		Me.txtGalleryURL.Location = New System.Drawing.Point(6, 19)
		Me.txtGalleryURL.Name = "txtGalleryURL"
		Me.txtGalleryURL.Size = New System.Drawing.Size(381, 20)
		Me.txtGalleryURL.TabIndex = 0
		'
		'groupBoxLogin
		'
		Me.groupBoxLogin.Controls.Add(Me.lblLoginInfo)
		Me.groupBoxLogin.Controls.Add(Me.lblRestKey)
		Me.groupBoxLogin.Controls.Add(Me.lblPassword)
		Me.groupBoxLogin.Controls.Add(Me.lblUsername)
		Me.groupBoxLogin.Controls.Add(Me.txtRESTKey)
		Me.groupBoxLogin.Controls.Add(Me.txtPassword)
		Me.groupBoxLogin.Controls.Add(Me.txtUsername)
		Me.groupBoxLogin.Location = New System.Drawing.Point(22, 163)
		Me.groupBoxLogin.Name = "groupBoxLogin"
		Me.groupBoxLogin.Size = New System.Drawing.Size(393, 169)
		Me.groupBoxLogin.TabIndex = 3
		Me.groupBoxLogin.TabStop = false
		Me.groupBoxLogin.Text = "Login"
		'
		'lblLoginInfo
		'
		Me.lblLoginInfo.Location = New System.Drawing.Point(14, 16)
		Me.lblLoginInfo.Name = "lblLoginInfo"
		Me.lblLoginInfo.Size = New System.Drawing.Size(361, 31)
		Me.lblLoginInfo.TabIndex = 6
		Me.lblLoginInfo.Text = "Please enter your Username/Password OR the REST API Key from your profile page."
		'
		'lblRestKey
		'
		Me.lblRestKey.AutoSize = true
		Me.lblRestKey.Location = New System.Drawing.Point(14, 128)
		Me.lblRestKey.Name = "lblRestKey"
		Me.lblRestKey.Size = New System.Drawing.Size(80, 13)
		Me.lblRestKey.TabIndex = 5
		Me.lblRestKey.Text = "REST API Key:"
		'
		'lblPassword
		'
		Me.lblPassword.AutoSize = true
		Me.lblPassword.Location = New System.Drawing.Point(14, 89)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.Size = New System.Drawing.Size(56, 13)
		Me.lblPassword.TabIndex = 4
		Me.lblPassword.Text = "Password:"
		'
		'lblUsername
		'
		Me.lblUsername.AutoSize = true
		Me.lblUsername.Location = New System.Drawing.Point(14, 63)
		Me.lblUsername.Name = "lblUsername"
		Me.lblUsername.Size = New System.Drawing.Size(58, 13)
		Me.lblUsername.TabIndex = 3
		Me.lblUsername.Text = "Username:"
		'
		'txtRESTKey
		'
		Me.txtRESTKey.Location = New System.Drawing.Point(101, 125)
		Me.txtRESTKey.Name = "txtRESTKey"
		Me.txtRESTKey.Size = New System.Drawing.Size(274, 20)
		Me.txtRESTKey.TabIndex = 2
		'
		'txtPassword
		'
		Me.txtPassword.Location = New System.Drawing.Point(101, 86)
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
		Me.txtPassword.Size = New System.Drawing.Size(274, 20)
		Me.txtPassword.TabIndex = 1
		'
		'txtUsername
		'
		Me.txtUsername.Location = New System.Drawing.Point(101, 60)
		Me.txtUsername.Name = "txtUsername"
		Me.txtUsername.Size = New System.Drawing.Size(274, 20)
		Me.txtUsername.TabIndex = 0
		'
		'statusStripLogin
		'
		Me.statusStripLogin.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusLabelLogin})
		Me.statusStripLogin.Location = New System.Drawing.Point(0, 385)
		Me.statusStripLogin.Name = "statusStripLogin"
		Me.statusStripLogin.Size = New System.Drawing.Size(438, 22)
		Me.statusStripLogin.TabIndex = 4
		Me.statusStripLogin.Text = "statusStrip1"
		'
		'statusLabelLogin
		'
		Me.statusLabelLogin.Name = "statusLabelLogin"
		Me.statusLabelLogin.Size = New System.Drawing.Size(0, 17)
		'
		'buttonLogin
		'
		Me.buttonLogin.Location = New System.Drawing.Point(322, 346)
		Me.buttonLogin.Name = "buttonLogin"
		Me.buttonLogin.Size = New System.Drawing.Size(75, 23)
		Me.buttonLogin.TabIndex = 6
		Me.buttonLogin.Text = "Login"
		Me.buttonLogin.UseVisualStyleBackColor = true
		AddHandler Me.buttonLogin.Click, AddressOf Me.ButtonLoginClick
		'
		'checkSaveLoginDetails
		'
		Me.checkSaveLoginDetails.AutoSize = true
		Me.checkSaveLoginDetails.Location = New System.Drawing.Point(36, 350)
		Me.checkSaveLoginDetails.Name = "checkSaveLoginDetails"
		Me.checkSaveLoginDetails.Size = New System.Drawing.Size(157, 17)
		Me.checkSaveLoginDetails.TabIndex = 5
		Me.checkSaveLoginDetails.Text = "Remember These Settings?"
		Me.checkSaveLoginDetails.UseVisualStyleBackColor = true
		'
		'FormLogin
		'
		Me.AcceptButton = Me.buttonLogin
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(438, 407)
		Me.Controls.Add(Me.checkSaveLoginDetails)
		Me.Controls.Add(Me.buttonLogin)
		Me.Controls.Add(Me.statusStripLogin)
		Me.Controls.Add(Me.groupBoxLogin)
		Me.Controls.Add(Me.groupBoxURL)
		Me.Controls.Add(Me.lblLoginTitle)
		Me.Controls.Add(Me.pictureGalleryLogo)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormLogin"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Login"
		AddHandler Load, AddressOf Me.FormLoginLoad
		CType(Me.pictureGalleryLogo,System.ComponentModel.ISupportInitialize).EndInit
		Me.groupBoxURL.ResumeLayout(false)
		Me.groupBoxURL.PerformLayout
		Me.groupBoxLogin.ResumeLayout(false)
		Me.groupBoxLogin.PerformLayout
		Me.statusStripLogin.ResumeLayout(false)
		Me.statusStripLogin.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private checkSaveLoginDetails As System.Windows.Forms.CheckBox
	Private buttonLogin As System.Windows.Forms.Button
	Private statusLabelLogin As System.Windows.Forms.ToolStripStatusLabel
	Private statusStripLogin As System.Windows.Forms.StatusStrip
	Private lblLoginInfo As System.Windows.Forms.Label
	Private lblRestKey As System.Windows.Forms.Label
	Private lblUsername As System.Windows.Forms.Label
	Private lblPassword As System.Windows.Forms.Label
	Private txtUsername As System.Windows.Forms.TextBox
	Private txtPassword As System.Windows.Forms.TextBox
	Private txtRESTKey As System.Windows.Forms.TextBox
	Private lblURLExample As System.Windows.Forms.Label
	Private txtGalleryURL As System.Windows.Forms.TextBox
	Private groupBoxLogin As System.Windows.Forms.GroupBox
	Private groupBoxURL As System.Windows.Forms.GroupBox
	Private lblLoginTitle As System.Windows.Forms.Label
	Private pictureGalleryLogo As System.Windows.Forms.PictureBox
End Class
