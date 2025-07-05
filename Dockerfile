# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file and all project files
COPY . .

# Restore using solution file
RUN dotnet restore VANTracker.sln

# Build and publish the specific Web project (replace with your main project folder name)
RUN dotnet publish VANTracker/VANTracker.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 8080
EXPOSE 8080

# Start the application (replace with your actual DLL name if different)
ENTRYPOINT ["dotnet", "VANTracker.dll"]
