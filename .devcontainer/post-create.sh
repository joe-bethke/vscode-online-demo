./.devcontainer/install-azds.sh
az login -u $1 -p $2
az aks get-credentials --name $AzureAksName --resource-group $AzureResourceGroupName
azds space select --name $(az account show --query "user.name" --output tsv | sed "s/\@.*//")
dotnet user-secrets set --project "../src/services/common/Services/Services.csproj" AppConfigurationConnectionString $(az appconfig credential list --name $AzureAppConfigName --resource-group $AzureResourceGroupName --query "[?name=='Primary'].connectionString | [0]" --output tsv)
