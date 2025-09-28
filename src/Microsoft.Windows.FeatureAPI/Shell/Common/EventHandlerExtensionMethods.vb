Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Diagnostics
Imports System.Threading

Namespace Shell
    ''' <summary>
    ''' Provides extension methods for raising events safely.
    ''' </summary>
    Public Module EventHandlerExtensionMethods

        ''' <summary>
        ''' Safely raises an event using EventArgs.Empty
        ''' </summary>
        ''' <param name="eventHandler">EventHandler to raise</param>
        ''' <param name="sender">Event sender</param>
        <System.Runtime.CompilerServices.Extension>
        Public Sub SafeRaise(eventHandler As EventHandler, sender As Object)
            Call eventHandler(sender, EventArgs.Empty)
        End Sub

        ''' <summary>
        ''' Safely raises an event.
        ''' </summary>
        ''' <typeparam name="T">Type of event args</typeparam>
        ''' <param name="eventHandler">EventHandler&lt;T&gt; to raise</param>
        ''' <param name="sender">Event sender</param>
        ''' <param name="args">Event args</param>
        <System.Runtime.CompilerServices.Extension>
        Public Sub SafeRaise(Of T As EventArgs)(eventHandler As EventHandler(Of T), sender As Object, args As T)
            Call eventHandler(sender, args)
        End Sub

        ''' <summary>
        ''' Safely raises an event using EventArgs.Empty
        ''' </summary>
        ''' <param name="eventHandler">EventHandler&lt;EventArgs&gt; to raise</param>
        ''' <param name="sender">Event sender</param>
        <System.Runtime.CompilerServices.Extension>
        Public Sub SafeRaise(eventHandler As EventHandler(Of EventArgs), sender As Object)
            SafeRaise(eventHandler, sender, EventArgs.Empty)
        End Sub
    End Module
End Namespace
