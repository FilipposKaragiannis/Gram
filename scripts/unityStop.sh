#!/bin/sh

cd /work/darts

PATH=$PATH:/usr/local/bin

/usr/local/bin/jake unity:editor:stop > /dev/null
