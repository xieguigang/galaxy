Imports System.Drawing
Imports System.IO
Imports Microsoft.Windows.Shell

Namespace ShellExtensions
    ''' <summary>
    ''' This interface exposes the ConsructBitmap function for initializing the 
    ''' Thumbnail Provider with a <see cref="Stream"/> 
    ''' If this interfaces is not used, then the handler must opt out of process isolation.
    ''' This interface can be used in conjunction with the other intialization interfaces,
    ''' but only 1 will be accessed according to the priorities preset by the Windows Shell:
    ''' <see cref="IThumbnailFromStream"/>
    ''' <see cref="IThumbnailFromShellObject"/>
    ''' <see cref="IThumbnailFromFile"/>
    ''' </summary>
    Public Interface IThumbnailFromStream
        ''' <summary>
        ''' Provides the <see cref="Stream"/> to the item from which a thumbnail should be created.
        ''' <remarks>Only 32bpp bitmaps support adornments. 
        ''' While 24bpp bitmaps will be displayed they will not display adornments.
        ''' Additional guidelines for developing thumbnails can be found at http://msdn.microsoft.com/en-us/library/cc144115(v=VS.85).aspx
        ''' </remarks>
        ''' </summary>
        ''' <param name="stream">Stream to initialize the thumbnail</param>
        ''' <param name="sideSize">Square side dimension in which the thumbnail should fit; the thumbnail will be scaled otherwise.</param>
        ''' <returns></returns>
        Function ConstructBitmap(stream As Stream, sideSize As Integer) As Bitmap
	End Interface

    ''' <summary>
    ''' This interface exposes the ConsructBitmap function for initializing the 
    ''' Thumbnail Provider with a <see cref="ShellObject"/>.
    ''' This interface can be used in conjunction with the other intialization interfaces,
    ''' but only 1 will be accessed according to the priorities preset by the Windows Shell:
    ''' <see cref="IThumbnailFromStream"/>
    ''' <see cref="IThumbnailFromShellObject"/>
    ''' <see cref="IThumbnailFromFile"/>
    ''' </summary>
    Public Interface IThumbnailFromShellObject
        ''' <summary>
        ''' Provides the <see cref="ShellObject"/> to the item from which a thumbnail should be created.
        ''' <remarks>Only 32bpp bitmaps support adornments. 
        ''' While 24bpp bitmaps will be displayed they will not display adornments.
        ''' Additional guidelines for developing thumbnails can be found at http://msdn.microsoft.com/en-us/library/cc144115(v=VS.85).aspx
        ''' </remarks>
        ''' </summary>
        ''' <param name="shellObject">ShellObject to initialize the thumbnail</param>
        ''' <param name="sideSize">Square side dimension in which the thumbnail should fit; the thumbnail will be scaled otherwise.</param>
        ''' <returns>Generated thumbnail</returns>
        Function ConstructBitmap(shellObject As ShellObject, sideSize As Integer) As Bitmap
	End Interface

    ''' <summary>
    ''' This interface exposes the ConsructBitmap function for initializing the 
    ''' Thumbnail Provider with file information.
    ''' This interface can be used in conjunction with the other intialization interfaces,
    ''' but only 1 will be accessed according to the priorities preset by the Windows Shell:
    ''' <see cref="IThumbnailFromStream"/>
    ''' <see cref="IThumbnailFromShellObject"/>
    ''' <see cref="IThumbnailFromFile"/>
    ''' </summary>
    Public Interface IThumbnailFromFile
        ''' <summary>
        ''' Provides the <see cref="FileInfo"/> to the item from which a thumbnail should be created.
        ''' <remarks>Only 32bpp bitmaps support adornments. 
        ''' While 24bpp bitmaps will be displayed they will not display adornments.
        ''' Additional guidelines for developing thumbnails can be found at http://msdn.microsoft.com/en-us/library/cc144115(v=VS.85).aspx
        ''' </remarks>
        ''' </summary>
        ''' <param name="info">FileInfo to initialize the thumbnail</param>
        ''' <param name="sideSize">Square side dimension in which the thumbnail should fit; the thumbnail will be scaled otherwise.</param>
        ''' <returns>Generated thumbnail</returns>
        Function ConstructBitmap(info As FileInfo, sideSize As Integer) As Bitmap
	End Interface
End Namespace
