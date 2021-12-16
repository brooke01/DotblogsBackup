using DotblogsBackup.BackupTools;
using System;
using System.IO;
using System.Text;

namespace DotblogsBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ToBackupStyleSheet theme-light.css ...");
            ToBackupStyleSheet();

            Console.WriteLine("ToBackupArticle starting...please waiting A few minutes");
            ToBackupArticle();

            Console.WriteLine("ToBackupPicture starting...please waiting A few minutes");
            ToBackupPicture();

            Console.WriteLine("All finish");
            Console.WriteLine(@"please push ""enter"" button to exit...");
            Console.ReadLine();
        }

        static void ToBackupStyleSheet()
        {
            string sSave = Tools.BrowserRequest("https://az788688.vo.msecnd.net/assets/css/theme-light.css");
            StreamWriter aWrite = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + "theme-light.css", false, Encoding.UTF8);
            aWrite.Write(sSave);
            aWrite.Close();
        }

        static void ToBackupArticle()
        {
            BackupArticle BackupArticle = new BackupArticle();
            BackupArticle.ClearOld();
            BackupArticle.GetPageList();
            BackupArticle.Save();
            Console.WriteLine("BackupArticle finish");
        }

        static void ToBackupPicture()
        {
            BackupPicture BackupPicture = new BackupPicture();
            BackupPicture.ClearOld();
            BackupPicture.GetPageList();
            BackupPicture.GetPictureList();
            BackupPicture.Save();
            Console.WriteLine("BackupPicture finish");
        }
    }
}
