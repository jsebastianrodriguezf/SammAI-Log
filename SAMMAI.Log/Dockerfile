# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7049
EXPOSE 5233


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SAMMAI.Log/SAMMAI.Log.csproj", "SAMMAI.Log/"]
RUN dotnet restore "./SAMMAI.Log/SAMMAI.Log.csproj"
COPY . .
WORKDIR "/src/SAMMAI.Log"
RUN dotnet build "./SAMMAI.Log.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SAMMAI.Log.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

# Adding SSL certificate
WORKDIR /
COPY --from=build /src/SAMMAI.Log/Certificates/sammai-certificate.pfx https/

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SAMMAI.Log.dll"]