' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Threading

Namespace ExtendedLinguisticServices
	' This singleton object is correctly finalized on appdomain unload.    
	Friend Class ServiceCache
		Inherits CriticalFinalizerObject
		Private Shared staticInstance As New ServiceCache()

		' Guid -> IntPtr
		Private _guidToService As New Dictionary(Of Guid, IntPtr)()
		' IntPtr -> this (serves as a set)
		Private _servicePointers As New List(Of IntPtr)()
		' The lock
		Private _cacheLock As New ReaderWriterLockSlim()
		' Active resources refcount, signed 64-bit
		Private _resourceRefCount As Long
		' Specifies if the object has been finalized:
		Private _finalized As Integer

		' Left empty for singleton enforcement
		Private Sub New()
		End Sub

		Protected Overrides Sub Finalize()
			Try
				ReleaseHandle()
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		Friend Function GetCachedService(ByRef guid As Guid) As IntPtr
			_cacheLock.EnterReadLock()
			Try
				Dim result As IntPtr
				_guidToService.TryGetValue(guid, result)
				Return result
			Finally
				_cacheLock.ExitReadLock()
			End Try
		End Function

		Friend Sub RegisterServices(ByRef originalPtr As IntPtr, services As IntPtr())
			Dim addedToCache As Boolean = False
			Dim succeeded As Boolean = False
			Dim length As Integer = services.Length
			' We are taking the write lock to make this function atomic.
			' Thus, enumerating services will generally be a slow operation
			' because of this bottleneck. However, creating a service
			' by GUID should still be relatively fast, since this function
			' will be called only the first time(s) the service is initialized.
			' Note that the write lock is being released in the Cleanup() method.
			_cacheLock.EnterWriteLock()
			Try
				TryRegisterServices(originalPtr, services, addedToCache)
				succeeded = True
				If addedToCache Then
					' Added to cache means that we have added at least one
					' of the services to the service cache. In this case,
					' we need to set the originalPtr to IntPtr.Zero, in order
					' to tell the caller not to free the native pointer.
					originalPtr = IntPtr.Zero
				End If
			Finally
				If Not succeeded Then
					RollBack(originalPtr, length)
				End If
				_cacheLock.ExitWriteLock()
			End Try
		End Sub

		Private Sub TryRegisterServices(originalPtr As IntPtr, services As IntPtr(), ByRef addedToCache As Boolean)
			' Here, we will try to add the newly enumerated
			' services to the service cache.

			Dim pServices As IntPtr = originalPtr
			For i As Integer = 0 To services.Length - 1
				Dim guid As Guid = CType(Marshal.PtrToStructure(CType(CType(pServices, UInt64) + InteropTools.OffsetOfGuidInService, IntPtr), InteropTools.TypeOfGuid), Guid)
				Dim cachedValue As IntPtr
				_guidToService.TryGetValue(guid, cachedValue)
				If cachedValue = IntPtr.Zero Then
					_guidToService.Add(guid, pServices)
					cachedValue = pServices
					addedToCache = True
				End If
				System.Diagnostics.Debug.Assert(cachedValue <> IntPtr.Zero, "Cached value is NULL")
				services(i) = cachedValue
				pServices = CType(CType(pServices, UInt64) + InteropTools.SizeOfService, IntPtr)
			Next
			If addedToCache Then
				' This means that at least one of the services was stored in the cache.
				' So we must keep the original pointer in our cleanup list.
				_servicePointers.Add(originalPtr)
			End If
		End Sub

		Private Sub RollBack(pServices As IntPtr, length As Integer)
			Dim succeeded As Boolean = False
			Try
				' First, remove the original pointer from the cleanup list.
				' The caller of RegisterServices() will take care of freeing it.
				_servicePointers.Remove(pServices)
				' Then, attempt to recover the state of the _guidToService Dictionary.
				' This should not fail.
				For i As Integer = 0 To length - 1
					Dim guid As Guid = CType(Marshal.PtrToStructure(CType(CType(pServices, UInt64) + InteropTools.OffsetOfGuidInService, IntPtr), InteropTools.TypeOfGuid), Guid)
					_guidToService.Remove(guid)
					pServices = CType(CType(pServices, UInt64) + InteropTools.SizeOfService, IntPtr)
				Next
				succeeded = True
			Finally
				If Not succeeded Then
					' This should never happen, as none of the above functions
					' should be allocating any memory, and this rollback
					' should generally happen in a low-memory condition.
					' In this case, keep the _servicePointers cleanup list,
					' but invalidate the whole cache, since we may still have
					' traces of the original pointer there.
					_guidToService = Nothing
				End If
			End Try
		End Sub

		Friend Function RegisterResource() As Boolean
			' The correctness of this code relies on the
			' fact that there can be no Int64.MaxValue / 2
			' executing this code at the same time.
			If Interlocked.Increment(_resourceRefCount) > Int64.MaxValue \ 2 Then
				Interlocked.Decrement(_resourceRefCount)
				Return False
			End If
			Return True
		End Function

		Friend Sub UnregisterResource()
			If Interlocked.Decrement(_resourceRefCount) = 0 AndAlso IsInvalid Then
				FreeAllServices()
			End If
		End Sub

		Private ReadOnly Property IsInvalid() As Boolean
			Get
				Return Interlocked.CompareExchange(_finalized, 1, 1) <> 0
			End Get
		End Property

		Private Sub ReleaseHandle()
			If Not IsInvalid Then
				If Interlocked.Read(_resourceRefCount) = 0 Then
					FreeAllServices()
				End If
				Interlocked.CompareExchange(_finalized, 1, 0)
			End If
		End Sub

		Private Sub FreeAllServices()
			' Don't use synchronization here.
			' This will only be called during finalization
			' and at that point synchronization doesn't matter.
			' Also, the lock object might have already been finalized.
			If _servicePointers IsNot Nothing Then
				For Each servicePtr As IntPtr In _servicePointers
					Win32NativeMethods.MappingFreeServicesVoid(servicePtr)
				Next
				_servicePointers = Nothing
				_guidToService = Nothing
			End If
		End Sub

		Friend Shared ReadOnly Property Instance() As ServiceCache
			Get
				Return staticInstance
			End Get
		End Property
	End Class
End Namespace
