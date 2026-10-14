# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.3] - 2026-10-14

Still early beta, not yet playtested in game. Save-compatible with 0.1.2 (no records added or removed, no form IDs changed).

### Fixed
- Sigrun's status line after Gerda's ledger, and Gerda's ledger itself, could show a raw location tag (`<Alias=HideLoc>`). The status line no longer uses a tag (the hideout is still named in the journal and objective), and the ledger now uses the quest's stored text.

### Changed
- The build check now fails on any `<Alias=...>` tag in dialogue (responses, prompts, topic text) or quest text (name, journal, objectives) that is not a filled, non-optional alias of the quest that owns the text, and on alias-created notes whose tags would not resolve.

## [0.1.2] - 2026-10-13

Still early beta, not yet playtested in game.

### Fixed
- The quest giver's accept line showed a raw location alias.

### Added
- A build check for cross-quest alias tokens.

## [0.1.1] - 2026-10-11

Still early beta, not yet playtested in game.

### Fixed
- Gerda's ledger could be missing from her, so the quest never moved past "find the fence". The quest now gives the ledger and Torvald's sword to Gerda by script when it starts, and checks again when she loads and when she dies. If you already carry the ledger, the quest catches up.
- Picking up Torvald's sword when Gerda is dead and the ledger is gone now gives you the ledger.
- Stuck saves (0.1.0): with Torvald's sword in your inventory, tell Sigrun "Gerda had his sword, but no ledger." She hands it over; read it and the quest continues.

### Changed
- Gerda now waits at her dungeon's boss spot instead of the outdoor map marker (dungeons without one keep the marker).

## [0.1.0] - 2026-10-07

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, round trip, voice file and SEQ checks, FOMOD schema, 15-plugin series compatibility check) but has not been playtested in-game yet.

### Added
- "You're a long way from any hall, warden." Talk to Sigrun near the Markarth Stables (level 12+). She gives you Torvald's scored-out oath-scroll.
- The fence, Gerda Quick-Purse, hides in a dungeon in the Reach, Haafingar or Hjaalmarch. She carries Gerda's Ledger and Torvald's Oath-Sword (an optional objective).
- Read the ledger: it names Torvald's hideout in Falkreath, Whiterun or the Rift. He waits at the entrance and will talk.
- Torvald tells his side if you ask. Then choose.
- Choice: Duel: Oathbreaker's Plate (orcish cuirass, fortify health 40, heavy armor 15), 600 gold.
- Choice: Yield: Oathkeeper's Aegis (orcish shield, fortify block 20, resist magic 15), 600 gold.
- Choice: Redeem: His sword reforged as the Oath-Renewed Greatsword (fire + turn undead) and the Oathbreaker's Plate, 600 gold.
- Soft integration with Steel Oath (runtime lookup, not a master).
- Soft integration with Contract Hunter (runtime lookup, not a master).
- Unvoiced dialogue with 26 silent voice files; `SEQ/OathbreakersPrice.seq` for the start-game-enabled dialogue quest.
- FOMOD installer with optional Papyrus sources.
