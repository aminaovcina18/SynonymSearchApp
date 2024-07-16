#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SynonymSearchApp_API/SynonymSearchApp_API.csproj", "SynonymSearchApp_API/"]
COPY ["SynonymSearchApp_ApplicationCore/SynonymSearchApp_ApplicationCore.csproj", "SynonymSearchApp_ApplicationCore/"]
COPY ["SynonymSearchApp_Domain/SynonymSearchApp_Domain.csproj", "SynonymSearchApp_Domain/"]
RUN dotnet restore "./SynonymSearchApp_API/./SynonymSearchApp_API.csproj"
COPY . .
WORKDIR "/src/SynonymSearchApp_API"
RUN dotnet build "./SynonymSearchApp_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SynonymSearchApp_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SynonymSearchApp_API.dll"]