#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitectureTemplate.Api/CleanArchitectureTemplate.Api.csproj", "CleanArchitectureTemplate.Api/"]
RUN dotnet restore "CleanArchitectureTemplate.Api/CleanArchitectureTemplate.Api.csproj"
COPY . .
WORKDIR "/src/CleanArchitectureTemplate.Api"
RUN dotnet build "CleanArchitectureTemplate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CleanArchitectureTemplate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitectureTemplate.Api.dll"]