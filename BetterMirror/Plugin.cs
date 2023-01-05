using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Collections;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public RenderTexture bm;
        bool set;
        void Start()
        {
            bm = new RenderTexture(2000, 2000, 24, RenderTextureFormat.Default);
            bm.Create();
            bm.filterMode = FilterMode.Point;
            bm.antiAliasing = 3;
            set = false;
        }
        void Update()
        {
            if (set == false &&GameObject.Find("Level/city").activeSelf == true || GameObject.Find("Level/city") != null)
            {
                GameObject.Find("mirrors2 (1)").GetComponent<Renderer>().materials[1].mainTexture = bm;
                GameObject.Find("CameraC").GetComponent<Camera>().targetTexture = bm;
                set= true;
            }
            else{}
        }
    }
}