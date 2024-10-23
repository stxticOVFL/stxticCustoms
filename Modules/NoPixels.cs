using HarmonyLib;
using MelonLoader;
using NeonLite;
using NeonLite.Modules;
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
    internal class NoPixels : IModule
    {
#pragma warning disable CS0414
        const bool priority = false;
        const bool active = true;

        static void Setup()
        {
        }

        static void Activate(bool _)
        {
            // AnimOverride helper
            MainMenu.Instance().GetComponentInChildren<Canvas>().pixelPerfect = false;
        }
    }
}
