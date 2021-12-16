using DotblogsBackup.BackupTools;
using DotblogsBackup.Classes;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DotblogsBackup
{
    public static class Tools
    {
        public static string BrowserRequest(string url)
        {
            HttpWebRequest myDefaultReq = HttpWebRequest.CreateHttp(url);
            WebResponse myRes = myDefaultReq.GetResponse();
            StreamReader aReader = new StreamReader(myRes.GetResponseStream(), Encoding.UTF8);
            String result = aReader.ReadToEnd();
            aReader.Close();

            return result;
        }

        public static void TestBackupOnePage()
        {
            string sTitle = "select指令-ROUND、CEILING、FLOOR、SQRT、SQUARE、CAST、CONVERT、GETDATE、DATEPART、DATEADD、DATEDIFF、UPPER、LOWER、LEN、DATALENGTH、CHARINDEX、LEFT、RIGHT、SUBSTRING、REPLICATE、REPLACE";
            string sUrl = "https://dotblogs.com.tw/brooke/2016/06/03/080230";

            DotblogIndex aPageClass = new DotblogIndex(sTitle, sUrl);
            Backup.allArticle.Add(aPageClass);

            BackupArticle BackupArticle = new BackupArticle();
            BackupArticle.Save();
            Console.WriteLine("TestBackupOnePage finish");
        }

        public static void TestBackupOnePagePictures()
        {
            string sTitle = "select指令-ROUND、CEILING、FLOOR、SQRT、SQUARE、CAST、CONVERT、GETDATE、DATEPART、DATEADD、DATEDIFF、UPPER、LOWER、LEN、DATALENGTH、CHARINDEX、LEFT、RIGHT、SUBSTRING、REPLICATE、REPLACE";
            string sUrl = "https://dotblogs.com.tw/brooke/2016/06/03/080230";

            DotblogIndex aPageClass = new DotblogIndex(sTitle, sUrl);
            Backup.allArticle.Add(aPageClass);

            BackupPicture BackupPicture = new BackupPicture();
            BackupPicture.GetPictureList();
            BackupPicture.Save();
            Console.WriteLine("TestBackupOnePagePictures finish");
        }

        public static void ChangePictureUrl()
        {
            //1、掃描 images 資料夾個數
            //EX：.\images\0001_Regex.IsMatch Method
            string[] picDirPaths = Directory.GetDirectories(@".\images");

            //2、掃描 images 資料夾底下檔案個數
            string[][] picFileNamePaths = new string[picDirPaths.Length][];
            for (int i = 0; i < picDirPaths.Length; i++)
            {
                //EX：.\images\0001_Regex.IsMatch Method\1572417365_367.png     
                string[] files = Directory.GetFiles(picDirPaths[i]);
                picFileNamePaths[i] = files;
            }
            for (int i = 0; i < picFileNamePaths.Length; i++)
            {
                for (int j = 0; j < picFileNamePaths[i].Length; j++)
                {
                    picFileNamePaths[i][j] = Regex.Replace(picFileNamePaths[i][j], @"\.\\images\\\d{4}_.+\\", "");
                }
            }

            //3、取得 images 底下資料夾名稱
            //EX：0001_Regex.IsMatch Method
            string[] dirNames = new string[picDirPaths.Length];
            for (int i = 0; i < dirNames.Length; i++)
            {
                dirNames[i] = Path.GetFileName(picDirPaths[i]);
            }

            //4、取得 .html 檔案路徑
            //EX：.\0001_Regex.IsMatch Method.html
            string[] htmlFilePaths = Directory.GetFiles(@".\", "*.html");

            //5、取得 .html 檔案名稱
            //EX：0001_Regex.IsMatch Method
            string[] htmlFileNames = new string[htmlFilePaths.Length];
            for (int i = 0; i < htmlFileNames.Length; i++)
            {
                htmlFileNames[i] = Regex.Match(htmlFilePaths[i], @"\.\\(.*)\.html").Groups[1].Value;
            }

            //TODO:各陣列內容順序務必同步，不可錯亂
            //TODO:在儲存圖片時，需注意同資料夾是否存在相同檔名(可能程式會出錯)
            //6、針對每一個 .html 檔案名稱，將所對應名稱圖片資料夾底下的所有圖片檔，改路徑回 .html 檔案          
            for (int i = 0; i < htmlFileNames.Length; i++)
            {
                //取得 .html 檔案內容
                StreamReader sr = new StreamReader(htmlFilePaths[i], Encoding.Default);
                string content = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                //將所對應名稱圖片資料夾底下的所有圖片檔，改路徑回 .html 檔案
                for (int j = 0; j < picFileNamePaths[i].Length; j++)
                {
                    content = Regex.Replace(content, @"https://az787680\.vo\.msecnd\.net/user/.*" + picFileNamePaths[i][j], @"./images/" + htmlFileNames[i] + "/" + picFileNamePaths[i][j]);
                }

                //.html 存檔
                StreamWriter sw = new StreamWriter(htmlFilePaths[i], false, Encoding.Default);
                sw.Write(content);
                sw.Close();
                sw.Dispose();
            }
        }
    }
}
