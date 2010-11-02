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
Public Partial Class FormPreferences
	
	' Location of the data folder.
	Dim strDataFolder As String = ""
	
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub FormPreferencesLoad(sender As Object, e As EventArgs)
		' Load existing preferences from the config file when the form loads.
		
		' Figure out if the data folder should be in the app directory or the application data directory.
		If System.IO.Directory.Exists(Application.StartupPath & "\data") Then
			strDataFolder = Application.StartupPath & "\data"
		Else
			strDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Gallery3Client"
		End If
		
		' Load preferences from the config file.
        Dim results() As DataRow
        Dim g3Prefs As New DataSet
        g3Prefs.ReadXmlSchema(Application.StartupPath & "\config.xsd")
        g3Prefs.ReadXml(strDataFolder & "\config.xml")
        results = g3Prefs.Tables(0).Select("ConfigName = 'G3URL'")
        textGallery3URL.Text = results(0).Item(1).ToString
        results = g3Prefs.Tables(0).Select("ConfigName = 'G3RESTKey'")
        textRESTKey.Text = results(0).Item(1).ToString		
        results = g3Prefs.Tables(0).Select("ConfigName = 'EmptyCache'")
        checkBoxImageCache.Checked = results(0).Item(1).ToString		
	End Sub ' END FormPreferencesLoad
	
	Sub ButtonSaveClick(sender As Object, e As EventArgs)
		' Save new settings to the config file before closing the window.
		
        ' Load the config file.
        Dim results() As DataRow
        Dim g3Prefs As New DataSet
        g3Prefs.ReadXmlSchema(Application.StartupPath & "\config.xsd")
        g3Prefs.ReadXml(strDataFolder & "\config.xml")
        
        ' Store the new settings.
        results = g3Prefs.Tables(0).Select("ConfigName = 'G3URL'")
        results(0).Item(1) = textGallery3URL.Text
        results = g3Prefs.Tables(0).Select("ConfigName = 'G3RESTKey'")
        results(0).Item(1) = textRESTKey.Text
        results = g3Prefs.Tables(0).Select("ConfigName = 'EmptyCache'")
        results(0).Item(1) = checkBoxImageCache.Checked
        
        ' Save the updated config data to the file.
        g3Prefs.WriteXml(strDataFolder & "\config.xml")
        
        ' Close this window.
        Me.Close()
	End Sub
End Class
