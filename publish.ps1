docker-compose.exe build
heroku container:push web -a scrum-poker-br
heroku container:release web -a scrum-poker-br
heroku logs --tail -a scrum-poker-br