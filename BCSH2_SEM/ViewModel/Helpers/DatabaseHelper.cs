using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace BCSH2_SEM.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        public static string dbFile = Path.Combine(Environment.CurrentDirectory, "BCSH2_SEM.db3");

        /// <summary>
        /// Inserts an item into the database.
        /// </summary>
        /// <typeparam name="T">The type of the item to insert.</typeparam>
        /// <param name="item">The item to insert.</param>
        /// <returns>True if the insertion was successful; otherwise, false.</returns>
        public static bool Insert<T>(T item)
        {
            try
            {
                using (var conn = new SQLiteConnection(dbFile))
                {
                    conn.CreateTable<T>();
                    var rowsAdded = conn.Insert(item);
                    return rowsAdded > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing item in the database.
        /// </summary>
        /// <typeparam name="T">The type of the item to update.</typeparam>
        /// <param name="item">The item to update.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public static bool Update<T>(T item)
        {
            try
            {
                using (var conn = new SQLiteConnection(dbFile))
                {
                    conn.CreateTable<T>();
                    var rowsUpdated = conn.Update(item);
                    return rowsUpdated > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <typeparam name="T">The type of the item to delete.</typeparam>
        /// <param name="item">The item to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public static bool Delete<T>(T item)
        {
            try
            {
                using (var conn = new SQLiteConnection(dbFile))
                {
                    conn.CreateTable<T>();
                    var rowsDeleted = conn.Delete(item);
                    return rowsDeleted > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Reads all items of type T from the database.
        /// </summary>
        /// <typeparam name="T">The type of the items to read.</typeparam>
        /// <returns>A list of items read from the database.</returns>
        public static List<T> Read<T>() where T : new()
        {
            try
            {
                using (var conn = new SQLiteConnection(dbFile))
                {
                    conn.CreateTable<T>();
                    return conn.Table<T>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data: {ex.Message}");
                return new List<T>();
            }
        }
    }
}
