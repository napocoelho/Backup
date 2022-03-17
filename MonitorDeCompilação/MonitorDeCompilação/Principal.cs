using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MonitorDeCompilação.Controllers;
using MonitorDeCompilação.Models;
using MonitorDeCompilação.ModelsCore;
using ConnectionManagerDll;

using Microsoft.Win32;

namespace MonitorDeCompilação
{
    public partial class Principal : Form
    {
        private StatusController StatusController { get; set; }

        public Principal()
        {
            try
            {
                InitializeComponent();


                // Configurando controles:
                btnShowVarejo.Enabled = true;
                btnShowAtacado.Enabled = true;
                btnShowAtacadoRemake.Enabled = true;

                btnShowVarejo.BackColor = Color.ForestGreen;
                btnShowAtacado.BackColor = Color.ForestGreen;
                btnShowAtacadoRemake.BackColor = Color.ForestGreen;

                btnShowVarejo.ForeColor = Color.WhiteSmoke;
                btnShowAtacado.ForeColor = Color.WhiteSmoke;
                btnShowAtacadoRemake.ForeColor = Color.WhiteSmoke;



                // Configurando infra:
                Conexao.Connect("SERVIDOR", "GESTPLUS_ALFA");
                this.StatusController = new StatusController();
                this.StatusController.LogadoChanged += StatusController_LogadoChanged;
                this.StatusController.BloqueioReloaded += StatusController_BloqueioReloaded;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um problema ao iniciar o aplicativo. Motivo: " + ex.Message);
                Application.Exit();
            }
        }

        void StatusController_LogadoChanged(StatusController sender)
        {

        }

        void StatusController_BloqueioReloaded(StatusController sender)
        {

            foreach (Sistema sistema in sender.Sistemas)
            {
                Button btn = null;

                // 
                if (sistema.Nome.ToUpper() == "VAREJO")
                {
                    btn = btnShowVarejo;
                }
                else if (sistema.Nome.ToUpper() == "ATACADO")
                {
                    btn = btnShowAtacado;
                }
                else if (sistema.Nome.ToUpper() == "ATACADO REMAKE")
                {
                    btn = btnShowAtacadoRemake;
                }


                // Atualiza status:
                if (sistema.Liberado)
                {
                    btn.Text = "Liberado";
                    btn.BackColor = Color.ForestGreen;
                }
                else
                {
                    List<string> usuarios = new List<string>();

                    foreach (Bloqueio bloqueio in sistema.Bloqueios)
                    {
                        usuarios.Add(bloqueio.Desenvolvedor.Nome);
                    }

                    btn.Text = "(" + string.Join(", ", usuarios) + ")";
                    btn.BackColor = Color.Firebrick;
                }
            }

        }

        private void Principal_Load(object sender, EventArgs e)
        {
            StatusController_BloqueioReloaded(this.StatusController);

            // Configurando combo de Emails:
            cbxUsuarios.DisplayMember = "Nome";
            cbxUsuarios.ValueMember = "Id";
            cbxUsuarios.DataSource = Repository.GetDesenvolvedores(false);
            cbxUsuarios.SelectedItem = null;

            // obtendo último email logado do Registry:
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
            key = key.OpenSubKey("Compilações");

            //if (key != null && key.GetValue("nome_dev") != null)
            if (key != null && key.GetValue("nome_dev") != null)
            {
                bool found = false;

                string nome = key.GetValue("nome_dev").ToString();

                foreach (object item in cbxUsuarios.Items)
                {
                    Desenvolvedor dev = item as Desenvolvedor;

                    if (dev != null)
                    {
                        if (dev.Nome.ToUpper() == nome.ToUpper())
                        {
                            cbxUsuarios.SelectedItem = dev;
                            found = true;
                            break;
                        }
                    }
                }


                if (!found)
                    cbxUsuarios.SelectedItem = null;
            }
        }



