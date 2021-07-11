#!/bin/sh

pubPath='publish';
deployPath='../unity/Assets/bin'

cd "src/" && pwd;

sudo dotnet publish Gram.sln -o publish --no-dependencies

rm -r "$deployPath"
mkdir "$deployPath"

cp -r "$pubPath" "$deployPath";


echo "Finished!!"