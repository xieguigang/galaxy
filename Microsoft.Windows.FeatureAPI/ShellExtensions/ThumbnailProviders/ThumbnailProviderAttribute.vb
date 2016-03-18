
Namespace ShellExtensions
	''' <summary>    
	''' This class attribute is applied to a Thumbnail Provider to specify registration parameters
	''' and aesthetic attributes.
	''' </summary>
	<AttributeUsage(AttributeTargets.[Class], AllowMultiple := False, Inherited := False)> _
	Public NotInheritable Class ThumbnailProviderAttribute
		Inherits Attribute
        ''' <summary>
        ''' Creates a new instance of the attribute.
        ''' </summary>
        ''' <param name="name__1">Name of the provider</param>
        ''' <param name="extensions__2">Semi-colon-separated list of extensions supported by this provider.</param>
        Public Sub New(name__1 As String, extensions__2 As String)
			If name__1 Is Nothing Then
				Throw New ArgumentNullException("name")
			End If
			If extensions__2 Is Nothing Then
				Throw New ArgumentNullException("extensions")
			End If

			Name = name__1
			Extensions = extensions__2

			DisableProcessIsolation = False
			ThumbnailCutoff = ThumbnailCutoffSize.Square20
			TypeOverlay = Nothing
			ThumbnailAdornment = ThumbnailAdornment.[Default]
		End Sub

		''' <summary>
		''' Gets the name of the provider
		''' </summary>
		Public Property Name() As String
			Get
				Return m_Name
			End Get
			Private Set
				m_Name = Value
			End Set
		End Property
		Private m_Name As String

		''' <summary>
		''' Gets the semi-colon-separated list of extensions supported by the provider.
		''' </summary>
		Public Property Extensions() As String
			Get
				Return m_Extensions
			End Get
			Private Set
				m_Extensions = Value
			End Set
		End Property
		Private m_Extensions As String

        ' optional parameters below.

        ''' <summary>
        ''' Opts-out of running within the surrogate process DllHost.exe.
        ''' This will reduce robustness and security.
        ''' This value should be true if the provider does not implement <see cref="IThumbnailFromStream"/> 
        ''' Note: The msdn documentation and property name are contradicting.
        ''' http://msdn.microsoft.com/en-us/library/cc144118(VS.85).aspx
        ''' </summary>
        Public Property DisableProcessIsolation() As Boolean
            Get
                Return m_DisableProcessIsolation
            End Get
            Set
                m_DisableProcessIsolation = Value
            End Set
        End Property
        Private m_DisableProcessIsolation As Boolean
		' If true: Makes it run IN PROCESS.

		''' <summary>
		''' Below this size thumbnail images will not be generated - file icons will be used instead.
		''' </summary>
		Public Property ThumbnailCutoff() As ThumbnailCutoffSize
			Get
				Return m_ThumbnailCutoff
			End Get
			Set
				m_ThumbnailCutoff = Value
			End Set
		End Property
		Private m_ThumbnailCutoff As ThumbnailCutoffSize

		''' <summary>
		''' A resource reference string pointing to the icon to be used as an overlay on the bottom right of the thumbnail.
		''' ex. ISVComponent.dll@,-155
		''' ex. C:\Windows\System32\SampleIcon.ico
		''' If an empty string is provided, no overlay will be used.
		''' If the property is set to null, the default icon for the associated icon will be used as an overlay.
		''' </summary>
		Public Property TypeOverlay() As String
			Get
				Return m_TypeOverlay
			End Get
			Set
				m_TypeOverlay = Value
			End Set
		End Property
		Private m_TypeOverlay As String

        ''' <summary>
        ''' Specifies the <see cref="ThumbnailAdornment"/> for the thumbnail.
        ''' <remarks>
        ''' Only 32bpp bitmaps support adornments. 
        ''' While 24bpp bitmaps will be displayed, their adornments will not.
        ''' If an adornment is specified by the file-type's associated application, 
        ''' the applications adornment will override the value specified in this registration.</remarks>
        ''' </summary>
        Public Property ThumbnailAdornment() As ThumbnailAdornment
			Get
				Return m_ThumbnailAdornment
			End Get
			Set
				m_ThumbnailAdornment = Value
			End Set
		End Property
		Private m_ThumbnailAdornment As ThumbnailAdornment
	End Class

	''' <summary>
	''' Defines the minimum thumbnail size for which thumbnails will be generated.
	''' </summary>
	Public Enum ThumbnailCutoffSize
		''' <summary>
		''' Default size of 20x20
		''' </summary>
		Square20 = -1
		'For 20x20, you do not add any key in the registry
		''' <summary>
		''' Size of 32x32
		''' </summary>
		Square32 = 0

		''' <summary>
		''' Size of 16x16
		''' </summary>
		Square16 = 1

		''' <summary>
		''' Size of 48x48
		''' </summary>
		Square48 = 2

		''' <summary>
		''' Size of 16x16. An alternative to Square16.
		''' </summary>
		Square16B = 3
	End Enum

	''' <summary>
	''' Adornment applied to thumbnails.
	''' </summary>
	Public Enum ThumbnailAdornment
		''' <summary>
		''' This will use the associated application's default icon as the adornment.
		''' </summary>
		[Default] = -1
		' Default behaviour for no value added in registry
		''' <summary>
		''' No adornment
		''' </summary>
		None = 0

		''' <summary>
		''' Drop shadow adornment
		''' </summary>
		DropShadow = 1

		''' <summary>
		''' Photo border adornment
		''' </summary>
		PhotoBorder = 2

		''' <summary>
		''' Video sprocket adornment
		''' </summary>
		VideoSprockets = 3
	End Enum


End Namespace
