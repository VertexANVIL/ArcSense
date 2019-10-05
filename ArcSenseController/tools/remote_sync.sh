#!/bin/bash

dotnet publish -r linux-arm /p:ShowLinkerSizeComparison=true
pushd ./bin/Debug/netcoreapp3.0/linux-arm/publish
rsync -avh --update * pi@arcproto-1:~/test/arcsense
popd