# Giai đoạn 1: Build code
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MatchAPI.csproj", "./"]
RUN dotnet restore "MatchAPI.csproj"
COPY . .
RUN dotnet publish "MatchAPI.csproj" -c Release -o /app/publish

# Giai đoạn 2: Chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
# QUAN TRỌNG: Đảm bảo tên file dll bên dưới đúng là tên Project của bạn
ENTRYPOINT ["dotnet", "MatchAPI.dll"]