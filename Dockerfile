# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project file
COPY MatchAPI/MatchAPI.csproj MatchAPI/
RUN dotnet restore MatchAPI/MatchAPI.csproj

# Copy rest of source code
COPY MatchAPI/ MatchAPI/

# Publish
RUN dotnet publish MatchAPI/MatchAPI.csproj -c Release -o /app/publish


# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Render uses $PORT variable
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "MatchAPI.dll"]
