using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
//using System.IO.Compression;
using Ionic.Zip;

namespace Comum.Utils
{
    public static class ZipHelper
    {
        public static FileInfo CompressFiles(string toZipFileFullPath, params FileInfo[] fileEntries)
        {
            string fullNameDirName = string.Empty;

            using (ZipFile zip = new ZipFile(toZipFileFullPath))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Level5;
                zip.UseZip64WhenSaving = Zip64Option.Always;
                zip.TempFileFolder = Path.GetTempPath();

                //zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                //zip.Password = "panacea";

                foreach (FileInfo entry in fileEntries)
                {
                    zip.AddFile(entry.FullName, "");
                }

                zip.Save();
            }

            return new FileInfo(toZipFileFullPath);
        }



        public static void CompressDirectory(DirectoryInfo fromDirectory, string toZipFileFullPath, CompressionLevel compressionLevel = CompressionLevel.BestSpeed)
        {
            string nonRelativePathPart = string.Empty;

            DirectoryInfo parent = fromDirectory.Parent;

            if (parent != null)
            {
                nonRelativePathPart = parent.FullName + Path.DirectorySeparatorChar;
            }

            string entryName = fromDirectory.FullName.SkipLeft(nonRelativePathPart).SkipRight(Path.DirectorySeparatorChar);


            ZipFile zip = File.Exists(toZipFileFullPath) ? ZipFile.Read(toZipFileFullPath) : new ZipFile(toZipFileFullPath);

            try
            {   
                zip.CompressionLevel = TranslateCompression(compressionLevel);
                zip.UseZip64WhenSaving = Zip64Option.Always;
                zip.TempFileFolder = Path.GetTempPath();

                //zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                //zip.Password = "panacea";

                zip.AddDirectory(fromDirectory.FullName, entryName);

                zip.Save();
            }
            finally
            {
                zip.Dispose();
            }
        }
        
        public static void CompressDirectory(DirectoryInfo fromDirectory, string toZipFileFullPath, List<string> searchPatternExpression,
                                             CompressionLevel compressionLevel = CompressionLevel.BestSpeed )
        {
            List<EntryInfo> allEntries = new List<EntryInfo>();
            allEntries.AddRange(MakeEntriesRecursive(fromDirectory, toZipFileFullPath));


            ZipFile zip = File.Exists(toZipFileFullPath) ? ZipFile.Read(toZipFileFullPath) : new ZipFile(toZipFileFullPath);

            try
            {
                zip.CompressionLevel = TranslateCompression(compressionLevel);
                zip.UseZip64WhenSaving = Zip64Option.Always;
                zip.TempFileFolder = Path.GetTempPath();

                //zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                //zip.Password = "panacea";

                foreach (EntryInfo entry in allEntries)
                {
                    if (entry.EntryType == EntryType.Directory)
                    {
                        try
                        {
                            if (!zip.ContainsEntry(entry.EntryName))
                            {
                                zip.AddDirectoryByName(entry.EntryName);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        try
                        {
                            zip.AddFile(entry.FullName, entry.EntryName);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                zip.Save();
            }
            finally
            {
                zip.Dispose();
            }
        }

        private static List<EntryInfo> MakeEntriesRecursive(DirectoryInfo fromDirectory, string toZipFileFullPath,
                                                                  string nonRelativePathPart = null)
        {
            List<EntryInfo> entries = new List<EntryInfo>();
            
            if (nonRelativePathPart == null)
            {
                DirectoryInfo parent = fromDirectory.Parent;

                if (parent != null)
                {
                    nonRelativePathPart = parent.FullName + Path.DirectorySeparatorChar;
                }
            }

            string entryName = fromDirectory.FullName.SkipLeft(nonRelativePathPart).SkipRight(Path.DirectorySeparatorChar);

            entries.Add(new EntryInfo(fromDirectory.FullName, entryName, EntryType.Directory));

            foreach (FileInfo file in fromDirectory.GetFiles())
            {
                entries.Add(new EntryInfo(file.FullName, entryName, EntryType.File));
            }

            foreach (DirectoryInfo dir in fromDirectory.GetDirectories())
            {
                //entries.Add(new ZipEntry(dir.FullName, dir.FullName.SkipLeft(nonRelativePathPart), EntryType.Directory));
                entries.AddRange(MakeEntriesRecursive(dir, toZipFileFullPath, nonRelativePathPart));
            }

            return entries;
        }

        private static string FormatEntryName(string pathName, EntryType entryType, string nonRelativePathPart = "")
        {
            string retorno = string.Empty;

            if (entryType == EntryType.File)
            {
                retorno = pathName.SkipLeft(nonRelativePathPart);
            }
            else
            {
                retorno = pathName.SkipLeft(nonRelativePathPart).SkipRight(@"\");
            }

            return retorno;
        }

        private static Ionic.Zlib.CompressionLevel TranslateCompression(Comum.Utils.CompressionLevel compressionLevel)
        {
            Ionic.Zlib.CompressionLevel retorno;

            if (compressionLevel == CompressionLevel.None)
            {
                retorno = Ionic.Zlib.CompressionLevel.None;
            }
            else if (compressionLevel == CompressionLevel.BestSpeed)
            {
                retorno = Ionic.Zlib.CompressionLevel.BestSpeed;
            }
            else if (compressionLevel == CompressionLevel.BestCompression)
            {
                retorno = Ionic.Zlib.CompressionLevel.BestCompression;
            }
            else if (compressionLevel == CompressionLevel.Normal)
            {
                retorno = Ionic.Zlib.CompressionLevel.Level5;
            }
            else
            {
                retorno = Ionic.Zlib.CompressionLevel.Level5;
            }

            return retorno;
        }
    }

    
    public class EntryInfo
    {
        /// <summary>
        /// Gets the full path of the directory or file to be archived.
        /// 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The name of the entry to create in the zip archive.
        /// </summary>
        public string EntryName { get; set; }

        /// <summary>
        /// The compression level.
        /// </summary>
        //public CompressionLevel Compression { get; set; }

        public EntryType EntryType { get; set; }

        public EntryInfo()
        {
            this.FullName = string.Empty;
            this.EntryName = string.Empty;
            //this.Compression = CompressionLevel.Optimal;
        }

        public EntryInfo(string fullName, string entryName, EntryType type)
            : base()
        {
            this.FullName = fullName;
            this.EntryName = entryName;
            //this.Compression = compression;
            this.EntryType = type;
        }

        
    }

    public enum EntryType
    {
        Directory = 1,
        File = 2        
    }

    public enum CompressionLevel
    {
        Normal = 0,
        BestSpeed = 1,        
        BestCompression = 2,
        None = 3
    }
}