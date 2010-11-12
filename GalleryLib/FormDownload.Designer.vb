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
Partial Class FormDownload
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDownload))
		Me.lblDownloadProgress = New System.Windows.Forms.Label
		Me.ProgressDownload = New System.Windows.Forms.ProgressBar
		Me.SuspendLayout
		'
		'lblDownloadProgress
		'
		Me.lblDownloadProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblDownloadProgress.Location = New System.Drawing.Point(12, 9)
		Me.lblDownloadProgress.Name = "lblDownloadProgress"
		Me.lblDownloadProgress.Size = New System.Drawing.Size(90, 23)
		Me.lblDownloadProgress.TabIndex = 0
		'
		'ProgressDownload
		'
		Me.ProgressDownload.Location = New System.Drawing.Point(108, 9)
		Me.ProgressDownload.Name = "ProgressDownload"
		Me.ProgressDownload.Size = New System.Drawing.Size(328, 23)
		Me.ProgressDownload.TabIndex = 1
		Me.ProgressDownload.Value = 50
		'
		'FormDownload
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(448, 43)
		Me.Controls.Add(Me.ProgressDownload)
		Me.Controls.Add(Me.lblDownloadProgress)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "FormDownload"
		Me.Text = "FormDownload"
		Me.ResumeLayout(false)
	End Sub
	Friend ProgressDownload As System.Windows.Forms.ProgressBar
	Friend lblDownloadProgress As System.Windows.Forms.Label
End Class
