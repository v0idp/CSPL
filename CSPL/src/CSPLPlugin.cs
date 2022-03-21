using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CSPL
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInProcess("Virtuoso.exe")]
    public class CSPLPlugin : BaseUnityPlugin
    {
        private const string modGUID = "void.CSPL";
        private const string modName = "Custom Samples & Patches Loader";
        private const string modVersion = "1.0.0";
        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = base.Logger;
            Logger.LogInfo($"Plugin {CSPLPlugin.modGUID} is loaded!");
            harmony.PatchAll();
        }
        
    }
}