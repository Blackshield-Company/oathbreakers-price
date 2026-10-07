Scriptname OBP_TIF_Reward extends TopicInfo Hidden
{Oathbreaker's Price - dialogue fragment: report to Sigrun.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as OBP_MainScript).Reward()
EndFunction
