#!/bin/bash
# Copyright (c) 3M. All rights reserved.

if [ -n "$1" ] ; then
  echo "Using connection string argument in place of environment variable"
  AppConfigurationConnectionString=$1
fi

# Fetch App configuration
_get_configuration() {

  if value=$(az appconfig kv show --key $1  --connection-string $AppConfigurationConnectionString | jq .value); then
    echo $value
  else
    echo "Failed to ping Application Configuration: $1"
    echo
    echo "Exiting set_env.sh"
    exit 1
  fi

}

modify_webui_policies(){

if az appconfig kv list --connection-string $AppConfigurationConnectionString --key "Global:ClientAuth:Roles:*" | jq -c '.[] | .value' > appconfig.txt; then
   echo "Pushed the appconfig value to appconfig.txt"
   echo "Adding the required contents to the policies.js file"
   IFS="%"
   policies_file="module.exports = {
       Policies: ["
   echo $policies_file
   echo "Reading the content from the appconfig.text and splitting the content based on requirement"
   while IFS= read -r line
   do
     val=$(echo $line| sed 's/\\//g')
     val=${val:2:-2}
     key=$(echo $val| cut -d ":" -f1)
     value=$(echo $val| cut -d ":" -f2)
     policies_file+="
       {
         Role:$key,
         DisplayName:$key,
         AllowedActions:$value,
       },"
   done < "appconfig.txt"

   policies_file=${policies_file::-1}
   policies_file+="
       ],
   };"
   echo $policies_file > /app/src/utilities/policies.js

   if [ -f appconfig.txt ]; then

     echo " exist"
     echo "Removing the appconfig.txt which was generated in the script"
     rm -f appconfig.txt
   fi

  
else
     echo "Failed to ping Application Configuration"
     echo
     echo "Exiting set_env.sh"
     exit 1
fi
}
main() {
  # For the script to fetch the secrets from key-vault foll. variable
  # AppConfigurationConnectionString must be available as "environment" variables.
  if [[ "$AppConfigurationConnectionString" != ""  ]]; then
    if az appconfig kv list  --connection-string $AppConfigurationConnectionString > /dev/null; then
        echo "Pinged Application Configuration Successfully"
    else
        echo "Failed to ping Application Configuration"
        echo
        echo "Exiting set_env.sh"
        exit 1
    fi

    modify_webui_policies
    cat /app/src/utilities/policies.js
  else
    echo "Required AppConfiguration Connection String Infomation does not exist in Environment Variables, the following environment variables must be set to run this script:"
    echo "AppConfigurationConnectionString"
    echo
    echo "Exiting set_env.sh"
    exit 1
  fi
}

main