package main

import (
	"flag"
	"log"
	"os"
	"path"

	"github.com/pipe01/SharpVue/HtmlInliner/transpiler"
)

func main() {
	t := &transpiler.Transpiler{}

	var htmlPath, outPath string
	flag.StringVar(&htmlPath, "html", "", "The path of the HTML entrypoint file")
	flag.StringVar(&outPath, "out", "", "The path of the output HTML file")
	flag.StringVar(&t.JSPrefix, "js-prefix", "/js", "All script files that start with this string will be inlined")
	flag.StringVar(&t.CSSPrefix, "css-prefix", "/css", "All style files that start with this string will be inlined")
	flag.StringVar(&t.ImagePrefix, "img-prefix", "/img", "All image files that start with this string will be inlined")
	flag.StringVar(&t.BasePath, "base-path", "", "Base path for all inlineable files. Defaults to the directory that the entrypoint is in")
	flag.Parse()

	if t.BasePath == "" {
		t.BasePath = path.Dir(htmlPath)
	}

	file, err := os.Open(htmlPath)
	if err != nil {
		log.Fatalf("failed to open html file: %s", err)
	}
	defer file.Close()

	out, err := os.Create(outPath)
	if err != nil {
		log.Fatalf("failed to open output file: %s", err)
	}
	defer out.Close()

	if err := t.Transpile(file, out); err != nil {
		log.Fatalf("failed to transpile: %s", err)
	}
}
