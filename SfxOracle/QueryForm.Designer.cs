namespace SfxOracle
{
    partial class QueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryForm));
            this.button1 = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.sqlText = new ICSharpCode.TextEditor.TextEditorControlEx();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rbResult = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rbBatchResult = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.lblxlsInfo = new System.Windows.Forms.Label();
            this.验证 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cb_sysdba = new System.Windows.Forms.CheckBox();
            this.cb_client = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(886, -3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 55);
            this.button1.TabIndex = 14;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(74, 16);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 21);
            this.txtIp.TabIndex = 15;
            this.txtIp.Text = "10.68.4.31";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(30, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "IP：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(198, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 14);
            this.label2.TabIndex = 18;
            this.label2.Text = "端口：";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(256, 10);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 17;
            this.txtPort.Text = "1521";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(366, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 20;
            this.label3.Text = "服务名：";
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(439, 10);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(100, 21);
            this.txtServiceName.TabIndex = 19;
            this.txtServiceName.Text = "sfxorcl";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(545, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 22;
            this.label4.Text = "用户名：";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(616, 10);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(100, 21);
            this.txtUserId.TabIndex = 21;
            this.txtUserId.Text = "sfx";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(722, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 14);
            this.label5.TabIndex = 24;
            this.label5.Text = "密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(780, 15);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 21);
            this.txtPassword.TabIndex = 23;
            this.txtPassword.Text = "371482";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(33, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1147, 511);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.sqlText);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1139, 485);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 235);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(850, 242);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // sqlText
            // 
           // this.sqlText.FoldingStrategy = null;
            this.sqlText.Font = new System.Drawing.Font("Courier New", 10F);
            this.sqlText.Location = new System.Drawing.Point(19, 6);
            this.sqlText.Name = "sqlText";
            this.sqlText.ShowVRuler = false;
            this.sqlText.Size = new System.Drawing.Size(824, 197);
            this.sqlText.SyntaxHighlighting = "SQL";
            this.sqlText.TabIndex = 14;
            this.sqlText.Text = "select  sysdate from dual";
            this.sqlText.Load += new System.EventHandler(this.sqlText_Load);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rbResult);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1139, 485);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "验证结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rbResult
            // 
            this.rbResult.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbResult.ForeColor = System.Drawing.Color.Blue;
            this.rbResult.Location = new System.Drawing.Point(25, 32);
            this.rbResult.Name = "rbResult";
            this.rbResult.Size = new System.Drawing.Size(1094, 422);
            this.rbResult.TabIndex = 0;
            this.rbResult.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rbBatchResult);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Controls.Add(this.lblxlsInfo);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1139, 485);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "批量验证结果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rbBatchResult
            // 
            this.rbBatchResult.Location = new System.Drawing.Point(922, 64);
            this.rbBatchResult.Name = "rbBatchResult";
            this.rbBatchResult.Size = new System.Drawing.Size(168, 376);
            this.rbBatchResult.TabIndex = 28;
            this.rbBatchResult.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1000, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 40);
            this.button2.TabIndex = 27;
            this.button2.Text = "导出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(50, 52);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(824, 416);
            this.dataGridView2.TabIndex = 1;
            // 
            // lblxlsInfo
            // 
            this.lblxlsInfo.AutoSize = true;
            this.lblxlsInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblxlsInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblxlsInfo.Location = new System.Drawing.Point(66, 23);
            this.lblxlsInfo.Name = "lblxlsInfo";
            this.lblxlsInfo.Size = new System.Drawing.Size(142, 14);
            this.lblxlsInfo.TabIndex = 0;
            this.lblxlsInfo.Text = "批量数据文件信息：";
            // 
            // 验证
            // 
            this.验证.Location = new System.Drawing.Point(1047, -1);
            this.验证.Name = "验证";
            this.验证.Size = new System.Drawing.Size(133, 55);
            this.验证.TabIndex = 26;
            this.验证.Text = "验证";
            this.验证.UseVisualStyleBackColor = true;
            this.验证.Click += new System.EventHandler(this.验证_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1186, 105);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 55);
            this.button3.TabIndex = 28;
            this.button3.Text = "批量验证";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "EXCEL文件|*.xls;*.xlsx";
            // 
            // cb_sysdba
            // 
            this.cb_sysdba.AutoSize = true;
            this.cb_sysdba.ForeColor = System.Drawing.Color.Blue;
            this.cb_sysdba.Location = new System.Drawing.Point(616, 51);
            this.cb_sysdba.Name = "cb_sysdba";
            this.cb_sysdba.Size = new System.Drawing.Size(78, 16);
            this.cb_sysdba.TabIndex = 29;
            this.cb_sysdba.Text = "AS SYSDBA";
            this.cb_sysdba.UseVisualStyleBackColor = true;
            this.cb_sysdba.CheckedChanged += new System.EventHandler(this.cb_sysdba_CheckedChanged);
            // 
            // cb_client
            // 
            this.cb_client.AutoSize = true;
            this.cb_client.ForeColor = System.Drawing.Color.Blue;
            this.cb_client.Location = new System.Drawing.Point(764, 51);
            this.cb_client.Name = "cb_client";
            this.cb_client.Size = new System.Drawing.Size(132, 16);
            this.cb_client.TabIndex = 30;
            this.cb_client.Text = "用oracle客户端驱动";
            this.cb_client.UseVisualStyleBackColor = true;
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 580);
            this.Controls.Add(this.cb_client);
            this.Controls.Add(this.cb_sysdba);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.验证);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtServiceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueryForm";
            this.Text = "oracle规范验证辅助工具-20180925";
            this.Load += new System.EventHandler(this.QueryForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private ICSharpCode.TextEditor.TextEditorControlEx sqlText;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rbResult;
        private System.Windows.Forms.Button 验证;
        private System.Windows.Forms.Button button3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblxlsInfo;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.RichTextBox rbBatchResult;
        private System.Windows.Forms.CheckBox cb_sysdba;
        private System.Windows.Forms.CheckBox cb_client;
    }
}