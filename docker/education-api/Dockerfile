FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LearningPlatform.sln", "./"]
COPY ["global.json", "./"]
COPY ["AspNetCore/AspNetCore.csproj", "AspNetCore/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["PostgreSql/PostgreSql.csproj", "PostgreSql/"]

RUN dotnet restore "AspNetCore/AspNetCore.csproj"

COPY . .
RUN dotnet publish "AspNetCore/AspNetCore.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AspNetCore.dll"]
