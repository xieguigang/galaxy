'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Security.Cryptography
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Shell
    ''' <summary>
    ''' The base class for all Shell objects in Shell Namespace.
    ''' </summary>
    Public MustInherit Class ShellObject
        Implements IDisposable
        Implements IEquatable(Of ShellObject)

#Region "Public Static Methods"

        ''' <summary>
        ''' Creates a ShellObject subclass given a parsing name.
        ''' For file system items, this method will only accept absolute paths.
        ''' </summary>
        ''' <param name="parsingName">The parsing name of the object.</param>
        ''' <returns>A newly constructed ShellObject object.</returns>
        Public Shared Function FromParsingName(parsingName As String) As ShellObject
            Return ShellObjectFactory.Create(parsingName)
        End Function

        ''' <summary>
        ''' Indicates whether this feature is supported on the current platform.
        ''' </summary>
        Public Shared ReadOnly Property IsPlatformSupported() As Boolean
            Get
                ' We need Windows Vista onwards ...
                Return CoreHelpers.RunningOnVista
            End Get
        End Property

#End Region

#Region "Internal Fields"

        ''' <summary>
        ''' Internal member to keep track of the native IShellItem2
        ''' </summary>
        Friend m_nativeShellItem As IShellItem2

#End Region

#Region "Constructors"

        Friend Sub New()
        End Sub

        Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub

#End Region

#Region "Protected Fields"

        ''' <summary>
        ''' Parsing name for this Object e.g. c:\Windows\file.txt,
        ''' or ::{Some Guid} 
        ''' </summary>
        Private _internalParsingName As String

        ''' <summary>
        ''' A friendly name for this object that' suitable for display
        ''' </summary>
        Private _internalName As String

        ''' <summary>
        ''' PID List (PIDL) for this object
        ''' </summary>
        Private _internalPIDL As IntPtr = IntPtr.Zero

#End Region

#Region "Internal Properties"

        ''' <summary>
        ''' Return the native ShellFolder object as newer IShellItem2
        ''' </summary>
        ''' <exception cref="System.Runtime.InteropServices.ExternalException">If the native object cannot be created.
        ''' The ErrorCode member will contain the external error code.</exception>
        Friend Overridable ReadOnly Property NativeShellItem2() As IShellItem2
            Get
                If m_nativeShellItem Is Nothing AndAlso ParsingName IsNot Nothing Then
                    Dim guid As New Guid(ShellIIDGuid.IShellItem2)
                    Dim retCode As Integer = ShellNativeMethods.SHCreateItemFromParsingName(ParsingName, IntPtr.Zero, guid, m_nativeShellItem)

                    If m_nativeShellItem Is Nothing OrElse Not CoreErrorHelper.Succeeded(retCode) Then
                        Throw New ShellException(GlobalLocalizedMessages.ShellObjectCreationFailed, Marshal.GetExceptionForHR(retCode))
                    End If
                End If
                Return m_nativeShellItem
            End Get
        End Property

        ''' <summary>
        ''' Return the native ShellFolder object
        ''' </summary>
        Friend Overridable ReadOnly Property NativeShellItem() As IShellItem
            Get
                Return NativeShellItem2
            End Get
        End Property

        ''' <summary>
        ''' Gets access to the native IPropertyStore (if one is already
        ''' created for this item and still valid. This is usually done by the
        ''' ShellPropertyWriter class. The reference will be set to null
        ''' when the writer has been closed/commited).
        ''' </summary>
        Friend Property NativePropertyStore() As IPropertyStore
            Get
                Return m_NativePropertyStore
            End Get
            Set
                m_NativePropertyStore = Value
            End Set
        End Property
        Private m_NativePropertyStore As IPropertyStore

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Updates the native shell item that maps to this shell object. This is necessary when the shell item 
        ''' changes after the shell object has been created. Without this method call, the retrieval of properties will
        ''' return stale data. 
        ''' </summary>
        ''' <param name="bindContext">Bind context object</param>
        Public Sub Update(bindContext As IBindCtx)
            Dim hr As HResult = HResult.Ok

            If NativeShellItem2 IsNot Nothing Then
                hr = NativeShellItem2.Update(bindContext)
            End If

            If CoreErrorHelper.Failed(hr) Then
                Throw New ShellException(hr)
            End If
        End Sub

#End Region

#Region "Public Properties"

        Private m_properties As ShellProperties
        ''' <summary>
        ''' Gets an object that allows the manipulation of ShellProperties for this shell item.
        ''' </summary>
        Public ReadOnly Property Properties() As ShellProperties
            Get
                If m_properties Is Nothing Then
                    m_properties = New ShellProperties(Me)
                End If
                Return m_properties
            End Get
        End Property

        ''' <summary>
        ''' Gets the parsing name for this ShellItem.
        ''' </summary>
        Public Overridable Property ParsingName() As String
            Get
                If _internalParsingName Is Nothing AndAlso m_nativeShellItem IsNot Nothing Then
                    _internalParsingName = ShellHelper.GetParsingName(m_nativeShellItem)
                End If
                Return If(_internalParsingName, String.Empty)
            End Get
            Protected Set
                _internalParsingName = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the normal display for this ShellItem.
        ''' </summary>
        Public Overridable Property Name() As String
            Get
                If _internalName Is Nothing AndAlso NativeShellItem IsNot Nothing Then
                    Dim pszString As IntPtr = IntPtr.Zero
                    Dim hr As HResult = NativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.Normal, pszString)
                    If hr = HResult.Ok AndAlso pszString <> IntPtr.Zero Then
                        _internalName = Marshal.PtrToStringAuto(pszString)

                        ' Free the string

                        Marshal.FreeCoTaskMem(pszString)
                    End If
                End If
                Return _internalName
            End Get

            Protected Set
                Me._internalName = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the PID List (PIDL) for this ShellItem.
        ''' </summary>
        Friend Overridable Property PIDL() As IntPtr
            Get
                ' Get teh PIDL for the ShellItem
                If _internalPIDL = IntPtr.Zero AndAlso NativeShellItem IsNot Nothing Then
                    _internalPIDL = ShellHelper.PidlFromShellItem(NativeShellItem)
                End If

                Return _internalPIDL
            End Get
            Set
                Me._internalPIDL = Value
            End Set
        End Property

        ''' <summary>
        ''' Overrides object.ToString()
        ''' </summary>
        ''' <returns>A string representation of the object.</returns>
        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        ''' <summary>
        ''' Returns the display name of the ShellFolder object. DisplayNameType represents one of the 
        ''' values that indicates how the name should look. 
        ''' See <see cref="Shell.DisplayNameType"/>for a list of possible values.
        ''' </summary>
        ''' <param name="displayNameType">A disaply name type.</param>
        ''' <returns>A string.</returns>
        Public Overridable Function GetDisplayName(displayNameType As DisplayNameType) As String
            Dim returnValue As String = Nothing

            Dim hr As HResult = HResult.Ok

            If NativeShellItem2 IsNot Nothing Then
                hr = NativeShellItem2.GetDisplayName(CType(displayNameType, ShellNativeMethods.ShellItemDesignNameOptions), returnValue)
            End If

            If hr <> HResult.Ok Then
                Throw New ShellException(GlobalLocalizedMessages.ShellObjectCannotGetDisplayName, hr)
            End If

            Return returnValue
        End Function

        ''' <summary>
        ''' Gets a value that determines if this ShellObject is a link or shortcut.
        ''' </summary>
        Public ReadOnly Property IsLink() As Boolean
            Get
                Try
                    Dim sfgao As ShellNativeMethods.ShellFileGetAttributesOptions
                    NativeShellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.Link, sfgao)
                    Return (sfgao And ShellNativeMethods.ShellFileGetAttributesOptions.Link) <> 0
                Catch generatedExceptionName As FileNotFoundException
                    Return False
                Catch generatedExceptionName As NullReferenceException
                    ' NativeShellItem is null
                    Return False
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that determines if this ShellObject is a file system object.
        ''' </summary>
        Public ReadOnly Property IsFileSystemObject() As Boolean
            Get
                Try
                    Dim sfgao As ShellNativeMethods.ShellFileGetAttributesOptions
                    NativeShellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem, sfgao)
                    Return (sfgao And ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) <> 0
                Catch generatedExceptionName As FileNotFoundException
                    Return False
                Catch generatedExceptionName As NullReferenceException
                    ' NativeShellItem is null
                    Return False
                End Try
            End Get
        End Property

        Private m_thumbnail As ShellThumbnail
        ''' <summary>
        ''' Gets the thumbnail of the ShellObject.
        ''' </summary>
        Public ReadOnly Property Thumbnail() As ShellThumbnail
            Get
                If m_thumbnail Is Nothing Then
                    m_thumbnail = New ShellThumbnail(Me)
                End If
                Return m_thumbnail
            End Get
        End Property

        Private parentShellObject As ShellObject
        ''' <summary>
        ''' Gets the parent ShellObject.
        ''' Returns null if the object has no parent, i.e. if this object is the Desktop folder.
        ''' </summary>
        Public ReadOnly Property Parent() As ShellObject
            Get
                If parentShellObject Is Nothing AndAlso NativeShellItem2 IsNot Nothing Then
                    Dim parentShellItem As IShellItem
                    Dim hr As HResult = NativeShellItem2.GetParent(parentShellItem)

                    If hr = HResult.Ok AndAlso parentShellItem IsNot Nothing Then
                        parentShellObject = ShellObjectFactory.Create(parentShellItem)
                    ElseIf hr = HResult.NoObject Then
                        ' Should return null if the parent is desktop
                        Return Nothing
                    Else
                        Throw New ShellException(hr)
                    End If
                End If

                Return parentShellObject
            End Get
        End Property


#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Release the native and managed objects
        ''' </summary>
        ''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing Then
                _internalName = Nothing
                _internalParsingName = Nothing
                m_properties = Nothing
                m_thumbnail = Nothing
                parentShellObject = Nothing
            End If

            If m_properties IsNot Nothing Then
                m_properties.Dispose()
            End If

            If _internalPIDL <> IntPtr.Zero Then
                ShellNativeMethods.ILFree(_internalPIDL)
                _internalPIDL = IntPtr.Zero
            End If

            If m_nativeShellItem IsNot Nothing Then
                Marshal.ReleaseComObject(m_nativeShellItem)
                m_nativeShellItem = Nothing
            End If

            If NativePropertyStore IsNot Nothing Then
                Marshal.ReleaseComObject(NativePropertyStore)
                NativePropertyStore = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Release the native objects.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Implement the finalizer.
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

