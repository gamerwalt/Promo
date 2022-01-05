function New-Cert
{
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout promo.reporting.api.key -out promo.reporting.api.crt -config promo.reporting.api.conf -subj /CN=promo.reporting.api
    openssl pkcs12 -export -out promo.reporting.api.pfx -inkey promo.reporting.api.key -in promo.reporting.api.crt -passout pass:promo.reporting.api
}

function Add-Cert
{
    $dir = Get-Location
    $fileName = "promo.reporting.api.crt"
    $file = "$dir\$fileName"
    $promoreportingapiCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @($file)
    $storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root
    $storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
    $store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
    try
    {
        $store.Add($promoreportingapiCaCert)
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

    Move-Item "promo.reporting.api.pfx" $httpsDir -Force
    Move-Item "promo.reporting.api.crt" ..
    Remove-Item "promo.reporting.api.key"
}

New-Cert
Add-Cert
Move-Files