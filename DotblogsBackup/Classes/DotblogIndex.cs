using System.Text.RegularExpressions;

namespace DotblogsBackup.Classes
{
    public class DotblogIndex
    {
        public string title { get; set; }
        public string url { get; set; }
        public MatchCollection pictures { get; set; }

        public DotblogIndex() { }

        public DotblogIndex(string title, string url, MatchCollection pictures = null)
        {
            this.title = title;
            this.url = url;
            this.pictures = pictures;
        }
    }
}
