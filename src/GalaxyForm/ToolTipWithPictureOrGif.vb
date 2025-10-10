Imports System.Drawing.Imaging

''' <summary>
''' https://github.com/Kurris/ToolTipWithPictureOrGif
''' </summary>
Public Class ToolTipWithPictureOrGif
    Inherits ToolTip
    ''' <summary>
    ''' 自定义控件,用于在tooltip中显示图片
    ''' </summary>
    Public Sub New()
        '必须设置该值为true才会调用draw事件
        OwnerDraw = True

        IsBalloon = False
        ToolTipIcon = ToolTipIcon.None
        AddHandler Popup, AddressOf CustomToolTip_Popup
        AddHandler Draw, AddressOf CustomToolTip_Draw
    End Sub

    ''' <summary>
    ''' 显示的图片
    ''' </summary>
    Private _showImage As Image

    ''' <summary>
    ''' 展示大小
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CustomToolTip_Popup(sender As Object, e As PopupEventArgs)
        e.ToolTipSize = _showImage.Size
    End Sub

    ''' <summary>
    ''' 自定义显示图片
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CustomToolTip_Draw(sender As Object, e As DrawToolTipEventArgs)
        If _showImage Is Nothing Then
            Throw New Exception("图片不能为空!")
        End If
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        Dim fd As FrameDimension = New FrameDimension(_showImage.FrameDimensionsList(0))

        '帧数
        Dim iCount = _showImage.GetFrameCount(fd)
        '帧数为1即为图片
        If iCount = 1 Then
            e.Graphics.DrawImage(_showImage, e.Bounds)
            Return
        End If

        While True
            For i = 0 To iCount - 1
                '选择当前帧画面
                _showImage.SelectActiveFrame(fd, i)
                'Tool
                e.Graphics.DrawImage(_showImage, e.Bounds)
                Threading.Thread.Sleep(100)
                Call Application.DoEvents()
            Next
        End While
    End Sub

    ''' <summary>
    ''' 绑定控件显示的图片
    ''' </summary>
    ''' <param name="ctrl">控件</param>
    ''' <param name="image">图片</param>
    Public Sub Binding(ctrl As Control, image As Image, Optional title As String = "Tooltip")
        _showImage = image
        'whatever you set 
        SetToolTip(ctrl, title)
    End Sub
End Class
