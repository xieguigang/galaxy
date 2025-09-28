Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Windows.ShellExtensions.Interop
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Shell

Namespace ShellExtensions

    ''' <summary>
    ''' This is the base class for all WinForms-based preview handlers and provides their basic functionality.
    ''' To create a custom preview handler that contains a WinForms user control,
    ''' a class must derive from this, use the <see cref="PreviewHandlerAttribute"/>,
    ''' and implement 1 or more of the following interfaces: 
    ''' <see cref="IPreviewFromStream"/>, 
    ''' <see cref="IPreviewFromShellObject"/>, 
    ''' <see cref="IPreviewFromFile"/>.   
    ''' </summary>
    Public MustInherit Class WinFormsPreviewHandler
        Inherits PreviewHandler
        Implements IDisposable
        ''' <summary>
        ''' This control must be populated by the deriving class before the preview is shown.
        ''' </summary>
        Public Property Control() As UserControl

        Protected Sub ThrowIfNoControl()
            If Control Is Nothing Then
                Throw New InvalidOperationException(LocalizedMessages.PreviewHandlerControlNotInitialized)
            End If
        End Sub

        ''' <summary>
        ''' Called when an exception is thrown during itialization of the preview control.
        ''' </summary>
        ''' <param name="caughtException"></param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification:="The object remains reachable through the Controls collection which can be disposed at a later time.")>
        Protected Overrides Sub HandleInitializeException(caughtException As Exception)
            If caughtException Is Nothing Then
                Throw New ArgumentNullException("caughtException")
            End If

            Control = New UserControl()
            Control.Controls.Add(
                New TextBox() With {
                    .[ReadOnly] = True,
                    .Multiline = True,
                    .Dock = DockStyle.Fill,
                    .Text = caughtException.ToString(),
                    .BackColor = Color.OrangeRed
           })
        End Sub

        Protected Overrides Sub UpdateBounds(bounds As NativeRect)
            Control.Bounds = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom)
            Control.Visible = True
        End Sub

        Protected Overrides Sub SetFocus()
            Control.Focus()
        End Sub

        Protected Overrides Sub SetBackground(argb As Integer)
            Control.BackColor = Color.FromArgb(argb)
        End Sub

        Protected Overrides Sub SetForeground(argb As Integer)
            Control.ForeColor = Color.FromArgb(argb)
        End Sub

        Protected Overrides Sub SetFont(font__1 As Interop.LogFont)
            Control.Font = Font.FromLogFont(font__1)
        End Sub

        Protected Overrides ReadOnly Property Handle() As IntPtr
            Get
                If True Then
                    Return Control.Handle
                End If
            End Get
        End Property

        Protected Overrides Sub SetParentHandle(handle As IntPtr)
            HandlerNativeMethods.SetParent(Control.Handle, handle)
        End Sub

#Region "IDisposable Members"

        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing AndAlso Control IsNot Nothing Then
                Control.Dispose()
            End If
        End Sub

#End Region
    End Class
End Namespace
