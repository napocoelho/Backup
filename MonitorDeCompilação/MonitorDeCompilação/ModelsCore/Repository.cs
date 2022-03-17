using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectionManagerDll;
using System.Data;
using System.ComponentModel;
using MonitorDeCompilação.Models;


namespace MonitorDeCompilação.ModelsCore
{
    public static class Repository
    {
        // <Caches
        private static CacheStock Stock = new CacheStock();
        // Caches>


        public static BindingList<TBase> GetCollection<TBase>(Action<DataRow, TBase> mappingFields, string tableName, bool useCache = true) where TBase : AbstractBase, new()
        {
            TypedCache<TBase> cache = Stock.Items<TBase>();

            if (useCache && cache.IsCached)
            {
                return cache.Values;
            }


            DataTable tbl = ConnectionManager.GetConnection.ExecuteDataTable("select * from [" + tableName + "]");
            BindingList<TBase> listaNova = new BindingList<TBase>();

            foreach (DataRow row in tbl.Rows)
            {
                TBase entidade = null;
                int id = int.Parse(row["Id"].ToString());

                entidade = cache.Values.Where(@new => @new.Id == id).FirstOrDefault();

                if (entidade == null)
                {
                    entidade = new TBase();
                    entidade.Id = id;
                    cache.Values.Add(entidade);
                }

                mappingFields(row, entidade);

                //entidade.Id = int.Parse(row["Id"].ToString());
                //entidade.Nome = row["Nome"].ToString();

                listaNova.Add(entidade);
            }

            // remove tudo o que não tiver na consulta atual:
            cache.Values.ToList().ForEach(@old =>
            {
                bool notExists = (listaNova.Where(@new => @new.Id == @old.Id).FirstOrDefault() == null);
                if (notExists)
                    cache.Values.Remove(@old);
            });

            cache.IsCached = true;

            return cache.Values;
        }

        public static BindingList<Sistema> GetSistemas(bool useCache = true)
        {
            Action<DataRow, Sistema> mapping = (row, entidade) =>
            {
                entidade.Nome = row["Nome"].ToString();
            };

            return GetCollection<Sistema>(mapping, "UTL_SISTEMAS", useCache);
        }

        public static BindingList<Desenvolvedor> GetDesenvolvedores(bool useCache = true)
        {
            Action<DataRow, Desenvolvedor> mapping = (row, entidade) =>
            {
                entidade.Nome = row["Nome"].ToString();
                entidade.Email = row["Email"].ToString();
            };

            return GetCollection<Desenvolvedor>(mapping, "UTL_DESENVOLVEDORES", useCache);
        }

        public static BindingList<Bloqueio> GetBloqueios(bool useCache = true)
        {
            Action<DataRow, Bloqueio> mapping = (row, entidade) =>
            {
                entidade.IdDesenvolvedor = int.Parse(row["IdDesenvolvedor"].ToString());
                entidade.IdSistema = int.Parse(row["IdSistema"].ToString());
            };

            return GetCollection<Bloqueio>(mapping, "UTL_BLOQUEIOS", useCache);
        }

        /*
        public static BindingList<Desenvolvedor> xxGetDesenvolvedores(bool useCache = true)
        {
            TypedCache<Desenvolvedor> cache = Stock.Items<Desenvolvedor>();

            if (useCache && cache.IsCached)
            {
                return cache.Values;
            }


            DataTable tbl = ConnectionManager.GetConnection.ExecuteDataTable("select * from UTL_DESENVOLVEDORES");
            BindingList<Desenvolvedor> listaNova = new BindingList<Desenvolvedor>();

            foreach (DataRow row in tbl.Rows)
            {
                Desenvolvedor entidade = null;
                int id = int.Parse(row["Id"].ToString());

                entidade = cache.Values.Where(@new => @new.Id == id).FirstOrDefault();

                if (entidade == null)
                {
                    entidade = new Desenvolvedor();
                    cache.Values.Add(entidade);
                }

                entidade.Id = id;
                entidade.Nome = row["Nome"].ToString();
                entidade.Email = row["Email"].ToString();

                listaNova.Add(entidade);
            }

            // remove tudo o que não tiver na consulta atual:
            cache.Values.ToList().ForEach(@old =>
            {
                bool notExists = (listaNova.Where(@new => @new.Id == @old.Id).FirstOrDefault() == null);
                if (notExists)
                    cache.Values.Remove(@old);
            });


            cache.IsCached = true;

            return cache.Values;
        }
        */

        /*
        public static BindingList<Sistema> xxGetSistemas(bool useCache = true)
        {
            TypedCache<Sistema> cache = Stock.Items<Sistema>();

            if (useCache && cache.IsCached)
            {
                return cache.Values;
            }


            DataTable tbl = ConnectionManager.GetConnection.ExecuteDataTable("select * from UTL_SISTEMAS");
            BindingList<Sistema> listaNova = new BindingList<Sistema>();

            foreach (DataRow row in tbl.Rows)
            {
                Sistema entidade = null;
                int id = int.Parse(row["Id"].ToString());

                entidade = cache.Values.Where(@new => @new.Id == id).FirstOrDefault();

                if (entidade == null)
                {
                    entidade = new Sistema();
                    cache.Values.Add(entidade);
                }

                entidade.Id = int.Parse(row["Id"].ToString());
                entidade.Nome = row["Nome"].ToString();

                listaNova.Add(entidade);
            }

            // remove tudo o que não tiver na consulta atual:
            cache.Values.ToList().ForEach(@old =>
            {
                bool notExists = (listaNova.Where(@new => @new.Id == @old.Id).FirstOrDefault() == null);
                if (notExists)
                    cache.Values.Remove(@old);
            });

            cache.IsCached = true;

            return cache.Values;
        }
        */

