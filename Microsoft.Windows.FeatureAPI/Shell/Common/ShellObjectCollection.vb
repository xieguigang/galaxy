'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Resources

Namespace Shell
	''' <summary>
	''' An ennumerable list of ShellObjects
	''' </summary>
	Public Class ShellObjectCollection
		Implements IEnumerable
		Implements IDisposable
		Implements IList(Of ShellObject)
		Private content As New List(Of ShellObject)()

		Private [readOnly] As Boolean
		Private isDisposed As Boolean

		#Region "construction/disposal/finialization"
		''' <summary>
		''' Creates a ShellObject collection from an IShellItemArray
		''' </summary>
		''' <param name="iArray">IShellItemArray pointer</param>
		''' <param name="readOnly">Indicates whether the collection shouldbe read-only or not</param>
		Friend Sub New(iArray As IShellItemArray, [readOnly] As Boolean)
			Me.[readOnly] = [readOnly]

			If iArray IsNot Nothing Then
				Try
					Dim itemCount As UInteger = 0
					iArray.GetCount(itemCount)
					content.Capacity = CInt(itemCount)
                    For index As UInteger = 0 To itemCount - 1UI
                        Dim iShellItem As IShellItem = Nothing
                        iArray.GetItemAt(index, iShellItem)
                        content.Add(ShellObjectFactory.Create(iShellItem))
                    Next
                Finally
					Marshal.ReleaseComObject(iArray)
				End Try
			End If
		End Sub

		''' <summary>
		''' Creates a ShellObjectCollection from an IDataObject passed during Drop operation.
		''' </summary>
		''' <param name="dataObject">An object that implements the IDataObject COM interface.</param>
		''' <returns>ShellObjectCollection created from the given IDataObject</returns>
		Public Shared Function FromDataObject(dataObject As System.Runtime.InteropServices.ComTypes.IDataObject) As ShellObjectCollection
			Dim shellItemArray As IShellItemArray
			Dim iid As New Guid(ShellIIDGuid.IShellItemArray)
			ShellNativeMethods.SHCreateShellItemArrayFromDataObject(dataObject, iid, shellItemArray)
			Return New ShellObjectCollection(shellItemArray, True)
		End Function

        ''' <summary>
        ''' Constructs an empty ShellObjectCollection	' Left empty
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		''' <summary>
		''' Standard Dispose pattern
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Standard Dispose patterns 
		''' </summary>
		''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			If isDisposed = False Then
				If disposing Then
					For Each shellObject As ShellObject In content
						shellObject.Dispose()
					Next

					content.Clear()
				End If

				isDisposed = True
			End If
		End Sub
		#End Region

		#Region "implementation"

		''' <summary>
		''' Item count
		''' </summary>
		Public ReadOnly Property Count() As Integer
			Get
				Return content.Count
			End Get
		End Property

        ''' <summary>
        ''' Collection enumeration
        ''' </summary>
        ''' <returns></returns>
        Public Iterator Function GetEnumerator() As System.Collections.IEnumerator Implements IEnumerable.GetEnumerator
            For Each obj As ShellObject In content
                Yield obj
            Next
        End Function

        ''' <summary>
        ''' Builds the data for the CFSTR_SHELLIDLIST Drag and Clipboard data format from the 
        ''' ShellObjects in the collection.
        ''' </summary>
        ''' <returns>A memory stream containing the drag/drop data.</returns>
        Public Function BuildShellIDList() As MemoryStream
			If content.Count = 0 Then
				Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionEmptyCollection)
			End If


			Dim mstream As New MemoryStream()
			Try
				Dim bwriter As New BinaryWriter(mstream)


				' number of IDLs to be written (shell objects + parent folder)
				Dim itemCount As UInteger = CUInt(content.Count + 1)

                ' grab the object IDLs        
                Dim Len As Integer = CInt(itemCount) - 1
                Dim idls As IntPtr() = New IntPtr(Len) {}

                For index As Integer = 0 To Len
                    If index = 0 Then
                        ' Because the ShellObjects passed in may be from anywhere, the 
                        ' parent folder reference must be the desktop.
                        idls(index) = DirectCast(KnownFolders.Desktop, ShellObject).PIDL
                    Else
                        idls(index) = content(index - 1).PIDL
                    End If
                Next

                ' calculate offset array (folder IDL + item IDLs)
                Dim offsets As UInteger() = New UInteger(Len + 1) {}
                For index As Integer = 0 To Len
                    If index = 0 Then
                        ' first offset equals size of CIDA header data
                        offsets(0) = CUInt(4 * (offsets.Length + 1))
                    Else
                        offsets(index) = offsets(index - 1) + ShellNativeMethods.ILGetSize(idls(index - 1))
                    End If
                Next

                ' Fill out the CIDA header
                '
                '    typedef struct _IDA {
                '    UINT cidl;          // number of relative IDList
                '    UINT aoffset[1];    // [0]: folder IDList, [1]-[cidl]: item IDList
                '    } CIDA, * LPIDA;
                '
                bwriter.Write(content.Count)
				For Each offset As UInteger In offsets
					bwriter.Write(offset)
				Next

                ' copy idls
                For Each idl As IntPtr In idls
                    Dim Len_idl As Integer = CInt(ShellNativeMethods.ILGetSize(idl)) - 1
                    Dim data As Byte() = New Byte(Len_idl) {}
                    Marshal.Copy(idl, data, 0, data.Length)
                    bwriter.Write(data, 0, data.Length)
                Next
            Catch
				mstream.Dispose()
				Throw
			End Try
			' return CIDA stream 
			Return mstream
		End Function
