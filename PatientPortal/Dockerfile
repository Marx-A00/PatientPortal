# Multi-stage build for production optimization
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PatientPortal/PatientPortal.csproj", "PatientPortal/"]
RUN dotnet restore "PatientPortal/PatientPortal.csproj"
COPY PatientPortal/ PatientPortal/
WORKDIR "/src/PatientPortal"
RUN dotnet build "PatientPortal.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PatientPortal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientPortal.dll"]