# Base image (now using SDK instead of ASP.NET runtime for EF tools)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add dotnet tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Add dotnet tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Copy project files
COPY Auth/Auth.csproj ./Auth/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

# Restore dependencies
RUN dotnet restore ./Auth/Auth.csproj

# Copy the rest of the source
COPY Auth/ ./Auth/
COPY Shared.Messaging/ ./Shared.Messaging/

# Build the project
WORKDIR /src/Auth
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image (use SDK instead of runtime to allow EF tools)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entrypoint
ENTRYPOINT ["dotnet", "Auth.dll"]
