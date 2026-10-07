using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using static Wave3Gen.Kit;
using static Wave3Gen.Vanilla;

namespace Wave3Gen;

/// <summary>Oathbreaker's Price: everything specific to this mod. Kit/Program/Vanilla/ItemData are shared.</summary>
internal static class ModInfo
{
    public const string PluginName = "OathbreakersPrice.esp";
    public const string Description = "Oathbreaker's Price: a manhunt for an oathbreaker knight with four endings and legendary gear.";

    const int APlayer = 0, ASigrun = 1, AScroll = 2, AAccLoc = 3, AAccMarker = 4, AFence = 5, AFenceLedger = 6, AOathSword = 7,
        AHideLoc = 8, AHideMarker = 9, ATorvald = 10;
    const int IPlayer = 0, IMarker = 1, ISigrun = 2;

    public static void Build(Kit k)
    {
        var qIntro = k.Id("OBP_Intro");
        var qMain = k.Id("OBP_Main");

        var gState = k.Global("OBP_State", 0);   // 0 new, 1 running, 2 killed, 3 surrendered, 4 redeemed
        var gMinLevel = k.Global("OBP_MinLevel", 12);
        var gReward = k.Global("OBP_RewardGold", 600);
        var gBounty = k.Global("OBP_HunterBonus", 300);
        var gSOT = k.Global("OBP_SeriesSOT", 0);   // 0 = Steel Oath not installed, else SOT_ActiveCount + 1
        var gCHT = k.Global("OBP_SeriesCHT", 0);   // 0 = Contract Hunter not installed, else CHT_BountiesClaimed + 1

        var wardens = k.Faction("OBP_WardenFaction", "Oath-Wardens");
        var forsworn = k.Faction("OBP_OathbreakerFaction", "Oathbreakers");
        var accHolds = k.List("OBP_FenceHolds", new[] { ReachHoldLocation, HaafingarHoldLocation, HjaalmarchHoldLocation });
        var hideHolds = k.List("OBP_HideHolds", new[] { FalkreathHoldLocation, WhiterunHoldLocation, RiftHoldLocation });

        // --- Legendary gear ------------------------------------------------------------------------------
        var plateEnch = k.ArmorEnchant("OBP_EnchOathbreakerPlate", "Broken Oath", (EnchFortifyHealthConstantSelf, 40), (EnchFortifyHeavyArmorConstantSelf, 15));
        var plate = k.ArmorLike("Orcish Armor", "OBP_OathbreakerPlate", T.Plate, 44, 2400, plateEnch);
        var aegisEnch = k.ArmorEnchant("OBP_EnchOathkeeperAegis", "Kept Oath", (EnchFortifyBlockConstantSelf, 20), (EnchResistMagicConstantSelf, 15));
        var aegis = k.ArmorLike("Orcish Shield", "OBP_OathkeeperAegis", T.Aegis, 33, 2000, aegisEnch);
        var oathSword = k.WeaponLike("Steel Greatsword", "OBP_OathSword", T.OathSword, 17, 300, null);
        var renewEnch = k.WeaponEnchant("OBP_EnchRenewedOath", "Renewed Oath", 40, (EnchFireDamageFFContact, 20, 0), (EnchTurnUndeadFFContact, 25, 30));
        var renewed = k.WeaponLike("Orcish Greatsword", "OBP_OathRenewed", T.Renewed, 21, 2600, renewEnch, 3000);

        var scroll = k.Note("OBP_OathScroll", T.ScrollName, T.Scroll);
        var fenceLedger = k.Note("OBP_FenceLedger", T.FenceLedgerName, T.FenceLedger, second: true);

        // --- Actors ----------------------------------------------------------------------------------------
        var sigrun = k.Speaker("OBP_Sigrun", T.SigrunName, T.SigrunShort, EncGuardSonsF02FemaleNord, ArmorSteelNoHelmetOutfit, wardens.FormKey, 12, 50, (K(0x013989), 1));
        var torvald = k.Speaker("OBP_Torvald", T.TorvaldName, T.TorvaldShort, EncGuardSonsM01MaleNordCommander, ArmorSteelPlateNoHelmetOutfit, forsworn.FormKey, 14, 55,
            (K(0x013987), 1), (Gold001, 300)); // Steel Greatsword
        torvald.Configuration.Flags &= ~NpcConfiguration.Flag.Essential;
        torvald.Configuration.Flags |= NpcConfiguration.Flag.Unique;
        var fence = k.Enemy("OBP_Fence", T.FenceName, T.FenceShort, LCharBanditBoss, NordRace);

        var msgNoStart = k.Msg("OBP_MsgNoStart", T.NoStart);

        // --- Intro ------------------------------------------------------------------------------------------
        var intro = k.Quest(qIntro, "OBP_Intro", T.QuestName, true, Quest.TypeEnum.None, 50);
        intro.Aliases.Add(Player(IPlayer));
        intro.Aliases.Add(Forced(IMarker, "Camp", MarkarthStablesMapMarker));
        intro.Aliases.Add(Created(ISigrun, "Sigrun", sigrun.FormKey, IMarker, false, Level.Medium, QuestAlias.Flag.StoresText));
        intro.NextAliasID = 3;
        intro.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("OBP_IntroScript",
                Vmad.Obj("OBP_Main", qMain), Vmad.Obj("OBP_State", gState), Vmad.Obj("OBP_MsgNoStart", msgNoStart), Vmad.Alias("Sigrun", qIntro, ISigrun)))
            .WithAliasScripts(qIntro, IPlayer, Vmad.Script("OBP_SeriesScript", Vmad.Obj("OBP_SeriesSOT", gSOT), Vmad.Obj("OBP_SeriesCHT", gCHT)));

