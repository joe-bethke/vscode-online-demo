param(
    [string] $automationAccountName,
    [string] $resourceGroup
)

function importRunbook($runbookName, $filepath) {
    
    Import-AzureRMAutomationRunbook -Name $runbookName -Path $scriptPath `
                                    -ResourceGroupName $resourceGroup `
                                    -AutomationAccountName $automationAccountName `
                                    -Type PowerShell `
                                    -Force

    Publish-AzureRmAutomationRunbook -Name $runbookName -AutomationAccountName $automationAccountName `
                                     -ResourceGroupName $resourceGroup
}

importRunbook -runbookName "CreateIoTHubTenant" -filepath "./CreateIoTHub.ps1" 
importRunbook -runbookName "DeleteIoTHubTenant" -filepath "./DeleteIoTHub.ps1"
