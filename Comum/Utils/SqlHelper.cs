using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comum.Models;
using Comum.Utils;
using ConnectionManagerDll;
using System.Data;

using System.IO;


namespace Comum.Utils
{
    public static class SqlHelper
    {
        public static void Fechar()
        {
            ConnectionManagerDll.ConnectionManager.Close();
        }

        public static ThreadSafeConnection Conectar(string serverName, int timeout = 0)
        {
            string conexao = "Initial Catalog=master;" +
                             "Data Source=" + serverName + ";" +
                             "User ID=sistema;" +
                             "Password=schwer_wissen;" +
                             "Connect Timeout=" + timeout + ";" +
                             "Application Name='Backup'";

            ConnectionManagerDll.ConnectionManager.CreateInstance(conexao);
            return ConnectionManagerDll.ConnectionManager.GetConnection;
        }

        public static ThreadSafeConnection Conectar(EventoDeBancos config, int timeout = 0)
        {
            string conexao = "Initial Catalog=master;" +
                             "Data Source=" + config.Servidor + ";" +
                             "User ID=sistema;" +
                             "Password=schwer_wissen;" +
                             "Connect Timeout=" + timeout + ";" +
                             "Application Name='Backup'";

            ConnectionManagerDll.ConnectionManager.CreateInstance(conexao);
            return ConnectionManagerDll.ConnectionManager.GetConnection;
        }





        public static void Shrink(string databaseName)
        {
            Use(databaseName);

            string StrSql = "select name from " + databaseName + ".[sys].[database_files] where type_desc = 'LOG'";
            string nomeLogicoLogGest = ConnectionManager.GetConnection.ExecuteScalar(StrSql).ToString();

            //'Liberando log do GESTPLUS_:
            ConnectionManager.GetConnection.ExecuteNonQuery("alter database " + databaseName + " set recovery simple");
            ConnectionManager.GetConnection.ExecuteNonQuery("checkpoint");
            ConnectionManager.GetConnection.ExecuteNonQuery("alter database " + databaseName + " set recovery full");
            ConnectionManager.GetConnection.ExecuteNonQuery("dbcc shrinkfile (" + nomeLogicoLogGest + " , 1)");

            //'Aqui pode demorar bastante. Vai depender da quantidade e da fragmentação dos dados no banco:
            string nomeLogicoDatGest;

            //'Recuperar nome DAT banco:
            StrSql = "select name as STRING from " + databaseName + ".[sys].[database_files] where type_desc = 'ROWS'";
            nomeLogicoDatGest = ConnectionManager.GetConnection.ExecuteScalar(StrSql).ToString();

            ConnectionManager.GetConnection.ExecuteNonQuery("dbcc shrinkfile (" + nomeLogicoDatGest + " , 1)");
        }

        public static void UpdateStatistics(string databaseName)
        {
            Use(databaseName);
            ConnectionManager.GetConnection.ExecuteNonQuery("sp_updatestats");
        }