        // --- Main -------------------------------------------------------------------------------------------
        var main = k.Quest(qMain, "OBP_Main", T.QuestName, false, Quest.TypeEnum.SideQuest, 60);
        Stage(main, 0, null, flags: QuestStage.Flag.StartUpStage);
        Stage(main, 10, T.Log10);
        Stage(main, 15, T.Log15);
        Stage(main, 20, T.Log20);
        Stage(main, 40, T.Log40);
        Stage(main, 50, T.Log50);
        Stage(main, 60, T.Log60);
        Stage(main, 70, T.Log70);
        Stage(main, 100, T.Log100, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 110, T.Log110, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 120, T.Log120, QuestLogEntry.Flag.CompleteQuest);
        Objective(main, 10, T.Obj10, AFence);
        Objective(main, 15, T.Obj15);
        Objective(main, 16, T.Obj16);
        Objective(main, 20, T.Obj20, ATorvald);
        Objective(main, 50, T.Obj50, ASigrun);
        Objective(main, 60, T.Obj60, ASigrun);
        Objective(main, 70, T.Obj70, ASigrun);

        main.Aliases.Add(Player(APlayer));
        main.Aliases.Add(new QuestAlias { ID = ASigrun, Type = QuestAlias.TypeEnum.Reference, Name = "Sigrun", Flags = QuestAlias.Flag.Optional | QuestAlias.Flag.StoresText });
        main.Aliases.Add(Created(AScroll, "Scroll", scroll.FormKey, APlayer, true, Level.Easy, 0));
        main.Aliases.Add(Dungeon(AAccLoc, "FenceLoc", accHolds.FormKey, LocTypeDungeon));
        main.Aliases.Add(MarkerOf(AAccMarker, "FenceMarker", AAccLoc));
        main.Aliases.Add(Created(AFence, "Fence", fence.FormKey, AAccMarker, false, Level.Hard, QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(AFenceLedger, "FenceLedger", fenceLedger.FormKey, AFence, true, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(Created(AOathSword, "OathSword", oathSword.FormKey, AFence, true, Level.Easy, 0));
        main.Aliases.Add(Dungeon(AHideLoc, "HideLoc", hideHolds.FormKey, LocTypeDungeon));
        main.Aliases.Add(MarkerOf(AHideMarker, "HideMarker", AHideLoc));
        main.Aliases.Add(Created(ATorvald, "Torvald", torvald.FormKey, AHideMarker, false, Level.Hard, QuestAlias.Flag.InitiallyDisabled | QuestAlias.Flag.StoresText));
        main.NextAliasID = (uint)main.Aliases.Count;
        main.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("OBP_MainScript",
                Vmad.Alias("Sigrun", qMain, ASigrun), Vmad.Alias("Torvald", qMain, ATorvald),
                Vmad.Obj("OBP_State", gState), Vmad.Obj("OBP_RewardGold", gReward), Vmad.Obj("OBP_HunterBonus", gBounty), Vmad.Obj("OBP_SeriesCHT", gCHT),
                Vmad.Obj("OBP_OathbreakerPlate", plate.FormKey), Vmad.Obj("OBP_OathkeeperAegis", aegis.FormKey),
                Vmad.Obj("OBP_OathSword", oathSword.FormKey), Vmad.Obj("OBP_OathRenewed", renewed.FormKey),
                Vmad.Obj("OBP_OathbreakerFaction", forsworn.FormKey), Vmad.Obj("PlayerFaction", PlayerFaction), Vmad.Obj("Gold001", Gold001)))
            .WithAliasScripts(qMain, AFenceLedger, Vmad.Script("OBP_FenceLedgerAliasScript"))
            .WithAliasScripts(qMain, AOathSword, Vmad.Script("OBP_OathSwordAliasScript"))
            .WithAliasScripts(qMain, ATorvald, Vmad.Script("OBP_TorvaldAliasScript"));

        // --- Dialogue ------------------------------------------------------------------------------------------
        var sSigrun = ("FemaleNord", T.SigrunName);
        var sTorvald = ("MaleNordCommander", T.TorvaldName);
        var isSigrun = Cond.IsID(sigrun.FormKey);
        var isTorvald = Cond.IsID(torvald.FormKey);
        Condition State(int v) => GlobalIs(gState, CompareOperator.EqualTo, v);
        Condition St(CompareOperator op, int s) => StageIs(qMain, op, s);

        var bIntro = k.Id("OBP_IntroBranch");
        var tStart = k.Topic("OBP_I_Start", qIntro, bIntro, T.TopicStart, sSigrun);
        var tAccept = k.Topic("OBP_I_Accept", qIntro, bIntro, T.TopicAccept, sSigrun);
        var tDecline = k.Topic("OBP_I_Decline", qIntro, bIntro, T.TopicDecline, sSigrun);
        k.Branch(bIntro, "OBP_IntroBranch", qIntro, tStart);
        var offer = new[] { isSigrun, State(0), PlayerLevelAtLeast(gMinLevel), Cond.QuestRunning(qMain, false) };
        k.Info(tStart, "OBP_I_StartSOT", T.StartSOT, offer.Append(GlobalIs(gSOT, CompareOperator.GreaterThanOrEqualTo, 1)).ToArray(), links: new[] { tAccept, tDecline });
        k.Info(tStart, "OBP_I_StartOffer", T.Start, offer, links: new[] { tAccept, tDecline });
        k.Info(tStart, "OBP_I_StartDone", T.Done, new[] { isSigrun, GlobalIs(gState, CompareOperator.GreaterThanOrEqualTo, 2) }, prompt: T.PromptDone);
        k.Info(tAccept, "OBP_I_AcceptInfo", T.Accept, new[] { isSigrun }, goodbye: true, fragment: "OBP_TIF_Accept");
        k.Info(tDecline, "OBP_I_DeclineInfo", T.Decline, new[] { isSigrun }, goodbye: true);

        var tStatus = k.TopTopic("OBP_S_Status", qMain, T.TopicStatus, sSigrun);
        k.Info(tStatus, "OBP_S_Status10", T.Status10, new[] { isSigrun, St(CompareOperator.LessThan, 20) });
        k.Info(tStatus, "OBP_S_Status20", T.Status20, new[] { isSigrun, St(CompareOperator.EqualTo, 20) });

        var tReport = k.TopTopic("OBP_S_Report", qMain, T.TopicReport, sSigrun);
        k.Info(tReport, "OBP_S_ReportDead", T.ReportDead, new[] { isSigrun, St(CompareOperator.EqualTo, 50) }, goodbye: true, fragment: "OBP_TIF_Reward");
        k.Info(tReport, "OBP_S_ReportYield", T.ReportYield, new[] { isSigrun, St(CompareOperator.EqualTo, 60) }, goodbye: true, fragment: "OBP_TIF_Reward");
        k.Info(tReport, "OBP_S_ReportOath", T.ReportOath, new[] { isSigrun, St(CompareOperator.EqualTo, 70) }, goodbye: true, fragment: "OBP_TIF_Reward");

        var bTorvald = k.Id("OBP_TorvaldBranch");
        var tHello = k.Topic("OBP_T_Hello", qMain, bTorvald, T.TopicHello, sTorvald);
        var tStory = k.Topic("OBP_T_Story", qMain, bTorvald, T.TopicStory, sTorvald);
        var tDuel = k.Topic("OBP_T_Duel", qMain, bTorvald, T.TopicDuel, sTorvald);
        var tYield = k.Topic("OBP_T_Yield", qMain, bTorvald, T.TopicYield, sTorvald);
        var tRedeem = k.Topic("OBP_T_Redeem", qMain, bTorvald, T.TopicRedeem, sTorvald);
        var tSwear = k.Topic("OBP_T_Swear", qMain, bTorvald, T.TopicSwear, sTorvald);
        k.Branch(bTorvald, "OBP_TorvaldBranch", qMain, tHello);
        var facing = new[] { isTorvald, St(CompareOperator.EqualTo, 20) };
        var opts = new[] { tStory, tDuel, tYield, tRedeem, tSwear };
        k.Info(tHello, "OBP_T_HelloInfo", T.Hello, facing, links: opts);
        k.Info(tStory, "OBP_T_StoryInfo", T.Story, facing, links: new[] { tDuel, tYield, tRedeem, tSwear });
        k.Info(tDuel, "OBP_T_DuelInfo", T.Duel, facing, goodbye: true, fragment: "OBP_TIF_Duel");
        k.Info(tYield, "OBP_T_YieldInfo", T.Yield, facing, goodbye: true, fragment: "OBP_TIF_Yield");
        k.Info(tRedeem, "OBP_T_RedeemInfo", T.Redeem, facing.Append(PlayerHas(oathSword.FormKey)).ToArray(), goodbye: true, fragment: "OBP_TIF_Redeem");
        k.Info(tRedeem, "OBP_T_RedeemNo", T.RedeemNo, facing);
        k.Info(tSwear, "OBP_T_SwearInfo", T.Swear, facing.Append(GlobalIs(gSOT, CompareOperator.GreaterThanOrEqualTo, 2)).ToArray(), goodbye: true, fragment: "OBP_TIF_Redeem");
        k.Info(tSwear, "OBP_T_SwearNo", T.SwearNo, facing);
    }
}

internal static class T
{
    public const string QuestName = "Oathbreaker's Price";
    public const string SigrunName = "Warden Sigrun Oath-Keeper", SigrunShort = "Sigrun";
    public const string TorvaldName = "Torvald the Oathbreaker", TorvaldShort = "Torvald";
    public const string FenceName = "Gerda Quick-Purse", FenceShort = "Gerda";

