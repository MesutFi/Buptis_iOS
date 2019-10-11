using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using UIKit;

namespace Buptis_iOS.Database
{
    class DataBase
    {
        public DataBase()
        {
            CreateDataBase();
        }
        public static string documentsFolder()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(path);
            return path;
        }
        public static void CreateDataBase()
        {
            var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"),  false);
            //Method not found: string SQLitePCL.raw.sqlite3_column_name(SQLitePCL.sqlite3_stmt,int)
            //Unable to uninstall 'SQLitePCLRaw.core.2.0.1' because
            //'SQLitePCLRaw.provider.dynamic_cdecl.2.0.1, SQLitePCLRaw.provider.sqlite3.ios_unified.1.1.14'
            //depend on it.
            conn.CreateTable<MEMBER_DATA>();
            conn.CreateTable<FILTRELER>();
            conn.CreateTable<CHAT_KEYS>();
            conn.Close();
        }

        #region MEMBER_DATA
        public static bool MEMBER_DATA_EKLE(MEMBER_DATA GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
                return false;
            }
        }
        public static List<MEMBER_DATA> MEMBER_DATA_GETIR()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                var gelenler = conn.Query<MEMBER_DATA>("Select * From MEMBER_DATA");
                conn.Close();
                return gelenler;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
        public static bool MEMBER_DATA_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Query<MEMBER_DATA>("Delete From MEMBER_DATA");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool MEMBER_DATA_Guncelle(MEMBER_DATA Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region FILTRELER
        public static bool FILTRELER_EKLE(FILTRELER GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
                return false;
            }
        }
        public static List<FILTRELER> FILTRELER_GETIR()
        {
            var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
            var gelenler = conn.Query<FILTRELER>("Select * From FILTRELER");
            conn.Close();
            return gelenler;
        }

        public static bool FILTRELER_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Query<FILTRELER>("Delete From FILTRELER");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }
        }

        #endregion

        #region CHAT KEYS
        public static bool CHAT_KEYS_EKLE(CHAT_KEYS GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
                return false;
            }
        }
        public static List<CHAT_KEYS> CHAT_KEYS_GETIR()
        {
            var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
            var gelenler = conn.Query<CHAT_KEYS>("Select * From CHAT_KEYS");
            conn.Close();
            return gelenler;
        }
        public static bool CHAT_KEYS_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Query<CHAT_KEYS>("Delete From CHAT_KEYS");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }
        }
        public static bool CHAT_KEYS_Guncelle(CHAT_KEYS Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Buptis.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion
    }
}