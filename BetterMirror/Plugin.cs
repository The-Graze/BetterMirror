using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Collections;
using Oculus.Platform;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
        public RenderTexture bm;
        public static volatile Plugin Instance;
        public int cull;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            Instance = this;
            bm = new RenderTexture(2000, 2000, 24, RenderTextureFormat.Default);
            bm.Create();
            bm.filterMode = FilterMode.Point;
            bm.antiAliasing = 3;
            GameObject.Find("LocalObjects_Prefab").AddComponent<LayerChanger>();
            gameObject.AddComponent<CamChange>();
        }
        void Update()
        {
            if(cull == 0) 
            {
                GameObject.Find("Environment Objects/LocalObjects_Prefab").transform.GetChild(3).gameObject.SetActive(true);

                GameObject.Find("mirrors2 (1)").GetComponent<Renderer>().materials[1].mainTexture = bm;
                GameObject.Find("CameraC").GetComponent<Camera>().targetTexture = bm;
            }
            else
            {
                gameObject.GetComponent<CamChange>().cull = cull;
                Destroy(this);
            }
        }
    }
    class CamChange : MonoBehaviour
    {
        public int cull;
        void Update()
        {
            if (cull != 0)
            {
                foreach (Camera c in Camera.allCameras)
                {
                    if (c != Camera.main && c.cullingMask != cull)
                    {
                        c.cullingMask = cull;
                    }
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
                Destroy(this);
            }
            if (gameObject.name == "Mirror Backdrop")
            {
                Destroy(gameObject);
            }
            if (gameObject.name == "CameraC")
            {
                Plugin.Instance.cull = gameObject.GetComponent<Camera>().cullingMask;
                GetComponent<Camera>().farClipPlane = 35;
                Destroy(this);
            }
            foreach (Transform t in gameObject.transform)
            {
                t.gameObject.AddComponent<LayerChanger>();
                Destroy(this);
            }
            Destroy(this);
        }
    }
}