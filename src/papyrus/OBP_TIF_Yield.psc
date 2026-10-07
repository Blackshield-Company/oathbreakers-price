Scriptname OBP_TIF_Yield extends TopicInfo Hidden
{Oathbreaker's Price - dialogue fragment: Torvald yields.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as OBP_MainScript).Yield()
EndFunction
