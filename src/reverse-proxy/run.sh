#!/bin/bash
/app/set_env.sh
echo "Starting server"
cat /app/nginx.conf
cp /app/nginx.conf /etc/nginx/nginx.conf
nginx -c /app/nginx.conf