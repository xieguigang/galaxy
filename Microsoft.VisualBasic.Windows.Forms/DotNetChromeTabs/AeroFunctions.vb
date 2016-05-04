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
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Namespace ChromeTabControl

    ''' <summary>
    ''' Functions designed to extend the glass of aero into the client.
    ''' Allowing the Chrome look feel more genuine.
    ''' </summary>
    Public Module AeroFunctions

        <DllImport("dwmapi.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Auto)>
        Public Function DwmExtendFrameIntoClientArea(hWnd As IntPtr, ByRef pMarinset As Margins) As Integer
        End Function

        <DllImport("dwmapi.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Auto)>
        Public Sub DwmIsCompositionEnabled(ByRef enabledptr As Integer)
        End Sub

        ''' <summary>
        ''' Determines whether or not Aero is enabled on the system.
        ''' </summary>
        ''' <returns>True if enabled.</returns>
        Public Function IsWindowsAeroEnabled() As Boolean
            Dim osInfo As System.OperatingSystem = System.Environment.OSVersion
            If osInfo.Version.Major < 6 Then
                Return False
            End If
            Dim compositionEnabled As Integer = 0
            DwmIsCompositionEnabled(compositionEnabled)
            If compositionEnabled > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' The margins defined for the API calls.
        ''' </summary>
        Public Structure Margins
            Public cxLeftWidth As Integer
            Public cxRightWidth As Integer
            Public cyTopHeight As Integer
            Public cyBottomHeight As Integer
        End Structure

        ''' <summary>
        ''' Makes a form have aero glass extend on it.
        ''' </summary>
        ''' <param name="hWnd">The handle of the form.</param>
        ''' <param name="leftMargin">The left margin.</param>
        ''' <param name="rightMargin">The right margin.</param>
        ''' <param name="topMargin">The top margin.</param>
        ''' <param name="bottomMargin">The bottom margin.</param>
        Public Function MakeWindowGlass(hWnd As IntPtr, leftMargin As Integer, rightMargin As Integer, topMargin As Integer, bottomMargin As Integer) As Boolean
            If Not IsWindowsAeroEnabled() Then
                Return False
            End If
            Dim margins As New Margins()
            If True Then
                margins.cxLeftWidth = leftMargin
                margins.cxRightWidth = rightMargin
                margins.cyTopHeight = topMargin
                margins.cyBottomHeight = bottomMargin
            End If
            DwmExtendFrameIntoClientArea(hWnd, margins)
            Return True
        End Function

        ''' <summary>
        ''' Gives a form a transparent tab control strip if the tab control is docked
        ''' as fill.
        ''' </summary>
        ''' <param name="form">The form to prepare.</param>
        ''' <returns>True if successful</returns>
        Public Sub ChromifyWindow(form As Form, tabcontrol As ChromeTabControl)
            If MakeWindowGlass(form.Handle, 5, 5, 40, 5) Then
                tabcontrol.BackColor = Color.Transparent
            Else
                tabcontrol.BackColor = Color.Gray
            End If
        End Sub
    End Module
End Namespace