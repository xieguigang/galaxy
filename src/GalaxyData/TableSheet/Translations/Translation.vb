Imports TranslationKey = Galaxy.Data.TableSheet.AdvancedDataGridViewTranslationKey

Namespace TableSheet

    ''' <summary>
    ''' Available translation keys
    ''' </summary>
    Public Enum AdvancedDataGridViewTranslationKey
        ADGVSortDateTimeASC
        ADGVSortDateTimeDESC
        ADGVSortBoolASC
        ADGVSortBoolDESC
        ADGVSortNumASC
        ADGVSortNumDESC
        ADGVSortTextASC
        ADGVSortTextDESC
        ADGVAddCustomFilter
        ADGVCustomFilter
        ADGVClearFilter
        ADGVClearSort
        ADGVButtonFilter
        ADGVButtonUndofilter
        ADGVNodeSelectAll
        ADGVNodeSelectEmpty
        ADGVNodeSelectTrue
        ADGVNodeSelectFalse
        ADGVFilterChecklistDisable
        ADGVEquals
        ADGVDoesNotEqual
        ADGVEarlierThan
        ADGVEarlierThanOrEqualTo
        ADGVLaterThan
        ADGVLaterThanOrEqualTo
        ADGVBetween
        ADGVGreaterThan
        ADGVGreaterThanOrEqualTo
        ADGVLessThan
        ADGVLessThanOrEqualTo
        ADGVBeginsWith
        ADGVDoesNotBeginWith
        ADGVEndsWith
        ADGVDoesNotEndWith
        ADGVContains
        ADGVDoesNotContain
        ADGVIncludeNullValues
        ADGVInvalidValue
        ADGVFilterStringDescription
        ADGVFormTitle
        ADGVLabelColumnNameText
        ADGVLabelAnd
        ADGVButtonOk
        ADGVButtonCancel
    End Enum

    Module AdvancedDataGridViewTranslations

        ''' <summary>
        ''' Internationalization strings
        ''' </summary>
        Public ReadOnly Translations As New Dictionary(Of String, String)() From {
            {TranslationKey.ADGVSortDateTimeASC.ToString(), "Sort Oldest to Newest"},
            {TranslationKey.ADGVSortDateTimeDESC.ToString(), "Sort Newest to Oldest"},
            {TranslationKey.ADGVSortBoolASC.ToString(), "Sort by False/True"},
            {TranslationKey.ADGVSortBoolDESC.ToString(), "Sort by True/False"},
            {TranslationKey.ADGVSortNumASC.ToString(), "Sort Smallest to Largest"},
            {TranslationKey.ADGVSortNumDESC.ToString(), "Sort Largest to Smallest"},
            {TranslationKey.ADGVSortTextASC.ToString(), "Sort А to Z"},
            {TranslationKey.ADGVSortTextDESC.ToString(), "Sort Z to A"},
            {TranslationKey.ADGVAddCustomFilter.ToString(), "Add a Custom Filter"},
            {TranslationKey.ADGVCustomFilter.ToString(), "Custom Filter"},
            {TranslationKey.ADGVClearFilter.ToString(), "Clear Filter"},
            {TranslationKey.ADGVClearSort.ToString(), "Clear Sort"},
            {TranslationKey.ADGVButtonFilter.ToString(), "Filter"},
            {TranslationKey.ADGVButtonUndofilter.ToString(), "Cancel"},
            {TranslationKey.ADGVNodeSelectAll.ToString(), "(Select All)"},
            {TranslationKey.ADGVNodeSelectEmpty.ToString(), "(Blanks)"},
            {TranslationKey.ADGVNodeSelectTrue.ToString(), "True"},
            {TranslationKey.ADGVNodeSelectFalse.ToString(), "False"},
            {TranslationKey.ADGVFilterChecklistDisable.ToString(), "Filter list is disabled"},
            {TranslationKey.ADGVEquals.ToString(), "equals"},
            {TranslationKey.ADGVDoesNotEqual.ToString(), "does not equal"},
            {TranslationKey.ADGVEarlierThan.ToString(), "earlier than"},
            {TranslationKey.ADGVEarlierThanOrEqualTo.ToString(), "earlier than or equal to"},
            {TranslationKey.ADGVLaterThan.ToString(), "later than"},
            {TranslationKey.ADGVLaterThanOrEqualTo.ToString(), "later than or equal to"},
            {TranslationKey.ADGVBetween.ToString(), "between"},
            {TranslationKey.ADGVGreaterThan.ToString(), "greater than"},
            {TranslationKey.ADGVGreaterThanOrEqualTo.ToString(), "greater than or equal to"},
            {TranslationKey.ADGVLessThan.ToString(), "less than"},
            {TranslationKey.ADGVLessThanOrEqualTo.ToString(), "less than or equal to"},
            {TranslationKey.ADGVBeginsWith.ToString(), "begins with"},
            {TranslationKey.ADGVDoesNotBeginWith.ToString(), "does not begin with"},
            {TranslationKey.ADGVEndsWith.ToString(), "ends with"},
            {TranslationKey.ADGVDoesNotEndWith.ToString(), "does not end with"},
            {TranslationKey.ADGVContains.ToString(), "contains"},
            {TranslationKey.ADGVDoesNotContain.ToString(), "does not contain"},
            {TranslationKey.ADGVIncludeNullValues.ToString(), "include empty strings"},
            {TranslationKey.ADGVInvalidValue.ToString(), "Invalid Value"},
            {TranslationKey.ADGVFilterStringDescription.ToString(), "Show rows where value {0} ""{1}"""},
            {TranslationKey.ADGVFormTitle.ToString(), "Custom Filter"},
            {TranslationKey.ADGVLabelColumnNameText.ToString(), "Show rows where value"},
            {TranslationKey.ADGVLabelAnd.ToString(), "And"},
            {TranslationKey.ADGVButtonOk.ToString(), "OK"},
            {TranslationKey.ADGVButtonCancel.ToString(), "Cancel"}
        }

    End Module
End Namespace