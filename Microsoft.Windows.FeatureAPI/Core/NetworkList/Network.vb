'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Net
	''' <summary>
	''' Represents a network on the local machine. 
	''' It can also represent a collection of network 
	''' connections with a similar network signature.
	''' </summary>
	''' <remarks>
	''' Instances of this class are obtained by calling 
	''' methods on the <see cref="NetworkListManager"/> class.
	''' </remarks>
	Public Class Network
		#Region "Private Fields"

		Private network As INetwork

		#End Region

		Friend Sub New(network__1 As INetwork)
			Me.network = network__1
		End Sub

		''' <summary>
		''' Gets or sets the category of a network. The 
		''' categories are trusted, untrusted, or 
		''' authenticated.
		''' </summary>
		''' <value>A <see cref="NetworkCategory"/> value.</value>
		Public Property Category() As NetworkCategory
			Get
				Return network.GetCategory()
			End Get

			Set
				network.SetCategory(value)
			End Set
		End Property

		''' <summary>
		''' Gets the local date and time when the network 
		''' was connected.
		''' </summary>
		''' <value>A <see cref="System.DateTime"/> object.</value>
		Public ReadOnly Property ConnectedTime() As DateTime
			Get
				Dim low As UInteger, high As UInteger, dummy1 As UInteger, dummy2 As UInteger
				network.GetTimeCreatedAndConnected(dummy1, dummy2, low, high)
				Dim time As Long = high
				' Shift the day info into the high order bits.
				time <<= 32
				time = time Or low
				Return DateTime.FromFileTimeUtc(time)
			End Get
		End Property

		''' <summary>
		''' Gets the network connections for the network.
		''' </summary>
		''' <value>A <see cref="NetworkConnectionCollection"/> object.</value>
		Public ReadOnly Property Connections() As NetworkConnectionCollection
			Get
				Return New NetworkConnectionCollection(network.GetNetworkConnections())
			End Get
		End Property

		''' <summary>
		''' Gets the connectivity state of the network.
		''' </summary>
		''' <value>A <see cref="Connectivity"/> value.</value>
		''' <remarks>Connectivity provides information on whether
		''' the network is connected, and the protocols
		''' in use for network traffic.</remarks>
		Public ReadOnly Property Connectivity() As ConnectivityStates
			Get
				Return network.GetConnectivity()
			End Get
		End Property

		''' <summary>
		''' Gets the local date and time when the 
		''' network was created.
		''' </summary>
		''' <value>A <see cref="System.DateTime"/> object.</value>
		Public ReadOnly Property CreatedTime() As DateTime
			Get
				Dim low As UInteger, high As UInteger, dummy1 As UInteger, dummy2 As UInteger
				network.GetTimeCreatedAndConnected(low, high, dummy1, dummy2)
				Dim time As Long = high
				'Shift the value into the high order bits.
				time <<= 32
				time = time Or low
				Return DateTime.FromFileTimeUtc(time)
			End Get
		End Property

		''' <summary>
		''' Gets or sets a description for the network.
		''' </summary>
		''' <value>A <see cref="System.String"/> value.</value>
		Public Property Description() As String
			Get
				Return network.GetDescription()
			End Get

			Set
				network.SetDescription(value)
			End Set
		End Property

		''' <summary>
		''' Gets the domain type of the network. 
		''' </summary>
		''' <value>A <see cref="DomainType"/> value.</value>
		''' <remarks>The domain
		''' indictates whether the network is an Active
		''' Directory Network, and whether the machine
		''' has been authenticated by Active Directory.</remarks>
		Public ReadOnly Property DomainType() As DomainType
			Get
				Return network.GetDomainType()
			End Get
		End Property

		''' <summary>
		''' Gets a value that indicates whether there is
		''' network connectivity.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public ReadOnly Property IsConnected() As Boolean
			Get
				Return network.IsConnected
			End Get
		End Property

		''' <summary>
		''' Gets a value that indicates whether there is 
		''' Internet connectivity.
		''' </summary>
		''' <value>A <see cref="System.Boolean"/> value.</value>
		Public ReadOnly Property IsConnectedToInternet() As Boolean
			Get
				Return network.IsConnectedToInternet
			End Get
		End Property

		''' <summary>
		''' Gets or sets the name of the network.
		''' </summary>
		''' <value>A <see cref="System.String"/> value.</value>
		Public Property Name() As String
			Get
				Return network.GetName()
			End Get

			Set
				network.SetName(value)
			End Set
		End Property

		''' <summary>
		''' Gets a unique identifier for the network.
		''' </summary>
		''' <value>A <see cref="System.Guid"/> value.</value>
		Public ReadOnly Property NetworkId() As Guid
			Get
				Return network.GetNetworkId()
			End Get
		End Property
	End Class
End Namespace
