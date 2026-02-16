'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows
Imports System.Windows.Interop
Imports System.Windows.Media.Imaging
Imports Microsoft.Windows.Internal

Namespace Shell
	''' <summary>
	''' Represents a standard system icon.
	''' </summary>
	Public Class StockIcon
		Implements IDisposable
		#Region "Private Members"

		Private m_identifier As StockIconIdentifier = StockIconIdentifier.Application
		Private m_currentSize As StockIconSize = StockIconSize.Large
		Private m_linkOverlay As Boolean
		Private m_selected As Boolean
		Private invalidateIcon As Boolean = True
		Private hIcon As IntPtr = IntPtr.Zero

		#End Region

		#Region "Public Constructors"

		''' <summary>
		''' Creates a new StockIcon instance with the specified identifer, default size 
		''' and no link overlay or selected states.
		''' </summary>
		''' <param name="id">A value that identifies the icon represented by this instance.</param>
		Public Sub New(id As StockIconIdentifier)
			m_identifier = id
			invalidateIcon = True
		End Sub

		''' <summary>
		''' Creates a new StockIcon instance with the specified identifer and options.
		''' </summary>
		''' <param name="id">A value that identifies the icon represented by this instance.</param>
		''' <param name="size">A value that indicates the size of the stock icon.</param>
		''' <param name="isLinkOverlay">A bool value that indicates whether the icon has a link overlay.</param>
		''' <param name="isSelected">A bool value that indicates whether the icon is in a selected state.</param>
		Public Sub New(id As StockIconIdentifier, size As StockIconSize, isLinkOverlay As Boolean, isSelected As Boolean)
			m_identifier = id
			m_linkOverlay = isLinkOverlay
			m_selected = isSelected
			m_currentSize = size
			invalidateIcon = True
		End Sub

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' Gets or sets a value indicating whether the icon appears selected.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public Property Selected() As Boolean
			Get
				Return m_selected
			End Get
			Set
				m_selected = value
				invalidateIcon = True
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value that cotrols whether to put a link overlay on the icon.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public Property LinkOverlay() As Boolean
			Get
				Return m_linkOverlay
			End Get
			Set
				m_linkOverlay = value
				invalidateIcon = True
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value that controls the size of the Stock Icon.
		''' </summary>
		''' <value>A <see cref="Shell.StockIconSize"/> value.</value>
		Public Property CurrentSize() As StockIconSize
			Get
				Return m_currentSize
			End Get
			Set
				m_currentSize = value
				invalidateIcon = True
			End Set
		End Property

		''' <summary>
		''' Gets or sets the Stock Icon identifier associated with this icon.
		''' </summary>
		Public Property Identifier() As StockIconIdentifier
			Get
				Return m_identifier
			End Get
			Set
				m_identifier = value
				invalidateIcon = True
			End Set
		End Property

		''' <summary>
		''' Gets the icon image in <see cref="System.Drawing.Bitmap"/> format. 
		''' </summary>
		Public ReadOnly Property Bitmap() As Bitmap
			Get
				UpdateHIcon()

				Return If(hIcon <> IntPtr.Zero, Bitmap.FromHicon(hIcon), Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the icon image in <see cref="System.Windows.Media.Imaging.BitmapSource"/> format. 
		''' </summary>
		Public ReadOnly Property BitmapSource() As BitmapSource
			Get
				UpdateHIcon()

				Return If((hIcon <> IntPtr.Zero), Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, Nothing), Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the icon image in <see cref="System.Drawing.Icon"/> format.
		''' </summary>
		Public ReadOnly Property Icon() As Icon
			Get
				UpdateHIcon()

				Return If(hIcon <> IntPtr.Zero, Icon.FromHandle(hIcon), Nothing)
			End Get
		End Property

		#End Region

		#Region "Private Methods"

		Private Sub UpdateHIcon()
			If invalidateIcon Then
				If hIcon <> IntPtr.Zero Then
					CoreNativeMethods.DestroyIcon(hIcon)
				End If

				hIcon = GetHIcon()

				invalidateIcon = False
			End If
		End Sub

		Private Function GetHIcon() As IntPtr
			' Create our internal flag to pass to the native method
			Dim flags As StockIconsNativeMethods.StockIconOptions = StockIconsNativeMethods.StockIconOptions.Handle

			' Based on the current settings, update the flags
			If CurrentSize = StockIconSize.Small Then
				flags = flags Or StockIconsNativeMethods.StockIconOptions.Small
			ElseIf CurrentSize = StockIconSize.ShellSize Then
				flags = flags Or StockIconsNativeMethods.StockIconOptions.ShellSize
			Else
					' default
				flags = flags Or StockIconsNativeMethods.StockIconOptions.Large
			End If

			If Selected Then
				flags = flags Or StockIconsNativeMethods.StockIconOptions.Selected
			End If

			If LinkOverlay Then
				flags = flags Or StockIconsNativeMethods.StockIconOptions.LinkOverlay
			End If

			' Create a StockIconInfo structure to pass to the native method.
			Dim info As New StockIconsNativeMethods.StockIconInfo()
			info.StuctureSize = CType(Marshal.SizeOf(GetType(StockIconsNativeMethods.StockIconInfo)), UInt32)

			' Pass the struct to the native method
			Dim hr As HResult = StockIconsNativeMethods.SHGetStockIconInfo(m_identifier, flags, info)

			' If we get an error, return null as the icon requested might not be supported
			' on the current system
			If hr <> HResult.Ok Then
				If hr = HResult.InvalidArguments Then
					Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, GlobalLocalizedMessages.StockIconInvalidGuid, m_identifier))
				End If

				Return IntPtr.Zero
			End If

			' If we succeed, return the HIcon
			Return info.Handle
		End Function

		#End Region

		#Region "IDisposable Members"

		''' <summary>
		''' Release the native and managed objects
		''' </summary>
		''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overridable Sub Dispose(disposing As Boolean)
					' dispose managed resources here
			If disposing Then
			End If

			' Unmanaged resources
			If hIcon <> IntPtr.Zero Then
				CoreNativeMethods.DestroyIcon(hIcon)
			End If
		End Sub

		''' <summary>
		''' Release the native objects
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

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

		#End Region
	End Class
End Namespace

