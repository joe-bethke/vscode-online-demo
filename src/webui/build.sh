#!/usr/bin/env bash
export NODE_PATH=src/  # fixes the "cannot find module app.config" issue

echo "Building app..."              && npm run build
#echo "Removing temp files..."       && rm -rf node_modules src public package.json Dockerfile .dockerignore

set -e
pwd && ls

# copying webui config
cp /app/public/webui-config.js /app/webui-config.js

# call in current shell.
echo "Creating/Updating web config"

#create log directories for nginx
mkdir -p /app/logs