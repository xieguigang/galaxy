'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Globalization
Imports System.Text

Namespace Internal

    ''' <summary>
    ''' Common Helper methods
    ''' </summary>
    Public Module CoreHelpers

        ''' <summary>
        ''' Determines if the application is running on XP
        ''' </summary>
        Public ReadOnly Property RunningOnXP() As Boolean
            Get
                Return Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.Major >= 5
            End Get
        End Property

        ''' <summary>
        ''' Throws PlatformNotSupportedException if the application is not running on Windows XP
        ''' </summary>
        Public Sub ThrowIfNotXP()
            If Not CoreHelpers.RunningOnXP Then
                Throw New PlatformNotSupportedException(GlobalLocalizedMessages.CoreHelpersRunningOnXp)
            End If
        End Sub

        ''' <summary>
        ''' Determines if the application is running on Vista
        ''' </summary>
        Public ReadOnly Property RunningOnVista() As Boolean
            Get
                Return Environment.OSVersion.Version.Major >= 6
            End Get
        End Property

        ''' <summary>
        ''' Throws PlatformNotSupportedException if the application is not running on Windows Vista
        ''' </summary>
        Public Sub ThrowIfNotVista()
            If Not CoreHelpers.RunningOnVista Then
                Throw New PlatformNotSupportedException(GlobalLocalizedMessages.CoreHelpersRunningOnVista)
            End If
        End Sub

        ''' <summary>
        ''' Determines if the application is running on Windows 7
        ''' </summary>
        Public ReadOnly Property RunningOnWin7() As Boolean
            Get
                ' Verifies that OS version is 6.1 or greater, and the Platform is WinNT.
                Return Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.CompareTo(New Version(6, 1)) >= 0
            End Get
        End Property

        ''' <summary>
        ''' Throws PlatformNotSupportedException if the application is not running on Windows 7
        ''' </summary>
        Public Sub ThrowIfNotWin7()
            If Not CoreHelpers.RunningOnWin7 Then
                Throw New PlatformNotSupportedException(GlobalLocalizedMessages.CoreHelpersRunningOn7)
            End If
        End Sub

        ''' <summary>
        ''' Get a string resource given a resource Id
        ''' </summary>
        ''' <param name="resourceId">The resource Id</param>
        ''' <returns>The string resource corresponding to the given resource Id. Returns null if the resource id
        ''' is invalid or the string cannot be retrieved for any other reason.</returns>
        Public Function GetStringResource(resourceId As String) As String
            Dim parts As String()
            Dim library As String
            Dim index As Integer

            If String.IsNullOrEmpty(resourceId) Then
                Return String.Empty
            End If

            ' Known folder "Recent" has a malformed resource id
            ' for its tooltip. This causes the resource id to
            ' parse into 3 parts instead of 2 parts if we don't fix.
            resourceId = resourceId.Replace("shell32,dll", "shell32.dll")
            parts = resourceId.Split(New Char() {","c})

            library = parts(0)
            library = library.Replace("@", String.Empty)
            library = Environment.ExpandEnvironmentVariables(library)
            Dim handle As IntPtr = CoreNativeMethods.LoadLibrary(library)

            parts(1) = parts(1).Replace("-", String.Empty)
            index = Integer.Parse(parts(1), CultureInfo.InvariantCulture)

            Dim stringValue As New StringBuilder(255)
            Dim retval As Integer = CoreNativeMethods.LoadString(handle, index, stringValue, 255)

            Return If(retval <> 0, stringValue.ToString(), Nothing)
        End Function
    End Module
End Namespace
