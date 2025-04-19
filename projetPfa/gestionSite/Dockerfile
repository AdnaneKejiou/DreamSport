# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

COPY gestionSite/gestionSite.csproj ./gestionSite/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/
RUN dotnet restore ./gestionSite/gestionSite.csproj

COPY gestionSite/ ./gestionSite/
COPY Shared.Messaging/ ./Shared.Messaging/
WORKDIR /src/gestionSite
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "gestionSite.dll"]
