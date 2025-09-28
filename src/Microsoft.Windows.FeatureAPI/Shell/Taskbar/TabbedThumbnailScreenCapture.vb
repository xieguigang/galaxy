'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports System.IO
Imports System.Windows
Imports System.Windows.Interop
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports Microsoft.Windows.Shell

Namespace Taskbar
	''' <summary>
	''' Helper class to capture a control or window as System.Drawing.Bitmap
	''' </summary>
	Public NotInheritable Class TabbedThumbnailScreenCapture
		Private Sub New()
		End Sub
		''' <summary>
		''' Captures a screenshot of the specified window at the specified
		''' bitmap size. <para/>NOTE: This method will not accurately capture controls
		''' that are hidden or obstructed (partially or completely) by another control (e.g. hidden tabs,
		''' or MDI child windows that are obstructed by other child windows/forms).
		''' </summary>
		''' <param name="windowHandle">The window handle.</param>
		''' <param name="bitmapSize">The requested bitmap size.</param>
		''' <returns>A screen capture of the window.</returns>        
		Public Shared Function GrabWindowBitmap(windowHandle As IntPtr, bitmapSize As System.Drawing.Size) As Bitmap
			If bitmapSize.Height <= 0 OrElse bitmapSize.Width <= 0 Then
				Return Nothing
			End If

			Dim windowDC As IntPtr = IntPtr.Zero

			Try
				windowDC = TabbedThumbnailNativeMethods.GetWindowDC(windowHandle)

				Dim realWindowSize As System.Drawing.Size
				TabbedThumbnailNativeMethods.GetClientSize(windowHandle, realWindowSize)

				If realWindowSize = System.Drawing.Size.Empty Then
					realWindowSize = New System.Drawing.Size(200, 200)
				End If

				Dim size As System.Drawing.Size = If((bitmapSize = System.Drawing.Size.Empty), realWindowSize, bitmapSize)

				Dim targetBitmap As Bitmap = Nothing
				Try


					targetBitmap = New Bitmap(size.Width, size.Height)

					Using targetGr As Graphics = Graphics.FromImage(targetBitmap)
						Dim targetDC As IntPtr = targetGr.GetHdc()
							'SRCCOPY
						Dim operation As UInteger = &Hcc0020

						Dim ncArea As System.Drawing.Size = WindowUtilities.GetNonClientArea(windowHandle)

						Dim success As Boolean = TabbedThumbnailNativeMethods.StretchBlt(targetDC, 0, 0, targetBitmap.Width, targetBitmap.Height, windowDC, _
							ncArea.Width, ncArea.Height, realWindowSize.Width, realWindowSize.Height, operation)

						targetGr.ReleaseHdc(targetDC)

						If Not success Then
							Return Nothing
						End If

						Return targetBitmap
					End Using
				Catch
					If targetBitmap IsNot Nothing Then
						targetBitmap.Dispose()
					End If
					Throw
				End Try
			Finally
				If windowDC <> IntPtr.Zero Then
					TabbedThumbnailNativeMethods.ReleaseDC(windowHandle, windowDC)
				End If
			End Try
		End Function

		''' <summary>
		''' Grabs a snapshot of a WPF UIElement and returns the image as Bitmap.
		''' </summary>
		''' <param name="element">Represents the element to take the snapshot from.</param>
		''' <param name="dpiX">Represents the X DPI value used to capture this snapshot.</param>
		''' <param name="dpiY">Represents the Y DPI value used to capture this snapshot.</param>
		''' <param name="width">The requested bitmap width.</param>
		''' <param name="height">The requested bitmap height.</param>
		''' <returns>Returns the bitmap (PNG format).</returns>
		Public Shared Function GrabWindowBitmap(element As UIElement, dpiX As Integer, dpiY As Integer, width As Integer, height As Integer) As Bitmap
			' Special case for HwndHost controls
			Dim host As HwndHost = TryCast(element, HwndHost)
			If host IsNot Nothing Then
				Dim handle As IntPtr = host.Handle
				Return GrabWindowBitmap(handle, New System.Drawing.Size(width, height))
			End If

			Dim bounds As Rect = VisualTreeHelper.GetDescendantBounds(element)

			' create the renderer.
			If bounds.Height = 0 OrElse bounds.Width = 0 Then
					' 0 sized element. Probably hidden
				Return Nothing
			End If

			Dim rendertarget As New RenderTargetBitmap(CInt(Math.Truncate(bounds.Width * dpiX / 96.0)), CInt(Math.Truncate(bounds.Height * dpiY / 96.0)), dpiX, dpiY, PixelFormats.[Default])

			Dim dv As New DrawingVisual()
			Using ctx As DrawingContext = dv.RenderOpen()
				Dim vb As New VisualBrush(element)
				ctx.DrawRectangle(vb, Nothing, New Rect(New System.Windows.Point(), bounds.Size))
			End Using

			rendertarget.Render(dv)

			Dim bmpe As BitmapEncoder = New PngBitmapEncoder()
			bmpe.Frames.Add(BitmapFrame.Create(rendertarget))

			Dim bmp As Bitmap
			' Create a MemoryStream with the image.            
			Using fl As New MemoryStream()
				bmpe.Save(fl)
				fl.Position = 0
				bmp = New Bitmap(fl)
			End Using

			Return DirectCast(bmp.GetThumbnailImage(width, height, Nothing, IntPtr.Zero), Bitmap)
		End Function

		''' <summary>
		''' Resizes the given bitmap while maintaining the aspect ratio.
		''' </summary>
		''' <param name="originalHBitmap">Original/source bitmap</param>
		''' <param name="newWidth">Maximum width for the new image</param>
		''' <param name="maxHeight">Maximum height for the new image</param>
		''' <param name="resizeIfWider">If true and requested image is wider than the source, the new image is resized accordingly.</param>
		''' <returns></returns>
		Friend Shared Function ResizeImageWithAspect(originalHBitmap As IntPtr, newWidth As Integer, maxHeight As Integer, resizeIfWider As Boolean) As Bitmap
			Dim originalBitmap As Bitmap = Bitmap.FromHbitmap(originalHBitmap)

			Try
				If resizeIfWider AndAlso originalBitmap.Width <= newWidth Then
					newWidth = originalBitmap.Width
				End If

				Dim newHeight As Integer = originalBitmap.Height * newWidth \ originalBitmap.Width

				If newHeight > maxHeight Then
					' Height resize if necessary
					newWidth = originalBitmap.Width * maxHeight \ originalBitmap.Height
					newHeight = maxHeight
				End If

				' Create the new image with the sizes we've calculated
				Return DirectCast(originalBitmap.GetThumbnailImage(newWidth, newHeight, Nothing, IntPtr.Zero), Bitmap)
			Finally
				originalBitmap.Dispose()
				originalBitmap = Nothing
			End Try
		End Function
	End Class
End Namespace
