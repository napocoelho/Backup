using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
        
using Comum.Utils;
using Comum.Models;

namespace Comum.Serviços
{
    public class ExecutorDeTarefasDeArquivos : IExecutor
    {
        public object Caller { get; private set; }
        public TarefaDeArquivos Tarefa { get; private set; }
        public Log Log { get; private set; }

        public ExecutorDeTarefasDeArquivos(TarefaDeArquivos tarefa, Log log, object caller)
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
            this.Log.Event(Caller, "Iniciou backup de arquivos");
            this.Log.Debug(Caller, "Iniciou backup de arquivos");

            try
            {
                List<FileInfo> files = new List<FileInfo>();
                List<string> filtros = new List<string>();
                string zipFilePath = string.Empty;

                //------------------ PROCEDIMENTO DE BACKUP ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (this.Tarefa.EfetuarBackup)
                {
                    if (this.Tarefa.Extensões != null)
                    {
                        filtros = this.Tarefa.Extensões.ToString().Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }

                    zipFilePath = this.Tarefa.DiretórioDestino + (this.Tarefa.DiretórioDestino.TakeRight(1) == "\\" ? string.Empty : "\\") + BackupFilesHelper.MakeZipFileBackupName(this.Tarefa.Nome, DateTime.Now);
                    zipFilePath = FileHelper.GetUNCPath(zipFilePath);

                    this.Log.Debug(Caller, "Iniciando compressão de arquivos");
                    foreach (DiretórioDeBackup dir in this.Tarefa.Diretórios)
                    {
                        ZipHelper.CompressDirectory(new DirectoryInfo(dir.Caminho), zipFilePath, CompressionLevel.Normal);
                    }
                }
                else
                {
                    this.Log.Event(Caller, "A tarefa não está habilitada para efetuar backup!");
                }
            }
            catch (Exception ex)
            {
                this.Log.Error(this, "Falha: " + ex.Message);
            }

            this.Log.Event (Caller, "Finalizou backup de arquivos");
            this.Log.Debug(Caller, "Finalizou backup de arquivos");

        }


        public List<FileInfo> ObterArquivos(string diretórioPath, bool recursivo, List<string> filtros)
        {
            List<FileInfo> arquivos = new List<FileInfo>();
            List<String> arquivosEncontrados = new List<string>();

            if (recursivo)
            {
                foreach (string subDirPath in Directory.GetDirectories(diretórioPath))
                {
                    arquivos.AddRange(ObterArquivos(subDirPath, recursivo, filtros));
                }
            }

            if (filtros != null && filtros.Count > 0)
            {
                foreach (string filtro in filtros)
                {
                    arquivosEncontrados.AddRange(Directory.GetFiles(diretórioPath, filtro));
                }
            }
            else
            {
                arquivosEncontrados.AddRange(Directory.GetFiles(diretórioPath));
            }

            foreach (string arquivoPath in arquivosEncontrados)
            {
                FileInfo file = new FileInfo(arquivoPath);
                arquivos.Add(file);
            }

            return arquivos;
        }
    }
}