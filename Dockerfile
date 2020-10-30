#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["XPlan.WebApi/XPlan.WebApi.csproj", "XPlan.WebApi/"]
COPY ["XPlan.Service/XPlan.Service.Core.csproj", "XPlan.Service/"]
COPY ["XPlan.Repository.Abstracts/XPlan.Repository.Abstracts.csproj", "XPlan.Repository.Abstracts/"]
COPY ["XPlan.Model/XPlan.Model.csproj", "XPlan.Model/"]
COPY ["XPlan.Service.Abstracts/XPlan.Service.Abstracts.csproj", "XPlan.Service.Abstracts/"]
COPY ["XPlan.Repository/XPlan.Repository.EntityFrameworkCore.csproj", "XPlan.Repository/"]
RUN dotnet restore "XPlan.WebApi/XPlan.WebApi.csproj"
COPY . .
WORKDIR "/src/XPlan.WebApi"
RUN dotnet build "XPlan.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XPlan.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XPlan.WebApi.dll"]