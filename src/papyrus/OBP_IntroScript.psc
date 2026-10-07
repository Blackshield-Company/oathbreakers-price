Scriptname OBP_IntroScript extends Quest
{Oathbreaker's Price - always-running quest. Holds Sigrun near the Markarth Stables and her opening topic.}

Quest Property OBP_Main Auto
GlobalVariable Property OBP_State Auto
{0 = not started, 1 = running, 2 = Torvald killed, 3 = yielded, 4 = redeemed}
Message Property OBP_MsgNoStart Auto
ReferenceAlias Property Sigrun Auto

Function Accept()
	If OBP_State.GetValueInt() != 0 || OBP_Main.IsRunning()
		Return
	EndIf
	If OBP_Main.Start()
		OBP_State.SetValue(1)
		(OBP_Main as OBP_MainScript).Begin(Sigrun.GetReference())
		Debug.Trace("OBP_ questline started")
	Else
		Debug.Trace("OBP_ OBP_Main failed to start (needs two free dungeons)")
		OBP_MsgNoStart.Show()
	EndIf
EndFunction
