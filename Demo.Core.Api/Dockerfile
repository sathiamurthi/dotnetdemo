#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Core.Api/Demo.Core.Api.csproj", "Demo.Core.Api/"]
RUN dotnet restore "Demo.Core.Api/Demo.Core.Api.csproj"
COPY . .
WORKDIR "/src/Demo.Core.Api"
RUN dotnet build "Demo.Core.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Core.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Core.Api.dll"]