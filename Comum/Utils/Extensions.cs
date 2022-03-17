using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.ComponentModel;

using Microsoft.Win32;

using Comum.Models;

namespace Comum.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Obtém uma quantidade de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        /// <returns>Retorna sequência de caracteres indicada.</returns>
        public static string TakeLeft(this string valor, int corte)
        {
            int idx = 0;
            StringBuilder leftPart = new StringBuilder("");

            for (; idx < valor.Count() && idx < corte; idx++)
            {
                leftPart.Append(valor[idx]);
            }

            return leftPart.ToString();
        }

        /// <summary>
        /// Obtém uma quantidade de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        /// <returns>Retorna sequência de caracteres indicada.</returns>
        public static string TakeRight(this string valor, int corte)
        {
            int idx = valor.Count() - corte;
            idx = (idx < 0 ? 0 : idx);

            StringBuilder rightPart = new StringBuilder("");

            for (; idx < valor.Count(); idx++)
            {
                rightPart.Append(valor[idx]);
            }

            return rightPart.ToString();
        }

        /// <summary>
        /// Ignora uma quantidade de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipRight(this string valor, int corte)
        {
            StringBuilder builder = new StringBuilder("");

            for (int idx = 0; idx < valor.Length - corte; idx++)
            {
                builder.Append(valor[idx]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ignora uma quantidade de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipLeft(this string valor, int corte)
        {
            StringBuilder builder = new StringBuilder("");

            for (int idx = corte; idx < valor.Length; idx++)
            {
                builder.Append(valor[idx]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ignora uma sequência de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipLeft(this string valor, string corte)
        {
            if (valor.Count() < corte.Count())
            {
                return valor;
            }

            StringBuilder builder = new StringBuilder("");
            int idx = 0;

            // Verifica se a substring [corte] é válida e, caso seja válida, posiciona [idx] após o tamanho de [corte];
            for (; idx < corte.Length; idx++)
            {
                // se não achou substring dentro de [valor], sai da função e retorna o próprio [valor];
                if (valor[idx] != corte[idx])
                {
                    return valor;   // substring [corte] não existe dentro de [valor];
                }
            }

            // Pega [valor] a partir do final do valor de corte;
            for (; idx < valor.Length; idx++)
            {
                builder.Append(valor[idx]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ignora uma sequência de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipLeft(this string valor, char corte)
        {
            return SkipLeft(valor, corte.ToString());
        }

        /// <summary>
        /// Ignora uma sequência de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipRight(this string valor, string corte)
        {
            if (valor.Count() < corte.Count())
            {
                return valor;
            }

            StringBuilder builder = new StringBuilder("");
            int idx = corte.Length - 1;
            int idy = valor.Length - 1;


            // Verifica se a substring [corte] é válida e, caso seja válida, posiciona [idx] após o tamanho de [corte];
            for (; idx > -1; idx--, idy--)
            {
                // se não achou substring dentro de [valor], sai da função e retorna o próprio [valor];
                if (corte[idx] != valor[idy])
                {
                    return valor;   // substring [corte] não existe dentro de [valor];
                }
            }

            int leftPartCount = idy + 1;

            // Pega [valor] a partir do final do valor de corte;
            for (idy = 0; idy < leftPartCount; idy++)
            {
                builder.Append(valor[idy]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ignora uma sequência de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipRight(this string valor, char corte)
        {
            return SkipRight(valor, corte.ToString());
        }

        public static int ToInt(this string value)
        {
            int parsed = 0;

            if (int.TryParse(value, out parsed))
            {
                return parsed;
            }

            return 0;
        }

        public static void AddRange<T>(this System.ComponentModel.BindingList<T> @this, System.Collections.Generic.IEnumerable<T> enumerable)
        {
            foreach (T item in enumerable)
            {
                @this.Add(item);
            }
        }



        /*
        public static FileInfo ZipCompress(this FileInfo fileToCompress, string toZipFileFullPath)
        {
            return ZipHelper.CompressFiles(toZipFileFullPath, fileToCompress);
        }
        */

        /*
        public static FileInfo Compress(this FileInfo fileToCompress, string destFileNamePath)
        {

            

            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                

                using (FileStream compressedFileStream = File.Create(destFileNamePath))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Fastest, false ))
                    {
                        
                        originalFileStream.CopyTo(compressionStream);
                        //Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                        //    fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                    }
                }
            }

            if (!File.Exists(destFileNamePath))
            {
                throw new Exception("Não foi possível compactar o arquivo '" + destFileNamePath + "'");
            }

            return new FileInfo(destFileNamePath);
        }

        public static FileInfo Compress(this FileInfo fileToCompress, DirectoryInfo destDirectory)
        {
            string fileNameWithoutExtension = fileToCompress.Name.SkipRight(fileToCompress.Extension.Length) ;
            string gzFileName = fileNameWithoutExtension + ".gz";
            string fullFilePath = destDirectory.FullName + @"\" + gzFileName;

            FileInfo newFile = fileToCompress.Compress(fullFilePath);

            return newFile;
        }
        */


        public static string FormatTo(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// Eliminate all the items of the Queue.
        /// </summary>
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item = default(T);

            while (queue.TryDequeue(out item))
            {
                // feito apenas para eliminar os itens da fila;
            }
        }

        public static DiaDaSemana ToDiaDaSemana(this DayOfWeek week)
        {
            return (DiaDaSemana)(week.GetHashCode() + 1);
        }

        public static DayOfWeek ToDayOfWeek(this  DiaDaSemana semana)
        {

            return (DayOfWeek)(semana.GetHashCode() - 1);
        }

        public static bool Contains(this RegistryKey rk, string valueName)
        {
            return rk.Contains(valueName, false);
        }

        public static bool Contains(this RegistryKey rk, string valueName, bool caseInsensitive)
        {
            if (caseInsensitive)
            {
                foreach (string name in rk.GetValueNames())
                    if (name.ToLower() == valueName.ToLower())
                        return true;
            }
            else
            {
                foreach (string name in rk.GetValueNames())
                    if (name == valueName)
                        return true;
            }

            return false;
        }
    }
}