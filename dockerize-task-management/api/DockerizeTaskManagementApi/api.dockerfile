FROM mcr.microsoft.com/dotnet/aspnet:5.0 as env
WORKDIR /publish

COPY ./publish .
 
ENTRYPOINT ["dotnet", "DockerizeTaskManagementApi.dll"]