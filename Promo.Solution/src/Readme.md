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

To set images for Docker
D:\Promo\Promo.Solution\src>docker image tag promocampaignapi promocampaignapi:latest

D:\Promo\Promo.Solution\src>docker image tag promoemailsapi promoemailsapi:latest

D:\Promo\Promo.Solution\src>docker image tag promomembershipapi promomembershipapi:latest

D:\Promo\Promo.Solution\src>docker image tag promoreportingapi promoreportingapi:latest

D:\Promo\Promo.Solution\src>docker image tag promorulesapi promorulesapi:latest

D:\Promo\Promo.Solution\src>docker image tag promosweepsapi promosweepsapi:latest

D:\Promo\Promo.Solution\src>docker push gamerwalt/promosweepsapi:latest
The push refers to repository [docker.io/gamerwalt/promosweepsapi]
An image does not exist locally with the tag: gamerwalt/promosweepsapi

D:\Promo\Promo.Solution\src>docker tag promocampaignapi:latest gamerwalt/dockerhub:promocampaignapi

D:\Promo\Promo.Solution\src>docker tag promoemailsapi:latest gamerwalt/dockerhub:promoemailsapi

D:\Promo\Promo.Solution\src>docker tag promomembershipapi:latest gamerwalt/dockerhub:promomembershipapi

D:\Promo\Promo.Solution\src>docker tag promoreportingapi:latest gamerwalt/dockerhub:promoreportingapi

D:\Promo\Promo.Solution\src>docker tag promorulesapi:latest gamerwalt/dockerhub:promorulesapi


#randomness
https://khalidabuhakmeh.com/creating-random-numbers-with-dotnet-core