using DotblogsBackup.Classes;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DotblogsBackup.BackupTools
{
    public class BackupArticle : Backup
    {
        public override void ClearOld()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory()))
            {
                string[] picList = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.html");
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    foreach (var item in picList)
                    {
                        File.Delete(item);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        public override void Save()
        {
            Console.WriteLine("總共 " + allArticle.Count + " 篇文章");
            int count = 0;
            foreach (DotblogIndex pageCollection in allArticle)
            {
                Boolean errorResult = false;
                do
                {
                    try
                    {
                        Console.WriteLine(string.Format("{0:0000}", count + 1) + " 正在處理：" + pageCollection.title + " : " + pageCollection.url);
                        GetData(count + 1, pageCollection);
                        Thread.Sleep(500);
                        errorResult = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        errorResult = true;
                    }
                } while (errorResult);
                count++;
            }
        }

        private void GetData(int count, DotblogIndex pageCollection)
        {
            //清洗頁面
            string sSave = PurgePage(Tools.BrowserRequest(pageCollection.url));
            string PurgePage(string sUrl)
            {
                //清洗 head tag
                sUrl = Regex.Replace(sUrl, "<head prefix.*</head>", Properties.Resources.head, RegexOptions.Singleline);

                //套用本機 theme-light.css
                sUrl = Regex.Replace(sUrl, @"<link href=""https://az788688.vo.msecnd.net/assets/css/theme-light.css.*"" rel=""stylesheet"">", @"<link href=""theme-light.css"" rel=""stylesheet"">", RegexOptions.Singleline);

                //清洗 footer 後面的 scripts
                Match matchFooterScripts = Regex.Match(sUrl, "</footer>(.*)</body>", RegexOptions.Singleline);
                sUrl = sUrl.Replace(matchFooterScripts.Groups[1].Value, "\n" + Properties.Resources.footer + "\n");

                //清除 sidebar 單元
                Match clearSidebar = Regex.Match(sUrl, @"(<div class=""sidebar"">.+)<input id=""fnBlogDetail""", RegexOptions.Singleline);
                sUrl = sUrl.Replace(clearSidebar.Groups[1].Value, "");
                return sUrl;
            }

            StreamWriter aWrite = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + string.Format("{0:0000}", count) + "_" + pageCollection.title + ".html", false, Encoding.UTF8);
            aWrite.Write(sSave);
            aWrite.Close();
        }
    }
}
