Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Reflection
Imports Microsoft.VisualBasic.Windows.Forms.PlugIns.Attributes
Imports Microsoft.VisualBasic.Language

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
        Public Function Load() As PlugInEntry
            Dim PlugInEntry = LoadMainModules(DLL)  'Get the plugin entry module.(获取插件主模块)

            If PlugInEntry Is Nothing Then
                Return Nothing
            End If

            Dim Initialize As EntryFlag = PlugInEntry.GetEntry(EntryTypes.Initialize)
            Dim Target As System.Windows.Forms.Form = Menu.FindForm

            If Not PlugInEntry.ShowOnMenu Then 'When the showonmenu property of this plugin entry is false, then this plugin will not load on the form menu but a initialize method is required.
                If Not Initialize Is Nothing Then
                    Call ReflectionAPI.Invoke(New Object() {Target}, Initialize.Target)

                    Return PlugInEntry
                Else
                    Return Nothing 'This plugin assembly have no initialize method nor name property, i really don't know how to processing it, so i treat is as nothing.
                End If
            Else
                If Not Initialize Is Nothing Then Call ReflectionAPI.Invoke(New Object() {Target}, Initialize.Target)
            End If

            Dim IconLoader As EntryFlag = PlugInEntry.GetEntry(EntryTypes.IconLoader)
            Dim MainModule = PlugInEntry.MainModule
            Dim PluginCommandType = GetType(PlugInCommand)
            Dim LQuery = From Method As MethodInfo In MainModule.GetMethods
                         Let attributes = Method.GetCustomAttributes(PluginCommandType, False)
                         Where attributes.Count = 1
                         Let command = DirectCast(attributes(0), PlugInCommand).Initialize(Method)
                         Select command Order By command.Path Descending  'Load the available plugin commands.(加载插件模块中可用的命令)

            Dim MenuEntry = New System.Windows.Forms.ToolStripMenuItem() With {.Text = PlugInEntry.Name}   '生成入口点，并加载于UI之上
            Call Menu.Items.Add(MenuEntry)

            If IconLoader Is Nothing Then
                Dim Instance = GetIconLoader(PlugInEntry.Assembly)
                Dim Method As MethodInfo = Instance.Last
                Dim Invoke As Func(Of String, Object) = Function(Name As String) Method.Invoke(Instance.First, New Object() {Name})
                IconLoader = New EntryFlag With {.GetIconInvoke = Invoke}
            End If

            MenuEntry.Image = IconLoader.GetIcon(PlugInEntry.Icon)
            PlugInEntry.IconImage = MenuEntry.Image

            For Each Command As PlugInCommand In LQuery.ToArray  '生成子菜单命令
                Dim Item As ToolStripMenuItem = AddCommand(MenuEntry, (From s As String In Command.Path.Split("\"c) Where Not String.IsNullOrEmpty(s) Select s).ToArray, Command.Name, p:=0)
                Item.Image = IconLoader.GetIcon(Command.Icon)
                AddHandler Item.Click, Sub() Command.Invoke(Target)      '关联命令
            Next

            Return PlugInEntry
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Assembly"></param>
        ''' <returns>{Resource Manager Instanc, GetObject MethodInfo}</returns>
        ''' <remarks></remarks>
        Private Shared Function GetIconLoader(Assembly As Assembly) As Object()
            Dim LQuery = From Type In Assembly.DefinedTypes Where String.Equals(Type.Name, "Resources") Select Type '
            LQuery = LQuery.ToArray
            If LQuery.Count > 0 Then
                Dim Resources = LQuery.First
                Dim ResourceManager As Object = (From Method In Resources.DeclaredMethods Where String.Equals("get_ResourceManager", Method.Name) Select Method).First.Invoke(Nothing, New Object() {})
                Dim ResourceManagerType As System.Type = ResourceManager.GetType
                Dim GetObject = (From Method In ResourceManagerType.GetMethods Where String.Equals("GetObject", Method.Name) Select Method).First
                Return New Object() {ResourceManager, GetObject}
            Else
                Return Nothing
            End If
        End Function

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