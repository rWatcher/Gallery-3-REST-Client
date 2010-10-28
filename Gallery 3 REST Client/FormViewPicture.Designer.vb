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
Partial Class FormViewPicture
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormViewPicture))
		Me.PictureResize = New System.Windows.Forms.PictureBox
		CType(Me.PictureResize,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'PictureResize
		'
		Me.PictureResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.PictureResize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.PictureResize.Location = New System.Drawing.Point(3, 3)
		Me.PictureResize.Name = "PictureResize"
		Me.PictureResize.Size = New System.Drawing.Size(100, 50)
		Me.PictureResize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.PictureResize.TabIndex = 0
		Me.PictureResize.TabStop = false
		'
		'FormViewPicture
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = true
		Me.ClientSize = New System.Drawing.Size(106, 56)
		Me.Controls.Add(Me.PictureResize)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormViewPicture"
		Me.Text = "FormViewPicture"
		CType(Me.PictureResize,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Friend PictureResize As System.Windows.Forms.PictureBox
End Class
