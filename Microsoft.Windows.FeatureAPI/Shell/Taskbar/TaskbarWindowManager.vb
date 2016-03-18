' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports System.Windows.Media
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Taskbar
	Friend NotInheritable Class TaskbarWindowManager
		Private Sub New()
		End Sub
		Friend Shared _taskbarWindowList As New List(Of TaskbarWindow)()

		Private Shared _buttonsAdded As Boolean

		Friend Shared Sub AddThumbnailButtons(userWindowHandle As IntPtr, ParamArray buttons As ThumbnailToolBarButton())
			' Try to get an existing taskbar window for this user windowhandle            
			Dim taskbarWindow As TaskbarWindow = GetTaskbarWindow(userWindowHandle, TaskbarProxyWindowType.ThumbnailToolbar)
			Dim temp As TaskbarWindow = Nothing
			Try
                AddThumbnailButtons(If(taskbarWindow, temp.DirectCopy(New TaskbarWindow(userWindowHandle, buttons))), taskbarWindow Is Nothing, buttons)
            Catch
				If temp IsNot Nothing Then
					temp.Dispose()
				End If
				Throw
			End Try
		End Sub

		Friend Shared Sub AddThumbnailButtons(control As System.Windows.UIElement, ParamArray buttons As ThumbnailToolBarButton())
			' Try to get an existing taskbar window for this user uielement            
			Dim taskbarWindow As TaskbarWindow = GetTaskbarWindow(control, TaskbarProxyWindowType.ThumbnailToolbar)
			Dim temp As TaskbarWindow = Nothing
			Try
                AddThumbnailButtons(If(taskbarWindow, temp.DirectCopy(New TaskbarWindow(control, buttons))), taskbarWindow Is Nothing, buttons)
            Catch
				If temp IsNot Nothing Then
					temp.Dispose()
				End If
				Throw
			End Try
		End Sub

		Private Shared Sub AddThumbnailButtons(taskbarWindow As TaskbarWindow, add As Boolean, ParamArray buttons As ThumbnailToolBarButton())
			If add Then
				_taskbarWindowList.Add(taskbarWindow)
			ElseIf taskbarWindow.ThumbnailButtons Is Nothing Then
				taskbarWindow.ThumbnailButtons = buttons
			Else
				' We already have buttons assigned
				Throw New InvalidOperationException(LocalizedMessages.TaskbarWindowManagerButtonsAlreadyAdded)
			End If
		End Sub

		Friend Shared Sub AddTabbedThumbnail(preview As TabbedThumbnail)
			' Create a TOP-LEVEL proxy window for the user's source window/control
			Dim taskbarWindow As TaskbarWindow = Nothing

			' get the TaskbarWindow for UIElement/WindowHandle respectfully.
			If preview.WindowHandle = IntPtr.Zero Then
				taskbarWindow = GetTaskbarWindow(preview.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail)
			Else
				taskbarWindow = GetTaskbarWindow(preview.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail)
			End If

			'create taskbar, or set its TabbedThumbnail
			If taskbarWindow Is Nothing Then
				taskbarWindow = New TaskbarWindow(preview)
				_taskbarWindowList.Add(taskbarWindow)
			ElseIf taskbarWindow.TabbedThumbnail Is Nothing Then
				taskbarWindow.TabbedThumbnail = preview
			End If

			' Listen for Title changes
			AddHandler preview.TitleChanged, New EventHandler(AddressOf thumbnailPreview_TitleChanged)
			AddHandler preview.TooltipChanged, New EventHandler(AddressOf thumbnailPreview_TooltipChanged)

			' Get/Set properties for proxy window
			Dim windowHandle As IntPtr = taskbarWindow.WindowToTellTaskbarAbout

			' Register this new tab and set it as being active.
			TaskbarList.Instance.RegisterTab(windowHandle, preview.ParentWindowHandle)
			TaskbarList.Instance.SetTabOrder(windowHandle, IntPtr.Zero)
			TaskbarList.Instance.SetTabActive(windowHandle, preview.ParentWindowHandle, 0)

			' We need to make sure we can set these properties even when running with admin 
			TabbedThumbnailNativeMethods.ChangeWindowMessageFilter(TabbedThumbnailNativeMethods.WmDwmSendIconicThumbnail, TabbedThumbnailNativeMethods.MsgfltAdd)

			TabbedThumbnailNativeMethods.ChangeWindowMessageFilter(TabbedThumbnailNativeMethods.WmDwmSendIconicLivePreviewBitmap, TabbedThumbnailNativeMethods.MsgfltAdd)

			' BUG: There should be somewhere to disable CustomWindowPreview. I didn't find it.
			TabbedThumbnailNativeMethods.EnableCustomWindowPreview(windowHandle, True)

			' Make sure we use the initial title set by the user
			' Trigger a "fake" title changed event, so the title is set on the taskbar thumbnail.
			' Empty/null title will be ignored.
			thumbnailPreview_TitleChanged(preview, EventArgs.Empty)
			thumbnailPreview_TooltipChanged(preview, EventArgs.Empty)

			' Indicate to the preview that we've added it on the taskbar
			preview.AddedToTaskbar = True
		End Sub

		Friend Shared Function GetTaskbarWindow(windowsControl As System.Windows.UIElement, taskbarProxyWindowType__1 As TaskbarProxyWindowType) As TaskbarWindow
			If windowsControl Is Nothing Then
				Throw New ArgumentNullException("windowsControl")
			End If

			Dim toReturn As TaskbarWindow = _taskbarWindowList.FirstOrDefault(Function(window) 
			Return (window.TabbedThumbnail IsNot Nothing AndAlso window.TabbedThumbnail.WindowsControl Is windowsControl) OrElse (window.ThumbnailToolbarProxyWindow IsNot Nothing AndAlso window.ThumbnailToolbarProxyWindow.WindowsControl Is windowsControl)

End Function)

			If toReturn IsNot Nothing Then
				If taskbarProxyWindowType__1 = TaskbarProxyWindowType.ThumbnailToolbar Then
					toReturn.EnableThumbnailToolbars = True
				ElseIf taskbarProxyWindowType__1 = TaskbarProxyWindowType.TabbedThumbnail Then
					toReturn.EnableTabbedThumbnails = True
				End If
			End If

			Return toReturn
		End Function

		Friend Shared Function GetTaskbarWindow(userWindowHandle As IntPtr, taskbarProxyWindowType__1 As TaskbarProxyWindowType) As TaskbarWindow
			If userWindowHandle = IntPtr.Zero Then
				Throw New ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "userWindowHandle")
			End If

			Dim toReturn As TaskbarWindow = _taskbarWindowList.FirstOrDefault(Function(window) window.UserWindowHandle = userWindowHandle)

			' If its not in the list, return null so it can be added.            
			If toReturn IsNot Nothing Then
				If taskbarProxyWindowType__1 = TaskbarProxyWindowType.ThumbnailToolbar Then
					toReturn.EnableThumbnailToolbars = True
				ElseIf taskbarProxyWindowType__1 = TaskbarProxyWindowType.TabbedThumbnail Then
					toReturn.EnableTabbedThumbnails = True
				End If
			End If

			Return toReturn
		End Function

		#Region "Message dispatch methods"
		Private Shared Sub DispatchTaskbarButtonMessages(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow)
			If m.Msg = CInt(TaskbarNativeMethods.WmTaskbarButtonCreated) Then
				AddButtons(taskbarWindow)
			Else
				If Not _buttonsAdded Then
					AddButtons(taskbarWindow)
				End If

				If m.Msg = TaskbarNativeMethods.WmCommand AndAlso CoreNativeMethods.GetHiWord(m.WParam.ToInt64(), 16) = ThumbButton.Clicked Then
					Dim buttonId As Integer = CoreNativeMethods.GetLoWord(m.WParam.ToInt64())

                    Dim buttonsFound = From b In taskbarWindow.ThumbnailButtons Where b.Id = buttonId Select b

                    For Each button As ThumbnailToolBarButton In buttonsFound
						button.FireClick(taskbarWindow)
					Next
				End If
			End If
		End Sub

		Private Shared Function DispatchActivateMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(WindowMessage.Activate) Then
				' Raise the event
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailActivated()
				SetActiveTab(taskbarWindow)
				Return True
			End If
			Return False
		End Function

		Private Shared Function DispatchSendIconThumbnailMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(TaskbarNativeMethods.WmDwmSendIconThumbnail) Then
				Dim width As Integer = CInt(CLng(m.LParam) >> 16)
				Dim height As Integer = CInt(CLng(m.LParam) And (&Hffff))
				Dim requestedSize As New Size(width, height)

				' Fire an event to let the user update their bitmap
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailBitmapRequested()

				Dim hBitmap As IntPtr = IntPtr.Zero

				' Default size for the thumbnail
				Dim realWindowSize As New Size(200, 200)

				' Get the size of teh control or UIElement
				If taskbarWindow.TabbedThumbnail.WindowHandle <> IntPtr.Zero Then
					TabbedThumbnailNativeMethods.GetClientSize(taskbarWindow.TabbedThumbnail.WindowHandle, realWindowSize)
				ElseIf taskbarWindow.TabbedThumbnail.WindowsControl IsNot Nothing Then
					realWindowSize = New Size(Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Width), Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Height))
				End If

				If realWindowSize.Height = -1 AndAlso realWindowSize.Width = -1 Then
                    realWindowSize.Width = realWindowSize.Height.DirectCopy(199)
                End If

				' capture the bitmap for the given control
				' If the user has already specified us a bitmap to use, use that.
				If taskbarWindow.TabbedThumbnail.ClippingRectangle IsNot Nothing AndAlso taskbarWindow.TabbedThumbnail.ClippingRectangle.Value <> Rectangle.Empty Then
					If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						hBitmap = GrabBitmap(taskbarWindow, realWindowSize)
					Else
						hBitmap = taskbarWindow.TabbedThumbnail.CurrentHBitmap
					End If

					' Clip the bitmap we just got.
					Dim bmp As Bitmap = Bitmap.FromHbitmap(hBitmap)

					Dim clippingRectangle As Rectangle = taskbarWindow.TabbedThumbnail.ClippingRectangle.Value

					' If our clipping rect is out of bounds, update it
					If clippingRectangle.Height > requestedSize.Height Then
						clippingRectangle.Height = requestedSize.Height
					End If
					If clippingRectangle.Width > requestedSize.Width Then
						clippingRectangle.Width = requestedSize.Width
					End If

					' NOTE: Is this a memory leak?
					bmp = bmp.Clone(clippingRectangle, bmp.PixelFormat)

					' Make sure we dispose the bitmap before assigning, otherwise we'll have a memory leak
					If hBitmap <> IntPtr.Zero AndAlso taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						ShellNativeMethods.DeleteObject(hBitmap)
					End If
					hBitmap = bmp.GetHbitmap()
					bmp.Dispose()
				Else
					' Else, user didn't want any clipping, if they haven't provided us a bitmap,
					' use the screencapture utility and capture it.

					hBitmap = taskbarWindow.TabbedThumbnail.CurrentHBitmap

					' If no bitmap, capture one using the utility
					If hBitmap = IntPtr.Zero Then
						hBitmap = GrabBitmap(taskbarWindow, realWindowSize)
					End If
				End If

				' Only set the thumbnail if it's not null. 
				' If it's null (either we didn't get the bitmap or size was 0),
				' let DWM handle it
				If hBitmap <> IntPtr.Zero Then
					Dim temp As Bitmap = TabbedThumbnailScreenCapture.ResizeImageWithAspect(hBitmap, requestedSize.Width, requestedSize.Height, True)

					If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						ShellNativeMethods.DeleteObject(hBitmap)
					End If

					hBitmap = temp.GetHbitmap()
					TabbedThumbnailNativeMethods.SetIconicThumbnail(taskbarWindow.WindowToTellTaskbarAbout, hBitmap)
					temp.Dispose()
				End If

				' If the bitmap we have is not coming from the user (i.e. we created it here),
				' then make sure we delete it as we don't need it now.
				If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
					ShellNativeMethods.DeleteObject(hBitmap)
				End If

				Return True
			End If
			Return False
		End Function

		Private Shared Function DispatchLivePreviewBitmapMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(TaskbarNativeMethods.WmDwmSendIconicLivePreviewBitmap) Then
				' Try to get the width/height
				Dim width As Integer = CInt(CLng(m.LParam) >> 16)
				Dim height As Integer = CInt(CLng(m.LParam) And (&Hffff))

				' Default size for the thumbnail
				Dim realWindowSize As New Size(200, 200)

				If taskbarWindow.TabbedThumbnail.WindowHandle <> IntPtr.Zero Then
					TabbedThumbnailNativeMethods.GetClientSize(taskbarWindow.TabbedThumbnail.WindowHandle, realWindowSize)
				ElseIf taskbarWindow.TabbedThumbnail.WindowsControl IsNot Nothing Then
					realWindowSize = New Size(Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Width), Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Height))
				End If

				' If we don't have a valid height/width, use the original window's size
				If width <= 0 Then
					width = realWindowSize.Width
				End If
				If height <= 0 Then
					height = realWindowSize.Height
				End If

				' Fire an event to let the user update their bitmap
				' Raise the event
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailBitmapRequested()

				' capture the bitmap for the given control
				' If the user has already specified us a bitmap to use, use that.
				Dim hBitmap As IntPtr = If(taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero, GrabBitmap(taskbarWindow, realWindowSize), taskbarWindow.TabbedThumbnail.CurrentHBitmap)

				' If we have a valid parent window handle,
				' calculate the offset so we can place the "peek" bitmap
				' correctly on the app window
				If taskbarWindow.TabbedThumbnail.ParentWindowHandle <> IntPtr.Zero AndAlso taskbarWindow.TabbedThumbnail.WindowHandle <> IntPtr.Zero Then
					Dim offset As New System.Drawing.Point()

					' if we don't have a offset specified already by the user...
					If Not taskbarWindow.TabbedThumbnail.PeekOffset.HasValue Then
						offset = WindowUtilities.GetParentOffsetOfChild(taskbarWindow.TabbedThumbnail.WindowHandle, taskbarWindow.TabbedThumbnail.ParentWindowHandle)
					Else
						offset = New System.Drawing.Point(Convert.ToInt32(taskbarWindow.TabbedThumbnail.PeekOffset.Value.X), Convert.ToInt32(taskbarWindow.TabbedThumbnail.PeekOffset.Value.Y))
					End If

					' Only set the peek bitmap if it's not null. 
					' If it's null (either we didn't get the bitmap or size was 0),
					' let DWM handle it
					If hBitmap <> IntPtr.Zero Then
						If offset.X >= 0 AndAlso offset.Y >= 0 Then
							TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, hBitmap, offset, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap)
						End If
					End If

					' If the bitmap we have is not coming from the user (i.e. we created it here),
					' then make sure we delete it as we don't need it now.
					If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						ShellNativeMethods.DeleteObject(hBitmap)
					End If

					Return True
				' Else, we don't have a valid window handle from the user. This is mostly likely because
				' we have a WPF UIElement control. If that's the case, use a different screen capture method
				' and also couple of ways to try to calculate the control's offset w.r.t it's parent.
				ElseIf taskbarWindow.TabbedThumbnail.ParentWindowHandle <> IntPtr.Zero AndAlso taskbarWindow.TabbedThumbnail.WindowsControl IsNot Nothing Then
					Dim offset As System.Windows.Point

					If Not taskbarWindow.TabbedThumbnail.PeekOffset.HasValue Then
						' Calculate the offset for a WPF UIElement control
						' For hidden controls, we can't seem to perform the transform.
						Dim objGeneralTransform As GeneralTransform = taskbarWindow.TabbedThumbnail.WindowsControl.TransformToVisual(taskbarWindow.TabbedThumbnail.WindowsControlParentWindow)
						offset = objGeneralTransform.Transform(New System.Windows.Point(0, 0))
					Else
						offset = New System.Windows.Point(taskbarWindow.TabbedThumbnail.PeekOffset.Value.X, taskbarWindow.TabbedThumbnail.PeekOffset.Value.Y)
					End If

					' Only set the peek bitmap if it's not null. 
					' If it's null (either we didn't get the bitmap or size was 0),
					' let DWM handle it
					If hBitmap <> IntPtr.Zero Then
						If offset.X >= 0 AndAlso offset.Y >= 0 Then
							TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, hBitmap, New System.Drawing.Point(CInt(Math.Truncate(offset.X)), CInt(Math.Truncate(offset.Y))), taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap)
						Else
							TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, hBitmap, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap)
						End If
					End If

					' If the bitmap we have is not coming from the user (i.e. we created it here),
					' then make sure we delete it as we don't need it now.
					If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						ShellNativeMethods.DeleteObject(hBitmap)
					End If

					Return True
				Else
                    ' Else (no parent specified), just set the bitmap. It would take over the entire 
                    ' application window (would work only if you are a MDI app)

                    ' Only set the peek bitmap if it's not null. 
                    ' If it's null (either we didn't get the bitmap or size was 0),
                    ' let DWM handle it
                    If hBitmap <> Nothing Then
                        TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, hBitmap, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap)
                    End If

                    ' If the bitmap we have is not coming from the user (i.e. we created it here),
                    ' then make sure we delete it as we don't need it now.
                    If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
						ShellNativeMethods.DeleteObject(hBitmap)
					End If

					Return True
				End If
			End If
			Return False
		End Function

		Private Shared Function DispatchDestroyMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(WindowMessage.Destroy) Then
				TaskbarList.Instance.UnregisterTab(taskbarWindow.WindowToTellTaskbarAbout)

				taskbarWindow.TabbedThumbnail.RemovedFromTaskbar = True

				Return True
			End If
			Return False
		End Function

		Private Shared Function DispatchNCDestroyMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(WindowMessage.NCDestroy) Then
				' Raise the event
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailClosed()

				' Remove the taskbar window from our internal list
				If _taskbarWindowList.Contains(taskbarWindow) Then
					_taskbarWindowList.Remove(taskbarWindow)
				End If

				taskbarWindow.Dispose()

				Return True
			End If
			Return False
		End Function

		Private Shared Function DispatchSystemCommandMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If m.Msg = CInt(WindowMessage.SystemCommand) Then
				If CInt(m.WParam) = TabbedThumbnailNativeMethods.ScClose Then
					' Raise the event
					If taskbarWindow.TabbedThumbnail.OnTabbedThumbnailClosed() Then
						' Remove the taskbar window from our internal list
						If _taskbarWindowList.Contains(taskbarWindow) Then
							_taskbarWindowList.Remove(taskbarWindow)
						End If

						taskbarWindow.Dispose()
						taskbarWindow = Nothing
					End If
				ElseIf CInt(m.WParam) = TabbedThumbnailNativeMethods.ScMaximize Then
					' Raise the event
					taskbarWindow.TabbedThumbnail.OnTabbedThumbnailMaximized()
				ElseIf CInt(m.WParam) = TabbedThumbnailNativeMethods.ScMinimize Then
					' Raise the event
					taskbarWindow.TabbedThumbnail.OnTabbedThumbnailMinimized()
				End If

				Return True
			End If
			Return False
		End Function

		#End Region

		''' <summary>
		''' Dispatches a window message so that the appropriate events
		''' can be invoked. This is used for the Taskbar's thumbnail toolbar feature.
		''' </summary>
		''' <param name="m">The window message, typically obtained
		''' from a Windows Forms or WPF window procedure.</param>
		''' <param name="taskbarWindow">Taskbar window for which we are intercepting the messages</param>
		''' <returns>Returns true if this method handles the window message</returns>           
		Friend Shared Function DispatchMessage(ByRef m As System.Windows.Forms.Message, taskbarWindow As TaskbarWindow) As Boolean
			If taskbarWindow.EnableThumbnailToolbars Then
				DispatchTaskbarButtonMessages(m, taskbarWindow)
			End If

			' If we are removed from the taskbar, ignore all the messages
			If taskbarWindow.EnableTabbedThumbnails Then
				If taskbarWindow.TabbedThumbnail Is Nothing OrElse taskbarWindow.TabbedThumbnail.RemovedFromTaskbar Then
					Return False
				End If

				If DispatchActivateMessage(m, taskbarWindow) Then
					Return True
				End If

				If DispatchSendIconThumbnailMessage(m, taskbarWindow) Then
					Return True
				End If

				If DispatchLivePreviewBitmapMessage(m, taskbarWindow) Then
					Return True
				End If

				If DispatchDestroyMessage(m, taskbarWindow) Then
					Return True
				End If

				If DispatchNCDestroyMessage(m, taskbarWindow) Then
					Return True
				End If

				If DispatchSystemCommandMessage(m, taskbarWindow) Then
					Return True
				End If
			End If

			Return False
		End Function

		''' <summary>
		''' Helper function to capture a bitmap for a given window handle or incase of WPF app,
		''' an UIElement.
		''' </summary>
		''' <param name="taskbarWindow">The proxy window for which a bitmap needs to be created</param>
		''' <param name="requestedSize">Size for the requested bitmap image</param>
		''' <returns>Bitmap captured from the window handle or UIElement. Null if the window is hidden or it's size is zero.</returns>
		Private Shared Function GrabBitmap(taskbarWindow As TaskbarWindow, requestedSize As System.Drawing.Size) As IntPtr
			Dim hBitmap As IntPtr = IntPtr.Zero

			If taskbarWindow.TabbedThumbnail.WindowHandle <> IntPtr.Zero Then
				'TabbedThumbnail is linked to WinformsControl
				If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
					Using bmp As Bitmap = TabbedThumbnailScreenCapture.GrabWindowBitmap(taskbarWindow.TabbedThumbnail.WindowHandle, requestedSize)

						hBitmap = bmp.GetHbitmap()
					End Using
				Else
					Using img As Image = Image.FromHbitmap(taskbarWindow.TabbedThumbnail.CurrentHBitmap)
						Using bmp As New Bitmap(img, requestedSize)
							hBitmap = If(bmp IsNot Nothing, bmp.GetHbitmap(), IntPtr.Zero)
						End Using
					End Using
				End If
			ElseIf taskbarWindow.TabbedThumbnail.WindowsControl IsNot Nothing Then
				'TabbedThumbnail is linked to a WPF UIElement
				If taskbarWindow.TabbedThumbnail.CurrentHBitmap = IntPtr.Zero Then
					Dim bmp As Bitmap = TabbedThumbnailScreenCapture.GrabWindowBitmap(taskbarWindow.TabbedThumbnail.WindowsControl, 96, 96, requestedSize.Width, requestedSize.Height)

					If bmp IsNot Nothing Then
						hBitmap = bmp.GetHbitmap()
						bmp.Dispose()
					End If
				Else
					Using img As Image = Image.FromHbitmap(taskbarWindow.TabbedThumbnail.CurrentHBitmap)
						Using bmp As New Bitmap(img, requestedSize)

							hBitmap = If(bmp IsNot Nothing, bmp.GetHbitmap(), IntPtr.Zero)
						End Using
					End Using
				End If
			End If

			Return hBitmap
		End Function

		Friend Shared Sub SetActiveTab(taskbarWindow As TaskbarWindow)
			If taskbarWindow IsNot Nothing Then
				TaskbarList.Instance.SetTabActive(taskbarWindow.WindowToTellTaskbarAbout, taskbarWindow.TabbedThumbnail.ParentWindowHandle, 0)
			End If
		End Sub

		Friend Shared Sub UnregisterTab(taskbarWindow As TaskbarWindow)
			If taskbarWindow IsNot Nothing Then
				TaskbarList.Instance.UnregisterTab(taskbarWindow.WindowToTellTaskbarAbout)
			End If
		End Sub

		Friend Shared Sub InvalidatePreview(taskbarWindow As TaskbarWindow)
			If taskbarWindow IsNot Nothing Then
				TabbedThumbnailNativeMethods.DwmInvalidateIconicBitmaps(taskbarWindow.WindowToTellTaskbarAbout)
			End If
		End Sub

		Private Shared Sub AddButtons(taskbarWindow As TaskbarWindow)
            ' Add the buttons
            ' Get the array of thumbnail buttons in native format
            Dim nativeButtons As ThumbButton() = (From thumbButton In taskbarWindow.ThumbnailButtons Select thumbButton.Win32ThumbButton).ToArray()

            ' Add the buttons on the taskbar
            Dim hr As HResult = TaskbarList.Instance.ThumbBarAddButtons(taskbarWindow.WindowToTellTaskbarAbout, CUInt(taskbarWindow.ThumbnailButtons.Length), nativeButtons)

			If Not CoreErrorHelper.Succeeded(hr) Then
				Throw New ShellException(hr)
			End If

			_buttonsAdded = True

			For Each button As ThumbnailToolBarButton In taskbarWindow.ThumbnailButtons
				button.AddedToTaskbar = _buttonsAdded
			Next
		End Sub

		#Region "Event handlers"

		Private Shared Sub thumbnailPreview_TooltipChanged(sender As Object, e As EventArgs)
			Dim preview As TabbedThumbnail = TryCast(sender, TabbedThumbnail)

			Dim taskbarWindow As TaskbarWindow = Nothing

			If preview.WindowHandle = IntPtr.Zero Then
				taskbarWindow = GetTaskbarWindow(preview.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail)
			Else
				taskbarWindow = GetTaskbarWindow(preview.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail)
			End If

			' Update the proxy window for the tabbed thumbnail            
			If taskbarWindow IsNot Nothing Then
				TaskbarList.Instance.SetThumbnailTooltip(taskbarWindow.WindowToTellTaskbarAbout, preview.Tooltip)
			End If
		End Sub

		Private Shared Sub thumbnailPreview_TitleChanged(sender As Object, e As EventArgs)
			Dim preview As TabbedThumbnail = TryCast(sender, TabbedThumbnail)

			Dim taskbarWindow As TaskbarWindow = Nothing

			If preview.WindowHandle = IntPtr.Zero Then
				taskbarWindow = GetTaskbarWindow(preview.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail)
			Else
				taskbarWindow = GetTaskbarWindow(preview.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail)
			End If

			' Update the proxy window for the tabbed thumbnail
			If taskbarWindow IsNot Nothing Then
				taskbarWindow.SetTitle(preview.Title)
			End If
		End Sub

#End Region
    End Class
End Namespace
