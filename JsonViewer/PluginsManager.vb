Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Reflection
Imports System.IO

Namespace EPocalipse.Json.Viewer
    Friend Class PluginsManager
        Private plugins As List(Of IJsonViewerPlugin) = New List(Of IJsonViewerPlugin)()
        Private textVisualizersField As List(Of ICustomTextProvider) = New List(Of ICustomTextProvider)()
        Private visualizersField As List(Of IJsonVisualizer) = New List(Of IJsonVisualizer)()
        Private _defaultVisualizer As IJsonVisualizer

        Public Sub New()
        End Sub

        Public Sub Initialize()
            Try
                Dim myDirectory As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                'AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;

                Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location)
                If config Is Nothing Then InitDefaults()
                Dim viewerConfig = CType(config.GetSection("jsonViewer"), ViewerConfiguration)
                InternalConfig(viewerConfig)
            Catch
                InitDefaults()
            End Try
        End Sub

        Private Sub InitDefaults()
            If _defaultVisualizer Is Nothing Then
                AddPlugin(New AjaxNetDateTime())
                AddPlugin(New CustomDate())
            End If
        End Sub

        Private Sub InternalConfig(viewerConfig As ViewerConfiguration)
            If viewerConfig IsNot Nothing Then
                For Each keyValue As KeyValueConfigurationElement In viewerConfig.Plugins
                    Dim type = keyValue.Value
                    Dim pluginType = System.Type.GetType(type, False)
                    If pluginType IsNot Nothing AndAlso GetType(IJsonViewerPlugin).IsAssignableFrom(pluginType) Then
                        Try
                            Dim plugin = CType(Activator.CreateInstance(pluginType), IJsonViewerPlugin)
                            AddPlugin(plugin)
                        Catch
                            'Silently ignore any errors in plugin creation
                        End Try
                    End If
                Next
            End If
        End Sub

        Private Sub AddPlugin(plugin As IJsonViewerPlugin)
            plugins.Add(plugin)
            If TypeOf plugin Is ICustomTextProvider Then textVisualizersField.Add(CType(plugin, ICustomTextProvider))
            If TypeOf plugin Is IJsonVisualizer Then
                If _defaultVisualizer Is Nothing Then _defaultVisualizer = CType(plugin, IJsonVisualizer)
                visualizersField.Add(CType(plugin, IJsonVisualizer))
            End If
        End Sub

        Public ReadOnly Property TextVisualizers As IEnumerable(Of ICustomTextProvider)
            Get
                Return textVisualizersField
            End Get
        End Property

        Public ReadOnly Property Visualizers As IEnumerable(Of IJsonVisualizer)
            Get
                Return visualizersField
            End Get
        End Property

        Public ReadOnly Property DefaultVisualizer As IJsonVisualizer
            Get
                Return _defaultVisualizer
            End Get
        End Property
    End Class
End Namespace
