Scriptname OBP_FenceLedgerAliasScript extends ReferenceAlias
{Oathbreaker's Price - on Gerda's ledger: taking it, then reading it.}

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		(GetOwningQuest() as OBP_MainScript).LedgerTaken()
	EndIf
EndEvent

Event OnRead()
	Utility.Wait(0.1)
	OBP_MainScript main = GetOwningQuest() as OBP_MainScript
	main.LedgerTaken()
	main.LedgerRead()
EndEvent
