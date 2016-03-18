' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Windows.Resources

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Represents an ELS service.
	''' A detailed overview of the Extended Linguistic Services platform is available at:
	''' http://msdn.microsoft.com/en-us/library/dd317839(VS.85).aspx
	''' </summary>
	Public Class MappingService
		Private _win32Service As Win32Service
		Private _service As IntPtr

		''' <summary>
		''' Constructs a new <see cref="MappingService">MappingService</see> object by instanciating an ELS service
		''' by its guid. For Windows 7, the only supported GUIDs are provided as
		''' readonly members of the <see cref="MappingAvailableServices">MappingAvailableServices</see> class.
		'''
		''' If the service
		''' with the specified guid doesn't exist, a <see cref="LinguisticException">LinguisticException</see> is thrown.
		''' </summary>
		''' <param name="serviceIdentifier">The guid of the service to instantiate.</param>        
		Public Sub New(serviceIdentifier As Guid)
			ThrowIfNotWin7()

			Dim servicePointer As IntPtr
			Dim serviceCount As UInt32 = 0
			Dim hresult As UInt32 = 0

			' First, check to see if we already have the service in the cache:
			servicePointer = ServiceCache.Instance.GetCachedService(serviceIdentifier)
			If servicePointer <> IntPtr.Zero Then
				_service = servicePointer
				_win32Service = InteropTools.Unpack(Of Win32Service)(_service)
			Else
				' pService is IntPtr.Zero in this case.
				' If we don't, we must find it via MappingGetServices:
				Dim guidPtr As IntPtr = IntPtr.Zero
				Try
					guidPtr = Marshal.AllocHGlobal(InteropTools.SizeOfGuid)
					Dim enumOptions As New Win32EnumOptions()
					enumOptions._size = InteropTools.SizeOfWin32EnumOptions
					Marshal.StructureToPtr(serviceIdentifier, guidPtr, False)
					enumOptions._pGuid = guidPtr
					hresult = Win32NativeMethods.MappingGetServices(enumOptions, servicePointer, serviceCount)
					If hresult <> 0 Then
						Throw New LinguisticException(hresult)
					End If
					If servicePointer = IntPtr.Zero Then
						Throw New InvalidOperationException()
					End If
					If serviceCount <> 1 Then
						hresult = Win32NativeMethods.MappingFreeServices(servicePointer)
						If hresult = 0 Then
							Throw New InvalidOperationException()
						Else
							Throw New LinguisticException(hresult)
						End If
					End If
					Dim services As IntPtr() = New IntPtr(0) {}
					ServiceCache.Instance.RegisterServices(servicePointer, services)
					_service = services(0)
					_win32Service = InteropTools.Unpack(Of Win32Service)(_service)
				Finally
					If servicePointer <> IntPtr.Zero Then
						' Ignore the result if an exception is being thrown.
						Win32NativeMethods.MappingFreeServicesVoid(servicePointer)
					End If
					If guidPtr <> IntPtr.Zero Then
						Marshal.FreeHGlobal(guidPtr)
					End If
				End Try
			End If
		End Sub

		Private Sub New(pService As IntPtr)
			_service = pService
			_win32Service = InteropTools.Unpack(Of Win32Service)(_service)
		End Sub

		''' <summary>
		''' Retrieves a list of available ELS platform-supported services, along with associated
		''' information, according to application-specified criteria.
		''' </summary>
		''' <param name="options">Optional. A <see cref="MappingEnumOptions">MappingEnumOptions</see> object containing criteria to use during
		''' enumeration of services. The application specifies null for this parameter to retrieve all
		''' installed services.</param>
		''' <returns>An array of <see cref="MappingService">MappingService</see> objects matching the criteria supplied in the options
		''' parameter.</returns>
		Public Shared Function GetServices(options As MappingEnumOptions) As MappingService()
			ThrowIfNotWin7()

			Dim servicePointer As IntPtr = IntPtr.Zero
			Dim serviceCount As UInt32 = 0
			Dim hresult As UInt32 = 0
			Dim guidPointer As IntPtr = IntPtr.Zero
			Try
				If options IsNot Nothing Then
					Dim enumOptions As Win32EnumOptions = options._win32EnumOption
					Dim pGuid As Nullable(Of Guid) = options._guid
					If pGuid IsNot Nothing Then
						Dim guid As Guid = CType(pGuid, Guid)
						guidPointer = Marshal.AllocHGlobal(InteropTools.SizeOfGuid)
						Marshal.StructureToPtr(guid, guidPointer, False)
						enumOptions._pGuid = guidPointer
					End If
					hresult = Win32NativeMethods.MappingGetServices(enumOptions, servicePointer, serviceCount)
				Else
					hresult = Win32NativeMethods.MappingGetServices(IntPtr.Zero, servicePointer, serviceCount)
				End If

				If hresult <> 0 Then
					Throw New LinguisticException(hresult)
				End If
				If (servicePointer = IntPtr.Zero) <> (serviceCount = 0) Then
					Throw New InvalidOperationException()
				End If

                Dim services As IntPtr() = New IntPtr(CInt(serviceCount - 1)) {}
                ServiceCache.Instance.RegisterServices(servicePointer, services)
                Dim result As MappingService() = New MappingService(CInt(serviceCount - 1)) {}
                For i As Integer = 0 To result.Length - 1
                    result(i) = New MappingService(services(i))
                Next
                Return result
			Finally
				If servicePointer <> IntPtr.Zero Then
					' Ignore the result if an exception is being thrown.
					Win32NativeMethods.MappingFreeServicesVoid(servicePointer)
				End If
				If guidPointer <> IntPtr.Zero Then
					Marshal.FreeHGlobal(guidPointer)
				End If
			End Try
		End Function

		''' <summary>
		''' Calls an ELS service to recognize text. For example, the Microsoft Language Detection service
		''' will attempt to recognize the language in which the input text is written.
		''' </summary>
		''' <param name="text">The text to recognize. The text must be UTF-16, but some services have additional
		''' requirements for the input format. This parameter cannot be set to null.</param>
		''' <param name="options">Optional. A <see cref="MappingOptions">MappingOptions</see> object containing options that affect the result and
		''' behavior of text recognition. The application does not have to specify values for all object members.
		''' This parameter can be set to null to use the default mapping options.</param>
		''' <returns>A <see cref="MappingPropertyBag">MappingPropertyBag</see> object in which the service has stored its results. The structure is filled
		''' with information produced by the service during text recognition.</returns>
		Public Function RecognizeText(text As String, options As MappingOptions) As MappingPropertyBag
			If text Is Nothing Then
				Throw New ArgumentNullException("text")
			End If
			Return RecognizeText(text, text.Length, 0, options)
		End Function

		''' <summary>
		''' Calls an ELS service to recognize text. For example, the Microsoft Language Detection service
		''' will attempt to recognize the language in which the input text is written.
		''' </summary>
		''' <param name="text">The text to recognize. The text must be UTF-16, but some services have additional
		''' requirements for the input format. This parameter cannot be set to null.</param>
		''' <param name="length">Length, in characters, of the text specified in text.</param>
		''' <param name="index">Index inside the specified text to be used by the service. This value should be
		''' between 0 and length-1. If the application wants to process the entire text, it should set this
		''' parameter to 0.</param>
		''' <param name="options">Optional. A <see cref="MappingOptions">MappingOptions</see> object containing options that affect the result and
		''' behavior of text recognition. The application does not have to specify values for all object members.
		''' This parameter can be set to null to use the default mapping options.</param>
		''' <returns>A <see cref="MappingPropertyBag">MappingPropertyBag</see> object in which the service has stored its results. The structure is filled
		''' with information produced by the service during text recognition.</returns>
		Public Function RecognizeText(text As String, length As Integer, index As Integer, options As MappingOptions) As MappingPropertyBag
			If text Is Nothing Then
				Throw New ArgumentNullException("text")
			End If
			If length > text.Length OrElse length < 0 Then
				Throw New ArgumentOutOfRangeException("length")
			End If
			If index < 0 Then
				Throw New ArgumentOutOfRangeException("index")
			End If

			Dim hResult As UInt32 = LinguisticException.Fail
			Dim bag As New MappingPropertyBag(options, text)
			Try
				hResult = Win32NativeMethods.MappingRecognizeText(_service, bag._text.AddrOfPinnedObject(), CUInt(length), CUInt(index), bag._options, bag._win32PropertyBag)
				If hResult <> 0 Then
					Throw New LinguisticException(hResult)
				End If
				Return bag
			Catch
				bag.Dispose()
				Throw
			End Try

		End Function

		Private Sub RunRecognizeText(threadContext As Object)
			Dim asyncResult As MappingRecognizeAsyncResult = DirectCast(threadContext, MappingRecognizeAsyncResult)
			Dim resultState As New MappingResultState()
			Dim bag As MappingPropertyBag = Nothing
			Try
				bag = Me.RecognizeText(asyncResult.Text, asyncResult.Length, asyncResult.Index, asyncResult.Options)
			Catch linguisticException As LinguisticException
				resultState = linguisticException.ResultState
			End Try
			asyncResult.SetResult(bag, resultState)
			' Don't catch any exceptions.
			Try
                asyncResult.AsyncCallback()(asyncResult)
            Finally
				Thread.MemoryBarrier()
				DirectCast(asyncResult.AsyncWaitHandle, ManualResetEvent).[Set]()
			End Try
		End Sub

		''' <summary>
		''' Calls an ELS service to recognize text. For example, the Microsoft Language Detection service
		''' will attempt to recognize the language in which the input text is written. The execution will be
		''' handled in a thread from the managed thread pool, and the asynchronous wait handle of the returned
		''' <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object will be notified when the operation completes. The results of
		''' the call will be stored inside the <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object.
		''' </summary>
		''' <param name="text">The text to recognize. The text must be UTF-16, but some services have additional
		''' requirements for the input format. This parameter cannot be set to null.</param>
		''' <param name="options">Optional. A <see cref="MappingOptions">MappingOptions</see> object containing options that affect the result and
		''' behavior of text recognition. The application does not have to specify values for all object members.
		''' This parameter can be set to null to use the default mapping options.</param>
		''' <param name="asyncCallback">An application callback delegate to receive callbacks with the results from
		''' the recognize operation. Cannot be set to null.</param>
		''' <param name="callerData">Optional. Private application object passed to the callback function
		''' by a service after text recognition is complete. The application must set this parameter to null to
		''' indicate no private application data.</param>
		''' <returns>A <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object describing the asynchronous operation.</returns>
		Public Function BeginRecognizeText(text As String, options As MappingOptions, asyncCallback As AsyncCallback, callerData As Object) As MappingRecognizeAsyncResult
			If text Is Nothing Then
				Throw New ArgumentNullException("text")
			End If
			Return BeginRecognizeText(text, text.Length, 0, options, asyncCallback, callerData)
		End Function

		''' <summary>
		''' Calls an ELS service to recognize text. For example, the Microsoft Language Detection service
		''' will attempt to recognize the language in which the input text is written. The execution will be
		''' handled in a thread from the managed thread pool, and the asynchronous wait handle of the returned
		''' <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object will be notified when the operation completes. The results of
		''' the call will be stored inside the <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object.
		''' </summary>
		''' <param name="text">The text to recognize. The text must be UTF-16, but some services have additional
		''' requirements for the input format. This parameter cannot be set to null.</param>
		''' <param name="length">Length, in characters, of the text specified in text.</param>
		''' <param name="index">Index inside the specified text to be used by the service. This value should be
		''' between 0 and length-1. If the application wants to process the entire text, it should set this
		''' parameter to 0.</param>
		''' <param name="options">Optional. A <see cref="MappingOptions">MappingOptions</see> object containing options that affect the result and
		''' behavior of text recognition. The application does not have to specify values for all object members.
		''' This parameter can be set to null to use the default mapping options.</param>
		''' <param name="asyncCallback">An application callback delegate to receive callbacks with the results from
		''' the recognize operation. Cannot be set to null.</param>
		''' <param name="callerData">Optional. Private application object passed to the callback function
		''' by a service after text recognition is complete. The application must set this parameter to null to
		''' indicate no private application data.</param>
		''' <returns>A <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object describing the asynchronous operation.</returns>
		Public Function BeginRecognizeText(text As String, length As Integer, index As Integer, options As MappingOptions, asyncCallback As AsyncCallback, callerData As Object) As MappingRecognizeAsyncResult
			If asyncCallback Is Nothing Then
				Throw New ArgumentNullException("asyncCallback")
			End If
			Dim result As New MappingRecognizeAsyncResult(callerData, asyncCallback, text, length, index, options)
			Try
				ThreadPool.QueueUserWorkItem(AddressOf Me.RunRecognizeText, result)
				Return result
			Catch
				result.Dispose()
				Throw
			End Try
		End Function

		''' <summary>
		''' Waits for the asynchronous operation to complete.
		''' </summary>
		''' <param name="asyncResult">The <see cref="MappingRecognizeAsyncResult">MappingRecognizeAsyncResult</see> object associated with the operation.</param>        
		Public Shared Sub EndRecognizeText(asyncResult As MappingRecognizeAsyncResult)
			If asyncResult IsNot Nothing AndAlso Not asyncResult.IsCompleted Then
				asyncResult.AsyncWaitHandle.WaitOne()
			End If
		End Sub

		''' <summary>
		''' Causes an ELS service to perform an action after text recognition has occurred. For example,
		''' a phone dialer service first must recognize phone numbers and then can perform the "action"
		''' of dialing a number.
		''' </summary>
		''' <param name="bag">A <see cref="MappingPropertyBag">MappingPropertyBag</see> object containing the results of a previous call to
		''' MappingService.MappingRecognizeText. This parameter cannot be set to null.</param>
		''' <param name="rangeIndex">A starting index inside the text recognition results for a recognized
		''' text range. This value should be between 0 and the range count.</param>
		''' <param name="actionId">The identifier of the action to perform.
		''' This parameter cannot be set to null.</param>
		Public Shared Sub DoAction(bag As MappingPropertyBag, rangeIndex As Integer, actionId As String)
			If bag Is Nothing Then
				Throw New ArgumentNullException("bag")
			End If

			If rangeIndex < 0 Then
				Throw New LinguisticException(LinguisticException.InvalidArgs)
			End If
			Dim hResult As UInt32 = Win32NativeMethods.MappingDoAction(bag._win32PropertyBag, CUInt(rangeIndex), actionId)
			If hResult <> 0 Then
				Throw New LinguisticException(hResult)
			End If
		End Sub

		Private Sub RunDoAction(threadContext As Object)
			Dim asyncResult As MappingActionAsyncResult = DirectCast(threadContext, MappingActionAsyncResult)
			Dim resultState As New MappingResultState()
			Try
				DoAction(asyncResult.PropertyBag, asyncResult.RangeIndex, asyncResult.ActionId)
			Catch linguisticException As LinguisticException
				resultState = linguisticException.ResultState
			End Try
			asyncResult.SetResult(asyncResult.PropertyBag, resultState)

			' Don't catch any exceptions.
			Try
                asyncResult.AsyncCallback()(asyncResult)
            Finally
				Thread.MemoryBarrier()
				DirectCast(asyncResult.AsyncWaitHandle, ManualResetEvent).[Set]()
			End Try
		End Sub

		''' <summary>
		''' Causes an ELS service to perform an action after text recognition has occurred. For example,
		''' a phone dialer service first must recognize phone numbers and then can perform the "action"
		''' of dialing a number.
		''' </summary>
		''' <param name="bag">A <see cref="MappingPropertyBag">MappingPropertyBag</see> object containing the results of a previous call to
		''' MappingService.MappingRecognizeText. This parameter cannot be set to null.</param>
		''' <param name="rangeIndex">A starting index inside the text recognition results for a recognized
		''' text range. This value should be between 0 and the range count.</param>
		''' <param name="actionId">The identifier of the action to perform.
		''' This parameter cannot be set to null.</param>
		''' <param name="asyncCallback">An application callback delegate to receive callbacks with the results from
		''' the action operation. Cannot be set to null.</param>
		''' <param name="callerData">Optional. Private application object passed to the callback function
		''' by a service after the action operation is complete. The application must set this parameter to null
		''' to indicate no private application data.</param>
		''' <returns>A <see cref="MappingActionAsyncResult">MappingActionAsyncResult</see> object describing the asynchronous operation.</returns>
		Public Function BeginDoAction(bag As MappingPropertyBag, rangeIndex As Integer, actionId As String, asyncCallback As AsyncCallback, callerData As Object) As MappingActionAsyncResult
			Dim result As New MappingActionAsyncResult(callerData, asyncCallback, bag, rangeIndex, actionId)
			Try
				ThreadPool.QueueUserWorkItem(AddressOf Me.RunDoAction, result)
				Return result
			Catch
				result.Dispose()
				Throw
			End Try
		End Function

		''' <summary>
		''' Waits for the asynchronous operation to complete.
		''' </summary>
		''' <param name="asyncResult">The MappingActionAsyncResult object associated with the operation.</param>
		Public Shared Sub EndDoAction(asyncResult As MappingActionAsyncResult)
			If asyncResult IsNot Nothing AndAlso Not asyncResult.IsCompleted Then
				asyncResult.AsyncWaitHandle.WaitOne()
			End If
		End Sub

		''' <summary>
		''' Copyright information about the service.
		''' </summary>
		Public ReadOnly Property Copyright() As String
			Get
				Return _win32Service._copyright
			End Get
		End Property

		''' <summary>
		''' Major version number that is used to track changes to the service.
		''' </summary>
		Public ReadOnly Property MajorVersion() As Integer
			Get
				Return _win32Service._majorVersion
			End Get
		End Property

		''' <summary>
		''' Minor version number that is used to track changes to the service.
		''' </summary>
		Public ReadOnly Property MinorVersion() As Integer
			Get
				Return _win32Service._minorVersion
			End Get
		End Property

		''' <summary>
		''' Build version that is used to track changes to the service.
		''' </summary>
		Public ReadOnly Property BuildVersion() As Integer
			Get
				Return _win32Service._buildVersion
			End Get
		End Property

		''' <summary>
		''' Step version that is used to track changes to the service.
		''' </summary>
		Public ReadOnly Property StepVersion() As Integer
			Get
				Return _win32Service._stepVersion
			End Get
		End Property

		''' <summary>
		''' Optional. A collection of input content types, following the format of the MIME content types, that
		''' identify the format that the service interprets when the application passes data. Examples of
		''' content types are "text/plain", "text/html" and "text/css".
		''' </summary>
		Public ReadOnly Property InputContentTypes() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._inputContentTypes, _win32Service._inputContentTypesCount)
			End Get
		End Property

		''' <summary>
		''' Optional. A collection of output content types, following the format of the MIME content types, that
		''' identify the format in which the service retrieves data.
		''' </summary>
		Public ReadOnly Property OutputContentTypes() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._outputContentTypes, _win32Service._outputContentTypesCount)
			End Get
		End Property

		''' <summary>
		''' A collection of the input languages, following the IETF naming convention, that the service accepts. This
		''' member is set to null if the service can work with any input language.
		''' </summary>
		Public ReadOnly Property InputLanguages() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._inputLanguages, _win32Service._inputLanguagesCount)
			End Get
		End Property

		''' <summary>
		''' A collection of output languages, following the IETF naming convention, in which the service can retrieve
		''' results. This member is set to null if the service can retrieve results in any language, or if the service
		''' ignores the output language.
		''' </summary>
		Public ReadOnly Property OutputLanguages() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._outputLanguages, _win32Service._outputLanguagesCount)
			End Get
		End Property

		''' <summary>
		''' A collection of input scripts, with Unicode standard script names, that are supported by the service.
		''' This member is set to null if the service can work with any scripts, or if the service ignores the
		''' input scripts.
		''' </summary>
		Public ReadOnly Property InputScripts() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._inputScripts, _win32Service._inputScriptsCount)
			End Get
		End Property

		''' <summary>
		''' A collection of output scripts supported by the service. This member is set to null if the service can work with
		''' any scripts, or the service ignores the output scripts.
		''' </summary>
		Public ReadOnly Property OutputScripts() As IEnumerable(Of String)
			Get
				Return InteropTools.UnpackStringArray(_win32Service._outputScripts, _win32Service._outputScriptsCount)
			End Get
		End Property

		''' <summary>
		''' Globally unique identifier (guid) for the service.
		''' </summary>
		Public ReadOnly Property Guid() As Guid
			Get
				Return _win32Service._guid
			End Get
		End Property

		''' <summary>
		''' The service category for the service, for example, "Transliteration".
		''' </summary>
		Public ReadOnly Property Category() As String
			Get
				Return _win32Service._category
			End Get
		End Property

		''' <summary>
		''' The service description. This text can be localized.
		''' </summary>
		Public ReadOnly Property Description() As String
			Get
				Return _win32Service._description
			End Get
		End Property

		''' <summary>
		''' Flag indicating the language mapping between input language and output language that is supported
		''' by the service. Possible values are shown in the following table.
		''' 0 - The input and output languages are not paired and the service can receive data in any of the
		''' input languages and render data in any of the output languages.
		''' 1 - The arrays of the input and output languages for the service are paired. In other words, given
		''' a particular input language, the service retrieves results in the paired language defined in the
		''' output language array. Use of the language pairing can be useful, for example, in bilingual
		''' dictionary scenarios.
		''' </summary>
		Public ReadOnly Property IsOneToOneLanguageMapping() As Boolean
			Get
				Return (_win32Service._serviceTypes And ServiceTypes.IsOneToOneLanguageMapping) = ServiceTypes.IsOneToOneLanguageMapping
			End Get
		End Property

		''' <summary>
		''' Indicates whether this feature is supported on the current platform.
		''' </summary>
		Public Shared ReadOnly Property IsPlatformSupported() As Boolean
			Get
				' We need Windows 7 onwards ...
				Return RunningOnWin7
			End Get
		End Property

		''' <summary>
		''' Determines if the application is running on Windows 7
		''' </summary>
		Friend Shared ReadOnly Property RunningOnWin7() As Boolean
			Get
				Return (Environment.OSVersion.Version.Major > 6) OrElse (Environment.OSVersion.Version.Major = 6 AndAlso Environment.OSVersion.Version.Minor >= 1)
			End Get
		End Property

		''' <summary>
		''' Throws PlatformNotSupportedException if the application is not running on Windows 7
		''' </summary>
		Friend Shared Sub ThrowIfNotWin7()
			If Not RunningOnWin7 Then
				Throw New PlatformNotSupportedException(LocalizedMessages.SupportedWindows7)
			End If
		End Sub

	End Class

End Namespace
