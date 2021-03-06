using System;
using System.IO;
using HarmonyLib;
using AudioHelm;
using VROSC;

namespace CSPL.Patches
{
    [HarmonyPatch]
    public static class SynthDataControllerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SynthDataController), nameof(SynthDataController.ApplyDefaults))]
        public static void ApplyDefaults_Postfix(SynthDataController __instance)
        {
            var directory = Directory.GetCurrentDirectory() + "\\import\\patches\\";
            var dirInfo = new DirectoryInfo(directory);
            var files = dirInfo.GetFiles("*.helm", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                for (var i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var patch = UnityEngine.JsonUtility.FromJson<HelmPatchFormat>(File.ReadAllText(files[i].FullName));
                        patch.patch_name = Path.GetFileNameWithoutExtension(files[i].Name);
                        patch.folder_name = "CustomPatches";
                        __instance.AddPatch(patch);
                        CSPLPlugin.Log.LogInfo($"Added custom patch {patch.patch_name}");
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