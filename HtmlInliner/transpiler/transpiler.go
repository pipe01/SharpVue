package transpiler

import (
	"encoding/base64"
	"fmt"
	"io"
	"io/ioutil"
	"net/http"
	"os"
	"path"
	"strings"

	"golang.org/x/net/html"
)

type Transpiler struct {
	BasePath, JSPrefix, CSSPrefix, ImagePrefix string
}

func (tr *Transpiler) Transpile(in io.Reader, out io.Writer) error {
	z := html.NewTokenizer(in)

	for {
		t := z.Next()
		writeRaw := true

		switch t {
		case html.ErrorToken:
			return z.Err()

		case html.StartTagToken:
			tn, hasAttr := z.TagName()

			if !hasAttr {
				break
			}

			tag := string(tn)

			if tag == "script" {
				attrs := readAttrs(z)
				src, hasSrc := attrs["src"]

				if hasSrc && strings.HasPrefix(src, tr.JSPrefix) {
					fmt.Fprint(out, "<script>")
					if err := tr.writeFile(out, src); err != nil {
						return fmt.Errorf("failed to write script file '%s': %w", src, err)
					}

					writeRaw = false
					break
				}
			} else if tag == "link" {
				attrs := readAttrs(z)
				href, hasHref := attrs["href"]

				if hasHref && attrs["rel"] == "stylesheet" && strings.HasPrefix(href, tr.CSSPrefix) {
					fmt.Fprint(out, "<style>")
					if err := tr.writeFile(out, href); err != nil {
						return fmt.Errorf("failed to write style file '%s': %w", href, err)
					}
					fmt.Fprint(out, "</style>")

					writeRaw = false
					break
				}
			} else if tag == "img" {
				attrs := readAttrs(z)
				src, hasSrc := attrs["src"]

				if hasSrc && strings.HasPrefix(src, tr.ImagePrefix) {
					fullPath := path.Join(tr.BasePath, src)

					file, err := os.Open(fullPath)
					if err != nil {
						return fmt.Errorf("failed to open image file '%s': %w", src, err)
					}
					defer file.Close()

					data, err := ioutil.ReadAll(file)
					if err != nil {
						return fmt.Errorf("failed to read image file '%s': %w", src, err)
					}

					mimetype := http.DetectContentType(data)
					enc := base64.StdEncoding.EncodeToString(data)

					fmt.Fprintf(out, "<img src=\"data:%s;base64,%s\">", mimetype, enc)
					writeRaw = false
				}
			}
		}

		if writeRaw {
			out.Write(z.Raw())
		}
	}
}

func readAttrs(z *html.Tokenizer) map[string]string {
	attrs := make(map[string]string)

	for {
		key, val, more := z.TagAttr()
		attrs[string(key)] = string(val)

		if !more {
			break
		}
	}

	return attrs
}

func (tr *Transpiler) writeFile(w io.Writer, fpath string) error {
	fpath = path.Join(tr.BasePath, fpath)

	file, err := os.Open(fpath)
	if err != nil {
		return err
	}
	defer file.Close()

	io.Copy(w, file)
	return nil
}
