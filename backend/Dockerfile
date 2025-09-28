FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY Vizinhanca.API/*.csproj ./Vizinhanca.API/
RUN dotnet restore ./Vizinhanca.API/

COPY . ./

RUN dotnet publish ./Vizinhanca.API/ -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Vizinhanca.API.dll"]