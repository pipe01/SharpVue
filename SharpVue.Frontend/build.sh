#/bin/bash
set -e

INLINER=../HtmlInliner
GENERATOR=../SharpVue.Generator

#npm run build

CUR=$PWD

if cygpath -V >/dev/null; then
	CUR=$(cygpath -w $CUR)
fi

ENTRY=$CUR/dist/index.html

(cd $INLINER && go run . -html "$ENTRY" -out "$GENERATOR/Vue/Static/SingleFile/index.html")

GENERATOR=$(readlink -f "$GENERATOR")

(cd dist && tar czf $GENERATOR/Vue/Static/Regular.tar.gz *)