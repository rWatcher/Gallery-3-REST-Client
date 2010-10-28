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
Partial Class FormAbout
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAbout))
		Me.richTextBoxLicense = New System.Windows.Forms.RichTextBox
		Me.button1 = New System.Windows.Forms.Button
		Me.SuspendLayout
		'
		'richTextBoxLicense
		'
		Me.richTextBoxLicense.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.richTextBoxLicense.BackColor = System.Drawing.SystemColors.InactiveBorder
		Me.richTextBoxLicense.Location = New System.Drawing.Point(12, 12)
		Me.richTextBoxLicense.Name = "richTextBoxLicense"
		Me.richTextBoxLicense.ReadOnly = true
		Me.richTextBoxLicense.Size = New System.Drawing.Size(406, 221)
		Me.richTextBoxLicense.TabIndex = 0
		Me.richTextBoxLicense.Text = resources.GetString("richTextBoxLicense.Text")
		'
		'button1
		'
		Me.button1.Location = New System.Drawing.Point(178, 239)
		Me.button1.Name = "button1"
		Me.button1.Size = New System.Drawing.Size(75, 23)
		Me.button1.TabIndex = 1
		Me.button1.Text = "Okay"
		Me.button1.UseVisualStyleBackColor = true
		AddHandler Me.button1.Click, AddressOf Me.Button1Click
		'
		'FormAbout
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(430, 272)
		Me.Controls.Add(Me.button1)
		Me.Controls.Add(Me.richTextBoxLicense)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormAbout"
		Me.Text = "About"
		Me.ResumeLayout(false)
	End Sub
	Private button1 As System.Windows.Forms.Button
	Private richTextBoxLicense As System.Windows.Forms.RichTextBox
End Class
