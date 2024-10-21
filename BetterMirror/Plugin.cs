using BepInEx;
using System.Collections;
using UnityEngine;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool ran;
        Camera MirrorCam;
        Transform Mirror;
        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }
        void OnGameInitialized()
        {
            Transform city = GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.FindChildRecursive("City_WorkingPrefab");
            Mirror = city.FindChildRecursive("DressingRoom_Mirrors_Prefab");

            Mirror.GetChild(1).gameObject.SetActive(false);
            MirrorCam = Mirror.GetComponentInChildren<Camera>();
            MirrorCam.farClipPlane = 35;
            MirrorCam.targetTexture.filterMode = FilterMode.Point;
            MirrorCam.targetTexture.width = MirrorCam.targetTexture.width * 5;
            MirrorCam.targetTexture.height = MirrorCam.targetTexture.height * 5;
            MirrorCam.depth = 5;
            MirrorCam.clearFlags = CameraClearFlags.Depth;

            StartCoroutine(UnLayer(city));
        }

        IEnumerator UnLayer(Transform city)
        {
            yield return new WaitForSeconds(2);
            foreach (Transform t in city)
            {
                if (t.gameObject.layer == LayerMask.NameToLayer("NoMirror"))
                {
                    t.gameObject.layer = 0;
                }
            }
        }
    }
}