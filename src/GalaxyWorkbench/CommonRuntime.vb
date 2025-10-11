﻿Imports System.Runtime.CompilerServices
Imports Galaxy.Workbench.CommonDialogs
Imports Galaxy.Workbench.DockDocument
Imports Microsoft.VisualStudio.WinForms.Docking

''' <summary>
''' Common workbench runtime
''' </summary>
Public Module CommonRuntime

    Public ReadOnly Property AppHost As AppHost

    ''' <summary>
    ''' set the opacity of the <see cref="MaskForm"/> when show the input dialog via <see cref="InputDialog.Input(Action(Of InputDialog), Action)"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property MaskOpacity As Double = 0.5

    Public ReadOnly Property IsDevelopmentMode As Boolean = False

    ''' <summary>
    ''' set value to the <see cref="AppHost"/> in current common workbench runtime.
    ''' </summary>
    ''' <param name="apphost"></param>
    Public Sub Hook(apphost As AppHost)
        _AppHost = apphost
    End Sub

    Public Sub LogText(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Information)
        End If
    End Sub

    Public Sub Warning(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Warning)
        End If
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, icon)
        End If
    End Sub

    Private Sub NoWorkbenchHostForm()
        Call MessageBox.Show("Unable to display the message content because no associated workstation host module could be found in the current runtime environment. Please link a host instance via a Hook function during the program's initialization phase.",
                             "Missing Host Configuration",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning)
    End Sub

    Public Function CenterToMain(target As Form) As Point
        Dim sizeBack = AppHost.GetClientSize
        Dim posBase = AppHost.GetDesktopLocation
        Dim sizeFore = target.Size

        Return New Point(
            posBase.X + (sizeBack.Width - sizeFore.Width) / 2,
            posBase.Y + (sizeBack.Height - sizeFore.Height) / 2
        )
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="showExplorer">
    ''' do specific callback from this parameter delegate if the pointer value is nothing nothing
    ''' </param>
    Public Function ShowSingleDocument(Of T As {New, DockContent})(Optional showExplorer As Action = Nothing) As T
        Dim DockPanel As DockPanel = DirectCast(AppHost.GetDockPanel, DockPanel)
        Dim targeted As T = DockPanel.Documents _
            .Where(Function(doc) TypeOf doc Is T) _
            .FirstOrDefault

        If targeted Is Nothing Then
            targeted = New T
        End If

        If Not showExplorer Is Nothing Then
            Call showExplorer()
        End If

        targeted.Show(DockPanel)
        targeted.DockState = DockState.Document

        Return targeted
    End Function

    Public Sub Dock(win As ToolWindow, prefer As DockState)
        Select Case win.DockState
            Case DockState.Hidden, DockState.Unknown
                win.DockState = prefer
            Case DockState.Float, DockState.Document,
                 DockState.DockTop,
                 DockState.DockRight,
                 DockState.DockLeft,
                 DockState.DockBottom

                ' do nothing 
            Case DockState.DockBottomAutoHide
                win.DockState = DockState.DockBottom
            Case DockState.DockLeftAutoHide
                win.DockState = DockState.DockLeft
            Case DockState.DockRightAutoHide
                win.DockState = DockState.DockRight
            Case DockState.DockTopAutoHide
                win.DockState = DockState.DockTop
        End Select
    End Sub

    ''' <summary>
    ''' create a new document tab page
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ShowDocument(Of T As {New, DocumentWindow})(Optional status As DockState = DockState.Document, Optional title As String = Nothing) As T
        Return ShowDocument(GetType(T), status, title)
    End Function

    Public Function ShowDocument(docType As Type,
                                 Optional status As DockState = DockState.Document,
                                 Optional title As String = Nothing) As DocumentWindow

        Dim newDoc As DocumentWindow = Activator.CreateInstance(docType)

        newDoc.Show(AppHost.GetDockPanel)
        newDoc.DockState = status

        If Not title.StringEmpty Then
            newDoc.TabText = title
        End If

        Return newDoc
    End Function

    Public Function ShowDocument(page As DocumentWindow, Optional status As DockState = DockState.Document) As DocumentWindow
        page.Show(AppHost.GetDockPanel)
        page.DockState = status
        Return page
    End Function
End Module
