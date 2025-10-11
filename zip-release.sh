# Arguments:
# 1. The directory: 'publish-win-x64'
# 2. The version:   'v1.3.0'

attr=$(echo "$1" | cut -b 8-) # Remove 'publish-'
cd $1
zip "builder-builder-$2-$attr.zip" -r ./
cd ../
