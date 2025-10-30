'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Diagnostics.CodeAnalysis
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Internal

    ''' <summary>
    ''' Represents the OLE struct PROPVARIANT.
    ''' This class is intended for internal use only.
    ''' </summary>
    ''' <remarks>
    ''' Originally sourced from http://blogs.msdn.com/adamroot/pages/interop-with-propvariants-in-net.aspx
    ''' and modified to support additional types including vectors and ability to set values
    ''' </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId:="_ptr2")>
    <StructLayout(LayoutKind.Explicit)>
    Public NotInheritable Class PropVariant
        Implements IDisposable

#Region "Vector Action Cache"

        ' A static dictionary of delegates to get data from array's contained within PropVariants
        Private Shared _vectorActions As Dictionary(Of Type, Action(Of PropVariant, Array, UInteger)) = Nothing

        <SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
        Private Shared Function GenerateVectorActions() As Dictionary(Of Type, Action(Of PropVariant, Array, UInteger))
            Dim cache As New Dictionary(Of Type, Action(Of PropVariant, Array, UInteger))()

            cache.Add(GetType(Int16), Sub(pv, array, i)
                                          Dim val As Short
                                          PropVariantNativeMethods.PropVariantGetInt16Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt16), Sub(pv, array, i)
                                           Dim val As UShort
                                           PropVariantNativeMethods.PropVariantGetUInt16Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(Int32), Sub(pv, array, i)
                                          Dim val As Integer
                                          PropVariantNativeMethods.PropVariantGetInt32Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt32), Sub(pv, array, i)
                                           Dim val As UInteger
                                           PropVariantNativeMethods.PropVariantGetUInt32Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(Int64), Sub(pv, array, i)
                                          Dim val As Long
                                          PropVariantNativeMethods.PropVariantGetInt64Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt64), Sub(pv, array, i)
                                           Dim val As ULong
                                           PropVariantNativeMethods.PropVariantGetUInt64Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(DateTime), Sub(pv, array, i)
                                             Dim val As System.Runtime.InteropServices.ComTypes.FILETIME
                                             PropVariantNativeMethods.PropVariantGetFileTimeElem(pv, i, val)

                                             Dim fileTime As Long = GetFileTimeAsLong(val)

                                             array.SetValue(DateTime.FromFileTime(fileTime), i)

                                         End Sub)

            cache.Add(GetType([Boolean]), Sub(pv, array, i)
                                              Dim val As Boolean
                                              PropVariantNativeMethods.PropVariantGetBooleanElem(pv, i, val)
                                              array.SetValue(val, i)

                                          End Sub)

            cache.Add(GetType([Double]), Sub(pv, array, i)
                                             Dim val As Double
                                             PropVariantNativeMethods.PropVariantGetDoubleElem(pv, i, val)
                                             array.SetValue(val, i)

                                         End Sub)

            cache.Add(GetType([Single]), Sub(pv, array, i)
                                             ' float
                                             Dim val As Single() = New Single(0) {}
                                             Marshal.Copy(pv._ptr2, val, CInt(i), 1)
                                             array.SetValue(val(0), CInt(i))

                                         End Sub)

            cache.Add(GetType([Decimal]), Sub(pv, array, i)
                                              Dim val As Integer() = New Integer(3) {}
                                              For a As Integer = 0 To val.Length - 1
                                                  'index * size + offset quarter
                                                  val(a) = Marshal.ReadInt32(pv._ptr2, CInt(i) * Marshal.SizeOf(Of Decimal) + a * 4)
                                              Next
                                              array.SetValue(New Decimal(val), i)

                                          End Sub)

            cache.Add(GetType([String]), Sub(pv, array, i)
                                             Dim val As String = String.Empty
                                             PropVariantNativeMethods.PropVariantGetStringElem(pv, i, val)
                                             array.SetValue(val, i)

                                         End Sub)

            Return cache
        End Function
#End Region

