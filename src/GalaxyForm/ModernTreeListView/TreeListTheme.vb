Namespace ModernTreeListView

    ''' <summary>
    ''' A bundle of all visual colors used by <see cref="ModernTreeListView(Of TModel)"/>.
    ''' Apply via <c>ApplyTheme</c> or the <c>Theme</c> property; use <see cref="Light"/> / <see cref="Dark"/> presets as starting points.
    ''' </summary>
    Public NotInheritable Class TreeListTheme
        Public Property BackColor As Drawing.Color
        Public Property ForeColor As Drawing.Color
        Public Property HeaderBackColor As Drawing.Color
        Public Property HeaderForeColor As Drawing.Color
        Public Property RowBackColor As Drawing.Color
        Public Property AlternatingRowBackColor As Drawing.Color
        Public Property SelectionBackColor As Drawing.Color
        Public Property SelectionForeColor As Drawing.Color
        Public Property GridLineColor As Drawing.Color
        Public Property ExpanderColor As Drawing.Color
        Public Property TreeLineColor As Drawing.Color
        Public Property HoverBackColor As Drawing.Color
        Public Property EditorBackColor As Drawing.Color
        Public Property EditorForeColor As Drawing.Color
        Public Property FocusCueColor As Drawing.Color

        ''' <summary>
        ''' Clean light preset (the control's defaults).
        ''' </summary>
        Public Shared ReadOnly Property Light As TreeListTheme
            Get
                Return New TreeListTheme() With {
                    .BackColor = Drawing.Color.White,
                    .ForeColor = Drawing.Color.FromArgb(33, 37, 41),
                    .HeaderBackColor = Drawing.Color.FromArgb(247, 248, 250),
                    .HeaderForeColor = Drawing.Color.FromArgb(52, 58, 64),
                    .RowBackColor = Drawing.Color.White,
                    .AlternatingRowBackColor = Drawing.Color.FromArgb(250, 251, 252),
                    .SelectionBackColor = Drawing.Color.FromArgb(0, 120, 212),
                    .SelectionForeColor = Drawing.Color.White,
                    .GridLineColor = Drawing.Color.FromArgb(234, 236, 239),
                    .ExpanderColor = Drawing.Color.FromArgb(108, 117, 125),
                    .TreeLineColor = Drawing.Color.FromArgb(206, 212, 218),
                    .HoverBackColor = Drawing.Color.FromArgb(241, 243, 245),
                    .EditorBackColor = Drawing.Color.White,
                    .EditorForeColor = Drawing.Color.FromArgb(33, 37, 41),
                    .FocusCueColor = Drawing.Color.FromArgb(100, 0, 120, 212)
                }
            End Get
        End Property

        ''' <summary>
        ''' Modern dark preset (VS Code-like palette).
        ''' </summary>
        Public Shared ReadOnly Property Dark As TreeListTheme
            Get
                Return New TreeListTheme() With {
                    .BackColor = Drawing.Color.FromArgb(30, 30, 30),
                    .ForeColor = Drawing.Color.FromArgb(232, 232, 232),
                    .HeaderBackColor = Drawing.Color.FromArgb(45, 45, 48),
                    .HeaderForeColor = Drawing.Color.FromArgb(208, 212, 217),
                    .RowBackColor = Drawing.Color.FromArgb(37, 37, 38),
                    .AlternatingRowBackColor = Drawing.Color.FromArgb(42, 42, 43),
                    .SelectionBackColor = Drawing.Color.FromArgb(10, 93, 171),
                    .SelectionForeColor = Drawing.Color.White,
                    .GridLineColor = Drawing.Color.FromArgb(63, 65, 68),
                    .ExpanderColor = Drawing.Color.FromArgb(160, 166, 173),
                    .TreeLineColor = Drawing.Color.FromArgb(74, 77, 82),
                    .HoverBackColor = Drawing.Color.FromArgb(51, 52, 55),
                    .EditorBackColor = Drawing.Color.FromArgb(45, 45, 48),
                    .EditorForeColor = Drawing.Color.FromArgb(232, 232, 232),
                    .FocusCueColor = Drawing.Color.FromArgb(100, 86, 156, 214)
                }
            End Get
        End Property
    End Class

End Namespace