using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comum.Models;
using System.Threading;
using Comum.Utils;

namespace Comum.Serviços
{
    public class ExecutorDeTarefasDeBancos : IExecutor
    {

        public object Caller { get; private set; }
        public TarefaDeBancos Tarefa { get; private set; }
        public Log Log { get; private set; }

        public ExecutorDeTarefasDeBancos(TarefaDeBancos tarefa, Log log, object caller)
        {
            if (tarefa == null)
            {
                throw new ArgumentNullException("tarefa", "A referência para [Tarefa] não pode ser nula.");
            }

            this.Tarefa = tarefa;
            this.Log = log;
            this.Caller = caller;
        }

        public void Executar()
        {
            this.Log.Debug(this, "ExecutarTarefa");
            this.Log.Event(this, "Iniciando execução de tarefas");


            try
            {
                SqlHelper.Conectar(this.Tarefa.Servidor, 10);
                SqlHelper.Conectar(this.Tarefa.Servidor);

                //OperaçõesDeBanco operação = new OperaçõesDeBanco(this, this.Log);

                foreach (string nomeBanco in this.Tarefa.Bancos)
                {
                    try
                    {
                        // Liberando vínculo com conexões anteriores:
                        SqlHelper.Use("master");
                        //SqlServerHelper.SetSingleUser(nomeBanco, false);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Warning(this, "Erro ao tentar utilizar o banco : master : " + ex.Message);
                    }

                    try
                    {

                        // Liberando vínculo com conexões anteriores:
                        SqlHelper.Use(nomeBanco);
                        //SqlServerHelper.SetSingleUser(nomeBanco, false);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(this, "Erro ao tentar utilizar o banco : " + nomeBanco + " : " + ex.Message);
                        continue;
                    }


                    //------------------ OTIMIZAÇÃO DIÁRIA ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    if (this.Tarefa.OtimizaçãoDiária)
                    {
                        this.OtimizaçãoDiária(nomeBanco);
                    }


                    //------------------ OTIMIZAÇÃO SEMANAL ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    if (this.Tarefa.OtimizaçãoSemanal)
                    {
                        this.OtimizaçãoSemanal(nomeBanco);
                    }


                    //------------------ PROCEDIMENTO DE BACKUP ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    if (this.Tarefa.EfetuarBackup)
                    {
                        this.EfetuarBackup(nomeBanco, this.Tarefa.DiretórioDestino);
                    }
                }


                //------------------ EXCLUSÃO SEMANAL ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (this.Tarefa.ExclusãoSemanal)
                {
                    this.ExclusãoSemanal();
                }


                //------------------ EXCLUSÃO MENSAL ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (this.Tarefa.ExclusãoMensal)
                {
                    this.ExclusãoMensal();
                }

                // Força a conexão a ser fechada;
                SqlHelper.Fechar();

            }
            catch (Exception ex)
            {
                this.Log.Error(this, "Falha: " + ex.Message);
            }

            

            this.Log.Event(this, "Finalizando execução de tarefas");
        }







        /*******************************************************************************************************************************************************************************************************************************************************************************************************************************************/




