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
using UnityEngine.UI;

namespace stxticCustoms.Modules
{
    [HarmonyPatch]
    internal class Lowercase
    {
#pragma warning disable CS0414
        const bool priority = true;
        const bool active = true;

        static FieldInfo defaultPro = AccessTools.Field(typeof(AxKLocalizedText), "m_textMeshPro_DefaultFontStyle");

        internal static void SetLower(TMP_Text text)
        {
            text.fontStyle |= FontStyles.LowerCase;
            var local = text.GetComponent<AxKLocalizedText>();
            if (local)
                defaultPro.SetValue(local, ((FontStyles)defaultPro.GetValue(local)) | FontStyles.LowerCase);
        }

        [HarmonyPatch(typeof(LevelInfo), "SetLevel")]
        [HarmonyPostfix]
        static void PreLevelInfo(LevelInfo __instance)
        {
            SetLower(__instance._levelEnvironmentNameText);
        }

        [HarmonyPatch(typeof(MenuScreenStaging), "Localize")]
        [HarmonyPostfix]
        static void PreStagingLocalize(MenuScreenStaging __instance)
        {
            SetLower(__instance.levelEnvironmentText);
        }


    }
}
