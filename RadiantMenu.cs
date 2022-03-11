using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using RadiantMenu.Consts;
using SFCore;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
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

        private readonly int _hkLogoBlackId;
        private Text VersionNumber = null;
        private string newVersionNumberText;

        public RadiantMenu() : base("Radiant Menu Theme")
        {
            LangStrings = new LanguageStrings();
            SpriteDict = new TextureStrings();

            MenuStyleHelper.AddMenuStyleHook += AddRadiantMenuStyle;

            _hkLogoBlackId = TitleLogoHelper.AddLogo(SpriteDict.Get(TextureStrings.HkLogoBlackKey));
            
            var splitText = Constants.GAME_VERSION.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            int part1 = int.Parse(splitText[0]);
            int part2 = int.Parse(splitText[1]);
            int part3 = int.Parse(splitText[2]) + 6;
            int part4 = int.Parse(splitText[3]) + 5555;
            newVersionNumberText = $"{part1}.{part2}.{part3}.{part4}";
            
            On.UIManager.Start += AddRadiantIcon;
            On.SetVersionNumber.Start += (orig, self) =>
            {
                orig(self);
                VersionNumber = self.GetComponent<Text>();
            };

            On.MenuStyles.SetStyle += (orig, self, index, fade, save) =>
            {
                if (VersionNumber != null)
                {
                    if (self.styles[index].displayName == "UI_MENU_STYLE_RADIANT")
                    {
                        VersionNumber.text = newVersionNumberText;
                    }
                    else
                    {
                        VersionNumber.text = Constants.GAME_VERSION;
                    }
                }
                orig(self, index, fade, save);
            };
        }

        public override void Initialize()
        {
            ModHooks.LanguageGetHook += OnLanguageGetHook;
        }

        private void AddRadiantIcon(On.UIManager.orig_Start orig, UIManager self)
        {
            orig(self);

            var dlc = self.transform.Find("UICanvas/MainMenuScreen/TeamCherryLogo/Hidden_Dreams_Logo").gameObject;

            var clone = Object.Instantiate(dlc, dlc.transform.parent);
            clone.SetActive(true);

            var pos = clone.transform.position;

            clone.transform.position = pos + new Vector3(3.1f, -0.15f, 0);

            var sr = clone.GetComponent<SpriteRenderer>();
            sr.sprite = SpriteDict.Get(TextureStrings.RadDlcLogoKey);
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
            const float rgbMultiplier = 0.875f; // 0 means black, 1 means original colour
            GameObject menuStylesGo = self.gameObject;
            var radiantStyleGo = menuStylesGo.transform.GetChild(4).gameObject;
            foreach (var sr in radiantStyleGo.GetComponentsInChildren<SpriteRenderer>())
            {
                var tmpColor = sr.color;
                tmpColor.r *= rgbMultiplier;
                tmpColor.g *= rgbMultiplier;
                tmpColor.b *= rgbMultiplier;
                sr.color = tmpColor;
            }
            foreach (var ps in radiantStyleGo.GetComponentsInChildren<ParticleSystem>())
            {
                var main = ps.main;
                var tmpGrad = main.startColor;
                var tmpColor = tmpGrad.colorMin;
                tmpColor.r *= rgbMultiplier;
                tmpColor.g *= rgbMultiplier;
                tmpColor.b *= rgbMultiplier;
                tmpGrad.colorMin = tmpColor;
                tmpColor = tmpGrad.colorMax;
                tmpColor.r *= rgbMultiplier;
                tmpColor.g *= rgbMultiplier;
                tmpColor.b *= rgbMultiplier;
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
