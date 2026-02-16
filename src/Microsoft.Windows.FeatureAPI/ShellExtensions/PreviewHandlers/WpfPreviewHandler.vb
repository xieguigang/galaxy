Imports System.Windows.Controls
Imports System.Windows.Interop
Imports System.Windows.Media
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.ShellExtensions.Interop
Imports std = System.Math

Namespace ShellExtensions

    ''' <summary>
    ''' This is the base class for all WPF-based preview handlers and provides their basic functionality.
    ''' To create a custom preview handler that contains a WPF user control,
    ''' a class must derive from this, use the <see cref="PreviewHandlerAttribute"/>,
    ''' and implement 1 or more of the following interfaces: 
    ''' <see cref="IPreviewFromStream"/>, 
    ''' <see cref="IPreviewFromShellObject"/>, 
    ''' <see cref="IPreviewFromFile"/>.   
    ''' </summary>
    Public MustInherit Class WpfPreviewHandler
        Inherits PreviewHandler
        Implements IDisposable
        Private _source As HwndSource = Nothing
        Private _parentHandle As IntPtr = IntPtr.Zero
        Private _bounds As NativeRect

        ''' <summary>
        ''' This control must be populated by the deriving class before the preview is shown.
        ''' </summary>
        Public Property Control() As UserControl

        ''' <summary>
        ''' Throws an exception if the Control property has not been populated.
        ''' </summary>
        Protected Sub ThrowIfNoControl()
            If Control Is Nothing Then
                Throw New InvalidOperationException(GlobalLocalizedMessages.PreviewHandlerControlNotInitialized)
            End If
        End Sub

        ''' <summary>
        ''' Updates the placement of the Control.
        ''' </summary>
        Protected Sub UpdatePlacement()
            If _source IsNot Nothing Then
                HandlerNativeMethods.SetParent(_source.Handle, _parentHandle)

                HandlerNativeMethods.SetWindowPos(_source.Handle, New IntPtr(CInt(SetWindowPositionInsertAfter.Top)), 0, 0, std.Abs(_bounds.Left - _bounds.Right), std.Abs(_bounds.Top - _bounds.Bottom),
                    SetWindowPositionOptions.ShowWindow)
            End If
        End Sub

        Protected Overrides Sub SetParentHandle(handle As IntPtr)
            _parentHandle = handle
            UpdatePlacement()
        End Sub

        Protected Overrides Sub Initialize()
            If _source Is Nothing Then
                ThrowIfNoControl()

                Dim p As New HwndSourceParameters()
                p.WindowStyle = CInt(WindowStyles.Child Or WindowStyles.Visible Or WindowStyles.ClipSiblings)
                p.ParentWindow = _parentHandle
                p.Width = std.Abs(_bounds.Left - _bounds.Right)
                p.Height = std.Abs(_bounds.Top - _bounds.Bottom)

                _source = New HwndSource(p)
                _source.CompositionTarget.BackgroundColor = Brushes.WhiteSmoke.Color
                _source.RootVisual = DirectCast(Control.Content, Visual)
            End If
            UpdatePlacement()
        End Sub

        Protected Overrides ReadOnly Property Handle() As IntPtr
            Get
                If True Then
                    If _source Is Nothing Then
                        Throw New InvalidOperationException(GlobalLocalizedMessages.WpfPreviewHandlerNoHandle)
                    End If
                    Return _source.Handle
                End If
            End Get
        End Property

        Protected Overrides Sub UpdateBounds(bounds As NativeRect)
            _bounds = bounds
            UpdatePlacement()
        End Sub

        Protected Overrides Sub HandleInitializeException(caughtException As Exception)
            If caughtException Is Nothing Then
                Return
            End If

            Dim text As New TextBox() With {
                .IsReadOnly = True,
                .MaxLines = 20,
                .Text = caughtException.ToString()
            }
            Control = New UserControl() With {
                .Content = text
            }
        End Sub

        Protected Overrides Sub SetFocus()
            Control.Focus()
        End Sub

        Protected Overrides Sub SetBackground(argb As Integer)
            'a         
            'r
            'g
            Control.Background = New SolidColorBrush(Color.FromArgb(CByte((argb >> 24) And &HFF), CByte((argb >> 16) And &HFF), CByte((argb >> 8) And &HFF), CByte(argb And &HFF)))
            'b
        End Sub

        Protected Overrides Sub SetForeground(argb As Integer)
            'a                
            'r
            'g
            Control.Foreground = New SolidColorBrush(Color.FromArgb(CByte((argb >> 24) And &HFF), CByte((argb >> 16) And &HFF), CByte((argb >> 8) And &HFF), CByte(argb And &HFF)))
            'b                 
        End Sub

        Protected Overrides Sub SetFont(font As Interop.LogFont)
            If font Is Nothing Then
                Throw New ArgumentNullException("font")
            End If

            Control.FontFamily = New FontFamily(font.FaceName)
            Control.FontSize = font.Height
            Control.FontWeight = If(font.Weight > 0 AndAlso font.Weight < 1000, System.Windows.FontWeight.FromOpenTypeWeight(font.Weight), System.Windows.FontWeights.Normal)
        End Sub

#Region "IDisposable Members"

        ''' <summary>
        ''' Preview handler control finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Disposes the control
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Provides means to dispose the object.
        ''' When overriden, it is imperative that base.Dispose(true) is called within the implementation.
        ''' </summary>
        ''' <param name="disposing"></param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing AndAlso _source IsNot Nothing Then
                _source.Dispose()
            End If
        End Sub

#End Region

    End Class
End Namespace
