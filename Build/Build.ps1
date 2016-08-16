param(
    [string]$Version = '99.1.1.0',
    [string]$PublisherName = 'CN=OutdoorTrackerTemporaryKey',
    [string]$PublisherDisplayName = 'Meinrad Jean-Richard',
    [string]$ProductDisplayName = 'Outdoor Tracker Beta',
    [string]$ProductPhoneId = '54D782E1-A8AC-41AB-BE55-188CB58957A6',
    [string]$IdentityName = 'OutdoorTrackerBeta',
    [string]$HockeyAppToken = '',
	[switch]$BuildForStore = $false
)

. "$PSScriptRoot\New-SelfSignedCertificateEx.ps1"

$ErrorActionPreference = "Stop"
$projectPath = "$PSScriptRoot\..\OutdoorTracker"
$projectFile = "$projectPath\OutdoorTracker.csproj"

function Build()
{
    Write-Host -Object 'Building solution' -ForegroundColor Green 
    &('C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe') $projectFile /v:m /p:Configuration="Release$(if ($BuildForStore) {';UapAppxPackageBuildMode=CI'})"
}

function UpdateFiles()
{
	Write-Host -Object 'Updating Files...' -ForegroundColor Green
    $appxManifestPath = "$projectPath\Package.appxmanifest"

    $appxManifest = [xml](get-content $appxManifestPath)

    $appxManifest.Package.PhoneIdentity.PhoneProductId = $ProductPhoneId

    $appxManifest.Package.Identity.Name = $IdentityName
    $appxManifest.Package.Identity.Publisher = $PublisherName
    $appxManifest.Package.Identity.Version = $Version

    $appxManifest.Package.Properties.DisplayName = $ProductDisplayName
    $appxManifest.Package.Properties.PublisherDisplayName = $PublisherDisplayName

    $appxManifest.Package.Applications.Application.VisualElements.DisplayName = $ProductDisplayName
    $appxManifest.Package.Applications.Application.VisualElements.DefaultTile.ShortName = $ProductDisplayName

    $appxManifest.save($appxManifestPath)
}

function CreateCertificate()
{
    Write-Host -Object "Searching Certificate..." -ForegroundColor Green
	$cert = Get-ChildItem -path cert:\Currentuser\My | Where-Object {$_.Subject -match $PublisherName} | Select-Object -First 1
	
	if (!$cert)
	{
		Write-Host -Object "Creating new Certificate.." -ForegroundColor Green

		$cert = New-SelfSignedCertificateEx -Subject "$PublisherName" `
											-StoreLocation "CurrentUser" `
											-KeySpec "Signature" `
											-KeyUsage "DigitalSignature" `
											-EnhancedKeyUsage @("1.3.6.1.5.5.7.3.3", "1.3.6.1.4.1.311.10.3.13") `
											-IsCA $false `
											-PathLength 0

		$cert = Get-ChildItem -path cert:\Currentuser\My | Where-Object {$_.Subject -match $PublisherName} | Select-Object -First 1
		
	} else {
	
		Write-Host -Object "Found existing Certificate, won't create a new one." -ForegroundColor Green
		
	}

    Write-Host -Object "Updating Thumbprint to '$($cert.Thumbprint)'." -ForegroundColor Green
    $project = [xml](get-content $projectFile)
    $project.Project.PropertyGroup[0].PackageCertificateThumbprint = $cert.Thumbprint
    $project.save($projectFile)
    
    return $cert.Thumbprint;
}

function UploadToHockeyApp
{
	$appxbundle="$env:APPVEYOR_BUILD_FOLDER\Build\OutdoorTracker.appxbundle"

	Write-Host "Looking for AppxBundle '$appxbundle'"

	If (Test-Path $appxbundle){
	  Write-Host "Got AppxBundle starting deployment to HockeyApp..."
	  curl.exe -s -F "status=2" -F "notify=1" -F "ipa=@${appxbundle}" -H "X-HockeyAppToken: $(HockeyAppToken)" https://rink.hockeyapp.net/api/2/apps/b2c844d2de1245bf8e2495ed20350fd8/app_versions/upload
	  Write-Host "Done deploying."
	} Else {
	  Write-Host "No AppxBundle found."
	}
}

# Main Entry Point
Write-Host -Object 'Starting Build...' -ForegroundColor Green

if ($BuildForStore)
{
	Write-Host -Object 'Building for store!' -ForegroundColor Green
	$certThumbprint = CreateCertificate;
} 

UpdateFiles;
Build;

if ($BuildForStore)
{
	Write-Host -Object "Deleting Certificate '$certThumbprint'." -ForegroundColor Green
	Remove-Item -Path "cert:\CurrentUser\My\$certThumbprint" -DeleteKey
}