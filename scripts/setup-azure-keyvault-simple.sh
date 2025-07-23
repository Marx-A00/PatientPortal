#!/bin/bash

# Simplified Azure Key Vault Setup for existing resource group
set -e

# Configuration  
RG="rg-patientportal-containers"
KEY_VAULT_NAME="kv-portal-88596"  # Use existing Key Vault
SQL_SERVER="sql-portal-$(date +%s | tail -c 8)"
SQL_PASSWORD="$(openssl rand -base64 12)$(date +%s | tail -c 3)!"
SQL_LOCATION="Central US"  # Use region with available quota

echo "🔐 Completing Azure Key Vault setup..."
echo "🏺 Using existing Key Vault: $KEY_VAULT_NAME"
echo "🗄️ Creating SQL Server in Central US..."

# Create SQL Server in Central US (has quota)
az sql server create \
    --name $SQL_SERVER \
    --resource-group $RG \
    --location "$SQL_LOCATION" \
    --admin-user sqladmin \
    --admin-password $SQL_PASSWORD \
    --output table

echo "🗄️ Creating database..."
az sql db create \
    --server $SQL_SERVER \
    --resource-group $RG \
    --name PatientPortalDB \
    --service-objective Basic \
    --output table

echo "🔥 Configuring firewall..."
az sql server firewall-rule create \
    --resource-group $RG \
    --server $SQL_SERVER \
    --name AllowAzureServices \
    --start-ip-address 0.0.0.0 \
    --end-ip-address 0.0.0.0

# Build connection string
CONN_STRING="Server=$SQL_SERVER.database.windows.net;Database=PatientPortalDB;User Id=sqladmin;Password=$SQL_PASSWORD;TrustServerCertificate=true;"

echo "🔑 Storing connection string in Key Vault..."
az keyvault secret set \
    --vault-name $KEY_VAULT_NAME \
    --name "ConnectionStrings--DefaultConnection" \
    --value "$CONN_STRING" \
    --output table

# Get current user for access policy  
CURRENT_USER=$(az account show --query user.name --output tsv)
echo "👤 Setting access policy for: $CURRENT_USER"

az keyvault set-policy \
    --name $KEY_VAULT_NAME \
    --upn $CURRENT_USER \
    --secret-permissions get list set delete backup restore recover purge \
    --output table

# Get Key Vault URI
KEY_VAULT_URI="https://$KEY_VAULT_NAME.vault.azure.net/"

echo ""
echo "🎉 Azure Key Vault setup complete!"
echo "🔗 Key Vault URI: $KEY_VAULT_URI" 
echo "🗄️ SQL Server: $SQL_SERVER.database.windows.net"
echo "🔑 SQL Password: $SQL_PASSWORD"
echo ""
echo "📝 Next Steps:"
echo "1. Add to user secrets: dotnet user-secrets set 'KeyVaultUri' '$KEY_VAULT_URI'"
echo "2. Test: az keyvault secret show --name 'ConnectionStrings--DefaultConnection' --vault-name '$KEY_VAULT_NAME'"
echo ""
echo "🐳 Ready for enterprise deployment!" 