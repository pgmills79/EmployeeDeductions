FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Deductoins.API/Deductoins.API.csproj", "Deductoins.API/"]
RUN dotnet restore "Deductoins.API/Deductoins.API.csproj"
COPY . .
WORKDIR "/src/Deductoins.API"
RUN dotnet build "Deductoins.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Deductoins.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Deductoins.API.dll"]
