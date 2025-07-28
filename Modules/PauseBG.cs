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
    internal class PauseGB : NeonLite.Modules.IModule
    {
#pragma warning disable CS0414
        const bool priority = true;
        const bool active = true;

        static MelonPreferences_Entry<float> scrollSpeed;
        static MelonPreferences_Entry<float> darkenAmount;
        static MelonPreferences_Entry<float> distortScale;
        static MelonPreferences_Entry<float> imageScale;

        static readonly MethodInfo ogstage = AccessTools.Method(typeof(MenuScreenStaging), "OnSetVisible");

        static void Setup()
        {
            scrollSpeed = Settings.Add(stxticCustoms.h, "PauseBG", "scrollSpeed", "scroll speed", null, 0.02f);
            darkenAmount = Settings.Add(stxticCustoms.h, "PauseBG", "darkenAmount", "darken amount", null, 0.2f);
            distortScale = Settings.Add(stxticCustoms.h, "PauseBG", "distortScale", "distort scale", null, 0.01f);
            imageScale = Settings.Add(stxticCustoms.h, "PauseBG", "imageScale", "image scale", null, 0.01f);
        }
        static void Activate(bool _)
        {
            Patching.AddPatch(ogstage, ApplyMatChanges, Patching.PatchTarget.Prefix);
        }

        static void ApplyMatChanges()
        {
            var bgm = MainMenu.Instance().GetComponentInChildren<MaterialHelper>(true).GetComponent<Image>().material;
            bgm.SetFloat("_ScrollSpeed", scrollSpeed.Value);
            bgm.SetFloat("_DarkenAmount", darkenAmount.Value);
            bgm.SetFloat("_DistortScale", distortScale.Value);
            bgm.mainTextureScale = new(imageScale.Value, imageScale.Value);
        }
    }
}
