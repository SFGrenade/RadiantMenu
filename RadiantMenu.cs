using System;
using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using RadiantMenu.Consts;
using SFCore;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.Audio;
using LanguageStrings = RadiantMenu.Consts.LanguageStrings;
using Object = UnityEngine.Object;

namespace RadiantMenu
{
    public class RadiantMenu : Mod
    {
        public override string GetVersion() => Util.GetVersion(Assembly.GetExecutingAssembly());

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                // Title Screen needs a preload for no reason
                new ValueTuple<string, string>("Room_shop", "_SceneManager")
            };
        }

        public LanguageStrings LangStrings { get; private set; }
        public TextureStrings SpriteDict { get; private set; }

        private int _hkLogoBlackId = -1;

        public RadiantMenu() : base("Radiant Menu Theme")
        {
            LangStrings = new LanguageStrings();
            SpriteDict = new TextureStrings();

            MenuStyleHelper.AddMenuStyleHook += AddRadiantMenuStyle;

            _hkLogoBlackId = TitleLogoHelper.AddLogo(SpriteDict.Get(TextureStrings.HkLogoBlackKey));
        }

        public override void Initialize()
        {
            ModHooks.LanguageGetHook += OnLanguageGetHook;
        }
        
        private string OnLanguageGetHook(string key, string sheet, string orig)
        {
            //Log($"Sheet: {sheet}; Key: {key}");
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return orig;
        }

        private (string languageString, GameObject styleGo, int titleIndex, string unlockKey, string[] achievementKeys, MenuStyles.MenuStyle.CameraCurves cameraCurves, AudioMixerSnapshot musicSnapshot) AddRadiantMenuStyle(MenuStyles self)
        {
            GameObject menuStylesGo = self.gameObject;
            var radiantStyleGo = menuStylesGo.transform.GetChild(4).gameObject;
            foreach (var sr in radiantStyleGo.GetComponentsInChildren<SpriteRenderer>())
            {
                var tmpColor = sr.color;
                tmpColor.r *= 0.75f;
                tmpColor.g *= 0.75f;
                tmpColor.b *= 0.75f;
                sr.color = tmpColor;
            }
            foreach (var ps in radiantStyleGo.GetComponentsInChildren<ParticleSystem>())
            {
                var main = ps.main;
                var tmpGrad = main.startColor;
                var tmpColor = tmpGrad.colorMin;
                tmpColor.r *= 0.75f;
                tmpColor.g *= 0.75f;
                tmpColor.b *= 0.75f;
                tmpGrad.colorMin = tmpColor;
                tmpColor = tmpGrad.colorMax;
                tmpColor.r *= 0.75f;
                tmpColor.g *= 0.75f;
                tmpColor.b *= 0.75f;
                tmpGrad.colorMax = tmpColor;
                main.startColor = tmpGrad;
            }
            radiantStyleGo.transform.localPosition = new Vector3(-6.72f, 3.72f);
            radiantStyleGo.transform.GetChild(0).localPosition = new Vector3(0, -2.73f, -29.2f);
            radiantStyleGo.transform.GetChild(0).localEulerAngles = new Vector3(-90, 90, -90);
            radiantStyleGo.transform.GetChild(0).GetChild(0).localPosition = new Vector3(0, -83.9f, -0.19f);
            radiantStyleGo.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(-90, 0, 0);
            radiantStyleGo.transform.GetChild(0).GetChild(1).localPosition = new Vector3(0, -91.6f, -0.19f);
            radiantStyleGo.transform.GetChild(0).GetChild(1).localEulerAngles = new Vector3(-90, 0, 29.95f);
            radiantStyleGo.transform.GetChild(0).GetChild(2).localPosition = new Vector3(0, -163.5f, -0.19f);
            radiantStyleGo.transform.GetChild(0).GetChild(2).localEulerAngles = new Vector3(-90, 0, 67.2f);
            radiantStyleGo.transform.GetChild(1).localPosition = new Vector3(0, 0, -1.145f);
            radiantStyleGo.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
            radiantStyleGo.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, -9, 47.5f);
            radiantStyleGo.transform.GetChild(1).GetChild(0).localEulerAngles = new Vector3(0, 0, -192.483f);
            radiantStyleGo.transform.GetChild(2).localPosition = new Vector3(0, 7.4f, 21.21f);
            radiantStyleGo.transform.GetChild(3).localPosition = new Vector3(0, -32.4f, 103.2f);
            radiantStyleGo.transform.GetChild(4).localPosition = new Vector3(0, -2.7f, 145.33f);
            radiantStyleGo.transform.GetChild(5).localPosition = new Vector3(0, -4.22f, 142.91f);

            GameObject audioGo = Object.Instantiate(self.styles[4].styleObject.transform.GetChild(8).gameObject, radiantStyleGo.transform);
            audioGo.transform.position = Vector3.zero;
            AudioSource aSource = audioGo.GetComponent<AudioSource>();
            //dream_dialogue_loop
            aSource.clip = null;
            foreach (var ac in Resources.FindObjectsOfTypeAll<AudioClip>())
            {
                if (ac.name == "dream_dialogue_loop")
                {
                    aSource.clip = ac;
                    break;
                }
            }
            aSource.volume = 0.5f;

            var cameraCurves = new MenuStyles.MenuStyle.CameraCurves
            {
                saturation = 1.0f,
                redChannel = new AnimationCurve(),
                greenChannel = new AnimationCurve(),
                blueChannel = new AnimationCurve()
            };
            cameraCurves.redChannel.AddKey(new Keyframe(0f, 0f));
            cameraCurves.redChannel.AddKey(new Keyframe(1f, 1f));
            cameraCurves.greenChannel.AddKey(new Keyframe(0f, 0f));
            cameraCurves.greenChannel.AddKey(new Keyframe(1f, 1f));
            cameraCurves.blueChannel.AddKey(new Keyframe(0f, 0f));
            cameraCurves.blueChannel.AddKey(new Keyframe(1f, 1f));

            //AudioMixerSnapshot audioSnapshot = self.styles[1].musicSnapshot.audioMixer.FindSnapshot("Normal - Gramaphone");
            AudioMixerSnapshot audioSnapshot = self.styles[1].musicSnapshot.audioMixer.FindSnapshot("Normal");
            return ("UI_MENU_STYLE_RADIANT", radiantStyleGo, _hkLogoBlackId, "", null, cameraCurves, audioSnapshot);
        }
    }
}
