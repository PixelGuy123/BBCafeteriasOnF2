using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace BBCafeteriaOnF2.Plugin
{
	[BepInPlugin(ModInfo.GUID, ModInfo.Name, ModInfo.Version)]
	public class BasePlugin : BaseUnityPlugin
	{
		void Awake()
		{
			Harmony harmony = new(ModInfo.GUID);
			harmony.PatchAll();
		}

		public static bool KeepModEnabled = true; // Since the mod is quite invasive, any other mod can modify this field to disable it

	}


	internal static class ModInfo
	{
		internal const string GUID = "pixelguy.pixelmodding.baldiplus.bbcafeF2";
		internal const string Name = "BB+ Cafeteria in F2";
		internal const string Version = "1.0.0";
	}

	[HarmonyPatch(typeof(LevelGenerator), "StartGenerate")] // I'll wait until the newest API releases, so I don't have to worry with this anymore
	internal class GenPatch
	{
		private static void Prefix(LevelGenerator __instance)
		{
			if (!BasePlugin.KeepModEnabled) return; // Will stop the mod from working (will only be useful if before it actually works lol)

			var cafes = Resources.FindObjectsOfTypeAll<CafeteriaCreator>();
			if (cafes.Length == 0) return;

			var cafe = cafes[0];

			CafeteriaCreator cafeteria;

			if (!didIt[0] && __instance.ld.name == "Main2")
			{
				didIt[0] = true;
				cafeteria = Object.Instantiate(cafe);
				AccessTools.Field(typeof(CafeteriaCreator), "minSize").SetValue(cafeteria, new IntVector2(8, 6));
				AccessTools.Field(typeof(CafeteriaCreator), "maxSize").SetValue(cafeteria, new IntVector2(11, 9));
				Object.DontDestroyOnLoad(cafeteria);
				__instance.ld.specialRooms = __instance.ld.specialRooms.AddToArray(new()
				{
					selection = cafeteria,
					weight = 150
				});
			}

			if (!didIt[1] && __instance.ld.name == "Endless1")
			{
				didIt[1] = true;
				cafeteria = Object.Instantiate(cafe);
				AccessTools.Field(typeof(CafeteriaCreator), "minSize").SetValue(cafeteria, new IntVector2(9, 8));
				AccessTools.Field(typeof(CafeteriaCreator), "maxSize").SetValue(cafeteria, new IntVector2(11, 9));
				Object.DontDestroyOnLoad(cafeteria);
				__instance.ld.specialRooms = __instance.ld.specialRooms.AddToArray(new()
				{
					selection = cafeteria,
					weight = 200
				});
			}

		}

		readonly static bool[] didIt = new bool[2];
	}
}
