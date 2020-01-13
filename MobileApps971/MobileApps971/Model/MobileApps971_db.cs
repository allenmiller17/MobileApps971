using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApps971.Model
{
    public interface IMobileApps971_db
    {
        SQLiteAsyncConnection GetConnection();
    }
}
