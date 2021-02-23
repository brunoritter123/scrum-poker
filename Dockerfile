#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ScrumPoker_Server/", "ScrumPoker_Server"]
RUN dotnet restore "ScrumPoker_Server/ScrumPoker.sln"
COPY . .

RUN dotnet build "ScrumPoker_Server/ScrumPoker.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScrumPoker_Server/ScrumPoker.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

RUN useradd -m myappuser

COPY --from=publish /app/publish .
RUN echo OK

COPY ./ScrumPoker_Server/data_base.db ./
COPY ./ScrumPoker_Client/dist/ ./wwwroot

RUN chmod 777 ./data_base.db
RUN chown -R myappuser:myappuser ./

USER myappuser

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet ScrumPoker.API.dll