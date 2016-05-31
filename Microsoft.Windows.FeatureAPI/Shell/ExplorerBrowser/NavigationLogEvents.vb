'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.VisualBasic.Serialization

Namespace Controls

    ''' <summary>
    ''' The event argument for NavigationLogChangedEvent
    ''' </summary>
    Public Class NavigationLogEventArgs
        Inherits EventArgs

        ''' <summary>
        ''' Indicates CanNavigateForward has changed
        ''' </summary>
        Public Property CanNavigateForwardChanged() As Boolean

        ''' <summary>
        ''' Indicates CanNavigateBackward has changed
        ''' </summary>
        Public Property CanNavigateBackwardChanged() As Boolean

        ''' <summary>
        ''' Indicates the Locations collection has changed
        ''' </summary>
        Public Property LocationsChanged() As Boolean

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
