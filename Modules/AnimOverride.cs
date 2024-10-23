using Discord;
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
    internal class AnimOverride : NeonLite.Modules.IModule
    {
#pragma warning disable CS0414
        const bool priority = true;
        const bool active = true;

        static RuntimeAnimatorController complete;
        static RuntimeAnimatorController medals;
        static Sprite timerIcon;
        static bool finishing = false;

        static void Setup()
        {
            stxticCustoms.AfterLoad += () =>
            {
                complete = stxticCustoms.bundle.LoadAsset<RuntimeAnimatorController>($"Assets/Animations/LevelCompleteAnim.controller");
                medals = stxticCustoms.bundle.LoadAsset<RuntimeAnimatorController>($"Assets/Animations/Timer and Medals.controller");
                timerIcon = stxticCustoms.bundle.LoadAsset<Sprite>($"Assets/Sprites/iconTimer.png");
            };
        }

        static readonly MethodInfo ogresvis = AccessTools.Method(typeof(MenuScreenResults), "OnSetVisible");

        static readonly MethodInfo oglvlcom = AccessTools.Method(typeof(MenuScreenResults), "LevelCompleteRoutine");
        static readonly MethodInfo ogeyeout = AccessTools.Method(typeof(EvilEye), "FadeOut");

        static readonly MethodInfo ogfcgvis = AccessTools.Method(typeof(FadeCanvasGroup), "SetVisible");
        static readonly MethodInfo ogixpvis = AccessTools.Method(typeof(InsightXpBar), "SetVisible");


        static void Activate(bool _)
        {
            var hm = Helpers.HM(OverrideAnim);
            hm.priority = Priority.Last;
            stxticCustoms.Harmony.Patch(ogresvis, prefix: hm);

            stxticCustoms.Harmony.Patch(oglvlcom, prefix: Helpers.HM(SetFinishing));
            stxticCustoms.Harmony.Patch(ogeyeout, prefix: Helpers.HM(SetNotFinishing));

            stxticCustoms.Harmony.Patch(ogfcgvis, prefix: Helpers.HM(OverrideVisibility));
            stxticCustoms.Harmony.Patch(ogixpvis, prefix: Helpers.HM(OverrideVisibility));
        }

        static void SetFinishing(MenuScreenResults __instance)
        {
            finishing = true;
#if DEBUG
            __instance.SetMedal(4, 0, 4, 0, 0, 4, false);
#endif
        }

        static void SetNotFinishing() => finishing = false;

        static void OverrideVisibility(ref bool visible, ref bool animate)
        {
            if (!finishing)
                return;
            visible = false;
            animate = false;
        }

        static void OverrideAnim(MenuScreenResults __instance)
        {
            LevelData _currentLevel = Singleton<Game>.Instance.GetCurrentLevel();
#if DEBUG
            GameDataManager.levelStats[_currentLevel.levelID]._newBest = true;
#endif
            __instance._newBestColor = Colorization.newBest.Value;
            __instance._levelCompleteNewBestText.GetComponent<TextMeshProUGUI>().color = Colorization.newBest.Value;
            __instance._resultsScreenNewBestTimeIndicator.GetComponent<TextMeshProUGUI>().color = Colorization.newBest.Value;

            var cg = __instance.levelComplete_Localized.GetOrAddComponent<CanvasGroup>();
            cg.interactable = cg.blocksRaycasts = false;
            cg = __instance._levelCompleteAnimator.transform.Find("BGFlame").GetOrAddComponent<CanvasGroup>();
            cg.interactable = cg.blocksRaycasts = false;
            cg.transform.Find("Sigil (1)").gameObject.SetActive(false);
            cg = __instance._medalAnimator.GetOrAddComponent<CanvasGroup>();
            cg.interactable = cg.blocksRaycasts = false;
            foreach (var text in __instance._medalAnimator.GetComponentsInChildren<TextMeshProUGUI>())
            {
                cg = text.GetOrAddComponent<CanvasGroup>();
                cg.interactable = cg.blocksRaycasts = false;
                var id = FontReplace.fontLib.GetBaseId(text.font);
                if (id != -1)
                    text.fontSharedMaterial = FontReplace.fontLib.textMeshProFontSets[id].englishFontMats[1];
            }
            var timers = __instance._levelCompleteAnimator.transform.Find("Timers");
            foreach (var text in timers.GetComponentsInChildren<TextMeshProUGUI>())
            {
                var id = FontReplace.fontLib.GetBaseId(text.font);
                if (id != -1)
                    text.fontSharedMaterial = FontReplace.fontLib.textMeshProFontSets[id].englishFontMats[1];
            }
            var t = timers.Find("Timer Icon").GetComponent<Image>();
            Color c = GameDataManager.levelStats[_currentLevel.levelID]._newBest ? Colorization.newBest.Value : Color.white;
            t.color = c;
            t.sprite = timerIcon;
            var flash = __instance._levelCompleteAnimator.transform.Find("Flash")?.gameObject ?? Utils.InstantiateUI(__instance._levelCompleteAnimator.transform.Find("BG").gameObject, "Flash", __instance._levelCompleteAnimator.transform);
            flash.GetComponent<Image>().color = Color.white;
            flash.transform.SetAsLastSibling();
            __instance._levelCompleteNewBestText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;

            var oldBest = __instance._levelCompleteBestStatsText.GetComponent<TextMeshProUGUI>();
            oldBest.color = Color.white.Alpha(.3f);

            oldBest = oldBest.transform.parent.Find("Timer Text (2)").GetComponent<TextMeshProUGUI>();
            oldBest.color = Color.white.Alpha(.3f);

            __instance._levelCompleteAnimator.runtimeAnimatorController = complete;
            __instance._medalAnimator.runtimeAnimatorController = medals;
        }
    }
}
