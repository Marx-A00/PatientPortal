#!/bin/bash
RG="rg-patientportal-containers"
LOCATION="East US"
CONTAINER_ENV="patientportal-env"
CONTAINER_APP="patientportal-demo"
SQL_SERVER="sql-patientportal-$(date +%s)"
SQL_PASSWORD="$(openssl rand -base64 12)$(date +%s | tail -c 3)!"

echo "🐳 Creating Docker-powered Azure infrastructure..."

# Install Container Apps extension
az extension add --name containerapp --upgrade

# Create resource group
az group create --name $RG --location "$LOCATION"

# Create SQL Server + Database
az sql server create \
  --name $SQL_SERVER \
  --resource-group $RG \
  --location "$LOCATION" \
  --admin-user sqladmin \
  --admin-password $SQL_PASSWORD

az sql db create \
  --server $SQL_SERVER \
  --resource-group $RG \
  --name PatientPortalDB \
  --service-objective Basic

az sql server firewall-rule create \
  --resource-group $RG \
  --server $SQL_SERVER \
  --name AllowAll \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 255.255.255.255

# Create Container Apps Environment
az containerapp env create \
  --name $CONTAINER_ENV \
  --resource-group $RG \
  --location "$LOCATION"

# Build connection string
CONN_STRING="Server=$SQL_SERVER.database.windows.net;Database=PatientPortalDB;User Id=sqladmin;Password=$SQL_PASSWORD;TrustServerCertificate=true;"

# Create Container App
az containerapp create \
  --name $CONTAINER_APP \
  --resource-group $RG \
  --environment $CONTAINER_ENV \
  --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest \
  --target-port 8080 \
  --ingress 'external' \
  --env-vars \
    "ConnectionStrings__DefaultConnection=$CONN_STRING" \
    "ASPNETCORE_ENVIRONMENT=Production"

# Get the FQDN
APP_FQDN=$(az containerapp show \
  --name $CONTAINER_APP \
  --resource-group $RG \
  --query properties.configuration.ingress.fqdn \
  --output tsv)

echo "✅ Container App URL: https://$APP_FQDN"
echo "📊 SQL Server: $SQL_SERVER.database.windows.net"
echo "🔑 SQL Password: $SQL_PASSWORD"
echo "🐳 Ready for Docker deployment!" 