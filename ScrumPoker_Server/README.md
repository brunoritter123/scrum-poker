# ScrumPoker_Server

Esse projeto foi gerado com  [.Net Core](https://dotnet.microsoft.com/download/dotnet-core/6.0) versão 6.0

## Instalação
- **dotnet** - Fazer a instalção do [.NET Core 6.0](https://dotnet.microsoft.com/download/dotnet-core/6.0)

## Ferramentas
Execute `dotnet tool restore` para instalar as ferramentas.
- **dotnet ef**
- **Stryker.Net**

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
Execute `dotnet test ScrumPoker.sln` para iniciar os testes unitários via [xUnit](https://xunit.net/). \

### Cobertura de testes
Entre na pasta de testes, execute `cd .\ScrumPoker.Tests\` \
Execute `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura` para avaliar a combertura de testes via [Coverlet](https://github.com/coverlet-coverage/coverlet).
Execute `dotnet reportgenerator -targetdir:C:\report` para gerar um relatório detalhado de cobertura via [ReportGenerator](https://github.com/danielpalme/ReportGenerator).

### Testes mutantes
Entre na pasta de testes, execute `cd .\ScrumPoker.Tests\` \
Execute o teste mutante para cada projeto e avaliar se todos os mutantes foram mortos:
- `dotnet stryker --project-file=ScrumPoker.API.csproj`
- `dotnet stryker --project-file=ScrumPoker.Application.csproj`
- `dotnet stryker --project-file=ScrumPoker.CrossCutting.csproj`
- `dotnet stryker --project-file=ScrumPoker.Data.csproj`
- `dotnet stryker --project-file=ScrumPoker.Domain.csproj`
