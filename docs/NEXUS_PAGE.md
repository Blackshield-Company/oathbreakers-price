# Nexus Mods page: Oathbreaker's Price

Early beta: v0.1.1 passed every automated build check but hasn't been playtested in-game. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** Oathbreaker's Price
- **Version:** 0.1.1 (early beta)
- **Category:** Quests and Adventures
- **Suggested tags:** Quests, Lore-Friendly, Dialogue, Choices and Consequences, Armour, Weapons, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT; allow modification with credit, no re-uploads of the archive.
- **File to upload:** `OathbreakersPrice-0.1.1.zip` (FOMOD), main file, named "Oathbreaker's Price".

## Description (paste into the BBCode editor)

Same text as `nexus/obp-description.txt` in the workspace:

```bbcode
[center][color=#e06c3c][b]EARLY RELEASE (v0.1.1)[/b]: new mod, not yet widely playtested. Please report bugs in the Bugs tab with your Papyrus log lines (grep OBP_). Source: [url=https://github.com/synthalorian/oathbreakers-price]GitHub[/url][/color][/center]

[center][size=6][b]OATHBREAKER'S PRICE[/b][/size]
[size=3][i]Nine pilgrims died because one knight ran. The Oath-Wardens want him found.[/i][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
[b]Status: Early beta (v0.1.1).[/b] Every automated build check passes, but it hasn't had wide in-game playtesting yet. It's playable now. Bug reports help shape 0.1.x.
An oathbreaker manhunt for the oath side of the series. [b]Warden Sigrun Oath-Keeper[/b] waits by the [b]Markarth Stables[/b]. A knight named [b]Torvald[/b] swore to guard nine pilgrims, ran when the Forsworn came, sold his oath-sword to a fence and vanished.

[size=4][b]The quest[/b][/size]
[list]
[*][b]"You're a long way from any hall, warden."[/b] Talk to Sigrun near the Markarth Stables (level 12+). She gives you Torvald's scored-out oath-scroll.
[*][b]The fence[/b], Gerda Quick-Purse, hides in a dungeon in the Reach, Haafingar or Hjaalmarch. She carries [b]Gerda's Ledger[/b] and [b]Torvald's Oath-Sword[/b] (an optional objective).
[*][b]Read the ledger[/b]: it names Torvald's hideout in Falkreath, Whiterun or the Rift. He waits at the entrance and will talk.
[*][b]Torvald[/b] tells his side if you ask. Then choose.
[/list]

[size=4][b]Choices and rewards[/b][/size]
[list]
[*][b]Duel[/b] ("Draw. Face your death like a knight." (or kill him any other way), then report): [b]Oathbreaker's Plate[/b] (orcish cuirass, fortify health 40, heavy armor 15), 600 gold
[*][b]Yield[/b] ("Yield. Answer to the warden." then report): [b]Oathkeeper's Aegis[/b] (orcish shield, fortify block 20, resist magic 15), 600 gold
[*][b]Redeem[/b] ("I recovered your oath-sword. Swear again." (needs the sword) or "Swear on my oath" (needs an active Steel Oath), then report): His sword reforged as the [b]Oath-Renewed Greatsword[/b] (fire + turn undead) [b]and[/b] the Oathbreaker's Plate, 600 gold
[/list]

[size=4][b]Enhanced items[/b][/size]
[list]
[*]Oathbreaker's Plate
[*]Oathkeeper's Aegis
[*]Torvald's Oath-Sword, upgradeable to the Oath-Renewed Greatsword
[*]Oath-Scroll, Gerda's Ledger
[/list]

[size=4][b]Part of the series[/b][/size]
Notices the other synthalorian mods when they're installed and never requires them. Each one is looked up at runtime with vanilla Papyrus (no SKSE, no masters):
[list]
[*][b]Steel Oath[/b]: Sigrun notices your oath (different opening), and with an active oath you can redeem Torvald by lending him yours, even without his sword.
[*][b]Contract Hunter[/b]: Sigrun adds a 300 gold hunter's bonus to the reward ([font=Courier New]OBP_HunterBonus[/font]).
[/list]

[size=4][b]Unvoiced dialogue that you can actually read[/b][/size]
The new lines are unvoiced and shown as subtitles. Every line ships with a silent voice file sized to its length, so subtitles stay on screen. [b]No Fuz Ro D'oh needed[/b]. Turn on dialogue subtitles.

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]OathbreakersPrice.esp[/b]. It's ESL-flagged. Safe to add mid-playthrough.

[size=4][b]Compatibility[/b][/size]
[list]
[*][b]No edits to vanilla records[/b], no cells, no navmesh. Skyrim.esm is the only master.
[*]New NPCs appear at vanilla map markers; mods that rebuild those spots may leave them standing somewhere odd.
[*]Compatible with all fifteen mods of the series (same author), checked by loading every plugin together: unique FormIDs, EditorIDs, scripts, globals and files, zero overrides.
[/list]

[size=4][b]What got scaled down / known issues[/b][/size]
[list]
[*]A yielded or redeemed Torvald fades out after the conversation; there is no escort back to Markarth and no later sighting on the roads.
[*]Gerda is a renamed bandit boss; she doesn't talk.
[*]Sigrun and Torvald are new NPCs at vanilla map markers; no cell edits.
[*]Not playtested in-game.
[*]Needs two free dungeons with map markers at quest start.
[*]Torvald is not essential: if something else kills him first, the quest moves to the duel ending.
[*]Settings are console globals: [code]set OBP_MinLevel to 12 / set OBP_RewardGold to 600 / set OBP_HunterBonus to 300[/code]
[/list]

[size=4][b]Uninstalling[/b][/size]
As with any scripted mod, finish the quest before removing it.

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/synthalorian/oathbreakers-price]github.com/synthalorian/oathbreakers-price[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]

```
