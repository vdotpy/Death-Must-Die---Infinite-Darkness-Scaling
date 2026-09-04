using System;
using System.Reflection;
using HarmonyLib;
using Death.Darkness;
using Death.Data;
using Death.TimesRealm;
using Death.TimesRealm.UserInterface.Darkness;
using TMPro;

namespace DMD.InfiniteDarkness
{
    // =========================================================================
    // THE UI ROW DISPLAY VISUAL UNCAPPER
    // Hooks the row rendering loop immediately after it runs
    // Forces the buttons to stay active and replaces the static "Max" label
    // with true numeric rank level and point values -currently overlapping fix later-
    // =========================================================================
    [HarmonyPatch(typeof(GUI_DarknessChallenge), "UpdateDisplayAsync")]
    public static class UncapChallengeUI_Patch
    {
        private static readonly FieldInfo DisableNextLevelsField = 
            typeof(GUI_DarknessChallenge).GetField("_disableNextLevels", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo PlusBtnField = 
            typeof(GUI_DarknessChallenge).GetField("_plusBtn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo PointsTextField = 
            typeof(GUI_DarknessChallenge).GetField("_pointsText", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo ChallengeField = 
            typeof(GUI_DarknessChallenge).GetField("_challenge", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo LevelField = 
            typeof(GUI_DarknessChallenge).GetField("_level", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        [HarmonyPostfix]
        public static void Postfix(GUI_DarknessChallenge __instance)
        {
            try
            {
                // 1. Force the row to stop flagging next levels as locked
                if (DisableNextLevelsField != null)
                {
                    DisableNextLevelsField.SetValue(__instance, false);
                }

                // 2. Force the interactive plus button object component to remain clickable
                if (PlusBtnField != null)
                {
                    var plusBtnObj = PlusBtnField.GetValue(__instance);
                    if (plusBtnObj != null)
                    {
                        var interactableProp = plusBtnObj.GetType().GetProperty("Interactable", BindingFlags.Public | BindingFlags.Instance);
                        if (interactableProp != null)
                        {
                            interactableProp.SetValue(plusBtnObj, true);
                        }
                    }
                }

                // 3. DYNAMIC LEVEL RANK INJECTOR
                // Overwrites the _pointsText component to display: "Lvl [X] (+[Y])"
                if (PointsTextField != null && ChallengeField != null && LevelField != null)
                {
                    var challenge = ChallengeField.GetValue(__instance) as ChallengeData;
                    var pointsText = PointsTextField.GetValue(__instance) as TextMeshProUGUI;
                    int level = (int)LevelField.GetValue(__instance);

                    if (pointsText != null && challenge != null)
                    {
                        // Safely compile a clean, readable layout string for your menu row
                        pointsText.text = $"Lv{level} (+{challenge.PointsPerLevel})";
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.ModLogger.LogError($"[Infinite Darkness] UI row layout hook crashed: {ex}");
            }
        }
    }

    // =========================================================================
    // CORE CONTROLLER METHOD UNCAPPER
    // Intercepts the concrete validation logic inside the game engine
    // Skips the hardcoded database check to allow infinite level point increases
    // =========================================================================
    [HarmonyPatch(typeof(DarknessController))]
    public static class GameControllerUncap_Patches
    {
        private static readonly PropertyInfo SelectedOptionsProperty = 
            typeof(DarknessController).GetProperty("SelectedOptions", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        [HarmonyPatch(nameof(DarknessController.CanIncreaseLevel))]
        [HarmonyPrefix]
        public static bool CanIncreaseLevelPrefix(ref bool __result)
        {
            __result = true; 
            return false; 
        }

        [HarmonyPatch(nameof(DarknessController.TryIncreaseLevel))]
        [HarmonyPrefix]
        public static bool TryIncreaseLevelPrefix(DarknessController __instance, ChallengeData challenge, ref bool __result)
        {
            try
            {
                if (challenge == null || SelectedOptionsProperty == null) return true;

                var selectedOptions = SelectedOptionsProperty.GetValue(__instance) as DarknessOptions;
                if (selectedOptions != null)
                {
                    selectedOptions.IncreaseChallenge(challenge.Code);
                    __result = true;
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Plugin.ModLogger.LogError($"[Infinite Darkness] Target point injection failed: {ex}");
            }
            return true;
        }

        [HarmonyPatch(nameof(DarknessController.PointsCap), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool PointsCapPrefix(ref int __result)
        {
            __result = 99999; 
            return false; 
        }
    }

    // =========================================================================
    // INFINITE TOTAL PROGRESS CALCULATOR
    // Ensures that when you crank up a vanilla row to Level 500, the mathematical
    // total is tracked and loaded flawlessly across save states
    // =========================================================================
    [HarmonyPatch(typeof(DarknessOptions), nameof(DarknessOptions.GetTotalPoints))]
    public static class InfiniteTotalPoints_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(DarknessOptions __instance, ref int __result)
        {
            var totalPoints = 0;
            try
            {
                foreach (DarknessOptions.Challenge challenge in __instance)
                {
                    if (Database.Darkness.TryGet(challenge.Code, out var challengeData))
                    {
                        totalPoints += challengeData.PointsPerLevel * challenge.Level;
                    }
                }
                __result = totalPoints;
                return false; 
            }
            catch (Exception ex)
            {
                Plugin.ModLogger.LogError($"[Infinite Darkness] Total points scoring crash: {ex}");
                return true; 
            }
        }
    }
}
