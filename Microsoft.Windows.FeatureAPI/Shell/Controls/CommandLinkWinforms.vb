'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Internal

Namespace Controls.WindowsForms
	''' <summary>
	''' Implements a CommandLink button that can be used in 
	''' WinForms user interfaces.
	''' </summary>    
	Public Class CommandLink
		Inherits Button
		''' <summary>
		''' Gets a System.Windows.Forms.CreateParams on the base class when 
		''' creating a window.
		''' </summary>        
		Protected Overrides ReadOnly Property CreateParams() As CreateParams

			Get
				' Add BS_COMMANDLINK style before control creation.
				Dim cp As CreateParams = MyBase.CreateParams

				cp.Style = AddCommandLinkStyle(cp.Style)

				Return cp
			End Get
		End Property

		' Let Windows handle the rendering.
		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		Public Sub New()
			CoreHelpers.ThrowIfNotVista()

			FlatStyle = FlatStyle.System
		End Sub

		' Add Design-Time Support.

		''' <summary>
		''' Increase default width.
		''' </summary>
		Protected Overrides ReadOnly Property DefaultSize() As System.Drawing.Size
			Get
				Return New System.Drawing.Size(180, 60)
			End Get
		End Property

		''' <summary>
		''' Specifies the supporting note text
		''' </summary>
		<Category("Appearance")> _
		<Description("Specifies the supporting note text.")> _
		<BrowsableAttribute(True)> _
		<DefaultValue("(Note Text)")> _
		Public Property NoteText() As String
			Get
				Return (GetNote(Me))
			End Get
			Set
				SetNote(Me, value)
			End Set
		End Property

		''' <summary>
		''' Enable shield icon to be set at design-time.
		''' </summary>
		<Category("Appearance")> _
		<Description("Indicates whether the button should be decorated with the security shield icon (Windows Vista only).")> _
		<BrowsableAttribute(True)> _
		<DefaultValue(False)> _
		Public Property UseElevationIcon() As Boolean
			Get
				Return (m_useElevationIcon)
			End Get
			Set
				m_useElevationIcon = value
				SetShieldIcon(Me, Me.m_useElevationIcon)
			End Set
		End Property
		Private m_useElevationIcon As Boolean


		#Region "Interop helpers"

		Private Shared Function AddCommandLinkStyle(style As Integer) As Integer
			' Only add BS_COMMANDLINK style on Windows Vista or above.
			' Otherwise, button creation will fail.
			If CoreHelpers.RunningOnVista Then
				style = style Or ShellNativeMethods.CommandLink
			End If

			Return style
		End Function

		Private Shared Function GetNote(Button As System.Windows.Forms.Button) As String
			Dim retVal As IntPtr = CoreNativeMethods.SendMessage(Button.Handle, ShellNativeMethods.GetNoteLength, IntPtr.Zero, IntPtr.Zero)

			' Add 1 for null terminator, to get the entire string back.
			Dim len As Integer = CInt(retVal) + 1
			Dim strBld As New StringBuilder(len)

			retVal = CoreNativeMethods.SendMessage(Button.Handle, ShellNativeMethods.GetNote, len, strBld)
			Return strBld.ToString()
		End Function

		Private Shared Sub SetNote(button As System.Windows.Forms.Button, text As String)
			' This call will be ignored on versions earlier than Windows Vista.
			CoreNativeMethods.SendMessage(button.Handle, ShellNativeMethods.SetNote, 0, text)
		End Sub

		Friend Shared Sub SetShieldIcon(Button As System.Windows.Forms.Button, Show As Boolean)
			Dim fRequired As New IntPtr(If(Show, 1, 0))
			CoreNativeMethods.SendMessage(Button.Handle, ShellNativeMethods.SetShield, IntPtr.Zero, fRequired)
		End Sub

		#End Region

		''' <summary>
		''' Indicates whether this feature is supported on the current platform.
		''' </summary>
		Public Shared ReadOnly Property IsPlatformSupported() As Boolean
			Get
				' We need Windows Vista onwards ...
				Return CoreHelpers.RunningOnVista
			End Get
		End Property
	End Class
End Namespace
