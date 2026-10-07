Scriptname OBP_TorvaldAliasScript extends ReferenceAlias
{Oathbreaker's Price - on Torvald.}

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as OBP_MainScript).TorvaldDied()
EndEvent
