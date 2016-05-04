'
'    ChromeTabControl is a .Net control that mimics Google Chrome's tab bar.
'    Copyright (C) 2013  Brandon Francis
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Namespace ChromeTabControl

    ''' <summary>
    ''' Items that go in the tab cotrol's tray.
    ''' </summary>
    Public Class TrayItem

        ''' <summary>
        ''' Raises a clicked event for this tray item.
        ''' </summary>
        ''' <param name="rect">The rectangle of this item.</param>
        Friend Sub RaiseClicked(rect As RectangleF)
            If _type = TrayItemType.Button Then
                RaiseEvent Clicked(rect)
            End If
        End Sub
        Public Event Clicked As ClickedEventHandler
        Public Delegate Sub ClickedEventHandler(rect As RectangleF)

        ''' <summary>
        ''' Sets the TabControl that owns this tray item.
        ''' This is used for refreshing the TabControl when
        ''' this item is updated.
        ''' </summary>
        ''' <param name="owner">The TabControl that owns this tray item.</param>
        Friend Sub SetOwnerTabControl(owner As ChromeTabControl)
            _tabControl = owner
        End Sub
        Private _tabControl As ChromeTabControl

        ''' <summary>
        ''' Creates a new tray item given an icon.
        ''' </summary>
        ''' <param name="icon">The icon to display for this item.</param>
        Public Sub New(icon As Bitmap)
            Image = icon
        End Sub

        ''' <summary>
        ''' Invalidates the tab control that owns this tray item.
        ''' </summary>
        Private Sub RefreshControl()
            If (_tabControl IsNot Nothing) Then
                _tabControl.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' The different tray item functionality types.
        ''' </summary>
        Public Enum TrayItemType As Byte
            Button = 0
            Icon = 1
        End Enum

        ''' <summary>
        ''' The tray item type for this tray item.
        ''' This decides what functionality the item has.
        ''' </summary>
        Public Property Type() As TrayItemType
            Get
                Return _type
            End Get
            Set
                If Value <> _type Then
                    _type = Value
                    RefreshControl()
                End If
            End Set
        End Property
        Private _type As TrayItemType = TrayItemType.Button

        ''' <summary>
        ''' Sets the image for this tray item.
        ''' </summary>
        Public WriteOnly Property Image() As Bitmap
            Set
                _image16 = Utils.ResizeBitmap(Value, New Size(16, 16))
                _image16Transparent = Utils.SetBitmapOpacity(_image16, 80)
                _image16Dark = Utils.TintBitmap(_image16, Color.Black, 30)
                RefreshControl()
            End Set
        End Property

        ''' <summary>
        ''' The 16x16 image for this tray item.
        ''' </summary>
        Public ReadOnly Property Image16() As Bitmap

        ''' <summary>
        ''' The 16x16 slightly transparent image for this tray item.
        ''' </summary>
        Public ReadOnly Property Image16Transparent() As Bitmap

        ''' <summary>
        ''' The 16x16 dark image for this tray item.
        ''' </summary>
        Public ReadOnly Property Image16Dark() As Bitmap

        ''' <summary>
        ''' Collection of tray items that syncs with the tab control.
        ''' </summary>
        Public Class TrayItemCollection
            Implements IList(Of TrayItem)

            Private _tabControl As ChromeTabControl

            Private _list As New List(Of TrayItem)()
            Friend Sub Changed()
                If (_tabControl IsNot Nothing) Then
                    _tabControl.UpdateAreas()
                End If
            End Sub

            Public Sub New(owner As ChromeTabControl)
                _tabControl = owner
            End Sub

            Public Sub Add(item As TrayItem) Implements IList(Of TrayItem).Add
                item.SetOwnerTabControl(_tabControl)
                _list.Add(item)
                Changed()
            End Sub

            Public Sub Clear() Implements ICollection(Of TrayItem).Clear
                For i As Integer = 0 To _list.Count - 1
                    _list(i).SetOwnerTabControl(DirectCast(Nothing, ChromeTabControl))
                Next
                _list.Clear()
                Changed()
            End Sub

            Public Function Contains(item As TrayItem) As Boolean Implements IList(Of TrayItem).Contains
                Return _list.Contains(item)
            End Function

            Public Sub CopyTo(array As TrayItem(), arrayIndex As Integer) Implements IList(Of TrayItem).CopyTo
                _list.CopyTo(array, arrayIndex)
            End Sub

            Public ReadOnly Property Count() As Integer Implements ICollection(Of TrayItem).Count
                Get
                    Return _list.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of TrayItem).IsReadOnly
                Get
                    Return True
                End Get
            End Property

            Public Function Remove(item As TrayItem) As Boolean Implements IList(Of TrayItem).Remove
                Dim rtn As Boolean = _list.Remove(item)
                Changed()
                Return rtn
            End Function

            Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of TrayItem) Implements IEnumerable(Of TrayItem).GetEnumerator
                Return _list.GetEnumerator()
            End Function

            Public Function IndexOf(item As TrayItem) As Integer Implements IList(Of TrayItem).IndexOf
                Return _list.IndexOf(item)
            End Function

            Public Sub Insert(index As Integer, item As TrayItem) Implements IList(Of TrayItem).Insert
                item.SetOwnerTabControl(_tabControl)
                _list.Insert(index, item)
                Changed()
            End Sub

            Default Public Property Item(index As Integer) As TrayItem Implements IList(Of TrayItem).Item
                Get
                    Return _list(index)
                End Get
                Set
                    Value.SetOwnerTabControl(_tabControl)
                    _list(index) = Value
                    Changed()
                End Set
            End Property

            Public Sub RemoveAt(index As Integer) Implements IList(Of TrayItem).RemoveAt
                _list.RemoveAt(index)
                Changed()
            End Sub

            Public Function GetEnumerator1() As System.Collections.IEnumerator
                Return _list.GetEnumerator()
            End Function
            Private Function System_Collections_IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return GetEnumerator1()
            End Function

        End Class

    End Class
End Namespace