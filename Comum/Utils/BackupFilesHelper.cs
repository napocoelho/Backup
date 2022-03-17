using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;


namespace Comum.Utils
{
    public static class BackupFilesHelper
    {

        /// <summary>
        /// Cria o nome do arquivo do banco de dados sem a extensão.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="diaHora"></param>
        /// <returns></returns>
        public static string MakeDatabaseBackupName(string databaseName, DateTime diaHora)
        {
            string seg = ("00" + diaHora.Second).TakeRight(2);
            string min = ("00" + diaHora.Minute).TakeRight(2);
            string hrs = ("00" + diaHora.Hour).TakeRight(2);

            string dia = ("00" + diaHora.Day).TakeRight(2);
            string mês = ("00" + diaHora.Month).TakeRight(2);
            string ano = diaHora.Year.ToString();

            return ("DB.{0}".FormatTo(databaseName) +
                    ".D{0}_{1}_{2}".FormatTo(ano, mês, dia) +
                    ".T{0}_{1}_{2}".FormatTo(hrs, min, seg) +
                    ".W{0}".FormatTo(diaHora.DayOfWeek.ToDiaDaSemana().GetHashCode())).ToUpper();
        }

        public static string MakeZipFileBackupName(string prefixName, DateTime diaHora)
        {
            string seg = ("00" + diaHora.Second).TakeRight(2);
            string min = ("00" + diaHora.Minute).TakeRight(2);
            string hrs = ("00" + diaHora.Hour).TakeRight(2);

            string dia = ("00" + diaHora.Day).TakeRight(2);
            string mês = ("00" + diaHora.Month).TakeRight(2);
            string ano = diaHora.Year.ToString();

            return ("{0}".FormatTo(prefixName) +
                    ".D{0}_{1}_{2}".FormatTo(ano, mês, dia) +
                    ".T{0}_{1}_{2}".FormatTo(hrs, min, seg)).ToUpper() + ".zip";
        }

        public static FileInfo CompressDatabaseBackupFile(FileInfo bakFileInfo, DirectoryInfo destDir)
        {
            if (bakFileInfo.Extension.ToLower() != ".bak")
            {
                throw new FileLoadException("Arquivos de backup devem possuir extensão .bak!");
            }

            string zipFileName = destDir + @"\" + Path.GetFileNameWithoutExtension(bakFileInfo.Name) + ".zip";

            return ZipHelper.CompressFiles(zipFileName, bakFileInfo);
        }

        public static DatabaseBackupFileInfo GetDatabaseFileInfo(FileInfo bakOrZipFile)
        {
            return new DatabaseBackupFileInfo(bakOrZipFile);
        }

        public static List<DatabaseBackupFileInfo> GetAllCompressedDatabaseFiles(DirectoryInfo destDir)
        {
            List<DatabaseBackupFileInfo> files = new List<DatabaseBackupFileInfo>();

            foreach (FileInfo file in destDir.GetFiles("*.zip"))
            {
                try
                {
                    DatabaseBackupFileInfo info = new DatabaseBackupFileInfo(file);
                    files.Add(info);
                }
                catch (Exception ex)
                { }
            }

            return files;
        }

        public static List<DatabaseBackupFileInfo> GetAllDatabaseFiles(DirectoryInfo destDir)
        {
            List<DatabaseBackupFileInfo> files = new List<DatabaseBackupFileInfo>();

            foreach (FileInfo file in destDir.GetFiles("*.zip"))
            {
                try
                {
                    DatabaseBackupFileInfo info = new DatabaseBackupFileInfo(file);
                    files.Add(info);
                }
                catch (Exception ex)
                { }
            }

            foreach (FileInfo file in destDir.GetFiles("*.bak"))
            {
                try
                {
                    DatabaseBackupFileInfo info = new DatabaseBackupFileInfo(file);
                    files.Add(info);
                }
                catch (Exception ex)
                { }
            }

            return files;
        }
    }



    public class DatabaseBackupFileInfo
    {
        public FileInfo File { get; private set; }
        public DateTime Time { get; private set; }
        public string Database { get; private set; }
        public DayOfWeek WeekDay { get; private set; }

        public bool IsZipFile { get { return File.Extension.ToLower() == ".zip"; } }

        public DatabaseBackupFileInfo(FileInfo file)
        {
            if (file.Extension.ToLower() != ".zip" && file.Extension.ToLower() != ".bak")
            {
                throw new FileLoadException  ("A extensão do arquivo não é reconhecida!");
            }

            this.File = file;

            // "DB.{0}.D{0}_{1}_{2}.T{0}_{1}_{2}.W{0}"
            string[] nameParts = Path.GetFileNameWithoutExtension(file.Name).Split(new string[] { "." }, StringSplitOptions.None);

            this.Database = nameParts[1].Trim().ToUpper();

            string [] dt = nameParts[2].SkipLeft(1). Split(new string[] { "_" }, StringSplitOptions.None);
            string [] tm = nameParts[3].SkipLeft(1).Split(new string[] { "_" }, StringSplitOptions.None);
            this.Time = new DateTime(dt[0].ToInt(), dt[1].ToInt(), dt[2].ToInt(), tm[0].ToInt(), tm[1].ToInt(), tm[2].ToInt() ) ;

            this.WeekDay = (DayOfWeek)(nameParts[4].SkipLeft(1).ToInt() - 1);
        }
    }
}