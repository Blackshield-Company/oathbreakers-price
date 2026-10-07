Scriptname OBP_TIF_Redeem extends TopicInfo Hidden
{Oathbreaker's Price - dialogue fragment: Torvald renews his oath.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as OBP_MainScript).Redeem()
EndFunction
