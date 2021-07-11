#!/bin/sh

pubPath='publish';
deployPath='../unity/Assets/bin'

cd "src/" && pwd;

dotnet publish Gram.sln -o publish --no-dependencies

rm -r "$deployPath"
mkdir "$deployPath"

find "$pubPath" -type f  \( -iname "Gram.Rpg.*" ! -iname ".deps.json" \) -exec cp "{}" "$deployPath" \;

echo "Finished!!"