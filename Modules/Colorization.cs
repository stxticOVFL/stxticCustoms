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
using UnityEngine;
using UnityEngine.UI;

namespace stxticCustoms.Modules
{
    [HarmonyPatch]
    internal class Colorization : NeonLite.Modules.IModule
    {
#pragma warning disable CS0414
        const bool priority = true;
        const bool active = true;

        static MelonPreferences_Entry<Color> faceColor;
        static MelonPreferences_Entry<Color> flameColor;
        static MelonPreferences_Entry<Color> flameColor2;
        static MelonPreferences_Entry<Color> finishColor;

        static MelonPreferences_Entry<Color> demonColor;
        static MelonPreferences_Entry<Color> demonFlame;
        static MelonPreferences_Entry<Color> demonFlame2;
        
        static readonly MethodInfo ogstage = AccessTools.Method(typeof(MenuScreenStaging), "OnSetVisible");
        static readonly MethodInfo ogresult = AccessTools.Method(typeof(MenuScreenResults), "OnSetVisible");

        static void Setup()
        {
            faceColor = Settings.Add(stxticCustoms.h, "Color", "faceColor", "fade color", null, new Color());
            flameColor = Settings.Add(stxticCustoms.h, "Color", "flameColor", "flame color", null, new Color());
            flameColor2 = Settings.Add(stxticCustoms.h, "Color", "flameColor2", "flame bar color", null, new Color());
            finishColor = Settings.Add(stxticCustoms.h, "Color", "finishColor", "finish color", null, new Color());

            demonColor = Settings.Add(stxticCustoms.h, "Color", "demonColor", "demon counter color", null, new Color());
            demonFlame = Settings.Add(stxticCustoms.h, "Color", "demonFlame", "demon flame color", null, new Color());
            demonFlame2 = Settings.Add(stxticCustoms.h, "Color", "demonFlame2", "demon flame bar color", null, new Color());

        }
        static void Activate(bool _)
        {
            stxticCustoms.Harmony.Patch(ogstage, prefix: Helpers.HM(ApplyBackColors));
            stxticCustoms.Harmony.Patch(ogresult, prefix: Helpers.HM(ApplyBackColors));
        }

        static void ApplyBackColors(MenuScreen __instance)
        {
            __instance.GetComponentInChildren<Image>().color = faceColor.Value;
            if (__instance is MenuScreenResults results)
            {
                foreach (var images in results._levelCompleteAnimator.transform.Find("BGFlame").GetComponentsInChildren<RawImage>())
                {
                    if (images.name == "BGFlame")
                        images.color = flameColor2.Value;
                    else
                        images.material.color = flameColor.Value;
                }
                var bgi = results._levelCompleteAnimator.transform.Find("BG").GetComponent<Image>();
                bgi.material = new(bgi.material) // this'll get GC'd all is fine
                {
                    color = (finishColor.Value * 2).Alpha(finishColor.Value.a)
                };
            }
        }

        [HarmonyPatch(typeof(PlayerUI), "Start")]
        [HarmonyPostfix]
        static void PostUIStart(PlayerUI __instance)
        {
            __instance.demonCounterNumberText.color = demonColor.Value;
            var bars = __instance.demonCounterHolder.transform.Find("BG Bars");
            bars.Find("BottomBar (3)").GetComponent<MeshRenderer>().material.color = demonFlame.Value;
            bars.Find("BottomBarDetail (4)").GetComponent<MeshRenderer>().material.color = demonFlame2.Value;
            bars.Find("BottomBarDetail (5)").GetComponent<MeshRenderer>().material.color = demonFlame2.Value;
        }

    }
}
