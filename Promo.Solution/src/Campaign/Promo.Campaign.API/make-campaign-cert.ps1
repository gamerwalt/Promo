function New-Cert
{
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout campaignapi.key -out campaignapi.crt -config campaignapi.conf -subj /CN=campaignapi
    openssl pkcs12 -export -out campaignapi.pfx -inkey campaignapi.key -in campaignapi.crt -passout pass:campaignapi
}

function Add-Cert
{
    $dir = Get-Location
    $fileName = "campaignapi.crt"
    $file = "$dir\$fileName"
    $campaignapiCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @($file)
    $storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root
    $storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
    $store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
    try
    {
        $store.Add($campaignapiCert)
        Write-Output "$fileName is successfully added to the Trusted Root."
    }
    finally
    {
        $store.Close()
        $store.Dispose()
    }
}

function Move-Files
{
    $userProfile = $ENV:USERPROFILE
    $httpsDir = "$userProfile\.aspnet\https"

    if (!(Test-Path -Path $httpsDir))
    {
        New-Item -ItemType directory -Path $httpsDir
    }

    Move-Item "campaignapi.pfx" $httpsDir -Force
    Move-Item "campaignapi.crt" ..
    Remove-Item "campaignapi.key"
}

New-Cert
Add-Cert
Move-Files