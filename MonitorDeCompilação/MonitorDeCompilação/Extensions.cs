using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.ComponentModel;

namespace MonitorDeCompilação
{

    #region Enums

    public enum ToSqlValueStringOption
    {
        None = 0,
        EmptyToNull = 1,
        EmptyOrWhiteSpacesToNull = 2
    }

    #endregion Enums


    public static class Extensions
    {
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> @this)
        {
            BindingList<T> lista = new BindingList<T>();

            foreach (T item in @this)
            {
                lista.Add(item);
            }

            return lista;
        }

        public static BindingList<T> ToBindingList<T>(this List<T> @this)
        {
            BindingList<T> lista = new BindingList<T>();

            foreach (T item in @this)
            {
                lista.Add(item);
            }

            return lista;
        }

        public static BindingList<T> ToBindingList<T>(this IList<T> @this)
        {
            BindingList<T> lista = new BindingList<T>();

            foreach (T item in @this)
            {
                lista.Add(item);
            }

            return lista;
        }

        public static string ToSqlValue(this string @this, ToSqlValueStringOption option = ToSqlValueStringOption.None)
        {
            if (option == ToSqlValueStringOption.EmptyToNull)
            {
                if (@this == string.Empty)
                {
                    return "Null";
                }
                else
                {
                    return "'" + @this + "'";
                }
            }
            if (option == ToSqlValueStringOption.EmptyOrWhiteSpacesToNull)
            {
                if (@this.Trim() == string.Empty)
                {
                    return "Null";
                }
                else
                {
                    return "'" + @this + "'";
                }
            }
            else
            {
                return "'" + @this + "'";
            }
        }

        public static string ToSqlValue(this bool @this)
        {
            return (@this ? "1" : "0");
        }

        public static string ToSqlValue(this int @this)
        {
            return @this.ToString();
        }

        public static string ToSqlValue(this decimal @this)
        {
            return @this.ToString().Replace(",", ".");
        }

        public static string JoinText(this IEnumerable<char> collection)
        {
            StringBuilder builder = new StringBuilder("");

            foreach(char chr in collection )
            {
                builder.Append(chr);
            }

            return builder.ToString();
        }
    }

}
