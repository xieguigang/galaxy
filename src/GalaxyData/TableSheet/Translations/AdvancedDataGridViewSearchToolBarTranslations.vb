Imports TranslationKey = Galaxy.Data.TableSheet.AdvancedDataGridViewSearchToolBarTranslationKey

Namespace TableSheet

    ''' <summary>
    ''' Available translation keys
    ''' </summary>
    Public Enum AdvancedDataGridViewSearchToolBarTranslationKey
        ADGVSTBLabelSearch
        ADGVSTBButtonFromBegin
        ADGVSTBButtonCaseSensitiveToolTip
        ADGVSTBButtonSearchToolTip
        ADGVSTBButtonCloseToolTip
        ADGVSTBButtonWholeWordToolTip
        ADGVSTBComboBoxColumnsAll
        ADGVSTBTextBoxSearchToolTip
    End Enum

    Module AdvancedDataGridViewSearchToolBarTranslations

        ''' <summary>
        ''' Internationalization strings
        ''' </summary>
        Public ReadOnly Translations As Dictionary(Of String, String) = New Dictionary(Of String, String)() From {
            {TranslationKey.ADGVSTBLabelSearch.ToString(), "Search:"},
            {TranslationKey.ADGVSTBButtonFromBegin.ToString(), "From Begin"},
            {TranslationKey.ADGVSTBButtonCaseSensitiveToolTip.ToString(), "Case Sensitivity"},
            {TranslationKey.ADGVSTBButtonSearchToolTip.ToString(), "Find Next"},
            {TranslationKey.ADGVSTBButtonCloseToolTip.ToString(), "Hide"},
            {TranslationKey.ADGVSTBButtonWholeWordToolTip.ToString(), "Search only Whole Word"},
            {TranslationKey.ADGVSTBComboBoxColumnsAll.ToString(), "(All Columns)"},
            {TranslationKey.ADGVSTBTextBoxSearchToolTip.ToString(), "Value for Search"}
        }

    End Module
End Namespace