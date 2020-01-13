using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.IO;
using SQLite;
using Xamarin.Forms;
using MobileApps971.iOS;
using MobileApps971.Model;
using MobileApps971.iOS.Model;

[assembly: Dependency(typeof(MobileApps971_db))]
namespace MobileApps971.iOS.Model
{
    class MobileApps971_db : IMobileApps971_db
    {

        public SQLiteAsyncConnection GetConnection()
        {
            string dbName = "MobileApps_db.sqlite";
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string fullPath = Path.Combine(folderPath, dbName);

            return new SQLiteAsyncConnection(fullPath);
        }


    }
}