    public const string TopicStart = "You're a long way from any hall, warden.";
    public static readonly string[] Start =
    {
        "Halls don't hold oaths. People do. I hunt the ones who break them.",
        "Torvald swore on steel to guard a caravan of pilgrims. When the Forsworn came, he ran, and nine pilgrims died.",
        "He sold his oath-sword to a fence and vanished. Find the fence, and you'll find him. There's coin in it, and steel.",
    };
    public static readonly string[] StartSOT =
    {
        "You carry a sworn oath. I can see it in how you stand. Then you understand what I hunt.",
        "Torvald swore on steel to guard nine pilgrims. When the Forsworn came, he ran, and they died.",
        "He sold his oath-sword to a fence and vanished. Help me make him answer for it.",
    };
    public const string PromptDone = "Any more oathbreakers, warden?";
    public static readonly string[] Done = { "Always. But Torvald's account is settled, and the pilgrims' families know it. That's your doing." };
    public const string TopicAccept = "I'll hunt him down.";
    public static readonly string[] Accept =
    {
        "Take his oath-scroll; he left it behind when he ran. The fence is Gerda Quick-Purse. She went to ground at <Alias=FenceLoc>.",
    };
    public const string TopicDecline = "Not my oath, not my problem.";
    public static readonly string[] Decline = { "Then don't make one you can't keep." };

