Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

Namespace Docking
    Friend Class DockAreasEditor
        Inherits UITypeEditor
        Private Class DockAreasEditorControl
            Inherits UserControl
            Private checkBoxFloat As CheckBox
            Private checkBoxDockLeft As CheckBox
            Private checkBoxDockRight As CheckBox
            Private checkBoxDockTop As CheckBox
            Private checkBoxDockBottom As CheckBox
            Private checkBoxDockFill As CheckBox
            Private m_oldDockAreas As DockAreas

            Public ReadOnly Property DockAreas As DockAreas
                Get
                    Dim lDockAreas As DockAreas = 0
                    If checkBoxFloat.Checked Then lDockAreas = lDockAreas Or DockAreas.Float
                    If checkBoxDockLeft.Checked Then lDockAreas = lDockAreas Or DockAreas.DockLeft
                    If checkBoxDockRight.Checked Then lDockAreas = lDockAreas Or DockAreas.DockRight
                    If checkBoxDockTop.Checked Then lDockAreas = lDockAreas Or DockAreas.DockTop
                    If checkBoxDockBottom.Checked Then lDockAreas = lDockAreas Or DockAreas.DockBottom
                    If checkBoxDockFill.Checked Then lDockAreas = lDockAreas Or DockAreas.Document

                    If lDockAreas = 0 Then
                        Return m_oldDockAreas
                    Else
                        Return lDockAreas
                    End If
                End Get
            End Property

            Public Sub New()
                checkBoxFloat = New CheckBox()
                checkBoxDockLeft = New CheckBox()
                checkBoxDockRight = New CheckBox()
                checkBoxDockTop = New CheckBox()
                checkBoxDockBottom = New CheckBox()
                checkBoxDockFill = New CheckBox()

                SuspendLayout()

                checkBoxFloat.Appearance = Appearance.Button
                checkBoxFloat.Dock = DockStyle.Top
                checkBoxFloat.Height = 24
                checkBoxFloat.Text = ResourceHelper.DockAreaEditor_FloatCheckBoxText
                checkBoxFloat.TextAlign = ContentAlignment.MiddleCenter
                checkBoxFloat.FlatStyle = FlatStyle.System

                checkBoxDockLeft.Appearance = Appearance.Button
                checkBoxDockLeft.Dock = DockStyle.Left
                checkBoxDockLeft.Width = 24
                checkBoxDockLeft.FlatStyle = FlatStyle.System

                checkBoxDockRight.Appearance = Appearance.Button
                checkBoxDockRight.Dock = DockStyle.Right
                checkBoxDockRight.Width = 24
                checkBoxDockRight.FlatStyle = FlatStyle.System

                checkBoxDockTop.Appearance = Appearance.Button
                checkBoxDockTop.Dock = DockStyle.Top
                checkBoxDockTop.Height = 24
                checkBoxDockTop.FlatStyle = FlatStyle.System

                checkBoxDockBottom.Appearance = Appearance.Button
                checkBoxDockBottom.Dock = DockStyle.Bottom
                checkBoxDockBottom.Height = 24
                checkBoxDockBottom.FlatStyle = FlatStyle.System

                checkBoxDockFill.Appearance = Appearance.Button
                checkBoxDockFill.Dock = DockStyle.Fill
                checkBoxDockFill.FlatStyle = FlatStyle.System

                Controls.AddRange(New Control() {checkBoxDockFill, checkBoxDockBottom, checkBoxDockTop, checkBoxDockRight, checkBoxDockLeft, checkBoxFloat})

                Size = New Size(160, 144)
                MyBase.BackColor = SystemColors.Control
                ResumeLayout()
            End Sub

            Public Sub SetStates(dockAreas As DockAreas)
                m_oldDockAreas = dockAreas
                If (dockAreas And DockAreas.DockLeft) <> 0 Then checkBoxDockLeft.Checked = True
                If (dockAreas And DockAreas.DockRight) <> 0 Then checkBoxDockRight.Checked = True
                If (dockAreas And DockAreas.DockTop) <> 0 Then checkBoxDockTop.Checked = True
                If (dockAreas And DockAreas.DockTop) <> 0 Then checkBoxDockTop.Checked = True
                If (dockAreas And DockAreas.DockBottom) <> 0 Then checkBoxDockBottom.Checked = True
                If (dockAreas And DockAreas.Document) <> 0 Then checkBoxDockFill.Checked = True
                If (dockAreas And DockAreas.Float) <> 0 Then checkBoxFloat.Checked = True
            End Sub
        End Class

        Private m_ui As DockAreasEditorControl = Nothing

        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.DropDown
        End Function

        Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            If m_ui Is Nothing Then m_ui = New DockAreasEditorControl()

            m_ui.SetStates(DirectCast(value, DockAreas))

            Dim edSvc = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
            edSvc.DropDownControl(m_ui)

            Return m_ui.DockAreas
        End Function
    End Class
End Namespace
