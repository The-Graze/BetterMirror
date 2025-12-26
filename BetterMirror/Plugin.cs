using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterMirror
{
    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<int> _quality;
        private Transform _mirror;
        private Camera _mirrorCam;

        private Plugin()
        {
            _quality = Config.Bind("Settings", "Mirror Quality Multiplier", 4,
                "Times' the mirror quality by this number");
        }

        private void Start() =>
            SceneManager.sceneLoaded += CityLoadedCheck;

        private void CityLoadedCheck(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "City") return;
            var city = GameObject.Find("City_Pretty").transform;
            _mirror = city.FindChildRecursive("DressingRoom_Mirrors_Prefab");

            _mirror.GetChild(1).gameObject.SetActive(false);
            _mirrorCam = _mirror.GetComponentInChildren<Camera>();
            _mirrorCam.farClipPlane = 35;
            _mirrorCam.targetTexture.filterMode = FilterMode.Point;

            _quality.Value = Mathf.Clamp(_quality.Value, 1, 4);

            _mirrorCam.targetTexture.width *= _quality.Value;
            _mirrorCam.targetTexture.height *= _quality.Value;

            SetLayers(city.transform);
        }

        private static void SetLayers(Transform t)
        {
            if (t.gameObject.layer == LayerMask.NameToLayer("NoMirror"))
                t.gameObject.layer = 0;

            foreach (Transform tr in t)
                SetLayers(tr);
        }
    }
}