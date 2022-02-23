# docker stop task-management-api
# docker rm task-management-api
# docker rmi task-management-api

# docker stop task-management-frontend
# docker rm task-management-frontend
# docker rmi task-management-frontend

cd ..\api\DockerizeTaskManagementApi

Remove-Item publish -Recurse
dotnet publish -o publish

cd ..\..

docker-compose -f dockerize/docker-compose.yml build 

docker rmi $(docker images -f "dangling=true" -q) -f

cd dockerize