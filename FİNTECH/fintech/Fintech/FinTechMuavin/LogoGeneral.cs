using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Fintech
{
    class LogoGeneral
    {
        public LogoGeneral(DataGridView dataGrid) //The body method of the class
        {
            ExcelWrite excel = new ExcelWrite();
            var sonuclar = GetTable("Information");

            BeginNumber(sonuclar, "191"); // it takes only the rows that "Hesap Kodu" begins with 191
            String str;  //declared to take "Açıklama" into a more readable variable 
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                str = sonuclar.Rows[i]["Açıklama"].ToString();// get the "Açıklama" column into the variable

                if (str.Contains("Satınalma")) // does not do anything, it passes over...
                    continue;
                //if we have one of these in "Açıklama" column, BillType() func. will split it into related columns
                else if (str.Contains("FT") || str.Contains("MK") || str.Contains("DK") || str.Contains("NOTER MAKBUZU")
                    || str.Contains("SMM") || str.Contains("AVANS HARCAMASI") || str.Contains("MAKBUZ NO")
                    || str.Contains("SF") || str.Contains("FİŞ") || str.StartsWith("KK"))
                { BillType(sonuclar, i); }
                //else {] DoNothingWithTheDataForNow();

                //does the calculation for "Tutar" column (Tutar=Borç-Alacak)
                sonuclar.Rows[i]["Tutar"] = Double.Parse(sonuclar.Rows[i]["Borç"].ToString()) - Double.Parse(sonuclar.Rows[i]["Alacak"].ToString());

                //does the calculation for "İşlem Döviz Borç" and "İşlem Döviz Alacak" columns according to "İşlem Döviz Bakiye" column
                if (Double.Parse(sonuclar.Rows[i]["İşlem Döviz Bakiye"].ToString()) > 0)
                {
                    sonuclar.Rows[i]["İşlem Döviz Borç"] = sonuclar.Rows[i]["İşlem Döviz Bakiye"];
                    sonuclar.Rows[i]["İşlem Döviz Alacak"] = 0;
                }
                else
                {
                    sonuclar.Rows[i]["İşlem Döviz Alacak"] = sonuclar.Rows[i]["İşlem Döviz Bakiye"];
                    sonuclar.Rows[i]["İşlem Döviz Borç"] = 0;
                }

                //does the calculation for "İşlem Döviz Tutar" column (İşlem Döviz Tutar = İşlem Döviz Borç - İşlem Döviz Alacak)
                sonuclar.Rows[i]["İşlem Döviz Tutar"] = Double.Parse(sonuclar.Rows[i]["İşlem Döviz Borç"].ToString()) - Double.Parse(sonuclar.Rows[i]["İşlem Döviz Alacak"].ToString());

                //does the calculation for "Döviz Kur" column
                //it may be a wrong calculation
                // Döviz Kur = Borç/Bakiye ??
                sonuclar.Rows[i]["Döviz Kur"] = Double.Parse(sonuclar.Rows[i]["Borç"].ToString()) / Double.Parse(sonuclar.Rows[i]["Bakiye"].ToString());

                if (str.StartsWith("Devreden")) //Deletes the row if "Açıklama" begins with "Devreden"
                    sonuclar.Rows[i].Delete();
            }
            sonuclar.AcceptChanges();
            //excel.ExcelWritten(sonuclar,"Logo Genel");
            dataGrid.DataSource = sonuclar;
        }
        #region The Methods that manipulates the data and puts to a DataTable object
        private DataTable GetTable(String tableName)
        {
            //For the column names according to Fintech ver. 2.0 in order
            String[] columnNames = {"Hesap Kodu","Hesap Adı", "YEVMİYE TARİHİ", "Fiş Türü" , "Fiş No.", "Açıklama",
            "Borç", "Alacak", "Bakiye", "Tutar", "İşlem Döviz Borç", "İşlem Döviz Alacak","İşlem Döviz Bakiye","İşlem Döviz Tutar",
            " ", "Döviz Adı", "Döviz Kur", "Belge Seri No", "Belge No", "Unvan","  ", "   "};

            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + variables.filePath + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
            connection.Open();
            OleDbCommand sec = new OleDbCommand("SELECT * FROM [" + variables.sheetName + "]", connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sec);
            DataTable DTexcel = new DataTable();
            adapter.Fill(DTexcel);

            for (int i = 0; i < 3; i++)
            { DTexcel.Columns.Remove(DTexcel.Columns[2]); } //deletes the columns "Hesap Özel Kodu", "Hesap Yetki Kodu", "Birim"

            for (int i = 0; i < 22; i++)//we have 21 columns
            {        
                if ((i >= 9 & i <= 11) || i == 13 || i == 14 || i > 15 & i < 19)
                {
                    DTexcel.Columns.Remove(DTexcel.Columns[i]); //We do not need old columns, first we delete
                    DTexcel.Columns.Add(columnNames[i], typeof(string)).SetOrdinal(i); // second we create a new one with name and order
                }
                else if (i == 12)//we need to change the order of "Bakiye" and "Döviz Adı"
                {
                    //since we delete 3 columns from the original table
                    DTexcel.Columns[15].SetOrdinal(12); // the column index of "Bakiye" is not 18, it's 18-3= 15
                    DTexcel.Columns[13].SetOrdinal(15); // (D.Adı column index=12) it's 13 because we set the order of "Bakiye" at 12
                    DTexcel.Columns[i].ColumnName = columnNames[i];
                }
                else if (i > 18)
                    DTexcel.Columns.Add(columnNames[i], typeof(string)); //Creates new columns and sets column's order
                else//if nothing to do else, then gives their names
                    DTexcel.Columns[i].ColumnName = columnNames[i]; // Sets the name of the columns from columnNames array
            }

            var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
            var table = reader.GetSchemaTable();
            connection.Close();
            return DTexcel;
        }

        void BeginNumber(DataTable sonuclar, string number)//Fetch the data according to "Hesap Kodu"
        {
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                if (!sonuclar.Rows[i][0].ToString().StartsWith(number))
                    sonuclar.Rows[i].Delete();
            }
            sonuclar.AcceptChanges();
        }
        #endregion

        #region Methods for Splitting the "Açıklama"

        void BillType(DataTable sonuclar, int i) // The function of "FT" ("Fatura")
        {
            String str = sonuclar.Rows[i]["Açıklama"].ToString();// "Açıklama" string'i

            //editting all to the same
            if (str.Contains("NOTER MAKBUZU"))
                str = str.Replace("NOTER MAKBUZU", "FT");
            else if (str.Contains("MKB"))
                str = str.Replace("MKB", "FT");
            else if (str.Contains("MK"))
                str = str.Replace("MK", "FT");
            else if (str.Contains("DK"))
                str = str.Replace("DK", "FT");
            /*else if (str.Contains("GARANTİ"))
                str = str.Replace("GARANTİ", "FT");*/
            else if (str.Contains("SMM"))
                str = str.Replace("SMM", "FT");
            else if (str.Contains("AVANS HARCAMASI"))
                str = str.Replace("AVANS HARCAMASI", "FT");
            else if (str.Contains("MAKBUZ NO"))
                str = str.Replace("MAKBUZ NO", "FT");
            else if (str.Contains("FİŞ "))
                str = str.Replace("FİŞ ", "FT");
            else if (str.Contains("FİŞ"))
                str = str.Replace("FİŞ", "FT");
            else if (str.Contains("SF"))
                str = str.Replace("SF", "FT");
            if (str.Contains("NOLU"))
                str = str.Replace("NOLU", "");
            if (str.Contains("E-BİLET"))
                str = str.Replace("E-BİLET", "");
            if (str.Contains("E BİLET NO"))
                str = str.Replace("E BİLET NO", "");
            if (str.StartsWith("KK"))
            {
                str = str.Replace("FT", "");
                if (str.Substring(0, 10).Contains("/"))
                    str = "FT" + str.Split('/')[1];
                else if (str.Substring(0, 10).Contains("-"))
                    str = "FT" + str.Split('-')[1];
            }

            int begin = str.IndexOf("FT") + 2, end = begin; // "begin" is the first index of the "Belge No" --- "end" is the last index
            long belgeNo;

            try
            {
                //To find the beginning index of the "belge no"
                for (; begin < str.Length;)
                {
                    if (!long.TryParse(str.Substring(begin, 1), out belgeNo))
                        begin++;
                    else
                        break;
                }
                //To find the end index of the "belge no" wtih help of TryParse method
                for (end = begin + 1; end < str.Length;)
                {
                    if (long.TryParse(str.Substring(end, 1), out belgeNo))
                        end++;
                    else
                    { end--; break; }
                }

                //Assigning the "belge no" by the help of the beginning and the end indexes
                sonuclar.Rows[i]["Belge No"] = str.Substring(begin, end - begin + 1).Trim();

                //Creating belgeSeriNo string 
                var belgeSeriNo = str.Substring(str.IndexOf("FT") + 2, (begin - (str.IndexOf("FT") + 2))).Trim();

                //Cleaning and assigning "Belge Seri No(from "-" and ":" etc.)
                if (belgeSeriNo.Contains("-"))
                    belgeSeriNo = belgeSeriNo.Replace("-", "").Trim();
                if (belgeSeriNo.Contains(":"))
                    belgeSeriNo = belgeSeriNo.Replace(":", "").Trim();
                if (belgeSeriNo.Contains("."))
                    belgeSeriNo = belgeSeriNo.Replace(".", "").Trim();
                sonuclar.Rows[i]["Belge Seri No"] = belgeSeriNo;

                //Editting the "belge no" of the "e-fatura"s
                if ((end - begin) > 9)//if it is greater than 9 number then it means that is "e-fatura"
                {
                    sonuclar.Rows[i]["Belge No"] = sonuclar.Rows[i]["Belge Seri No"] + sonuclar.Rows[i]["Belge No"].ToString();
                    sonuclar.Rows[i]["Belge Seri No"] = "";
                }
                //Assigning the "unvan"
                sonuclar.Rows[i]["Unvan"] = str.Substring(end + 1).Trim();
            }
            catch (Exception) { } //doNothing();

            //Cleaning and assigning "Unvan"
            String unvan = sonuclar.Rows[i]["Unvan"].ToString();
            if (unvan.Equals("") || unvan == null)
                unvan = str.Substring(str.IndexOf("FT") + 2).Trim();
            if (unvan.Trim().StartsWith("-"))
                unvan = unvan.ToString().Replace("-", "").Trim();
            if (unvan.Trim().StartsWith(":"))
                unvan = unvan.Replace(":", "").Trim();
            if (unvan.Trim().StartsWith("/"))
                unvan = unvan.Replace("/", "").Trim();
            if (unvan.Contains("BORÇ"))
                unvan = unvan.Replace("BORÇ DEKONTU", "").Trim();
            if (unvan.Contains("/"))
                unvan = unvan.Remove(unvan.IndexOf("/")).Trim();
            if (unvan.Contains("-"))
                unvan = unvan.Remove(unvan.IndexOf("-")).Trim();
            sonuclar.Rows[i]["Unvan"] = unvan;
            /*if (sonuclar.Rows[i][8].ToString().Contains("GARANTİ"))
                sonuclar.Rows[i][7] = "GARANTİ";*/

            AlertEnumber(sonuclar, i);
        }
        #endregion

        void AlertEnumber(DataTable sonuclar, int i)
        {
            /*if (sonuclar.Rows[i][""].ToString().Length == 15)
            {
                //uyarı vericek.
            }*/
        }

        void DocumentNumber()
        {

        }
    }
}
