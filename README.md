# buildingway-template

A `dotnet new` template for a new Dalamud plugin repository: the plugin, a game-independent Core
project with tests, thin workflow callers into
[buildingway-ci](https://github.com/Cellar-Mander/buildingway-ci), and doc stubs. Build settings
and code style come from the `Buildingway.Build` MSBuild SDK and the dev and release commands from
`Buildingway.Tools`, so the template carries no `Directory.Build.props`, `.editorconfig` or scripts.
Issue forms and the PR template come from the owner's `.github` repository.

## Using it

```bash
dotnet new install ./template
dotnet new dalamud-plugin -n SayMyName -o say-my-name \
  --displayName "Say My Name" \
  --pluginAuthor "Cellar Mander" \
  --repoUrl https://github.com/Cellar-Mander/say-my-name \
  --punchline "Get a sound and highlighted chat when other players mention you."
```

`-n` is the internal name: PascalCase, used for projects, namespaces and paths, and lower-cased
for the chat command (`/saymyname`). Then follow "New repository setup" at the end of the
generated `CONTRIBUTING.md`.

`displayName` and `punchline` are written into C# strings and XML as given, so leave out `"`,
`<` and `&`, or escape them by hand afterwards.

To pick up template changes, `dotnet new install ./template --force`.

## Checking a change

The Template workflow (`.github/workflows/template.yml`) generates a project into `sample/` with
`.github/generate-sample.sh` and runs on it what its own CI would: a check for unreplaced
placeholders, tool restore and the Core tests on Linux, formatting and the full build against the
pinned Dalamud on Windows, and actionlint and zizmor on its workflows. It runs on every push and
PR to main. Before generating a new repo, run it by hand (Actions > Template > Run workflow): the
10.0.x SDK, runner images and GitHub Packages access can change since the last run. Each `Buildingway.*` package on
GitHub Packages must grant this repository read access under Manage Actions access.

To try a change locally, run `.github/generate-sample.sh` from the repo root (it reinstalls the
template from this checkout), then build or test in `sample/`.

## Adding a lesson

How a lesson from a plugin repo becomes a change here or in the shared packages, and how it
reaches repos that already exist: [docs/lessons.md](docs/lessons.md).
