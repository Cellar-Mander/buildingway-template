# From a lesson to a template change

A lesson is something a plugin repo learned the hard way: a release that broke, a check that let a
bug through, a setup step someone forgot. This doc covers how one gets into the template and the
shared packages, and how the change then reaches repos that already exist.

The first round of lessons, taken from say-my-name's history, is in
[say-my-name-inventory.md](say-my-name-inventory.md). That was a one-off snapshot. New lessons
follow the steps below.

## 1. Record the lesson where it happened

When a fix or debrief in a plugin repo turns up something every plugin should have, add a task
to **this** repo's ebd project (`project:buildingway-template`), usually a `+chore`, with a
`Lesson:` note naming where it came from:

```
task add project:buildingway-template +chore "Fail the release when the CHANGELOG has no entry"
task annotate <ref> 'Why now: say-my-name shipped v1.0.0.1 with empty release notes'
task annotate <ref> 'Lesson: say-my-name `fcb4e8b`, or debrief commitment `<ref>`'
```

Point the `Lesson:` note at the commit, PR or debrief commitment that shows what went wrong. If
there is no single source, say what happened in one line.

## 2. Link it to the debrief commitment, if one led to it

A debrief commitment (`+debrief`) that calls for a template change is carried out by that change:

- The template task's `Lesson:` note names the commitment's ref.
- The template PR's description names both refs.
- The commitment is done only once the change has reached the repos it was about (step 5), not
  when the template PR merges. Completing debrief commitments stays with the user.

## 3. Put the change where it will spread

Pick the lowest layer the lesson belongs to. Changes to the shared packages reach existing repos
through a version bump, but files copied in by the template have to be ported to each repo by
hand.

| The lesson is about | Change it in | Reaches existing repos by |
|---|---|---|
| Build settings, analyzers, code style | [buildingway-build](https://github.com/Cellar-Mander/buildingway-build) (`Buildingway.Build` SDK) | Bumping `msbuild-sdks` in `global.json` |
| Dev, release or setup commands | [buildingway-tools](https://github.com/Cellar-Mander/buildingway-tools) (`Buildingway.Tools`) | Bumping `.config/dotnet-tools.json` |
| CI jobs: tests, PR build, release, mutation, workflow lint | [buildingway-ci](https://github.com/Cellar-Mander/buildingway-ci) | Moving the SHA pin in each caller workflow |
| GitHub settings: ruleset, labels, Actions permissions | `setup-repo` in Buildingway.Tools | A Tools bump, then `dotnet buildingway setup-repo` |
| Repo files: docs, csproj and test scaffolding, caller workflows, `dependabot.yml` | `template/` here | A hand port in each repo |

If a lesson needs a template file to change on every repo, think about moving that logic into
one of the packages instead, so the next change is only a bump.

## 4. Make the change

- **A package or buildingway-ci:** change it there and release it by pushing a `v<semver>` tag
  (see "Releasing" in the build and tools READMEs, "Changing this repo" in buildingway-ci's). Then move the template to the new version in a
  PR here, as in `bde6693` (buildingway-ci v0.7.0) or `5fcf096` (Buildingway.Build 0.2.0), and let
  the Template workflow run on it.
- **The template:** change `template/` and check it with the Template workflow, or locally with
  `.github/generate-sample.sh` (see "Checking a change" in the [README](../README.md)).

Either way, add a `Lesson:` trailer to the template commit so the history can be searched:

```
Fail the release when the CHANGELOG has no entry

Lesson: say-my-name fcb4e8b
```

```bash
git log --format='%h %s%n  %(trailers:key=Lesson,valueonly)' --grep='^Lesson:'
```

## 5. Bring it to existing repos

Repos generated earlier don't change when the template does. `dotnet new install ./template
--force` only affects repos generated after it.

- **buildingway-ci pins:** each repo's Dependabot `github-actions` entry proposes the new SHA as a
  PR, after its 7-day cooldown. Merge it, or move the pin by hand to get it sooner.
- **Buildingway.Build and Buildingway.Tools:** each repo's Dependabot `nuget` entry covers
  `global.json` and `dotnet-tools.json`, but these packages come from GitHub Packages, which
  needs a token even to read, and `dependabot.yml` has no registry for it. Until a repo has
  received a Dependabot PR for a `Buildingway.*` package, assume it won't, and bump the version by
  hand.
- **Template files:** generate a sample from the changed template (`.github/generate-sample.sh`)
  and diff the changed files against the repo's own copies. Port the change, keeping whatever the
  repo has made its own. Give each repo's port a task in that repo's ebd project, so it isn't lost.
- **GitHub settings:** after the Tools bump, run `dotnet buildingway setup-repo --check` in the
  repo, then `setup-repo` if it reports differences. It overwrites settings that differ.

Once every repo the lesson was about has the change, annotate the template task with where it
landed. If a debrief commitment led to it, it can now be completed.
