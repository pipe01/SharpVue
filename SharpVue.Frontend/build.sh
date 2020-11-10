#/bin/bash

#npm run build

INLINER=../HtmlInliner
GENERATOR=../SharpVue.Generator

CUR=$PWD

if cygpath -V >/dev/null; then
	CUR=$(cygpath -w $CUR)
fi

ENTRY=$CUR/dist/index.html

(cd $INLINER && go run . -html "$ENTRY" -out "$GENERATOR/Vue/Static/SingleFile/index.html")

cp -r dist/* $GENERATOR/Vue/Static/Regular/