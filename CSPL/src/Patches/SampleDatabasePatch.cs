using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using VROSC;
using UnityEngine;

namespace CSPL.Patches
{
    [HarmonyPatch]
    public static class SampleDatabasePatch
    {
        private static Dictionary<string, int> sampleGroups = new Dictionary<string, int>()
        {
            { "Bass Drums", 0 },
            { "Snares",     1000 },
            { "Hi-hats",    2000 },
            { "Rides",      3000 },
            { "Car Doors",  4000 },
            { "Toms",       5000 },
            { "Crashes",    6000 },
            { "Effects",    7000 },
        };

        private static Dictionary<int, List<SampleData>> samples = new Dictionary<int, List<SampleData>>()
        {
            { 0,    new List<SampleData>() },
            { 1000, new List<SampleData>() },
            { 2000, new List<SampleData>() },
            { 3000, new List<SampleData>() },
            { 4000, new List<SampleData>() },
            { 5000, new List<SampleData>() },
            { 6000, new List<SampleData>() },
            { 7000, new List<SampleData>() },
        };

        private static bool IsSamplesInjected = false;

        static SampleDatabasePatch()
        {
            LoadCustomSamples();
        }

        private static void LoadCustomSamples()
        {
            CSPLPlugin.Log.LogInfo("Loading custom samples...");

            var customSamplesPath = Directory.GetCurrentDirectory() + "\\import\\samples\\";
            var directories = Directory.GetDirectories(customSamplesPath);
            foreach (string d in directories)
            {
                var dirInfo = new DirectoryInfo(d);
                var dirName = dirInfo.Name;
                var dirPath = dirInfo.FullName;

                CSPLPlugin.Log.LogDebug($"({dirName}) Checking path: {dirPath}");

                var groupId = sampleGroups[dirName];
                var index = groupId + 100;
                var files = dirInfo.GetFiles("*.wav", SearchOption.AllDirectories);

                foreach (FileInfo f in files)
                {
                    try
                    {
                        var fileName = Path.GetFileNameWithoutExtension(f.FullName);
                        var audioClip = AudioImport.Import(f.FullName);
                        var sampleData = ScriptableObject.CreateInstance<SampleData>();
                        sampleData._id = index++;
                        sampleData.name = fileName;
                        sampleData._displayName = fileName;
                        sampleData._audioClip = audioClip;
                        samples[groupId].Add(sampleData);

                        CSPLPlugin.Log.LogInfo($"Imported {f.Name} into {sampleGroups.FirstOrDefault(x => x.Value == groupId).Key} with id {index}");

                        index += 1;
                    }
                    catch (Exception ex)
                    {
                        CSPLPlugin.Log.LogError($"Failed to import {f.Name} into {sampleGroups.FirstOrDefault(x => x.Value == groupId).Key}");
                        CSPLPlugin.Log.LogError($"Message: {ex.Message}\nStacktrace: {ex.StackTrace}");
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SampleDatabase), nameof(SampleDatabase.GetGroup))]
        public static void GetGroup_Prefix(ref SampleGroup[] ____samplegroups)
        {
            if (!IsSamplesInjected)
            {
                for (int i = 0; i < ____samplegroups.Length; i++)
                {
                    var groupId = ____samplegroups[i].Id;
                    if (samples.ContainsKey(groupId) && samples[groupId].Count > 0)
                    {
                        ____samplegroups[i]._samples.AddRange(samples[groupId]);
                    }
                }
                IsSamplesInjected = true;
            }
        }
    }
}
