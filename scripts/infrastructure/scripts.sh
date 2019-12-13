# usage: sh scripts.sh CQRS-Azure-Serverless NorthEurope

resourceGroupName=$1
resourceGroupLocation=$2

az deployment create -l $resourceGroupLocation --template-file resourceGroup.json --parameters resourceGroupName=$resourceGroupName

az group deployment create --resource-group $resourceGroupName --template-file keyVault.json

az group deployment create --resource-group $resourceGroupName --template-file storageAccount.json

az group deployment create --resource-group $resourceGroupName --template-file cosmosdb.json

az group deployment create --resource-group $resourceGroupName --template-file functionapp.json