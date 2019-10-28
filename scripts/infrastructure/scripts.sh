

az group create --name CQRS-Azure-Serverless --location WestEurope

az keyvault create -n cqrsazureserverless -g CQRS-Azure-Serverless

az group deployment create --resource-group CQRS-Azure-Serverless --template-file storageAccount.json

az group deployment create --resource-group CQRS-Azure-Serverless --template-file cosmosdb.json

az group deployment create --resource-group CQRS-Azure-Serverless --template-file functionapp.json