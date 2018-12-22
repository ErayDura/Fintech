using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.LucaNew
{
    public interface LucaN
    {
      //  OleDbConnection CreateOleDbConnection();
      //  DataTable ExcelToDataTable(OleDbConnection connection, DataTable mainDataTable);
       // DataTable ClassSwitcherRelatedToColumnNumber(DataTable mainDataTable, OleDbConnection connection);

        void generate();

        DataTable DataTableForNColumn();
        object GetTableName(OleDbConnection connection, OleDbCommand selectAllData);
        DataTable DeleteAndPlace(string accountName, string accountCode, string referanceWord, DataTable mainDataTable);
        DataTable AddNewColumns(DataTable mainDataTable);
        DataTable ChangeColumnNames(DataTable mainDataTable);
        DataTable SetColumnOrdinals(DataTable mainDataTable);
        DataTable DeleteUnnecessaryColumns(DataTable mainDataTable);
        DataTable ExchangeRate(DataTable mainDataTable);

    }
}
