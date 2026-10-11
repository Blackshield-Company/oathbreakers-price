Scriptname OBP_MainScript extends Quest
{Oathbreaker's Price - questline controller.
Stages: 10 find the fence, 15 ledger taken, 20 find Torvald, 40 confronted, 50 killed, 60 yielded, 70 redeemed,
100/110/120 completed (killed / yielded / redeemed).}

ReferenceAlias Property Sigrun Auto
ReferenceAlias Property Torvald Auto
GlobalVariable Property OBP_State Auto
GlobalVariable Property OBP_RewardGold Auto
GlobalVariable Property OBP_HunterBonus Auto
GlobalVariable Property OBP_SeriesCHT Auto
Armor Property OBP_OathbreakerPlate Auto
Armor Property OBP_OathkeeperAegis Auto
Weapon Property OBP_OathSword Auto
Weapon Property OBP_OathRenewed Auto
Faction Property OBP_OathbreakerFaction Auto
Faction Property PlayerFaction Auto
MiscObject Property Gold001 Auto
ReferenceAlias Property Fence Auto
ReferenceAlias Property FenceBoss Auto
{Optional: the fence's dungeon boss spot.}
ReferenceAlias Property FenceLedger Auto
ReferenceAlias Property OathSwordRef Auto
Form Property OBP_FenceLedgerItem Auto

Bool PendingDismiss = False
Bool PendingStop = False

Function Begin(ObjectReference akSigrun)
	If akSigrun
		Sigrun.ForceRefTo(akSigrun)
	EndIf
	SetStage(10)
	PlaceAtBoss(Fence, FenceBoss)
	EnsureItems()
	SetObjectiveDisplayed(10)
EndFunction

; 0.1.1: at quest start, and when Gerda loads or dies.
Function EnsureItems()
	If GetStage() != 10
		Return
	EndIf
	If Game.GetPlayer().GetItemCount(OBP_FenceLedgerItem) > 0
		LedgerTaken()
		Return
	EndIf
	EnsureOn(Fence, FenceLedger, OBP_FenceLedgerItem)
	EnsureOn(Fence, OathSwordRef, OBP_OathSword)
EndFunction

; 0.1.1: Torvald's sword was picked up. If Gerda is dead and the ledger is gone, hand it over.
Function SwordPickedUp()
	EnsureItems()
	If GetStage() == 10 && Lost(Fence, OBP_FenceLedgerItem)
		RecoverLedger()
	EndIf
EndFunction

; 0.1.1: Sigrun's "Gerda had no ledger" dialogue.
Function RecoverLedger()
	If GetStage() != 10
		Return
	EndIf
	GiveToPlayer(FenceLedger, OBP_FenceLedgerItem)
	LedgerTaken()
EndFunction

Function LedgerTaken()
	If GetStage() != 10
		Return
	EndIf
	SetObjectiveCompleted(10)
	SetStage(15)
	SetObjectiveDisplayed(15)
	If Game.GetPlayer().GetItemCount(OBP_OathSword) == 0
		SetObjectiveDisplayed(16)
	EndIf
EndFunction

Function LedgerRead()
	If GetStage() != 15
		Return
	EndIf
	SetObjectiveCompleted(15)
	SetStage(20)
	ObjectReference t = Torvald.GetReference()
	If t && t.IsDisabled()
		t.Enable()
	EndIf
	SetObjectiveDisplayed(20)
EndFunction

Function SwordTaken()
	If IsObjectiveDisplayed(16)
		SetObjectiveCompleted(16)
	EndIf
EndFunction

Function Duel()
	If GetStage() != 20
		Return
	EndIf
	SetStage(40)
	OBP_OathbreakerFaction.SetEnemy(PlayerFaction)
	Actor t = Torvald.GetActorReference()
	If t && !t.IsDead()
		t.SetActorValue("Aggression", 1.0)
		t.StartCombat(Game.GetPlayer())
	EndIf
EndFunction

Function TorvaldDied()
	If GetStage() < 20 || GetStage() >= 50
		Return
	EndIf
	SetObjectiveCompleted(20)
	SetObjectiveDisplayed(16, False)
	SetStage(50)
	SetObjectiveDisplayed(50)
EndFunction

Function Yield()
	If GetStage() != 20
		Return
	EndIf
	SetStage(40)
	SetObjectiveCompleted(20)
	SetObjectiveDisplayed(16, False)
	SetStage(60)
	SetObjectiveDisplayed(60)
	PendingDismiss = True
	RegisterForSingleUpdate(4.0)
EndFunction

; Torvald swears again, on his recovered sword or on the player's Steel Oath. The sword is reforged.
Function Redeem()
	If GetStage() != 20
		Return
	EndIf
	SetStage(40)
	SetObjectiveCompleted(20)
	SetObjectiveDisplayed(16, False)
	SetStage(70)
	SetObjectiveDisplayed(70)
	PendingDismiss = True
	RegisterForSingleUpdate(4.0)
EndFunction

Function Reward()
	Int stage = GetStage()
	If stage != 50 && stage != 60 && stage != 70
		Return
	EndIf
	Actor player = Game.GetPlayer()
	Int gold = OBP_RewardGold.GetValueInt()
	If OBP_SeriesCHT.GetValueInt() >= 1
		gold += OBP_HunterBonus.GetValueInt()
	EndIf
	player.AddItem(Gold001, gold)
	If stage == 50
		player.AddItem(OBP_OathbreakerPlate, 1)
		SetObjectiveCompleted(50)
		OBP_State.SetValue(2)
		SetStage(100)
	ElseIf stage == 60
		player.AddItem(OBP_OathkeeperAegis, 1)
		SetObjectiveCompleted(60)
		OBP_State.SetValue(3)
		SetStage(110)
	Else
		Bool wasEquipped = player.IsEquipped(OBP_OathSword)
		If player.GetItemCount(OBP_OathSword) > 0
			player.RemoveItem(OBP_OathSword, 1, True)
		EndIf
		player.AddItem(OBP_OathRenewed, 1)
		If wasEquipped
			player.EquipItem(OBP_OathRenewed, False, True)
		EndIf
		player.AddItem(OBP_OathbreakerPlate, 1)
		SetObjectiveCompleted(70)
		OBP_State.SetValue(4)
		SetStage(120)
	EndIf
	PendingStop = True
	RegisterForSingleUpdate(4.0)
EndFunction

Event OnUpdate()
	If PendingDismiss
		PendingDismiss = False
		Actor t = Torvald.GetActorReference()
		If t && !t.IsDead() && !t.IsDisabled()
			t.Disable(True)
		EndIf
	EndIf
	If PendingStop
		PendingStop = False
		Debug.Trace("OBP_ questline finished at stage " + GetStage() + ", state " + OBP_State.GetValueInt())
		Stop()
	EndIf
EndEvent

; ---------------------------------------------------------------------------
; 0.1.1: alias-created quest items could be missing from the NPC that should carry them
; (same bug as Champion's Reforge 0.1.0 / The Gray-Mane Debt 0.1.0). These re-add them by script.
; ---------------------------------------------------------------------------

; Make sure akHolder carries akBase (unless the player already has it).
Function EnsureOn(ReferenceAlias akHolder, ReferenceAlias akItem, Form akBase)
	Actor h = akHolder.GetReference() as Actor
	If !h || !akBase || Game.GetPlayer().GetItemCount(akBase) > 0 || h.GetItemCount(akBase) > 0
		Return
	EndIf
	ObjectReference r = akItem.GetReference()
	If !r
		r = h.PlaceAtMe(akBase, 1, True)
		akItem.ForceRefTo(r)
	EndIf
	h.AddItem(r, 1, True)
	Debug.Trace("OBP_ EnsureOn: re-added " + akBase + " to " + h)
EndFunction

; Hand akBase straight to the player (unstick path). The item alias's own pickup event then advances the quest.
Function GiveToPlayer(ReferenceAlias akItem, Form akBase)
	Actor player = Game.GetPlayer()
	If !akBase || player.GetItemCount(akBase) > 0
		Return
	EndIf
	ObjectReference r = akItem.GetReference()
	If !r
		r = player.PlaceAtMe(akBase, 1, True)
		akItem.ForceRefTo(r)
	EndIf
	player.AddItem(r, 1)
	Debug.Trace("OBP_ GiveToPlayer: " + akBase)
EndFunction

; True when the carrier is dead or gone and no longer has the item.
Bool Function Lost(ReferenceAlias akHolder, Form akBase)
	Actor h = akHolder.GetReference() as Actor
	Return !h || (h.IsDead() && h.GetItemCount(akBase) == 0)
EndFunction

; Move a spawned NPC from the outdoor map marker to the location's boss spot, if it has one.
Function PlaceAtBoss(ReferenceAlias akActor, ReferenceAlias akSpot)
	Actor a = akActor.GetReference() as Actor
	ObjectReference spot = akSpot.GetReference()
	If a && spot && !a.IsDead()
		a.MoveTo(spot)
		Debug.Trace("OBP_ moved " + a + " to the boss spot")
	ElseIf a
		Debug.Trace("OBP_ no boss spot for " + a + "; stays at the map marker")
	EndIf
EndFunction
