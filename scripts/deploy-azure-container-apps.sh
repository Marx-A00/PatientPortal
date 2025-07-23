#!/bin/bash

# Deploy Azure Container Apps with Key Vault Integration
set -e

# Configuration
RG="rg-patientportal-containers"
LOCATION="East US"
CONTAINER_ENV="patientportal-env"
CONTAINER_APP="patientportal-demo"
KEY_VAULT_NAME="${KEY_VAULT_NAME:-kv-portal-88596}"
REGISTRY="ghcr.io"
IMAGE_NAME="$(git config --get remote.origin.url | sed 's/.*\/\([^.]*\).git/\1/')/patientportal"

echo "🐳 Deploying Azure Container Apps with Key Vault integration..."
echo "📍 Resource Group: $RG"
echo "🌐 Container Environment: $CONTAINER_ENV"
echo "📦 Container App: $CONTAINER_APP"

# Install Container Apps extension
az extension add --name containerapp --upgrade 2>/dev/null || true

# Create Container Apps Environment
echo "🌐 Creating Container Apps Environment..."
az containerapp env create \
  --name $CONTAINER_ENV \
  --resource-group $RG \
  --location "$LOCATION" \
  --output table

# Create Container App with placeholder image
echo "📦 Creating Container App..."
az containerapp create \
  --name $CONTAINER_APP \
  --resource-group $RG \
  --environment $CONTAINER_ENV \
  --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest \
  --target-port 8080 \
  --ingress 'external' \
  --env-vars \
    "ASPNETCORE_ENVIRONMENT=Production" \
    "KeyVaultUri=https://$KEY_VAULT_NAME.vault.azure.net/" \
  --output table

# Enable system-assigned managed identity
echo "🔗 Enabling managed identity..."
PRINCIPAL_ID=$(az containerapp identity assign \
  --name $CONTAINER_APP \
  --resource-group $RG \
  --system-assigned \
  --query principalId \
  --output tsv)

echo "🔑 Configuring Key Vault access for managed identity..."
# Assign Key Vault Secrets User role to the managed identity
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee $PRINCIPAL_ID \
  --scope "/subscriptions/$(az account show --query id --output tsv)/resourceGroups/$RG/providers/Microsoft.KeyVault/vaults/$KEY_VAULT_NAME"

# Get the container app URL
APP_URL=$(az containerapp show \
  --name $CONTAINER_APP \
  --resource-group $RG \
  --query properties.configuration.ingress.fqdn \
  --output tsv)

echo ""
echo "🎉 Azure Container Apps deployment complete!"
echo "🌐 App URL: https://$APP_URL"
echo "🔐 Key Vault: https://$KEY_VAULT_NAME.vault.azure.net/"
echo "🆔 Managed Identity ID: $PRINCIPAL_ID"
echo ""
echo "📝 Next Steps:"
echo "1. Build and push Docker image:"
echo "   docker build -t $REGISTRY/$IMAGE_NAME:latest -f PatientPortal/Dockerfile ."
echo "   docker push $REGISTRY/$IMAGE_NAME:latest"
echo ""
echo "2. Update Container App with your image:"
echo "   az containerapp update --name $CONTAINER_APP --resource-group $RG --image $REGISTRY/$IMAGE_NAME:latest"
echo ""
echo "3. Or use GitHub Actions to automate deployment!"
echo ""
echo "🚀 Ready for production deployment!" 