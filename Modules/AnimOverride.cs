using Discord;
using HarmonyLib;
using MelonLoader;
using NeonLite;
using NeonLite.Modules;
using System;
using System.Collections;
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
    internal class AnimOverride : MonoBehaviour, NeonLite.Modules.IModule
    {
#pragma warning disable CS0414
        const bool priority = true;
        static bool active = true;

        static RuntimeAnimatorController complete;
        static RuntimeAnimatorController medals;
        static Sprite timerIcon;
        static bool finishing = false;


        enum Animations
        {
            None,
            Mizuki,
            Bourbon
        }

        static MelonPreferences_Entry<Animations> setting;

        static void Setup()
        {
            setting = Settings.Add(stxticCustoms.h, "Misc", "animOverride", "anim override", null, Animations.None);
            active = setting.SetupForModule(Activate, (_, after) => after != Animations.None);
        }

        static readonly MethodInfo ogresvis = AccessTools.Method(typeof(MenuScreenResults), "OnSetVisible");

        static readonly MethodInfo oglvlcom = AccessTools.Method(typeof(MenuScreenResults), "LevelCompleteRoutine");
        static readonly MethodInfo ogeyeout = AccessTools.Method(typeof(EvilEye), "FadeOut");

        static readonly MethodInfo ogfcgvis = AccessTools.Method(typeof(FadeCanvasGroup), "SetVisible");
        static readonly MethodInfo ogixpvis = AccessTools.Method(typeof(InsightXpBar), "SetVisible");


        static void LoadAssets()
        {
            complete = stxticCustoms.bundle.LoadAsset<RuntimeAnimatorController>($"Assets/Animations/{setting.Value}/LevelCompleteAnim.controller");
            medals = stxticCustoms.bundle.LoadAsset<RuntimeAnimatorController>($"Assets/Animations/{setting.Value}/Timer and Medals.controller");
            timerIcon = stxticCustoms.bundle.LoadAsset<Sprite>($"Assets/Sprites/iconTimer.png");
        }

        static void Activate(bool activate)
        {
            var hm = Helpers.HM(OverrideAnim);
            hm.priority = Priority.Last;
            Patching.TogglePatch(activate, ogresvis, hm, Patching.PatchTarget.Prefix);

            Patching.TogglePatch(activate, oglvlcom, SetFinishing, Patching.PatchTarget.Prefix);
            Patching.TogglePatch(activate, ogeyeout, SetNotFinishing, Patching.PatchTarget.Prefix);

            Patching.TogglePatch(activate, ogfcgvis, OverrideVisibility, Patching.PatchTarget.Prefix);
            Patching.TogglePatch(activate, ogixpvis, OverrideVisibility, Patching.PatchTarget.Prefix);

            if (stxticCustoms.invoked)
                LoadAssets();
            else
                stxticCustoms.AfterLoad += LoadAssets;
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

        static GameObject b_flame;

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

            if (setting.Value == Animations.Bourbon)
            {
                if (!b_flame)
                {
                    b_flame = GameObject.Instantiate(cg.gameObject, __instance._levelCompleteAnimator.transform);
                    b_flame.transform.SetSiblingIndex(cg.transform.GetSiblingIndex() + 1);
                    b_flame.transform.rotation = Quaternion.Euler(0, 0, 65);
                    b_flame.GetComponentsInChildren<RawImage>().Do(x =>
                    {
                        x.color = Color.white;
                        x.material = new Material(x.material);
                        x.material.color = Color.white;

                        if (x.material.HasProperty("_ScrollSpeed"))
                        {
                            x.material.SetFloat("_ScrollSpeed", 5);
                            x.material.SetFloat("_OpacityMain", 0.3f);
                        }
                    });
                }
            }
            else if (b_flame)
                b_flame.SetActive(false);

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

            if (setting.Value == Animations.Bourbon)
            {
                var m = medals.animationClips.First(x => x.name == "uiAnim_Medals");
                var e = m.events[0];
                if (m.events.Length <= 1 && string.IsNullOrEmpty(e.functionName))
                {
                    __instance._medalAnimator.GetOrAddComponent<AnimOverride>();
                    e.functionName = "SetBG";
                    e.objectReferenceParameter = __instance;
                    m.AddEvent(e);
                }
            }
        }

        public void SetBG(MenuScreenResults results)
        {
            var bg = results._levelCompleteAnimator.transform.Find("BG").GetComponent<Image>();
            var m = CommunityMedals.GetMedalIndex(Singleton<Game>.Instance.GetCurrentLevel().levelID);
            StartCoroutine(SetBGRoutine(bg.material, CommunityMedals.AdjustedColor(CommunityMedals.Colors[m])));
        }

        IEnumerator SetBGRoutine(Material bg, Color color)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            s -= 0.1f;
            v += 0.2f;
            color = Color.HSVToRGB(h, s, v);

            const float TIME = 15f / 60f;
            var og = bg.color / 2;
            float t = 0;
            while (t < TIME)
            {
                var nt = AxKEasing.EaseOutCubic(0, 1, t / TIME);
                bg.color = (Color.Lerp(og, color, nt) * 2).Alpha(1);
                yield return null;
                t += Time.unscaledDeltaTime;
            }
        }
    }
}
