'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Linq
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Resources

Namespace Shell
    ''' <summary>
    ''' Create and modify search folders.
    ''' </summary>
    Public Class ShellSearchFolder
        Inherits ShellSearchCollection
        ''' <summary>
        ''' Create a simple search folder. Once the appropriate parameters are set, 
        ''' the search folder can be enumerated to get the search results.
        ''' </summary>
        ''' <param name="searchCondition">Specific condition on which to perform the search (property and expected value)</param>
        ''' <param name="searchScopePath">List of folders/paths to perform the search on. These locations need to be indexed by the system.</param>
        Public Sub New(searchCondition As SearchCondition, ParamArray searchScopePath As ShellContainer())
            CoreHelpers.ThrowIfNotVista()

            NativeSearchFolderItemFactory = DirectCast(New SearchFolderItemFactoryCoClass(), ISearchFolderItemFactory)

            Me.SearchCondition = searchCondition

            If searchScopePath IsNot Nothing AndAlso searchScopePath.Length > 0 AndAlso searchScopePath(0) IsNot Nothing Then
                Me.SearchScopePaths = searchScopePath.[Select](Function(cont) cont.ParsingName)
            End If
        End Sub

        ''' <summary>
        ''' Create a simple search folder. Once the appropiate parameters are set, 
        ''' the search folder can be enumerated to get the search results.
        ''' </summary>
        ''' <param name="searchCondition">Specific condition on which to perform the search (property and expected value)</param>
        ''' <param name="searchScopePath">List of folders/paths to perform the search on. These locations need to be indexed by the system.</param>
        Public Sub New(searchCondition As SearchCondition, ParamArray searchScopePath As String())
            CoreHelpers.ThrowIfNotVista()

            NativeSearchFolderItemFactory = DirectCast(New SearchFolderItemFactoryCoClass(), ISearchFolderItemFactory)

            If searchScopePath IsNot Nothing AndAlso searchScopePath.Length > 0 AndAlso searchScopePath(0) IsNot Nothing Then
                Me.SearchScopePaths = searchScopePath
            End If

            Me.SearchCondition = searchCondition
        End Sub

        Friend Property NativeSearchFolderItemFactory() As ISearchFolderItemFactory
            Get
                Return m_NativeSearchFolderItemFactory
            End Get
            Set
                m_NativeSearchFolderItemFactory = Value
            End Set
        End Property
        Private m_NativeSearchFolderItemFactory As ISearchFolderItemFactory

        Private m_searchCondition As SearchCondition
        ''' <summary>
        ''' Gets the <see cref="Shell.SearchCondition"/> of the search. 
        ''' When this property is not set, the resulting search will have no filters applied.
        ''' </summary>
        Public Property SearchCondition() As SearchCondition
            Get
                Return m_searchCondition
            End Get
            Private Set
                m_searchCondition = Value

                NativeSearchFolderItemFactory.SetCondition(m_searchCondition.NativeSearchCondition)
            End Set
        End Property

        Private m_searchScopePaths As String()
        ''' <summary>
        ''' Gets the search scope, as specified using an array of locations to search. 
        ''' The search will include this location and all its subcontainers. The default is FOLDERID_Profile
        ''' </summary>
        Public Iterator Property SearchScopePaths() As IEnumerable(Of String)
            Get
                For Each scopePath As String In m_searchScopePaths
                    Yield scopePath
                Next
            End Get
            Private Set
                m_searchScopePaths = Value.ToArray()
                Dim shellItems As New List(Of IShellItem)(m_searchScopePaths.Length)

                Dim shellItemGuid As New Guid(ShellIIDGuid.IShellItem)
                Dim shellItemArrayGuid As New Guid(ShellIIDGuid.IShellItemArray)

                ' Create IShellItem for all the scopes we were given
                For Each path As String In m_searchScopePaths
                    Dim scopeShellItem As IShellItem

                    Dim hr As Integer = ShellNativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, shellItemGuid, scopeShellItem)

                    If CoreErrorHelper.Succeeded(hr) Then
                        shellItems.Add(scopeShellItem)
                    End If
                Next

                ' Create a new IShellItemArray
                Dim scopeShellItemArray As IShellItemArray = New ShellItemArray(shellItems.ToArray())

                ' Set the scope on the native ISearchFolderItemFactory
                Dim hResult As HResult = NativeSearchFolderItemFactory.SetScope(scopeShellItemArray)

                If Not CoreErrorHelper.Succeeded(CInt(hResult)) Then
                    Throw New ShellException(CInt(hResult))
                End If
            End Set
        End Property

        Friend Overrides ReadOnly Property NativeShellItem() As IShellItem
            Get
                Dim guid As New Guid(ShellIIDGuid.IShellItem)

                If NativeSearchFolderItemFactory Is Nothing Then
                    Return Nothing
                End If

                Dim shellItem As IShellItem
                Dim hr As Integer = NativeSearchFolderItemFactory.GetShellItem(guid, shellItem)

                If Not CoreErrorHelper.Succeeded(hr) Then
                    Throw New ShellException(hr)
                End If

                Return shellItem
            End Get
        End Property


        ''' <summary>
        ''' Creates a list of stack keys, as specified. If this method is not called, 
        ''' by default the folder will not be stacked.
        ''' </summary>
        ''' <param name="canonicalNames">Array of canonical names for properties on which the folder is stacked.</param>
        ''' <exception cref="System.ArgumentException">If one of the given canonical names is invalid.</exception>
        Public Sub SetStacks(ParamArray canonicalNames As String())
            If canonicalNames Is Nothing Then
                Throw New ArgumentNullException("canonicalNames")
            End If
            Dim propertyKeyList As New List(Of PropertyKey)()

            For Each prop As String In canonicalNames
                ' Get the PropertyKey using the canonicalName passed in
                Dim propKey As PropertyKey
                Dim result As Integer = PropertySystemNativeMethods.PSGetPropertyKeyFromName(prop, propKey)

                If Not CoreErrorHelper.Succeeded(result) Then
                    Throw New ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, "canonicalNames", Marshal.GetExceptionForHR(result))
                End If

                propertyKeyList.Add(propKey)
            Next

            If propertyKeyList.Count > 0 Then
                SetStacks(propertyKeyList.ToArray())
            End If
        End Sub

        ''' <summary>
        ''' Creates a list of stack keys, as specified. If this method is not called, 
        ''' by default the folder will not be stacked.
        ''' </summary>
        ''' <param name="propertyKeys">Array of property keys on which the folder is stacked.</param>
        Public Sub SetStacks(ParamArray propertyKeys As PropertyKey())
            If propertyKeys IsNot Nothing AndAlso propertyKeys.Length > 0 Then
                NativeSearchFolderItemFactory.SetStacks(CUInt(propertyKeys.Length), propertyKeys)
            End If
        End Sub

        ''' <summary>
        ''' Sets the search folder display name.
        ''' </summary>
        Public Sub SetDisplayName(displayName As String)
            Dim hr As HResult = NativeSearchFolderItemFactory.SetDisplayName(displayName)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub



        ''' <summary>
        ''' Sets the search folder icon size.
        ''' The default settings are based on the FolderTypeID which is set by the 
        ''' SearchFolder::SetFolderTypeID method.
        ''' </summary>
        Public Sub SetIconSize(value As Integer)
            Dim hr As HResult = NativeSearchFolderItemFactory.SetIconSize(value)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub

        ''' <summary>
        ''' Sets a search folder type ID, as specified. 
        ''' </summary>
        Public Sub SetFolderTypeID(value As Guid)
            Dim hr As HResult = NativeSearchFolderItemFactory.SetFolderTypeID(value)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub

        ''' <summary>
        ''' Sets folder logical view mode. The default settings are based on the FolderTypeID which is set 
        ''' by the SearchFolder::SetFolderTypeID method.        
        ''' </summary>
        ''' <param name="mode">The logical view mode to set.</param>
        Public Sub SetFolderLogicalViewMode(mode As FolderLogicalViewMode)
            Dim hr As HResult = NativeSearchFolderItemFactory.SetFolderLogicalViewMode(mode)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub

        ''' <summary>
        ''' Creates a new column list whose columns are all visible, 
        ''' given an array of PropertyKey structures. The default is based on FolderTypeID.
        ''' </summary>
        ''' <remarks>This property may not work correctly with the ExplorerBrowser control.</remarks>
        Public Sub SetVisibleColumns(value As PropertyKey())
            Dim hr As HResult = NativeSearchFolderItemFactory.SetVisibleColumns(If(value Is Nothing, 0UI, CUInt(value.Length)), value)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(LocalizedMessages.ShellSearchFolderUnableToSetVisibleColumns, Marshal.GetExceptionForHR(CInt(hr)))
            End If
        End Sub

        ''' <summary>
        ''' Creates a list of sort column directions, as specified.
        ''' </summary>
        ''' <remarks>This property may not work correctly with the ExplorerBrowser control.</remarks>
        Public Sub SortColumns(value As SortColumn())
            Dim hr As HResult = NativeSearchFolderItemFactory.SetSortColumns(If(value Is Nothing, 0UI, CUInt(value.Length)), value)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(LocalizedMessages.ShellSearchFolderUnableToSetSortColumns, Marshal.GetExceptionForHR(CInt(hr)))
            End If
        End Sub

        ''' <summary>
        ''' Sets a group column, as specified. If no group column is specified, no grouping occurs. 
        ''' </summary>
        ''' <remarks>This property may not work correctly with the ExplorerBrowser control.</remarks>
        Public Sub SetGroupColumn(propertyKey As PropertyKey)
            Dim hr As HResult = NativeSearchFolderItemFactory.SetGroupColumn(propertyKey)

            If Not CoreErrorHelper.Succeeded(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub
    End Class
End Namespace
