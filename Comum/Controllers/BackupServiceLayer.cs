using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;


using Comum.Models;
using Comum.Utils;
using Comum.Controllers;

/*
namespace Comum.Controllers
{
    /// <summary>
    /// Camada fina e testável do serviço de backups.
    /// </summary>
    public class BackupServiceLayer
    {
        private AgendaService Controller { get; set; }

        public BackupServiceLayer(AgendaService controller)
        {
            Directory.SetCurrentDirectory(RegeditHelper.DiretórioRaiz);

            this.Controller = controller;

            Repository.Log.CompleteLog += Log_CompleteLog;
        }

        public void OnStart(string[] args)
        {
            this.Controller.IniciarExecução();
        }

        public void OnStop()
        {
            //this.Controller.PararExecução();
        }

        public void OnShutdown()
        {
            //this.Controller.PararExecução();
        }


        private void Log_CompleteLog(object sender, StatusLog status, string message, DateTime time)
        {
            string fileName = RegeditHelper.DiretórioRaiz + @"\LogCompleto.txt";
            bool gravou = false;



            string[] content = {"{0} - {1}     {2}     {3}".FormatTo(  time.ToShortDateString(),
                                                                    time.ToShortTimeString(),
                                                                    status.ToString(),
                                                                    message
                                                                    )};


            lock (Repository.GetInstance)
            {
                while (!gravou)
                {
                    try
                    {
                        File.AppendAllLines(fileName, content, Encoding.UTF8);
                        //File.AppendAllText(fileName, content, Encoding.UTF8);

                        gravou = true;
                        System.Threading.Thread.Sleep(50);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}
*/