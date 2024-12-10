FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cloud_Store/Cloud_Store.csproj", "Cloud_Store/"]
RUN dotnet restore "Cloud_Store/Cloud_Store.csproj"
COPY . .
WORKDIR "/src/Cloud_Store"
RUN dotnet build "Cloud_Store.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cloud_Store.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Add a new migration stage
FROM build AS migrations
WORKDIR "/src/Cloud_Store"
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN mkdir "Database"
RUN touch "Database/cloud_store.db"
RUN dotnet ef migrations add InitialMigration
RUN dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy migration files if needed
COPY --from=migrations /src/Cloud_Store/Database ./Database
RUN mkdir Files
ENTRYPOINT ["dotnet", "Cloud_Store.dll"]