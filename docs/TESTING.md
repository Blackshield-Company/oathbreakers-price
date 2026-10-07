# In-game test checklist

The build verifies the plugin, scripts, dialogue structure, voice files and SEQ automatically; none of that proves the quest plays well. Turn on **dialogue subtitles** and Papyrus logging:

```ini
; Skyrim.ini (or SkyrimCustom.ini), [Papyrus]
bEnableLogging=1
bEnableTrace=1
bLoadDebugInformation=1
```

Every log line from this mod starts with `OBP_`. On each load you should see `OBP_ series check: ...` with the detected series mods. `help OBP_ 4` lists the records (ESL-flagged, FormIDs look like `FExxx8xx`).

## Opening

- [ ] Sigrun at the Markarth Stables map marker; opening at level 12+. With Steel Oath installed, the opening changes.
- [ ] Accept: oath-scroll added, stage 10.

## Hunt

- [ ] Gerda (hostile) at the first dungeon entrance; taking her ledger: stage 15 (+ optional sword objective if you don't have the sword).
- [ ] Reading the ledger: stage 20, Torvald enabled at the second dungeon.

## Endings (save at stage 20)

- [ ] Duel: Torvald hostile; death: stage 50; Sigrun: plate + gold.
- [ ] Yield: fades out, stage 60; Sigrun: shield + gold.
- [ ] Redeem with the sword: stage 70; Sigrun: sword swapped for the Oath-Renewed Greatsword + plate.
- [ ] Redeem without the sword: Torvald refuses; with an active Steel Oath, "Swear on my oath" works instead.

## Series integration

- [ ] Without any other series mod: the plain opening is offered, the log shows all integration values 0, nothing errors.
- [ ] With each listed mod installed, its variant appears (see README, Series integration).

## What to report

Your game version, load order, the `OBP_` lines from Papyrus.0.log, and which step went wrong.
