#!/bin/sh

rm -f ./nuget/*
dotnet pack --configuration Release --output ./nuget
# The source option here references 'github' in the nuget.config file
dotnet nuget push ./nuget/*.nupkg --api-key "$1" --source "https://nuget.pkg.github.com/mitchpaulus/index.json"
