#In Production
#Secrets may not be used in Production.
#To run on HTTPS we will need to pass the certificate’s path and password from the command line 
#using the following arguments for the “docker run” command:
#-e ASPNETCORE_Kestrel__Certificates__Default__Password
#-e ASPNETCORE_Kestrel__Certificates__Default__Path

#//PRODUCTION
#https://github.com/dotnet/dotnet-docker/blob/main/samples/host-aspnetcore-https.md

#//DEVELOPMENT
#https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md


For Docker and ASP.Net core container to container communication 
https://github.com/gamerwalt/Sample-Docker-Https/tree/main/SampleAPI

Download OpensslForWindows
C:\Program Files\OpenSSL-Win64
Add OpenSsl to Path


Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass