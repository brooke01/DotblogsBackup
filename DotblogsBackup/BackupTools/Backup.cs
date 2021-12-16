using DotblogsBackup.Classes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace DotblogsBackup.BackupTools
{
    public abstract class Backup
    {
        static public List<DotblogIndex> allArticle = new List<DotblogIndex>();
        static private string _dotblogs = "https://dotblogs.com.tw";
        static private string _userName = "brooke";

        static public void SetDotblogsUrl(string dotblogs, string userName)
        {
            _dotblogs = dotblogs;
            _userName = userName;
        }

        private string GetDotblogsUrl()
        {
            return _dotblogs + "/" + _userName + "/";
        }

        abstract public void ClearOld();

        virtual public void GetPageList()
        {
            if (allArticle.Count != 0) { return; }

            int PagedListCount = 1;
            Boolean isNext;
            try
            {
                do
                {
                    String sPageContent = Tools.BrowserRequest(GetDotblogsUrl() + PagedListCount.ToString());

                    SaveToAllArticle();
                    void SaveToAllArticle()
                    {
                        MatchCollection articleTitles = Regex.Matches(sPageContent, @"<h3 class=\""article__title\"">\r\n\s+(<.+?)\r\n\s+</h3>", RegexOptions.Singleline);
                        foreach (Match mArticle in articleTitles)
                        {
                            string ahref = mArticle.Groups[1].ToString();
                            string sUrl = _dotblogs + Regex.Match(ahref, "<a href=\"(.+?)\">.+?</a>").Groups[1].Value;
                            string sTitle = Regex.Match(ahref, "<a href=\".+?\">(.+?)</a>").Groups[1].Value;

                            //HtmlDecode
                            sTitle = HttpUtility.HtmlDecode(sTitle);

                            //檔案命名規則 \ / ? : * " > < |                        
                            sTitle = Regex.Replace(sTitle, @"\\|/|\?|:|\*|""|>|<|\|", "_");

                            DotblogIndex aPageClass = new DotblogIndex(sTitle, sUrl);
                            allArticle.Add(aPageClass);
                        }
                    }

                    isNext = Regex.IsMatch(sPageContent, "PagedList-skipToNext");

                    if (isNext == true)
                    {
                        Console.WriteLine("掃描 PagedListCount:" + PagedListCount);
                        PagedListCount++;
                    }
                    else
                    {
                        PagedListCount = -1;
                    }

                    Thread.Sleep(100);

                } while (isNext);//判定是否還有下一頁
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        virtual public void GetPictureList() { }

        abstract public void Save();
    }
}
