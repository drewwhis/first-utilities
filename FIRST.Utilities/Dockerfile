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
