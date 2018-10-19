using System;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp22
{
     class Netsis
    {
        public Netsis(DataGridView dataGrid)
        {
            //ExcelWrite excel = new ExcelWrite();
            var sonuclar = GetTable("Information");
            string b = "";
            for (int i = 0; i < sonuclar.Rows.Count; i++) //Tarihin Hesap Kodu kısmıno alıyorum
            {
                
                if (sonuclar.Rows[i][2].ToString().StartsWith("Hesap Kodu"))
                {
                    sonuclar.Rows[i + 3][0] = sonuclar.Rows[i + 1][2].ToString();
                    b= sonuclar.Rows[i + 3][0].ToString();
                }

                sonuclar.Rows[i][0] = b;
            }
            sonuclar.Rows[2][0]= sonuclar.Rows[0][2].ToString();
            string c = "";
            for (int i = 0; i < sonuclar.Rows.Count; i++)//Açıklama kısmındaki Hesap adı kısmını çekme.
            {
                if (sonuclar.Rows[i][8].ToString().StartsWith("Hesap Adı"))
                {
                    sonuclar.Rows[i + 3][1] = sonuclar.Rows[i + 1][8].ToString();
                    c = sonuclar.Rows[i + 3][1].ToString();
                }
                sonuclar.Rows[i][1]= c;
            }
            sonuclar.Rows[2][1] = sonuclar.Rows[0][8].ToString();


            sonuclar.AcceptChanges();
            

            for (int i = 0; i < sonuclar.Rows.Count; i++) //belge No
                {
                    var a = sonuclar.Rows[i][8].ToString();
                    var s = a.Split(' ');
                Int64 serino;

                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("FN:"))
                    {
                        if (Int64.TryParse(s[N].Substring(3), out serino))
                        {
                            sonuclar.Rows[i][6] += s[N].Substring(3).ToString();
                        }
                        else if (s[N].Length == 4)
                        {
                          sonuclar.Rows[i][6] += s[N+1].ToString();
                        }
                        else 
                        {
                            String str2 = s[N].ToString();
                            Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                            Match result = re.Match(str2);
                            sonuclar.Rows[i][6] += result.Groups[2].Value;
                        }

                    }
                    else if (s[N].ToString().StartsWith("NO:"))
                    {
                        String str3 = s[N].ToString();
                        Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                        Match result = re.Match(str3);
                        sonuclar.Rows[i][6] += result.Groups[2].Value;
                    }

                }

                    //if (s[0].ToString().Length==1 && Int64.TryParse(s[1] , out serino))
                    //{
                    //    sonuclar.Rows[i][6] += s[1].ToString();
                    //}
                    
            }
            for(int i=0; i<sonuclar.Rows.Count; i++)
            {

            }

            for (int i = 0; i < sonuclar.Rows.Count; i++)//Belge Seri No
            {
                var a = sonuclar.Rows[i][8].ToString();
                var s = a.Split(' ');
                Int64 serino3;
                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("FN:"))
                    {
                        if (s[N].ToString().Length==4)
                         sonuclar.Rows[i][5] += s[N].ToString().Substring(3);
                    }
                     
                }
                //sonuclar.AcceptChanges();
                ////excel.ExcelWritten(sonuclar, "Netsis");
                //dataGrid.DataSource = sonuclar;
            }
            

            for (int i = 0; i < sonuclar.Rows.Count; i++)//SN kısımlarını seri noya yazdır.
            {
                var a = sonuclar.Rows[i][8].ToString();
                var s = a.Split(' ');
                Int64 serino;
                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("SN:")) { }
                    else if(s[N].StartsWith("A101")) { sonuclar.Rows[i][7] += "A 101"; }
                    else if (Int64.TryParse(s[N].ToString(), out serino)) { }
                    else if (s[N].ToString().StartsWith("FN")) { }
                    else if (s[N].ToString().StartsWith("NO")) { }
                    else if (s[N].ToString().StartsWith("MS")) { }
                    else if (s[N].ToString().StartsWith("KDV")) { }
                    else if (s[N].ToString().StartsWith("FT")) { }
                    else if (s[N].ToString().StartsWith("Fiş")) { }
                    else if (s[N].ToString().StartsWith("2018") && s[N].ToString().EndsWith("2018")) { }
                    else if (s[N].ToString().StartsWith("NL")) { }
                    else if (s[N].ToString().Contains("/")) { }
                    else if (s[N].ToString().StartsWith("KDV")) { }
                    else if (s[N].ToString().Equals("FİŞ")) { }
                    else if (s[N].Length < 3) { }
                    else if (s[N].ToString().Contains(".FT.NIZ")) { var d = s[N].ToString(); var f = a.Split('.'); sonuclar.Rows[i][7] += s[0]; }
                    else if (s[N].Length > 3 && Int64.TryParse(s[N].ToString().Substring(3), out serino)) { }
                    
                    else { sonuclar.Rows[i][7] += s[N].ToString() + " "; }
                    
                }


            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                if (sonuclar.Rows[i][2].ToString().EndsWith("2018"))
                {

                }
                else
                {
                    sonuclar.Rows[i].Delete();
                }
               
            }
             
            sonuclar.AcceptChanges();
            for (int i = 0; i < sonuclar.Rows.Count; i++)//191le başlayanlar
            {
                if (sonuclar.Rows[i][0].ToString().StartsWith("191"))
                {

                }
                else
                {
                    sonuclar.Rows[i].Delete();
                }

            }



            dataGrid.DataSource = sonuclar;
            }
        

        private DataTable GetTable(String tableName)
        {
            OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + variables.filePath + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
            baglanti.Open();
            OleDbCommand sec = new OleDbCommand("SELECT * FROM [Orjinal$]", baglanti);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sec);

            DataTable DTexcel = new DataTable();

            adapter.Fill(DTexcel);
            DTexcel.Columns[0].ColumnName = "Tarih";
            DTexcel.Columns.Add("Hesap Kodu", typeof(String)).SetOrdinal(0);
            DTexcel.Columns.Add("Hesap Adı ", typeof(String)).SetOrdinal(1);
            DTexcel.Columns.Add("Belge No", typeof(String)).SetOrdinal(3);
            DTexcel.Columns.Add("Belge Seri No", typeof(String)).SetOrdinal(4);
            DTexcel.Columns.Add("Unvan", typeof(String));
            DTexcel.Columns[7].ColumnName = "Açıklama";
            DTexcel.Columns[5].ColumnName = "Fiş No";
            DTexcel.Columns[6].ColumnName = "Sr";
            DTexcel.Columns[8].ColumnName = "Borç Tutarı";
            DTexcel.Columns[9].ColumnName = "Alacak Tutarı";
            DTexcel.Columns[10].ColumnName = "İşlem Döviz Borç Tutarı";
            DTexcel.Columns[11].ColumnName = "işlem Döviz Alacak Tutarı";
            DTexcel.Columns[12].ColumnName = "Bakiye Tutarı";
            DTexcel.Columns[13].ColumnName = "İşlem Döviz Bakiye Tutarı";
            DTexcel.Columns[14].ColumnName = "Firma Döviz";
            DTexcel.Columns[15].ColumnName = "Döviz Adı";
            DTexcel.Columns[16].ColumnName = "Döviz Kuru";
            DTexcel.Columns[5].SetOrdinal(3);
            DTexcel.Columns[6].SetOrdinal(4);
            DTexcel.Columns["Belge No"].SetOrdinal(6);
            DTexcel.Columns["Belge Seri No"].SetOrdinal(5);
            DTexcel.Columns["Unvan"].SetOrdinal(7);

            var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
            var table = reader.GetSchemaTable();

            baglanti.Close();
            return DTexcel;
        }
       

    }
}
