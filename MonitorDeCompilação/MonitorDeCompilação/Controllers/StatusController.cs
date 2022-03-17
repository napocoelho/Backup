using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using MonitorDeCompilação.Models;
using MonitorDeCompilação.ModelsCore;
using ConnectionManagerDll;


using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using OpenPop.Mime;



namespace MonitorDeCompilação.Controllers
{
    public class StatusController : INotifyPropertyChanged
    {
        private static object LOCK = new object();
        private bool autoAtualização;
        private Desenvolvedor logado;

        public Desenvolvedor Logado
        {
            get { return this.logado; }
            set
            {
                if (this.logado != value)
                {
                    this.logado = value;
                    OnLogadoChanged();
                    OnPropertyChanged("Logado");
                }
            }
        }

        public BindingList<Sistema> Sistemas { get; private set; }

        public bool PossuiPermissãoParaAlterar { get { return this.Logado != null; } }



        public bool AutoAtualização
        {
            get
            {
                lock (LOCK)
                {
                    return autoAtualização;
                }
            }
            set
            {
                lock (LOCK)
                {
                    autoAtualização = value;
                }
            }
        }

        public StatusController()
        {
            this.Logado = null;
            this.autoAtualização = true;
            this.Sistemas = Repository.GetSistemas().ToBindingList<Sistema>();


            Task.Run(() =>
            {
                while (true)
                {
                    if (!this.AutoAtualização)
                    {
                        System.Threading.Thread.Sleep(500);
                        continue;
                    }

                    try
                    {
                        Repository.GetBloqueios(false); // força leitura do banco de dados;
                        OnBloqueioReloaded();
                    }
                    catch (Exception ex)
                    {
                        ConnectionManagerDll.ConnectionManager.Reconnect();
                    }

                    System.Threading.Thread.Sleep(5000);
                }
            });
        }

        public void AlterarBloqueio(Sistema sistema)
        {
            if (!this.PossuiPermissãoParaAlterar)
                return;

            try
            {
                ConnectionManager.GetConnection.BeginTransaction();

                Repository.GetBloqueios(false);


                bool isSistemaBloqueadoPeloUsuario = this.Logado.Bloqueios.Where(x => x.IdSistema == sistema.Id).Count() > 0;

                if (!isSistemaBloqueadoPeloUsuario)
                {
                    Bloqueio novoBloqueio = new Bloqueio(sistema, this.Logado);
                    Repository.Save(novoBloqueio);
                }
                else
                {
                    foreach (Bloqueio item in this.Logado.Bloqueios.Where(b => b.Sistema.Id == sistema.Id))
                    {
                        Repository.Delete(item);
                    }
                }


                foreach (Bloqueio block in this.Logado.Bloqueios)
                {
                    string teste = block.Sistema.Id.ToString();
                }

                ConnectionManager.GetConnection.CommitTransaction();

                OnBloqueioReloaded();
            }
            catch (Exception ex)
            {
                Repository.GetBloqueios(false); // voltando ao status gravado no bd;
                ConnectionManager.GetConnection.RollbackTransaction();
                throw ex;
            }

        }



        private bool ValidateConnection(string hostname, int port, bool useSsl, string username, string password)
        {
            //http://hpop.sourceforge.net/exampleFetchAllMessages.php

            bool isConnected = false;

            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                isConnected = client.Connected;
            }
            

            return isConnected;
        }

        public bool Autenticar(string usuario, string senha)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            string text = web.DownloadString("http://gdatasistemas.com/gdata/parolas.tmp");
            string[] registros = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            usuario = (usuario.ToUpper() + "@").Replace("@@", "@");
            senha = senha.ToUpper();

            foreach (string registro in registros)
            {
                string usuarioCriptografado = registro.TakeWhile(chr => chr != ':').JoinText().ToUpper();
                string senhaCriptografada = registro.Replace(usuarioCriptografado + ":", string.Empty);

                string senhaHashed = this.GetMd5Hash(usuario + senha);

                if (usuarioCriptografado.Equals(usuario) && senhaHashed.Equals(senhaCriptografada))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetMd5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public void Logar(Desenvolvedor dev, string senha)
        {
            if (dev == null)
                throw new Exception("Registro de desenvolvedor não encontrado!");

            //if (ValidateConnection("pop.gmail.com", 995, true, dev.Email, senha))
            if(Autenticar (dev.Nome , senha))
            {
                this.Logado = dev;
            }
            else
            {
                throw new Exception("Email ou senha inválidos!");
            }
        }

        public void Deslogar()
        {
            this.Logado = null;
        }


        #region Eventos

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event Action<StatusController> LogadoChanged;

        private void OnLogadoChanged()
        {
            if (LogadoChanged != null)
                LogadoChanged(this);
        }

        public event Action<StatusController> BloqueioReloaded;

        private void OnBloqueioReloaded()
        {
            if (BloqueioReloaded != null)
                BloqueioReloaded(this);
        }

        #endregion Eventos

    }
}