        public static void CorrigirNomesLogicos(string databaseName)
        {
            string StrSql, nomeLogicoLogGest, nomeLogicoDatGest;

            Use(databaseName);

            /*
            'Esta função mantém os nomes lógicos dos bancos de dados, seguindo o padrão:
            ' - Nome lógico para o arquivo de "DADOS":   GESTPLUS_MULTIMIX
            ' - Nome lógico para o arquivo de "LOG":     GESTPLUS_MULTIMIX_log
            ' - Nome lógico para o arquivo de "DADOS":   AUDITORIA_MULTIMIX
            ' - Nome lógico para o arquivo de "LOG":     AUDITORIA_MULTIMIX_log
            */


            // 'Recuperar nome LOG do GESTPLUS e AUDITORIA:
            StrSql = "select name as STRING from " + databaseName + ".[sys].[database_files] where type_desc = 'LOG'";
            nomeLogicoLogGest = ConnectionManager.GetConnection.ExecuteScalar(StrSql).ToString();

            // 'Recuperar nome DAT do GESTPLUS e AUDITORIA:
            StrSql = "select name as STRING from " + databaseName + ".[sys].[database_files] where type_desc = 'ROWS'";
            nomeLogicoDatGest = ConnectionManager.GetConnection.ExecuteScalar(StrSql).ToString();

            // 'Verificando nome do arquivo de log. Se não estiver padronizado, será renomeado:
            if (nomeLogicoLogGest.ToUpper() != (databaseName + "_log").ToUpper() || nomeLogicoDatGest.ToUpper() != databaseName.ToUpper())
            {
                // 'Gestplus:
                StrSql = "ALTER DATABASE [" + databaseName + "] MODIFY FILE (NAME = " + nomeLogicoDatGest + ", NEWNAME = " + databaseName + " )";
                ConnectionManager.GetConnection.ExecuteNonQuery(StrSql);

                StrSql = "ALTER DATABASE [" + databaseName + "] MODIFY FILE (NAME = " + nomeLogicoLogGest + ", NEWNAME = " + databaseName + "_log )";
                ConnectionManager.GetConnection.ExecuteNonQuery(StrSql);
            }
        }

        public static void RemoverTabelasTemporarias(string databaseName)
        {
            Use(databaseName);

            DataTable table = ConnectionManager.GetConnection.ExecuteDataTable("select name from " + databaseName + ".sys.objects where name like 'TEMP_%'");


            foreach (DataRow row in table.Rows)
            {
                ConnectionManager.GetConnection.ExecuteNonQuery("delete from " + row["name"].ToString());
                ConnectionManager.GetConnection.ExecuteNonQuery("drop table " + row["name"].ToString());
            }
        }


        public static void Use(string databaseName)
        {
            ConnectionManager.GetConnection.ExecuteNonQuery("use " + databaseName);
        }

        /// <summary>
        /// Kill all processes on the database.
        /// </summary>
        public static void KillAll(string databaseName)
        {
            databaseName = databaseName.Trim().ToUpper();

            //Use(databaseName);
            DataTable result = ConnectionManager.GetConnection.ExecuteDataTable("sp_who2");
            int selfSpid = int.Parse(ConnectionManager.GetConnection.ExecuteScalar("select @@spid").ToString());

            foreach (DataRow xRow in result.Rows)
            {
                string dbName = xRow["DBName"].ToString().Trim().ToUpper();
                int spid = int.Parse(xRow["SPID"].ToString().Trim().ToUpper());
                string hostName = xRow["HostName"].ToString().Trim();

                if (dbName == databaseName
                    && hostName != "."      //--> processos internos do Sql Server
                    && spid != selfSpid)    //--> o próprio processo ao qual se está executando
                {
                    ConnectionManager.GetConnection.ExecuteNonQuery("kill " + spid);
                }
            }
        }

        public static void RebuildAllIndexes(string databaseName, int fillFactor = 90)
        {
            Use(databaseName);

            string sql = string.Empty;

            databaseName = databaseName.Trim();

            sql = @"SELECT '[' + table_catalog + '].[' + table_schema + '].[' + table_name + ']' as tableName FROM " + databaseName + ".INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'";
            DataTable result = ConnectionManager.GetConnection.ExecuteDataTable(sql);

            foreach (DataRow xRow in result.Rows)
            {
                string fullTableName = xRow["tableName"].ToString();
                sql = @"ALTER INDEX ALL ON " + fullTableName + " REBUILD WITH (FILLFACTOR = " + fillFactor + ")";
                ConnectionManager.GetConnection.ExecuteNonQuery(sql);
            }
        }

        public static void SetSingleUser(string databaseName, bool active = true)
        {
            if (active)
            {
                ConnectionManager.GetConnection.ExecuteNonQuery(@"exec sp_dboption '" + databaseName + "', 'single_user', 'true'");
            }
            else
            {
                ConnectionManager.GetConnection.ExecuteNonQuery(@"exec sp_dboption '" + databaseName + "', 'single_user', 'false'");
            }
        }

