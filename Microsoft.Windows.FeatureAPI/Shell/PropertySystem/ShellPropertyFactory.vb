Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Windows.Resources

Namespace Shell.PropertySystem

	''' <summary>
	''' Factory class for creating typed ShellProperties.
	''' Generates/caches expressions to create generic ShellProperties.
	''' </summary>
	Friend NotInheritable Class ShellPropertyFactory
		Private Sub New()
		End Sub
		' Constructor cache.  It takes object as the third param so a single function will suffice for both constructors.
		Private Shared _storeCache As New Dictionary(Of Integer, Func(Of PropertyKey, ShellPropertyDescription, Object, IShellProperty))()

		''' <summary>
		''' Creates a generic ShellProperty.
		''' </summary>
		''' <param name="propKey">PropertyKey</param>
		''' <param name="shellObject">Shell object from which to get property</param>
		''' <returns>ShellProperty matching type of value in property.</returns>
		Public Shared Function CreateShellProperty(propKey As PropertyKey, shellObject As ShellObject) As IShellProperty
			Return GenericCreateShellProperty(propKey, shellObject)
		End Function

		''' <summary>
		''' Creates a generic ShellProperty.
		''' </summary>
		''' <param name="propKey">PropertyKey</param>
		''' <param name="store">IPropertyStore from which to get property</param>
		''' <returns>ShellProperty matching type of value in property.</returns>
		Public Shared Function CreateShellProperty(propKey As PropertyKey, store As IPropertyStore) As IShellProperty
			Return GenericCreateShellProperty(propKey, store)
		End Function

		Private Shared Function GenericCreateShellProperty(Of T)(propKey As PropertyKey, thirdArg As T) As IShellProperty
			Dim thirdType As Type = If((TypeOf thirdArg Is ShellObject), GetType(ShellObject), GetType(T))

			Dim propDesc As ShellPropertyDescription = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propKey)

			' Get the generic type
			Dim type As Type = GetType(ShellProperty(Of )).MakeGenericType(VarEnumToSystemType(propDesc.VarEnumType))

			' The hash for the function is based off the generic type and which type (constructor) we're using.
			Dim hash As Integer = GetTypeHash(type, thirdType)

			Dim ctor As Func(Of PropertyKey, ShellPropertyDescription, Object, IShellProperty)
			If Not _storeCache.TryGetValue(hash, ctor) Then
				Dim argTypes As Type() = {GetType(PropertyKey), GetType(ShellPropertyDescription), thirdType}
				ctor = ExpressConstructor(type, argTypes)
				_storeCache.Add(hash, ctor)
			End If

			Return ctor(propKey, propDesc, thirdArg)
		End Function

		''' <summary>
		''' Converts VarEnum to its associated .net Type.
		''' </summary>
		''' <param name="VarEnumType">VarEnum value</param>
		''' <returns>Associated .net equivelent.</returns>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")> _
		Public Shared Function VarEnumToSystemType(VarEnumType As VarEnum) As Type
			Select Case VarEnumType
				Case (VarEnum.VT_EMPTY), (VarEnum.VT_NULL)
					Return GetType([Object])
				Case (VarEnum.VT_UI1)
					Return GetType(System.Nullable(Of [Byte]))
				Case (VarEnum.VT_I2)
					Return GetType(System.Nullable(Of Int16))
				Case (VarEnum.VT_UI2)
					Return GetType(System.Nullable(Of UInt16))
				Case (VarEnum.VT_I4)
					Return GetType(System.Nullable(Of Int32))
				Case (VarEnum.VT_UI4)
					Return GetType(System.Nullable(Of UInt32))
				Case (VarEnum.VT_I8)
					Return GetType(System.Nullable(Of Int64))
				Case (VarEnum.VT_UI8)
					Return GetType(System.Nullable(Of UInt64))
				Case (VarEnum.VT_R8)
					Return GetType(System.Nullable(Of [Double]))
				Case (VarEnum.VT_BOOL)
					Return GetType(System.Nullable(Of [Boolean]))
				Case (VarEnum.VT_FILETIME)
					Return GetType(System.Nullable(Of DateTime))
				Case (VarEnum.VT_CLSID)
					Return GetType(System.Nullable(Of IntPtr))
				Case (VarEnum.VT_CF)
					Return GetType(System.Nullable(Of IntPtr))
				Case (VarEnum.VT_BLOB)
					Return GetType([Byte]())
				Case (VarEnum.VT_LPWSTR)
					Return GetType([String])
				Case (VarEnum.VT_UNKNOWN)
					Return GetType(System.Nullable(Of IntPtr))
				Case (VarEnum.VT_STREAM)
					Return GetType(IStream)
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI1)
					Return GetType([Byte]())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_I2)
					Return GetType(Int16())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI2)
					Return GetType(UInt16())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_I4)
					Return GetType(Int32())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI4)
					Return GetType(UInt32())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_I8)
					Return GetType(Int64())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI8)
					Return GetType(UInt64())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_R8)
					Return GetType([Double]())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_BOOL)
					Return GetType([Boolean]())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_FILETIME)
					Return GetType(DateTime())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_CLSID)
					Return GetType(IntPtr())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_CF)
					Return GetType(IntPtr())
				Case (VarEnum.VT_VECTOR Or VarEnum.VT_LPWSTR)
					Return GetType([String]())
				Case Else
					Return GetType([Object])
			End Select
		End Function

		#Region "Private static helper functions"

		' Creates an expression for the specific constructor of the given type.
		Private Shared Function ExpressConstructor(type As Type, argTypes As Type()) As Func(Of PropertyKey, ShellPropertyDescription, Object, IShellProperty)
			Dim typeHash As Integer = GetTypeHash(argTypes)

			' Finds the correct constructor by matching the hash of the types.
			Dim ctorInfo As ConstructorInfo = type.GetConstructors(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.[Public]).FirstOrDefault(Function(x) typeHash = GetTypeHash(x.GetParameters().[Select](Function(a) a.ParameterType)))

			If ctorInfo Is Nothing Then
				Throw New ArgumentException(LocalizedMessages.ShellPropertyFactoryConstructorNotFound, "type")
			End If

			Dim key = Expression.Parameter(argTypes(0), "propKey")
			Dim desc = Expression.Parameter(argTypes(1), "desc")
			Dim third = Expression.Parameter(GetType(Object), "third")
			'needs to be object to avoid casting later
			Dim create = Expression.[New](ctorInfo, key, desc, Expression.Convert(third, argTypes(2)))

			Return Expression.Lambda(Of Func(Of PropertyKey, ShellPropertyDescription, Object, IShellProperty))(create, key, desc, third).Compile()
		End Function

		Private Shared Function GetTypeHash(ParamArray types As Type()) As Integer
			Return GetTypeHash(DirectCast(types, IEnumerable(Of Type)))
		End Function

		' Creates a hash code, unique to the number and order of types.
		Private Shared Function GetTypeHash(types As IEnumerable(Of Type)) As Integer
			Dim hash As Integer = 0
			For Each type As Type In types
				hash = hash * 31 + type.GetHashCode()
			Next
			Return hash
		End Function

		#End Region
	End Class
End Namespace
