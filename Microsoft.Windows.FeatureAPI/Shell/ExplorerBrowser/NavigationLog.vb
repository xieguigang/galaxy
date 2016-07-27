'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.Windows.Controls.WindowsForms
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Shell

Namespace Controls

    ''' <summary>
    ''' The navigation log is a history of the locations visited by the explorer browser. 
    ''' </summary>
    Public Class ExplorerBrowserNavigationLog
#Region "operations"
        ''' <summary>
        ''' Clears the contents of the navigation log.
        ''' </summary>
        Public Sub ClearLog()
            ' nothing to do
            If _locations.Count = 0 Then
                Return
            End If

            Dim oldCanNavigateBackward As Boolean = CanNavigateBackward
            Dim oldCanNavigateForward As Boolean = CanNavigateForward

            _locations.Clear()
            Me.m_currentLocationIndex = -1

            Dim args As New NavigationLogEventArgs()
            args.LocationsChanged = True
            args.CanNavigateBackwardChanged = (oldCanNavigateBackward <> CanNavigateBackward)
            args.CanNavigateForwardChanged = (oldCanNavigateForward <> CanNavigateForward)
            RaiseEvent NavigationLogChanged(Me, args)
        End Sub
#End Region

#Region "properties"
        ''' <summary>
        ''' Indicates the presence of locations in the log that can be 
        ''' reached by calling Navigate(Forward)
        ''' </summary>
        Public ReadOnly Property CanNavigateForward() As Boolean
            Get
                Return (CurrentLocationIndex < (_locations.Count - 1))
            End Get
        End Property

        ''' <summary>
        ''' Indicates the presence of locations in the log that can be 
        ''' reached by calling Navigate(Backward)
        ''' </summary>
        Public ReadOnly Property CanNavigateBackward() As Boolean
            Get
                Return (CurrentLocationIndex > 0)
            End Get
        End Property

        ''' <summary>
        ''' The navigation log
        ''' </summary>
        Public ReadOnly Iterator Property Locations() As IEnumerable(Of ShellObject)
            Get
                For Each obj As ShellObject In _locations
                    Yield obj
                Next
            End Get
        End Property
        Private _locations As New List(Of ShellObject)()

        ''' <summary>
        ''' An index into the Locations collection. The ShellObject pointed to 
        ''' by this index is the current location of the ExplorerBrowser.
        ''' </summary>
        Public ReadOnly Property CurrentLocationIndex() As Integer
            Get
                Return m_currentLocationIndex
            End Get
        End Property


        ''' <summary>
        ''' Gets the shell object in the Locations collection pointed to
        ''' by CurrentLocationIndex.
        ''' </summary>
        Public ReadOnly Property CurrentLocation() As ShellObject
            Get
                If m_currentLocationIndex < 0 Then
                    Return Nothing
                End If

                Return _locations(m_currentLocationIndex)
            End Get
        End Property
#End Region

#Region "events"
        ''' <summary>
        ''' Fires when the navigation log changes or 
        ''' the current navigation position changes
        ''' </summary>
        Public Event NavigationLogChanged As EventHandler(Of NavigationLogEventArgs)
#End Region

#Region "implementation"

        Private parent As ExplorerBrowser = Nothing

        ''' <summary>
        ''' The pending navigation log action. null if the user is not navigating 
        ''' via the navigation log.
        ''' </summary>
        Private pendingNavigation As PendingNavigation

        ''' <summary>
        ''' The index into the Locations collection. -1 if the Locations colleciton 
        ''' is empty.
        ''' </summary>
        Private m_currentLocationIndex As Integer = -1

        Friend Sub New(parent As ExplorerBrowser)
            If parent Is Nothing Then
                Throw New ArgumentException(LocalizedMessages.NavigationLogNullParent, "parent")
            End If

            ' Hook navigation events from the parent to distinguish between
            ' navigation log induced navigation, and other navigations.
            Me.parent = parent
            AddHandler Me.parent.NavigationComplete, New EventHandler(Of NavigationCompleteEventArgs)(AddressOf OnNavigationComplete)
            AddHandler Me.parent.NavigationFailed, New EventHandler(Of NavigationFailedEventArgs)(AddressOf OnNavigationFailed)
        End Sub

        Private Sub OnNavigationFailed(sender As Object, args As NavigationFailedEventArgs)
            pendingNavigation = Nothing
        End Sub

        Private Sub OnNavigationComplete(sender As Object, args As NavigationCompleteEventArgs)
            Dim eventArgs As New NavigationLogEventArgs()
            Dim oldCanNavigateBackward As Boolean = CanNavigateBackward
            Dim oldCanNavigateForward As Boolean = CanNavigateForward

            If (pendingNavigation IsNot Nothing) Then
                ' navigation log traversal in progress

                ' determine if new location is the same as the traversal request
                Dim result As Integer = 0
                pendingNavigation.Location.NativeShellItem.Compare(args.NewLocation.NativeShellItem, SICHINTF.SICHINT_ALLFIELDS, result)
                Dim shellItemsEqual As Boolean = (result = 0)
                If shellItemsEqual = False Then
                    ' new location is different than traversal request, 
                    ' behave is if it never happened!
                    ' remove history following currentLocationIndex, append new item
                    If m_currentLocationIndex < (_locations.Count - 1) Then
                        _locations.RemoveRange(CInt(m_currentLocationIndex) + 1, CInt(_locations.Count - (m_currentLocationIndex + 1)))
                    End If
                    _locations.Add(args.NewLocation)
                    m_currentLocationIndex = (_locations.Count - 1)
                    eventArgs.LocationsChanged = True
                Else
                    ' log traversal successful, update index
                    m_currentLocationIndex = CInt(pendingNavigation.Index)
                    eventArgs.LocationsChanged = False
                End If
                pendingNavigation = Nothing
            Else
                ' remove history following currentLocationIndex, append new item
                If m_currentLocationIndex < (_locations.Count - 1) Then
                    _locations.RemoveRange(CInt(m_currentLocationIndex) + 1, CInt(_locations.Count - (m_currentLocationIndex + 1)))
                End If
                _locations.Add(args.NewLocation)
                m_currentLocationIndex = (_locations.Count - 1)
                eventArgs.LocationsChanged = True
            End If

            ' update event args
            eventArgs.CanNavigateBackwardChanged = (oldCanNavigateBackward <> CanNavigateBackward)
            eventArgs.CanNavigateForwardChanged = (oldCanNavigateForward <> CanNavigateForward)

            RaiseEvent NavigationLogChanged(Me, eventArgs)
        End Sub

        Friend Function NavigateLog(direction As NavigationLogDirection) As Boolean
            ' determine proper index to navigate to
            Dim locationIndex As Integer = 0
            If direction = NavigationLogDirection.Backward AndAlso CanNavigateBackward Then
                locationIndex = (m_currentLocationIndex - 1)
            ElseIf direction = NavigationLogDirection.Forward AndAlso CanNavigateForward Then
                locationIndex = (m_currentLocationIndex + 1)
            Else
                Return False
            End If

            ' initiate traversal request
            Dim location As ShellObject = _locations(CInt(locationIndex))
            pendingNavigation = New PendingNavigation(location, locationIndex)
            parent.Navigate(location)
            Return True
        End Function

        Friend Function NavigateLog(index As Integer) As Boolean
            ' can't go anywhere
            If index >= _locations.Count OrElse index < 0 Then
                Return False
            End If

            ' no need to re navigate to the same location
            If index = m_currentLocationIndex Then
                Return False
            End If

            ' initiate traversal request
            Dim location As ShellObject = _locations(CInt(index))
            pendingNavigation = New PendingNavigation(location, index)
            parent.Navigate(location)
            Return True
        End Function

#End Region
    End Class

    ''' <summary>
    ''' A navigation traversal request
    ''' </summary>
    Friend Class PendingNavigation

        Friend Sub New(location__1 As ShellObject, index__2 As Integer)
            Location = location__1
            Index = index__2
        End Sub

        Friend Property Location() As ShellObject

        Friend Property Index() As Integer

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
