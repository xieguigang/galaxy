'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Shell

Namespace Controls

    ''' <summary>
    ''' Event argument for The NavigationPending event
    ''' </summary>
    Public Class NavigationPendingEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The location being navigated to
        ''' </summary>
        Public Property PendingLocation() As ShellObject
            Get
                Return m_PendingLocation
            End Get
            Set
                m_PendingLocation = Value
            End Set
        End Property
        Private m_PendingLocation As ShellObject

        ''' <summary>
        ''' Set to 'True' to cancel the navigation.
        ''' </summary>
        Public Property Cancel() As Boolean
            Get
                Return m_Cancel
            End Get
            Set
                m_Cancel = Value
            End Set
        End Property
        Private m_Cancel As Boolean

    End Class

    ''' <summary>
    ''' Event argument for The NavigationComplete event
    ''' </summary>
    Public Class NavigationCompleteEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The new location of the explorer browser
        ''' </summary>
        Public Property NewLocation() As ShellObject
            Get
                Return m_NewLocation
            End Get
            Set
                m_NewLocation = Value
            End Set
        End Property
        Private m_NewLocation As ShellObject
    End Class

    ''' <summary>
    ''' Event argument for the NavigatinoFailed event
    ''' </summary>
    Public Class NavigationFailedEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The location the the browser would have navigated to.
        ''' </summary>
        Public Property FailedLocation() As ShellObject
            Get
                Return m_FailedLocation
            End Get
            Set
                m_FailedLocation = Value
            End Set
        End Property
        Private m_FailedLocation As ShellObject
    End Class
End Namespace
