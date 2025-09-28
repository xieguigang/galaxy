' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows
Imports System.Windows.Forms
Imports Microsoft.Windows.Internal

Namespace Taskbar
	Friend NotInheritable Class TabbedThumbnailProxyWindow
		Inherits Form
		Implements IDisposable

		Friend Sub New(preview As TabbedThumbnail)
			TabbedThumbnail = preview
			Size = New System.Drawing.Size(1, 1)

			If Not String.IsNullOrEmpty(preview.Title) Then
				Text = preview.Title
			End If

			If preview.WindowsControl IsNot Nothing Then
				WindowsControl = preview.WindowsControl
			End If
		End Sub

		Friend Property TabbedThumbnail() As TabbedThumbnail
			Get
				Return m_TabbedThumbnail
			End Get
			Private Set
				m_TabbedThumbnail = Value
			End Set
		End Property
		Private m_TabbedThumbnail As TabbedThumbnail

		Friend Property WindowsControl() As UIElement
			Get
				Return m_WindowsControl
			End Get
			Private Set
				m_WindowsControl = Value
			End Set
		End Property
		Private m_WindowsControl As UIElement

		Friend ReadOnly Property WindowToTellTaskbarAbout() As IntPtr
			Get
				Return Me.Handle
			End Get
		End Property

		Protected Overrides Sub WndProc(ByRef m As Message)
			Dim handled As Boolean = False

			If Me.TabbedThumbnail IsNot Nothing Then
				handled = TaskbarWindowManager.DispatchMessage(m, Me.TabbedThumbnail.TaskbarWindow)
			End If

			' If it's a WM_Destroy message, then also forward it to the base class (our native window)
			If (m.Msg = CInt(WindowMessage.Destroy)) OrElse (m.Msg = CInt(WindowMessage.NCDestroy)) OrElse ((m.Msg = CInt(WindowMessage.SystemCommand)) AndAlso (CInt(m.WParam) = TabbedThumbnailNativeMethods.ScClose)) Then
				MyBase.WndProc(m)
			ElseIf Not handled Then
				MyBase.WndProc(m)
			End If
		End Sub

		#Region "IDisposable Members"

		''' <summary>
		''' 
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		''' <summary>
		''' Release the native objects.
		''' </summary>
		Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Protected Overrides Sub Dispose(disposing As Boolean)
			If disposing Then
				' Dispose managed resources
				If TabbedThumbnail IsNot Nothing Then
					TabbedThumbnail.Dispose()
				End If

				TabbedThumbnail = Nothing

				WindowsControl = Nothing
			End If

			MyBase.Dispose(disposing)
		End Sub

		#End Region
	End Class
End Namespace
