Namespace ModernTreeListView

    ''' <summary>
    ''' Defines a column in the ModernTreeListView.
    ''' </summary>
    Public NotInheritable Class TreeListColumn(Of TModel)
        Public Property Title As String
        Public Property Width As Integer
        Public Property Getter As Func(Of TModel, Object)
        Public Property Formatter As Func(Of Object, String)
        Public Property Alignment As System.Windows.Forms.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Left

        ''' <summary>
        ''' Optional minimum width when the user resizes the column.
        ''' </summary>
        Public Property MinWidth As Integer = 36

        ''' <summary>
        ''' Whether cells in this column can be edited in place. Default: true.
        ''' </summary>
        Public Property IsEditable As Boolean = True

        ''' <summary>
        ''' Optional factory creating a custom in-place editor for this column.
        ''' Receives the model and the current cell value; return the (unparented) editor control.
        ''' Pair with <see cref="EditorValueExtractor"/> to read the value back on commit.
        ''' </summary>
        Public Property EditorFactory As Func(Of TModel, Object, System.Windows.Forms.Control)

        ''' <summary>
        ''' Optional delegate extracting the committed value from the editor control.
        ''' When null, built-in extraction is used (TextBox.Text, CheckBox.Checked, DateTimePicker.Value, ...).
        ''' </summary>
        Public Property EditorValueExtractor As Func(Of System.Windows.Forms.Control, Object)

        Friend Sub New(title As String, getter As Func(Of TModel, Object), width As Integer)
            Me.Title = title
            Me.Getter = getter
            Me.Width = Math.Max(width, MinWidth)
        End Sub
    End Class

End Namespace