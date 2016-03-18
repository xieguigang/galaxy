'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Net
	''' <summary>
	''' Specifies types of network connectivity.
	''' </summary>    
	<Flags> _
	Public Enum ConnectivityStates
		''' <summary>
		''' The underlying network interfaces have no 
		''' connectivity to any network.
		''' </summary>
		None = 0
		''' <summary>
		''' There is connectivity to the Internet 
		''' using the IPv4 protocol.
		''' </summary>        
		IPv4Internet = &H40
		''' <summary>
		''' There is connectivity to a routed network
		''' using the IPv4 protocol.
		''' </summary>        
		IPv4LocalNetwork = &H20
		''' <summary>
		''' There is connectivity to a network, but 
		''' the service cannot detect any IPv4 
		''' network traffic.
		''' </summary>
		IPv4NoTraffic = 1
		''' <summary>
		''' There is connectivity to the local 
		''' subnet using the IPv4 protocol.
		''' </summary>
		IPv4Subnet = &H10
		''' <summary>
		''' There is connectivity to the Internet 
		''' using the IPv4 protocol.
		''' </summary>
		IPv6Internet = &H400
		''' <summary>
		''' There is connectivity to a local 
		''' network using the IPv6 protocol.
		''' </summary>
		IPv6LocalNetwork = &H200
		''' <summary>
		''' There is connectivity to a network, 
		''' but the service cannot detect any 
		''' IPv6 network traffic
		''' </summary>
		IPv6NoTraffic = 2
		''' <summary>
		''' There is connectivity to the local 
		''' subnet using the IPv6 protocol.
		''' </summary>
		IPv6Subnet = &H100
	End Enum

	''' <summary>
	''' Specifies the domain type of a network.
	''' </summary>
	Public Enum DomainType
		''' <summary>
		''' The network is not an Active Directory network.
		''' </summary>
		NonDomainNetwork = 0
		''' <summary>
		''' The network is an Active Directory network, but this machine is not authenticated against it.
		''' </summary>
		DomainNetwork = 1
		''' <summary>
		''' The network is an Active Directory network, and this machine is authenticated against it.
		''' </summary>
		DomainAuthenticated = 2
	End Enum

	''' <summary>
	''' Specifies the trust level for a 
	''' network.
	''' </summary>
	Public Enum NetworkCategory
		''' <summary>
		''' The network is a public (untrusted) network. 
		''' </summary>
		[Public]
		''' <summary>
		''' The network is a private (trusted) network. 
		''' </summary>
		[Private]
		''' <summary>
		''' The network is authenticated against an Active Directory domain.
		''' </summary>
		Authenticated
	End Enum

	''' <summary>
	''' Specifies the level of connectivity for 
	''' networks returned by the 
	''' <see cref="NetworkListManager"/> 
	''' class.
	''' </summary>
	<Flags> _
	Public Enum NetworkConnectivityLevels
		''' <summary>
		''' Networks that the machine is connected to.
		''' </summary>
		Connected = 1
		''' <summary>
		''' Networks that the machine is not connected to.
		''' </summary>
		Disconnected = 2
		''' <summary>
		''' All networks.
		''' </summary>
		All = 3
	End Enum


End Namespace
