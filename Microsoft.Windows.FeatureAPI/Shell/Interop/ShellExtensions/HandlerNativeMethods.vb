Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes

Namespace Shell.Interop
	Friend NotInheritable Class HandlerNativeMethods
		Private Sub New()
		End Sub
		<DllImport("user32.dll")> _
		Friend Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr
		End Function

		<DllImport("user32.dll", CharSet := CharSet.Auto)> _
		Friend Shared Function GetFocus() As IntPtr
		End Function

		Friend Shared ReadOnly PreviewHandlerGuid As New Guid("{8895b1c6-b41f-4c1c-a562-0d564250836f}")
		Friend Shared ReadOnly ThumbnailProviderGuid As New Guid("{e357fccd-a995-4576-b01f-234630154e96}")
	End Class

	#Region "Interfaces"

	''' <summary>
	''' ComVisible interface for native IThumbnailProvider
	''' </summary>
	<ComVisible(True)> _
	<Guid("e357fccd-a995-4576-b01f-234630154e96")> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IThumbnailProvider
		''' <summary>
		''' Gets a pointer to a bitmap to display as a thumbnail
		''' </summary>
		''' <param name="squareLength"></param>
		''' <param name="bitmapHandle"></param>
		''' <param name="bitmapType"></param>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId := "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId := "1#")> _
		Sub GetThumbnail(squareLength As UInteger, ByRef bitmapHandle As IntPtr, ByRef bitmapType As UInt32)
	End Interface

	''' <summary>
	''' Provides means by which to intiailze with a file.
	''' </summary>
	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")> _
	Public Interface IInitializeWithFile
		''' <summary>
		''' Initializes with a file.
		''' </summary>
		''' <param name="filePath"></param>
		''' <param name="fileMode"></param>
		Sub Initialize(<MarshalAs(UnmanagedType.LPWStr)> filePath As String, fileMode As AccessModes)
	End Interface

	''' <summary>
	''' Provides means by which to initialize with a stream.
	''' </summary>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")> _
	<ComVisible(True)> _
	<Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IInitializeWithStream
		''' <summary>
		''' Initializes with a stream.
		''' </summary>
		''' <param name="stream"></param>
		''' <param name="fileMode"></param>
		Sub Initialize(stream As IStream, fileMode As AccessModes)
	End Interface

	''' <summary>
	''' Provides means by which to initialize with a ShellObject
	''' </summary>
	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")> _
	Public Interface IInitializeWithItem
		''' <summary>
		''' Initializes with ShellItem
		''' </summary>
		''' <param name="shellItem"></param>
		''' <param name="accessMode"></param>
		Sub Initialize(shellItem As IntPtr, accessMode As AccessModes)
	End Interface

	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352")> _
	Interface IObjectWithSite
		Sub SetSite(<[In], MarshalAs(UnmanagedType.IUnknown)> pUnkSite As Object)
		Sub GetSite(ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppvSite As Object)
	End Interface

	<ComImport> _
	<Guid("00000114-0000-0000-C000-000000000046")> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Interface IOleWindow
		Sub GetWindow(ByRef phwnd As IntPtr)
		Sub ContextSensitiveHelp(<MarshalAs(UnmanagedType.Bool)> fEnterMode As Boolean)
	End Interface

	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")> _
	Interface IPreviewHandler
		Sub SetWindow(hwnd As IntPtr, ByRef rect As RECT)
		Sub SetRect(ByRef rect As RECT)
		Sub DoPreview()
		Sub Unload()
		Sub SetFocus()
		Sub QueryFocus(ByRef phwnd As IntPtr)
		<PreserveSig> _
		Function TranslateAccelerator(ByRef pmsg As MSG) As UInteger
	End Interface

	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("fec87aaf-35f9-447a-adb7-20234491401a")> _
	Interface IPreviewHandlerFrame
		Sub GetWindowContext(pinfo As IntPtr)
		<PreserveSig> _
		Function TranslateAccelerator(ByRef pmsg As MSG) As UInteger
	End Interface

	<ComImport> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	<Guid("8327b13c-b63f-4b24-9b8a-d010dcc3f599")> _
	Interface IPreviewHandlerVisuals
		Sub SetBackgroundColor(color As COLORREF)
		Sub SetFont(ByRef plf As LogFont)
		Sub SetTextColor(color As COLORREF)
	End Interface
	#End Region

	#Region "Structs"

	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
	Friend Class LogFont
		Friend height As Integer
		Friend width As Integer
		Friend escapement As Integer
		Friend orientation As Integer
		Friend weight As Integer
		Friend italic As Byte
		Friend underline As Byte
		Friend strikeOut As Byte
		Friend charSet As Byte
		Friend outPrecision As Byte
		Friend clipPrecision As Byte
		Friend quality As Byte
		Friend pitchAndFamily As Byte
		<MarshalAs(UnmanagedType.ByValTStr, SizeConst := 32)> _
		Friend lfFaceName As String = String.Empty
	End Class

	<StructLayout(LayoutKind.Sequential)> _
	Friend Structure COLORREF
		Public Dword As UInteger
		Public ReadOnly Property Color() As Color
			Get
				Return Color.FromArgb(CInt(&HffUI And Dword), CInt(&Hff00UI And Dword) >> 8, CInt(&Hff0000UI And Dword) >> 16)
			End Get
		End Property
	End Structure

	<StructLayout(LayoutKind.Sequential)> _
	Friend Structure MSG
		Public hwnd As IntPtr
		Public message As Integer
		Public wParam As IntPtr
		Public lParam As IntPtr
		Public time As Integer
		Public pt_x As Integer
		Public pt_y As Integer
	End Structure

	<StructLayout(LayoutKind.Sequential)> _
	Friend Structure RECT
		Public ReadOnly left As Integer
		Public ReadOnly top As Integer
		Public ReadOnly right As Integer
		Public ReadOnly bottom As Integer
		Public Function ToRectangle() As Rectangle
			Return Rectangle.FromLTRB(left, top, right, bottom)
		End Function
	End Structure

	#End Region

	''' <summary>
	''' Thumbnail Alpha Types
	''' </summary>
	Public Enum ThumbnailAlphaType
		''' <summary>
		''' Let the system decide.
		''' </summary>
		Unknown = 0

		''' <summary>
		''' No transparency
		''' </summary>
		NoAlphaChannel = 1

		''' <summary>
		''' Has transparency
		''' </summary>
		HasAlphaChannel = 2
	End Enum

End Namespace
