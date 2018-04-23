using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;

using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace WindowsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        DataTable dataTable;
        private void Form1_Load(object sender, EventArgs e) {
            dataTable = new DataTable();
            dataTable.Columns.Add("Column1");
            dataTable.Columns.Add("Column2", typeof(DateTime));
            dataTable.Rows.Add(1, DateTime.Now);
            dataTable.Rows.Add(2, DateTime.Now.AddDays(1));
            dataTable.Rows.Add(3, DateTime.Now.AddMonths(1));
            dataTable.Rows.Add(4, DateTime.Now.AddYears(1));
            dataTable.Rows.Add(5, DateTime.Now.AddYears(1).AddDays(1));
            dataTable.Rows.Add(6, DateTime.Now.AddYears(2));
            dataTable.Rows.Add(7, DateTime.Now.AddYears(3));
            dataTable.Rows.Add(8, DateTime.Now.AddYears(4));
            gridControl1.DataSource = dataTable;
            gridView1.Columns["Column2"].OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.List;
        }

        private void gridView1_ShowFilterPopupListBox(object sender, FilterPopupListBoxEventArgs e) {
            if (e.Column.FieldName == "Column2") {
                int currentYear = -1;
                ArrayList newItems = new ArrayList();
                for (int i = 0; i < e.ComboBox.Items.Count; i++) {
                    FilterItem item = e.ComboBox.Items[i] as FilterItem;
                    if (item != null) {
                        if (item.Value is DateTime) {
                            DateTime date = (DateTime)item.Value;
                            if (date.Year != currentYear) {
                                currentYear = date.Year;
                                string filter = GetFilterString(e.Column.FieldName, date);
                                newItems.Add(new FilterItem(currentYear.ToString(), new ColumnFilterInfo(filter)));
                            }
                        }
                    }
                }
                e.ComboBox.Items.Clear();
                foreach (object item in newItems) {
                    e.ComboBox.Items.Add(item);
                }
            }
        }
        protected virtual string GetFilterString(string columnName, DateTime date) {
            string date1Str, date2Str;
            DateTime date1 = date.AddDays(1 - date.DayOfYear).Subtract(date.TimeOfDay);
            DateTime date2 = date1.AddYears(1);
            date1Str = "#" + date1.ToString("g") + "#";
            date2Str = "#" + date2.ToString("g") + "#";
            return String.Format("[{0}] >= #{1}# AND [{0}] < #{2}#",
                   columnName, date1.ToString("d"), date2.ToString("d"));
        }
    }
}
