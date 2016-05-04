'
'    ChromeTabControl is a .Net control that mimics Google Chrome's tab bar.
'    Copyright (C) 2013  Brandon Francis
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'


Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms


''' <summary>
''' Example tab page.
''' </summary>
Public Partial Class TestPage
	Inherits ChromeTabControl.TabPage
	Public Sub New()
		InitializeComponent()
	End Sub

	Private Sub TestPage_Load(sender As Object, e As EventArgs)
	End Sub

	Friend Overrides Function NewInstanceAttempted(newInstance As ChromeTabControl.TabPage) As Boolean
		Return True
	End Function

End Class
