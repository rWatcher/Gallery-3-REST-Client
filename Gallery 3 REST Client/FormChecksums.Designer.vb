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
Partial Class FormChecksums
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormChecksums))
		Me.lblLocalFolder = New System.Windows.Forms.Label
		Me.lblRemoteAlbum = New System.Windows.Forms.Label
		Me.txtLocalFolder = New System.Windows.Forms.TextBox
		Me.txtRemoteAlbum = New System.Windows.Forms.TextBox
		Me.listFiles = New System.Windows.Forms.ListView
		Me.columnLocalFile = New System.Windows.Forms.ColumnHeader
		Me.columnRemoteFile = New System.Windows.Forms.ColumnHeader
		Me.columnLocalChecksum = New System.Windows.Forms.ColumnHeader
		Me.columnRemoteChecksum = New System.Windows.Forms.ColumnHeader
		Me.statusStripCompareWindow = New System.Windows.Forms.StatusStrip
		Me.statusCompare = New System.Windows.Forms.ToolStripStatusLabel
		Me.buttonRecheck = New System.Windows.Forms.Button
		Me.statusStripCompareWindow.SuspendLayout
		Me.SuspendLayout
		'
		'lblLocalFolder
		'
		Me.lblLocalFolder.AutoSize = true
		Me.lblLocalFolder.Location = New System.Drawing.Point(12, 9)
		Me.lblLocalFolder.Name = "lblLocalFolder"
		Me.lblLocalFolder.Size = New System.Drawing.Size(39, 13)
		Me.lblLocalFolder.TabIndex = 0
		Me.lblLocalFolder.Text = "Folder:"
		'
		'lblRemoteAlbum
		'
		Me.lblRemoteAlbum.AutoSize = true
		Me.lblRemoteAlbum.Location = New System.Drawing.Point(12, 57)
		Me.lblRemoteAlbum.Name = "lblRemoteAlbum"
		Me.lblRemoteAlbum.Size = New System.Drawing.Size(39, 13)
		Me.lblRemoteAlbum.TabIndex = 1
		Me.lblRemoteAlbum.Text = "Album:"
		'
		'txtLocalFolder
		'
		Me.txtLocalFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.txtLocalFolder.Location = New System.Drawing.Point(74, 9)
		Me.txtLocalFolder.Multiline = true
		Me.txtLocalFolder.Name = "txtLocalFolder"
		Me.txtLocalFolder.ReadOnly = true
		Me.txtLocalFolder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtLocalFolder.Size = New System.Drawing.Size(360, 42)
		Me.txtLocalFolder.TabIndex = 2
		'
		'txtRemoteAlbum
		'
		Me.txtRemoteAlbum.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.txtRemoteAlbum.Location = New System.Drawing.Point(74, 57)
		Me.txtRemoteAlbum.Multiline = true
		Me.txtRemoteAlbum.Name = "txtRemoteAlbum"
		Me.txtRemoteAlbum.ReadOnly = true
		Me.txtRemoteAlbum.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtRemoteAlbum.Size = New System.Drawing.Size(360, 42)
		Me.txtRemoteAlbum.TabIndex = 3
		'
		'listFiles
		'
		Me.listFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.listFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnLocalFile, Me.columnRemoteFile, Me.columnLocalChecksum, Me.columnRemoteChecksum})
		Me.listFiles.Location = New System.Drawing.Point(12, 105)
		Me.listFiles.Name = "listFiles"
		Me.listFiles.Size = New System.Drawing.Size(422, 179)
		Me.listFiles.TabIndex = 4
		Me.listFiles.UseCompatibleStateImageBehavior = false
		Me.listFiles.View = System.Windows.Forms.View.Details
		'
		'columnLocalFile
		'
		Me.columnLocalFile.Text = "Local File"
		Me.columnLocalFile.Width = 94
		'
		'columnRemoteFile
		'
		Me.columnRemoteFile.Text = "Remote File"
		Me.columnRemoteFile.Width = 100
		'
		'columnLocalChecksum
		'
		Me.columnLocalChecksum.Text = "Local Checksum"
		Me.columnLocalChecksum.Width = 92
		'
		'columnRemoteChecksum
		'
		Me.columnRemoteChecksum.Text = "Remote Checksum"
		Me.columnRemoteChecksum.Width = 102
		'
		'statusStripCompareWindow
		'
		Me.statusStripCompareWindow.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusCompare})
		Me.statusStripCompareWindow.Location = New System.Drawing.Point(0, 316)
		Me.statusStripCompareWindow.Name = "statusStripCompareWindow"
		Me.statusStripCompareWindow.Size = New System.Drawing.Size(446, 22)
		Me.statusStripCompareWindow.TabIndex = 5
		Me.statusStripCompareWindow.Text = "statusStrip1"
		'
		'statusCompare
		'
		Me.statusCompare.Name = "statusCompare"
		Me.statusCompare.Size = New System.Drawing.Size(118, 17)
		Me.statusCompare.Text = "toolStripStatusLabel1"
		'
		'buttonRecheck
		'
		Me.buttonRecheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.buttonRecheck.Enabled = false
		Me.buttonRecheck.Location = New System.Drawing.Point(345, 290)
		Me.buttonRecheck.Name = "buttonRecheck"
		Me.buttonRecheck.Size = New System.Drawing.Size(89, 23)
		Me.buttonRecheck.TabIndex = 6
		Me.buttonRecheck.Text = "Recheck Errors"
		Me.buttonRecheck.UseVisualStyleBackColor = true
		AddHandler Me.buttonRecheck.Click, AddressOf Me.ButtonRecheckClick
		'
		'FormChecksums
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(446, 338)
		Me.Controls.Add(Me.buttonRecheck)
		Me.Controls.Add(Me.statusStripCompareWindow)
		Me.Controls.Add(Me.listFiles)
		Me.Controls.Add(Me.txtRemoteAlbum)
		Me.Controls.Add(Me.txtLocalFolder)
		Me.Controls.Add(Me.lblRemoteAlbum)
		Me.Controls.Add(Me.lblLocalFolder)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "FormChecksums"
		Me.Text = "Compare Files"
		Me.statusStripCompareWindow.ResumeLayout(false)
		Me.statusStripCompareWindow.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Friend buttonRecheck As System.Windows.Forms.Button
	Friend statusCompare As System.Windows.Forms.ToolStripStatusLabel
	Private statusStripCompareWindow As System.Windows.Forms.StatusStrip
	Private columnRemoteChecksum As System.Windows.Forms.ColumnHeader
	Private columnLocalChecksum As System.Windows.Forms.ColumnHeader
	Private columnRemoteFile As System.Windows.Forms.ColumnHeader
	Private columnLocalFile As System.Windows.Forms.ColumnHeader
	Private listFiles As System.Windows.Forms.ListView
	Friend txtRemoteAlbum As System.Windows.Forms.TextBox
	Friend txtLocalFolder As System.Windows.Forms.TextBox
	Private lblRemoteAlbum As System.Windows.Forms.Label
	Private lblLocalFolder As System.Windows.Forms.Label
End Class
