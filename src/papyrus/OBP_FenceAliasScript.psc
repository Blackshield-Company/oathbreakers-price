Scriptname OBP_FenceAliasScript extends ReferenceAlias
{Oathbreaker's Price - on Gerda the fence. 0.1.1: re-checks that she carries the ledger and Torvald's sword.}

Event OnLoad()
	(GetOwningQuest() as OBP_MainScript).EnsureItems()
EndEvent

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as OBP_MainScript).EnsureItems()
EndEvent
