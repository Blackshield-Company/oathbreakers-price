Scriptname OBP_SeriesScript extends ReferenceAlias
{Oathbreaker's Price - soft integration with the rest of the series, on the player alias of OBP_Intro.
Plugins are looked up at runtime (vanilla Papyrus, no SKSE); when one is missing the call returns None.}

GlobalVariable Property OBP_SeriesSOT Auto
{0 = Steel Oath not installed, otherwise SOT_ActiveCount + 1 (2 or more = the player has an active oath)}
GlobalVariable Property OBP_SeriesCHT Auto
{0 = Contract Hunter not installed, otherwise CHT_BountiesClaimed + 1}

Event OnInit()
	Refresh()
EndEvent

Event OnPlayerLoadGame()
	Refresh()
EndEvent

Function Refresh()
	GlobalVariable sotActive = Game.GetFormFromFile(0x80C, "SteelOath.esp") as GlobalVariable ; soft: SOT_ActiveCount
	If sotActive
		OBP_SeriesSOT.SetValue(sotActive.GetValue() + 1.0)
	Else
		OBP_SeriesSOT.SetValue(0.0)
	EndIf
	GlobalVariable chtClaimed = Game.GetFormFromFile(0x81C, "ContractHunter.esp") as GlobalVariable ; soft: CHT_BountiesClaimed
	If chtClaimed
		OBP_SeriesCHT.SetValue(chtClaimed.GetValue() + 1.0)
	Else
		OBP_SeriesCHT.SetValue(0.0)
	EndIf
	Debug.Trace("OBP_ series check: SOT " + OBP_SeriesSOT.GetValueInt() + ", CHT " + OBP_SeriesCHT.GetValueInt())
EndFunction
