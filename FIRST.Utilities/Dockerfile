FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FIRST.Utilities/FIRST.Utilities.csproj", "FIRST.Utilities/"]
COPY ["FIRST.Utilities.Client/FIRST.Utilities.Client.csproj", "FIRST.Utilities.Client/"]
COPY ["FIRST.Utilities.Configuration/FIRST.Utilities.Configuration.csproj", "FIRST.Utilities.Configuration/"]
COPY ["FIRST.Utilities.DataServices/FIRST.Utilities.DataServices.csproj", "FIRST.Utilities.DataServices/"]
COPY ["FIRST.Utilities.Entities/FIRST.Utilities.Entities.csproj", "FIRST.Utilities.Entities/"]
COPY ["FIRST.Utilities.Repositories/FIRST.Utilities.Repositories.csproj", "FIRST.Utilities.Repositories/"]
COPY ["FIRST.Utilities.Data/FIRST.Utilities.Data.csproj", "FIRST.Utilities.Data/"]
COPY ["FIRST.Utilities.WebServices/FIRST.Utilities.WebServices.csproj", "FIRST.Utilities.WebServices/"]
RUN dotnet restore "FIRST.Utilities/FIRST.Utilities.csproj"
COPY . .
WORKDIR "/src/FIRST.Utilities"
RUN dotnet build "FIRST.Utilities.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIRST.Utilities.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIRST.Utilities.dll"]
