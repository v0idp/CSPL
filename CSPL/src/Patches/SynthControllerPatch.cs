using System;
using System.IO;
using HarmonyLib;
using AudioHelm;
using VROSC;

namespace CSPL.Patches
{
    [HarmonyPatch]
    public static class SynthControllerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SynthController), nameof(SynthController.SynthsDataLoaded))]
        public static void SynthsDataLoaded_Postfix(SynthController __instance, ref InstrumentDataController dataController)
        {
            var directory = Directory.GetCurrentDirectory() + "\\import\\patches\\";
            var dirInfo = new DirectoryInfo(directory);
            var files = dirInfo.GetFiles("*.helm", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                CSPLPlugin.Log.LogInfo($"Loading custom patches...");
                for (var i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var patch = UnityEngine.JsonUtility.FromJson<HelmPatchFormat>(File.ReadAllText(files[i].FullName));
                        patch.patch_name = Path.GetFileNameWithoutExtension(files[i].Name);
                        patch.folder_name = "CustomPatches";
                        __instance.SynthDataController._defaultPatches.Add(new PatchSettings(patch));
                        __instance.SynthDataController.AddPatch(patch);
                        CSPLPlugin.Log.LogInfo($"Added custom patch \"{patch.patch_name}\"");
                    }
                    catch (Exception ex)
                    {
                        CSPLPlugin.Log.LogError($"Failed to add patch {files[i].Name}");
                        CSPLPlugin.Log.LogError($"Message: {ex.Message}\nStacktrace: {ex.StackTrace}");
                    }
                }
            }
            else
            {
                CSPLPlugin.Log.LogWarning("Couldn't find any custom helm patches!");
            }
        }
    }
}