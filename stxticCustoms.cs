using MelonLoader;
using stxticCustoms.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace stxticCustoms
{
    public class stxticCustoms : MelonMod
    {
        internal static new HarmonyLib.Harmony Harmony { get; private set; }
        internal static MelonLogger.Instance Logger { get; private set; }
        internal const string h = "stxtic Customization";
        internal static AssetBundle bundle;
        internal static event Action AfterLoad;

        public override void OnInitializeMelon()
        {
#if DEBUG
            NeonLite.Modules.Anticheat.Register(MelonAssembly);
#endif
            NeonLite.Settings.AddHolder(h);
            Harmony = HarmonyInstance;
            Logger = LoggerInstance;

            NeonLite.NeonLite.LoadModules(MelonAssembly);
        }
        public override void OnLateInitializeMelon()
        {
            bundle = AssetBundle.LoadFromMemory(Resources.r.fonts);
            AfterLoad?.Invoke();
        }
    }
}
