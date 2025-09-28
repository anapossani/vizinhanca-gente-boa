
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Vizinhanca.API/Vizinhanca.API.csproj", "Vizinhanca.API/"]
RUN dotnet restore "Vizinhanca.API/Vizinhanca.API.csproj"

COPY . .
WORKDIR "/src/Vizinhanca.API"

RUN dotnet build "Vizinhanca.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vizinhanca.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Vizinhanca.API.dll"]