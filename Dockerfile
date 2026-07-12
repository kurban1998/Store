# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the entire solution so cross-project references resolve
COPY . .

# Restore and publish the WebApp project against the full solution context
RUN dotnet restore WebApp/WebApp.csproj
RUN dotnet publish WebApp/WebApp.csproj -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Railway provides PORT; bind Kestrel to 0.0.0.0:$PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

ENTRYPOINT ["dotnet", "WebApp.dll"]
