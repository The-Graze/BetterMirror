using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Collections;
using Oculus.Platform;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    public class Plugin : BaseUnityPlugin
    {
        public RenderTexture bm;
        bool ran;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            bm = new RenderTexture(2000, 2000, 24, RenderTextureFormat.Default);
            bm.filterMode = FilterMode.Point;
            bm.antiAliasing = 3;
            bm.Create();
            GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.GetChild(3).gameObject.AddComponent<LayerChanger>();

            int mirrorOnlyMask = 1 << LayerMask.NameToLayer("MirrorOnly");
            int noMirrorMask = 1 << LayerMask.NameToLayer("NoMirror");

            Camera.allCameras[1].cullingMask |= mirrorOnlyMask;
            Camera.allCameras[1].cullingMask &= ~noMirrorMask;
        }
        void Update()
        {
            if (ran == false)
            {
                try
                {
                    GameObject.Find("mirrors2 (1)").GetComponent<Renderer>().materials[1].mainTexture = bm;
                    GameObject.Find("CameraC").GetComponent<Camera>().targetTexture = bm;
                    GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.GetChild(3).gameObject.SetActive(false);
                    ran = true;
                }
                catch
                {
                    GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.GetChild(3).gameObject.SetActive(true);
                }
            }
        }
    }

    public class LayerChanger : MonoBehaviour
    {
        void Start()
        {
            if (gameObject.layer == LayerMask.NameToLayer("NoMirror"))
            {
                gameObject.layer = 0;
            }
            if (gameObject.name == "Mirror Backdrop")
            {
                Destroy(gameObject);
            }
            if (gameObject.name == "CameraC")
            {
                GetComponent<Camera>().farClipPlane = 35;
            }
            foreach (Transform t in gameObject.transform)
            {
                t.gameObject.AddComponent<LayerChanger>();
            }
            Destroy(this);
        }
    }
}