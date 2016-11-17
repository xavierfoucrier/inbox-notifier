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
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.linkRestartToApply = new System.Windows.Forms.LinkLabel();
			this.labelRestartToApply = new System.Windows.Forms.Label();
			this.fieldLanguage = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.fieldRunAtWindowsStartup = new System.Windows.Forms.CheckBox();
			this.fieldMinimizeToSystray = new System.Windows.Forms.CheckBox();
			this.fieldAskonExit = new System.Windows.Forms.CheckBox();
			this.tabPageAccount = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.labelTokenDelivery = new System.Windows.Forms.Label();
			this.labelEmailAddress = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonGmailDisconnect = new System.Windows.Forms.Button();
			this.tabPageNotification = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.fieldAttemptToReconnectNotification = new System.Windows.Forms.CheckBox();
			this.fieldNetworkConnectivityNotification = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.fieldStepDelay = new System.Windows.Forms.ComboBox();
			this.fieldSpamNotification = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.fieldNumericDelay = new System.Windows.Forms.NumericUpDown();
			this.fieldMessageNotification = new System.Windows.Forms.CheckBox();
			this.fieldAudioNotification = new System.Windows.Forms.CheckBox();
			this.tabPagePrivacy = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.pictureBoxPrivacyPreview = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.fieldPrivacyNotificationAll = new System.Windows.Forms.RadioButton();
			this.fieldPrivacyNotificationShort = new System.Windows.Forms.RadioButton();
			this.fieldPrivacyNotificationNone = new System.Windows.Forms.RadioButton();
			this.tabPageAbout = new System.Windows.Forms.TabPage();
			this.linkVersion = new System.Windows.Forms.LinkLabel();
			this.linkLicense = new System.Windows.Forms.LinkLabel();
			this.linkWebsiteXavier = new System.Windows.Forms.LinkLabel();
			this.linkWebsiteYusuke = new System.Windows.Forms.LinkLabel();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
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
			this.timerReconnect = new System.Windows.Forms.Timer(this.components);
			this.tabControl.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageAccount.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabPageNotification.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).BeginInit();
			this.tabPagePrivacy.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrivacyPreview)).BeginInit();
			this.tabPageAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
			this.SuspendLayout();
			// 
			// separator
			// 
			this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.separator, "separator");
			this.separator.Name = "separator";
			// 
			// buttonOK
			// 
			resources.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageGeneral);
			this.tabControl.Controls.Add(this.tabPageAccount);
			this.tabControl.Controls.Add(this.tabPageNotification);
			this.tabControl.Controls.Add(this.tabPagePrivacy);
			this.tabControl.Controls.Add(this.tabPageAbout);
			resources.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.groupBox5);
			this.tabPageGeneral.Controls.Add(this.groupBox1);
			resources.ApplyResources(this.tabPageGeneral, "tabPageGeneral");
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.linkRestartToApply);
			this.groupBox5.Controls.Add(this.labelRestartToApply);
			this.groupBox5.Controls.Add(this.fieldLanguage);
			this.groupBox5.Controls.Add(this.label3);
			resources.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			// 
			// linkRestartToApply
			// 
			this.linkRestartToApply.ActiveLinkColor = System.Drawing.Color.RoyalBlue;
			resources.ApplyResources(this.linkRestartToApply, "linkRestartToApply");
			this.linkRestartToApply.LinkColor = System.Drawing.Color.RoyalBlue;
			this.linkRestartToApply.Name = "linkRestartToApply";
			this.linkRestartToApply.TabStop = true;
			this.linkRestartToApply.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
			this.linkRestartToApply.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRestartToApply_LinkClicked);
			// 
			// labelRestartToApply
			// 
			resources.ApplyResources(this.labelRestartToApply, "labelRestartToApply");
			this.labelRestartToApply.ForeColor = System.Drawing.Color.RoyalBlue;
			this.labelRestartToApply.Image = global::notifier.Properties.Resources.information;
			this.labelRestartToApply.Name = "labelRestartToApply";
			// 
			// fieldLanguage
			// 
			this.fieldLanguage.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::notifier.Properties.Settings.Default, "Language", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldLanguage.Items.AddRange(new object[] {
            resources.GetString("fieldLanguage.Items"),
            resources.GetString("fieldLanguage.Items1")});
			resources.ApplyResources(this.fieldLanguage, "fieldLanguage");
			this.fieldLanguage.Name = "fieldLanguage";
			this.fieldLanguage.Text = global::notifier.Properties.Settings.Default.Language;
			this.fieldLanguage.SelectionChangeCommitted += new System.EventHandler(this.fieldLanguage_SelectionChangeCommitted);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.fieldRunAtWindowsStartup);
			this.groupBox1.Controls.Add(this.fieldMinimizeToSystray);
			this.groupBox1.Controls.Add(this.fieldAskonExit);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// fieldRunAtWindowsStartup
			// 
			resources.ApplyResources(this.fieldRunAtWindowsStartup, "fieldRunAtWindowsStartup");
			this.fieldRunAtWindowsStartup.Checked = global::notifier.Properties.Settings.Default.RunAtWindowsStartup;
			this.fieldRunAtWindowsStartup.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "RunAtWindowsStartup", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldRunAtWindowsStartup.Name = "fieldRunAtWindowsStartup";
			this.fieldRunAtWindowsStartup.UseVisualStyleBackColor = true;
			this.fieldRunAtWindowsStartup.CheckedChanged += new System.EventHandler(this.fieldRunAtWindowsStartup_CheckedChanged);
			// 
			// fieldMinimizeToSystray
			// 
			resources.ApplyResources(this.fieldMinimizeToSystray, "fieldMinimizeToSystray");
			this.fieldMinimizeToSystray.Checked = true;
			this.fieldMinimizeToSystray.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.fieldMinimizeToSystray.Name = "fieldMinimizeToSystray";
			this.fieldMinimizeToSystray.UseVisualStyleBackColor = true;
			// 
			// fieldAskonExit
			// 
			resources.ApplyResources(this.fieldAskonExit, "fieldAskonExit");
			this.fieldAskonExit.Checked = global::notifier.Properties.Settings.Default.AskonExit;
			this.fieldAskonExit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAskonExit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AskonExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAskonExit.Name = "fieldAskonExit";
			this.fieldAskonExit.UseVisualStyleBackColor = true;
			// 
			// tabPageAccount
			// 
			this.tabPageAccount.Controls.Add(this.groupBox6);
			resources.ApplyResources(this.tabPageAccount, "tabPageAccount");
			this.tabPageAccount.Name = "tabPageAccount";
			this.tabPageAccount.UseVisualStyleBackColor = true;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.labelTokenDelivery);
			this.groupBox6.Controls.Add(this.labelEmailAddress);
			this.groupBox6.Controls.Add(this.label5);
			this.groupBox6.Controls.Add(this.label4);
			this.groupBox6.Controls.Add(this.buttonGmailDisconnect);
			resources.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			// 
			// labelTokenDelivery
			// 
			resources.ApplyResources(this.labelTokenDelivery, "labelTokenDelivery");
			this.labelTokenDelivery.ForeColor = System.Drawing.Color.Gray;
			this.labelTokenDelivery.Name = "labelTokenDelivery";
			// 
			// labelEmailAddress
			// 
			resources.ApplyResources(this.labelEmailAddress, "labelEmailAddress");
			this.labelEmailAddress.ForeColor = System.Drawing.Color.Gray;
			this.labelEmailAddress.Name = "labelEmailAddress";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// buttonGmailDisconnect
			// 
			resources.ApplyResources(this.buttonGmailDisconnect, "buttonGmailDisconnect");
			this.buttonGmailDisconnect.Name = "buttonGmailDisconnect";
			this.buttonGmailDisconnect.UseVisualStyleBackColor = true;
			this.buttonGmailDisconnect.Click += new System.EventHandler(this.buttonGmailDisconnect_Click);
			// 
			// tabPageNotification
			// 
			this.tabPageNotification.Controls.Add(this.groupBox3);
			this.tabPageNotification.Controls.Add(this.groupBox2);
			resources.ApplyResources(this.tabPageNotification, "tabPageNotification");
			this.tabPageNotification.Name = "tabPageNotification";
			this.tabPageNotification.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.fieldAttemptToReconnectNotification);
			this.groupBox3.Controls.Add(this.fieldNetworkConnectivityNotification);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// fieldAttemptToReconnectNotification
			// 
			resources.ApplyResources(this.fieldAttemptToReconnectNotification, "fieldAttemptToReconnectNotification");
			this.fieldAttemptToReconnectNotification.Checked = global::notifier.Properties.Settings.Default.AttemptToReconnectNotification;
			this.fieldAttemptToReconnectNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAttemptToReconnectNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AttemptToReconnectNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAttemptToReconnectNotification.Name = "fieldAttemptToReconnectNotification";
			this.fieldAttemptToReconnectNotification.UseVisualStyleBackColor = true;
			// 
			// fieldNetworkConnectivityNotification
			// 
			resources.ApplyResources(this.fieldNetworkConnectivityNotification, "fieldNetworkConnectivityNotification");
			this.fieldNetworkConnectivityNotification.Checked = global::notifier.Properties.Settings.Default.NetworkConnectivityNotification;
			this.fieldNetworkConnectivityNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldNetworkConnectivityNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "NetworkConnectivityNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldNetworkConnectivityNotification.Name = "fieldNetworkConnectivityNotification";
			this.fieldNetworkConnectivityNotification.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.fieldStepDelay);
			this.groupBox2.Controls.Add(this.fieldSpamNotification);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.fieldNumericDelay);
			this.groupBox2.Controls.Add(this.fieldMessageNotification);
			this.groupBox2.Controls.Add(this.fieldAudioNotification);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// fieldStepDelay
			// 
			this.fieldStepDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldStepDelay.FormattingEnabled = true;
			this.fieldStepDelay.Items.AddRange(new object[] {
            resources.GetString("fieldStepDelay.Items"),
            resources.GetString("fieldStepDelay.Items1")});
			resources.ApplyResources(this.fieldStepDelay, "fieldStepDelay");
			this.fieldStepDelay.Name = "fieldStepDelay";
			this.fieldStepDelay.SelectionChangeCommitted += new System.EventHandler(this.fieldStepDelay_SelectionChangeCommitted);
			// 
			// fieldSpamNotification
			// 
			resources.ApplyResources(this.fieldSpamNotification, "fieldSpamNotification");
			this.fieldSpamNotification.Checked = global::notifier.Properties.Settings.Default.SpamNotification;
			this.fieldSpamNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldSpamNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "SpamNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldSpamNotification.Name = "fieldSpamNotification";
			this.fieldSpamNotification.UseVisualStyleBackColor = true;
			this.fieldSpamNotification.Click += new System.EventHandler(this.fieldSpamNotification_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// fieldNumericDelay
			// 
			this.fieldNumericDelay.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::notifier.Properties.Settings.Default, "NumericDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			resources.ApplyResources(this.fieldNumericDelay, "fieldNumericDelay");
			this.fieldNumericDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.fieldNumericDelay.Name = "fieldNumericDelay";
			this.fieldNumericDelay.Value = global::notifier.Properties.Settings.Default.NumericDelay;
			this.fieldNumericDelay.ValueChanged += new System.EventHandler(this.fieldNumericDelay_ValueChanged);
			// 
			// fieldMessageNotification
			// 
			resources.ApplyResources(this.fieldMessageNotification, "fieldMessageNotification");
			this.fieldMessageNotification.Checked = global::notifier.Properties.Settings.Default.MessageNotification;
			this.fieldMessageNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldMessageNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "MessageNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldMessageNotification.Name = "fieldMessageNotification";
			this.fieldMessageNotification.UseVisualStyleBackColor = true;
			// 
			// fieldAudioNotification
			// 
			resources.ApplyResources(this.fieldAudioNotification, "fieldAudioNotification");
			this.fieldAudioNotification.Checked = global::notifier.Properties.Settings.Default.AudioNotification;
			this.fieldAudioNotification.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldAudioNotification.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AudioNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAudioNotification.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "MessageNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAudioNotification.Enabled = global::notifier.Properties.Settings.Default.MessageNotification;
			this.fieldAudioNotification.Name = "fieldAudioNotification";
			this.fieldAudioNotification.UseVisualStyleBackColor = true;
			// 
			// tabPagePrivacy
			// 
			this.tabPagePrivacy.Controls.Add(this.groupBox4);
			resources.ApplyResources(this.tabPagePrivacy, "tabPagePrivacy");
			this.tabPagePrivacy.Name = "tabPagePrivacy";
			this.tabPagePrivacy.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.pictureBoxPrivacyPreview);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationAll);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationShort);
			this.groupBox4.Controls.Add(this.fieldPrivacyNotificationNone);
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			// 
			// pictureBoxPrivacyPreview
			// 
			this.pictureBoxPrivacyPreview.Image = global::notifier.Properties.Resources.information;
			resources.ApplyResources(this.pictureBoxPrivacyPreview, "pictureBoxPrivacyPreview");
			this.pictureBoxPrivacyPreview.Name = "pictureBoxPrivacyPreview";
			this.pictureBoxPrivacyPreview.TabStop = false;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// fieldPrivacyNotificationAll
			// 
			resources.ApplyResources(this.fieldPrivacyNotificationAll, "fieldPrivacyNotificationAll");
			this.fieldPrivacyNotificationAll.Name = "fieldPrivacyNotificationAll";
			this.fieldPrivacyNotificationAll.UseVisualStyleBackColor = true;
			this.fieldPrivacyNotificationAll.CheckedChanged += new System.EventHandler(this.fieldPrivacyNotificationAll_CheckedChanged);
			// 
			// fieldPrivacyNotificationShort
			// 
			resources.ApplyResources(this.fieldPrivacyNotificationShort, "fieldPrivacyNotificationShort");
			this.fieldPrivacyNotificationShort.Checked = true;
			this.fieldPrivacyNotificationShort.Name = "fieldPrivacyNotificationShort";
			this.fieldPrivacyNotificationShort.TabStop = true;
			this.fieldPrivacyNotificationShort.UseVisualStyleBackColor = true;
			this.fieldPrivacyNotificationShort.CheckedChanged += new System.EventHandler(this.fieldPrivacyNotificationShort_CheckedChanged);
			// 
			// fieldPrivacyNotificationNone
			// 
			resources.ApplyResources(this.fieldPrivacyNotificationNone, "fieldPrivacyNotificationNone");
			this.fieldPrivacyNotificationNone.Name = "fieldPrivacyNotificationNone";
			this.fieldPrivacyNotificationNone.UseVisualStyleBackColor = true;
			this.fieldPrivacyNotificationNone.CheckedChanged += new System.EventHandler(this.fieldPrivacyNotificationNone_CheckedChanged);
			// 
			// tabPageAbout
			// 
			this.tabPageAbout.Controls.Add(this.linkVersion);
			this.tabPageAbout.Controls.Add(this.linkLicense);
			this.tabPageAbout.Controls.Add(this.linkWebsiteXavier);
			this.tabPageAbout.Controls.Add(this.linkWebsiteYusuke);
			this.tabPageAbout.Controls.Add(this.label11);
			this.tabPageAbout.Controls.Add(this.label12);
			this.tabPageAbout.Controls.Add(this.label13);
			this.tabPageAbout.Controls.Add(this.label14);
			resources.ApplyResources(this.tabPageAbout, "tabPageAbout");
			this.tabPageAbout.Name = "tabPageAbout";
			this.tabPageAbout.UseVisualStyleBackColor = true;
			// 
			// linkVersion
			// 
			resources.ApplyResources(this.linkVersion, "linkVersion");
			this.linkVersion.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkVersion.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkVersion.Name = "linkVersion";
			this.linkVersion.TabStop = true;
			this.linkVersion.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkVersion_LinkClicked);
			// 
			// linkLicense
			// 
			this.linkLicense.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			resources.ApplyResources(this.linkLicense, "linkLicense");
			this.linkLicense.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkLicense.Name = "linkLicense";
			this.linkLicense.TabStop = true;
			this.linkLicense.UseCompatibleTextRendering = true;
			this.linkLicense.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLicense_LinkClicked);
			// 
			// linkWebsiteXavier
			// 
			this.linkWebsiteXavier.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			resources.ApplyResources(this.linkWebsiteXavier, "linkWebsiteXavier");
			this.linkWebsiteXavier.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteXavier.Name = "linkWebsiteXavier";
			this.linkWebsiteXavier.TabStop = true;
			this.linkWebsiteXavier.UseCompatibleTextRendering = true;
			this.linkWebsiteXavier.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteXavier.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebsiteXavier_LinkClicked);
			// 
			// linkWebsiteYusuke
			// 
			this.linkWebsiteYusuke.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			resources.ApplyResources(this.linkWebsiteYusuke, "linkWebsiteYusuke");
			this.linkWebsiteYusuke.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteYusuke.Name = "linkWebsiteYusuke";
			this.linkWebsiteYusuke.TabStop = true;
			this.linkWebsiteYusuke.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.linkWebsiteYusuke.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebsiteYusuke_LinkClicked);
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// label12
			// 
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// label13
			// 
			resources.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			// 
			// label14
			// 
			resources.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			// 
			// labelSettingsSaved
			// 
			resources.ApplyResources(this.labelSettingsSaved, "labelSettingsSaved");
			this.labelSettingsSaved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			this.labelSettingsSaved.Name = "labelSettingsSaved";
			// 
			// notifyIcon
			// 
			resources.ApplyResources(this.notifyIcon, "notifyIcon");
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// pictureBoxHeader
			// 
			this.pictureBoxHeader.Image = global::notifier.Properties.Resources.header;
			resources.ApplyResources(this.pictureBoxHeader, "pictureBoxHeader");
			this.pictureBoxHeader.Name = "pictureBoxHeader";
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
			resources.ApplyResources(this.menuItemSynchronize, "menuItemSynchronize");
			this.menuItemSynchronize.Index = 0;
			this.menuItemSynchronize.Click += new System.EventHandler(this.menuItemSynchronize_Click);
			// 
			// menuItemMarkAsRead
			// 
			resources.ApplyResources(this.menuItemMarkAsRead, "menuItemMarkAsRead");
			this.menuItemMarkAsRead.Index = 1;
			this.menuItemMarkAsRead.Click += new System.EventHandler(this.menuItemMarkAsRead_Click);
			// 
			// menuItemTimout
			// 
			resources.ApplyResources(this.menuItemTimout, "menuItemTimout");
			this.menuItemTimout.Index = 2;
			this.menuItemTimout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTimeoutDisabled,
            this.menuItem1,
            this.menuItemTimeout30m,
            this.menuItemTimeout1h,
            this.menuItemTimeout2h,
            this.menuItemTimeout5h});
			// 
			// menuItemTimeoutDisabled
			// 
			this.menuItemTimeoutDisabled.Checked = true;
			this.menuItemTimeoutDisabled.Index = 0;
			this.menuItemTimeoutDisabled.RadioCheck = true;
			resources.ApplyResources(this.menuItemTimeoutDisabled, "menuItemTimeoutDisabled");
			this.menuItemTimeoutDisabled.Click += new System.EventHandler(this.menuItemTimeoutDisabled_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			resources.ApplyResources(this.menuItem1, "menuItem1");
			// 
			// menuItemTimeout30m
			// 
			this.menuItemTimeout30m.Index = 2;
			this.menuItemTimeout30m.RadioCheck = true;
			resources.ApplyResources(this.menuItemTimeout30m, "menuItemTimeout30m");
			this.menuItemTimeout30m.Click += new System.EventHandler(this.menuItemTimeout30m_Click);
			// 
			// menuItemTimeout1h
			// 
			this.menuItemTimeout1h.Index = 3;
			this.menuItemTimeout1h.RadioCheck = true;
			resources.ApplyResources(this.menuItemTimeout1h, "menuItemTimeout1h");
			this.menuItemTimeout1h.Click += new System.EventHandler(this.menuItemTimeout1h_Click);
			// 
			// menuItemTimeout2h
			// 
			this.menuItemTimeout2h.Index = 4;
			this.menuItemTimeout2h.RadioCheck = true;
			resources.ApplyResources(this.menuItemTimeout2h, "menuItemTimeout2h");
			this.menuItemTimeout2h.Click += new System.EventHandler(this.menuItemTimeout2h_Click);
			// 
			// menuItemTimeout5h
			// 
			this.menuItemTimeout5h.Index = 5;
			this.menuItemTimeout5h.RadioCheck = true;
			resources.ApplyResources(this.menuItemTimeout5h, "menuItemTimeout5h");
			this.menuItemTimeout5h.Click += new System.EventHandler(this.menuItemTimeout5h_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.DefaultItem = true;
			resources.ApplyResources(this.menuItemSettings, "menuItemSettings");
			this.menuItemSettings.Index = 3;
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 4;
			resources.ApplyResources(this.menuItem2, "menuItem2");
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 5;
			resources.ApplyResources(this.menuItemExit, "menuItemExit");
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = global::notifier.Properties.Settings.Default.TimerInterval;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// timerReconnect
			// 
			this.timerReconnect.Interval = 10000;
			this.timerReconnect.Tick += new System.EventHandler(this.timerReconnect_Tick);
			// 
			// Main
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBoxHeader);
			this.Controls.Add(this.separator);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.labelSettingsSaved);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Main";
			this.ShowInTaskbar = false;
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.Load += new System.EventHandler(this.Main_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageAccount.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.tabPageNotification.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).EndInit();
			this.tabPagePrivacy.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrivacyPreview)).EndInit();
			this.tabPageAbout.ResumeLayout(false);
			this.tabPageAbout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label separator;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox fieldRunAtWindowsStartup;
		private System.Windows.Forms.CheckBox fieldMinimizeToSystray;
		private System.Windows.Forms.CheckBox fieldAskonExit;
		private System.Windows.Forms.TabPage tabPageAbout;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
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
		private System.Windows.Forms.CheckBox fieldMessageNotification;
		private System.Windows.Forms.CheckBox fieldNetworkConnectivityNotification;
		private System.Windows.Forms.TabPage tabPagePrivacy;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationShort;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationNone;
		private System.Windows.Forms.RadioButton fieldPrivacyNotificationAll;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkWebsiteXavier;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ComboBox fieldLanguage;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelRestartToApply;
		private System.Windows.Forms.TabPage tabPageAccount;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button buttonGmailDisconnect;
		private System.Windows.Forms.Label labelEmailAddress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelTokenDelivery;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel linkVersion;
		private System.Windows.Forms.LinkLabel linkRestartToApply;
		private System.Windows.Forms.LinkLabel linkLicense;
		private System.Windows.Forms.CheckBox fieldAttemptToReconnectNotification;
		private System.Windows.Forms.Timer timerReconnect;
		private System.Windows.Forms.PictureBox pictureBoxPrivacyPreview;
	}
}

