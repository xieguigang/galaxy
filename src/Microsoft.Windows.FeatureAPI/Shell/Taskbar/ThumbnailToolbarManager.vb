'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Windows

Namespace Taskbar
	''' <summary>
	''' Thumbnail toolbar manager class for adding a thumbnail toolbar with a specified set of buttons 
	''' to the thumbnail image of a window in a taskbar button flyout.
	''' </summary>
	Public Class ThumbnailToolBarManager
				' Hide the public constructor so users can't create an instance of this class.
		Friend Sub New()
		End Sub

		''' <summary>
		''' Adds thumbnail toolbar for the specified window.
		''' </summary>
		''' <param name="windowHandle">Window handle for which the thumbnail toolbar buttons need to be added</param>
		''' <param name="buttons">Thumbnail buttons for the window's thumbnail toolbar</param>
		''' <exception cref="System.ArgumentException">If the number of buttons exceed the maximum allowed capacity (7).</exception>
		''' <exception cref="System.ArgumentException">If the Window Handle passed in invalid</exception>
		''' <remarks>After a toolbar has been added to a thumbnail, buttons can be altered only through various 
		''' properties on the <see cref="T:Taskbar.ThumbnailToolBarButton"/>. While individual buttons cannot be added or removed, 
		''' they can be shown and hidden through <see cref="P:Taskbar.ThumbnailToolBarButton.Visible"/> as needed. 
		''' The toolbar itself cannot be removed without re-creating the window itself.
		''' </remarks>
		Public Sub AddButtons(windowHandle As IntPtr, ParamArray buttons As ThumbnailToolBarButton())
			If windowHandle = IntPtr.Zero Then
				Throw New ArgumentException(GlobalLocalizedMessages.ThumbnailManagerInvalidHandle, "windowHandle")
			End If
			VerifyButtons(buttons)

			' Add the buttons to our window manager, which will also create a proxy window
			TaskbarWindowManager.AddThumbnailButtons(windowHandle, buttons)
		End Sub

		''' <summary>
		''' Adds thumbnail toolbar for the specified WPF Control.
		''' </summary>
		''' <param name="control">WPF Control for which the thumbnail toolbar buttons need to be added</param>
		''' <param name="buttons">Thumbnail buttons for the window's thumbnail toolbar</param>
		''' <exception cref="System.ArgumentException">If the number of buttons exceed the maximum allowed capacity (7).</exception>
		''' <exception cref="System.ArgumentNullException">If the control passed in null</exception>
		''' <remarks>After a toolbar has been added to a thumbnail, buttons can be altered only through various 
		''' properties on the ThumbnailToolBarButton. While individual buttons cannot be added or removed, 
		''' they can be shown and hidden through ThumbnailToolBarButton.Visible as needed. 
		''' The toolbar itself cannot be removed without re-creating the window itself.
		''' </remarks>
		Public Sub AddButtons(control As UIElement, ParamArray buttons As ThumbnailToolBarButton())
			If control Is Nothing Then
				Throw New ArgumentNullException("control")
			End If
			VerifyButtons(buttons)

			' Add the buttons to our window manager, which will also create a proxy window
			TaskbarWindowManager.AddThumbnailButtons(control, buttons)
		End Sub

		Private Shared Sub VerifyButtons(ParamArray buttons As ThumbnailToolBarButton())
			If buttons IsNot Nothing AndAlso buttons.Length = 0 Then
				Throw New ArgumentException(GlobalLocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons")
			End If
			If buttons.Length > 7 Then
				Throw New ArgumentException(GlobalLocalizedMessages.ThumbnailToolbarManagerMaxButtons, "buttons")
			End If
		End Sub
	End Class
End Namespace
