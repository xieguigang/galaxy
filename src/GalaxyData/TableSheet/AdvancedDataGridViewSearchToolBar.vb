#Region "License"
' Advanced DataGridView
'
' Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
' Original work Copyright (c), 2013 Zuby <zuby@me.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System.ComponentModel
Imports System.IO
Imports Galaxy.Data.TableSheet.Events
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace TableSheet

    <DesignerCategory("")>
    Partial Public Class AdvancedDataGridViewSearchToolBar
        Inherits ToolStrip

#Region "public events"

        Public Event Search As AdvancedDataGridViewSearchToolBarSearchEventHandler

#End Region


#Region "class properties"

        Private _columnsList As DataGridViewColumnCollection = Nothing

        Private Const ButtonCloseEnabled As Boolean = False

#End Region


#Region "translations"

        ''' <summary>
        ''' Available translation keys
        ''' </summary>
        Public Enum TranslationKey
            ADGVSTBLabelSearch
            ADGVSTBButtonFromBegin
            ADGVSTBButtonCaseSensitiveToolTip
            ADGVSTBButtonSearchToolTip
            ADGVSTBButtonCloseToolTip
            ADGVSTBButtonWholeWordToolTip
            ADGVSTBComboBoxColumnsAll
            ADGVSTBTextBoxSearchToolTip
        End Enum

        ''' <summary>
        ''' Internationalization strings
        ''' </summary>
        Public Shared Translations As Dictionary(Of String, String) = New Dictionary(Of String, String)() From {
    {TranslationKey.ADGVSTBLabelSearch.ToString(), "Search:"},
    {TranslationKey.ADGVSTBButtonFromBegin.ToString(), "From Begin"},
    {TranslationKey.ADGVSTBButtonCaseSensitiveToolTip.ToString(), "Case Sensitivity"},
    {TranslationKey.ADGVSTBButtonSearchToolTip.ToString(), "Find Next"},
    {TranslationKey.ADGVSTBButtonCloseToolTip.ToString(), "Hide"},
    {TranslationKey.ADGVSTBButtonWholeWordToolTip.ToString(), "Search only Whole Word"},
    {TranslationKey.ADGVSTBComboBoxColumnsAll.ToString(), "(All Columns)"},
    {TranslationKey.ADGVSTBTextBoxSearchToolTip.ToString(), "Value for Search"}
}

        ''' <summary>
        ''' Used to check if components translations has to be updated
        ''' </summary>
        Private _translationsRefreshComponentTranslationsCheck As Dictionary(Of String, String) = New Dictionary(Of String, String)() From {
        }

#End Region


#Region "constructor"

        ''' <summary>
        ''' AdvancedDataGridViewSearchToolBar constructor
        ''' </summary>
        Public Sub New()
            Call MyBase.New

            'initialize components
            InitializeComponent()

            RefreshComponentTranslations()

            'set default values
            If Not ButtonCloseEnabled Then Items.RemoveAt(0)
            comboBox_columns.SelectedIndex = 0
        End Sub

#End Region


