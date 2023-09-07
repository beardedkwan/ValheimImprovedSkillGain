using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace ImprovedSkillGain
{
    public class PluginInfo
    {
        public const string Name = "ImprovedSkillGain";
        public const string Guid = "beardedkwan.ImprovedSkillGain";
        public const string Version = "1.0.0";
    }

    public class ImprovedSkillGainConfig
    {
        public static ConfigEntry<float> Modifier { get; set; }
    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class ImprovedSkillGain : BaseUnityPlugin
    {
        void Awake()
        {
            // Initialize config
            ImprovedSkillGainConfig.Modifier = Config.Bind("General", "Modifier", 75f, "Modifier as a percentage of how much to increase the skill gain.");

            Harmony harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Skills), "RaiseSkill")]
        public static class Skills_Patch
        {
            private static void Prefix(ref float factor)
            {
                float modifier = ImprovedSkillGainConfig.Modifier.Value;
                modifier = modifier / 100;

                factor = factor + modifier;
                Debug.Log($"Raiseskill patch: modifier = {modifier} / factor = {factor}");
            }
        }
    }
}
