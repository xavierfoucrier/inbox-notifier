namespace notifier {
	partial class Main {
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.separator = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPagePreferences = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.fieldStepDelay = new System.Windows.Forms.ComboBox();
			this.fieldAudioNotification = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.fieldNumericDelay = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.fieldStartWithWindows = new System.Windows.Forms.CheckBox();
			this.fieldMinimizeToSystray = new System.Windows.Forms.CheckBox();
			this.fieldAskonExit = new System.Windows.Forms.CheckBox();
			this.tabPageAbout = new System.Windows.Forms.TabPage();
			this.linkWebsiteYusuke = new System.Windows.Forms.LinkLabel();
			this.label10 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.labelSettingsSaved = new System.Windows.Forms.Label();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.tabControl.SuspendLayout();
			this.tabPagePreferences.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tabPageAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// separator
			// 
			this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.separator.Location = new System.Drawing.Point(12, 268);
			this.separator.Name = "separator";
			this.separator.Size = new System.Drawing.Size(540, 2);
			this.separator.TabIndex = 22;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(477, 281);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 19;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPagePreferences);
			this.tabControl.Controls.Add(this.tabPageAbout);
			this.tabControl.Location = new System.Drawing.Point(12, 83);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(540, 171);
			this.tabControl.TabIndex = 18;
			// 
			// tabPagePreferences
			// 
			this.tabPagePreferences.Controls.Add(this.groupBox2);
			this.tabPagePreferences.Controls.Add(this.groupBox1);
			this.tabPagePreferences.Location = new System.Drawing.Point(4, 22);
			this.tabPagePreferences.Name = "tabPagePreferences";
			this.tabPagePreferences.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePreferences.Size = new System.Drawing.Size(532, 145);
			this.tabPagePreferences.TabIndex = 1;
			this.tabPagePreferences.Text = "Général";
			this.tabPagePreferences.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.fieldStepDelay);
			this.groupBox2.Controls.Add(this.fieldAudioNotification);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.fieldNumericDelay);
			this.groupBox2.Location = new System.Drawing.Point(269, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(257, 133);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Emails";
			// 
			// fieldStepDelay
			// 
			this.fieldStepDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::notifier.Properties.Settings.Default, "StepDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldStepDelay.DisplayMember = "minute(s)";
			this.fieldStepDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldStepDelay.Enabled = false;
			this.fieldStepDelay.FormattingEnabled = true;
			this.fieldStepDelay.Items.AddRange(new object[] {
            "minute(s)",
            "heure(s)"});
			this.fieldStepDelay.Location = new System.Drawing.Point(171, 24);
			this.fieldStepDelay.Name = "fieldStepDelay";
			this.fieldStepDelay.Size = new System.Drawing.Size(73, 21);
			this.fieldStepDelay.TabIndex = 2;
			this.fieldStepDelay.Text = global::notifier.Properties.Settings.Default.StepDelay;
			this.fieldStepDelay.ValueMember = "minute(s)";
			// 
			// fieldAudioNotification
			// 
			this.fieldAudioNotification.AutoSize = true;
			this.fieldAudioNotification.Checked = global::notifier.Properties.Settings.Default.AudioNotification;
			this.fieldAudioNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAudioNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AudioNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAudioNotification.Location = new System.Drawing.Point(17, 71);
			this.fieldAudioNotification.Name = "fieldAudioNotification";
			this.fieldAudioNotification.Size = new System.Drawing.Size(233, 17);
			this.fieldAudioNotification.TabIndex = 0;
			this.fieldAudioNotification.Text = "Notification sonore à la réception des emails";
			this.fieldAudioNotification.UseVisualStyleBackColor = true;
			this.fieldAudioNotification.CheckedChanged += new System.EventHandler(this.fieldAudioNotification_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Délai de vérification :";
			// 
			// fieldNumericDelay
			// 
			this.fieldNumericDelay.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::notifier.Properties.Settings.Default, "NumericDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldNumericDelay.Enabled = false;
			this.fieldNumericDelay.Location = new System.Drawing.Point(124, 24);
			this.fieldNumericDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.fieldNumericDelay.Name = "fieldNumericDelay";
			this.fieldNumericDelay.Size = new System.Drawing.Size(41, 20);
			this.fieldNumericDelay.TabIndex = 0;
			this.fieldNumericDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.fieldNumericDelay.Value = global::notifier.Properties.Settings.Default.NumericDelay;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.fieldStartWithWindows);
			this.groupBox1.Controls.Add(this.fieldMinimizeToSystray);
			this.groupBox1.Controls.Add(this.fieldAskonExit);
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(257, 133);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Comportement";
			// 
			// fieldStartWithWindows
			// 
			this.fieldStartWithWindows.AutoSize = true;
			this.fieldStartWithWindows.Checked = global::notifier.Properties.Settings.Default.StartWithWindows;
			this.fieldStartWithWindows.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldStartWithWindows.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "StartWithWindows", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldStartWithWindows.Enabled = false;
			this.fieldStartWithWindows.Location = new System.Drawing.Point(15, 25);
			this.fieldStartWithWindows.Name = "fieldStartWithWindows";
			this.fieldStartWithWindows.Size = new System.Drawing.Size(189, 17);
			this.fieldStartWithWindows.TabIndex = 0;
			this.fieldStartWithWindows.Text = "Lancer au démarrage de Windows";
			this.fieldStartWithWindows.UseVisualStyleBackColor = true;
			// 
			// fieldMinimizeToSystray
			// 
			this.fieldMinimizeToSystray.AutoSize = true;
			this.fieldMinimizeToSystray.Checked = true;
			this.fieldMinimizeToSystray.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.fieldMinimizeToSystray.Enabled = false;
			this.fieldMinimizeToSystray.Location = new System.Drawing.Point(15, 48);
			this.fieldMinimizeToSystray.Name = "fieldMinimizeToSystray";
			this.fieldMinimizeToSystray.Size = new System.Drawing.Size(195, 17);
			this.fieldMinimizeToSystray.TabIndex = 0;
			this.fieldMinimizeToSystray.Text = "Réduire dans la zone de notification";
			this.fieldMinimizeToSystray.UseVisualStyleBackColor = true;
			// 
			// fieldAskonExit
			// 
			this.fieldAskonExit.AutoSize = true;
			this.fieldAskonExit.Checked = global::notifier.Properties.Settings.Default.AskonExit;
			this.fieldAskonExit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAskonExit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AskonExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAskonExit.Location = new System.Drawing.Point(15, 71);
			this.fieldAskonExit.Name = "fieldAskonExit";
			this.fieldAskonExit.Size = new System.Drawing.Size(226, 17);
			this.fieldAskonExit.TabIndex = 2;
			this.fieldAskonExit.Text = "Me demander avant de quitter l\'application";
			this.fieldAskonExit.UseVisualStyleBackColor = true;
			this.fieldAskonExit.CheckedChanged += new System.EventHandler(this.fieldAskonExit_CheckedChanged);
			// 
			// tabPageAbout
			// 
			this.tabPageAbout.Controls.Add(this.linkWebsiteYusuke);
			this.tabPageAbout.Controls.Add(this.label10);
			this.tabPageAbout.Controls.Add(this.label7);
			this.tabPageAbout.Controls.Add(this.label11);
			this.tabPageAbout.Controls.Add(this.label12);
			this.tabPageAbout.Controls.Add(this.label13);
			this.tabPageAbout.Controls.Add(this.labelVersion);
			this.tabPageAbout.Controls.Add(this.label14);
			this.tabPageAbout.Location = new System.Drawing.Point(4, 22);
			this.tabPageAbout.Name = "tabPageAbout";
			this.tabPageAbout.Size = new System.Drawing.Size(532, 145);
			this.tabPageAbout.TabIndex = 2;
			this.tabPageAbout.Text = "À propos";
			this.tabPageAbout.UseVisualStyleBackColor = true;
			// 
			// linkWebsiteYusuke
			// 
			this.linkWebsiteYusuke.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteYusuke.AutoSize = true;
			this.linkWebsiteYusuke.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteYusuke.Location = new System.Drawing.Point(264, 39);
			this.linkWebsiteYusuke.Name = "linkWebsiteYusuke";
			this.linkWebsiteYusuke.Size = new System.Drawing.Size(132, 13);
			this.linkWebsiteYusuke.TabIndex = 25;
			this.linkWebsiteYusuke.TabStop = true;
			this.linkWebsiteYusuke.Text = "p.yusukekamiyamane.com";
			this.linkWebsiteYusuke.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(264, 97);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(93, 26);
			this.label10.TabIndex = 24;
			this.label10.Text = "© 2016\r\nAll rights reserved.";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(25, 97);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(168, 26);
			this.label7.TabIndex = 21;
			this.label7.Text = "Visual Studio 2013, C#\r\nDéveloppé par Xavier FOUCRIER";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(264, 79);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(60, 13);
			this.label11.TabIndex = 15;
			this.label11.Text = "Copyright";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(264, 21);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(46, 13);
			this.label12.TabIndex = 16;
			this.label12.Text = "Crédits";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(25, 79);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(95, 13);
			this.label13.TabIndex = 17;
			this.label13.Text = "Développement";
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(25, 39);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(10, 13);
			this.labelVersion.TabIndex = 18;
			this.labelVersion.Text = "-";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(25, 21);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(49, 13);
			this.label14.TabIndex = 19;
			this.label14.Text = "Version";
			// 
			// labelSettingsSaved
			// 
			this.labelSettingsSaved.AutoSize = true;
			this.labelSettingsSaved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.labelSettingsSaved.Location = new System.Drawing.Point(12, 286);
			this.labelSettingsSaved.Name = "labelSettingsSaved";
			this.labelSettingsSaved.Size = new System.Drawing.Size(199, 13);
			this.labelSettingsSaved.TabIndex = 21;
			this.labelSettingsSaved.Text = "Préférences d\'application sauvegardées.";
			this.labelSettingsSaved.Visible = false;
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "Synchronisation en cours ...";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::notifier.Properties.Resources.header;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(564, 70);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 23;
			this.pictureBox1.TabStop = false;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettings,
            this.menuItem2,
            this.menuItemExit});
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 0;
			this.menuItemSettings.Text = "Préférences";
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Text = "Quitter";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 316);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.separator);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.labelSettingsSaved);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Main";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Préférences";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.Load += new System.EventHandler(this.Main_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPagePreferences.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageAbout.ResumeLayout(false);
			this.tabPageAbout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label separator;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPagePreferences;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox fieldStepDelay;
		private System.Windows.Forms.CheckBox fieldAudioNotification;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown fieldNumericDelay;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox fieldStartWithWindows;
		private System.Windows.Forms.CheckBox fieldMinimizeToSystray;
		private System.Windows.Forms.CheckBox fieldAskonExit;
		private System.Windows.Forms.TabPage tabPageAbout;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label labelSettingsSaved;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.LinkLabel linkWebsiteYusuke;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemSettings;
	}
}

