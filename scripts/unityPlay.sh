#!/bin/sh

cd /work/darts

/usr/bin/osascript  scripts/bringAllUnityToFront.scpt

scripts/deleteLogs.sh

PATH=$PATH:/usr/local/bin

/usr/local/bin/jake unity:editor:play > /dev/null
