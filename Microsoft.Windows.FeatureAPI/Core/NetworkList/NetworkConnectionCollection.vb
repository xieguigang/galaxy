'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net
	''' <summary>
	''' An enumerable collection of <see cref="NetworkConnection"/> objects.
	''' </summary>
	Public Class NetworkConnectionCollection
		Implements IEnumerable(Of NetworkConnection)
		#Region "Private Fields"

		Private networkConnectionEnumerable As IEnumerable

		#End Region

		Friend Sub New(networkConnectionEnumerable As IEnumerable)
			Me.networkConnectionEnumerable = networkConnectionEnumerable
		End Sub

#Region "IEnumerable<NetworkConnection> Members"

        ''' <summary>
        ''' Returns the strongly typed enumerator for this collection.
        ''' </summary>
        ''' <returns>A <see cref="System.Collections.Generic.IEnumerator(Of T)"/> object.</returns>
        Public Iterator Function GetEnumerator() As IEnumerator(Of NetworkConnection) Implements IEnumerable(Of NetworkConnection).GetEnumerator
            For Each networkConnection As INetworkConnection In networkConnectionEnumerable
                Yield New NetworkConnection(networkConnection)
            Next
        End Function

#End Region

#Region "IEnumerable Members"

        ''' <summary>
        ''' Returns the enumerator for this collection.
        ''' </summary>
        '''<returns>A <see cref="System.Collections.IEnumerator"/> object.</returns> 
        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            For Each networkConnection As INetworkConnection In networkConnectionEnumerable
                Yield New NetworkConnection(networkConnection)
            Next
        End Function

#End Region
    End Class
End Namespace
