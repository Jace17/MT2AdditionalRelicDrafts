using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MT2AdditionalRelicDrafts.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        public void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            // Uncomment if you need harmony patches, if you are writing your own custom effects.
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    public class IncreaseDraftOptionsForBlessingReward
    {
        public static void Postfix(SaveManager __instance)
        {
            GrantableRewardData relicDraftRewardData = __instance.GetAllGameData().FindRewardDataByName("BlessingDraftReward");
            if (relicDraftRewardData != null)
            {
                Traverse.Create(relicDraftRewardData).Field("draftOptionsCount").SetValue(3U);
            }
        }
    }

    [HarmonyPatch(typeof(NodeDistanceData), "GetBattleRewards")]
    public class AddRelicDraftToBattleRewards
    {
        public static void Postfix(SaveManager saveManager, bool isEndlessRun, bool hasAlliedChampion, ref List<GrantableRewardData> __result, NodeDistanceData __instance, RewardNodeData ___battleReward)
        {
            if(!isEndlessRun && !__result.IsNullOrEmpty())
            {
                GrantableRewardData relicDraftRewardData = saveManager.GetAllGameData().FindRewardDataByName("BlessingDraftRewardBig");
                if (relicDraftRewardData != null)
                {
                    __result.Add(relicDraftRewardData);
                }
            }           
        }
    }
}
