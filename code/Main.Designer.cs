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
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.labelTotalLabels = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.labelTotalDrafts = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.labelTotalMails = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.labelTotalUnreadMails = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.labelTokenDelivery = new System.Windows.Forms.Label();
			this.labelEmailAddress = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonGmailDisconnect = new System.Windows.Forms.Button();
			this.tabPageNotification = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.fieldNotificationBehavior = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
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
			this.tabPageUpdate = new System.Windows.Forms.TabPage();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.fieldUpdateQuiet = new System.Windows.Forms.CheckBox();
			this.fieldUpdateDownload = new System.Windows.Forms.CheckBox();
			this.buttonCheckForUpdate = new System.Windows.Forms.Button();
			this.labelUpdateControl = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.fieldUpdatePeriod = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.fieldUpdateService = new System.Windows.Forms.CheckBox();
			this.tabPageAbout = new System.Windows.Forms.TabPage();
			this.linkSoftpedia = new System.Windows.Forms.LinkLabel();
			this.linkCheckForUpdate = new System.Windows.Forms.LinkLabel();
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
			this.menuItemNewMessage = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemSynchronize = new System.Windows.Forms.MenuItem();
			this.menuItemMarkAsRead = new System.Windows.Forms.MenuItem();
			this.menuItemTimout = new System.Windows.Forms.MenuItem();
			this.menuItemTimeoutDisabled = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout30m = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout1h = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout2h = new System.Windows.Forms.MenuItem();
			this.menuItemTimeout5h = new System.Windows.Forms.MenuItem();
			this.menuItemTimeoutIndefinitely = new System.Windows.Forms.MenuItem();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.timerReconnect = new System.Windows.Forms.Timer(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.tabControl.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageAccount.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabPageNotification.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldNumericDelay)).BeginInit();
			this.tabPagePrivacy.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrivacyPreview)).BeginInit();
			this.tabPageUpdate.SuspendLayout();
			this.groupBox7.SuspendLayout();
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
			this.tabControl.Controls.Add(this.tabPageUpdate);
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
            resources.GetString("fieldLanguage.Items1"),
            resources.GetString("fieldLanguage.Items2")});
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
			this.fieldAskonExit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "AskonExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldAskonExit.Name = "fieldAskonExit";
			this.fieldAskonExit.UseVisualStyleBackColor = true;
			// 
			// tabPageAccount
			// 
			this.tabPageAccount.Controls.Add(this.groupBox8);
			this.tabPageAccount.Controls.Add(this.groupBox6);
			resources.ApplyResources(this.tabPageAccount, "tabPageAccount");
			this.tabPageAccount.Name = "tabPageAccount";
			this.tabPageAccount.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.labelTotalLabels);
			this.groupBox8.Controls.Add(this.label18);
			this.groupBox8.Controls.Add(this.labelTotalDrafts);
			this.groupBox8.Controls.Add(this.label16);
			this.groupBox8.Controls.Add(this.labelTotalMails);
			this.groupBox8.Controls.Add(this.label10);
			this.groupBox8.Controls.Add(this.labelTotalUnreadMails);
			this.groupBox8.Controls.Add(this.label6);
			resources.ApplyResources(this.groupBox8, "groupBox8");
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.TabStop = false;
			// 
			// labelTotalLabels
			// 
			resources.ApplyResources(this.labelTotalLabels, "labelTotalLabels");
			this.labelTotalLabels.Name = "labelTotalLabels";
			// 
			// label18
			// 
			resources.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			// 
			// labelTotalDrafts
			// 
			resources.ApplyResources(this.labelTotalDrafts, "labelTotalDrafts");
			this.labelTotalDrafts.Name = "labelTotalDrafts";
			// 
			// label16
			// 
			resources.ApplyResources(this.label16, "label16");
			this.label16.Name = "label16";
			// 
			// labelTotalMails
			// 
			resources.ApplyResources(this.labelTotalMails, "labelTotalMails");
			this.labelTotalMails.Name = "labelTotalMails";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// labelTotalUnreadMails
			// 
			resources.ApplyResources(this.labelTotalUnreadMails, "labelTotalUnreadMails");
			this.labelTotalUnreadMails.Name = "labelTotalUnreadMails";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
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
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Controls.Add(this.fieldNotificationBehavior);
			this.groupBox3.Controls.Add(this.label9);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// label15
			// 
			resources.ApplyResources(this.label15, "label15");
			this.label15.ForeColor = System.Drawing.Color.RoyalBlue;
			this.label15.Image = global::notifier.Properties.Resources.information;
			this.label15.Name = "label15";
			// 
			// fieldNotificationBehavior
			// 
			this.fieldNotificationBehavior.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "MessageNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldNotificationBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldNotificationBehavior.Enabled = global::notifier.Properties.Settings.Default.MessageNotification;
			this.fieldNotificationBehavior.FormattingEnabled = true;
			this.fieldNotificationBehavior.Items.AddRange(new object[] {
            resources.GetString("fieldNotificationBehavior.Items"),
            resources.GetString("fieldNotificationBehavior.Items1")});
			resources.ApplyResources(this.fieldNotificationBehavior, "fieldNotificationBehavior");
			this.fieldNotificationBehavior.Name = "fieldNotificationBehavior";
			this.fieldNotificationBehavior.SelectionChangeCommitted += new System.EventHandler(this.fieldNotificationBehavior_SelectionChangeCommitted);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "MessageNotification", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.label9.Enabled = global::notifier.Properties.Settings.Default.MessageNotification;
			this.label9.Name = "label9";
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
			// tabPageUpdate
			// 
			this.tabPageUpdate.Controls.Add(this.groupBox7);
			resources.ApplyResources(this.tabPageUpdate, "tabPageUpdate");
			this.tabPageUpdate.Name = "tabPageUpdate";
			this.tabPageUpdate.UseVisualStyleBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.fieldUpdateQuiet);
			this.groupBox7.Controls.Add(this.fieldUpdateDownload);
			this.groupBox7.Controls.Add(this.buttonCheckForUpdate);
			this.groupBox7.Controls.Add(this.labelUpdateControl);
			this.groupBox7.Controls.Add(this.label8);
			this.groupBox7.Controls.Add(this.fieldUpdatePeriod);
			this.groupBox7.Controls.Add(this.label7);
			this.groupBox7.Controls.Add(this.fieldUpdateService);
			resources.ApplyResources(this.groupBox7, "groupBox7");
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.TabStop = false;
			// 
			// fieldUpdateQuiet
			// 
			resources.ApplyResources(this.fieldUpdateQuiet, "fieldUpdateQuiet");
			this.fieldUpdateQuiet.Checked = global::notifier.Properties.Settings.Default.UpdateQuiet;
			this.fieldUpdateQuiet.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldUpdateQuiet.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdateQuiet.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "UpdateQuiet", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdateQuiet.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.fieldUpdateQuiet.Name = "fieldUpdateQuiet";
			this.fieldUpdateQuiet.UseVisualStyleBackColor = true;
			// 
			// fieldUpdateDownload
			// 
			resources.ApplyResources(this.fieldUpdateDownload, "fieldUpdateDownload");
			this.fieldUpdateDownload.Checked = global::notifier.Properties.Settings.Default.UpdateDownload;
			this.fieldUpdateDownload.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "UpdateDownload", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdateDownload.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdateDownload.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.fieldUpdateDownload.Name = "fieldUpdateDownload";
			this.fieldUpdateDownload.UseVisualStyleBackColor = true;
			// 
			// buttonCheckForUpdate
			// 
			resources.ApplyResources(this.buttonCheckForUpdate, "buttonCheckForUpdate");
			this.buttonCheckForUpdate.Name = "buttonCheckForUpdate";
			this.buttonCheckForUpdate.UseVisualStyleBackColor = true;
			this.buttonCheckForUpdate.Click += new System.EventHandler(this.buttonCheckForUpdate_Click);
			// 
			// labelUpdateControl
			// 
			resources.ApplyResources(this.labelUpdateControl, "labelUpdateControl");
			this.labelUpdateControl.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.labelUpdateControl.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.labelUpdateControl.ForeColor = System.Drawing.Color.Gray;
			this.labelUpdateControl.Name = "labelUpdateControl";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.label8.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.label8.Name = "label8";
			// 
			// fieldUpdatePeriod
			// 
			this.fieldUpdatePeriod.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdatePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fieldUpdatePeriod.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.fieldUpdatePeriod.FormattingEnabled = true;
			this.fieldUpdatePeriod.Items.AddRange(new object[] {
            resources.GetString("fieldUpdatePeriod.Items"),
            resources.GetString("fieldUpdatePeriod.Items1"),
            resources.GetString("fieldUpdatePeriod.Items2"),
            resources.GetString("fieldUpdatePeriod.Items3")});
			resources.ApplyResources(this.fieldUpdatePeriod, "fieldUpdatePeriod");
			this.fieldUpdatePeriod.Name = "fieldUpdatePeriod";
			this.fieldUpdatePeriod.SelectedIndexChanged += new System.EventHandler(this.fieldUpdatePeriod_SelectedIndexChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.label7.Enabled = global::notifier.Properties.Settings.Default.UpdateService;
			this.label7.Name = "label7";
			// 
			// fieldUpdateService
			// 
			resources.ApplyResources(this.fieldUpdateService, "fieldUpdateService");
			this.fieldUpdateService.Checked = global::notifier.Properties.Settings.Default.UpdateService;
			this.fieldUpdateService.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fieldUpdateService.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::notifier.Properties.Settings.Default, "UpdateService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fieldUpdateService.Name = "fieldUpdateService";
			this.fieldUpdateService.UseVisualStyleBackColor = true;
			// 
			// tabPageAbout
			// 
			this.tabPageAbout.Controls.Add(this.linkSoftpedia);
			this.tabPageAbout.Controls.Add(this.linkCheckForUpdate);
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
			// linkSoftpedia
			// 
			this.linkSoftpedia.Cursor = System.Windows.Forms.Cursors.Hand;
			this.linkSoftpedia.Image = global::notifier.Properties.Resources.softpedia;
			resources.ApplyResources(this.linkSoftpedia, "linkSoftpedia");
			this.linkSoftpedia.Name = "linkSoftpedia";
			this.linkSoftpedia.Click += new System.EventHandler(this.linkSoftpedia_Click);
			// 
			// linkCheckForUpdate
			// 
			this.linkCheckForUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.linkCheckForUpdate.Image = global::notifier.Properties.Resources.update_check;
			resources.ApplyResources(this.linkCheckForUpdate, "linkCheckForUpdate");
			this.linkCheckForUpdate.Name = "linkCheckForUpdate";
			this.linkCheckForUpdate.Click += new System.EventHandler(this.linkCheckForUpdate_Click);
			// 
			// linkVersion
			// 
			this.linkVersion.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(33)))));
			resources.ApplyResources(this.linkVersion, "linkVersion");
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
			this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
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
            this.menuItemNewMessage,
            this.menuItem4,
            this.menuItemSynchronize,
            this.menuItemMarkAsRead,
            this.menuItemTimout,
            this.menuItemSettings,
            this.menuItem2,
            this.menuItemExit});
			// 
			// menuItemNewMessage
			// 
			this.menuItemNewMessage.Index = 0;
			resources.ApplyResources(this.menuItemNewMessage, "menuItemNewMessage");
			this.menuItemNewMessage.Click += new System.EventHandler(this.menuItemNewMessage_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			resources.ApplyResources(this.menuItem4, "menuItem4");
			// 
			// menuItemSynchronize
			// 
			resources.ApplyResources(this.menuItemSynchronize, "menuItemSynchronize");
			this.menuItemSynchronize.Index = 2;
			this.menuItemSynchronize.Click += new System.EventHandler(this.menuItemSynchronize_Click);
			// 
			// menuItemMarkAsRead
			// 
			resources.ApplyResources(this.menuItemMarkAsRead, "menuItemMarkAsRead");
			this.menuItemMarkAsRead.Index = 3;
			this.menuItemMarkAsRead.Click += new System.EventHandler(this.menuItemMarkAsRead_Click);
			// 
			// menuItemTimout
			// 
			resources.ApplyResources(this.menuItemTimout, "menuItemTimout");
			this.menuItemTimout.Index = 4;
			this.menuItemTimout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTimeoutDisabled,
            this.menuItem1,
            this.menuItemTimeout30m,
            this.menuItemTimeout1h,
            this.menuItemTimeout2h,
            this.menuItemTimeout5h,
            this.menuItemTimeoutIndefinitely});
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
			// menuItemTimeoutIndefinitely
			// 
			this.menuItemTimeoutIndefinitely.Index = 6;
			resources.ApplyResources(this.menuItemTimeoutIndefinitely, "menuItemTimeoutIndefinitely");
			this.menuItemTimeoutIndefinitely.Click += new System.EventHandler(this.menuItemTimeoutIndefinitely_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.DefaultItem = true;
			resources.ApplyResources(this.menuItemSettings, "menuItemSettings");
			this.menuItemSettings.Index = 5;
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 6;
			resources.ApplyResources(this.menuItem2, "menuItem2");
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 7;
			resources.ApplyResources(this.menuItemExit, "menuItemExit");
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// timerReconnect
			// 
			this.timerReconnect.Tick += new System.EventHandler(this.timerReconnect_Tick);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = global::notifier.Properties.Settings.Default.TimerInterval;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
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
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Main";
			this.ShowInTaskbar = false;
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.Load += new System.EventHandler(this.Main_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
			this.tabControl.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageAccount.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
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
			this.tabPageUpdate.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
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
		private System.Windows.Forms.CheckBox fieldMessageNotification;
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
		private System.Windows.Forms.Timer timerReconnect;
		private System.Windows.Forms.PictureBox pictureBoxPrivacyPreview;
		private System.Windows.Forms.LinkLabel linkCheckForUpdate;
		private System.Windows.Forms.TabPage tabPageUpdate;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.CheckBox fieldUpdateService;
		private System.Windows.Forms.ComboBox fieldUpdatePeriod;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label labelUpdateControl;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button buttonCheckForUpdate;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelTotalUnreadMails;
		private System.Windows.Forms.Label labelTotalMails;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label labelTotalDrafts;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label labelTotalLabels;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.CheckBox fieldUpdateDownload;
		private System.Windows.Forms.MenuItem menuItemNewMessage;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemTimeoutIndefinitely;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox fieldNotificationBehavior;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox fieldUpdateQuiet;
		private System.Windows.Forms.LinkLabel linkSoftpedia;
	}
}

