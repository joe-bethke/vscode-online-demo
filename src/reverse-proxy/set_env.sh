#!/bin/bash
# Copyright (c) 3M. All rights reserved. 

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
modify_cors() {
  
  value=$(_get_configuration "Global:ClientAuth:CorsWhitelist" | sed 's/"//g' )
  echo $value
  if [ -z "$value" ]
  then
    echo "No CORSWhiteList found in App Config"
  else
    cors="add_header 'Access-Control-Allow-Headers' \$http_access_control_request_headers; add_header Access-Control-Allow-Credentials true always;        add_header Access-Control-Allow-Methods \"GET, PUT, POST, DELETE, HEAD, OPTIONS\" always;        add_header Access-Control-Allow-Origin $value always; if (\$request_method = OPTIONS ) {return 204;}"
    sed -i "s@# {{INJECT_CORS_HERE}}@$cors@g" /app/nginx.conf
    echo "Set CORSWhiteList"
  fi

}

main() {
  # For the script to fetch the secrets from key-vault foll. variable
  # AppConfigurationConnectionString must be available as "environment" variables.
  if [[ "$AppConfigurationConnectionString" != ""  ]]; then
    if az appconfig kv list  --connection-string $AppConfigurationConnectionString > /dev/null; then
        echo "Pinged Application Configuration Successfully"
        modify_cors
    else
        echo "Failed to ping Application Configuration"
        echo
        echo "Exiting set_env.sh"
        exit 1
    fi

  else
    echo "Required AppConfiguration Connection String Infomation does not exist in Environment Variables, the following environment variables must be set to run this script:"
    echo "AppConfigurationConnectionString"
    echo
    echo "Exiting set_env.sh"
    exit 1
  fi
}

main
