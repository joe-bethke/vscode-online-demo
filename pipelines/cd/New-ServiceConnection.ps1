Function New-ServiceConnection {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $True)]
        $ServicePrincipalId,
        [Parameter(Mandatory = $True)]
        $ServicePrincipalKey,
        [Parameter(Mandatory = $True)]
        $TenantId,
        [Parameter(Mandatory = $True)]
        $SubscriptionId,
        [Parameter(Mandatory = $True)]
        $SubscriptionName,
        [Parameter(Mandatory = $False)]
        $Description,
        [Parameter(Mandatory = $True)]
        $Name,
        [Parameter(Mandatory = $True)]
        $ProjectId,
        [Parameter(Mandatory = $True)]
        $ProjectName
    )

    $template = Get-Content service-connection-template.json -Raw | ConvertFrom-Json -Depth 100
    $template.authorization.parameters.serviceprincipalid = $ServicePrincipalId
    $template.authorization.parameters.serviceprincipalkey = $ServicePrincipalKey
    $template.authorization.parameters.tenantid = $TenantId
    $template.data.subscriptionId = $SubscriptionId
    $template.data.subscriptionName = $SubscriptionName
    $template.description = $Description
    $template.name = $Name
    $template.serviceEndpointProjectReferences[0].description = $Description
    $template.serviceEndpointProjectReferences[0].name = $Name
    $template.serviceEndpointProjectReferences[0].projectReference.id = $ProjectId
    $template.serviceEndpointProjectReferences[0].projectReference.name = $ProjectName
    $file = "$([System.IO.Path]::GetTempFileName()).json"
    $template | ConvertTo-Json -Depth 100 | Set-Content -Path $file
    az devops service-endpoint create --service-endpoint-configuration $file
    return $file
}