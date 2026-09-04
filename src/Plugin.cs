using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DMD.InfiniteDarkness
{
    [BepInPlugin("com.vdotpy.dmd.infinitedarkness", "InfiniteDarkness", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource ModLogger { get; private set; }
        internal static Plugin Instance { get; private set; }

        private void Awake()
        {
            ModLogger = Logger;
            Instance = this;

            ModLogger.LogInfo("Infinite Darkness Mod Initializing... Breaking all boundaries!");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
