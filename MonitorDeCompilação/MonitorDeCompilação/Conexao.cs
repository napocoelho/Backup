using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerDll;

namespace MonitorDeCompilação
{
    public static class Conexao
    {
        public static ThreadSafeConnection Connect(string nomeServidor, string nomeBaseDados = "master")
        {

            List<String> CommandsList;
            string StrConexao;


            CommandsList = new List<String>();
            CommandsList.Add("SET LANGUAGE 'Português (Brasil)'");
            CommandsList.Add("SET LOCK_TIMEOUT 5000");

            nomeBaseDados = nomeBaseDados.Trim();
            nomeServidor = nomeServidor.Trim();

            StrConexao = "Initial Catalog=" + nomeBaseDados + ";" +
                         "Data Source=" + nomeServidor + ";" +
                         "User ID=sistema;" +
                         "Password=schwer_wissen;" +
                         "Connect Timeout=30;" +
                         "Application Name='Snipers'";

            /*
            'StrConexao = "Initial Catalog=" & StrnomeBaseDados & ";" & _
            '             "Data Source=" & StrnomeServidor & ";" & _
            '             "User ID=preview;" & _
            '             "Password=admin@atende1317;" & _
            '             "Connect Timeout=0;" & _
            '             "Application Name='Teste'"
            */

            ConnectionManager.CreateInstance(StrConexao, CommandsList);
            return ConnectionManager.GetConnection;
        }
    }
}