    public const string TopicStatus = "About Torvald...";
    public static readonly string[] Status10 = { "Gerda will have his sword, and she'll know where he went. Make her talk, or read what she carries." };
    public static readonly string[] Status20 = { "He's at <Alias=HideLoc>. Kill him, bring him back, or... if there's a better answer, I'll hear it." };

    public const string TopicReport = "Torvald's account is settled.";
    public static readonly string[] ReportDead =
    {
        "Dead by your hand. The pilgrims are avenged, if not returned.",
        "This was forged for the warden who finished this hunt. Oathbreaker's plate, they'll call it, and they'll mean you broke him.",
    };
    public static readonly string[] ReportYield =
    {
        "He yielded? Then he'll stand before the pilgrims' families and hear every name. That's harder than dying.",
        "Take this shield. The Oathkeeper's Aegis. A kept oath is the strongest guard there is.",
    };
    public static readonly string[] ReportOath =
    {
        "He swore again, on his own sword, to guard the roads until his last day? I didn't think he had it in him.",
        "Then the sword is yours to carry, and so is this plate. A warden who mends an oath deserves both.",
    };

    public const string TopicHello = "Torvald. The Oath-Wardens sent me.";
    public static readonly string[] Hello = { "Of course they did. I've been waiting for someone. I'm almost glad it's finally you." };
    public const string TopicStory = "Why did you run?";
    public static readonly string[] Story =
    {
        "Thirty Forsworn on the ridge and me with a dozen frightened pilgrims. I told myself one sword wouldn't change it.",
        "I've told myself that every night since. It doesn't get truer.",
    };
    public const string TopicDuel = "Draw. Face your death like a knight.";
    public static readonly string[] Duel = { "At least you're giving me that. Come, then!" };
    public const string TopicYield = "Yield. Answer to the warden.";
    public static readonly string[] Yield = { "...Aye. Tell Sigrun I'll come. I'm tired of hiding from nine graves." };
    public const string TopicRedeem = "I recovered your oath-sword. Swear again.";
    public static readonly string[] Redeem =
    {
        "My sword... On this steel, I swear to guard the roads of the Reach until my last breath. Keep it, and hold me to it.",
    };
    public static readonly string[] RedeemNo = { "Swear on what? I sold my sword. Without it, my word is wind." };
    public const string TopicSwear = "Swear on my oath, then. I'll stand witness.";
    public static readonly string[] Swear = { "You'd lend me your own oath? ...Then on yours, I swear to guard the roads until my last breath." };
    public static readonly string[] SwearNo = { "You'd stand witness? You've sworn nothing yourself. An oath needs an oath to stand on." };

