' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace ExtendedLinguisticServices

	<Flags> _
	Friend Enum ServiceTypes
		None = &H0
		IsOneToOneLanguageMapping = &H1
		HasSubServices = &H2
		OnlineOnly = &H4
		HighLevel = &H8
		LowLevel = &H16
	End Enum

	<Flags> _
	Friend Enum EnumTypes
		None = &H0
		OnlineService = &H1
		OfflineService = &H2
		HighLevel = &H4
		LowLevel = &H8
	End Enum

	' Lives in native memory.
	' Only used for a temporary managed copy.
	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure Win32Service
		Friend _size As IntPtr
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _copyright As String
		Friend _majorVersion As UShort
		Friend _minorVersion As UShort
		Friend _buildVersion As UShort
		Friend _stepVersion As UShort
		Friend _inputContentTypesCount As UInteger
		Friend _inputContentTypes As IntPtr
		Friend _outputContentTypesCount As UInteger
		Friend _outputContentTypes As IntPtr
		Friend _inputLanguagesCount As UInteger
		Friend _inputLanguages As IntPtr
		Friend _outputLanguagesCount As UInteger
		Friend _outputLanguages As IntPtr
		Friend _inputScriptsCount As UInteger
		Friend _inputScripts As IntPtr
		Friend _outputScriptsCount As UInteger
		Friend _outputScripts As IntPtr
		Friend _guid As Guid
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _category As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _description As String
		Friend _privateDataSize As UInteger
		Friend _privateData As IntPtr
		Friend _context As IntPtr
		Friend _serviceTypes As ServiceTypes
	End Structure

	' Lives in managed memory. Used to pass parameters.
	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure Win32EnumOptions
		Friend _size As IntPtr
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _category As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputLanguage As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputLanguage As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputScript As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputScript As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputContentType As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputContentType As String
		Friend _pGuid As IntPtr
		Friend _serviceTypes As EnumTypes
	End Structure

	' Lives in managed memory. Used to pass parameters.
	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure Win32Options
		Friend _size As IntPtr
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputLanguage As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputLanguage As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputScript As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputScript As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _inputContentType As String
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _outputContentType As String
		Friend _UILanguage As IntPtr
		Friend _recognizeCallback As IntPtr
		Friend _recognizeCallerData As IntPtr
		Friend _recognizeCallerDataSize As UInteger
		Friend _actionCallback As IntPtr
		Friend _actionCallerData As IntPtr
		Friend _actionCallerDataSize As UInteger
		Friend _serviceFlag As UInteger
		Friend _getActionDisplayName As UInteger
	End Structure

	' Lives in managed memory. Used to represent results.
	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure Win32DataRange
		Friend _startIndex As UInteger
		Friend _endIndex As UInteger
		Friend _description As IntPtr
		Friend _descriptionLength As UInteger
		Friend _data As IntPtr
		Friend _dataSize As UInteger
		<MarshalAs(UnmanagedType.LPWStr)> _
		Friend _contentType As String
		Friend _actionIDs As IntPtr
		Friend _actionsCount As UInteger
		Friend _actionDisplayNames As IntPtr
	End Structure

	' Lives in managed memory.
	<StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)> _
	Friend Structure Win32PropertyBag
		Friend _size As IntPtr
		Friend _ranges As IntPtr
		Friend _rangesCount As UInteger
		Friend _serviceData As IntPtr
		Friend _serviceDataSize As UInteger
		Friend _callerData As IntPtr
		Friend _callerDataSize As UInteger
		Friend _context As IntPtr
	End Structure

End Namespace
