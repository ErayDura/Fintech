
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Fintech
{
    class LucaFilter
    {
        private DataTable sonuclar;

        public LucaFilter(DataTable sonuclar)
        {
            this.sonuclar = sonuclar;
        }


        public DataTable DoFilter()
        {
            sonuclar.AcceptChanges();
            string[] splitText = null;
            string text = null;

            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                try
                {
                    text = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
                    splitText = text.Split('-');
                    sonuclar.Rows[i]["BELGE TARİHİ"] = splitText[0];
                    if (splitText[1].Length != 16 && splitText[1].Length != 15)
                    {
                        if (splitText[1].Length >= 1)
                        {
                            try
                            {
                                int.Parse(splitText[1][0].ToString());
                                //FİŞ
                                sonuclar.Rows[i]["BELGE NO"] = splitText[1].ToString();
                                for (int c = 2; c < splitText.Length; c++)
                                {
                                    sonuclar.Rows[i]["ÜNVAN"] += splitText[c];
                                }
                                var check = sonuclar.Rows[i]["BELGE NO"].ToString();
                                if (sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith(check))
                                {
                                    var chklenght = check.Length;
                                    sonuclar.Rows[i]["ÜNVAN"] = sonuclar.Rows[i]["ÜNVAN"].ToString().Remove(0, chklenght + 1);
                                }
                            }
                            catch (Exception)
                            {
                                if (splitText[1].Length != 1)
                                {
                                    try
                                    {
                                        int.Parse(splitText[1][1].ToString());
                                        sonuclar.Rows[i]["BELGE SERİ"] = splitText[1][0];
                                        for (int c = 2; c < splitText.Length; c++)
                                        {
                                            sonuclar.Rows[i]["ÜNVAN"] += splitText[c];
                                        }
                                    }
                                    catch (Exception)
                                    {

                                        sonuclar.Rows[i]["BELGE SERİ"] = splitText[1][0].ToString() + splitText[1][1].ToString();
                                        for (int j = 2; j < splitText[1].Length; j++)
                                        {
                                            sonuclar.Rows[i]["BELGE NO"] += splitText[1][j].ToString();

                                        }
                                        for (int c = 2; c < splitText.Length; c++)
                                        {
                                            sonuclar.Rows[i]["ÜNVAN"] += splitText[c];
                                        }
                                    }
                                }
                                else
                                {
                                    sonuclar.Rows[i]["BELGE SERİ"] = splitText[1][0].ToString();
                                    sonuclar.Rows[i]["BELGE NO"] = splitText[2].ToString();
                                    for (int c = 3; c < splitText.Length; c++)
                                    {
                                        sonuclar.Rows[i]["ÜNVAN"] = splitText[c];
                                    }
                                }
                                if (splitText[1].Length == 2)
                                {
                                    sonuclar.Rows[i]["BELGE NO"] = splitText[2].ToString();
                                    for (int c = 3; c < splitText.Length; c++)
                                    {
                                        sonuclar.Rows[i]["ÜNVAN"] += splitText[c];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //E FATURA
                        sonuclar.Rows[i]["BELGE NO"] = splitText[1].ToString();
                        for (int c = 2; c < splitText.Length; c++)
                        {
                            sonuclar.Rows[i]["ÜNVAN"] += splitText[c];
                        }
                    }
                }
                catch (Exception sa)
                {
                    //MessageBox.Show(sa.ToString());
                }

            }


            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().EndsWith("MAHSUBU") || sonuclar.Rows[i]["AÇIKLAMA"].ToString().EndsWith("TAHAKKUKU") || sonuclar.Rows[i]["AÇIKLAMA"].ToString().EndsWith("TAH."))
                {

                    sonuclar.Rows[i]["BELGE SERİ"] = "";
                    sonuclar.Rows[i]["BELGE NO"] = "";
                    sonuclar.Rows[i]["ÜNVAN"] = "";
                }

            }
            sonuclar.AcceptChanges();
            return sonuclar;
        }
        //public int SplitDescription(System.Data.DataRow row)
        //{
        //    row[5].ToString();
        //    return 5;
        //}
    }
}