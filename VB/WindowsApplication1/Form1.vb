Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections
Imports System.Windows.Forms

Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub
		Private dataTable As DataTable
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			dataTable = New DataTable()
			dataTable.Columns.Add("Column1")
			dataTable.Columns.Add("Column2", GetType(DateTime))
			dataTable.Rows.Add(1, DateTime.Now)
			dataTable.Rows.Add(2, DateTime.Now.AddDays(1))
			dataTable.Rows.Add(3, DateTime.Now.AddMonths(1))
			dataTable.Rows.Add(4, DateTime.Now.AddYears(1))
			dataTable.Rows.Add(5, DateTime.Now.AddYears(1).AddDays(1))
			dataTable.Rows.Add(6, DateTime.Now.AddYears(2))
			dataTable.Rows.Add(7, DateTime.Now.AddYears(3))
			dataTable.Rows.Add(8, DateTime.Now.AddYears(4))
			gridControl1.DataSource = dataTable
			gridView1.Columns("Column2").OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.List
		End Sub

		Private Sub gridView1_ShowFilterPopupListBox(ByVal sender As Object, ByVal e As FilterPopupListBoxEventArgs) Handles gridView1.ShowFilterPopupListBox
			If e.Column.FieldName = "Column2" Then
				Dim currentYear As Integer = -1
				Dim newItems As New ArrayList()
				For i As Integer = 0 To e.ComboBox.Items.Count - 1
					Dim item As FilterItem = TryCast(e.ComboBox.Items(i), FilterItem)
					If item IsNot Nothing Then
						If TypeOf item.Value Is DateTime Then
							Dim [date] As DateTime = CDate(item.Value)
							If [date].Year <> currentYear Then
								currentYear = [date].Year
								Dim filter As String = GetFilterString(e.Column.FieldName, [date])
								newItems.Add(New FilterItem(currentYear.ToString(), New ColumnFilterInfo(filter)))
							End If
						End If
					End If
				Next i
				e.ComboBox.Items.Clear()
				For Each item As Object In newItems
					e.ComboBox.Items.Add(item)
				Next item
			End If
		End Sub
		Protected Overridable Function GetFilterString(ByVal columnName As String, ByVal [date] As DateTime) As String
			Dim date1Str, date2Str As String
			Dim date1 As DateTime = [date].AddDays(1 - [date].DayOfYear).Subtract([date].TimeOfDay)
			Dim date2 As DateTime = date1.AddYears(1)
			date1Str = "#" & date1.ToString("g") & "#"
			date2Str = "#" & date2.ToString("g") & "#"
			Return String.Format("[{0}] >= #{1}# AND [{0}] < #{2}#", columnName, date1.ToString("d"), date2.ToString("d"))
		End Function
	End Class
End Namespace
