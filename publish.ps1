$env:ASPNETCORE_ENVIRONMENT='Production'

cd .\ScrumPoker_Client\
ng build --configuration production

cd ..

cd .\ScrumPoker_Server\
dotnet ef database update --project ./ScrumPoker.Data/ --startup-project ./ScrumPoker.API/

cd ..

docker-compose.exe build
heroku container:push web -a scrum-poker-br
heroku container:release web -a scrum-poker-br
heroku logs --tail -a scrum-poker-br