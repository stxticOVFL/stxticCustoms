using HarmonyLib;
using MelonLoader;
using NeonLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace stxticCustoms.Modules
{
    [HarmonyPatch]
    internal class FontReplace : NeonLite.Modules.IModule
    {
#pragma warning disable CS0414
        const bool priority = true;
        const bool active = true;

        static MelonPreferences_Entry<int> lbIndex;
        static MelonPreferences_Entry<int> timerIndex;
        static readonly Dictionary<int, int> replacements = [];
        internal static AxKLocalizedText_FontLib fontLib;

        static FieldInfo localizedInit = AccessTools.Field(typeof(AxKLocalizedText), "m_init");

        static void Setup()
        {
            var setting = Settings.Add(stxticCustoms.h, "Fonts", "replacements", "font replacements", null, "");
            lbIndex = Settings.Add(stxticCustoms.h, "Fonts", "lbIndex", "leaderboard username font index", null, 0);
            timerIndex = Settings.Add(stxticCustoms.h, "Fonts", "timerIndex", "timer font index", null, 1);

            setting.OnEntryValueChanged.Subscribe((_, after) => ProcessReplacements(after));
            stxticCustoms.AfterLoad += () =>
            {
                fontLib = stxticCustoms.bundle.LoadAsset<AxKLocalizedText_FontLib>($"Assets/Fonts/FontLib.asset");
                ProcessReplacements(setting.Value);
            };
        }
        static void Activate(bool _) { }
        static void ProcessReplacements(string str)
        {
            var lines = str.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            replacements.Clear();
            foreach (var line in lines)
            {
                stxticCustoms.Logger.Msg(line);
                if (line.Trim() == "")
                    continue;
                var s = line.Trim().Split('=');
                stxticCustoms.Logger.Msg(s[0]);
                stxticCustoms.Logger.Msg(s[1]);

                replacements.Add(int.Parse(s[0].Trim()), int.Parse(s[1].Trim()));
            }
        }

        [HarmonyPatch(typeof(AxKLocalizedText), "ChangeFont")]
        [HarmonyPrefix]
        static void CheckAutoLower(AxKLocalizedText __instance, int ___m_fontIndex)
        {
            if (__instance.unityUIText || !replacements.ContainsKey(___m_fontIndex))
                return;
            Lowercase.SetLower(__instance.textMeshPro == null ? __instance.textMeshProUGUI : __instance.textMeshPro);
        }

        [HarmonyPatch(typeof(AxKLocalizedText_FontLib), "GetReplacement_TMP_FONT")]
        [HarmonyPrefix]
        static bool GetFontOverride(AxKLocalizedText_FontLib __instance, int index, ref TMP_FontAsset __result)
        {
            if (__instance == fontLib || !replacements.ContainsKey(index))
                return true;
            __result = fontLib.GetReplacement_TMP_FONT(replacements[index]);
            return __result == null;
        }

        [HarmonyPatch(typeof(AxKLocalizedText_FontLib), "GetReplacement_TMP_FONT_Material")]
        [HarmonyPrefix]
        static bool GetMaterialOverride(AxKLocalizedText_FontLib __instance, int index, Material sourceMaterial, ref Material __result)
        {
            if (!replacements.ContainsKey(index))
                return true;

            Material[] ogMats = __instance.textMeshProFontSets[index].englishFontMats;
            int num = -1;
            for (int i = 0; i < ogMats.Length; i++)
            {
                if (ogMats[i] == sourceMaterial)
                    num = i;
            }

            if (num < 0)
                return false;

            __result = fontLib.textMeshProFontSets[replacements[index]].englishFontMats[num];
            return __result == null;
        }

        static void DoQuickReplacement(TMP_Text text)
        {
            var pre = text.text;
            var localization = text.GetOrAddComponent<AxKLocalizedText>();
            localization.textMeshProUGUI = text.GetComponent<TextMeshProUGUI>();
            localization.textMeshPro = text.GetComponent<TextMeshPro>();
            AxKLocalizedTextLord.GetInstance().RemoveText(localization);
            localizedInit.SetValue(localization, false);
            localization.Init();
            localization.Localize();
            text.text = pre;
            AxKLocalizedTextLord.GetInstance().RemoveText(localization);
            UnityEngine.Object.Destroy(localization);
        }

        [HarmonyPatch(typeof(LevelInfo), "SetLevel")]
        [HarmonyPostfix]
        static void PreLevelInfo(LevelInfo __instance)
        {
            DoQuickReplacement(__instance._goldMedalTime);
            DoQuickReplacement(__instance._silverMedalTime);
            DoQuickReplacement(__instance._aceMedalTime);
            DoQuickReplacement(__instance._levelBestTime);
            //var id = fontLib.GetBaseId(__instance._levelBestTime.font);
            //__instance._levelBestTime.fontSharedMaterial = fontLib.textMeshProFontSets[id].englishFontMats[1];
        }

        [HarmonyPatch(typeof(MenuScreenResults), "OnSetVisible")]
        [HarmonyPrefix]
        static void PostResults(MenuScreenResults __instance)
        {
            // hide shit idc about too
            DoQuickReplacement(__instance._resultsScreenLevelTime);
            DoQuickReplacement(__instance._levelCompleteBestStatsText);
            DoQuickReplacement(__instance._levelCompleteStatsText);
        }

        [HarmonyPatch(typeof(Guirao.UltimateTextDamage.UITextDamage), "Start")]
        [HarmonyPostfix]
        static void PostDamageText(Guirao.UltimateTextDamage.UITextDamage __instance)
        {
            DoQuickReplacement(__instance.UsedLabel);
        }

        [HarmonyPatch(typeof(LeaderboardScore), "SetScore")]
        [HarmonyPostfix]
        static void PostSetScore(LeaderboardScore __instance)
        {
            if (!fontLib)
                return;
            __instance._username.font = fontLib.fontSets[lbIndex.Value].english;
            __instance._username.resizeTextForBestFit = false;
            __instance._username.fontSize -= 1;
            DoQuickReplacement(__instance._ranking);
            __instance._ranking.enableAutoSizing = false;
            __instance._ranking.fontSize -= 2;
            DoQuickReplacement(__instance._scoreValue);
            __instance._scoreValue.enableAutoSizing = false;
            __instance._scoreValue.fontSize -= 2;
        }
        [HarmonyPatch(typeof(PlayerUI), "Start")]
        [HarmonyPostfix]
        static void PostUIStart(PlayerUI __instance)
        {
            DoQuickReplacement(__instance.demonCounterNumberText);
            __instance.demonCounterNumberText.characterSpacing = -10;
            __instance.demonCounterTitleText.enableAutoSizing = false;
            __instance.demonCounterTitleText.fontSize -= .5f;
            __instance.timerText.font = fontLib.textMeshProFontSets[timerIndex.Value].english;
            __instance.timerText.fontSize = 250;
        }
        [HarmonyPatch(typeof(UICard), "Start")]
        [HarmonyPostfix]
        static void PostCardStart(UICard __instance)
        {
            var text = __instance.GetComponentInChildren<TextMeshPro>();
            text.enableAutoSizing = false;
            text.fontSize = 4f;
        }
        [HarmonyPatch(typeof(LevelGate), "Start")]
        [HarmonyPostfix]
        static void PostGateStart(LevelGate __instance)
        {
            DoQuickReplacement(__instance.counterText);
        }
        [HarmonyPatch(typeof(PlayerUICardHUD), "Setup")]
        [HarmonyPostfix]
        static void PostCardHUDStart(PlayerUICardHUD __instance)
        {
            DoQuickReplacement(__instance.textAmmo);
        }

        // risky! but should be ok
        [HarmonyPatch(typeof(MenuButtonBase), "Start")]
        [HarmonyPatch(typeof(Selectable), "Awake")]
        [HarmonyPostfix]
        static void PostButtonStart(Selectable __instance)
        {
            if (!fontLib)
                return;
            var text = __instance.transform.GetComponentInChildren<TextMeshProUGUI>();
            if (text && text.transform.parent == __instance.transform)
            {
                if (text.font == fontLib.textMeshProFontSets[0].english)
                {
                    if (text.fontSize < 20)
                        text.fontSize = 20;
                    if (text.fontSizeMax < 24)
                        text.fontSizeMax = 24;
                }
                else if (text.font == fontLib.textMeshProFontSets[1].english)
                {
                    if (text.fontSize > 28)
                        text.fontSize = 28;
                    if (text.fontSizeMax > 28)
                        text.fontSizeMax = 28;
                }
            }
        }
    }
}
