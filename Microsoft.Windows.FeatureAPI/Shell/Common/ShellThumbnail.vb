'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports System.Windows.Media.Imaging
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Represents a thumbnail or an icon for a ShellObject.
	''' </summary>
	Public Class ShellThumbnail
		#Region "Private members"

		''' <summary>
		''' Native shellItem
		''' </summary>
		Private shellItemNative As IShellItem

		''' <summary>
		''' Internal member to keep track of the current size
		''' </summary>
		Private m_currentSize As New System.Windows.Size(256, 256)

		#End Region

		#Region "Constructors"

		''' <summary>
		''' Internal constructor that takes in a parent ShellObject.
		''' </summary>
		''' <param name="shellObject"></param>
		Friend Sub New(shellObject As ShellObject)
			If shellObject Is Nothing OrElse shellObject.NativeShellItem Is Nothing Then
				Throw New ArgumentNullException("shellObject")
			End If

			shellItemNative = shellObject.NativeShellItem
		End Sub

		#End Region

		#Region "Public properties"

		''' <summary>
		''' Gets or sets the default size of the thumbnail or icon. The default is 32x32 pixels for icons and 
		''' 256x256 pixels for thumbnails.
		''' </summary>
		''' <remarks>If the size specified is larger than the maximum size of 1024x1024 for thumbnails and 256x256 for icons,
		''' an <see cref="System.ArgumentOutOfRangeException"/> is thrown.
		''' </remarks>
		Public Property CurrentSize() As System.Windows.Size
			Get
				Return m_currentSize
			End Get
			Set
				' Check for 0; negative number check not required as System.Windows.Size only allows positive numbers.
				If value.Height = 0 OrElse value.Width = 0 Then
					Throw New System.ArgumentOutOfRangeException("value", LocalizedMessages.ShellThumbnailSizeCannotBe0)
				End If

				Dim size As System.Windows.Size = If((FormatOption = ShellThumbnailFormatOption.IconOnly), DefaultIconSize.Maximum, DefaultThumbnailSize.Maximum)

				If value.Height > size.Height OrElse value.Width > size.Width Then
					Throw New System.ArgumentOutOfRangeException("value", String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.ShellThumbnailCurrentSizeRange, size.ToString()))
				End If

				m_currentSize = value
			End Set
		End Property

		''' <summary>
		''' Gets the thumbnail or icon image in <see cref="System.Drawing.Bitmap"/> format.
		''' Null is returned if the ShellObject does not have a thumbnail or icon image.
		''' </summary>
		Public ReadOnly Property Bitmap() As Bitmap
			Get
				Return GetBitmap(CurrentSize)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon image in <see cref="System.Windows.Media.Imaging.BitmapSource"/> format. 
		''' Null is returned if the ShellObject does not have a thumbnail or icon image.
		''' </summary>
		Public ReadOnly Property BitmapSource() As BitmapSource
			Get
				Return GetBitmapSource(CurrentSize)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon image in <see cref="System.Drawing.Icon"/> format. 
		''' Null is returned if the ShellObject does not have a thumbnail or icon image.
		''' </summary>
		Public ReadOnly Property Icon() As Icon
			Get
				Return Icon.FromHandle(Bitmap.GetHicon())
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in small size and <see cref="System.Drawing.Bitmap"/> format.
		''' </summary>
		Public ReadOnly Property SmallBitmap() As Bitmap
			Get
				Return GetBitmap(DefaultIconSize.Small, DefaultThumbnailSize.Small)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in small size and <see cref="System.Windows.Media.Imaging.BitmapSource"/> format.
		''' </summary>
		Public ReadOnly Property SmallBitmapSource() As BitmapSource
			Get
				Return GetBitmapSource(DefaultIconSize.Small, DefaultThumbnailSize.Small)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in small size and <see cref="System.Drawing.Icon"/> format.
		''' </summary>
		Public ReadOnly Property SmallIcon() As Icon
			Get
				Return Icon.FromHandle(SmallBitmap.GetHicon())
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in Medium size and <see cref="System.Drawing.Bitmap"/> format.
		''' </summary>
		Public ReadOnly Property MediumBitmap() As Bitmap
			Get
				Return GetBitmap(DefaultIconSize.Medium, DefaultThumbnailSize.Medium)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in medium size and <see cref="System.Windows.Media.Imaging.BitmapSource"/> format.
		''' </summary>
		Public ReadOnly Property MediumBitmapSource() As BitmapSource
			Get
				Return GetBitmapSource(DefaultIconSize.Medium, DefaultThumbnailSize.Medium)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in Medium size and <see cref="System.Drawing.Icon"/> format.
		''' </summary>
		Public ReadOnly Property MediumIcon() As Icon
			Get
				Return Icon.FromHandle(MediumBitmap.GetHicon())
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in large size and <see cref="System.Drawing.Bitmap"/> format.
		''' </summary>
		Public ReadOnly Property LargeBitmap() As Bitmap
			Get
				Return GetBitmap(DefaultIconSize.Large, DefaultThumbnailSize.Large)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in large size and <see cref="System.Windows.Media.Imaging.BitmapSource"/> format.
		''' </summary>
		Public ReadOnly Property LargeBitmapSource() As BitmapSource
			Get
				Return GetBitmapSource(DefaultIconSize.Large, DefaultThumbnailSize.Large)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in Large size and <see cref="System.Drawing.Icon"/> format.
		''' </summary>
		Public ReadOnly Property LargeIcon() As Icon
			Get
				Return Icon.FromHandle(LargeBitmap.GetHicon())
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in extra large size and <see cref="System.Drawing.Bitmap"/> format.
		''' </summary>
		Public ReadOnly Property ExtraLargeBitmap() As Bitmap
			Get
				Return GetBitmap(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in Extra Large size and <see cref="System.Windows.Media.Imaging.BitmapSource"/> format.
		''' </summary>
		Public ReadOnly Property ExtraLargeBitmapSource() As BitmapSource
			Get
				Return GetBitmapSource(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge)
			End Get
		End Property

		''' <summary>
		''' Gets the thumbnail or icon in Extra Large size and <see cref="System.Drawing.Icon"/> format.
		''' </summary>
		Public ReadOnly Property ExtraLargeIcon() As Icon
			Get
				Return Icon.FromHandle(ExtraLargeBitmap.GetHicon())
			End Get
		End Property

		''' <summary>
		''' Gets or sets a value that determines if the current retrieval option is cache or extract, cache only, or from memory only.
		''' The default is cache or extract.
		''' </summary>
		Public Property RetrievalOption() As ShellThumbnailRetrievalOption
			Get
				Return m_RetrievalOption
			End Get
			Set
				m_RetrievalOption = Value
			End Set
		End Property
		Private m_RetrievalOption As ShellThumbnailRetrievalOption

		Private m_formatOption As ShellThumbnailFormatOption = ShellThumbnailFormatOption.[Default]
		''' <summary>
		''' Gets or sets a value that determines if the current format option is thumbnail or icon, thumbnail only, or icon only.
		''' The default is thumbnail or icon.
		''' </summary>
		Public Property FormatOption() As ShellThumbnailFormatOption
			Get
				Return m_formatOption
			End Get
			Set
				m_formatOption = value

				' Do a similar check as we did in CurrentSize property setter,
				' If our mode is IconOnly, then our max is defined by DefaultIconSize.Maximum. We should make sure 
				' our CurrentSize is within this max range
				If FormatOption = ShellThumbnailFormatOption.IconOnly AndAlso (CurrentSize.Height > DefaultIconSize.Maximum.Height OrElse CurrentSize.Width > DefaultIconSize.Maximum.Width) Then
					CurrentSize = DefaultIconSize.Maximum
				End If
			End Set
		End Property


		''' <summary>
		''' Gets or sets a value that determines if the user can manually stretch the returned image.
		''' The default value is false.
		''' </summary>
		''' <remarks>
		''' For example, if the caller passes in 80x80 a 96x96 thumbnail could be returned. 
		''' This could be used as a performance optimization if the caller will need to stretch 
		''' the image themselves anyway. Note that the Shell implementation performs a GDI stretch blit. 
		''' If the caller wants a higher quality image stretch, they should pass this flag and do it themselves.
		''' </remarks>
		Public Property AllowBiggerSize() As Boolean
			Get
				Return m_AllowBiggerSize
			End Get
			Set
				m_AllowBiggerSize = Value
			End Set
		End Property
		Private m_AllowBiggerSize As Boolean

		#End Region

		#Region "Private Methods"

		Private Function CalculateFlags() As ShellNativeMethods.SIIGBF
			Dim flags As ShellNativeMethods.SIIGBF = &H0

			If AllowBiggerSize Then
				flags = flags Or ShellNativeMethods.SIIGBF.BiggerSizeOk
			End If

			If RetrievalOption = ShellThumbnailRetrievalOption.CacheOnly Then
				flags = flags Or ShellNativeMethods.SIIGBF.InCacheOnly
			ElseIf RetrievalOption = ShellThumbnailRetrievalOption.MemoryOnly Then
				flags = flags Or ShellNativeMethods.SIIGBF.MemoryOnly
			End If

			If FormatOption = ShellThumbnailFormatOption.IconOnly Then
				flags = flags Or ShellNativeMethods.SIIGBF.IconOnly
			ElseIf FormatOption = ShellThumbnailFormatOption.ThumbnailOnly Then
				flags = flags Or ShellNativeMethods.SIIGBF.ThumbnailOnly
			End If

			Return flags
		End Function

		Private Function GetHBitmap(size As System.Windows.Size) As IntPtr
			Dim hbitmap As IntPtr = IntPtr.Zero

			' Create a size structure to pass to the native method
			Dim nativeSIZE As New CoreNativeMethods.Size()
			nativeSIZE.Width = Convert.ToInt32(size.Width)
			nativeSIZE.Height = Convert.ToInt32(size.Height)

			' Use IShellItemImageFactory to get an icon
			' Options passed in: Resize to fit
			Dim hr As HResult = DirectCast(shellItemNative, IShellItemImageFactory).GetImage(nativeSIZE, CalculateFlags(), hbitmap)

			If hr = HResult.Ok Then
				Return hbitmap
			ElseIf CUInt(hr) = &H8004b200UI AndAlso FormatOption = ShellThumbnailFormatOption.ThumbnailOnly Then
				' Thumbnail was requested, but this ShellItem doesn't have a thumbnail.
				Throw New InvalidOperationException(LocalizedMessages.ShellThumbnailDoesNotHaveThumbnail, Marshal.GetExceptionForHR(CInt(hr)))
			ElseIf CUInt(hr) = &H80040154UI Then
				' REGDB_E_CLASSNOTREG
				Throw New NotSupportedException(LocalizedMessages.ShellThumbnailNoHandler, Marshal.GetExceptionForHR(CInt(hr)))
			End If

			Throw New ShellException(hr)
		End Function

		Private Function GetBitmap(iconOnlySize As System.Windows.Size, thumbnailSize As System.Windows.Size) As Bitmap
			Return GetBitmap(If(FormatOption = ShellThumbnailFormatOption.IconOnly, iconOnlySize, thumbnailSize))
		End Function

		Private Function GetBitmap(size As System.Windows.Size) As Bitmap
			Dim hBitmap As IntPtr = GetHBitmap(size)

			' return a System.Drawing.Bitmap from the hBitmap
			Dim returnValue As Bitmap = Bitmap.FromHbitmap(hBitmap)

			' delete HBitmap to avoid memory leaks
			ShellNativeMethods.DeleteObject(hBitmap)

			Return returnValue
		End Function

		Private Function GetBitmapSource(iconOnlySize As System.Windows.Size, thumbnailSize As System.Windows.Size) As BitmapSource
			Return GetBitmapSource(If(FormatOption = ShellThumbnailFormatOption.IconOnly, iconOnlySize, thumbnailSize))
		End Function

		Private Function GetBitmapSource(size As System.Windows.Size) As BitmapSource
			Dim hBitmap As IntPtr = GetHBitmap(size)

			' return a System.Media.Imaging.BitmapSource
			' Use interop to create a BitmapSource from hBitmap.
			Dim returnValue As BitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

			' delete HBitmap to avoid memory leaks
			ShellNativeMethods.DeleteObject(hBitmap)

			Return returnValue
		End Function

		#End Region

	End Class
End Namespace
