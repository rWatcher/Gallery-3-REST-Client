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
Partial Class FormAlbumBrowser
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
		Me.components = New System.ComponentModel.Container
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAlbumBrowser))
		Me.menuMain = New System.Windows.Forms.MenuStrip
		Me.optionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.aboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.emptyImageCacheToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.fullscreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.albumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.compareToLocalFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.uploadFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.splitContainer1 = New System.Windows.Forms.SplitContainer
		Me.treeAlbums = New System.Windows.Forms.TreeView
		Me.listPictures = New System.Windows.Forms.ListView
		Me.ImageListThumbs = New System.Windows.Forms.ImageList(Me.components)
		Me.statusGallery = New System.Windows.Forms.StatusStrip
		Me.labelGalleryStatus = New System.Windows.Forms.ToolStripStatusLabel
		Me.menuMain.SuspendLayout
		Me.splitContainer1.Panel1.SuspendLayout
		Me.splitContainer1.Panel2.SuspendLayout
		Me.splitContainer1.SuspendLayout
		Me.statusGallery.SuspendLayout
		Me.SuspendLayout
		'
		'menuMain
		'
		Me.menuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.optionsToolStripMenuItem, Me.albumToolStripMenuItem})
		Me.menuMain.Location = New System.Drawing.Point(0, 0)
		Me.menuMain.Name = "menuMain"
		Me.menuMain.Size = New System.Drawing.Size(624, 24)
		Me.menuMain.TabIndex = 0
		Me.menuMain.Text = "menuStrip1"
		'
		'optionsToolStripMenuItem
		'
		Me.optionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.aboutToolStripMenuItem, Me.emptyImageCacheToolStripMenuItem, Me.fullscreenToolStripMenuItem})
		Me.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem"
		Me.optionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
		Me.optionsToolStripMenuItem.Text = "Options"
		'
		'aboutToolStripMenuItem
		'
		Me.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
		Me.aboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
		Me.aboutToolStripMenuItem.Text = "About"
		AddHandler Me.aboutToolStripMenuItem.Click, AddressOf Me.AboutToolStripMenuItemClick
		'
		'emptyImageCacheToolStripMenuItem
		'
		Me.emptyImageCacheToolStripMenuItem.Name = "emptyImageCacheToolStripMenuItem"
		Me.emptyImageCacheToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
		Me.emptyImageCacheToolStripMenuItem.Text = "Empty Image Cache"
		AddHandler Me.emptyImageCacheToolStripMenuItem.Click, AddressOf Me.EmptyImageCacheToolStripMenuItemClick
		'
		'fullscreenToolStripMenuItem
		'
		Me.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem"
		Me.fullscreenToolStripMenuItem.ShortcutKeyDisplayString = "F11"
		Me.fullscreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11
		Me.fullscreenToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
		Me.fullscreenToolStripMenuItem.Text = "Fullscreen"
		AddHandler Me.fullscreenToolStripMenuItem.Click, AddressOf Me.FullscreenToolStripMenuItemClick
		'
		'albumToolStripMenuItem
		'
		Me.albumToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.compareToLocalFolderToolStripMenuItem, Me.uploadFilesToolStripMenuItem})
		Me.albumToolStripMenuItem.Name = "albumToolStripMenuItem"
		Me.albumToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
		Me.albumToolStripMenuItem.Text = "Album"
		'
		'compareToLocalFolderToolStripMenuItem
		'
		Me.compareToLocalFolderToolStripMenuItem.Name = "compareToLocalFolderToolStripMenuItem"
		Me.compareToLocalFolderToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
		Me.compareToLocalFolderToolStripMenuItem.Text = "Compare To Local Folder"
		AddHandler Me.compareToLocalFolderToolStripMenuItem.Click, AddressOf Me.CompareToLocalFolderToolStripMenuItemClick
		'
		'uploadFilesToolStripMenuItem
		'
		Me.uploadFilesToolStripMenuItem.Name = "uploadFilesToolStripMenuItem"
		Me.uploadFilesToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
		Me.uploadFilesToolStripMenuItem.Text = "Upload Files"
		AddHandler Me.uploadFilesToolStripMenuItem.Click, AddressOf Me.UploadFilesToolStripMenuItemClick
		'
		'splitContainer1
		'
		Me.splitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.splitContainer1.Location = New System.Drawing.Point(0, 27)
		Me.splitContainer1.Name = "splitContainer1"
		'
		'splitContainer1.Panel1
		'
		Me.splitContainer1.Panel1.Controls.Add(Me.treeAlbums)
		'
		'splitContainer1.Panel2
		'
		Me.splitContainer1.Panel2.Controls.Add(Me.listPictures)
		Me.splitContainer1.Size = New System.Drawing.Size(624, 390)
		Me.splitContainer1.SplitterDistance = 208
		Me.splitContainer1.TabIndex = 2
		'
		'treeAlbums
		'
		Me.treeAlbums.Dock = System.Windows.Forms.DockStyle.Fill
		Me.treeAlbums.Location = New System.Drawing.Point(0, 0)
		Me.treeAlbums.Name = "treeAlbums"
		Me.treeAlbums.Size = New System.Drawing.Size(204, 386)
		Me.treeAlbums.TabIndex = 0
		AddHandler Me.treeAlbums.AfterSelect, AddressOf Me.TreeAlbumsAfterSelect
		'
		'listPictures
		'
		Me.listPictures.Dock = System.Windows.Forms.DockStyle.Fill
		Me.listPictures.LargeImageList = Me.ImageListThumbs
		Me.listPictures.Location = New System.Drawing.Point(0, 0)
		Me.listPictures.Name = "listPictures"
		Me.listPictures.Size = New System.Drawing.Size(408, 386)
		Me.listPictures.TabIndex = 0
		Me.listPictures.UseCompatibleStateImageBehavior = false
		AddHandler Me.listPictures.DoubleClick, AddressOf Me.ListPicturesSelectedDoubleClick
		'
		'ImageListThumbs
		'
		Me.ImageListThumbs.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
		Me.ImageListThumbs.ImageSize = New System.Drawing.Size(64, 64)
		Me.ImageListThumbs.TransparentColor = System.Drawing.Color.Transparent
		'
		'statusGallery
		'
		Me.statusGallery.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.labelGalleryStatus})
		Me.statusGallery.Location = New System.Drawing.Point(0, 420)
		Me.statusGallery.Name = "statusGallery"
		Me.statusGallery.Size = New System.Drawing.Size(624, 22)
		Me.statusGallery.TabIndex = 3
		Me.statusGallery.Text = "statusStrip1"
		'
		'labelGalleryStatus
		'
		Me.labelGalleryStatus.Name = "labelGalleryStatus"
		Me.labelGalleryStatus.Size = New System.Drawing.Size(118, 17)
		Me.labelGalleryStatus.Text = "toolStripStatusLabel1"
		'
		'FormAlbumBrowser
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(624, 442)
		Me.Controls.Add(Me.statusGallery)
		Me.Controls.Add(Me.splitContainer1)
		Me.Controls.Add(Me.menuMain)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MainMenuStrip = Me.menuMain
		Me.Name = "FormAlbumBrowser"
		Me.Text = "Browse Albums"
		AddHandler Load, AddressOf Me.FormAlbumBrowserLoad
		Me.menuMain.ResumeLayout(false)
		Me.menuMain.PerformLayout
		Me.splitContainer1.Panel1.ResumeLayout(false)
		Me.splitContainer1.Panel2.ResumeLayout(false)
		Me.splitContainer1.ResumeLayout(false)
		Me.statusGallery.ResumeLayout(false)
		Me.statusGallery.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private fullscreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private emptyImageCacheToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private aboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private uploadFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private compareToLocalFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private labelGalleryStatus As System.Windows.Forms.ToolStripStatusLabel
	Private statusGallery As System.Windows.Forms.StatusStrip
	Friend ImageListThumbs As System.Windows.Forms.ImageList
	Private listPictures As System.Windows.Forms.ListView
	Private splitContainer1 As System.Windows.Forms.SplitContainer
	Private treeAlbums As System.Windows.Forms.TreeView
	Private optionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private albumToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private menuMain As System.Windows.Forms.MenuStrip
End Class