#Region "Dynamic Construction / Factory (Expressions)"

        ''' <summary>
        ''' Attempts to create a PropVariant by finding an appropriate constructor.
        ''' </summary>
        ''' <param name="value">Object from which PropVariant should be created.</param>
        Public Shared Function FromObject(value As Object) As PropVariant
            If value Is Nothing Then
                Return New PropVariant()
            Else
                Dim func As Func(Of Object, PropVariant) = GetDynamicConstructor(value.[GetType]())
                Return func(value)
            End If
        End Function

        ' A dictionary and lock to contain compiled expression trees for constructors
        Private Shared _cache As New Dictionary(Of Type, Func(Of Object, PropVariant))()
        Private Shared _padlock As New Object()

        ' Retrieves a cached constructor expression.
        ' If no constructor has been cached, it attempts to find/add it.  If it cannot be found
        ' an exception is thrown.
        ' This method looks for a public constructor with the same parameter type as the object.
        Private Shared Function GetDynamicConstructor(type As Type) As Func(Of Object, PropVariant)
            SyncLock _padlock
                ' initial check, if action is found, return it
                Dim action As Func(Of Object, PropVariant)
                If Not _cache.TryGetValue(type, action) Then
                    ' iterates through all constructors
                    Dim constructor As ConstructorInfo = GetType(PropVariant).GetConstructor(New Type() {type})

                    If constructor Is Nothing Then
                        ' if the method was not found, throw.
                        Throw New ArgumentException(LocalizedMessages.PropVariantTypeNotSupported)
                    Else
                        ' if the method was found, create an expression to call it.
                        ' create parameters to action                    
                        Dim arg As ParameterExpression = Expression.Parameter(GetType(Object), "arg")

                        ' create an expression to invoke the constructor with an argument cast to the correct type
                        Dim create As NewExpression = Expression.[New](constructor, Expression.Convert(arg, type))

                        ' compiles expression into an action delegate
                        action = Expression.Lambda(Of Func(Of Object, PropVariant))(create, arg).Compile()
                        _cache.Add(type, action)
                    End If
                End If
                Return action
            End SyncLock
        End Function

#End Region

