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
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Partial Class TestForm
	Inherits Form
    Private control As ChromeTabControl.ChromeTabControl

    Public Sub New()
		InitializeComponent()
	End Sub

	Private Sub Form1_Load(sender As Object, e As EventArgs)
		Me.DoubleBuffered = True

        ' Let's create a new tab control
        control = New ChromeTabControl.ChromeTabControl()

        ' Let's make the top bar transparent and look better
        ChromeTabControl.AeroFunctions.ChromifyWindow(Me, control)

		' Add a handler for when the new tab button is clicked
		AddHandler control.NewTabClicked, AddressOf control_NewTabClicked

		' Let's add it to the form
		control.Dock = DockStyle.Fill
		Me.Controls.Add(control)

		' Let's add 2 test pages to it
		control.TabPages.AddNoAnimate(New TestPage())
		control.TabPages.AddNoAnimate(New TestPage())

	End Sub

	Private Sub control_NewTabClicked(sender As Object, e As EventArgs)
		' Let's add in a new tab page
		control.TabPages.Add(New TestPage())
	End Sub


End Class
