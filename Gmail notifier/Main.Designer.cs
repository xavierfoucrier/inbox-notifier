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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.fieldStartWithWindows = new System.Windows.Forms.CheckBox();
			this.fieldMinimizeToSystray = new System.Windows.Forms.CheckBox();
			this.fieldAskonExit = new System.Windows.Forms.CheckBox();
			this.tabPageNotification = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.fieldNetworkConnectivityNotification = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.fieldStepDelay = new System.Windows.Forms.ComboBox();
			this.fieldSpamNotification = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.fieldNumericDelay = new System.Windows.Forms.NumericUpDown();
			this.fieldNotification = new System.Windows.Forms.CheckBox();
			this.fieldAudioNotification = new System.Windows.Forms.CheckBox();
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
			this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemSynchronize = new System.Windows.Forms.MenuItem();
			this.menuItemMarkAsRead = new System.Windows.Forms.MenuItem();
			this.menuItemTimout = new System.Windows.Forms.MenuItem();
			this.menuItemTimeoutDisabled = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout30m = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout1h = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout2h = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout5h = new System.Windows.Forms.MenuItem();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.tabPagePrivacy = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.fieldPrivacyNotificationNone = new System.Windows.Forms.RadioButton();
			this.fieldPrivacyNotificationShort = new System.Windows.Forms.RadioButton();
			this.fieldPrivacyNotificationAll = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabPagePreferences.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageNotification.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).BeginInit();
			this.tabPageAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
			this.tabPagePrivacy.SuspendLayout();
			this.groupBox4.SuspendLayout();
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
			this.tabControl.Controls.Add(this.tabPageNotification);
			this.tabControl.Controls.Add(this.tabPagePrivacy);
			this.tabControl.Controls.Add(this.tabPageAbout);
			this.tabControl.Location = new System.Drawing.Point(12, 83);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(540, 171);
			this.tabControl.TabIndex = 18;
			this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
			// 
			// tabPagePreferences
			// 
			this.tabPagePreferences.Controls.Add(this.groupBox1);
			this.tabPagePreferences.Location = new System.Drawing.Point(4, 22);
			this.tabPagePreferences.Name = "tabPagePreferences";
			this.tabPagePreferences.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePreferences.Size = new System.Drawing.Size(532, 145);
			this.tabPagePreferences.TabIndex = 1;
			this.tabPagePreferences.Text = "Général";
			this.tabPagePreferences.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.fieldStartWithWindows);
			this.groupBox1.Controls.Add(this.fieldMinimizeToSystray);
			this.groupBox1.Controls.Add(this.fieldAskonExit);
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(519, 133);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Comportement";
			// 
			// fieldStartWithWindows
			// 
			this.fieldStartWithWindows.AutoSize = true;
			this.fieldStartWithWindows.Checked = global::notifier.Properties.Settings.Default.StartWithWindows;
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
			this.fieldAskonExit.Click += new System.EventHandler(this.fieldAskonExit_Click);
			// 
			// tabPageNotification
			// 
			this.tabPageNotification.Controls.Add(this.groupBox3);
			this.tabPageNotification.Controls.Add(this.groupBox2);
			this.tabPageNotification.Location = new System.Drawing.Point(4, 22);
			this.tabPageNotification.Name = "tabPageNotification";
			this.tabPageNotification.Size = new System.Drawing.Size(532, 145);
			this.tabPageNotification.TabIndex = 3;
			this.tabPageNotification.Text = "Notification";
			this.tabPageNotification.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.fieldNetworkConnectivityNotification);
			this.groupBox3.Location = new System.Drawing.Point(270, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(255, 133);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Réseau";
			// 
			// fieldNetworkConnectivityNotification
			// 
			this.fieldNetworkConnectivityNotification.AutoSize = true;
			this.fieldNetworkConnectivityNotification.Checked = global::notifier.Properties.Settings.Default.NetworkConnectivityNotification;
			this.fieldNetworkConnectivityNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldNetworkConnectivityNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "NetworkConnectivityNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldNetworkConnectivityNotification.Location = new System.Drawing.Point(15, 25);
			this.fieldNetworkConnectivityNotification.Name = "fieldNetworkConnectivityNotification";
			this.fieldNetworkConnectivityNotification.Size = new System.Drawing.Size(162, 17);
			this.fieldNetworkConnectivityNotification.TabIndex = 0;
			this.fieldNetworkConnectivityNotification.Text = "Perte de connectivité réseau";
			this.fieldNetworkConnectivityNotification.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.fieldStepDelay);
			this.groupBox2.Controls.Add(this.fieldSpamNotification);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.fieldNumericDelay);
			this.groupBox2.Controls.Add(this.fieldNotification);
			this.groupBox2.Controls.Add(this.fieldAudioNotification);
			this.groupBox2.Location = new System.Drawing.Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(257, 133);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Courrier";
			// 
			// fieldStepDelay
			// 
			this.fieldStepDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::notifier.Properties.Settings.Default, "StepDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldStepDelay.DisplayMember = "minute(s)";
			this.fieldStepDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldStepDelay.FormattingEnabled = true;
			this.fieldStepDelay.Items.AddRange(new object[] {
            "minute(s)",
            "heure(s)"});
			this.fieldStepDelay.Location = new System.Drawing.Point(171, 101);
			this.fieldStepDelay.Name = "fieldStepDelay";
			this.fieldStepDelay.Size = new System.Drawing.Size(73, 21);
			this.fieldStepDelay.TabIndex = 2;
			this.fieldStepDelay.ValueMember = "minute(s)";
			// 
			// fieldSpamNotification
			// 
			this.fieldSpamNotification.AutoSize = true;
			this.fieldSpamNotification.Checked = global::notifier.Properties.Settings.Default.SpamNotification;
			this.fieldSpamNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldSpamNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "SpamNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldSpamNotification.Location = new System.Drawing.Point(15, 71);
			this.fieldSpamNotification.Name = "fieldSpamNotification";
			this.fieldSpamNotification.Size = new System.Drawing.Size(154, 17);
			this.fieldSpamNotification.TabIndex = 0;
			this.fieldSpamNotification.Text = "Courrier indésirable (SPAM)";
			this.fieldSpamNotification.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 104);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Délai de vérification :";
			// 
			// fieldNumericDelay
			// 
			this.fieldNumericDelay.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::notifier.Properties.Settings.Default, "NumericDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldNumericDelay.Location = new System.Drawing.Point(124, 101);
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
			// fieldNotification
			// 
			this.fieldNotification.AutoSize = true;
			this.fieldNotification.Checked = true;
			this.fieldNotification.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.fieldNotification.Enabled = false;
			this.fieldNotification.Location = new System.Drawing.Point(15, 25);
			this.fieldNotification.Name = "fieldNotification";
			this.fieldNotification.Size = new System.Drawing.Size(115, 17);
			this.fieldNotification.TabIndex = 0;
			this.fieldNotification.Text = "Nouveau message";
			this.fieldNotification.UseVisualStyleBackColor = true;
			// 
			// fieldAudioNotification
			// 
			this.fieldAudioNotification.AutoSize = true;
			this.fieldAudioNotification.Checked = global::notifier.Properties.Settings.Default.AudioNotification;
			this.fieldAudioNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAudioNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AudioNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAudioNotification.Location = new System.Drawing.Point(15, 48);
			this.fieldAudioNotification.Name = "fieldAudioNotification";
			this.fieldAudioNotification.Size = new System.Drawing.Size(114, 17);
			this.fieldAudioNotification.TabIndex = 0;
			this.fieldAudioNotification.Text = "Notification sonore";
			this.fieldAudioNotification.UseVisualStyleBackColor = true;
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
			this.linkWebsiteYusuke.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebsiteYusuke_LinkClicked);
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
			this.notifyIcon.Text = "Démarrage ...";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// pictureBoxHeader
			// 
			this.pictureBoxHeader.Image = global::notifier.Properties.Resources.header;
			this.pictureBoxHeader.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxHeader.Name = "pictureBoxHeader";
			this.pictureBoxHeader.Size = new System.Drawing.Size(564, 70);
			this.pictureBoxHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxHeader.TabIndex = 23;
			this.pictureBoxHeader.TabStop = false;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSynchronize,
            this.menuItemMarkAsRead,
            this.menuItemTimout,
            this.menuItemSettings,
            this.menuItem2,
            this.menuItemExit});
			// 
			// menuItemSynchronize
			// 
			this.menuItemSynchronize.Index = 0;
			this.menuItemSynchronize.Text = "Synchroniser";
			this.menuItemSynchronize.Click += new System.EventHandler(this.menuItemSynchronize_Click);
			// 
			// menuItemMarkAsRead
			// 
			this.menuItemMarkAsRead.Enabled = false;
			this.menuItemMarkAsRead.Index = 1;
			this.menuItemMarkAsRead.Text = "Marquer comme lu(s)";
			this.menuItemMarkAsRead.Click += new System.EventHandler(this.menuItemMarkAsRead_Click);
			// 
			// menuItemTimout
			// 
			this.menuItemTimout.Index = 2;
			this.menuItemTimout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTimeoutDisabled,
            this.menuItem1,
            this.menuItemTimeout30m,
            this.menuItemTimeout1h,
            this.menuItemTimeout2h,
            this.menuItemTimeout5h});
			this.menuItemTimout.Text = "Ne pas déranger";
			// 
			// menuItemTimeoutDisabled
			// 
			this.menuItemTimeoutDisabled.Checked = true;
			this.menuItemTimeoutDisabled.Index = 0;
			this.menuItemTimeoutDisabled.RadioCheck = true;
			this.menuItemTimeoutDisabled.Text = "Désactivé";
			this.menuItemTimeoutDisabled.Click += new System.EventHandler(this.menuItemTimeoutDisabled_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "-";
			// 
			// menuItemTimeout30m
			// 
			this.menuItemTimeout30m.Index = 2;
			this.menuItemTimeout30m.RadioCheck = true;
			this.menuItemTimeout30m.Text = "30 minutes";
			this.menuItemTimeout30m.Click += new System.EventHandler(this.menuItemTimeout30m_Click);
			// 
			// menuItemTimeout1h
			// 
			this.menuItemTimeout1h.Index = 3;
			this.menuItemTimeout1h.RadioCheck = true;
			this.menuItemTimeout1h.Text = "1 heure";
			this.menuItemTimeout1h.Click += new System.EventHandler(this.menuItemTimeout1h_Click);
			// 
			// menuItemTimeout2h
			// 
			this.menuItemTimeout2h.Index = 4;
			this.menuItemTimeout2h.RadioCheck = true;
			this.menuItemTimeout2h.Text = "2 heures";
			this.menuItemTimeout2h.Click += new System.EventHandler(this.menuItemTimeout2h_Click);
			// 
			// menuItemTimeout5h
			// 
			this.menuItemTimeout5h.Index = 5;
			this.menuItemTimeout5h.RadioCheck = true;
			this.menuItemTimeout5h.Text = "5 heures";
			this.menuItemTimeout5h.Click += new System.EventHandler(this.menuItemTimeout5h_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.DefaultItem = true;
			this.menuItemSettings.Index = 3;
			this.menuItemSettings.Text = "Ouvrir";
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 4;
			this.menuItem2.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 5;
			this.menuItemExit.Text = "Quitter";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = global::notifier.Properties.Settings.Default.TimerInterval;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// tabPagePrivacy
			// 
			this.tabPagePrivacy.Controls.Add(this.groupBox4);
			this.tabPagePrivacy.Location = new System.Drawing.Point(4, 22);
			this.tabPagePrivacy.Name = "tabPagePrivacy";
			this.tabPagePrivacy.Size = new System.Drawing.Size(532, 145);
			this.tabPagePrivacy.TabIndex = 4;
			this.tabPagePrivacy.Text = "Confidentialité";
			this.tabPagePrivacy.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationAll);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationShort);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationNone);
			this.groupBox4.Location = new System.Drawing.Point(6, 6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(519, 133);
			this.groupBox4.TabIndex = 4;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Notification";
			// 
			// fieldPrivacyNotificationNone
			// 
			this.fieldPrivacyNotificationNone.AutoSize = true;
			this.fieldPrivacyNotificationNone.Enabled = false;
			this.fieldPrivacyNotificationNone.Location = new System.Drawing.Point(15, 50);
			this.fieldPrivacyNotificationNone.Name = "fieldPrivacyNotificationNone";
			this.fieldPrivacyNotificationNone.Size = new System.Drawing.Size(195, 17);
			this.fieldPrivacyNotificationNone.TabIndex = 1;
			this.fieldPrivacyNotificationNone.TabStop = true;
			this.fieldPrivacyNotificationNone.Text = "Afficher tout le contenu du message";
			this.fieldPrivacyNotificationNone.UseVisualStyleBackColor = true;
			// 
			// fieldPrivacyNotificationShort
			// 
			this.fieldPrivacyNotificationShort.AutoSize = true;
			this.fieldPrivacyNotificationShort.Enabled = false;
			this.fieldPrivacyNotificationShort.Location = new System.Drawing.Point(15, 73);
			this.fieldPrivacyNotificationShort.Name = "fieldPrivacyNotificationShort";
			this.fieldPrivacyNotificationShort.Size = new System.Drawing.Size(228, 17);
			this.fieldPrivacyNotificationShort.TabIndex = 1;
			this.fieldPrivacyNotificationShort.TabStop = true;
			this.fieldPrivacyNotificationShort.Text = "Afficher une partie du contenu du message";
			this.fieldPrivacyNotificationShort.UseVisualStyleBackColor = true;
			// 
			// fieldPrivacyNotificationAll
			// 
			this.fieldPrivacyNotificationAll.AutoSize = true;
			this.fieldPrivacyNotificationAll.Enabled = false;
			this.fieldPrivacyNotificationAll.Location = new System.Drawing.Point(15, 96);
			this.fieldPrivacyNotificationAll.Name = "fieldPrivacyNotificationAll";
			this.fieldPrivacyNotificationAll.Size = new System.Drawing.Size(200, 17);
			this.fieldPrivacyNotificationAll.TabIndex = 1;
			this.fieldPrivacyNotificationAll.TabStop = true;
			this.fieldPrivacyNotificationAll.Text = "Masquer tout le contenu du message";
			this.fieldPrivacyNotificationAll.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(191, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "À la réception d\'un nouveau message :";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 316);
			this.Controls.Add(this.pictureBoxHeader);
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
			this.Text = "Gmail notifier";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.Load += new System.EventHandler(this.Main_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPagePreferences.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageNotification.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).EndInit();
			this.tabPageAbout.ResumeLayout(false);
			this.tabPageAbout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
			this.tabPagePrivacy.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label separator;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPagePreferences;
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
		private System.Windows.Forms.PictureBox pictureBoxHeader;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemSettings;
		private System.Windows.Forms.MenuItem menuItemSynchronize;
		private System.Windows.Forms.MenuItem menuItemMarkAsRead;
		private System.Windows.Forms.MenuItem menuItemTimout;
		private System.Windows.Forms.MenuItem menuItemTimeout30m;
		private System.Windows.Forms.MenuItem menuItemTimeout1h;
		private System.Windows.Forms.MenuItem menuItemTimeout2h;
		private System.Windows.Forms.MenuItem menuItemTimeout5h;
		private System.Windows.Forms.MenuItem menuItemTimeoutDisabled;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.TabPage tabPageNotification;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox fieldStepDelay;
		private System.Windows.Forms.CheckBox fieldSpamNotification;
		private System.Windows.Forms.CheckBox fieldAudioNotification;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown fieldNumericDelay;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox fieldNotification;
		private System.Windows.Forms.CheckBox fieldNetworkConnectivityNotification;
		private System.Windows.Forms.TabPage tabPagePrivacy;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationShort;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationNone;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationAll;
		private System.Windows.Forms.Label label2;
	}
}

