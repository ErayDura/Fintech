using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fintech.LucaNew
{
    class Luca13 : LucaN
    {
        private DataGridView dtg;
        private DataTable mainDataTable;
        private OleDbConnection connection;
        public Luca13(DataGridView dtg, DataTable mainDataTable, OleDbConnection connection)
        {
            this.mainDataTable = mainDataTable;
            this.connection = connection;
            this.dtg = dtg;
            generate();
        }
        public DataTable AddNewColumns(DataTable mainDataTable)
        {
            mainDataTable.Columns.Add("HESAP ADI");
            mainDataTable.Columns.Add("HESAP KODU");
            mainDataTable.Columns.Add("BELGE TARİHİ");
            mainDataTable.Columns.Add("BELGE SERİ");
            mainDataTable.Columns.Add("BELGE NO");
            mainDataTable.Columns.Add("ÜNVAN");
            mainDataTable.Columns.Add("TUTAR");
            mainDataTable.Columns.Add("FİRMA DÖVİZ");
            mainDataTable.Columns.Add("DÖVİZ ADI");
            mainDataTable.Columns.Add("SR");
            mainDataTable.Columns.Add("İŞLEM DÖVİZ TUTAR");
            return mainDataTable;
        }

        public DataTable ChangeColumnNames(DataTable mainDataTable)
        {
            mainDataTable.Columns["F1"].ColumnName = "YEVMİYE TARİHİ";
            mainDataTable.Columns["F3"].ColumnName = "FİŞ NO";
            mainDataTable.Columns["F2"].ColumnName = "FİŞ TÜRÜ";
            mainDataTable.Columns["F4"].ColumnName = "AÇIKLAMA";
            mainDataTable.Columns["F5"].ColumnName = "BORÇ";
            mainDataTable.Columns["F6"].ColumnName = "ALACAK";
            mainDataTable.Columns["F7"].ColumnName = "BAKİYE";
            mainDataTable.Columns["F9"].ColumnName = "İŞLEM DÖVİZ BORÇ";
            mainDataTable.Columns["F10"].ColumnName = "İŞLEM DÖVİZ ALACAK";
            mainDataTable.Columns["F11"].ColumnName = "İŞLEM DÖVİZ BAKİYE";
            mainDataTable.Columns["F13"].ColumnName = "DÖVİZ KUR";
            return mainDataTable;
        }

        public DataTable DataTableForNColumn()
        {
            OleDbCommand selectAllData = new OleDbCommand("SELECT * FROM [" + variables.sheetName + "]", connection);
            var tablename = GetTableName(connection, selectAllData);
            var accountCode = mainDataTable.Rows[4][0].ToString();
            var accountName = mainDataTable.Rows[4][1].ToString();
            mainDataTable = AddNewColumns(mainDataTable);
            connection.Open();
            OleDbCommand findReferanceCommand = new OleDbCommand("SELECT * FROM [" + variables.sheetName + "] WHERE [" + tablename + "] LIKE '%NAKL%'", connection);
            var referanceWord = findReferanceCommand.ExecuteScalar().ToString();
            connection.Close();
            mainDataTable.Rows[0].Delete();
            mainDataTable = DeleteAndPlace(accountName, accountCode, referanceWord, mainDataTable);
            mainDataTable.AcceptChanges();//Değişiklikler uygulanıyor.
            GC.Collect();
            Console.WriteLine(mainDataTable.Rows[0][0].ToString());
            Console.WriteLine(mainDataTable.Columns.Count.ToString());

            mainDataTable = ChangeColumnNames(mainDataTable);

            mainDataTable = SetColumnOrdinals(mainDataTable);


            //for (int i = 0; i < mainDataTable.Rows.Count; i++)
            //{
            //    if (mainDataTable.Rows[i]["HESAP KODU"].ToString().StartsWith("191"))
            //    {
            //        LucaFilter lucaFilter = new LucaFilter(mainDataTable);
            //        mainDataTable = lucaFilter.DoFilter();

            //    }
            //    else
            //    {
            //        continue;
            //    }
            //}


            mainDataTable = DeleteUnnecessaryColumns(mainDataTable);

            //mainDataTable = ExchangeRate(mainDataTable);

            mainDataTable.AcceptChanges();


            return mainDataTable;
        }

        public DataTable DeleteAndPlace(string accountName, string accountCode, string referanceWord, DataTable mainDataTable)
        {
            for (int i = 1; i < mainDataTable.Rows.Count; i++)
            {
                //Aldığımız değerleri oluşturduğumuz kolonların içine atıyoruz.
                mainDataTable.Rows[i]["HESAP KODU"] = accountCode.ToString();
                mainDataTable.Rows[i]["HESAP ADI"] = accountName;
                //----------------------------------------------------------------

                try
                {
                    if (mainDataTable.Rows[i][0].ToString() == referanceWord)
                    {
                        accountCode = mainDataTable.Rows[i - 2][0].ToString();
                        accountName = mainDataTable.Rows[i - 2][1].ToString();
                        mainDataTable.Rows[i].Delete();
                        mainDataTable.Rows[i - 1].Delete();
                        mainDataTable.Rows[i - 2].Delete();
                        mainDataTable.Rows[i - 3].Delete();
                        mainDataTable.Rows[i - 4].Delete();
                        mainDataTable.Rows[i - 5].Delete();
                    }
                    if (i == mainDataTable.Rows.Count - 1)
                    {
                        mainDataTable.Rows[i].Delete();
                        mainDataTable.Rows[i - 1].Delete();
                        mainDataTable.Rows[i - 2].Delete();
                    }

                }
                catch (Exception ba)
                {
                    MessageBox.Show(ba.ToString());
                }

            }
            return mainDataTable;
        }

        public DataTable DeleteUnnecessaryColumns(DataTable mainDataTable)
        {
            mainDataTable.Columns.Remove("F8");
            mainDataTable.Columns.Remove("F12");
            return mainDataTable;
        }

        public DataTable ExchangeRate(DataTable mainDataTable)
        {
           
            for (int i = 0; i < mainDataTable.Rows.Count; i++)
            {
                double a = Convert.ToDouble(mainDataTable.Rows[i]["ALACAK"].ToString());
                double b = Convert.ToDouble(mainDataTable.Rows[i]["İŞLEM DÖVİZ ALACAK"].ToString());
                //mainDataTable.Rows[i]["Döviz Kur"].DataType = typeof(string);
                if (b == 0)
                {
                    mainDataTable.Rows[i]["DÖVİZ KUR"] = "TRY";
                    
                }
                else
                {

                    var exchangeRateResult = a / b;
                    if (exchangeRateResult == 1)
                    {
                        mainDataTable.Rows[i]["DÖVİZ KUR"] = "TRY";
                    }
                }

            }
            return mainDataTable;
        }

        public void generate()
        {
            variables.FirmaAdi = mainDataTable.Rows[0][3].ToString();
            mainDataTable.Rows[0].Delete();
            mainDataTable.AcceptChanges();
            GC.Collect();
            dtg.DataSource = mainDataTable;
        }

        public object GetTableName(OleDbConnection connection, OleDbCommand selectAllData)
        {
            connection.Open();
            var reader = selectAllData.ExecuteReader(CommandBehavior.SchemaOnly);
            var table = reader.GetSchemaTable();
            var nameCol = table.Columns["ColumnName"];
            var tablename = table.Rows[0][nameCol];
            connection.Close();
            return tablename;
        }

        public DataTable SetColumnOrdinals(DataTable mainDataTable)
        {
            mainDataTable.Columns["HESAP KODU"].SetOrdinal(0);
            mainDataTable.Columns["HESAP ADI"].SetOrdinal(1);
            mainDataTable.Columns["YEVMİYE TARİHİ"].SetOrdinal(2);
            mainDataTable.Columns["FİŞ TÜRÜ"].SetOrdinal(3);
            mainDataTable.Columns["FİŞ NO"].SetOrdinal(4);
            mainDataTable.Columns["AÇIKLAMA"].SetOrdinal(5);
            mainDataTable.Columns["BORÇ"].SetOrdinal(6);
            mainDataTable.Columns["ALACAK"].SetOrdinal(7);
            mainDataTable.Columns["BAKİYE"].SetOrdinal(8);
            mainDataTable.Columns["TUTAR"].SetOrdinal(9);
            mainDataTable.Columns["İŞLEM DÖVİZ BORÇ"].SetOrdinal(10);
            mainDataTable.Columns["İŞLEM DÖVİZ ALACAK"].SetOrdinal(11);
            mainDataTable.Columns["İŞLEM DÖVİZ BAKİYE"].SetOrdinal(12);
            mainDataTable.Columns["İŞLEM DÖVİZ TUTAR"].SetOrdinal(13);
            mainDataTable.Columns["FİRMA DÖVİZ"].SetOrdinal(14);
            mainDataTable.Columns["DÖVİZ ADI"].SetOrdinal(15);
            mainDataTable.Columns["DÖVİZ KUR"].SetOrdinal(16);
            mainDataTable.Columns["BELGE SERİ"].SetOrdinal(17);
            mainDataTable.Columns["BELGE NO"].SetOrdinal(18);
            mainDataTable.Columns["ÜNVAN"].SetOrdinal(19);
            mainDataTable.Columns["BELGE TARİHİ"].SetOrdinal(20);
            mainDataTable.Columns["SR"].SetOrdinal(21);
            return mainDataTable; ;


            return mainDataTable;
        }
    }
}