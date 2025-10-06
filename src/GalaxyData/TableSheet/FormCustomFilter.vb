#Region "License"
' Advanced DataGridView
'
' Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
' Original work Copyright (c), 2013 Zuby <zuby@me.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System.Globalization
Imports System.Threading


Namespace TableSheet
    Partial Friend Class FormCustomFilter : Inherits Form

#Region "class properties"

        Private Enum FilterType
            Unknown
            DateTime
            TimeSpan
            [String]
            Float
            [Integer]
        End Enum

        Private _filterType As FilterType = FilterType.Unknown
        Private _valControl1 As Control = Nothing
        Private _valControl2 As Control = Nothing

        Private _filterDateAndTimeEnabled As Boolean = True

        Private _filterString As String = Nothing
        Private _filterStringDescription As String = Nothing

#End Region


#Region "constructors"

        ''' <summary>
        ''' Main constructor
        ''' </summary>
        ''' <param name="dataType"></param>
        ''' <param name="filterDateAndTimeEnabled"></param>
        Public Sub New(dataType As Type, filterDateAndTimeEnabled As Boolean)
            MyBase.New()
            'initialize components
            InitializeComponent()

            'set component translations
            Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVFormTitle.ToString())
            label_columnName.Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLabelColumnNameText.ToString())
            label_and.Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLabelAnd.ToString())
            button_ok.Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVButtonOk.ToString())
            button_cancel.Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVButtonCancel.ToString())

            If dataType Is GetType(Date) Then
                _filterType = FilterType.DateTime
            ElseIf dataType Is GetType(TimeSpan) Then
                _filterType = FilterType.TimeSpan
            ElseIf dataType Is GetType(Integer) OrElse
                dataType Is GetType(Long) OrElse
                dataType Is GetType(Short) OrElse
                dataType Is GetType(UInteger) OrElse
                dataType Is GetType(ULong) OrElse
                dataType Is GetType(UShort) OrElse
                dataType Is GetType(Byte) OrElse
                dataType Is GetType(SByte) Then
                _filterType = FilterType.Integer
            ElseIf dataType Is GetType(Single) OrElse dataType Is GetType(Double) OrElse dataType Is GetType(Decimal) Then
                _filterType = FilterType.Float
            ElseIf dataType Is GetType(String) Then
                _filterType = FilterType.String
            Else
                _filterType = FilterType.Unknown
            End If

            _filterDateAndTimeEnabled = filterDateAndTimeEnabled

            Select Case _filterType
                Case FilterType.DateTime
                    _valControl1 = New DateTimePicker()
                    _valControl2 = New DateTimePicker()
                    If _filterDateAndTimeEnabled Then
                        Dim dt = Thread.CurrentThread.CurrentCulture.DateTimeFormat

                        TryCast(_valControl1, DateTimePicker).CustomFormat = dt.ShortDatePattern & " " & "HH:mm"
                        TryCast(_valControl2, DateTimePicker).CustomFormat = dt.ShortDatePattern & " " & "HH:mm"
                        TryCast(_valControl1, DateTimePicker).Format = DateTimePickerFormat.Custom
                        TryCast(_valControl2, DateTimePicker).Format = DateTimePickerFormat.Custom
                    Else
                        TryCast(_valControl1, DateTimePicker).Format = DateTimePickerFormat.Short
                        TryCast(_valControl2, DateTimePicker).Format = DateTimePickerFormat.Short
                    End If

                    comboBox_filterType.Items.AddRange(New String() {
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEarlierThan.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEarlierThanOrEqualTo.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLaterThan.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLaterThanOrEqualTo.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBetween.ToString())
                    })

                Case FilterType.TimeSpan
                    _valControl1 = New TextBox()
                    _valControl2 = New TextBox()
                    comboBox_filterType.Items.AddRange(New String() {
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVContains.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotContain.ToString())
                    })

                Case FilterType.Integer, FilterType.Float
                    _valControl1 = New TextBox()
                    _valControl2 = New TextBox()
                    AddHandler _valControl1.TextChanged, AddressOf valControl_TextChanged
                    AddHandler _valControl2.TextChanged, AddressOf valControl_TextChanged
                    comboBox_filterType.Items.AddRange(New String() {
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVGreaterThan.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVGreaterThanOrEqualTo.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLessThan.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLessThanOrEqualTo.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBetween.ToString())
                    })
                    _valControl1.Tag = True
                    _valControl2.Tag = True
                    button_ok.Enabled = False
                Case Else
                    _valControl1 = New TextBox()
                    Dim includeNullCheckBox As CheckBox = New CheckBox()
                    includeNullCheckBox.Text = AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVIncludeNullValues.ToString())
                    _valControl2 = includeNullCheckBox
                    comboBox_filterType.Items.AddRange(New String() {
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBeginsWith.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotBeginWith.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEndsWith.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEndWith.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVContains.ToString()),
                        AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotContain.ToString())
                    })
            End Select
            comboBox_filterType.SelectedIndex = 0

            _valControl1.Name = "valControl1"
            _valControl1.Location = New Point(20, comboBox_filterType.Location.Y + comboBox_filterType.Size.Height + 10)
            _valControl1.Size = New Size(166, 20)
            _valControl1.Width = comboBox_filterType.Width - 20
            _valControl1.TabIndex = 4
            _valControl1.Visible = True
            AddHandler _valControl1.KeyDown, AddressOf valControl_KeyDown

            _valControl2.Name = "valControl2"
            _valControl2.Location = New Point(20, label_and.Location.Y + label_and.Size.Height + 10)
            _valControl2.Size = New Size(166, 20)
            _valControl2.Width = comboBox_filterType.Width - 20
            _valControl2.TabIndex = 5
            _valControl2.Visible = False
            AddHandler _valControl2.KeyDown, AddressOf valControl_KeyDown

            label_and.Location = New Point(12, (_valControl1.Location.Y + _valControl1.Size.Height + _valControl2.Location.Y - label_and.Size.Height) / 2)

            Controls.Add(_valControl1)
            Controls.Add(_valControl2)

            comboBox_filterType_SelectedIndexChanged(Nothing, Nothing)

            errorProvider.SetIconAlignment(_valControl1, ErrorIconAlignment.MiddleRight)
            errorProvider.SetIconPadding(_valControl1, -18)
            errorProvider.SetIconAlignment(_valControl2, ErrorIconAlignment.MiddleRight)
            errorProvider.SetIconPadding(_valControl2, -18)
        End Sub

        ''' <summary>
        ''' Form loaders
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub FormCustomFilter_Load(sender As Object, e As EventArgs)
        End Sub

#End Region


#Region "public filter methods"

        ''' <summary>
        ''' Get the Filter string
        ''' </summary>
        Public ReadOnly Property FilterString As String
            Get
                Return _filterString
            End Get
        End Property

        ''' <summary>
        ''' Get the Filter string description
        ''' </summary>
        Public ReadOnly Property FilterStringDescription As String
            Get
                Return _filterStringDescription
            End Get
        End Property

#End Region


#Region "filter builder"

        ''' <summary>
        ''' Build a Filter string
        ''' </summary>
        ''' <param name="filterType"></param>
        ''' <param name="filterDateAndTimeEnabled"></param>
        ''' <param name="filterTypeConditionText"></param>
        ''' <param name="control1"></param>
        ''' <param name="control2"></param>
        ''' <returns></returns>
        Private Function BuildCustomFilter(filterType As FilterType, filterDateAndTimeEnabled As Boolean, filterTypeConditionText As String, control1 As Control, control2 As Control) As String
            Dim filterString = ""

            Dim column = "[{0}] "

            If filterType = FilterType.Unknown Then column = "Convert([{0}], 'System.String') "

            filterString = column

            Select Case filterType
                Case FilterType.DateTime
                    Dim dt = CType(control1, DateTimePicker).Value
                    dt = New DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0)

                    If Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString())) Then
                        filterString = "Convert([{0}], 'System.String') LIKE '%" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "%'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEarlierThan.ToString())) Then
                        filterString += "< '" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEarlierThanOrEqualTo.ToString())) Then
                        filterString += "<= '" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLaterThan.ToString())) Then
                        filterString += "> '" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLaterThanOrEqualTo.ToString())) Then
                        filterString += ">= '" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBetween.ToString())) Then
                        Dim dt1 = CType(control2, DateTimePicker).Value
                        dt1 = New DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0)
                        filterString += ">= '" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "'"
                        filterString += " AND " & column & "<= '" & Convert.ToString(If(filterDateAndTimeEnabled, dt1, dt1.Date), CultureInfo.CurrentCulture) & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString())) Then
                        filterString = "Convert([{0}], 'System.String') NOT LIKE '%" & Convert.ToString(If(filterDateAndTimeEnabled, dt, dt.Date), CultureInfo.CurrentCulture) & "%'"
                    End If

                Case FilterType.TimeSpan
                    Try
                        Dim ts = TimeSpan.Parse(control1.Text)

                        If Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVContains.ToString())) Then
                            filterString = "(Convert([{0}], 'System.String') LIKE '%P" & (If(ts.Days > 0, ts.Days.ToString() & "D", "")) & If(ts.TotalHours > 0, "T", "") & (If(ts.Hours > 0, ts.Hours.ToString() & "H", "")) & (If(ts.Minutes > 0, ts.Minutes.ToString() & "M", "")) & (If(ts.Seconds > 0, ts.Seconds.ToString() & "S", "")) & "%')"
                        ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotContain.ToString())) Then
                            filterString = "(Convert([{0}], 'System.String') NOT LIKE '%P" & (If(ts.Days > 0, ts.Days.ToString() & "D", "")) & If(ts.TotalHours > 0, "T", "") & (If(ts.Hours > 0, ts.Hours.ToString() & "H", "")) & (If(ts.Minutes > 0, ts.Minutes.ToString() & "M", "")) & (If(ts.Seconds > 0, ts.Seconds.ToString() & "S", "")) & "%')"
                        End If
                    Catch __unusedFormatException1__ As FormatException
                        filterString = Nothing
                    End Try

                Case FilterType.Integer, FilterType.Float

                    Dim num = control1.Text

                    If filterType = FilterType.Float Then num = num.Replace(",", ".")

                    If Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString())) Then
                        filterString += "= " & num
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBetween.ToString())) Then
                        filterString += ">= " & num & " AND " & column & "<= " & If(filterType = FilterType.Float, control2.Text.Replace(",", "."), control2.Text)
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString())) Then
                        filterString += "<> " & num
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVGreaterThan.ToString())) Then
                        filterString += "> " & num
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVGreaterThanOrEqualTo.ToString())) Then
                        filterString += ">= " & num
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLessThan.ToString())) Then
                        filterString += "< " & num
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVLessThanOrEqualTo.ToString())) Then
                        filterString += "<= " & num
                    End If

                Case Else
                    Dim txt = FormatFilterString(control1.Text)
                    If Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEquals.ToString())) Then
                        filterString += "LIKE '" & txt & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString())) Then
                        filterString += "NOT LIKE '" & txt & "'" & (If(TryCast(_valControl2, CheckBox).Checked, " OR " & column & "IS NULL", ""))
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBeginsWith.ToString())) Then
                        filterString += "LIKE '" & txt & "%'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVEndsWith.ToString())) Then
                        filterString += "LIKE '%" & txt & "'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotBeginWith.ToString())) Then
                        filterString += "NOT LIKE '" & txt & "%'" & (If(TryCast(_valControl2, CheckBox).Checked, " OR " & column & "IS NULL", ""))
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEndWith.ToString())) Then
                        filterString += "NOT LIKE '%" & txt & "'" & (If(TryCast(_valControl2, CheckBox).Checked, " OR " & column & "IS NULL", ""))
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVContains.ToString())) Then
                        filterString += "LIKE '%" & txt & "%'"
                    ElseIf Equals(filterTypeConditionText, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotContain.ToString())) Then
                        filterString += "NOT LIKE '%" & txt & "%'" & (If(TryCast(_valControl2, CheckBox).Checked, " OR " & column & "IS NULL", ""))
                    End If
            End Select

            Return filterString
        End Function

        ''' <summary>
        ''' Format a text Filter string
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        Private Function FormatFilterString(text As String) As String
            Dim result = ""
            Dim s As String
            Dim replace = {"%", "[", "]", "*", """", "\"}

            For i = 0 To text.Length - 1
                s = text(i).ToString()
                If replace.Contains(s) Then
                    result += "[" & s & "]"
                Else
                    result += s
                End If
            Next

            Return result.Replace("'", "''").Replace("{", "{{").Replace("}", "}}")
        End Function


#End Region


#Region "buttons events"

        ''' <summary>
        ''' Button Cancel Clieck
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub button_cancel_Click(sender As Object, e As EventArgs)
            _filterStringDescription = Nothing
            _filterString = Nothing
            Close()
        End Sub

        ''' <summary>
        ''' Button OK Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub button_ok_Click(sender As Object, e As EventArgs)
            If _valControl1.Visible AndAlso _valControl1.Tag IsNot Nothing AndAlso CBool(_valControl1.Tag) OrElse _valControl2.Visible AndAlso _valControl2.Tag IsNot Nothing AndAlso CBool(_valControl2.Tag) Then
                button_ok.Enabled = False
                Return
            End If

            Dim filterString = BuildCustomFilter(_filterType, _filterDateAndTimeEnabled, comboBox_filterType.Text, _valControl1, _valControl2)

            If Not String.IsNullOrEmpty(filterString) Then
                _filterString = filterString
                _filterStringDescription = String.Format(AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVFilterStringDescription.ToString()), comboBox_filterType.Text, _valControl1.Text)
                If _valControl2.Visible Then _filterStringDescription += " " & label_and.Text & " """ & _valControl2.Text & """"
                DialogResult = DialogResult.OK
            Else
                _filterString = Nothing
                _filterStringDescription = Nothing
                DialogResult = DialogResult.Cancel
            End If

            Close()
        End Sub

#End Region


#Region "changed status events"

        ''' <summary>
        ''' Changed condition type
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub comboBox_filterType_SelectedIndexChanged(sender As Object, e As EventArgs)
            If _filterType = FilterType.String AndAlso (Equals(comboBox_filterType.Text, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEqual.ToString())) OrElse
                Equals(comboBox_filterType.Text, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotBeginWith.ToString())) OrElse
                Equals(comboBox_filterType.Text, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotEndWith.ToString())) OrElse
                Equals(comboBox_filterType.Text, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVDoesNotContain.ToString()))) Then
                _valControl2.Visible = True
                TryCast(_valControl2, CheckBox).Checked = False
                label_and.Visible = False
            ElseIf Equals(comboBox_filterType.Text, AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVBetween.ToString())) Then
                _valControl2.Visible = True
                label_and.Visible = True
            Else
                _valControl2.Visible = False
                label_and.Visible = False
            End If
            button_ok.Enabled = Not (_valControl1.Visible AndAlso _valControl1.Tag IsNot Nothing AndAlso CBool(_valControl1.Tag)) OrElse _valControl2.Visible AndAlso _valControl2.Tag IsNot Nothing AndAlso CBool(_valControl2.Tag)
        End Sub

        ''' <summary>
        ''' Changed a control Text
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub valControl_TextChanged(sender As Object, e As EventArgs)
            Dim hasErrors = False
            Select Case _filterType
                Case FilterType.Integer
                    Dim val As Long
                    hasErrors = Not (Long.TryParse(TryCast(sender, TextBox).Text, val))

                Case FilterType.Float
                    Dim val1 As Double
                    hasErrors = Not (Double.TryParse(TryCast(sender, TextBox).Text, val1))
            End Select

            TryCast(sender, Control).Tag = hasErrors OrElse TryCast(sender, TextBox).Text.Length = 0

            If hasErrors AndAlso TryCast(sender, TextBox).Text.Length > 0 Then
                errorProvider.SetError(TryCast(sender, Control), AdvancedDataGridViewTranslations.Translations(AdvancedDataGridViewTranslationKey.ADGVInvalidValue.ToString()))
            Else
                errorProvider.SetError(TryCast(sender, Control), "")
            End If

            button_ok.Enabled = Not (_valControl1.Visible AndAlso _valControl1.Tag IsNot Nothing AndAlso CBool(_valControl1.Tag)) OrElse _valControl2.Visible AndAlso _valControl2.Tag IsNot Nothing AndAlso CBool(_valControl2.Tag)
        End Sub

        ''' <summary>
        ''' KeyDown on a control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub valControl_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyData = Keys.Enter Then
                If sender Is _valControl1 Then
                    If _valControl2.Visible Then
                        _valControl2.Focus()
                    Else
                        button_ok_Click(button_ok, New EventArgs())
                    End If
                Else
                    button_ok_Click(button_ok, New EventArgs())
                End If

                e.SuppressKeyPress = False
                e.Handled = True
            End If
        End Sub

#End Region

    End Class
End Namespace
