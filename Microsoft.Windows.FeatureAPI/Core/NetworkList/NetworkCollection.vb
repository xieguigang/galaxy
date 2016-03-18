'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Net
	''' <summary>
	''' An enumerable collection of <see cref="Network"/> objects.
	''' </summary>
	Public Class NetworkCollection
		Implements IEnumerable(Of Network)
		#Region "Private Fields"

		Private networkEnumerable As IEnumerable

		#End Region

		Friend Sub New(networkEnumerable As IEnumerable)
			Me.networkEnumerable = networkEnumerable
		End Sub

#Region "IEnumerable<Network> Members"

        ''' <summary>
        ''' Returns the strongly typed enumerator for this collection.
        ''' </summary>
        ''' <returns>An <see cref="System.Collections.Generic.IEnumerator(Of T)"/>  object.</returns>
        Public Iterator Function GetEnumerator() As IEnumerator(Of Network) Implements IEnumerable(Of Network).GetEnumerator
            For Each network As INetwork In networkEnumerable
                Yield New Network(network)
            Next
        End Function

#End Region

#Region "IEnumerable Members"

        ''' <summary>
        ''' Returns the enumerator for this collection.
        ''' </summary>
        '''<returns>An <see cref="System.Collections.IEnumerator"/> object.</returns> 
        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            For Each network As INetwork In networkEnumerable
                Yield New Network(network)
            Next
        End Function

#End Region
    End Class
End Namespace
