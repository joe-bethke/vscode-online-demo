echo "Install azds cli..."
./.devcontainer/install-azds.sh > /dev/null
echo
echo "Azure Login with local environment vars"
echo $1
echo $2
az login -u $1 -p $2
echo
echo "Setup az aks credentials"
az aks get-credentials --name $AzureAksName --resource-group $AzureResourceGroupName
echo
echo "Create a new dev space"
azds space select --name $(az account show --query "user.name" --output tsv | sed "s/\@.*//")
echo
echo "Set AppConfigurationConnectionString in dotnet user-secrets store"
dotnet user-secrets set --project "src/services/common/Services/Services.csproj" AppConfigurationConnectionString $(az appconfig credential list --name $AzureAppConfigName --resource-group $AzureResourceGroupName --query "[?name=='Primary'].connectionString | [0]" --output tsv)
