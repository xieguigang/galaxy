'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.ObjectModel
Imports System.Linq
Imports Microsoft.Windows.Resources

Namespace Dialogs
	''' <summary>
	''' Strongly typed collection for dialog controls.
	''' </summary>
	''' <typeparam name="T">DialogControl</typeparam>
	Public NotInheritable Class DialogControlCollection(Of T As DialogControl)
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
				Throw New InvalidOperationException(LocalizedMessages.DialogCollectionCannotHaveDuplicateNames)
			End If
			If control.HostingDialog IsNot Nothing Then
				Throw New InvalidOperationException(LocalizedMessages.DialogCollectionControlAlreadyHosted)
			End If
			If Not hostingDialog.IsCollectionChangeAllowed() Then
				Throw New InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog)
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
			' Notify that we're about to remove a control.
			' Throw if dialog showing.
			If Not hostingDialog.IsCollectionChangeAllowed() Then
				Throw New InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog)
			End If

			Dim control As DialogControl = DirectCast(Items(index), DialogControl)

			' Unparent and remove.
			control.HostingDialog = Nothing
			MyBase.RemoveItem(index)

			hostingDialog.ApplyCollectionChanged()
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
                    Throw New ArgumentException(LocalizedMessages.DialogCollectionControlNameNull, "name")
                End If

                Return Items.FirstOrDefault(Function(x) x.Name = name)
            End Get
        End Property

        ''' <summary>
        ''' Searches for the control who's id matches the value
        ''' passed in the <paramref name="id"/> parameter.
        ''' </summary>
        ''' 
        ''' <param name="id">An integer containing the identifier of the 
        ''' control being searched for.</param>
        ''' 
        ''' <returns>A DialogControl who's id matches the value of the
        ''' <paramref name="id"/> parameter.</returns>        
        Friend Function GetControlbyId(id As Integer) As DialogControl
			Return Items.FirstOrDefault(Function(x) x.Id = id)
		End Function
	End Class
End Namespace
