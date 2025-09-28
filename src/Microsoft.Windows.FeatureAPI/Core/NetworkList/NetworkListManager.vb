'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Internal

Namespace Net
	''' <summary>
	''' Provides access to objects that represent networks and network connections.
	''' </summary>
	Public NotInheritable Class NetworkListManager
		Private Sub New()
		End Sub
		#Region "Private Fields"

		Shared manager As New NetworkListManagerClass()

		#End Region

		''' <summary>
		''' Retrieves a collection of <see cref="Network"/> objects that represent the networks defined for this machine.
		''' </summary>
		''' <param name="level">
		''' The <see cref="NetworkConnectivityLevels"/> that specify the connectivity level of the returned <see cref="Network"/> objects.
		''' </param>
		''' <returns>
		''' A <see cref="NetworkCollection"/> of <see cref="Network"/> objects.
		''' </returns>
		Public Shared Function GetNetworks(level As NetworkConnectivityLevels) As NetworkCollection
			' Throw PlatformNotSupportedException if the user is not running Vista or beyond
			CoreHelpers.ThrowIfNotVista()

			Return New NetworkCollection(manager.GetNetworks(level))
		End Function

		''' <summary>
		''' Retrieves the <see cref="Network"/> identified by the specified network identifier.
		''' </summary>
		''' <param name="networkId">
		''' A <see cref="System.Guid"/> that specifies the unique identifier for the network.
		''' </param>
		''' <returns>
		''' The <see cref="Network"/> that represents the network identified by the identifier.
		''' </returns>
		Public Shared Function GetNetwork(networkId As Guid) As Network
			' Throw PlatformNotSupportedException if the user is not running Vista or beyond
			CoreHelpers.ThrowIfNotVista()

			Return New Network(manager.GetNetwork(networkId))
		End Function

		''' <summary>
		''' Retrieves a collection of <see cref="NetworkConnection"/> objects that represent the connections for this machine.
		''' </summary>
		''' <returns>
		''' A <see cref="NetworkConnectionCollection"/> containing the network connections.
		''' </returns>
		Public Shared Function GetNetworkConnections() As NetworkConnectionCollection
			' Throw PlatformNotSupportedException if the user is not running Vista or beyond
			CoreHelpers.ThrowIfNotVista()

			Return New NetworkConnectionCollection(manager.GetNetworkConnections())
		End Function

		''' <summary>
		''' Retrieves the <see cref="NetworkConnection"/> identified by the specified connection identifier.
		''' </summary>
		''' <param name="networkConnectionId">
		''' A <see cref="System.Guid"/> that specifies the unique identifier for the network connection.
		''' </param>
		''' <returns>
		''' The <see cref="NetworkConnection"/> identified by the specified identifier.
		''' </returns>
		Public Shared Function GetNetworkConnection(networkConnectionId As Guid) As NetworkConnection
			' Throw PlatformNotSupportedException if the user is not running Vista or beyond
			CoreHelpers.ThrowIfNotVista()

			Return New NetworkConnection(manager.GetNetworkConnection(networkConnectionId))
		End Function

		''' <summary>
		''' Gets a value that indicates whether this machine 
		''' has Internet connectivity.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public Shared ReadOnly Property IsConnectedToInternet() As Boolean
			Get
				' Throw PlatformNotSupportedException if the user is not running Vista or beyond
				CoreHelpers.ThrowIfNotVista()

				Return manager.IsConnectedToInternet
			End Get
		End Property

		''' <summary>
		''' Gets a value that indicates whether this machine 
		''' has network connectivity.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public Shared ReadOnly Property IsConnected() As Boolean
			Get
				' Throw PlatformNotSupportedException if the user is not running Vista or beyond
				CoreHelpers.ThrowIfNotVista()

				Return manager.IsConnected
			End Get
		End Property

		''' <summary>
		''' Gets the connectivity state of this machine.
		''' </summary>
		''' <value>A <see cref="Connectivity"/> value.</value>
		Public Shared ReadOnly Property Connectivity() As ConnectivityStates
			Get
				' Throw PlatformNotSupportedException if the user is not running Vista or beyond
				CoreHelpers.ThrowIfNotVista()

				Return manager.GetConnectivity()
			End Get
		End Property
	End Class

End Namespace
