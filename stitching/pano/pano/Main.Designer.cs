namespace pano
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.zgjidhDirektori = new System.Windows.Forms.FolderBrowserDialog();
            this.folderPath = new System.Windows.Forms.Button();
            this.initPto = new System.Windows.Forms.OpenFileDialog();
            this.initPtoBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.totFotos = new System.Windows.Forms.Label();
            this.initPathLabel = new System.Windows.Forms.TextBox();
            this.initPtoLabel = new System.Windows.Forms.TextBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label2 = new System.Windows.Forms.Label();
            this.fotombetura = new System.Windows.Forms.Label();
            this.panotoolsBtn = new System.Windows.Forms.Button();
            this.panotoolsdir = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.watermark = new System.Windows.Forms.TextBox();
            this.watermarkBtn = new System.Windows.Forms.Button();
            this.watermarkDialog = new System.Windows.Forms.OpenFileDialog();
            this.huginDir = new System.Windows.Forms.TextBox();
            this.huginBtn = new System.Windows.Forms.Button();
            this.imageMagickDir = new System.Windows.Forms.TextBox();
            this.imageMagickBtn = new System.Windows.Forms.Button();
            this.track = new System.Windows.Forms.TextBox();
            this.trackBtn = new System.Windows.Forms.Button();
            this.trackDialog = new System.Windows.Forms.OpenFileDialog();
            this.fotoperfunduara = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.initID = new System.Windows.Forms.TextBox();
            this.pitchcheck = new System.Windows.Forms.CheckBox();
            this.status = new System.Windows.Forms.RichTextBox();
            this.ndalBtn = new System.Windows.Forms.Button();
            this.instruksione = new System.Windows.Forms.Label();
            this.mode1 = new System.Windows.Forms.RadioButton();
            this.mode2 = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zgjidhDirektori
            // 
            this.zgjidhDirektori.SelectedPath = "C:\\";
            // 
            // folderPath
            // 
            this.folderPath.BackColor = System.Drawing.Color.Firebrick;
            this.folderPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.folderPath.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.folderPath.Location = new System.Drawing.Point(12, 33);
            this.folderPath.Name = "folderPath";
            this.folderPath.Size = new System.Drawing.Size(173, 23);
            this.folderPath.TabIndex = 1;
            this.folderPath.Text = "Zgjidh direktorine fillestare";
            this.folderPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.folderPath.UseVisualStyleBackColor = false;
            this.folderPath.Click += new System.EventHandler(this.folderPath_Click);
            // 
            // initPto
            // 
            this.initPto.FileName = "pano.pto";
            // 
            // initPtoBtn
            // 
            this.initPtoBtn.Location = new System.Drawing.Point(12, 109);
            this.initPtoBtn.Name = "initPtoBtn";
            this.initPtoBtn.Size = new System.Drawing.Size(172, 23);
            this.initPtoBtn.TabIndex = 4;
            this.initPtoBtn.Text = "Zgjidh pto fillestare";
            this.initPtoBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.initPtoBtn.UseVisualStyleBackColor = true;
            this.initPtoBtn.Click += new System.EventHandler(this.initPtoBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 484);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Totali:";
            // 
            // totFotos
            // 
            this.totFotos.AutoSize = true;
            this.totFotos.Location = new System.Drawing.Point(75, 484);
            this.totFotos.Name = "totFotos";
            this.totFotos.Size = new System.Drawing.Size(0, 13);
            this.totFotos.TabIndex = 7;
            // 
            // initPathLabel
            // 
            this.initPathLabel.Location = new System.Drawing.Point(191, 35);
            this.initPathLabel.Name = "initPathLabel";
            this.initPathLabel.Size = new System.Drawing.Size(316, 20);
            this.initPathLabel.TabIndex = 8;
            // 
            // initPtoLabel
            // 
            this.initPtoLabel.Location = new System.Drawing.Point(191, 109);
            this.initPtoLabel.Name = "initPtoLabel";
            this.initPtoLabel.Size = new System.Drawing.Size(316, 20);
            this.initPtoLabel.TabIndex = 9;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 530);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Foto te mbetura:";
            // 
            // fotombetura
            // 
            this.fotombetura.AutoSize = true;
            this.fotombetura.Location = new System.Drawing.Point(125, 530);
            this.fotombetura.Name = "fotombetura";
            this.fotombetura.Size = new System.Drawing.Size(0, 13);
            this.fotombetura.TabIndex = 14;
            // 
            // panotoolsBtn
            // 
            this.panotoolsBtn.Location = new System.Drawing.Point(11, 222);
            this.panotoolsBtn.Name = "panotoolsBtn";
            this.panotoolsBtn.Size = new System.Drawing.Size(172, 23);
            this.panotoolsBtn.TabIndex = 15;
            this.panotoolsBtn.Text = "Krpanotools Path";
            this.panotoolsBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.panotoolsBtn.UseVisualStyleBackColor = true;
            this.panotoolsBtn.Click += new System.EventHandler(this.panotoolsBtn_Click);
            // 
            // panotoolsdir
            // 
            this.panotoolsdir.Location = new System.Drawing.Point(190, 221);
            this.panotoolsdir.Name = "panotoolsdir";
            this.panotoolsdir.Size = new System.Drawing.Size(316, 20);
            this.panotoolsdir.TabIndex = 16;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(528, 35);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(156, 50);
            this.start.TabIndex = 17;
            this.start.Text = "Fillo";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // watermark
            // 
            this.watermark.Location = new System.Drawing.Point(190, 259);
            this.watermark.Name = "watermark";
            this.watermark.Size = new System.Drawing.Size(316, 20);
            this.watermark.TabIndex = 19;
            // 
            // watermarkBtn
            // 
            this.watermarkBtn.Location = new System.Drawing.Point(11, 260);
            this.watermarkBtn.Name = "watermarkBtn";
            this.watermarkBtn.Size = new System.Drawing.Size(172, 23);
            this.watermarkBtn.TabIndex = 18;
            this.watermarkBtn.Text = "Imazhi Watermark";
            this.watermarkBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.watermarkBtn.UseVisualStyleBackColor = true;
            this.watermarkBtn.Click += new System.EventHandler(this.watermarkBtn_Click);
            // 
            // watermarkDialog
            // 
            this.watermarkDialog.FileName = "watermark.png";
            // 
            // huginDir
            // 
            this.huginDir.Location = new System.Drawing.Point(190, 147);
            this.huginDir.Name = "huginDir";
            this.huginDir.Size = new System.Drawing.Size(316, 20);
            this.huginDir.TabIndex = 22;
            // 
            // huginBtn
            // 
            this.huginBtn.Location = new System.Drawing.Point(11, 148);
            this.huginBtn.Name = "huginBtn";
            this.huginBtn.Size = new System.Drawing.Size(172, 23);
            this.huginBtn.TabIndex = 21;
            this.huginBtn.Text = "Hugin Path";
            this.huginBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.huginBtn.UseVisualStyleBackColor = true;
            this.huginBtn.Click += new System.EventHandler(this.huginBtn_Click);
            // 
            // imageMagickDir
            // 
            this.imageMagickDir.Location = new System.Drawing.Point(190, 184);
            this.imageMagickDir.Name = "imageMagickDir";
            this.imageMagickDir.Size = new System.Drawing.Size(316, 20);
            this.imageMagickDir.TabIndex = 24;
            // 
            // imageMagickBtn
            // 
            this.imageMagickBtn.Location = new System.Drawing.Point(11, 185);
            this.imageMagickBtn.Name = "imageMagickBtn";
            this.imageMagickBtn.Size = new System.Drawing.Size(172, 23);
            this.imageMagickBtn.TabIndex = 23;
            this.imageMagickBtn.Text = "ImageMagick Path";
            this.imageMagickBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imageMagickBtn.UseVisualStyleBackColor = true;
            this.imageMagickBtn.Click += new System.EventHandler(this.imageMagickBtn_Click);
            // 
            // track
            // 
            this.track.Location = new System.Drawing.Point(191, 296);
            this.track.Name = "track";
            this.track.Size = new System.Drawing.Size(316, 20);
            this.track.TabIndex = 26;
            // 
            // trackBtn
            // 
            this.trackBtn.Location = new System.Drawing.Point(12, 297);
            this.trackBtn.Name = "trackBtn";
            this.trackBtn.Size = new System.Drawing.Size(172, 23);
            this.trackBtn.TabIndex = 25;
            this.trackBtn.Text = "Track.txt";
            this.trackBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trackBtn.UseVisualStyleBackColor = true;
            this.trackBtn.Click += new System.EventHandler(this.trackBtn_Click);
            // 
            // trackDialog
            // 
            this.trackDialog.FileName = "track.txt";
            // 
            // fotoperfunduara
            // 
            this.fotoperfunduara.AutoSize = true;
            this.fotoperfunduara.Location = new System.Drawing.Point(125, 508);
            this.fotoperfunduara.Name = "fotoperfunduara";
            this.fotoperfunduara.Size = new System.Drawing.Size(0, 13);
            this.fotoperfunduara.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 508);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Foto te perfunduara:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 556);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Foto e fundit e perpunuar:";
            // 
            // initID
            // 
            this.initID.Location = new System.Drawing.Point(190, 553);
            this.initID.Name = "initID";
            this.initID.Size = new System.Drawing.Size(317, 20);
            this.initID.TabIndex = 30;
            this.initID.TextChanged += new System.EventHandler(this.initID_TextChanged);
            // 
            // pitchcheck
            // 
            this.pitchcheck.AutoSize = true;
            this.pitchcheck.Location = new System.Drawing.Point(16, 341);
            this.pitchcheck.Name = "pitchcheck";
            this.pitchcheck.Size = new System.Drawing.Size(108, 17);
            this.pitchcheck.TabIndex = 32;
            this.pitchcheck.Text = "Korigjo Pitch/Roll";
            this.pitchcheck.UseVisualStyleBackColor = true;
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.LightGray;
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.status.Location = new System.Drawing.Point(528, 112);
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(343, 461);
            this.status.TabIndex = 39;
            this.status.Text = "STATUS:";
            // 
            // ndalBtn
            // 
            this.ndalBtn.Location = new System.Drawing.Point(715, 35);
            this.ndalBtn.Name = "ndalBtn";
            this.ndalBtn.Size = new System.Drawing.Size(156, 50);
            this.ndalBtn.TabIndex = 42;
            this.ndalBtn.Text = "Ndalo";
            this.ndalBtn.UseVisualStyleBackColor = true;
            this.ndalBtn.Click += new System.EventHandler(this.ndalBtn_Click);
            // 
            // instruksione
            // 
            this.instruksione.AutoSize = true;
            this.instruksione.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instruksione.Location = new System.Drawing.Point(12, 72);
            this.instruksione.Name = "instruksione";
            this.instruksione.Size = new System.Drawing.Size(67, 13);
            this.instruksione.TabIndex = 43;
            this.instruksione.Text = "Instruksione:";
            this.instruksione.Click += new System.EventHandler(this.label5_Click);
            // 
            // mode1
            // 
            this.mode1.AutoSize = true;
            this.mode1.Checked = true;
            this.mode1.Location = new System.Drawing.Point(15, 383);
            this.mode1.Name = "mode1";
            this.mode1.Size = new System.Drawing.Size(105, 17);
            this.mode1.TabIndex = 44;
            this.mode1.TabStop = true;
            this.mode1.Text = "Multi Pano Mode";
            this.mode1.UseVisualStyleBackColor = true;
            this.mode1.CheckedChanged += new System.EventHandler(this.mode1_CheckedChanged);
            // 
            // mode2
            // 
            this.mode2.AutoSize = true;
            this.mode2.Location = new System.Drawing.Point(178, 383);
            this.mode2.Name = "mode2";
            this.mode2.Size = new System.Drawing.Size(112, 17);
            this.mode2.TabIndex = 45;
            this.mode2.Text = "Single Pano Mode";
            this.mode2.UseVisualStyleBackColor = true;
            this.mode2.CheckedChanged += new System.EventHandler(this.mode2_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 650);
            this.Controls.Add(this.mode2);
            this.Controls.Add(this.mode1);
            this.Controls.Add(this.instruksione);
            this.Controls.Add(this.ndalBtn);
            this.Controls.Add(this.status);
            this.Controls.Add(this.pitchcheck);
            this.Controls.Add(this.initID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fotoperfunduara);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.track);
            this.Controls.Add(this.trackBtn);
            this.Controls.Add(this.imageMagickDir);
            this.Controls.Add(this.imageMagickBtn);
            this.Controls.Add(this.huginDir);
            this.Controls.Add(this.huginBtn);
            this.Controls.Add(this.watermark);
            this.Controls.Add(this.watermarkBtn);
            this.Controls.Add(this.start);
            this.Controls.Add(this.panotoolsdir);
            this.Controls.Add(this.panotoolsBtn);
            this.Controls.Add(this.fotombetura);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.initPtoLabel);
            this.Controls.Add(this.initPathLabel);
            this.Controls.Add(this.totFotos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.initPtoBtn);
            this.Controls.Add(this.folderPath);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "albumi.com";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog zgjidhDirektori;
        private System.Windows.Forms.Button folderPath;
        private System.Windows.Forms.OpenFileDialog initPto;
        private System.Windows.Forms.Button initPtoBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label totFotos;
        private System.Windows.Forms.TextBox initPathLabel;
        private System.Windows.Forms.TextBox initPtoLabel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label fotombetura;
        private System.Windows.Forms.Button panotoolsBtn;
        private System.Windows.Forms.TextBox panotoolsdir;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.TextBox watermark;
        private System.Windows.Forms.Button watermarkBtn;
        private System.Windows.Forms.OpenFileDialog watermarkDialog;
        private System.Windows.Forms.TextBox huginDir;
        private System.Windows.Forms.Button huginBtn;
        private System.Windows.Forms.TextBox imageMagickDir;
        private System.Windows.Forms.Button imageMagickBtn;
        private System.Windows.Forms.TextBox track;
        private System.Windows.Forms.Button trackBtn;
        private System.Windows.Forms.OpenFileDialog trackDialog;
        private System.Windows.Forms.Label fotoperfunduara;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox initID;
        private System.Windows.Forms.CheckBox pitchcheck;
        private System.Windows.Forms.RichTextBox status;
        private System.Windows.Forms.Button ndalBtn;
        private System.Windows.Forms.Label instruksione;
        private System.Windows.Forms.RadioButton mode1;
        private System.Windows.Forms.RadioButton mode2;


    }
}

