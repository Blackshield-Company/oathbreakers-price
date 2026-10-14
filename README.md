# Oathbreaker's Price

*Nine pilgrims died because one knight ran. The Oath-Wardens want him found.*

An oathbreaker manhunt for the oath side of the series. **Warden Sigrun Oath-Keeper** waits by the **Markarth Stables**. A knight named **Torvald** swore to guard nine pilgrims, ran when the Forsworn came, sold his oath-sword to a fence and vanished.

**Status: v0.1.3, early beta.** It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, byte-exact round trip, voice file and SEQ checks, FOMOD schema, the series compatibility check) but has not been playtested in-game yet. Expect rough edges and please report what you find. See [docs/TESTING.md](docs/TESTING.md).

- Game: Skyrim Special Edition / Anniversary Edition 1.6.x (also fine on 1.5.97). Works on Windows and on Linux through Proton.
- Requirements: none beyond the base game. No SKSE, no SkyUI, no DLC, no Creation Club content required.
- Plugin: `OathbreakersPrice.esp`, ESL-flagged, Skyrim.esm is the only master. **No vanilla records edited** (zero overrides).
- Part of the synthalorian series and compatible with all of it: [Blackshield Company](https://github.com/synthalorian/blackshield-company), [Bounties of Oblivion](https://github.com/synthalorian/bounties-of-oblivion), [Contract Hunter](https://github.com/synthalorian/contract-hunter), [Steel Oath](https://github.com/synthalorian/steel-oath), [Ember Smithing](https://github.com/synthalorian/ember-smithing), [The Gray-Mane Debt](https://github.com/synthalorian/the-gray-mane-debt), [Blackshield Codex](https://github.com/synthalorian/blackshield-codex), [War Banners](https://github.com/synthalorian/war-banners), [Roadwarden](https://github.com/synthalorian/roadwarden), [Sellsword Roster](https://github.com/synthalorian/sellsword-roster), [The Turncoat Contract](https://github.com/synthalorian/the-turncoat-contract), [The Ledger of Ash](https://github.com/synthalorian/the-ledger-of-ash), [Oathbreaker's Price](https://github.com/synthalorian/oathbreakers-price), [Champion's Reforge](https://github.com/synthalorian/champions-reforge), [The Barrow-Iron Pact](https://github.com/synthalorian/barrow-iron-pact). Own prefix (`OBP_`), records, globals, scripts and files; checked by loading all 15 plugins together (see [tools/modkit](tools/modkit/README.md)).

## The quest

1. **"You're a long way from any hall, warden."** Talk to Sigrun near the Markarth Stables (level 12+). She gives you Torvald's scored-out oath-scroll.
2. **The fence**, Gerda Quick-Purse, hides in a dungeon in the Reach, Haafingar or Hjaalmarch. She carries **Gerda's Ledger** and **Torvald's Oath-Sword** (an optional objective).
3. **Read the ledger**: it names Torvald's hideout in Falkreath, Whiterun or the Rift. He waits at the entrance and will talk.
4. **Torvald** tells his side if you ask. Then choose.

| Choice | How | Reward |
|---|---|---|
| Duel | "Draw. Face your death like a knight." (or kill him any other way), then report | **Oathbreaker's Plate** (orcish cuirass, fortify health 40, heavy armor 15), 600 gold |
| Yield | "Yield. Answer to the warden." then report | **Oathkeeper's Aegis** (orcish shield, fortify block 20, resist magic 15), 600 gold |
| Redeem | "I recovered your oath-sword. Swear again." (needs the sword) or "Swear on my oath" (needs an active Steel Oath), then report | His sword reforged as the **Oath-Renewed Greatsword** (fire + turn undead) **and** the Oathbreaker's Plate, 600 gold |

## Items

- Oathbreaker's Plate
- Oathkeeper's Aegis
- Torvald's Oath-Sword, upgradeable to the Oath-Renewed Greatsword
- Oath-Scroll, Gerda's Ledger

## Series integration (optional, no masters)

When other mods of the series are installed, this one notices. Nothing here is a master and nothing is required: a script on the player looks each plugin up at runtime with `Game.GetFormFromFile` (vanilla Papyrus since 2012, no SKSE) on every game load, and when the plugin is missing the call returns None and that feature is skipped.

| Mod | Reads | Effect |
|---|---|---|
| Steel Oath | `SOT_ActiveCount` | Sigrun notices your oath (different opening), and with an active oath you can redeem Torvald by lending him yours, even without his sword. |
| Contract Hunter | `CHT_BountiesClaimed` | Sigrun adds a 300 gold hunter's bonus to the reward (`OBP_HunterBonus`). |

## Unvoiced dialogue

All 26 new lines are unvoiced, shown as subtitles, and each one ships with a silent voice file sized to its length, so subtitles stay on screen (same approach as The Gray-Mane Debt). Turn subtitles on. Fuz Ro D'oh is not needed and doesn't conflict.

## Install

Grab `OathbreakersPrice-0.1.3.zip` from [Releases](https://github.com/synthalorian/oathbreakers-price/releases) (FOMOD installer).

- **Mod Organizer 2** or **Vortex**: install the zip and enable `OathbreakersPrice.esp`.
- **Manual (Linux/Proton)**: copy everything inside `00 Core/` (plugin, `Scripts/`, `SEQ/`, `Sound/`) into `.../steamapps/common/Skyrim Special Edition/Data/` and add `*OathbreakersPrice.esp` to `Plugins.txt` in the Proton prefix (`.../compatdata/489830/pfx/drive_c/users/steamuser/AppData/Local/Skyrim Special Edition/`).

Safe to add to an existing save. Finish the quest before uninstalling.

## Console tunables

```
set OBP_MinLevel to 12
set OBP_RewardGold to 600
set OBP_HunterBonus to 300
```

## What got scaled down

- A yielded or redeemed Torvald fades out after the conversation; there is no escort back to Markarth and no later sighting on the roads.
- Gerda is a renamed bandit boss; she doesn't talk.
- Sigrun and Torvald are new NPCs at vanilla map markers; no cell edits.

## Known issues and risks

- Not playtested in-game.
- Needs two free dungeons with map markers at quest start.
- Torvald is not essential: if something else kills him first, the quest moves to the duel ending.

## Build from source

`./build.sh` on Linux builds the release zip: Papyrus is compiled with the open-source [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler) against clean-room vanilla declaration stubs, the plugin is generated from C# with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) (`src/plugin/OathbreakersPrice.Generator`: `Mod.cs` holds this mod, `Kit.cs`/`Program.cs`/`Vanilla.cs` are shared by the third wave), the SEQ file and silent voice files are written next to it, and everything is validated. The zip is reproducible: a fresh clone rebuilds it byte for byte. `src/plugin/formids.json` pins every FormID; never edit or reuse entries.

Current build: 58 new records (FormIDs 800-839): Armor 2, Book 2, DialogBranch 4, DialogResponses 18, DialogTopic 11, Faction 2, FormList 2, GlobalFloat 6, Message 1, Npc 3, ObjectEffect 3, Quest 2, Weapon 2; 11 Papyrus scripts; 26 silent voice files; 1 SEQ entry.

## Credits

- Built with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) and [russo-2025/papyrus-compiler](https://github.com/russo-2025/papyrus-compiler).
- Skyrim and every referenced vanilla record and mesh belong to Bethesda Softworks. This mod references them from Skyrim.esm and ships no Bethesda assets.
