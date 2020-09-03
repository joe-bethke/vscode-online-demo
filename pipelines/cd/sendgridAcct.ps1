# Create a SendGrid Acct on Azure
# Create the API Key for the Acct on SendGrid
# Store the API Key to the Keyvault
# Store the SendGrid UserId and Password to the Keyvault

param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullorEmpty()]
    [string] $location,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullorEmpty()]
    [string] $resourceGroup,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullorEmpty()]
    [string] $resourceName,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullorEmpty()]
    [string] $keyVaultName,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullorEmpty()]
    [string] $email,
    [string] $firstName,
    [string] $lastName
)

$resourceType = 'Sendgrid.Email/accounts'
$apiVersion = '2015-04-08'

# use the defaults if values provided are null or empty
if ([string]::IsNullOrEmpty($firstName)){ $firstName = $email.Split('@')[0] }
if ([string]::IsNullOrEmpty($lastName)){ $lastName = $email.Split('@')[0] }

# Generate a Random password 
function Get-RandomCharacters($length, $characters) {
    $random = 1..$length | ForEach-Object { Get-Random -Maximum $characters.length }
    $private:ofs=""
    return [String]$characters[$random]
}
function ScrambletheString([string]$inputString){
    $characterArray = $inputstring.ToCharArray()   
    $scrambledStringArray = $characterArray | Get-Random -Count $characterArray.Length     
    $outputString = -join $scrambledStringArray
    return $outputString 
}

function CreateRandomPassword() {
    $password = Get-RandomCharacters -length 4 -characters 'abcdefghiklmnoprstuvwxyz'
    $password += Get-RandomCharacters -length 3 -characters '!"$%&/()=?}][{@#*+'
    $password += Get-RandomCharacters -length 3 -characters '1234567890'
    $password += Get-RandomCharacters -length 4 -characters 'ABCDEFGHKLMNOPRSTUVWXYZ'

    $password = ScrambletheString $password
    return $password
}

function CreateSendGridAccount() {
    $password = CreateRandomPassword
    $newAzResourceParameters = @{
        Location          = $location
        ResourceGroupName = $resourceGroup
        ResourceType      = $resourceType
        ResourceName      = $resourceName
        Plan = @{
            Name          = 'free'
            Publisher     = 'Sendgrid'
            Product       = 'sendgrid_azure'
            PromotionCode = ''
        }
        Properties = @{
            password              = $password
            acceptMarketingEmails = '0'
            email                 = $email
            firstName             = $firstName
            lastName              = $lastName
            company               = '3M'
            website               = 'https:\\www.3m.com'
        }
    }

    try{
        # accept the legal terms to purchase from market place, required for the first time
        Get-AzureRmMarketplaceTerms -Publisher 'Sendgrid' -Product 'sendgrid_azure' -Name 'free' | Set-AzureRmMarketplaceTerms -Accept -ErrorAction Stop
        # register the namespace 'Sendgrid.Email'
        Register-AzureRmResourceProvider -ProviderNamespace Sendgrid.Email
        Start-Sleep -Seconds 60
        # create the sendgrid account
        New-AzureRmResource @newAzResourceParameters -Force -ErrorAction Stop
        Write-Output "SendGrid Acct $resourceName created successfully"
        
        # adding SendGrid Acct password to Keyvault 
        $vaultSGPwd = ConvertTo-SecureString -String $password -AsPlainText -Force
        Set-AzureKeyVaultSecret -VaultName $keyVaultName -Name "Global--SendGridAcctPassword" -SecretValue $vaultSGPwd
        Write-Output "SendGrid Acct Password added to Keyvault"
        
        $result = Get-AzureRmResource -ResourceType $resourceType `
                                      -ResourceName $resourceName `
                                      -ResourceGroupName $resourceGroup `
                                      -ErrorAction Stop
                            
        $sendgridUserId = $result.Properties.username 
        # call to SendGrid Acct to create the API Key
        $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $sendgridUserId,$password)))
        $sendGridUri = 'https://api.sendgrid.com/v3/api_keys'
        $requestHeader = @{ Authorization = ("Basic {0}" -f $base64AuthInfo) }    
        $requestBody = @"
        {
        "name": "sendgrid-email-api-key",
        "scopes": ["mail.send", "alerts.create","alerts.read"]
        }
"@    
        $response = Invoke-RestMethod -uri $sendGridUri -Method Post -ContentType "application/json" -Headers $requestHeader -Body $requestBody
    
        $sendGridApiKey = $response.api_key
        if (![string]::IsNullOrEmpty($sendGridApiKey)){
            Write-Host "Successfully created an API Key on the Sendgrid Account"
        }   
        
        # adding SendGrid API key to Keyvault 
        $vaulSGKey = ConvertTo-SecureString -String $sendGridApiKey -AsPlainText -Force
        Set-AzureKeyVaultSecret -VaultName $keyVaultName -Name "IdentityGatewayService--SendGridApiKey" -SecretValue $vaulSGKey
        Write-Output "SendGrid API Key added to Keyvault"
    }
    catch {
        Write-Error -Message $_.Exception
        throw $_.Exception
    }
}

# check SendGrid acct if exists  
$result = Get-AzureRmResource -ResourceType $resourceType `
                              -ResourceName $resourceName `
                              -ResourceGroupName $resourceGroup `
                              -ErrorAction SilentlyContinue

if ([string]::IsNullOrEmpty($result)){
    Write-Output "SendGrid Acct does not exist, creating one"
    #Create a new acct
    CreateSendGridAccount
}
else {
Write-Output "SendGrid Acct $resourceName already exists"
}
