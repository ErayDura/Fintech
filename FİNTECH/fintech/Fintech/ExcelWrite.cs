using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
namespace Fintech
{
	class ExcelWrite
	{
		public void ExcelWritten(DataTable a, string excelName)
		{
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Dosyası|*.xlsx";
            if (save.ShowDialog() == DialogResult.OK)
            {
                var wb = new XLWorkbook();
                var dataTable = a;
                wb.Worksheets.Add(dataTable, "sayfa1");
                wb.SaveAs(variables.savePath);
                MessageBox.Show("Excele Yazıldı.");
            }
		}
	}
}