    public const string Log10 = "Warden Sigrun Oath-Keeper is hunting Torvald, a knight who abandoned nine pilgrims to the Forsworn. He sold his oath-sword to a fence, Gerda Quick-Purse, who is hiding at <Alias=FenceLoc>.";
    public const string Log15 = "I found Gerda's ledger. I should read it.";
    public const string Log20 = "Gerda's ledger says Torvald is hiding at <Alias=HideLoc>.";
    public const string Log40 = "I found Torvald.";
    public const string Log50 = "Torvald is dead. I should tell Sigrun.";
    public const string Log60 = "Torvald yielded and will answer to the Oath-Wardens. I should tell Sigrun.";
    public const string Log70 = "Torvald swore a new oath. I should tell Sigrun.";
    public const string Log100 = "I told Sigrun that Torvald is dead. She gave me the Oathbreaker's Plate.";
    public const string Log110 = "Torvald yielded. Sigrun gave me the Oathkeeper's Aegis.";
    public const string Log120 = "Torvald renewed his oath. His sword was reforged as the Oath-Renewed Greatsword, and Sigrun gave me the Oathbreaker's Plate as well.";

    public const string Obj10 = "Find the fence, Gerda Quick-Purse, at <Alias=FenceLoc>";
    public const string Obj15 = "Read Gerda's ledger";
    public const string Obj16 = "(Optional) Recover Torvald's oath-sword";
    public const string Obj20 = "Confront Torvald at <Alias=HideLoc>";
    public const string Obj50 = "Tell Sigrun that Torvald is dead";
    public const string Obj60 = "Tell Sigrun that Torvald yielded";
    public const string Obj70 = "Tell Sigrun that Torvald renewed his oath";

    public const string ScrollName = "Torvald's Oath-Scroll";
    public const string Scroll = "I, Torvald, knight of no hall but my own, swear on this steel to guard the pilgrims of Dibella from Markarth to the shrine, and to give my life before theirs.\n\n(Someone has scored a line through the name.)";
    public const string FenceLedgerName = "Gerda's Ledger";
    public const string FenceLedger = "Bought: one greatsword, good steel, oath-runes on the blade. Paid the knight 40 septims; he didn't haggle.\n\nHe asked about quiet places to vanish. Sent him to <Alias=HideLoc>. Charged him for that too.";
    public const string Plate = "Oathbreaker's Plate";
    public const string Aegis = "Oathkeeper's Aegis";
    public const string OathSword = "Torvald's Oath-Sword";
    public const string Renewed = "Oath-Renewed Greatsword";
    public const string NoStart = "Sigrun shakes her head. \"The trail's cold. Come back later.\"\n\n(The questline could not start: two free dungeons are needed. Nothing has changed; try again later.)";
}
