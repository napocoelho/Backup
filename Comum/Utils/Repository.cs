using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

using Comum.Models;

//using System.Data.SQLite;
using System.Data;
using System.ComponentModel;


namespace Comum.Utils
{

    [Serializable]
    public class Repository : BindableBase
    {
        private static Repository _INSTANCE_ = null;

        private static string REPOSITORY_FILE_NAME = Parâmetros.GetInstance.DiretórioDoExecutável + @"\Repository.xml";
        private static string REPOSITORY_FILE_NAME_BKP = Parâmetros.GetInstance.DiretórioDoExecutável + @"\Repository.Backup.xml";
        private static string REPOSITORY_FILE_NAME_TEMP = Parâmetros.GetInstance.DiretórioDoExecutável + @"\Repository.Temp";

        public Agenda Agenda { get; set; }

        //[XmlArrayItem(Type = typeof(TarefaBase)), XmlArrayItem(Type = typeof(TarefaBackupDeArquivos)), XmlArrayItem(Type = typeof(TarefaBackupDeBancos))]   //==> por causa da hierarquia;
        //[XmlIgnoreAttribute]
        
        public Repository()
        {
            if (this.Agenda == null)
            {
                this.Agenda = new Agenda();
            }
        }



        public static void Save()
        {
            try
            {
                if (File.Exists(REPOSITORY_FILE_NAME_BKP))
                {
                    File.Delete(REPOSITORY_FILE_NAME_BKP);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Repository));

                using (TextWriter writer = new StreamWriter(REPOSITORY_FILE_NAME_TEMP, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, _INSTANCE_);
                }

                // Renomeando arquivo com nome temporário para o nome final:
                if (File.Exists(REPOSITORY_FILE_NAME_TEMP))
                {
                    if (File.Exists(REPOSITORY_FILE_NAME))
                    {
                        File.Delete(REPOSITORY_FILE_NAME);
                    }

                    File.Move(REPOSITORY_FILE_NAME_TEMP, REPOSITORY_FILE_NAME);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(REPOSITORY_FILE_NAME_TEMP))
                {
                    File.Delete(REPOSITORY_FILE_NAME_TEMP);
                }

                // Se der alguma merda, recupera o backup:
                throw ex;
            }
        }

        public static void Load()
        {
            try
            {
                if (!File.Exists(REPOSITORY_FILE_NAME))
                {
                    _INSTANCE_ = new Repository();
                    Save();
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Repository));

                    using (TextReader reader = new StreamReader(REPOSITORY_FILE_NAME, Encoding.UTF8))
                    {
                        _INSTANCE_ = serializer.Deserialize(reader) as Repository;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public static Repository GetInstance
        {
            get
            {
                if (_INSTANCE_ == null)
                {
                    Load();
                }

                return _INSTANCE_;


                
            }
        }

    }
}