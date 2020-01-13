using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileApps971.Model;
using Xamarin.Forms;
using SQLite;
using System.IO;
using MobileApps971.Droid;
using MobileApps971.Droid.Model;

[assembly: Dependency(typeof(MobileApps971_db))]
namespace MobileApps971.Droid.Model
{
    public class MobileApps971_db : IMobileApps971_db
    {
        public SQLiteAsyncConnection GetConnection()
        {
            string dbName = "MobileApps_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dbName);

            return new SQLiteAsyncConnection(fullPath);
        }
    }
}