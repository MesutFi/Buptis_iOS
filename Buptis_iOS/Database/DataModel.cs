﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using UIKit;

namespace Buptis_iOS.Database
{
    class DataModel
    {
    }
    public class MEMBER_DATA
    {
        [PrimaryKey, AutoIncrement]
        public bool activated { get; set; }
       // public List<string> authorities { get; set; }
        public DateTime? birthDayDate { get; set; }
        public int? boost { get; set; }
        public DateTime? boostTime { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string gold { get; set; }
        public int id { get; set; }
        public string imageUrl { get; set; }
        public string langKey { get; set; }
        public string lastModifiedBy { get; set; }
        public string lastModifiedDate { get; set; }
        public string lastName { get; set; }
        public string login { get; set; }
        public int? messageCount { get; set; }
        public int? superBoost { get; set; }
        public DateTime? superBoostTime { get; set; }
        public string userJob { get; set; }
        //------------------------------------
        public string townId { get; set; }
        public string API_TOKEN { get; set; }
        public string password { get; set; }
    }

    public class BILDIRIM
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string BildirimID { get; set; }
        public bool isRead { get; set; }
    }

    public class FILTRELER
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int Cinsiyet { get; set; }
        public int minAge { get; set; }
        public int maxAge { get; set; }
    }
    public class CHAT_KEYS
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public string MessageKey { get; set; }

    }

}