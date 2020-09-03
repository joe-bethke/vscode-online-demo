<#
    .DESCRIPTION
        A run book for creating a new IoT Hub for a tenant

    .NOTES
        AUTHOR: Nate Oelke
                Sean Dubiel
#>

param
(
    [Parameter (Mandatory = $false)]
    [object] $WebhookData
)

# Make sure this runbook was triggered by a webhook
if (-Not $WebhookData) {
    # Error
    Write-Error "This runbook is meant to be started from an Azure alert webhook only."
}

# Retrieve the data from the Webhook request body
$data = (ConvertFrom-Json -InputObject $WebhookData.RequestBody)

# Authenticate with the service principle
$connectionName = "AzureRunAsConnection"
try
{
    $servicePrincipalConnection = Get-AutomationConnection -Name $connectionName

    "Logging in to Azure..."
    $connectionResult =  Connect-AzAccount -Tenant $servicePrincipalConnection.TenantID `
                             -ApplicationId $servicePrincipalConnection.ApplicationID   `
                             -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint `
                             -ServicePrincipal `
                             -Subscription $data.subscriptionId

    "Logged in successfully using servicePrincipal"
}
catch {
    if (!$servicePrincipalConnection)
    {
        $ErrorMessage = "Connection $connectionName not found."
        throw $ErrorMessage
    } else{
        Write-Error -Message $_.Exception
        throw $_.Exception
    }
}

# Get data 
$appConfigConnectionString = $data.appConfigConnectionString
$setAppConfigEndpoint = $data.setAppConfigEndpoint
$data.token

# Delete IoT Hub using Azure REST API
function deleteIotHubUsingRestApi(){
    $requestheader = @{
        "Authorization" = "Bearer " + $data.token
        "Content-Type" = "application/json"
      }
      
      $iotHubUri = "https://management.azure.com/subscriptions/$($data.subscriptionId)/resourceGroups/$($data.resourceGroup)/providers/Microsoft.Devices/IotHubs/$($data.iotHubName)?api-version=2019-03-22-preview"
      $result = (Invoke-RestMethod -Method delete -Headers $requestheader -Uri $iotHubUri)
}

# Delete the IoT Hub connection string from app config
function removeIoTHubConnStringFromAppconfig(){
    $requestheader = @{
        "Content-Type" = "application/json"
    }
    $appConfigKey= "tenant:$($data.tenantId):iotHubConnectionString"
  
    $appConfigBody = @"
    {
       connectionstring : "$appConfigConnectionString", name : "$appConfigKey",
    }
"@
    $appConfigBody
    $result = (Invoke-RestMethod -ContentType 'application/json' -Method delete -Headers $requestheader -Uri $setAppConfigEndpoint -Body $appConfigBody)
}

# Remove DPS (Device provisioning Service)
function deleteDps(){
    $ifExists = Get-AzIoTDeviceProvisioningService -ResourceGroupName $($data.resourceGroup) -Name $($data.dpsName) 
    if ($ifExists.Name){
        $result = Remove-AzIoTDeviceProvisioningService -ResourceGroupName $($data.resourceGroup) -Name $($data.dpsName) -PassThru
        Start-Sleep -Second 60    
        if ($result -eq 'True' ) { 
            Write-Output "DPS $($data.dpsName) deleted successfully" 
        }
    }
    else { Write-Error "DPS does not exist: $($data.dpsName)" }       
}

try {
    # Call the functions 
    deleteIotHubUsingRestApi
    removeIoTHubConnStringFromAppconfig
    deleteDps
    "Done"
}
catch {
    Write-Error -Message $_.Exception
    throw $_.Exception
}