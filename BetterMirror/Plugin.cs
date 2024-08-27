using BepInEx;
using UnityEngine;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool ran;
        Camera MirrorCam;
        GameObject Mirror;
        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }
        void OnGameInitialized()
        {
            GameObject city = GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.FindChildRecursive("City_WorkingPrefab").gameObject;
            Mirror = city.transform.FindChildRecursive("DressingRoom_Mirrors_Prefab").gameObject;

            Mirror.transform.GetChild(1).gameObject.SetActive(false);
            MirrorCam = Mirror.transform.GetComponentInChildren<Camera>();
            MirrorCam.farClipPlane = 35;
            MirrorCam.targetTexture.filterMode = FilterMode.Point;
            MirrorCam.targetTexture.width = MirrorCam.targetTexture.width * 10;
            MirrorCam.targetTexture.height = MirrorCam.targetTexture.height * 10;
        }
    }
}