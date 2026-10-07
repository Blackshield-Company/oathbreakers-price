Scriptname OBP_TIF_Accept extends TopicInfo Hidden
{Oathbreaker's Price - dialogue fragment: accept.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as OBP_IntroScript).Accept()
EndFunction
