'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Net

    ''' <summary>
    ''' Represents a connection to a network.
    ''' </summary>
    ''' <remarks> A collection containing instances of this class is obtained by calling
    ''' the <see cref="P:Net.Network.Connections"/> property.</remarks>
    Public Class NetworkConnection

#Region "Private Fields"

        Private networkConnection As INetworkConnection

#End Region

        Friend Sub New(networkConnection__1 As INetworkConnection)
            Me.networkConnection = networkConnection__1
        End Sub

        ''' <summary>
        ''' Retrieves an object that represents the network 
        ''' associated with this connection.
        ''' </summary>
        ''' <returns>A <see cref="Network"/> object.</returns>
        Public ReadOnly Property Network() As Network
            Get
                Return New Network(networkConnection.GetNetwork())
            End Get
        End Property

        ''' <summary>
        ''' Gets the adapter identifier for this connection.
        ''' </summary>
        ''' <value>A <see cref="System.Guid"/> object.</value>
        Public ReadOnly Property AdapterId() As Guid
            Get
                Return networkConnection.GetAdapterId()
            End Get
        End Property
        ''' <summary>
        ''' Gets the unique identifier for this connection.
        ''' </summary>
        ''' <value>A <see cref="System.Guid"/> object.</value>
        Public ReadOnly Property ConnectionId() As Guid
            Get
                Return networkConnection.GetConnectionId()
            End Get
        End Property
        ''' <summary>
        ''' Gets a value that indicates the connectivity of this connection.
        ''' </summary>
        ''' <value>A <see cref="Connectivity"/> value.</value>
        Public ReadOnly Property Connectivity() As ConnectivityStates
            Get
                Return networkConnection.GetConnectivity()
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indicates whether the network associated
        ''' with this connection is 
        ''' an Active Directory network and whether the machine
        ''' has been authenticated by Active Directory.
        ''' </summary>
        ''' <value>A <see cref="DomainType"/> value.</value>
        Public ReadOnly Property DomainType() As DomainType
            Get
                Return networkConnection.GetDomainType()
            End Get
        End Property
        ''' <summary>
        ''' Gets a value that indicates whether this 
        ''' connection has Internet access.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public ReadOnly Property IsConnectedToInternet() As Boolean
            Get
                Return networkConnection.IsConnectedToInternet
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that indicates whether this connection has
        ''' network connectivity.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value.</value>
        Public ReadOnly Property IsConnected() As Boolean
            Get
                Return networkConnection.IsConnected
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