        /*
        public static BindingList<Bloqueio> xxGetBloqueios(bool useCache = true)
        {
            TypedCache<Bloqueio> cache = Stock.Items<Bloqueio>();

            if (useCache && cache.IsCached)
            {
                return cache.Values;
            }
            

            DataTable tbl = ConnectionManager.GetConnection.ExecuteDataTable("select * from UTL_BLOQUEIOS");
            BindingList<Bloqueio> listaNova = new BindingList<Bloqueio>();

            foreach (DataRow row in tbl.Rows)
            {
                Bloqueio entidade = null;
                int id = int.Parse(row["Id"].ToString());

                entidade = cache.Values.Where(@new => @new.Id == id).FirstOrDefault();

                if (entidade == null)
                {
                    entidade = new Bloqueio();
                    cache.Values.Add(entidade);
                }

                entidade.Id = int.Parse(row["Id"].ToString());
                entidade.IdDesenvolvedor = int.Parse(row["IdDesenvolvedor"].ToString());
                entidade.IdSistema = int.Parse(row["IdSistema"].ToString());

                listaNova.Add(entidade);
            }

            // remove tudo o que não tiver na consulta atual:
            cache.Values.ToList().ForEach(@old =>
            {
                bool notExists = (listaNova.Where(@new => @new.Id == @old.Id).FirstOrDefault() == null);
                if (notExists)
                    cache.Values.Remove(@old);
            });

            cache.IsCached = true;

            return cache.Values;
        }
        */


        public static void Save(Desenvolvedor item)
        {
            string sql = null;
            string nome = item.Nome.ToSqlValue(ToSqlValueStringOption.EmptyOrWhiteSpacesToNull);
            string email = item.Email.ToSqlValue(ToSqlValueStringOption.EmptyOrWhiteSpacesToNull);
            TypedCache<Desenvolvedor> cache = Stock.Items<Desenvolvedor>();

            // Insert:
            if (item.Id == 0)
            {
                sql = "insert into UTL_DESENVOLVEDORES ( Nome, Email ) VALUES ( {0}, {1} ); select SCOPE_IDENTITY();";
                sql = string.Format(sql, nome, email);

                ConnectionManager.GetConnection.BeginTransaction();
                object identity = ConnectionManager.GetConnection.ExecuteScalar(sql);
                ConnectionManager.GetConnection.TryCommitTransaction();

                item.Id = (int)identity;

                cache.Values.Add(item);
            }
            // Update:
            else
            {
                sql = "update UTL_DESENVOLVEDORES set Nome = {0}, Email = {1} ) where Id = {2}";
                sql = string.Format(sql, nome, email, item.Id);

                int count = ConnectionManager.GetConnection.ExecuteNonQuery(sql);
            }
        }

        public static void Save(Sistema item)
        {
            string sql = null;
            string nome = item.Nome.ToSqlValue(ToSqlValueStringOption.EmptyOrWhiteSpacesToNull);
            TypedCache<Sistema> cache = Stock.Items<Sistema>();

            // Insert:
            if (item.Id == 0)
            {
                sql = "insert into UTL_SISTEMAS ( Nome ) VALUES ( {0} ); select SCOPE_IDENTITY();";
                sql = string.Format(sql, nome);

                ConnectionManager.GetConnection.BeginTransaction();
                object identity = ConnectionManager.GetConnection.ExecuteScalar(sql);
                ConnectionManager.GetConnection.TryCommitTransaction();

                item.Id = (int)identity;

                cache.Values.Add(item);
            }
            // Update:
            else
            {


                sql = "update UTL_SISTEMAS set Nome = {0} ) where Id = {1}";
                sql = string.Format(sql, nome, item.Id);

                int count = ConnectionManager.GetConnection.ExecuteNonQuery(sql);
            }
        }

        public static void Save(Bloqueio item)
        {
            string sql = null;
            TypedCache<Bloqueio> cache = Stock.Items<Bloqueio>();
            
            // Insert:
            if (item.Id == 0)
            {
                sql = "insert into UTL_BLOQUEIOS ( IdDesenvolvedor, IdSistema ) VALUES ( {0}, {1} ); select SCOPE_IDENTITY();";
                sql = string.Format(sql, item.IdDesenvolvedor, item.IdSistema);

                ConnectionManager.GetConnection.BeginTransaction();
                object identity = ConnectionManager.GetConnection.ExecuteScalar(sql);
                ConnectionManager.GetConnection.TryCommitTransaction();

                item.Id = int.Parse(identity.ToString());

                cache.Values.Add(item);
            }
            // Update:
            else
            {
                sql = "update UTL_BLOQUEIOS set IdDesenvolvedor = {0}, IdSistema = {1} ) where Id = {3}";
                sql = string.Format(sql, item.IdDesenvolvedor, item.IdSistema, item.Id);

                int count = ConnectionManager.GetConnection.ExecuteNonQuery(sql);
            }
        }

        public static void Delete(Bloqueio item)
        {
            TypedCache<Bloqueio> cache = Stock.Items<Bloqueio>();
            string sql = "delete from UTL_BLOQUEIOS where Id = " + item.Id;

            ConnectionManager.GetConnection.BeginTransaction();
            int count = ConnectionManager.GetConnection.ExecuteNonQuery(sql);
            //Bloqueios.Remove(item);
            cache.Values.Remove(item);
            ConnectionManager.GetConnection.TryCommitTransaction();
        }

    }
}