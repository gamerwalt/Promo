function New-Cert
{
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout promo.sweeps.api.key -out promo.sweeps.api.crt -config promo.sweeps.api.conf -subj /CN=promo.sweeps.api
    openssl pkcs12 -export -out promo.sweeps.api.pfx -inkey promo.sweeps.api.key -in promo.sweeps.api.crt -passout pass:promo.sweeps.api
}

function Add-Cert
{
    $dir = Get-Location
    $fileName = "promo.sweeps.api.crt"
    $file = "$dir\$fileName"
    $promosweepsapiCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @($file)
    $storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root
    $storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
    $store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
    try
    {
        $store.Add($promosweepsapiCaCert)
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

    Move-Item "promo.sweeps.api.pfx" $httpsDir -Force
    Move-Item "promo.sweeps.api.crt" ..
    Remove-Item "promo.sweeps.api.key"
}

New-Cert
Add-Cert
Move-Files