/app/set_policies.sh
/app/build.sh

if /app/set_env.sh; then # Set App config values for Auth Config
  echo "Set Variables"
else
  exit 1
fi

#Move new file to destination
cp /app/webui-config.js /app/build/webui-config.js
echo "Starting server"
nginx -c /app/nginx.conf
