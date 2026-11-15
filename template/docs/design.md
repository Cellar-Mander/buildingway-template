# Design

What My Plugin Display Name does, stated precisely enough to test against. Mutation triage checks
surviving mutants against this file (`spec=` in `.claude/checks`), so write down each behaviour the
tests should hold, including edge cases and limits.

## Behaviour

Settings are saved half a second after the last change, so dragging a slider writes them once, and also when the settings window closes or the plugin unloads.

## Limits

Caps on user-configurable lists and text, and how input from other players or the game is bounded.
