'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Resources

Namespace ApplicationServices

    ''' <summary>
    ''' A snapshot of the state of the battery.
    ''' </summary>
    Public Class BatteryState

        Friend Sub New()
            Dim state As PowerManagementNativeMethods.SystemBatteryState = Power.GetSystemBatteryState()

            If Not state.BatteryPresent Then
                Throw New InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent)
            End If

            ACOnline = state.AcOnLine
            MaxCharge = CInt(state.MaxCapacity)
            CurrentCharge = CInt(state.RemainingCapacity)
            ChargeRate = CInt(state.Rate)

            Dim estimatedTime As UInteger = state.EstimatedTime
            If estimatedTime <> UInteger.MaxValue Then
                ' uint.MaxValue signifies indefinite estimated time (plugged in)
                EstimatedTimeRemaining = New TimeSpan(0, 0, CInt(estimatedTime))
            Else
                EstimatedTimeRemaining = TimeSpan.MaxValue
            End If

            SuggestedCriticalBatteryCharge = CInt(state.DefaultAlert1)
            SuggestedBatteryWarningCharge = CInt(state.DefaultAlert2)
        End Sub

#Region "Public properties"

        ''' <summary>
        ''' Gets a value that indicates whether the battery charger is 
        ''' operating on external power.
        ''' </summary>
        ''' <value>A <see cref="System.Boolean"/> value. <b>True</b> indicates the battery charger is operating on AC power.</value>
        Public Property ACOnline() As Boolean

        ''' <summary>
        ''' Gets the maximum charge of the battery (in mW).
        ''' </summary>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Property MaxCharge() As Integer

        ''' <summary>
        ''' Gets the current charge of the battery (in mW).
        ''' </summary>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Property CurrentCharge() As Integer

        ''' <summary>
        ''' Gets the rate of discharge for the battery (in mW). 
        ''' </summary>
        ''' <remarks>
        ''' If plugged in, fully charged: DischargeRate = 0.
        ''' If plugged in, charging: DischargeRate = positive mW per hour.
        ''' If unplugged: DischargeRate = negative mW per hour.
        ''' </remarks>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Property ChargeRate() As Integer

        ''' <summary>
        ''' Gets the estimated time remaining until the battery is empty.
        ''' </summary>
        ''' <value>A <see cref="System.TimeSpan"/> object.</value>
        Public Property EstimatedTimeRemaining() As TimeSpan

        ''' <summary>
        ''' Gets the manufacturer's suggested battery charge level 
        ''' that should cause a critical alert to be sent to the user.
        ''' </summary>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Property SuggestedCriticalBatteryCharge() As Integer

        ''' <summary>
        ''' Gets the manufacturer's suggested battery charge level
        ''' that should cause a warning to be sent to the user.
        ''' </summary>
        ''' <value>An <see cref="System.Int32"/> value.</value>
        Public Property SuggestedBatteryWarningCharge() As Integer
#End Region

        ''' <summary>
        ''' Generates a string that represents this <b>BatteryState</b> object.
        ''' </summary>
        ''' <returns>A <see cref="System.String"/> representation of this object's current state.</returns>        
        Public Overrides Function ToString() As String
            Return String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.BatteryStateStringRepresentation, Environment.NewLine, ACOnline, MaxCharge, CurrentCharge,
                ChargeRate, EstimatedTimeRemaining, SuggestedCriticalBatteryCharge, SuggestedBatteryWarningCharge)
        End Function
    End Class
End Namespace
