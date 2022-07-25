#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CleanArchitectureTemplate.Api/CleanArchitectureTemplate.Api.csproj", "CleanArchitectureTemplate.Api/"]
RUN dotnet restore "CleanArchitectureTemplate.Api/CleanArchitectureTemplate.Api.csproj"
COPY . .
WORKDIR "/src/CleanArchitectureTemplate.Api"
RUN dotnet build "CleanArchitectureTemplate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitectureTemplate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitectureTemplate.Api.dll"]