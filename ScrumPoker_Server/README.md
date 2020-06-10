# ScrumPoker_Server

Esse projeto foi gerado com  [.Net Core](https://dotnet.microsoft.com/download/dotnet-core/3.1) versão 3.1

## Instalação
- **dotnet** - Fazer a instalção do [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- **dotnet ef** - Execute `dotnet tool install -g dotnet-ef --version 3.1.1`

## Configurando user-secrets
Execute `dotnet user-secrets init --project ./ScrumPoker.API/` para criar o arquivo de *secrets*.
- Execute `dotnet user-secrets set ConnStr "sua-connection-string" --project ScrumPoker.API/` para informar sua *connection string* do banco de dados **sqlite**.
- Execute `dotnet user-secrets set JwtSecretKey "sua-token-jwt" --project ScrumPoker.API/` para informar sua *scret key* do **token** para autenticação usando **JWT**.
- Execute `dotnet user-secrets set EmailPassword "sua-senha-do-email" --project ScrumPoker.API/` para informar sua senha do **e-mail** para uso da aplicaçao.


## Servidor de desenvolvimento
Execute `dotnet watch run --project ScrumPoker.API/` para iniciar o servidor. Navegue em `http://localhost:5000/`. O aplicativo será recarregado automaticamente se você alterar qualquer um dos arquivos de origem.

## Criando migrations
Execute `dotnet ef migrations add "nome-da-migration" --project ./ScrumPoker.Data/ --startup-project ./ScrumPoker.API/` para gerar uma nova *migration* do banco de dados.

## Atualizando o banco de dados
Execute `dotnet ef database update --project ./ScrumPoker.Data/ --startup-project ./ScrumPoker.API/` atulizando o banco de banco de dados com as migrations do projeto.

## Build
Execute `dotnet build ScrumPoker_Server.sln` para contruir os executaveis do projeto.

## Teste unitários
Execute `dotnet test ScrumPoker_Server.sln` para iniciar os testes unitários via [xUnit](https://xunit.net/).
