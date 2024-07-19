using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace RadiantMenu.Consts
{
    public class TextureStrings
    {
        #region Misc
        public const string HkLogoBlackKey = "HKLogoBlack";
        private const string HkLogoBlackFile = "RadiantMenu.Resources.HKLogoBlack.png";
        public const string RadDlcLogoKey = "RadiantMenuLogoThing";
        private const string RadDlcLogoFile = "RadiantMenu.Resources.RadiantMenuLogoThing.png";
        #endregion Misc

        private readonly Dictionary<string, Sprite> _dict;

        public TextureStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            _dict = new Dictionary<string, Sprite>();
            string[] tmpTextureFiles = {
                HkLogoBlackFile,
                RadDlcLogoFile
            };
            string[] tmpTextureKeys = {
                HkLogoBlackKey,
                RadDlcLogoKey
            };
            for (int i = 0; i < tmpTextureFiles.Length; i++)
            {
                using (Stream s = asm.GetManifestResourceStream(tmpTextureFiles[i]))
                {
                    if (s == null) continue;

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    s.Dispose();

                    //Create texture from bytes
                    var tex = new Texture2D(2, 2);

                    tex.LoadImage(buffer, true);

                    // Create sprite from texture
                    // Split is to cut off the PaleCourtStuff.Resources. and the .png
                    _dict.Add(tmpTextureKeys[i], Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 64));
                }
            }
        }

        public Sprite Get(string key)
        {
            return _dict[key];
        }
    }
}