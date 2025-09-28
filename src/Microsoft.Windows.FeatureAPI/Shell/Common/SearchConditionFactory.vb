'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal

Namespace Shell
    ''' <summary>
    ''' Provides methods for creating or resolving a condition tree 
    ''' that was obtained by parsing a query string.
    ''' </summary>
    Public NotInheritable Class SearchConditionFactory
        Private Sub New()
        End Sub
        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and constant value. 
        ''' </summary>
        ''' <param name="propertyName">The name of a property to be compared, or null for an unspecified property. 
        ''' The locale name of the leaf node is LOCALE_NAME_USER_DEFAULT.</param>
        ''' <param name="value">The constant value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyName As String, value As String, operation As SearchConditionOperation) As SearchCondition
            Using propVar As New PropVariant(value)
                Return CreateLeafCondition(propertyName, propVar, Nothing, operation)
            End Using
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and constant value. 
        ''' Overload method takes a DateTime parameter for the comparison value.
        ''' </summary>
        ''' <param name="propertyName">The name of a property to be compared, or null for an unspecified property. 
        ''' The locale name of the leaf node is LOCALE_NAME_USER_DEFAULT.</param>
        ''' <param name="value">The DateTime value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyName As String, value As DateTime, operation As SearchConditionOperation) As SearchCondition
            Using propVar As New PropVariant(value)
                Return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.DateTime", operation)
            End Using
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Integer value. 
        ''' </summary>
        ''' <param name="propertyName">The name of a property to be compared, or null for an unspecified property. 
        ''' The locale name of the leaf node is LOCALE_NAME_USER_DEFAULT.</param>
        ''' <param name="value">The Integer value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyName As String, value As Integer, operation As SearchConditionOperation) As SearchCondition
            Using propVar As New PropVariant(value)
                Return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.Integer", operation)
            End Using
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Boolean value. 
        ''' </summary>
        ''' <param name="propertyName">The name of a property to be compared, or null for an unspecified property. 
        ''' The locale name of the leaf node is LOCALE_NAME_USER_DEFAULT.</param>
        ''' <param name="value">The Boolean value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyName As String, value As Boolean, operation As SearchConditionOperation) As SearchCondition
            Using propVar As New PropVariant(value)
                Return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.Boolean", operation)
            End Using
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Floating Point value. 
        ''' </summary>
        ''' <param name="propertyName">The name of a property to be compared, or null for an unspecified property. 
        ''' The locale name of the leaf node is LOCALE_NAME_USER_DEFAULT.</param>
        ''' <param name="value">The Floating Point value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyName As String, value As Double, operation As SearchConditionOperation) As SearchCondition
            Using propVar As New PropVariant(value)
                Return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.FloatingPoint", operation)
            End Using
        End Function

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
        Private Shared Function CreateLeafCondition(propertyName As String, propVar As PropVariant, valueType As String, operation As SearchConditionOperation) As SearchCondition
            Dim nativeConditionFactory As IConditionFactory = Nothing
            Dim condition As SearchCondition = Nothing

            Try
                ' Same as the native "IConditionFactory:MakeLeaf" method
                nativeConditionFactory = DirectCast(New ConditionFactoryCoClass(), IConditionFactory)

                Dim nativeCondition As ICondition = Nothing

                If String.IsNullOrEmpty(propertyName) OrElse propertyName.ToUpperInvariant() = "SYSTEM.NULL" Then
                    propertyName = Nothing
                End If

                Dim hr As HResult = HResult.Fail

                hr = nativeConditionFactory.MakeLeaf(propertyName, operation, valueType, propVar, Nothing, Nothing,
                    Nothing, False, nativeCondition)

                If Not CoreErrorHelper.Succeeded(hr) Then
                    Throw New ShellException(hr)
                End If

                ' Create our search condition and set the various properties.
                condition = New SearchCondition(nativeCondition)
            Finally
                If nativeConditionFactory IsNot Nothing Then
                    Marshal.ReleaseComObject(nativeConditionFactory)
                End If
            End Try

            Return condition
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and constant value. 
        ''' </summary>
        ''' <param name="propertyKey">The property to be compared.</param>
        ''' <param name="value">The constant value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyKey As PropertyKey, value As String, operation As SearchConditionOperation) As SearchCondition
            Dim canonicalName As String
            PropertySystemNativeMethods.PSGetNameFromPropertyKey(propertyKey, canonicalName)

            If String.IsNullOrEmpty(canonicalName) Then
                Throw New ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey")
            End If

            Return CreateLeafCondition(canonicalName, value, operation)
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and constant value. 
        ''' Overload method takes a DateTime parameter for the comparison value.
        ''' </summary>
        ''' <param name="propertyKey">The property to be compared.</param>
        ''' <param name="value">The DateTime value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyKey As PropertyKey, value As DateTime, operation As SearchConditionOperation) As SearchCondition
            Dim canonicalName As String
            PropertySystemNativeMethods.PSGetNameFromPropertyKey(propertyKey, canonicalName)

            If String.IsNullOrEmpty(canonicalName) Then
                Throw New ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey")
            End If
            Return CreateLeafCondition(canonicalName, value, operation)
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Boolean value. 
        ''' Overload method takes a DateTime parameter for the comparison value.
        ''' </summary>
        ''' <param name="propertyKey">The property to be compared.</param>
        ''' <param name="value">The boolean value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyKey As PropertyKey, value As Boolean, operation As SearchConditionOperation) As SearchCondition
            Dim canonicalName As String
            PropertySystemNativeMethods.PSGetNameFromPropertyKey(propertyKey, canonicalName)

            If String.IsNullOrEmpty(canonicalName) Then
                Throw New ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey")
            End If
            Return CreateLeafCondition(canonicalName, value, operation)
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Floating Point value. 
        ''' Overload method takes a DateTime parameter for the comparison value.
        ''' </summary>
        ''' <param name="propertyKey">The property to be compared.</param>
        ''' <param name="value">The Floating Point value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyKey As PropertyKey, value As Double, operation As SearchConditionOperation) As SearchCondition
            Dim canonicalName As String
            PropertySystemNativeMethods.PSGetNameFromPropertyKey(propertyKey, canonicalName)

            If String.IsNullOrEmpty(canonicalName) Then
                Throw New ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey")
            End If
            Return CreateLeafCondition(canonicalName, value, operation)
        End Function

        ''' <summary>
        ''' Creates a leaf condition node that represents a comparison of property value and Integer value. 
        ''' Overload method takes a DateTime parameter for the comparison value.
        ''' </summary>
        ''' <param name="propertyKey">The property to be compared.</param>
        ''' <param name="value">The Integer value against which the property value should be compared.</param>
        ''' <param name="operation">Specific condition to be used when comparing the actual value and the expected value of the given property</param>
        ''' <returns>SearchCondition based on the given parameters</returns>
        ''' <remarks>
        ''' The search will only work for files that are indexed, as well as the specific properties are indexed. To find 
        ''' the properties that are indexed, look for the specific property's property description and 
        ''' <see cref="P:Shell.PropertySystem.ShellPropertyDescription.TypeFlags"/> property for IsQueryable flag.
        ''' </remarks>
        Public Shared Function CreateLeafCondition(propertyKey As PropertyKey, value As Integer, operation As SearchConditionOperation) As SearchCondition
            Dim canonicalName As String
            PropertySystemNativeMethods.PSGetNameFromPropertyKey(propertyKey, canonicalName)

            If String.IsNullOrEmpty(canonicalName) Then
                Throw New ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey")
            End If
            Return CreateLeafCondition(canonicalName, value, operation)
        End Function

        ''' <summary>
        ''' Creates a condition node that is a logical conjunction ("AND") or disjunction ("OR") 
        ''' of a collection of subconditions.
        ''' </summary>
        ''' <param name="conditionType">The SearchConditionType of the condition node. 
        ''' Must be either AndCondition or OrCondition.</param>
        ''' <param name="simplify">TRUE to logically simplify the result, if possible; 
        ''' then the result will not necessarily to be of the specified kind. FALSE if the result should 
        ''' have exactly the prescribed structure. An application that plans to execute a query based on the 
        ''' condition tree would typically benefit from setting this parameter to TRUE. </param>
        ''' <param name="conditionNodes">Array of subconditions</param>
        ''' <returns>New SearchCondition based on the operation</returns>
        Public Shared Function CreateAndOrCondition(conditionType As SearchConditionType, simplify As Boolean, ParamArray conditionNodes As SearchCondition()) As SearchCondition
            ' Same as the native "IConditionFactory:MakeAndOr" method
            Dim nativeConditionFactory As IConditionFactory = DirectCast(New ConditionFactoryCoClass(), IConditionFactory)
            Dim result As ICondition = Nothing

            Try
                ' 
                Dim conditionList As New List(Of ICondition)()
                If conditionNodes IsNot Nothing Then
                    For Each c As SearchCondition In conditionNodes
                        conditionList.Add(c.NativeSearchCondition)
                    Next
                End If

                Dim subConditions As IEnumUnknown = New EnumUnknownClass(conditionList.ToArray())

                Dim hr As HResult = nativeConditionFactory.MakeAndOr(conditionType, subConditions, simplify, result)

                If Not CoreErrorHelper.Succeeded(hr) Then
                    Throw New ShellException(hr)
                End If
            Finally
                If nativeConditionFactory IsNot Nothing Then
                    Marshal.ReleaseComObject(nativeConditionFactory)
                End If
            End Try

            Return New SearchCondition(result)
        End Function

        ''' <summary>
        ''' Creates a condition node that is a logical negation (NOT) of another condition 
        ''' (a subnode of this node). 
        ''' </summary>
        ''' <param name="conditionToBeNegated">SearchCondition node to be negated.</param>
        ''' <param name="simplify">True to logically simplify the result if possible; False otherwise. 
        ''' In a query builder scenario, simplyfy should typically be set to false.</param>
        ''' <returns>New SearchCondition</returns>
        Public Shared Function CreateNotCondition(conditionToBeNegated As SearchCondition, simplify As Boolean) As SearchCondition
            If conditionToBeNegated Is Nothing Then
                Throw New ArgumentNullException("conditionToBeNegated")
            End If

            ' Same as the native "IConditionFactory:MakeNot" method
            Dim nativeConditionFactory As IConditionFactory = DirectCast(New ConditionFactoryCoClass(), IConditionFactory)
            Dim result As ICondition

            Try
                Dim hr As HResult = nativeConditionFactory.MakeNot(conditionToBeNegated.NativeSearchCondition, simplify, result)

                If Not CoreErrorHelper.Succeeded(hr) Then
                    Throw New ShellException(hr)
                End If
            Finally
                If nativeConditionFactory IsNot Nothing Then
                    Marshal.ReleaseComObject(nativeConditionFactory)
                End If
            End Try

            Return New SearchCondition(result)
        End Function

        ''' <summary>
        ''' Parses an input string that contains Structured Query keywords (using Advanced Query Syntax 
        ''' or Natural Query Syntax) and produces a SearchCondition object.
        ''' </summary>
        ''' <param name="query">The query to be parsed</param>
        ''' <returns>Search condition resulting from the query</returns>
        ''' <remarks>For more information on structured query syntax, visit http://msdn.microsoft.com/en-us/library/bb233500.aspx and
        ''' http://www.microsoft.com/windows/products/winfamily/desktopsearch/technicalresources/advquery.mspx</remarks>
        Public Shared Function ParseStructuredQuery(query As String) As SearchCondition
            Return ParseStructuredQuery(query, Nothing)
        End Function

        ''' <summary>
        ''' Parses an input string that contains Structured Query keywords (using Advanced Query Syntax 
        ''' or Natural Query Syntax) and produces a SearchCondition object.
        ''' </summary>
        ''' <param name="query">The query to be parsed</param>
        ''' <param name="cultureInfo">The culture used to select the localized language for keywords.</param>
        ''' <returns>Search condition resulting from the query</returns>
        ''' <remarks>For more information on structured query syntax, visit http://msdn.microsoft.com/en-us/library/bb233500.aspx and
        ''' http://www.microsoft.com/windows/products/winfamily/desktopsearch/technicalresources/advquery.mspx</remarks>
        Public Shared Function ParseStructuredQuery(query As String, cultureInfo As CultureInfo) As SearchCondition
            If String.IsNullOrEmpty(query) Then
                Throw New ArgumentNullException("query")
            End If

            Dim nativeQueryParserManager As IQueryParserManager = DirectCast(New QueryParserManagerCoClass(), IQueryParserManager)
            Dim queryParser As IQueryParser = Nothing
            Dim querySolution As IQuerySolution = Nothing
            Dim result As ICondition = Nothing

            Dim mainType As IEntity = Nothing
            Dim searchCondition As SearchCondition = Nothing
            Try
                ' First, try to create a new IQueryParser using IQueryParserManager
                Dim guid As New Guid(ShellIIDGuid.IQueryParser)
                Dim hr As HResult = nativeQueryParserManager.CreateLoadedParser("SystemIndex", If(cultureInfo Is Nothing, CUShort(0), CUShort(cultureInfo.LCID)), guid, queryParser)

                If Not CoreErrorHelper.Succeeded(hr) Then
                    Throw New ShellException(hr)
                End If

                If queryParser IsNot Nothing Then
                    ' If user specified natural query, set the option on the query parser
                    Using optionValue As New PropVariant(True)
                        hr = queryParser.SetOption(StructuredQuerySingleOption.NaturalSyntax, optionValue)
                    End Using

                    If Not CoreErrorHelper.Succeeded(hr) Then
                        Throw New ShellException(hr)
                    End If

                    ' Next, try to parse the query.
                    ' Result would be IQuerySolution that we can use for getting the ICondition and other
                    ' details about the parsed query.
                    hr = queryParser.Parse(query, Nothing, querySolution)

                    If Not CoreErrorHelper.Succeeded(hr) Then
                        Throw New ShellException(hr)
                    End If

                    If querySolution IsNot Nothing Then
                        ' Lastly, try to get the ICondition from this parsed query
                        hr = querySolution.GetQuery(result, mainType)

                        If Not CoreErrorHelper.Succeeded(hr) Then
                            Throw New ShellException(hr)
                        End If
                    End If
                End If

                searchCondition = New SearchCondition(result)
                Return searchCondition
            Catch
                If searchCondition IsNot Nothing Then
                    searchCondition.Dispose()
                End If
                Throw
            Finally
                If nativeQueryParserManager IsNot Nothing Then
                    Marshal.ReleaseComObject(nativeQueryParserManager)
                End If

                If queryParser IsNot Nothing Then
                    Marshal.ReleaseComObject(queryParser)
                End If

                If querySolution IsNot Nothing Then
                    Marshal.ReleaseComObject(querySolution)
                End If

                If mainType IsNot Nothing Then
                    Marshal.ReleaseComObject(mainType)
                End If
            End Try
        End Function
    End Class
End Namespace
