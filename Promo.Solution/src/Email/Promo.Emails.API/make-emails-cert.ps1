function New-Cert
{
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout emailsapi.key -out emailsapi.crt -config emailsapi.conf -subj /CN=emailsapi
    openssl pkcs12 -export -out emailsapi.pfx -inkey emailsapi.key -in emailsapi.crt -passout pass:emailsapi
}

function Add-Cert
{
    $dir = Get-Location
    $fileName = "emailsapi.crt"
    $file = "$dir\$fileName"
    $promoemailsapiCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @($file)
    $storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root
    $storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
    $store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
    try
    {
        $store.Add($promoemailsapiCaCert)
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

    Move-Item "emailsapi.pfx" $httpsDir -Force
    Move-Item "emailsapi.crt" ..
    Remove-Item "emailsapi.key"
}

New-Cert
Add-Cert
Move-Files