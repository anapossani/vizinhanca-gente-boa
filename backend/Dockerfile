FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Vizinhanca.API/Vizinhanca.API.csproj", "Vizinhanca.API/"]
RUN dotnet restore "Vizinhanca.API/Vizinhanca.API.csproj"

COPY . .
WORKDIR "/src/Vizinhanca.API"

RUN dotnet publish "Vizinhanca.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Vizinhanca.API.dll"]