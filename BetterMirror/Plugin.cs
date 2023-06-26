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
        bool set;
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
            GameObject.Find("Level").AddComponent<LayerChanger>();
        }
        void Update()
        {
            if (set == false &&GameObject.Find("Level/city").activeSelf == true || GameObject.Find("Level/city") != null)
            {
                GameObject.Find("mirrors2 (1)").GetComponent<Renderer>().materials[1].mainTexture = bm;
                GameObject.Find("CameraC").GetComponent<Camera>().targetTexture = bm;
                set = true;
            }
            if (set == true && cull != 0)
            {
                Camera ca = GameObject.Find("Shoulder Camera").GetComponent<Camera>();
                ca.cullingMask = cull;
                Destroy(this);
            }
        }
    }
    public class LayerChanger : MonoBehaviour
    {
        void Start()
        {
            Invoke("aDestory", 5);
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
                Plugin.Instance.cull = GetComponent<Camera>().cullingMask;
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
        void aDestory()
        {
            Destroy(this);
        }
    }
}