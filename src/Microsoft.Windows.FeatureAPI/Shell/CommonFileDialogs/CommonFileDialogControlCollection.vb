'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq

Namespace Dialogs.Controls
	''' <summary>
	''' Provides a strongly typed collection for dialog controls.
	''' </summary>
	''' <typeparam name="T">DialogControl</typeparam>
	Public NotInheritable Class CommonFileDialogControlCollection(Of T As DialogControl)
		Inherits Collection(Of T)
		Private hostingDialog As IDialogControlHost

		Friend Sub New(host As IDialogControlHost)
			hostingDialog = host
		End Sub

		''' <summary>
		''' Inserts an dialog control at the specified index.
		''' </summary>
		''' <param name="index">The location to insert the control.</param>
		''' <param name="control">The item to insert.</param>
		''' <permission cref="System.InvalidOperationException">A control with 
		''' the same name already exists in this collection -or- 
		''' the control is being hosted by another dialog -or- the associated dialog is 
		''' showing and cannot be modified.</permission>
		Protected Overrides Sub InsertItem(index As Integer, control As T)
			' Check for duplicates, lack of host, 
			' and during-show adds.
			If Items.Contains(control) Then
				Throw New InvalidOperationException(LocalizedMessages.DialogControlCollectionMoreThanOneControl)
			End If
			If control.HostingDialog IsNot Nothing Then
				Throw New InvalidOperationException(LocalizedMessages.DialogControlCollectionRemoveControlFirst)
			End If
			If Not hostingDialog.IsCollectionChangeAllowed() Then
				Throw New InvalidOperationException(LocalizedMessages.DialogControlCollectionModifyingControls)
			End If
			If TypeOf control Is CommonFileDialogMenuItem Then
				Throw New InvalidOperationException(LocalizedMessages.DialogControlCollectionMenuItemControlsCannotBeAdded)
			End If

			' Reparent, add control.
			control.HostingDialog = hostingDialog
			MyBase.InsertItem(index, control)

			' Notify that we've added a control.
			hostingDialog.ApplyCollectionChanged()
		End Sub

		''' <summary>
		''' Removes the control at the specified index.
		''' </summary>
		''' <param name="index">The location of the control to remove.</param>
		''' <permission cref="System.InvalidOperationException">
		''' The associated dialog is 
		''' showing and cannot be modified.</permission>
		Protected Overrides Sub RemoveItem(index As Integer)
			Throw New NotSupportedException(LocalizedMessages.DialogControlCollectionCannotRemoveControls)
		End Sub

        ''' <summary>
        ''' Defines the indexer that supports accessing controls by name. If there is more than one control with the same name, only the <B>first control</B> will be returned.
        ''' </summary>
        ''' <remarks>
        ''' <para>Control names are case sensitive.</para>
        ''' <para>This indexer is useful when the dialog is created in XAML
        ''' rather than constructed in code.</para></remarks>
        '''<exception cref="System.ArgumentException">
        ''' The name cannot be null or a zero-length string.</exception>
        Default Public Overloads ReadOnly Property Item(name As String) As T
            Get
                If String.IsNullOrEmpty(name) Then
                    Throw New ArgumentException(LocalizedMessages.DialogControlCollectionEmptyName, "name")
                End If

                For Each control As T In MyBase.Items
                    Dim groupBox As CommonFileDialogGroupBox
                    ' NOTE: we don't ToLower() the strings - casing effects 
                    ' hash codes, so we are case-sensitive.
                    If control.Name = name Then
                        Return control
                    ElseIf groupBox.DirectCopy(TryCast(control, CommonFileDialogGroupBox)) IsNot Nothing Then
                        For Each subControl As T In groupBox.Items
                            If subControl.Name = name Then
                                Return subControl
                            End If
                        Next
                    End If
                Next
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Recursively searches for the control who's id matches the value
        ''' passed in the <paramref name="id"/> parameter.
        ''' </summary>
        ''' 
        ''' <param name="id">An integer containing the identifier of the 
        ''' control being searched for.</param>
        ''' 
        ''' <returns>A DialogControl who's id matches the value of the
        ''' <paramref name="id"/> parameter.</returns>
        ''' 
        Friend Function GetControlbyId(id As Integer) As DialogControl
			Return GetSubControlbyId(Items.Cast(Of DialogControl)(), id)
		End Function

		''' <summary>
		''' Recursively searches for a given control id in the 
		''' collection passed via the <paramref name="controlCollection"/> parameter.
		''' </summary>
		''' 
		''' <param name="controlCollection">A Collection&lt;CommonFileDialogControl&gt;</param>
		''' <param name="id">An int containing the identifier of the control 
		''' being searched for.</param>
		''' 
		''' <returns>A DialogControl who's Id matches the value of the
		''' <paramref name="id"/> parameter.</returns>
		''' 
		Friend Function GetSubControlbyId(controlCollection As IEnumerable(Of DialogControl), id As Integer) As DialogControl
			' if ctrlColl is null, it will throw in the foreach.
			If controlCollection Is Nothing Then
				Return Nothing
			End If

			For Each control As DialogControl In controlCollection
				If control.Id = id Then
					Return control
				End If

				' Search GroupBox child items
				Dim groupBox As CommonFileDialogGroupBox = TryCast(control, CommonFileDialogGroupBox)
				If groupBox IsNot Nothing Then
					Dim temp = GetSubControlbyId(groupBox.Items, id)
					If temp IsNot Nothing Then
						Return temp
					End If
				End If
			Next

			Return Nothing
		End Function

    End Class
End Namespace
