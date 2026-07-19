Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Galaxy.Workbench.CommonDialogs
Imports Galaxy.Workbench.Container
Imports Galaxy.Workbench.DockDocument
Imports Galaxy.Workbench.DockDocument.Presets
Imports Microsoft.VisualBasic.ComponentModel.Collection
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

    ReadOnly toolWindows As New Dictionary(Of String, ToolWindow)

    Public ReadOnly Property IsDevelopmentMode As Boolean
        Get
            Return Container.AppEnvironment.IsDevelopmentMode
        End Get
    End Property

    Public ReadOnly Property UISettings As UISettings
        Get
            Static ui As UISettings = Nothing

            If ui Is Nothing Then
                ui = UISettings.LoadSettings
            End If

            Return ui
        End Get
    End Property

    Dim outputWindow As OutputWindow
    Dim propertyWindow As PropertyWindow

    Public Function GetPropertyWindow() As PropertyWindow
        If AppHost Is Nothing Then
            Call NoWorkbenchHostForm()
        End If

        If propertyWindow Is Nothing OrElse propertyWindow.IsDisposed Then
            propertyWindow = New PropertyWindow
            propertyWindow.Name = "property_win"
            propertyWindow.TabText = "Property Window"
            propertyWindow.Show(AppHost.GetDockPanel, DockState.DockRightAutoHide)

            Call RegisterToolWindow(propertyWindow)
        End If

        Return propertyWindow
    End Function

    Public Function GetOutputWindow() As OutputWindow
        If AppHost Is Nothing Then
            Call NoWorkbenchHostForm()
        End If

        If outputWindow Is Nothing OrElse outputWindow.IsDisposed Then
            outputWindow = New OutputWindow
            outputWindow.Name = "output_win"
            outputWindow.TabText = "Output Window"
            outputWindow.Show(AppHost.GetDockPanel, DockState.DockBottomAutoHide)

            Call RegisterToolWindow(outputWindow)
        End If

        Return outputWindow
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub RegisterOutputWindow()
        Call GetOutputWindow()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub RegisterPropertyWindow()
        Call GetPropertyWindow()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function TryGetToolWindow(name As String) As ToolWindow
        Return toolWindows.TryGetValue(name)
    End Function

    ''' <summary>
    ''' set value to the <see cref="AppHost"/> in current common workbench runtime.
    ''' </summary>
    ''' <param name="apphost">should be the main window of the current process application</param>
    ''' <remarks>
    ''' this method andalso apply of the <see cref="UISettings"/> to the <see cref="AppHost"/> object 
    ''' if the given <paramref name="apphost"/> is a winform object.
    ''' </remarks>
    Public Sub Hook(apphost As AppHost)
        _AppHost = apphost

        If apphost IsNot Nothing AndAlso apphost.GetType.IsInheritsFrom(GetType(Form), strict:=False) Then
            Dim form As Form = DirectCast(apphost, Form)

            If UISettings.rememberLocation Then
                If UISettings.left <= 0 Then UISettings.left = 100
                If UISettings.top <= 0 Then UISettings.top = 100
                If UISettings.width <= 0 Then UISettings.width = 1000
                If UISettings.height <= 0 Then UISettings.height = 800

                form.Location = New Point(UISettings.left, UISettings.top)
                form.Size = New Size(UISettings.width, UISettings.height)

                If UISettings.windowState <> FormWindowState.Normal Then
                    form.WindowState = UISettings.windowState
                End If
            End If

            Call Microsoft.Windows.Hook(form)
        End If
    End Sub

    Public Sub RegisterToolWindow(tool As ToolWindow, Optional dock As DockState = DockState.DockLeftAutoHide)
        If Not tool.Name.StringEmpty(, True) Then
            toolWindows(tool.Name) = tool
            toolWindows(tool.Name).Show(AppHost.GetDockPanel, dock)

            Dim setting As DockSettings = UISettings.windows.KeyItem(tool.Name)

            If Not setting Is Nothing Then
                Call setting.ApplySettings(tool)
            End If

            Call CommonRuntime.Dock(tool, prefer:=dock)
        End If
    End Sub

    Public Sub SaveUISettings()
        If AppHost IsNot Nothing AndAlso UISettings IsNot Nothing Then
            Dim size As Size = AppHost.GetClientSize
            Dim position As Point = AppHost.GetDesktopLocation

            UISettings.windows = DockSettings.GetSettings(toolWindows.Values).ToArray
            UISettings.windowState = AppHost.GetWindowState
            UISettings.width = size.Width
            UISettings.height = size.Height
            UISettings.left = position.X
            UISettings.top = position.Y
            UISettings.Save()
        End If
    End Sub

    Public Sub LogText(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Information)

            If Not outputWindow Is Nothing Then
                Call outputWindow.AppendLine(msg)
            End If
        End If
    End Sub

    Public Sub Warning(msg As String)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, Icons8.Warning)

            If Not outputWindow Is Nothing Then
                Call outputWindow.AppendLine(msg)
            End If
        End If
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing)
        If AppHost Is Nothing Then
            NoWorkbenchHostForm()
        Else
            Call AppHost.StatusMessage(msg, icon)

            If Not outputWindow Is Nothing Then
                Call outputWindow.AppendLine(msg)
            End If
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
    ''' Save current active document as file if it supported to be saved
    ''' </summary>
    Public Sub SaveCurrentDocument()
        Dim doc As DocumentWindow = TryCast(AppHost.ActiveDocument, DocumentWindow)

        If Not doc Is Nothing Then
            Try
                Call doc.SaveDocument()
            Catch ex As Exception
                Call Warning(ex.Message)
                Call App.LogException(ex)
            End Try
        End If
    End Sub

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

    Dim XorKey As Byte

    Public Sub SetSeed(key As Byte)
        XorKey = key
    End Sub

    ''' <summary>
    ''' 在内存中组装并解密获取RSA公钥
    ''' </summary>
    Public Function AssembleKey(ParamArray keyParts As String()) As String
        Try
            ' 1. 在内存中拼接片段
            Dim combinedBase64 As String = String.Concat(keyParts)
            ' 2. Base64 解码为字节数组
            Dim encryptedBytes As Byte() = Convert.FromBase64String(combinedBase64)

            ' 3. 进行 XOR 解密 (XOR加密和解密是互逆的，再异或一次就还原了)
            For i As Integer = 0 To encryptedBytes.Length - 1
                encryptedBytes(i) = encryptedBytes(i) Xor XorKey
            Next

            ' 4. 将字节数组转换回原始的XML字符串
            Return Encoding.UTF8.GetString(encryptedBytes)
        Catch ex As Exception
            ' 如果解密失败（说明数组被篡改或损坏），返回空字符串或抛出异常
            ' 工业软件中建议静默处理，不要给破解者明确提示
            Return String.Empty
        End Try
    End Function
End Module