#End Region

#Region "equality and hashing"

        ''' <summary>
        ''' Returns the hash code of the object.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetHashCode() As Integer
            If Not hashValue.HasValue Then
                Dim size As UInteger = ShellNativeMethods.ILGetSize(PIDL)
                If size <> 0 Then
                    Dim Len As Integer = CInt(size) - 1
                    Dim pidlData As Byte() = New Byte(Len) {}
                    Marshal.Copy(PIDL, pidlData, 0, CInt(size))
                    Dim hashData As Byte() = ShellObject.hashProvider.ComputeHash(pidlData)
                    hashValue = BitConverter.ToInt32(hashData, 0)
                Else
                    hashValue = 0

                End If
            End If
            Return hashValue.Value
        End Function
        Private Shared hashProvider As New MD5CryptoServiceProvider()
        Private hashValue As System.Nullable(Of Integer)

        ''' <summary>
        ''' Determines if two ShellObjects are identical.
        ''' </summary>
        ''' <param name="other">The ShellObject to comare this one to.</param>
        ''' <returns>True if the ShellObjects are equal, false otherwise.</returns>
        Public Overloads Function Equals(other As ShellObject) As Boolean Implements IEquatable(Of ShellObject).Equals
            Dim areEqual As Boolean = False

            If other IsNot Nothing Then
                Dim ifirst As IShellItem = Me.NativeShellItem
                Dim isecond As IShellItem = other.NativeShellItem
                If ifirst IsNot Nothing AndAlso isecond IsNot Nothing Then
                    Dim result As Integer = 0
                    Dim hr As HResult = ifirst.Compare(isecond, SICHINTF.SICHINT_ALLFIELDS, result)

                    areEqual = (hr = HResult.Ok) AndAlso (result = 0)
                End If
            End If

            Return areEqual
        End Function

        ''' <summary>
        ''' Returns whether this object is equal to another.
        ''' </summary>
        ''' <param name="obj">The object to compare against.</param>
        ''' <returns>Equality result.</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me.Equals(TryCast(obj, ShellObject))
        End Function

        ''' <summary>
        ''' Implements the == (equality) operator.
        ''' </summary>
        ''' <param name="leftShellObject">First object to compare.</param>
        ''' <param name="rightShellObject">Second object to compare.</param>
        ''' <returns>True if leftShellObject equals rightShellObject; false otherwise.</returns>
        Public Shared Operator =(leftShellObject As ShellObject, rightShellObject As ShellObject) As Boolean
            If DirectCast(leftShellObject, Object) Is Nothing Then
                Return (DirectCast(rightShellObject, Object) Is Nothing)
            End If
            Return leftShellObject.Equals(rightShellObject)
        End Operator

        ''' <summary>
        ''' Implements the != (inequality) operator.
        ''' </summary>
        ''' <param name="leftShellObject">First object to compare.</param>
        ''' <param name="rightShellObject">Second object to compare.</param>
        ''' <returns>True if leftShellObject does not equal leftShellObject; false otherwise.</returns>        
        Public Shared Operator <>(leftShellObject As ShellObject, rightShellObject As ShellObject) As Boolean
            Return Not (leftShellObject = rightShellObject)
        End Operator


#End Region
    End Class
End Namespace
