az group create --name CQRS-Azure-Serverless --location WestEurope

az group deployment create --resource-group CQRS-Azure-Serverless --template-file functionapp.json