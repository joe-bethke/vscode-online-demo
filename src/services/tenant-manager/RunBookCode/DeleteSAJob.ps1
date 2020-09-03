<#
    .DESCRIPTION
        A run book for deleting SA job created for the tenant
    .NOTES
        AUTHOR: Radha Mohanty
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

# Authenticate with the service principal
$connectionName = "AzureRunAsConnection"
try
{
    $servicePrincipalConnection = Get-AutomationConnection -Name $connectionName

    "Logging in to Azure..."
    Connect-AzAccount -Tenant $servicePrincipalConnection.TenantID `
                             -ApplicationId $servicePrincipalConnection.ApplicationID   `
                             -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint `
                             -ServicePrincipal `
                             -SubscriptionId $data.subscriptionId
    "Successfully Logged in using ServicePrincipal."
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
$resourceGroup = $data.resourceGroup
$saJobName = $data.saJobName

# stop the SA job and Remove the SA job when the tenant is deleted
Stop-AzStreamAnalyticsJob -Name $saJobName -ResourceGroupName $resourceGroup
Start-Sleep -Seconds 120
Write-Output "Stream Analytics Job $saJobName has been stopped."
Remove-AzStreamAnalyticsJob -Name $saJobName -ResourceGroupName $resourceGroup
Write-Output "Stream Analytics job $saJobName has been deprovisioned."
# done
