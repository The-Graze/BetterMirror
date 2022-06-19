using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace BetterMirror
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.4")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public GameObject bm;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }
        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("BetterMirror.Assets.bm");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject mirror = Instantiate(bundle.LoadAsset<GameObject>("bm"));
            GameObject.Find("Level/city/CosmeticsRoomAnchor/ShoppingCenterAnchor/mirrors2 (1)/ShoppingCart/").transform.SetParent(GameObject.Find("bm(Clone)").transform, false);
            Destroy(GameObject.Find("Level/city/CosmeticsRoomAnchor/ShoppingCenterAnchor/mirrors2 (1)/"));
            GameObject.Find("bm(Clone)").transform.SetParent(GameObject.Find("Level/city/CosmeticsRoomAnchor/").transform, true);
        }

    }


}