#End Region

#Region "IList<ShellObject> Members"

        ''' <summary>
        ''' Returns the index of a particualr shell object in the collection
        ''' </summary>
        ''' <param name="item">The item to search for.</param>
        ''' <returns>The index of the item found, or -1 if not found.</returns>
        Public Function IndexOf(item As ShellObject) As Integer Implements IList(Of ShellObject).IndexOf
            Return content.IndexOf(item)
        End Function

        ''' <summary>
        ''' Inserts a new shell object into the collection.
        ''' </summary>
        ''' <param name="index">The index at which to insert.</param>
        ''' <param name="item">The item to insert.</param>
        Public Sub Insert(index As Integer, item As ShellObject) Implements IList(Of ShellObject).Insert
            If [readOnly] Then
                Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly)
            End If

            content.Insert(index, item)
        End Sub

        ''' <summary>
        ''' Removes the specified ShellObject from the collection
        ''' </summary>
        ''' <param name="index">The index to remove at.</param>
        Public Sub RemoveAt(index As Integer) Implements IList(Of ShellObject).RemoveAt
			If [readOnly] Then
				Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly)
			End If

			content.RemoveAt(index)
		End Sub

		''' <summary>
		''' The collection indexer
		''' </summary>
		''' <param name="index">The index of the item to retrieve.</param>
		''' <returns>The ShellObject at the specified index</returns>
		Public Default Property Item(index As Integer) As ShellObject Implements IList(Of ShellObject).Item
			Get
				Return content(index)
			End Get
			Set
				If [readOnly] Then
					Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly)
				End If

				content(index) = value
			End Set
		End Property

#End Region

#Region "ICollection<ShellObject> Members"

        ''' <summary>
        ''' Adds a ShellObject to the collection,
        ''' </summary>
        ''' <param name="item">The ShellObject to add.</param>
        Public Sub Add(item As ShellObject) Implements IList(Of ShellObject).Add
            If [readOnly] Then
                Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly)
            End If

            content.Add(item)
        End Sub

        ''' <summary>
        ''' Clears the collection of ShellObjects.
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of ShellObject).Clear
			If [readOnly] Then
				Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly)
			End If

			content.Clear()
		End Sub

        ''' <summary>
        ''' Determines if the collection contains a particular ShellObject.
        ''' </summary>
        ''' <param name="item">The ShellObject.</param>
        ''' <returns>true, if the ShellObject is in the list, false otherwise.</returns>
        Public Function Contains(item As ShellObject) As Boolean Implements IList(Of ShellObject).Contains
            Return content.Contains(item)
        End Function

        ''' <summary>
        ''' Copies the ShellObjects in the collection to a ShellObject array.
        ''' </summary>
        ''' <param name="array">The destination to copy to.</param>
        ''' <param name="arrayIndex">The index into the array at which copying will commence.</param>
        Public Sub CopyTo(array As ShellObject(), arrayIndex As Integer) Implements IList(Of ShellObject).CopyTo
            If array Is Nothing Then
                Throw New ArgumentNullException("array")
            End If
            If array.Length < arrayIndex + content.Count Then
                Throw New ArgumentException(LocalizedMessages.ShellObjectCollectionArrayTooSmall, "array")
            End If

            For index As Integer = 0 To content.Count - 1
                array(index + arrayIndex) = content(index)
            Next
        End Sub

        ''' <summary>
        ''' Retrieves the number of ShellObjects in the collection
        ''' </summary>
        Private ReadOnly Property ICollection_Count() As Integer Implements ICollection(Of ShellObject).Count
			Get
				Return content.Count
			End Get
		End Property

		''' <summary>
		''' If true, the contents of the collection are immutable.
		''' </summary>
		Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of ShellObject).IsReadOnly
			Get
				Return [readOnly]
			End Get
		End Property

        ''' <summary>
        ''' Removes a particular ShellObject from the list.
        ''' </summary>
        ''' <param name="item">The ShellObject to remove.</param>
        ''' <returns>True if the item could be removed, false otherwise.</returns>
        Public Function Remove(item As ShellObject) As Boolean Implements IList(Of ShellObject).Remove
            If [readOnly] Then
                Throw New InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly)
            End If

            Return content.Remove(item)
        End Function

#End Region

#Region "IEnumerable<ShellObject> Members"

        ''' <summary>
        ''' Allows for enumeration through the list of ShellObjects in the collection.
        ''' </summary>
        ''' <returns>The IEnumerator interface to use for enumeration.</returns>
        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator(Of ShellObject) Implements IEnumerable(Of ShellObject).GetEnumerator
            For Each obj As ShellObject In content
                Yield obj
            Next
        End Function

#End Region
    End Class
End Namespace
