#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builddeps
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
COPY ["src/People.BlazorWasmServer/People.BlazorWasmServer.csproj", "src/People.BlazorWasmServer/"]
COPY ["src/People.Infrastructure/People.Infrastructure.csproj", "src/People.Infrastructure/"]
COPY ["src/People.Application/People.Application.csproj", "src/People.Application/"]
COPY ["src/People.Shared/People.Shared.csproj", "src/People.Shared/"]
COPY ["src/People.BlazorWasmClient/People.BlazorWasmClient.csproj", "src/People.BlazorWasmClient/"]
RUN dotnet restore "./src/People.BlazorWasmServer/./People.BlazorWasmServer.csproj"
COPY . .

FROM builddeps AS build
WORKDIR "/app/src/People.BlazorWasmServer"
RUN dotnet build "./People.BlazorWasmServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./People.BlazorWasmServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM builddeps as dbmigration
WORKDIR /
RUN dotnet tool install --global dotnet-ef --version 7.0.13
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef database update \
    --project /app/src/People.Infrastructure/People.Infrastructure.csproj \
    --startup-project /app/src/People.BlazorWasmServer/People.BlazorWasmServer.csproj \
    --configuration $BUILD_CONFIGURATION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=dbmigration /app/src/People.BlazorWasmServer/people.db .
ENTRYPOINT ["dotnet", "People.BlazorWasmServer.dll"]
