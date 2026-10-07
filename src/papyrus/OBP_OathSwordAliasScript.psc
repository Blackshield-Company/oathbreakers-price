Scriptname OBP_OathSwordAliasScript extends ReferenceAlias
{Oathbreaker's Price - on Torvald's oath-sword.}

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		(GetOwningQuest() as OBP_MainScript).SwordTaken()
	EndIf
EndEvent
