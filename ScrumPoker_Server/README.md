# ScrumPoker_Server

Esse projeto foi gerado com  [.Net Core](https://dotnet.microsoft.com/download/dotnet-core/5.0) versão 5.0

## Instalação
- **dotnet** - Fazer a instalção do [.NET Core 5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)
- **dotnet ef** - Execute `dotnet tool install -g dotnet-ef --version 5.0.5`

## Configurando 'appsettings.json'
Acesse o arquivo appsettings.[Ambiente].json (Ambiente é conforme configurado na variável de ambiente ASPNETCORE_ENVIRONMENT).
- Informe a configuração do *TokenConfig*
- Informe a configuração do *EmailConfig*
- Informe a configuração do *ConnectionStrings*
- Não esqueça  de 



## Servidor de desenvolvimento
Execute `dotnet watch run --project ScrumPoker.API/` para iniciar o servidor. Navegue em `http://localhost:5000/`. O aplicativo será recarregado automaticamente se você alterar qualquer um dos arquivos de origem.

## Criando migrations
Execute `dotnet ef migrations add "nome-da-migration" --project ./ScrumPoker.Data/ --startup-project ./ScrumPoker.API/` para gerar uma nova *migration* do banco de dados.

## Atualizando o banco de dados
Execute `dotnet ef database update --project ./ScrumPoker.Data/ --startup-project ./ScrumPoker.API/` atulizando o banco de banco de dados com as migrations do projeto.

## Build
Execute `dotnet build ScrumPoker.sln` para contruir os executaveis do projeto.

## Teste unitários
Execute `dotnet test ScrumPoker.sln` para iniciar os testes unitários via [xUnit](https://xunit.net/).

docker-compose.exe build
heroku apps:create scrum-poker-br
heroku container:push web -a scrum-poker-br
heroku container:release web -a scrum-poker-br
heroku logs --tail -a scrum-poker-br
heroku apps:destroy scrum-poker-br