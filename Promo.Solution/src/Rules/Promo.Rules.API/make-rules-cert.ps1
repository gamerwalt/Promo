function New-Cert
{
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout promo.rules.api.key -out promo.rules.api.crt -config promo.rules.api.conf -subj /CN=promo.rules.api
    openssl pkcs12 -export -out promo.rules.api.pfx -inkey promo.rules.api.key -in promo.rules.api.crt -passout pass:promo.rules.api
}

function Add-Cert
{
    $dir = Get-Location
    $fileName = "promo.rules.api.crt"
    $file = "$dir\$fileName"
    $promorulesapiCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @($file)
    $storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root
    $storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
    $store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
    try
    {
        $store.Add($promorulesapiCaCert)
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

    Move-Item "promo.rules.api.pfx" $httpsDir -Force
    Move-Item "promo.rules.api.crt" ..
    Remove-Item "promo.rules.api.key"
}

New-Cert
Add-Cert
Move-Files