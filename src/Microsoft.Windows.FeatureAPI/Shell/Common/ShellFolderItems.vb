'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell
	Class ShellFolderItems
		Implements IEnumerator(Of ShellObject)
		#Region "Private Fields"

		Private nativeEnumIdList As IEnumIDList
		Private currentItem As ShellObject
		Private nativeShellFolder As ShellContainer

		#End Region

		#Region "Internal Constructor"

		Friend Sub New(nativeShellFolder As ShellContainer)
			Me.nativeShellFolder = nativeShellFolder

			Dim hr As HResult = nativeShellFolder.NativeShellFolder.EnumObjects(IntPtr.Zero, ShellNativeMethods.ShellFolderEnumerationOptions.Folders Or ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders, nativeEnumIdList)


			If Not CoreErrorHelper.Succeeded(hr) Then
				If hr = HResult.Canceled Then
					Throw New System.IO.FileNotFoundException()
				Else
					Throw New ShellException(hr)
				End If


			End If
		End Sub

		#End Region

		#Region "IEnumerator<ShellObject> Members"

		Public ReadOnly Property Current() As ShellObject Implements IEnumerator(Of ShellObject).Current
			Get
				Return currentItem
			End Get
		End Property

		#End Region

		#Region "IDisposable Members"

		Public Sub Dispose() Implements IDisposable.Dispose
			If nativeEnumIdList IsNot Nothing Then
				Marshal.ReleaseComObject(nativeEnumIdList)
				nativeEnumIdList = Nothing
			End If
		End Sub

		#End Region

		#Region "IEnumerator Members"

		Private ReadOnly Property IEnumerator_Current() As Object Implements IEnumerator.Current
			Get
				Return currentItem
			End Get
		End Property


		''' <summary>
		''' 
		''' </summary>
		''' <returns></returns>
		Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
			If nativeEnumIdList Is Nothing Then
				Return False
			End If

			Dim item As IntPtr
			Dim numItemsReturned As UInteger
			Dim itemsRequested As UInteger = 1
			Dim hr As HResult = nativeEnumIdList.[Next](itemsRequested, item, numItemsReturned)

			If numItemsReturned < itemsRequested OrElse hr <> HResult.Ok Then
				Return False
			End If

			currentItem = ShellObjectFactory.Create(item, nativeShellFolder)

			Return True
		End Function

		''' <summary>
		''' 
		''' </summary>
		Public Sub Reset() Implements IEnumerator.Reset
			If nativeEnumIdList IsNot Nothing Then
				nativeEnumIdList.Reset()
			End If
		End Sub


		#End Region
	End Class
End Namespace
