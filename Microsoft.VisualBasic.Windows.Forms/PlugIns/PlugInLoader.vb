Imports System.Reflection
Imports Microsoft.VisualBasic.Windows.Forms.PlugIns.Attributes
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.SoftwareToolkits

Namespace PlugIns

    Public NotInheritable Class PlugInLoader

        Public ReadOnly Property Menu As MenuStrip
        Public ReadOnly Property DLL As String

        Sub New(menu As MenuStrip, dll As String)
            Me.Menu = menu
            Me.DLL = dll
        End Sub

        ''' <summary>
        ''' 加载插件命令
        ''' </summary>
        ''' <returns>返回成功加载的命令的数目</returns>
        ''' <remarks></remarks>
        Public Function Load() As PlugInEntry()
            Dim lstPlugIns As PlugInEntry() = LoadMainModules(DLL)  'Get the plugin entry module.(获取插件主模块)

            If lstPlugIns Is Nothing Then
                Return Nothing
            End If

            For Each plugin As PlugInEntry In lstPlugIns
                Call __init(plugin)
            Next

            Return lstPlugIns
        End Function

        Private Sub __init(ByRef plugin As PlugInEntry)
            Dim initFlag As EntryFlag = plugin.GetEntry(EntryTypes.Initialize)
            Dim host As System.Windows.Forms.Form = Menu.FindForm

            If Not plugin.base.ShowOnMenu Then ' When the showonmenu property of this plugin entry is false, then this plugin will not load on the form menu but a initialize method is required.
                If Not initFlag Is Nothing Then
                    Call ReflectionAPI.Invoke(New Object() {host}, initFlag.Target)
                    Return
                Else
                    Return  ' This plugin assembly have no initialize method nor name property, i really don't know how to processing it, so i treat is as nothing.
                End If
            Else
                If Not initFlag Is Nothing Then
                    Call ReflectionAPI.Invoke(New Object() {host}, initFlag.Target)
                End If
            End If

            Dim IconLoader As EntryFlag = plugin.GetEntry(EntryTypes.IconLoader)
            Dim PluginCommandType As Type = GetType(PlugInCommand)
            Dim LQuery = From Method As MethodInfo
                         In plugin.MainModule.GetMethods
                         Let attrs As Object() = Method.GetCustomAttributes(PluginCommandType, False)
                         Where attrs.Length = 1
                         Let attr As Attributes.PlugInCommand = DirectCast(attrs(0), Attributes.PlugInCommand)
                         Let command As PlugInCommand = New PlugInCommand(attr, Method)
                         Select command
                         Order By command.base.Path Descending  'Load the available plugin commands.(加载插件模块中可用的命令)

            Dim MenuEntry = New ToolStripMenuItem() With {.Text = plugin.base.Name}   '生成入口点，并加载于UI之上

            Call Menu.Items.Add(MenuEntry)

            If IconLoader Is Nothing Then
                Dim resMgr As Resources = Resources.DirectLoadFrom(plugin.Assembly)  ' 由于都是从同一个dll文件之中加在出来的，所以都公用同一个资源管理器了
                IconLoader = New EntryFlag(IconLoader, resMgr)
            End If

            MenuEntry.Image = IconLoader.GetIcon(plugin.base.Icon)
            plugin.IconImage = MenuEntry.Image

            For Each Command As PlugInCommand In LQuery  '生成子菜单命令
                Dim Item As ToolStripMenuItem =
                    MenuAPI.AddCommand(MenuEntry, Command.base.Path, Command.base.Name)
                Item.Image = IconLoader.GetIcon(Command.base.Icon)
                AddHandler Item.Click, Sub() Command.Invoke(host)      '关联命令
            Next
        End Sub

        Protected Friend Shared Function LoadMainModules(dll As String) As PlugInEntry()
            If Not dll.FileExists Then
                Return Nothing
            Else
                dll = FileIO.FileSystem.GetFileInfo(dll).FullName
            End If

            Dim assembly As Assembly = Assembly.LoadFile(dll)
            Dim EntryType As Type = GetType(PlugInEntry)
            Dim FindModule As PlugInEntry() =
                LinqAPI.Exec(Of PlugInEntry) <= From type As Type
                                                In assembly.DefinedTypes
                                                Let attrs As Object() = type.GetCustomAttributes(EntryType, False)
                                                Where attrs.Length = 1
                                                Let attr As Attributes.PlugInEntry = DirectCast(attrs(Scan0), Attributes.PlugInEntry)
                                                Select New PlugInEntry(attr, type, assembly) '
            Return FindModule
        End Function
    End Class
End Namespace