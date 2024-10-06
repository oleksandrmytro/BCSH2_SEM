using System.IO;
using SQLite;

namespace BCSH2_SEM.ViewModel.Helpers;

public class DatabaseHelper
{
    private static string dbFile = Path.Combine(Environment.CurrentDirectory, "BCSH2_SEM.db3");

    public static bool Insert<T>(T item)
    {
        var result = false;

        using (var conn = new SQLiteConnection(dbFile))
        {
            conn.CreateTable<T>();
            var rowsAdded = conn.Insert(item);
            if (rowsAdded > 0) result = true;
        }
        return result;
    }

    public static bool Update<T>(T item)
    {
        var result = false;

        using (var conn = new SQLiteConnection(dbFile))
        {
            conn.CreateTable<T>();
            var rowsUpdated = conn.Update(item);
            if (rowsUpdated > 0) result = true;
        }
        return result;
    }

    public static bool Delete<T>(T item)
    {
        var result = false;

        using (var conn = new SQLiteConnection(dbFile))
        {
            conn.CreateTable<T>();
            var rowsDeleted = conn.Delete(item);
            if (rowsDeleted > 0) result = true;
        }

        return result;
    }

    public static List<T> Read<T>() where T : new()
    {
        using (var conn = new SQLiteConnection(dbFile))
        {
            conn.CreateTable<T>();
            return conn.Table<T>().ToList();
        }
    }
}