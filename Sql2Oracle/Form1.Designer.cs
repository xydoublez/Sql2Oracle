namespace Sql2Oracle
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.sqlConnStr = new System.Windows.Forms.TextBox();
            this.oracleStr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.batchNumber = new System.Windows.Forms.TextBox();
            this.sqlText = new ICSharpCode.TextEditor.TextEditorControlEx();
            this.label5 = new System.Windows.Forms.Label();
            this.chkClob = new System.Windows.Forms.CheckBox();
            this.rbResult = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtThreadCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(761, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 53);
            this.button1.TabIndex = 0;
            this.button1.Text = "导入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sqlConnStr
            // 
            this.sqlConnStr.Location = new System.Drawing.Point(12, 12);
            this.sqlConnStr.Name = "sqlConnStr";
            this.sqlConnStr.Size = new System.Drawing.Size(552, 21);
            this.sqlConnStr.TabIndex = 2;
            this.sqlConnStr.Text = "Data Source=10.68.4.31;Initial Catalog=comm;Persist Security Info=True;User ID=sa" +
    ";Password=Sfx371482";
            // 
            // oracleStr
            // 
            this.oracleStr.Location = new System.Drawing.Point(12, 59);
            this.oracleStr.Name = "oracleStr";
            this.oracleStr.Size = new System.Drawing.Size(552, 21);
            this.oracleStr.TabIndex = 3;
            this.oracleStr.Text = resources.GetString("oracleStr.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(570, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "SQLSERVER源数源";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(570, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "目标数源ORACLE";
            // 
            // tableName
            // 
            this.tableName.Location = new System.Drawing.Point(93, 36);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(125, 21);
            this.tableName.TabIndex = 6;
            this.tableName.Text = "out_pats";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(12, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "表名";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(221, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "批处理数量";
            // 
            // batchNumber
            // 
            this.batchNumber.Location = new System.Drawing.Point(362, 32);
            this.batchNumber.Name = "batchNumber";
            this.batchNumber.Size = new System.Drawing.Size(125, 21);
            this.batchNumber.TabIndex = 8;
            this.batchNumber.Text = "10000";
            // 
            // sqlText
            // 
            //this.sqlText.FoldingStrategy = null;
            this.sqlText.Font = new System.Drawing.Font("Courier New", 10F);
            this.sqlText.Location = new System.Drawing.Point(12, 100);
            this.sqlText.Name = "sqlText";
            this.sqlText.ShowVRuler = false;
            this.sqlText.Size = new System.Drawing.Size(824, 397);
            this.sqlText.SyntaxHighlighting = "SQL";
            this.sqlText.TabIndex = 10;
            this.sqlText.Text = "select * from comm.dict.out_pats";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(29, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(475, 24);
            this.label5.TabIndex = 11;
            this.label5.Text = "注意：超长字段，请再字段名后追加_CLOB";
            // 
            // chkClob
            // 
            this.chkClob.AutoSize = true;
            this.chkClob.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkClob.ForeColor = System.Drawing.Color.Blue;
            this.chkClob.Location = new System.Drawing.Point(511, 80);
            this.chkClob.Name = "chkClob";
            this.chkClob.Size = new System.Drawing.Size(97, 20);
            this.chkClob.TabIndex = 12;
            this.chkClob.Text = "CLOB模式";
            this.chkClob.UseVisualStyleBackColor = true;
            // 
            // rbResult
            // 
            this.rbResult.Location = new System.Drawing.Point(894, 59);
            this.rbResult.Name = "rbResult";
            this.rbResult.ReadOnly = true;
            this.rbResult.Size = new System.Drawing.Size(202, 418);
            this.rbResult.TabIndex = 13;
            this.rbResult.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(929, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 53);
            this.button2.TabIndex = 14;
            this.button2.Text = "清除日志";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(504, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 24);
            this.label6.TabIndex = 16;
            this.label6.Text = "最大线程数";
            // 
            // txtThreadCount
            // 
            this.txtThreadCount.Location = new System.Drawing.Point(660, 35);
            this.txtThreadCount.Name = "txtThreadCount";
            this.txtThreadCount.Size = new System.Drawing.Size(58, 21);
            this.txtThreadCount.TabIndex = 15;
            this.txtThreadCount.Text = "100";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 509);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtThreadCount);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rbResult);
            this.Controls.Add(this.chkClob);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sqlText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.batchNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.oracleStr);
            this.Controls.Add(this.sqlConnStr);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Sql2Oracle";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox sqlConnStr;
        private System.Windows.Forms.TextBox oracleStr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox batchNumber;
        private ICSharpCode.TextEditor.TextEditorControlEx sqlText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkClob;
        private System.Windows.Forms.RichTextBox rbResult;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtThreadCount;
    }
}

