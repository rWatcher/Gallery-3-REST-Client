'
' Created by SharpDevelop.
' User: Eric
' Date: 11/2/2010
' Time: 4:26 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class FormPreferences
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormPreferences))
		Me.labelPrefsHeading = New System.Windows.Forms.Label
		Me.labelG3URL = New System.Windows.Forms.Label
		Me.labelRESTKey = New System.Windows.Forms.Label
		Me.checkBoxImageCache = New System.Windows.Forms.CheckBox
		Me.textGallery3URL = New System.Windows.Forms.TextBox
		Me.textRESTKey = New System.Windows.Forms.TextBox
		Me.buttonSave = New System.Windows.Forms.Button
		Me.SuspendLayout
		'
		'labelPrefsHeading
		'
		Me.labelPrefsHeading.Anchor = System.Windows.Forms.AnchorStyles.Top
		Me.labelPrefsHeading.AutoSize = true
		Me.labelPrefsHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.labelPrefsHeading.Location = New System.Drawing.Point(85, 10)
		Me.labelPrefsHeading.Name = "labelPrefsHeading"
		Me.labelPrefsHeading.Size = New System.Drawing.Size(161, 20)
		Me.labelPrefsHeading.TabIndex = 0
		Me.labelPrefsHeading.Text = "Saved Preferences"
		'
		'labelG3URL
		'
		Me.labelG3URL.AutoSize = true
		Me.labelG3URL.Location = New System.Drawing.Point(12, 46)
		Me.labelG3URL.Name = "labelG3URL"
		Me.labelG3URL.Size = New System.Drawing.Size(76, 13)
		Me.labelG3URL.TabIndex = 1
		Me.labelG3URL.Text = "Gallery 3 URL:"
		'
		'labelRESTKey
		'
		Me.labelRESTKey.AutoSize = true
		Me.labelRESTKey.Location = New System.Drawing.Point(12, 73)
		Me.labelRESTKey.Name = "labelRESTKey"
		Me.labelRESTKey.Size = New System.Drawing.Size(80, 13)
		Me.labelRESTKey.TabIndex = 2
		Me.labelRESTKey.Text = "REST API Key:"
		'
		'checkBoxImageCache
		'
		Me.checkBoxImageCache.AutoSize = true
		Me.checkBoxImageCache.Location = New System.Drawing.Point(12, 96)
		Me.checkBoxImageCache.Name = "checkBoxImageCache"
		Me.checkBoxImageCache.Size = New System.Drawing.Size(164, 17)
		Me.checkBoxImageCache.TabIndex = 3
		Me.checkBoxImageCache.Text = "Empty Image Cache On Exit?"
		Me.checkBoxImageCache.UseVisualStyleBackColor = true
		'
		'textGallery3URL
		'
		Me.textGallery3URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textGallery3URL.Location = New System.Drawing.Point(94, 43)
		Me.textGallery3URL.Name = "textGallery3URL"
		Me.textGallery3URL.Size = New System.Drawing.Size(227, 20)
		Me.textGallery3URL.TabIndex = 4
		'
		'textRESTKey
		'
		Me.textRESTKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textRESTKey.Location = New System.Drawing.Point(94, 70)
		Me.textRESTKey.Name = "textRESTKey"
		Me.textRESTKey.Size = New System.Drawing.Size(227, 20)
		Me.textRESTKey.TabIndex = 5
		'
		'buttonSave
		'
		Me.buttonSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonSave.Location = New System.Drawing.Point(246, 123)
		Me.buttonSave.Name = "buttonSave"
		Me.buttonSave.Size = New System.Drawing.Size(75, 23)
		Me.buttonSave.TabIndex = 6
		Me.buttonSave.Text = "Save"
		Me.buttonSave.UseVisualStyleBackColor = true
		AddHandler Me.buttonSave.Click, AddressOf Me.ButtonSaveClick
		'
		'FormPreferences
		'
		Me.AcceptButton = Me.buttonSave
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(331, 158)
		Me.Controls.Add(Me.buttonSave)
		Me.Controls.Add(Me.textRESTKey)
		Me.Controls.Add(Me.textGallery3URL)
		Me.Controls.Add(Me.checkBoxImageCache)
		Me.Controls.Add(Me.labelRESTKey)
		Me.Controls.Add(Me.labelG3URL)
		Me.Controls.Add(Me.labelPrefsHeading)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "FormPreferences"
		Me.Text = "Preferences"
		AddHandler Load, AddressOf Me.FormPreferencesLoad
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private buttonSave As System.Windows.Forms.Button
	Private textRESTKey As System.Windows.Forms.TextBox
	Private textGallery3URL As System.Windows.Forms.TextBox
	Private checkBoxImageCache As System.Windows.Forms.CheckBox
	Private labelRESTKey As System.Windows.Forms.Label
	Private labelG3URL As System.Windows.Forms.Label
	Private labelPrefsHeading As System.Windows.Forms.Label
End Class