#Region "translations methods"

        ''' <summary>
        ''' Set translation dictionary
        ''' </summary>
        ''' <param name="translations"></param>
        Public Shared Sub SetTranslations(translations As IDictionary(Of String, String))
            'set localization strings
            If translations IsNot Nothing Then
                For Each translation In translations
                    If AdvancedDataGridViewSearchToolBar.Translations.ContainsKey(translation.Key) Then AdvancedDataGridViewSearchToolBar.Translations(translation.Key) = translation.Value
                Next
            End If
        End Sub

        ''' <summary>
        ''' Get translation dictionary
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetTranslations() As IDictionary(Of String, String)
            Return Translations
        End Function

        ''' <summary>
        ''' Load translations from file
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        Public Shared Function LoadTranslationsFromFile(filename As String) As IDictionary(Of String, String)
            Dim ret As IDictionary(Of String, String) = New Dictionary(Of String, String)()

            If Not String.IsNullOrEmpty(filename) Then
                'deserialize the file
                Try
                    Dim jsontext = File.ReadAllText(filename)
                    Dim translations As Dictionary(Of String, String) = jsontext.LoadJSON(Of Dictionary(Of String, String))

                    For Each translation In translations
                        If Not ret.ContainsKey(translation.Key) AndAlso AdvancedDataGridViewSearchToolBar.Translations.ContainsKey(translation.Key) Then ret.Add(translation.Key, translation.Value)
                    Next

                Catch
                End Try
            End If

            'add default translations if not in files
            For Each translation In GetTranslations()
                If Not ret.ContainsKey(translation.Key) Then ret.Add(translation.Key, translation.Value)
            Next

            Return ret
        End Function

        ''' <summary>
        ''' Update components translations
        ''' </summary>
        Private Sub RefreshComponentTranslations()
            comboBox_columns.BeginUpdate()
            comboBox_columns.Items.Clear()
            comboBox_columns.Items.AddRange(New Object() {Translations(TranslationKey.ADGVSTBComboBoxColumnsAll.ToString())})
            If _columnsList IsNot Nothing Then
                For Each c As DataGridViewColumn In _columnsList
                    If c.Visible Then comboBox_columns.Items.Add(c.HeaderText)
                Next
            End If
            comboBox_columns.SelectedIndex = 0
            comboBox_columns.EndUpdate()
            button_close.ToolTipText = Translations(TranslationKey.ADGVSTBButtonCloseToolTip.ToString())
            label_search.Text = Translations(TranslationKey.ADGVSTBLabelSearch.ToString())
            textBox_search.ToolTipText = Translations(TranslationKey.ADGVSTBTextBoxSearchToolTip.ToString())
            button_frombegin.ToolTipText = Translations(TranslationKey.ADGVSTBButtonFromBegin.ToString())
            button_casesensitive.ToolTipText = Translations(TranslationKey.ADGVSTBButtonCaseSensitiveToolTip.ToString())
            button_search.ToolTipText = Translations(TranslationKey.ADGVSTBButtonSearchToolTip.ToString())
            button_wholeword.ToolTipText = Translations(TranslationKey.ADGVSTBButtonWholeWordToolTip.ToString())
            textBox_search.Text = textBox_search.ToolTipText
        End Sub

#End Region


#Region "button events"

        ''' <summary>
        ''' button Search Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub button_search_Click(sender As Object, e As EventArgs)
            If textBox_search.TextLength > 0 AndAlso Not Equals(textBox_search.Text, textBox_search.ToolTipText) AndAlso SearchEvent IsNot Nothing Then
                Dim c As DataGridViewColumn = Nothing
                If comboBox_columns.SelectedIndex > 0 AndAlso _columnsList IsNot Nothing AndAlso _columnsList.GetColumnCount(DataGridViewElementStates.Visible) > 0 Then
                    Dim cols As DataGridViewColumn() = _columnsList.Cast(Of DataGridViewColumn)().Where(Function(col) col.Visible).ToArray()

                    If cols.Length = comboBox_columns.Items.Count - 1 Then
                        If Equals(cols(comboBox_columns.SelectedIndex - 1).HeaderText, comboBox_columns.SelectedItem.ToString()) Then
                            c = cols(comboBox_columns.SelectedIndex - 1)
                        End If
                    End If
                End If

                Dim args As New AdvancedDataGridViewSearchToolBarSearchEventArgs(textBox_search.Text, c, button_casesensitive.Checked, button_wholeword.Checked, button_frombegin.Checked)
                RaiseEvent Search(Me, args)
            End If
        End Sub

        ''' <summary>
        ''' button Close Click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub button_close_Click(sender As Object, e As EventArgs)
            Hide()
        End Sub

#End Region


#Region "textbox search events"

        ''' <summary>
        ''' textBox Search TextChanged event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub textBox_search_TextChanged(sender As Object, e As EventArgs)
            button_search.Enabled = textBox_search.TextLength > 0 AndAlso Not Equals(textBox_search.Text, textBox_search.ToolTipText)
        End Sub


        ''' <summary>
        ''' textBox Search Enter event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub textBox_search_Enter(sender As Object, e As EventArgs)
            If Equals(textBox_search.Text, textBox_search.ToolTipText) AndAlso textBox_search.ForeColor = Color.LightGray Then
                textBox_search.Text = ""
            Else
                textBox_search.SelectAll()
            End If

            textBox_search.ForeColor = SystemColors.WindowText
        End Sub

        ''' <summary>
        ''' textBox Search Leave event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub textBox_search_Leave(sender As Object, e As EventArgs)
            If Equals(textBox_search.Text.Trim(), "") Then
                textBox_search.Text = textBox_search.ToolTipText
                textBox_search.ForeColor = Color.LightGray
            End If
        End Sub


        ''' <summary>
        ''' textBox Search KeyDown event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub textBox_search_KeyDown(sender As Object, e As KeyEventArgs)
            If textBox_search.TextLength > 0 AndAlso Not Equals(textBox_search.Text, textBox_search.ToolTipText) AndAlso e.KeyData = Keys.Enter Then
                button_search_Click(button_search, New EventArgs())
                e.SuppressKeyPress = True
                e.Handled = True
            End If
        End Sub

#End Region


#Region "public methods"

        ''' <summary>
        ''' Set Columns to search in
        ''' </summary>
        ''' <param name="columns"></param>
        Public Sub SetColumns(columns As DataGridViewColumnCollection)
            _columnsList = columns
            comboBox_columns.BeginUpdate()
            comboBox_columns.Items.Clear()
            comboBox_columns.Items.AddRange(New Object() {Translations(TranslationKey.ADGVSTBComboBoxColumnsAll.ToString())})
            If _columnsList IsNot Nothing Then
                For Each c As DataGridViewColumn In _columnsList
                    If c.Visible Then comboBox_columns.Items.Add(c.HeaderText)
                Next
            End If
            comboBox_columns.SelectedIndex = 0
            comboBox_columns.EndUpdate()
        End Sub

#End Region


#Region "resize events"

        ''' <summary>
        ''' Resize event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ResizeMe(sender As Object, e As EventArgs)
            SuspendLayout()
            Dim w1 = 150
            Dim w2 = 150
            Dim oldW = comboBox_columns.Width + textBox_search.Width
            For Each c As ToolStripItem In MyBase.Items
                c.Overflow = ToolStripItemOverflow.Never
                c.Visible = True
            Next

            Dim width = PreferredSize.Width - oldW + w1 + w2
            If MyBase.Width < width Then
                label_search.Visible = False
                GetResizeBoxSize(PreferredSize.Width - oldW + w1 + w2, w1, w2)
                width = PreferredSize.Width - oldW + w1 + w2

                If MyBase.Width < width Then
                    button_casesensitive.Overflow = ToolStripItemOverflow.Always
                    GetResizeBoxSize(PreferredSize.Width - oldW + w1 + w2, w1, w2)
                    width = PreferredSize.Width - oldW + w1 + w2
                End If

                If MyBase.Width < width Then
                    button_wholeword.Overflow = ToolStripItemOverflow.Always
                    GetResizeBoxSize(PreferredSize.Width - oldW + w1 + w2, w1, w2)
                    width = PreferredSize.Width - oldW + w1 + w2
                End If

                If MyBase.Width < width Then
                    button_frombegin.Overflow = ToolStripItemOverflow.Always
                    separator_search.Visible = False
                    GetResizeBoxSize(PreferredSize.Width - oldW + w1 + w2, w1, w2)
                    width = PreferredSize.Width - oldW + w1 + w2
                End If

                If MyBase.Width < width Then
                    comboBox_columns.Overflow = ToolStripItemOverflow.Always
                    textBox_search.Overflow = ToolStripItemOverflow.Always
                    w1 = 150
                    w2 = Math.Max(MyBase.Width - PreferredSize.Width - textBox_search.Margin.Left - textBox_search.Margin.Right, 75)
                    textBox_search.Overflow = ToolStripItemOverflow.Never
                    width = PreferredSize.Width - textBox_search.Width + w2
                End If
                If MyBase.Width < width Then
                    button_search.Overflow = ToolStripItemOverflow.Always
                    w2 = Math.Max(MyBase.Width - PreferredSize.Width + textBox_search.Width, 75)
                    width = PreferredSize.Width - textBox_search.Width + w2
                End If
                If MyBase.Width < width Then
                    button_close.Overflow = ToolStripItemOverflow.Always
                    textBox_search.Margin = New Padding(8, 2, 8, 2)
                    w2 = Math.Max(MyBase.Width - PreferredSize.Width + textBox_search.Width, 75)
                    width = PreferredSize.Width - textBox_search.Width + w2
                End If

                If MyBase.Width < width Then
                    w2 = Math.Max(MyBase.Width - PreferredSize.Width + textBox_search.Width, 20)
                    width = PreferredSize.Width - textBox_search.Width + w2
                End If
                If width > MyBase.Width Then
                    textBox_search.Overflow = ToolStripItemOverflow.Always
                    textBox_search.Margin = New Padding(0, 2, 8, 2)
                    w2 = 150
                End If
            Else
                GetResizeBoxSize(width, w1, w2)
            End If

            If comboBox_columns.Width <> w1 Then comboBox_columns.Width = w1
            If textBox_search.Width <> w2 Then textBox_search.Width = w2

            ResumeLayout()
        End Sub

        ''' <summary>
        ''' Get a Resize Size for a box
        ''' </summary>
        ''' <param name="width"></param>
        ''' <param name="w1"></param>
        ''' <param name="w2"></param>
        Private Sub GetResizeBoxSize(width As Integer, ByRef w1 As Integer, ByRef w2 As Integer)
            Dim dif As Integer = Math.Round((width - MyBase.Width) / 2.0, 0, MidpointRounding.AwayFromZero)

            Dim oldW1 = w1
            Dim oldW2 = w2
            If MyBase.Width < width Then
                w1 = Math.Max(w1 - dif, 75)
                w2 = Math.Max(w2 - dif, 75)
            Else
                w1 = Math.Min(w1 - dif, 150)
                w2 += MyBase.Width - width + oldW1 - w1
            End If
        End Sub

#End Region


#Region "paint events"

        ''' <summary>
        ''' On Paint event
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            'check if translations are changed and update components
            If Not (_translationsRefreshComponentTranslationsCheck Is Translations OrElse (_translationsRefreshComponentTranslationsCheck.Count = Translations.Count AndAlso Not _translationsRefreshComponentTranslationsCheck.Except(Translations).Any())) Then
                _translationsRefreshComponentTranslationsCheck = Translations
                RefreshComponentTranslations()
            End If

            MyBase.OnPaint(e)
        End Sub

#End Region

    End Class


End Namespace
