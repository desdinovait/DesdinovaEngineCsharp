; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=Sors Adversa
AppVerName=Sors Adversa 0.3
AppPublisher=DesdinovaTeam
AppPublisherURL=http://www.desdinova.it/
AppSupportURL=http://www.desdinova.it/
AppUpdatesURL=http://www.desdinova.it/
DefaultDirName={pf}\Sors Adversa
DefaultGroupName=Sors Adversa
AllowNoIcons=true
OutputBaseFilename=setup
Compression=lzma
SolidCompression=true
WizardImageFile=D:\DesdinovaEngineX\SorsAdversa\Setup files\Sors.bmp
WizardImageBackColor=clWhite
WizardSmallImageFile=D:\DesdinovaEngineX\SorsAdversa\Setup files\SorsMini.bmp

[Languages]
Name: english; MessagesFile: compiler:Default.isl
Name: italian; MessagesFile: compiler:Languages\Italian.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: D:\DesdinovaEngineX\SorsAdversa\bin\x86\Debug\SorsAdversa.exe; DestDir: {app}; Flags: ignoreversion
Source: D:\DesdinovaEngineX\SorsAdversa\bin\x86\Debug\DesdinovaEngineX.dll; DestDir: {app}; Flags: ignoreversion
Source: D:\DesdinovaEngineX\SorsAdversa\bin\x86\Debug\Content\*; DestDir: {app}\Content; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: {group}\Sors Adversa; Filename: {app}\SorsAdversa.exe
Name: {group}\{cm:UninstallProgram,Sors Adversa}; Filename: {uninstallexe}
Name: {commondesktop}\Sors Adversa; Filename: {app}\SorsAdversa.exe; Tasks: desktopicon

[Run]
Filename: {app}\SorsAdversa.exe; Description: {cm:LaunchProgram,Sors Adversa}; Flags: nowait postinstall skipifsilent
