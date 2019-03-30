using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace tspAuto.Domain
{
    public static class MethodPack
    {
        public static SQLiteCommand Generate_Insert_Command(string table, string[] columns, object[] values, SQLiteConnection con)
        {
            string insertString = $"INSERT INTO {table}(";

            foreach (string i in columns)
            {
                insertString += i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ") VALUES(";

            foreach (string i in columns)
            {
                insertString += "@" + i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ");";

            SQLiteCommand command = new SQLiteCommand(insertString, con);

            for (int i = 0; i < columns.Length; i++)
            {
                command.Parameters.AddWithValue(columns[i], values[i]);
            }

            return command;
        }

        public static SQLiteCommand Generate_Query_Command(string word, string table, string[] columns, SQLiteConnection con)
        {
            if (new Regex("[ğĞüÜşŞıİöÖçÇ]").Match(word).Success)
            {
                // (?i) ignores case sensitivity
                string arama = word;

                arama = new Regex("[ğĞ]").Replace(arama, "[ğĞ]");
                arama = new Regex("[üÜ]").Replace(arama, "[üÜ]");
                arama = new Regex("[şŞ]").Replace(arama, "[şŞ]");
                arama = new Regex("[ıI]").Replace(arama, "[ıI]");
                arama = new Regex("[iİ]").Replace(arama, "[iİ]");
                arama = new Regex("[öÖ]").Replace(arama, "[öÖ]");
                arama = new Regex("[çÇ]").Replace(arama, "[çÇ]");

                arama = "(?i)" + arama;

                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} REGEXP @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", arama);

                return command;
            }
            else
            {
                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} LIKE @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", "%" + word + "%");

                return command;
            }
        }

        public delegate void CodeBlock(SQLiteConnection con);

        public static void VeritabaniKodBlogu(CodeBlock codeBlock)
        {
            try
            {
                if (Properties.Settings.Default.DatabaseFilePath != "" && System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                        {
                            //code goes here
                            codeBlock.Invoke(con);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu.\n\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                }
                else if (Properties.Settings.Default.DatabaseFilePath == "")
                {
                    System.Windows.MessageBox.Show("Veritabanı seçilmemiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                    return;
                }
                else if (!System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    System.Windows.MessageBox.Show("Veritabanı silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                    return;
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Windows.MessageBox.Show("Bazı dosyalar silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                return;
            }
        }
    }
}
