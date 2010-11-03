'
' Created by SharpDevelop.
' User: Eric
' Date: 11/3/2010
' Time: 3:03 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class FormCreateAlbum
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCreateAlbum))
		Me.labelTitle = New System.Windows.Forms.Label
		Me.labelDescription = New System.Windows.Forms.Label
		Me.labelDirectoryName = New System.Windows.Forms.Label
		Me.labelInternetAddress = New System.Windows.Forms.Label
		Me.textTitle = New System.Windows.Forms.TextBox
		Me.textDescription = New System.Windows.Forms.TextBox
		Me.textDirectoryName = New System.Windows.Forms.TextBox
		Me.textInternetAddress = New System.Windows.Forms.TextBox
		Me.buttonCreate = New System.Windows.Forms.Button
		Me.SuspendLayout
		'
		'labelTitle
		'
		Me.labelTitle.AutoSize = true
		Me.labelTitle.Location = New System.Drawing.Point(12, 10)
		Me.labelTitle.Name = "labelTitle"
		Me.labelTitle.Size = New System.Drawing.Size(30, 13)
		Me.labelTitle.TabIndex = 0
		Me.labelTitle.Text = "Title:"
		'
		'labelDescription
		'
		Me.labelDescription.AutoSize = true
		Me.labelDescription.Location = New System.Drawing.Point(12, 35)
		Me.labelDescription.Name = "labelDescription"
		Me.labelDescription.Size = New System.Drawing.Size(63, 13)
		Me.labelDescription.TabIndex = 1
		Me.labelDescription.Text = "Description:"
		'
		'labelDirectoryName
		'
		Me.labelDirectoryName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.labelDirectoryName.AutoSize = true
		Me.labelDirectoryName.Location = New System.Drawing.Point(12, 178)
		Me.labelDirectoryName.Name = "labelDirectoryName"
		Me.labelDirectoryName.Size = New System.Drawing.Size(83, 13)
		Me.labelDirectoryName.TabIndex = 2
		Me.labelDirectoryName.Text = "Directory Name:"
		'
		'labelInternetAddress
		'
		Me.labelInternetAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.labelInternetAddress.AutoSize = true
		Me.labelInternetAddress.Location = New System.Drawing.Point(12, 202)
		Me.labelInternetAddress.Name = "labelInternetAddress"
		Me.labelInternetAddress.Size = New System.Drawing.Size(87, 13)
		Me.labelInternetAddress.TabIndex = 3
		Me.labelInternetAddress.Text = "Internet Address:"
		'
		'textTitle
		'
		Me.textTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textTitle.Location = New System.Drawing.Point(106, 9)
		Me.textTitle.Name = "textTitle"
		Me.textTitle.Size = New System.Drawing.Size(272, 20)
		Me.textTitle.TabIndex = 4
		AddHandler Me.textTitle.TextChanged, AddressOf Me.TextTitleTextChanged
		'
		'textDescription
		'
		Me.textDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textDescription.Location = New System.Drawing.Point(106, 32)
		Me.textDescription.Multiline = true
		Me.textDescription.Name = "textDescription"
		Me.textDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.textDescription.Size = New System.Drawing.Size(272, 138)
		Me.textDescription.TabIndex = 5
		'
		'textDirectoryName
		'
		Me.textDirectoryName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textDirectoryName.Location = New System.Drawing.Point(106, 175)
		Me.textDirectoryName.Name = "textDirectoryName"
		Me.textDirectoryName.Size = New System.Drawing.Size(272, 20)
		Me.textDirectoryName.TabIndex = 6
		'
		'textInternetAddress
		'
		Me.textInternetAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textInternetAddress.Location = New System.Drawing.Point(106, 199)
		Me.textInternetAddress.Name = "textInternetAddress"
		Me.textInternetAddress.Size = New System.Drawing.Size(272, 20)
		Me.textInternetAddress.TabIndex = 7
		'
		'buttonCreate
		'
		Me.buttonCreate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonCreate.Location = New System.Drawing.Point(278, 233)
		Me.buttonCreate.Name = "buttonCreate"
		Me.buttonCreate.Size = New System.Drawing.Size(75, 23)
		Me.buttonCreate.TabIndex = 8
		Me.buttonCreate.Text = "Add Album"
		Me.buttonCreate.UseVisualStyleBackColor = true
		AddHandler Me.buttonCreate.Click, AddressOf Me.ButtonCreateClick
		'
		'FormCreateAlbum
		'
		Me.AcceptButton = Me.buttonCreate
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(390, 268)
		Me.Controls.Add(Me.buttonCreate)
		Me.Controls.Add(Me.textInternetAddress)
		Me.Controls.Add(Me.textDirectoryName)
		Me.Controls.Add(Me.textDescription)
		Me.Controls.Add(Me.textTitle)
		Me.Controls.Add(Me.labelInternetAddress)
		Me.Controls.Add(Me.labelDirectoryName)
		Me.Controls.Add(Me.labelDescription)
		Me.Controls.Add(Me.labelTitle)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormCreateAlbum"
		Me.Text = "Add Album"
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private buttonCreate As System.Windows.Forms.Button
	Private textInternetAddress As System.Windows.Forms.TextBox
	Private textDirectoryName As System.Windows.Forms.TextBox
	Private textDescription As System.Windows.Forms.TextBox
	Private textTitle As System.Windows.Forms.TextBox
	Private labelInternetAddress As System.Windows.Forms.Label
	Private labelDirectoryName As System.Windows.Forms.Label
	Private labelDescription As System.Windows.Forms.Label
	Private labelTitle As System.Windows.Forms.Label
End Class