        public void OtimizaçãoDiária(string nomeBanco)
        {
            this.Log.Event(this.Caller, nomeBanco + " : Início da otimização diária");

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Removendo tabelas temporárias");
                SqlHelper.RemoverTabelasTemporarias(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : RemoverTabelasTemporarias : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Corrigindo nomes lógicos");
                SqlHelper.CorrigirNomesLogicos(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : CorrigirNomesLogicos : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Efetuando Shrink");
                SqlHelper.Shrink(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : Shrink : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Limpando Cache");
                SqlHelper.FreeCache(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : FreeCache : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Efetuando UpdateStatistics");
                SqlHelper.UpdateStatistics(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : UpdateStatistics : " + ex.Message);
            }

            this.Log.Event(this.Caller, "Término da otimização diária");
        }


        public void OtimizaçãoSemanal(string nomeBanco)
        {
            this.Log.Event(this.Caller, "Início da otimização semanal");

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Efetuando CheckDb");
                SqlHelper.CheckDb(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : CheckDb : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Reconstruindo todos os índices");
                SqlHelper.RebuildAllIndexes(nomeBanco, 90);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, "RebuildAllIndexes : " + nomeBanco + " : " + ex.Message);
            }

            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Efetuando UpdateStatistics");
                SqlHelper.UpdateStatistics(nomeBanco);
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : UpdateStatistics : " + ex.Message);
            }

            this.Log.Event(this.Caller, "Término da otimização semanal");
        }


        public void EfetuarBackup(string nomeBanco, string dirDestino)
        {
            this.Log.Event(this.Caller, "Início do procedimento de backup");

            DateTime dataHora = DateTime.Now;
            string dbFileName = null;
            //string dbFileNameZip = (BackupFilesHelper.CriarNomeArquivoDeBanco(nomeBanco, dataHora)).ToUpper() + ".zip";
            System.IO.FileInfo arquivoBkp = null;
            System.IO.FileInfo arquivoZip = null;
            bool efetuouBackup = false;


            //------------------ BACKUP -------------------------
            try
            {
                this.Log.Debug(this.Caller, nomeBanco + " : Efetuando backup");

                dbFileName = (BackupFilesHelper.MakeDatabaseBackupName(nomeBanco, dataHora)).ToUpper() + ".bak";

                // Fazendo o backup:
                arquivoBkp = SqlHelper.Backup(nomeBanco, dbFileName);
                efetuouBackup = true;
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : Efetuando backup : " + ex.Message);
            }


            //------------------ COMPACTAÇÃO --------------------
            try
            {
                if (efetuouBackup)
                {
                    // Compactando:
                    this.Log.Debug(this.Caller, nomeBanco + " : Efetuando compactação zip");
                    arquivoZip = BackupFilesHelper.CompressDatabaseBackupFile(arquivoBkp, new System.IO.DirectoryInfo(dirDestino));
                }
                else
                {
                    this.Log.Debug(this.Caller, nomeBanco + " : Não efetuou compactação. Houve algum problema na criação do backup");
                    this.Log.Warning(this.Caller, nomeBanco + " : Não efetuou compactação. Houve algum problema na criação do backup");
                }
            }
            catch (Exception ex)
            {
                this.Log.Error(this.Caller, nomeBanco + " : Efetuando compactação : " + ex.Message);
            }
            finally
            {
                try
                {
                    if (arquivoBkp != null && arquivoBkp.Exists)
                    {
                        arquivoBkp.Delete();
                    }
                }
                catch (Exception ex)
                {
                    this.Log.Error(this.Caller, nomeBanco + " : Efetuando compactação : Limpeza de arquivos : " + ex.Message);
                }
            }

            this.Log.Event(this.Caller, "Término do procedimento de backup");
        }


        public void ExclusãoSemanal()
        {
            this.Log.Event(this.Caller, "Início de exclusão semanal de backups antigos");

            List<DatabaseBackupFileInfo> arquivos = BackupFilesHelper.GetAllCompressedDatabaseFiles(new System.IO.DirectoryInfo(this.Tarefa.DiretórioDestino));
            Dictionary<string, DateTime> backupsMaisRecentes = new Dictionary<string, DateTime>();


            // Obtém data mais recente entre os backups de cada banco de dados:
            foreach (DatabaseBackupFileInfo info in arquivos)
            {
                if (!backupsMaisRecentes.ContainsKey(info.Database))
                {
                    backupsMaisRecentes[info.Database] = info.Time;
                }
                else
                {
                    if (backupsMaisRecentes[info.Database] < info.Time)
                    {
                        backupsMaisRecentes[info.Database] = info.Time;
                    }
                }
            }


            // Procura arquivos com mais de 7 dias de existência, apartir da data do arquivo mais recente.
            // Ele não deve apagar o último backup, de cada banco de dados, neste caso.
            foreach (DatabaseBackupFileInfo info in arquivos)
            {
                try
                {
                    //DateTime umaSemanaAtrás = DateTime.Today.Subtract(TimeSpan.FromDays(7));
                    DateTime umaSemanaAtrás = backupsMaisRecentes[info.Database].Subtract(TimeSpan.FromDays(7));

                    // Excluir backups de uma semana antes do backup mais recente;
                    if (info.Time <= umaSemanaAtrás
                        || (info.Time.Year == umaSemanaAtrás.Year
                            && info.Time.Month == umaSemanaAtrás.Month
                            && info.Time.Day == umaSemanaAtrás.Day))
                    {
                        if (info.File.Exists)
                        {
                            this.Log.Debug(this.Caller, "Excluindo backup antigo : " + info.File.Name);
                            info.File.Delete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Log.Error(this.Caller, "Exclusão semanal : " + info.File.Name + " : " + ex.Message);
                }
            }

            this.Log.Event(this.Caller, "Término de exclusão semanal de backups antigos");
        }


        public void ExclusãoMensal()
        {
            this.Log.Event(this.Caller, "Início de exclusão mensal de backups antigos");

            List<DatabaseBackupFileInfo> arquivos = BackupFilesHelper.GetAllCompressedDatabaseFiles(new System.IO.DirectoryInfo(this.Tarefa.DiretórioDestino));
            Dictionary<string, DateTime> registrosMaisRecentes = new Dictionary<string, DateTime>();


            // Obtém data mais recente entre os backups de cada banco de dados:
            foreach (DatabaseBackupFileInfo info in arquivos)
            {
                if (!registrosMaisRecentes.ContainsKey(info.Database))
                {
                    registrosMaisRecentes[info.Database] = info.Time;
                }
                else
                {
                    if (registrosMaisRecentes[info.Database] < info.Time)
                    {
                        registrosMaisRecentes[info.Database] = info.Time;
                    }
                }
            }

            // Procura arquivos com mais de 30 dias de existência, apartir da data do arquivo mais recente.
            // Ele não deve apagar o último backup, de cada banco de dados, neste caso.
            foreach (DatabaseBackupFileInfo info in arquivos)
            {
                try
                {
                    //DateTime umaSemanaAtrás = DateTime.Today.Subtract(TimeSpan.FromDays(30));
                    DateTime umaMêsAtrás = registrosMaisRecentes[info.Database].Subtract(TimeSpan.FromDays(30));

                    // Excluir backups de um mês antes do backup mais recente;
                    if (info.Time <= umaMêsAtrás
                        || (info.Time.Year == umaMêsAtrás.Year
                            && info.Time.Month == umaMêsAtrás.Month
                            && info.Time.Day == umaMêsAtrás.Day))
                    {
                        if (info.File.Exists)
                        {
                            this.Log.Debug(this.Caller, "Excluindo backup antigo : " + info.File.Name);
                            info.File.Delete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Log.Error(this.Caller, "Exclusão mensal : " + info.File.Name + " : " + ex.Message);
                }
            }

            this.Log.Event(this.Caller, "Término de exclusão mensal de backups antigos");
        }

        /*******************************************************************************************************************************************************************************************************************************************************************************************************************************************/
    }
}