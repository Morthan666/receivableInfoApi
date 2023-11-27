FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ["src/ReceivableInfoApi.WebApi/ReceivableInfoApi.WebApi.csproj", "ReceivableInfoApi.WebApi/"]
COPY ["src/ReceivableInfoApi.Common/ReceivableInfoApi.Common.csproj", "ReceivableInfoApi.Common/"]
COPY ["src/ReceivableInfoApi.DataAccess/ReceivableInfoApi.DataAccess.csproj", "ReceivableInfoApi.DataAccess/"]
RUN dotnet restore "ReceivableInfoApi.WebApi/ReceivableInfoApi.WebApi.csproj"
COPY . .
WORKDIR "/src/ReceivableInfoApi.WebApi"
RUN dotnet build "ReceivableInfoApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReceivableInfoApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReceivableInfoApi.WebApi.dll"]
