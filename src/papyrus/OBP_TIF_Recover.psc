Scriptname OBP_TIF_Recover extends TopicInfo Hidden
{Oathbreaker's Price - dialogue fragment (0.1.1): Gerda had no ledger; Sigrun hands it over.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as OBP_MainScript).RecoverLedger()
EndFunction