        public static void CheckDb(string databaseName)
        {
            databaseName = databaseName.Trim();

            try
            {
                // Fazendo primeira vez:
                Use(databaseName);
                KillAll(databaseName);

                ConnectionManager.GetConnection.ExecuteNonQuery(@"exec sp_configure 'allow updates', 1");
                SetSingleUser(databaseName, false);
                SetSingleUser(databaseName, true);
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC CHECKDB ('" + databaseName + "',REPAIR_REBUILD) ");
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC CHECKDB ('" + databaseName + "', REPAIR_ALLOW_DATA_LOSS)");


                // Fazendo segunda vez:
                Use(databaseName);
                KillAll(databaseName);

                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC CHECKDB ('" + databaseName + "',REPAIR_REBUILD) ");
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC CHECKDB ('" + databaseName + "', REPAIR_ALLOW_DATA_LOSS)");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SetSingleUser(databaseName, false);
            }
        }


        public static void FreeCache(string databaseName)
        {
            databaseName = databaseName.Trim();

            try
            {
                Use(databaseName);
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC FREESESSIONCACHE");
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC DROPCLEANBUFFERS");
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC FREEPROCCACHE");
                ConnectionManager.GetConnection.ExecuteNonQuery(@"DBCC FREESYSTEMCACHE ( 'ALL' )");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SetSingleUser(databaseName, false);
            }
        }

        /// <summary>
        /// Inicia processo de backup de um banco de dados.
        /// </summary>
        /// <param name="databaseName">Nome do banco de dados</param>
        /// <returns>Retorna o caminho completo do arquivo do backup criado</returns>
        public static FileInfo Backup(string databaseName, string fileName)
        {
            databaseName = databaseName.Trim().ToUpper();

            FileInfo fileToReturn = null;
            string sql = string.Empty;
            string dirName = Parâmetros.GetInstance.DiretórioDoExecutável + @"\Backuping";
            dirName = FileHelper.GetUNCPath(dirName);
            string bkpFileNameTemp = dirName + @"\temp.bak";
            string bkpFileName = dirName + @"\" + fileName;



            try
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                if (File.Exists(bkpFileNameTemp))
                {
                    File.Delete(bkpFileNameTemp);
                }

                if (File.Exists(bkpFileName))
                {
                    File.Delete(bkpFileName);
                }

                //bkpFileNameTemp = @"F:\Dados\Backup_Clientes\Testes\temp.bak";

                sql = "BACKUP DATABASE " + databaseName + " TO DISK = '" + bkpFileNameTemp + "' WITH FORMAT";
                ConnectionManager.GetConnection.ExecuteNonQuery(sql);


                // Pode esperar de 0 a 10 segundos para garantir que o arquivo foi liberado pelo Sql Server após o backup;
                double segundosEsperados = 0;
                while (!File.Exists(bkpFileNameTemp))
                {
                    segundosEsperados += 0.5;
                    System.Threading.Thread.Sleep(500);

                    if (segundosEsperados > 60)
                    {
                        throw new FileNotFoundException("O tempo limite para gerar o arquivo de backup foi atingido. O arquivo de backup não foi gerado pelo Sql Server!");
                    }
                }

                File.Move(bkpFileNameTemp, bkpFileName);    // Renomeando o arquivo;
                System.Threading.Thread.Sleep(500);
                fileToReturn = new FileInfo(bkpFileName);
            }
            catch (Exception ex)
            {
                if (File.Exists(bkpFileName))
                {
                    File.Delete(bkpFileName);
                }

                throw ex;
            }
            finally
            {
                if (File.Exists(bkpFileNameTemp))
                {
                    File.Delete(bkpFileNameTemp);
                }
            }

            return fileToReturn;
        }







    }
}