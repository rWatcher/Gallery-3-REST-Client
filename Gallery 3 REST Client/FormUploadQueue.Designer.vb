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
Partial Class FormUploadQueue
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormUploadQueue))
		Me.lblUploadTo = New System.Windows.Forms.Label
		Me.textUploadDestination = New System.Windows.Forms.TextBox
		Me.listUploadQueue = New System.Windows.Forms.ListView
		Me.columnFileName = New System.Windows.Forms.ColumnHeader
		Me.columnUploadStatus = New System.Windows.Forms.ColumnHeader
		Me.buttonAddToQueue = New System.Windows.Forms.Button
		Me.buttonUpload = New System.Windows.Forms.Button
		Me.progressUpload = New System.Windows.Forms.ProgressBar
		Me.SuspendLayout
		'
		'lblUploadTo
		'
		Me.lblUploadTo.AutoSize = true
		Me.lblUploadTo.Location = New System.Drawing.Point(12, 8)
		Me.lblUploadTo.Name = "lblUploadTo"
		Me.lblUploadTo.Size = New System.Drawing.Size(84, 13)
		Me.lblUploadTo.TabIndex = 0
		Me.lblUploadTo.Text = "Upload Files To:"
		'
		'textUploadDestination
		'
		Me.textUploadDestination.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.textUploadDestination.Location = New System.Drawing.Point(105, 8)
		Me.textUploadDestination.Multiline = true
		Me.textUploadDestination.Name = "textUploadDestination"
		Me.textUploadDestination.ReadOnly = true
		Me.textUploadDestination.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.textUploadDestination.Size = New System.Drawing.Size(267, 40)
		Me.textUploadDestination.TabIndex = 1
		'
		'listUploadQueue
		'
		Me.listUploadQueue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.listUploadQueue.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnFileName, Me.columnUploadStatus})
		Me.listUploadQueue.Location = New System.Drawing.Point(12, 54)
		Me.listUploadQueue.Name = "listUploadQueue"
		Me.listUploadQueue.Size = New System.Drawing.Size(360, 184)
		Me.listUploadQueue.TabIndex = 2
		Me.listUploadQueue.UseCompatibleStateImageBehavior = false
		Me.listUploadQueue.View = System.Windows.Forms.View.Details
		'
		'columnFileName
		'
		Me.columnFileName.Text = "File"
		Me.columnFileName.Width = 226
		'
		'columnUploadStatus
		'
		Me.columnUploadStatus.Text = "Status"
		Me.columnUploadStatus.Width = 106
		'
		'buttonAddToQueue
		'
		Me.buttonAddToQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonAddToQueue.Location = New System.Drawing.Point(200, 277)
		Me.buttonAddToQueue.Name = "buttonAddToQueue"
		Me.buttonAddToQueue.Size = New System.Drawing.Size(75, 23)
		Me.buttonAddToQueue.TabIndex = 3
		Me.buttonAddToQueue.Text = "Add Files..."
		Me.buttonAddToQueue.UseVisualStyleBackColor = true
		AddHandler Me.buttonAddToQueue.Click, AddressOf Me.ButtonAddToQueueClick
		'
		'buttonUpload
		'
		Me.buttonUpload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonUpload.Location = New System.Drawing.Point(281, 277)
		Me.buttonUpload.Name = "buttonUpload"
		Me.buttonUpload.Size = New System.Drawing.Size(75, 23)
		Me.buttonUpload.TabIndex = 4
		Me.buttonUpload.Text = "Upload"
		Me.buttonUpload.UseVisualStyleBackColor = true
		AddHandler Me.buttonUpload.Click, AddressOf Me.ButtonUploadClick
		'
		'progressUpload
		'
		Me.progressUpload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.progressUpload.Location = New System.Drawing.Point(12, 244)
		Me.progressUpload.Name = "progressUpload"
		Me.progressUpload.Size = New System.Drawing.Size(360, 23)
		Me.progressUpload.TabIndex = 5
		'
		'FormUploadQueue
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(384, 312)
		Me.Controls.Add(Me.progressUpload)
		Me.Controls.Add(Me.buttonUpload)
		Me.Controls.Add(Me.buttonAddToQueue)
		Me.Controls.Add(Me.listUploadQueue)
		Me.Controls.Add(Me.textUploadDestination)
		Me.Controls.Add(Me.lblUploadTo)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "FormUploadQueue"
		Me.Text = "Upload Files"
		AddHandler Load, AddressOf Me.FormUploadQueueLoad
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private progressUpload As System.Windows.Forms.ProgressBar
	Private buttonUpload As System.Windows.Forms.Button
	Private buttonAddToQueue As System.Windows.Forms.Button
	Private columnUploadStatus As System.Windows.Forms.ColumnHeader
	Private columnFileName As System.Windows.Forms.ColumnHeader
	Private listUploadQueue As System.Windows.Forms.ListView
	Friend textUploadDestination As System.Windows.Forms.TextBox
	Private lblUploadTo As System.Windows.Forms.Label
End Class