        private void btnShowVarejo_Click(object sender, EventArgs e)
        {
            if (!this.StatusController.PossuiPermissãoParaAlterar)
                return;

            string nomeSistema = "Varejo".ToUpper();
            Sistema sistema = Repository.GetSistemas(false).Where(x => x.Nome.ToUpper() == nomeSistema).FirstOrDefault();
            this.StatusController.AlterarBloqueio(sistema);
        }

        private void btnShowAtacado_Click(object sender, EventArgs e)
        {
            if (!this.StatusController.PossuiPermissãoParaAlterar)
                return;

            string nomeSistema = "Atacado".ToUpper();
            Sistema sistema = Repository.GetSistemas(false).Where(x => x.Nome.ToUpper() == nomeSistema).FirstOrDefault();
            this.StatusController.AlterarBloqueio(sistema);
        }

        private void btnShowAtacadoRemake_Click(object sender, EventArgs e)
        {
            if (!this.StatusController.PossuiPermissãoParaAlterar)
                return;

            string nomeSistema = "Atacado Remake".ToUpper();
            Sistema sistema = Repository.GetSistemas(false).Where(x => x.Nome.ToUpper() == nomeSistema).FirstOrDefault();
            this.StatusController.AlterarBloqueio(sistema);
        }





        private void TentarLogar()
        {
            Desenvolvedor dev = cbxUsuarios.SelectedItem as Desenvolvedor;

            if (this.StatusController.PossuiPermissãoParaAlterar)
            {
                this.StatusController.Deslogar();

                btnLogin.Text = "Logar";
                txtSenha.Text = "";
                cbxUsuarios.Enabled = true;
                txtSenha.Enabled = true;

                // excluindo último email no Registry:
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                key = key.CreateSubKey("Compilações");
                key.DeleteValue("nome_dev");
            }
            else
            {
                try
                {
                    btnLogin.Enabled = false;
                    this.StatusController.Logar(dev, txtSenha.Text);
                    btnLogin.Text = "Deslogar";
                    cbxUsuarios.Enabled = false;
                    txtSenha.Enabled = false;

                    // registrando último email no Registry:
                    RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                    key = key.CreateSubKey("Compilações");
                    key.SetValue("nome_dev", dev.Nome);
                    btnLogin.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    btnLogin.Enabled = true;
                }
            }


        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            TentarLogar();
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TentarLogar();
            }
        }

        private void cbxEmails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TentarLogar();
            }
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TentarLogar();
            }
        }

        private void Principal_Resize(object sender, EventArgs e)
        {
            int with;

            // grupos:
            with = this.Width - 35;
            gbVarejo.Width = with;
            gbAtacado.Width = with;
            gbAtacadoRemake.Width = with;
            gbDesenvolvedores.Width = with;


            // botões dos grupos:
            with = gbVarejo.Width - 15;
            btnShowVarejo.Width = with;
            btnShowAtacado.Width = with;
            btnShowAtacadoRemake.Width = with;

            // controles do grupo Desenvolvedores:
            btnLogin.Left = gbDesenvolvedores.Width - btnLogin.Width - 7;
            //cbxEmails.Width = (cbxEmails.Left + cbxEmails.Width) - btnLogin.Left - 7;
            cbxUsuarios.Width = (btnLogin.Left - cbxUsuarios.Left) - 7;
            txtSenha.Width = (btnLogin.Left - txtSenha.Left) - 7;

            // altura do grupo Desenvolvedores:
            gbDesenvolvedores.Top = this.Height - gbDesenvolvedores.Height - 50;
        }

        private void Principal_Activated(object sender, EventArgs e)
        {
            if (StatusController != null)
            {
                StatusController.AutoAtualização = true;
            }
        }

        private void Principal_Deactivate(object sender, EventArgs e)
        {
            if (StatusController != null)
            {
                StatusController.AutoAtualização = false;
            }
        }
    }
}
