Scriptname OBP_OathSwordAliasScript extends ReferenceAlias
{Oathbreaker's Price - on Torvald's oath-sword.}

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		OBP_MainScript main = GetOwningQuest() as OBP_MainScript
		main.SwordTaken()
		main.SwordPickedUp()
	EndIf
EndEvent