#Region "Fields"

        <FieldOffset(0)>
        Private _decimal As Decimal

        ' This is actually a VarEnum value, but the VarEnum type
        ' requires 4 bytes instead of the expected 2.
        <FieldOffset(0)>
        Private _valueType As UShort

        ' Reserved Fields
        '[FieldOffset(2)]
        'ushort _wReserved1;
        '[FieldOffset(4)]
        'ushort _wReserved2;
        '[FieldOffset(6)]
        'ushort _wReserved3;

        ' In order to allow x64 compat, we need to allow for
        ' expansion of the IntPtr. However, the BLOB struct
        ' uses a 4-byte int, followed by an IntPtr, so
        ' although the valueData field catches most pointer values,
        ' we need an additional 4-bytes to get the BLOB
        ' pointer. The valueDataExt field provides this, as well as
        ' the last 4-bytes of an 8-byte value on 32-bit
        ' architectures.
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")>
        <FieldOffset(12)>
        Private _ptr2 As IntPtr
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")>
        <FieldOffset(8)>
        Private _ptr As IntPtr
        <FieldOffset(8)>
        Private _int32 As Int32
        <FieldOffset(8)>
        Private _uint32 As UInt32
        <FieldOffset(8)>
        Private _byte As Byte
        <FieldOffset(8)>
        Private _sbyte As SByte
        <FieldOffset(8)>
        Private _short As Short
        <FieldOffset(8)>
        Private _ushort As UShort
        <FieldOffset(8)>
        Private _long As Long
        <FieldOffset(8)>
        Private _ulong As ULong
        <FieldOffset(8)>
        Private _double As Double
        <FieldOffset(8)>
        Private _float As Single

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constrcutor	' left empty
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Set a string value
        ''' </summary>
        Public Sub New(value As String)
            If value Is Nothing Then
                Throw New ArgumentException(LocalizedMessages.PropVariantNullString, "value")
            End If

            _valueType = CUShort(VarEnum.VT_LPWSTR)
            _ptr = Marshal.StringToCoTaskMemUni(value)
        End Sub

        ''' <summary>
        ''' Set a string vector
        ''' </summary>
        Public Sub New(value As String())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromStringVector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a bool vector
        ''' </summary>
        Public Sub New(value As Boolean())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromBooleanVector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a short vector
        ''' </summary>
        Public Sub New(value As Short())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt16Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a short vector
        ''' </summary>
        Public Sub New(value As UShort())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If


            PropVariantNativeMethods.InitPropVariantFromUInt16Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set an int vector
        ''' </summary>
        Public Sub New(value As Integer())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt32Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set an uint vector
        ''' </summary>
        Public Sub New(value As UInteger())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromUInt32Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a long vector
        ''' </summary>
        Public Sub New(value As Long())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt64Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a ulong vector
        ''' </summary>
        Public Sub New(value As ULong())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromUInt64Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>>
        ''' Set a double vector
        ''' </summary>
        Public Sub New(value As Double())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromDoubleVector(value, CUInt(value.Length), Me)
        End Sub


        ''' <summary>
        ''' Set a DateTime vector
        ''' </summary>
        Public Sub New(value As DateTime())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            Dim fileTimeArr As System.Runtime.InteropServices.ComTypes.FILETIME() = New System.Runtime.InteropServices.ComTypes.FILETIME(value.Length - 1) {}

            For i As Integer = 0 To value.Length - 1
                fileTimeArr(i) = DateTimeToFileTime(value(i))
            Next

            PropVariantNativeMethods.InitPropVariantFromFileTimeVector(fileTimeArr, CUInt(fileTimeArr.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a bool value
        ''' </summary>
        Public Sub New(value As Boolean)
            _valueType = CUShort(VarEnum.VT_BOOL)
            _int32 = If((value = True), -1, 0)
        End Sub

        ''' <summary>
        ''' Set a DateTime value
        ''' </summary>
        Public Sub New(value As DateTime)
            _valueType = CUShort(VarEnum.VT_FILETIME)

            Dim ft As System.Runtime.InteropServices.ComTypes.FILETIME = DateTimeToFileTime(value)
            PropVariantNativeMethods.InitPropVariantFromFileTime(ft, Me)
        End Sub


        ''' <summary>
        ''' Set a byte value
        ''' </summary>
        Public Sub New(value As Byte)
            _valueType = CUShort(VarEnum.VT_UI1)
            _byte = value
        End Sub

        ''' <summary>
        ''' Set a sbyte value
        ''' </summary>
        Public Sub New(value As SByte)
            _valueType = CUShort(VarEnum.VT_I1)
            _sbyte = value
        End Sub

        ''' <summary>
        ''' Set a short value
        ''' </summary>
        Public Sub New(value As Short)
            _valueType = CUShort(VarEnum.VT_I2)
            _short = value
        End Sub

        ''' <summary>
        ''' Set an unsigned short value
        ''' </summary>
        Public Sub New(value As UShort)
            _valueType = CUShort(VarEnum.VT_UI2)
            _ushort = value
        End Sub

        ''' <summary>
        ''' Set an int value
        ''' </summary>
        Public Sub New(value As Integer)
            _valueType = CUShort(VarEnum.VT_I4)
            _int32 = value
        End Sub

        ''' <summary>
        ''' Set an unsigned int value
        ''' </summary>
        Public Sub New(value As UInteger)
            _valueType = CUShort(VarEnum.VT_UI4)
            _uint32 = value
        End Sub

        ''' <summary>
        ''' Set a decimal  value
        ''' </summary>
        Public Sub New(value As Decimal)
            _decimal = value

            ' It is critical that the value type be set after the decimal value, because they overlap.
            ' If valuetype is written first, its value will be lost when _decimal is written.
            _valueType = CUShort(VarEnum.VT_DECIMAL)
        End Sub

        ''' <summary>
        ''' Create a PropVariant with a contained decimal array.
        ''' </summary>
        ''' <param name="value">Decimal array to wrap.</param>
        Public Sub New(value As Decimal())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            _valueType = CUShort(VarEnum.VT_DECIMAL Or VarEnum.VT_VECTOR)
            _int32 = value.Length

            ' allocate required memory for array with 128bit elements
            _ptr2 = Marshal.AllocCoTaskMem(value.Length * Marshal.SizeOf(Of Decimal))
            For i As Integer = 0 To value.Length - 1
                Dim bits As Integer() = Decimal.GetBits(value(i))
                Marshal.Copy(bits, 0, _ptr2, bits.Length)
            Next
        End Sub

        ''' <summary>
        ''' Create a PropVariant containing a float type.
        ''' </summary>        
        Public Sub New(value As Single)
            _valueType = CUShort(VarEnum.VT_R4)

            _float = value
        End Sub

        ''' <summary>
        ''' Creates a PropVariant containing a float[] array.
        ''' </summary>        
        Public Sub New(value As Single())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            _valueType = CUShort(VarEnum.VT_R4 Or VarEnum.VT_VECTOR)
            _int32 = value.Length

            _ptr2 = Marshal.AllocCoTaskMem(value.Length * 4)

            Marshal.Copy(value, 0, _ptr2, value.Length)
        End Sub

        ''' <summary>
        ''' Set a long
        ''' </summary>
        Public Sub New(value As Long)
            _long = value
            _valueType = CUShort(VarEnum.VT_I8)
        End Sub

        ''' <summary>
        ''' Set a ulong
        ''' </summary>
        Public Sub New(value As ULong)
            _valueType = CUShort(VarEnum.VT_UI8)
            _ulong = value
        End Sub

        ''' <summary>
        ''' Set a double
        ''' </summary>
        Public Sub New(value As Double)
            _valueType = CUShort(VarEnum.VT_R8)
            _double = value
        End Sub

#End Region

#Region "Uncalled methods - These are currently not called, but I think may be valid in the future."

        ''' <summary>
        ''' Set an IUnknown value
        ''' </summary>
        ''' <param name="value">The new value to set.</param>
        Friend Sub SetIUnknown(value As Object)
            _valueType = CUShort(VarEnum.VT_UNKNOWN)
            _ptr = Marshal.GetIUnknownForObject(value)
        End Sub


        ''' <summary>
        ''' Set a safe array value
        ''' </summary>
        ''' <param name="array">The new value to set.</param>
        Friend Sub SetSafeArray(array As Array)
            If array Is Nothing Then
                Throw New ArgumentNullException("array")
            End If
            Const vtUnknown As UShort = 13
            Dim psa As IntPtr = PropVariantNativeMethods.SafeArrayCreateVector(vtUnknown, 0, CUInt(array.Length))

            Dim pvData As IntPtr = PropVariantNativeMethods.SafeArrayAccessData(psa)
            Try
                ' to remember to release lock on data
                For i As Integer = 0 To array.Length - 1
                    Dim obj As Object = array.GetValue(i)
                    Dim punk As IntPtr = If((obj IsNot Nothing), Marshal.GetIUnknownForObject(obj), IntPtr.Zero)
                    Marshal.WriteIntPtr(pvData, i * IntPtr.Size, punk)
                Next
            Finally
                PropVariantNativeMethods.SafeArrayUnaccessData(psa)
            End Try

            _valueType = CUShort(VarEnum.VT_ARRAY) Or CUShort(VarEnum.VT_UNKNOWN)
            _ptr = psa
        End Sub

#End Region

#Region "public Properties"

        ''' <summary>
        ''' Gets or sets the variant type.
        ''' </summary>
        Public Property VarType() As VarEnum
            Get
                Return CType(_valueType, VarEnum)
            End Get
            Set
                _valueType = CUShort(Value)
            End Set
        End Property

        ''' <summary>
        ''' Checks if this has an empty or null value
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsNullOrEmpty() As Boolean
            Get
                Return (_valueType = CUShort(VarEnum.VT_EMPTY) OrElse _valueType = CUShort(VarEnum.VT_NULL))
            End Get
        End Property

        ''' <summary>
        ''' Gets the variant value.
        ''' </summary>
        Public ReadOnly Property Value() As Object
            Get
                Select Case CType(_valueType, VarEnum)
                    Case VarEnum.VT_I1
                        Return _sbyte
                    Case VarEnum.VT_UI1
                        Return _byte
                    Case VarEnum.VT_I2
                        Return _short
                    Case VarEnum.VT_UI2
                        Return _ushort
                    Case VarEnum.VT_I4, VarEnum.VT_INT
                        Return _int32
                    Case VarEnum.VT_UI4, VarEnum.VT_UINT
                        Return _uint32
                    Case VarEnum.VT_I8
                        Return _long
                    Case VarEnum.VT_UI8
                        Return _ulong
                    Case VarEnum.VT_R4
                        Return _float
                    Case VarEnum.VT_R8
                        Return _double
                    Case VarEnum.VT_BOOL
                        Return _int32 = -1
                    Case VarEnum.VT_ERROR
                        Return _long
                    Case VarEnum.VT_CY
                        Return _decimal
                    Case VarEnum.VT_DATE
                        Return DateTime.FromOADate(_double)
                    Case VarEnum.VT_FILETIME
                        Return DateTime.FromFileTime(_long)
                    Case VarEnum.VT_BSTR
                        Return Marshal.PtrToStringBSTR(_ptr)
                    Case VarEnum.VT_BLOB
                        Return GetBlobData()
                    Case VarEnum.VT_LPSTR
                        Return Marshal.PtrToStringAnsi(_ptr)
                    Case VarEnum.VT_LPWSTR
                        Return Marshal.PtrToStringUni(_ptr)
                    Case VarEnum.VT_UNKNOWN
                        Return Marshal.GetObjectForIUnknown(_ptr)
                    Case VarEnum.VT_DISPATCH
                        Return Marshal.GetObjectForIUnknown(_ptr)
                    Case VarEnum.VT_DECIMAL
                        Return _decimal
                    Case VarEnum.VT_ARRAY Or VarEnum.VT_UNKNOWN
                        Return CrackSingleDimSafeArray(_ptr)
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_LPWSTR)
                        Return GetVector(Of String)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I2)
                        Return GetVector(Of Int16)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI2)
                        Return GetVector(Of UInt16)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I4)
                        Return GetVector(Of Int32)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI4)
                        Return GetVector(Of UInt32)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I8)
                        Return GetVector(Of Int64)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI8)
                        Return GetVector(Of UInt64)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_R4)
                        Return GetVector(Of Single)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_R8)
                        Return GetVector(Of [Double])()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_BOOL)
                        Return GetVector(Of [Boolean])()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_FILETIME)
                        Return GetVector(Of DateTime)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_DECIMAL)
                        Return GetVector(Of [Decimal])()
                    Case Else
                        ' if the value cannot be marshaled
                        Return Nothing
                End Select
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Shared Function GetFileTimeAsLong(ByRef val As System.Runtime.InteropServices.ComTypes.FILETIME) As Long
            Return (CLng(val.dwHighDateTime) << 32) + val.dwLowDateTime
        End Function

        Private Shared Function DateTimeToFileTime(value As DateTime) As System.Runtime.InteropServices.ComTypes.FILETIME
            Dim hFT As Long = value.ToFileTime()
            Dim ft As New System.Runtime.InteropServices.ComTypes.FILETIME()
            ft.dwLowDateTime = CInt(hFT And &HFFFFFFFFUI)
            ft.dwHighDateTime = CInt(hFT >> 32)
            Return ft
        End Function

        Private Function GetBlobData() As Object
            Dim blobData As Byte() = New Byte(_int32 - 1) {}

            Dim pBlobData As IntPtr = _ptr2
            Marshal.Copy(pBlobData, blobData, 0, _int32)

            Return blobData
        End Function

        Private Function GetVector(Of T)() As Array
            Dim count As Integer = PropVariantNativeMethods.PropVariantGetElementCount(Me)
            If count <= 0 Then
                Return Nothing
            End If

            SyncLock _padlock
                If _vectorActions Is Nothing Then
                    _vectorActions = GenerateVectorActions()
                End If
            End SyncLock

            Dim action As Action(Of PropVariant, Array, UInteger)
            If Not _vectorActions.TryGetValue(GetType(T), action) Then
                Throw New InvalidCastException(LocalizedMessages.PropVariantUnsupportedType)
            End If

            Dim array As Array = New T(count - 1) {}
            Dim Length As UInteger = CUInt(CLng(count) - 1)

            For i As UInteger = 0 To Length
                action(Me, array, i)
            Next

            Return array
        End Function

        Private Shared Function CrackSingleDimSafeArray(psa As IntPtr) As Array
            Dim cDims As UInteger = PropVariantNativeMethods.SafeArrayGetDim(psa)
            If cDims <> 1 Then
                Throw New ArgumentException(LocalizedMessages.PropVariantMultiDimArray, "psa")
            End If

            Dim lBound As Integer = PropVariantNativeMethods.SafeArrayGetLBound(psa, 1UI)
            Dim uBound As Integer = PropVariantNativeMethods.SafeArrayGetUBound(psa, 1UI)

            Dim n As Integer = uBound - lBound + 1
            ' uBound is inclusive
            Dim array As Object() = New Object(n - 1) {}
            For i As Integer = lBound To uBound
                array(i) = PropVariantNativeMethods.SafeArrayGetElement(psa, i)
            Next

            Return array
        End Function

#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Disposes the object, calls the clear function.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            PropVariantNativeMethods.PropVariantClear(Me)

            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose()
            Finally
                MyBase.Finalize()
            End Try
        End Sub

#End Region

        ''' <summary>
        ''' Provides an simple string representation of the contained data and type.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1}", Value, VarType.ToString())
        End Function

    End Class

End Namespace
