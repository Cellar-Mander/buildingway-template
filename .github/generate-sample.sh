#!/usr/bin/env bash
# Generates a project from ./template into ./sample, as the README's "Using it" does. Used by
# workflows/template.yml; run it locally from the repo root to try a template change.
set -euo pipefail

name=${SAMPLE_NAME:-TemplateCheck}

rm -rf sample
dotnet new install ./template --force
dotnet new dalamud-plugin -n "$name" -o sample \
  --displayName "Template Check" \
  --pluginAuthor "Template CI" \
  --repoUrl "https://github.com/Cellar-Mander/template-check" \
  --punchline "Checks that the template generates a working project."
