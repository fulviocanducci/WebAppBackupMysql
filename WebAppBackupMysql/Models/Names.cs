using System;

namespace WebAppBackupMysql.Models
{
    public record Names
    {
        public Names(string prefix = null)
        {
            Prefix = prefix;
            Name = (Prefix ?? "") + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
        }

        public string Name { get; init; }
        public string Prefix { get; init; }


        public string GetNameSql()
        {
            return string.Format("pk-{0}{1}", Name, ".sql");
        }
        public string GetNameZip()
        {
            return string.Format("pk-{0}{1}", Name, ".zip");
        }
    }
}
