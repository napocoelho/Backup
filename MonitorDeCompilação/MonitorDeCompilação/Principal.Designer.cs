namespace MonitorDeCompilação
{
    partial class Principal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.btnShowVarejo = new System.Windows.Forms.Button();
            this.btnShowAtacado = new System.Windows.Forms.Button();
            this.btnShowAtacadoRemake = new System.Windows.Forms.Button();
            this.gbAtacadoRemake = new System.Windows.Forms.GroupBox();
            this.gbAtacado = new System.Windows.Forms.GroupBox();
            this.gbVarejo = new System.Windows.Forms.GroupBox();
            this.gbDesenvolvedores = new System.Windows.Forms.GroupBox();
            this.cbxUsuarios = new System.Windows.Forms.ComboBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.gbAtacadoRemake.SuspendLayout();
            this.gbAtacado.SuspendLayout();
            this.gbVarejo.SuspendLayout();
            this.gbDesenvolvedores.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowVarejo
            // 
            this.btnShowVarejo.BackColor = System.Drawing.Color.ForestGreen;
            this.btnShowVarejo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowVarejo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowVarejo.Location = new System.Drawing.Point(6, 18);
            this.btnShowVarejo.Name = "btnShowVarejo";
            this.btnShowVarejo.Size = new System.Drawing.Size(389, 54);
            this.btnShowVarejo.TabIndex = 0;
            this.btnShowVarejo.TabStop = false;
            this.btnShowVarejo.Text = "Liberado";
            this.btnShowVarejo.UseVisualStyleBackColor = false;
            this.btnShowVarejo.Click += new System.EventHandler(this.btnShowVarejo_Click);
            // 
            // btnShowAtacado
            // 
            this.btnShowAtacado.BackColor = System.Drawing.Color.ForestGreen;
            this.btnShowAtacado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAtacado.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowAtacado.Location = new System.Drawing.Point(6, 18);
            this.btnShowAtacado.Name = "btnShowAtacado";
            this.btnShowAtacado.Size = new System.Drawing.Size(389, 54);
            this.btnShowAtacado.TabIndex = 1;
            this.btnShowAtacado.TabStop = false;
            this.btnShowAtacado.Text = "Liberado";
            this.btnShowAtacado.UseVisualStyleBackColor = false;
            this.btnShowAtacado.Click += new System.EventHandler(this.btnShowAtacado_Click);
            // 
            // btnShowAtacadoRemake
            // 
            this.btnShowAtacadoRemake.BackColor = System.Drawing.Color.ForestGreen;
            this.btnShowAtacadoRemake.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAtacadoRemake.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowAtacadoRemake.Location = new System.Drawing.Point(6, 18);
            this.btnShowAtacadoRemake.Name = "btnShowAtacadoRemake";
            this.btnShowAtacadoRemake.Size = new System.Drawing.Size(389, 54);
            this.btnShowAtacadoRemake.TabIndex = 2;
            this.btnShowAtacadoRemake.TabStop = false;
            this.btnShowAtacadoRemake.Text = "Liberado";
            this.btnShowAtacadoRemake.UseVisualStyleBackColor = false;
            this.btnShowAtacadoRemake.Click += new System.EventHandler(this.btnShowAtacadoRemake_Click);
            // 
            // gbAtacadoRemake
            // 
            this.gbAtacadoRemake.Controls.Add(this.btnShowAtacadoRemake);
            this.gbAtacadoRemake.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAtacadoRemake.Location = new System.Drawing.Point(12, 180);
            this.gbAtacadoRemake.Name = "gbAtacadoRemake";
            this.gbAtacadoRemake.Size = new System.Drawing.Size(401, 78);
            this.gbAtacadoRemake.TabIndex = 12;
            this.gbAtacadoRemake.TabStop = false;
            this.gbAtacadoRemake.Text = "ATACADO REMAKE";
            // 
            // gbAtacado
            // 
            this.gbAtacado.Controls.Add(this.btnShowAtacado);
            this.gbAtacado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAtacado.Location = new System.Drawing.Point(12, 96);
            this.gbAtacado.Name = "gbAtacado";
            this.gbAtacado.Size = new System.Drawing.Size(401, 78);
            this.gbAtacado.TabIndex = 13;
            this.gbAtacado.TabStop = false;
            this.gbAtacado.Text = "OPERA";
            // 
            // gbVarejo
            // 
            this.gbVarejo.Controls.Add(this.btnShowVarejo);
            this.gbVarejo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbVarejo.Location = new System.Drawing.Point(12, 12);
            this.gbVarejo.Name = "gbVarejo";
            this.gbVarejo.Size = new System.Drawing.Size(401, 78);
            this.gbVarejo.TabIndex = 14;
            this.gbVarejo.TabStop = false;
            this.gbVarejo.Text = "VAREJO";
            // 
            // gbDesenvolvedores
            // 
            this.gbDesenvolvedores.Controls.Add(this.cbxUsuarios);
            this.gbDesenvolvedores.Controls.Add(this.txtSenha);
            this.gbDesenvolvedores.Controls.Add(this.label2);
            this.gbDesenvolvedores.Controls.Add(this.label1);
            this.gbDesenvolvedores.Controls.Add(this.btnLogin);
            this.gbDesenvolvedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDesenvolvedores.Location = new System.Drawing.Point(12, 269);
            this.gbDesenvolvedores.Name = "gbDesenvolvedores";
            this.gbDesenvolvedores.Size = new System.Drawing.Size(401, 78);
            this.gbDesenvolvedores.TabIndex = 15;
            this.gbDesenvolvedores.TabStop = false;
            this.gbDesenvolvedores.Text = "DESENVOLVEDORES";
            // 
            // cbxUsuarios
            // 
            this.cbxUsuarios.FormattingEnabled = true;
            this.cbxUsuarios.Location = new System.Drawing.Point(62, 27);
            this.cbxUsuarios.Name = "cbxUsuarios";
            this.cbxUsuarios.Size = new System.Drawing.Size(262, 21);
            this.cbxUsuarios.TabIndex = 0;
            this.cbxUsuarios.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxEmails_KeyDown);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(62, 52);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(262, 20);
            this.txtSenha.TabIndex = 1;
            this.txtSenha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Senha";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Usuário";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(330, 27);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(65, 45);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Logar";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnLogin_KeyDown);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 356);
            this.Controls.Add(this.gbDesenvolvedores);
            this.Controls.Add(this.gbVarejo);
            this.Controls.Add(this.gbAtacado);
            this.Controls.Add(this.gbAtacadoRemake);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Principal";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitor de compilação";
            this.Activated += new System.EventHandler(this.Principal_Activated);
            this.Deactivate += new System.EventHandler(this.Principal_Deactivate);
            this.Load += new System.EventHandler(this.Principal_Load);
            this.Resize += new System.EventHandler(this.Principal_Resize);
            this.gbAtacadoRemake.ResumeLayout(false);
            this.gbAtacado.ResumeLayout(false);
            this.gbVarejo.ResumeLayout(false);
            this.gbDesenvolvedores.ResumeLayout(false);
            this.gbDesenvolvedores.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowVarejo;
        private System.Windows.Forms.Button btnShowAtacado;
        private System.Windows.Forms.Button btnShowAtacadoRemake;
        private System.Windows.Forms.GroupBox gbAtacadoRemake;
        private System.Windows.Forms.GroupBox gbAtacado;
        private System.Windows.Forms.GroupBox gbVarejo;
        private System.Windows.Forms.GroupBox gbDesenvolvedores;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.ComboBox cbxUsuarios;

    }
}

