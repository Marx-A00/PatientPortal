#!/bin/bash

# Azure Key Vault Setup Script for Patient Portal
# This script creates and configures Azure Key Vault for secure secret management

set -e  # Exit on any error

# Configuration
RG="rg-patientportal-containers"
LOCATION="Central US"
KEY_VAULT_NAME="kv-portal-$(date +%s | tail -c 6)"
APP_NAME="patientportal-demo"
SQL_SERVER="sql-portal-$(date +%s | tail -c 8)"
SQL_PASSWORD="$(openssl rand -base64 12)$(date +%s | tail -c 3)!"

echo "🔐 Setting up Azure Key Vault for Patient Portal..."
echo "📍 Resource Group: $RG"
echo "🏺 Key Vault: $KEY_VAULT_NAME"

# Login check
if ! az account show &> /dev/null; then
    echo "❌ Please login to Azure first: az login"
    exit 1
fi

# Create resource group if it doesn't exist
echo "📦 Creating resource group..."
az group create --name $RG --location "$LOCATION" --output table

# Create Key Vault
echo "🏺 Creating Azure Key Vault..."
az keyvault create \
    --name $KEY_VAULT_NAME \
    --resource-group $RG \
    --location "$LOCATION" \
    --enabled-for-template-deployment true \
    --enabled-for-disk-encryption true \
    --enabled-for-deployment true \
    --output table

# Create SQL Server and Database
echo "🗄️ Creating SQL Server and Database..."
az sql server create \
    --name $SQL_SERVER \
    --resource-group $RG \
    --location "$LOCATION" \
    --admin-user sqladmin \
    --admin-password $SQL_PASSWORD \
    --output table

az sql db create \
    --server $SQL_SERVER \
    --resource-group $RG \
    --name PatientPortalDB \
    --service-objective Basic \
    --output table

# Configure firewall to allow Azure services
az sql server firewall-rule create \
    --resource-group $RG \
    --server $SQL_SERVER \
    --name AllowAzureServices \
    --start-ip-address 0.0.0.0 \
    --end-ip-address 0.0.0.0

# Build connection string
CONN_STRING="Server=$SQL_SERVER.database.windows.net;Database=PatientPortalDB;User Id=sqladmin;Password=$SQL_PASSWORD;TrustServerCertificate=true;"

# Store secrets in Key Vault
echo "🔑 Storing secrets in Key Vault..."
az keyvault secret set \
    --vault-name $KEY_VAULT_NAME \
    --name "ConnectionStrings--DefaultConnection" \
    --value "$CONN_STRING" \
    --output table

az keyvault secret set \
    --vault-name $KEY_VAULT_NAME \
    --name "SqlServer--Name" \
    --value "$SQL_SERVER" \
    --output table

az keyvault secret set \
    --vault-name $KEY_VAULT_NAME \
    --name "SqlServer--Password" \
    --value "$SQL_PASSWORD" \
    --output table

# Get current user for access policy
CURRENT_USER=$(az account show --query user.name --output tsv)
echo "👤 Setting access policy for: $CURRENT_USER"

# Set access policy for current user
az keyvault set-policy \
    --name $KEY_VAULT_NAME \
    --upn $CURRENT_USER \
    --secret-permissions get list set delete backup restore recover purge \
    --output table

# Create system-assigned managed identity for Container App (if it exists)
if az containerapp show --name $APP_NAME --resource-group $RG &> /dev/null; then
    echo "🔗 Configuring managed identity for Container App..."
    
    # Enable system-assigned managed identity
    PRINCIPAL_ID=$(az containerapp identity assign \
        --name $APP_NAME \
        --resource-group $RG \
        --system-assigned \
        --query principalId \
        --output tsv)
    
    # Set access policy for managed identity
    az keyvault set-policy \
        --name $KEY_VAULT_NAME \
        --object-id $PRINCIPAL_ID \
        --secret-permissions get list \
        --output table
    
    echo "✅ Managed identity configured with Key Vault access"
fi

# Get Key Vault URI
KEY_VAULT_URI="https://$KEY_VAULT_NAME.vault.azure.net/"

echo "🎉 Azure Key Vault setup complete!"
echo ""
echo "📋 Configuration Details:"
echo "🏺 Key Vault Name: $KEY_VAULT_NAME"
echo "🔗 Key Vault URI: $KEY_VAULT_URI"
echo "🗄️ SQL Server: $SQL_SERVER.database.windows.net"
echo "🔑 SQL Password: $SQL_PASSWORD"
echo ""
echo "📝 Next Steps:"
echo "1. Update your app configuration with KeyVaultUri: $KEY_VAULT_URI"
echo "2. For local development, add this to your user secrets:"
echo "   dotnet user-secrets set 'KeyVaultUri' '$KEY_VAULT_URI'"
echo "3. For Container Apps, set environment variable:"
echo "   KeyVaultUri=$KEY_VAULT_URI"
echo ""
echo "🔍 Test Key Vault access:"
echo "   az keyvault secret show --name 'ConnectionStrings--DefaultConnection' --vault-name '$KEY_VAULT_NAME'"
echo ""
echo "🐳 Ready for secure containerized deployment!" 