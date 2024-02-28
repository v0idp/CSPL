using HarmonyLib;
using VROSC;

namespace CSPL.Patches
{
    [HarmonyPatch]
    public static class SessionsManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SessionsManager), nameof(SessionsManager.SaveSessionAs))]
        public static void SaveSessionAs_Prefix(SessionsManager __instance, ref SessionSaveData sessionSaveData)
        {
            if (sessionSaveData.IsShared)
            {
                sessionSaveData.IsShared = false;
                sessionSaveData.IsCloud = true;
                CSPLPlugin.Log.LogInfo("Preventing session from being shared to community...");
                CSPLPlugin.Log.LogInfo("Session will be uploaded to cloud instead!");
            }
        }
    }
}
