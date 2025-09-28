Namespace JSON.Plugin
    Friend Class PluginsManager
        Private plugins As List(Of IJsonViewerPlugin) = New List(Of IJsonViewerPlugin)()
        Private textVisualizersField As List(Of ICustomTextProvider) = New List(Of ICustomTextProvider)()
        Private visualizersField As List(Of IJsonVisualizer) = New List(Of IJsonVisualizer)()
        Private _defaultVisualizer As IJsonVisualizer

        Public Sub New()
        End Sub

        Public Sub Initialize()
            InitDefaults()
        End Sub

        Private Sub InitDefaults()
            If _defaultVisualizer Is Nothing Then
                AddPlugin(New AjaxNetDateTime())
                AddPlugin(New CustomDate())
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
