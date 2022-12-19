#define MyAppName "Inbox Notifier"
#define MyAppVersion "3.9.0"
#define MyAppYear GetDateTimeString('yyyy', '', '');
#define MyAppPublisher "Xavier Foucrier"
#define MyAppURL "https://github.com/xavierfoucrier/inbox-notifier"
#define MyAppExeName "Inbox Notifier.exe"
#define MyAppRegistryKeyName "Inbox Notifier"

[Setup]
AppId={{D5D279BC-098C-4CBE-BC2C-FBA3F75F773F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppCopyright=Copyright (c) {#MyAppYear} {#MyAppPublisher}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppMutex=inboxnotifier-115e363ecbfefd771e55c6874680bc0a
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=..\LICENSE.md
OutputDir=..\setup
ArchitecturesInstallIn64BitMode=x64
OutputBaseFilename={#MyAppName} {#MyAppVersion}
SetupIconFile=setup.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
WizardImageFile=image.bmp
PrivilegesRequired=admin
UsedUserAreasWarning=no
VersionInfoVersion={#MyAppVersion}
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1
Name: "startup"; Description: "{cm:AutoStartProgram,{#MyAppName}}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone;

[Files]
Source: "..\code\bin\Release 32 bits (x86)\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "..\code\bin\Release 32 bits (x86)\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: not Is64BitInstallMode
Source: "..\code\bin\Release 64 bits (x64)\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "..\code\bin\Release 64 bits (x64)\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: Is64BitInstallMode

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Parameters: "install {language} {code:autorun}"; Flags: runhidden
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#MyAppRegistryKeyName}"; ValueData: "{app}\{#MyAppExeName}"; Tasks: startup; Flags: uninsdeletevalue;

[Code]
function autorun(Value: string): string;
begin
	if RegValueExists(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Run', '{#MyAppRegistryKeyName}') then begin
		Result := 'auto';
	end	else begin
		Result := 'none';
	end;
end;

[UninstallDelete]
Type: filesandordirs; Name: "{localappdata}\{#MyAppName}"