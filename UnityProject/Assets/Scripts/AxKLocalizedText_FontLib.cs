using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AxKLocalizedText_FontLib", order = 1)]
public class AxKLocalizedText_FontLib : ScriptableObject
{
    // Token: 0x06000E3D RID: 3645 RVA: 0x000553C0 File Offset: 0x000535C0
    public int GetBaseId(Font font)
    {
        for (int i = 0; i < this.fontSets.Length; i++)
        {
            if (font == this.fontSets[i].english)
            {
                return i;
            }
        }
        return -1;
    }

    // Token: 0x06000E3E RID: 3646 RVA: 0x000553FC File Offset: 0x000535FC
    public int GetBaseId(TMP_FontAsset font)
    {
        for (int i = 0; i < this.textMeshProFontSets.Length; i++)
        {
            if (font == this.textMeshProFontSets[i].english)
            {
                return i;
            }
        }
        return -1;
    }

    // Token: 0x06000E43 RID: 3651 RVA: 0x00008551 File Offset: 0x00006751
    public AxKLocalizedText_FontLib()
    {
    }

    // Token: 0x04001121 RID: 4385
    public AxKLocalizedText_FontLib.FontSet[] fontSets;

    // Token: 0x04001122 RID: 4386
    public AxKLocalizedText_FontLib.FontSetPro[] textMeshProFontSets;

    // Token: 0x020003A1 RID: 929
    [Serializable]
    public struct FontSet
    {
        // Token: 0x060017B8 RID: 6072 RVA: 0x00087940 File Offset: 0x00085B40
        public Font GetFontAtIndex(int index)
        {
            Font font = null;
            if (index == 0)
            {
                font = this.english;
            }
            else if (index == 1)
            {
                font = this.french;
            }
            else if (index == 2)
            {
                font = this.italian;
            }
            else if (index == 3)
            {
                font = this.german;
            }
            else if (index == 4)
            {
                font = this.spanish;
            }
            else if (index == 5)
            {
                font = this.russian;
            }
            else if (index == 6)
            {
                font = this.japanese;
            }
            else if (index == 7)
            {
                font = this.korean;
            }
            else if (index == 8)
            {
                font = this.chineseSimp;
            }
            else if (index == 9)
            {
                font = this.chinese;
            }
            else if (index == 10)
            {
                font = this.polish;
            }
            else if (index == 11)
            {
                font = this.portuguese;
            }
            else if (index == 12)
            {
                font = this.spanishLatin;
            }
            else if (index == 13)
            {
                font = this.turkish;
            }
            if (font == null)
            {
                return this.english;
            }
            return font;
        }

        // Token: 0x04001E5D RID: 7773
        public Font english;

        // Token: 0x04001E5E RID: 7774
        public Font french;

        // Token: 0x04001E5F RID: 7775
        public Font italian;

        // Token: 0x04001E60 RID: 7776
        public Font german;

        // Token: 0x04001E61 RID: 7777
        public Font spanish;

        // Token: 0x04001E62 RID: 7778
        public Font russian;

        // Token: 0x04001E63 RID: 7779
        public Font japanese;

        // Token: 0x04001E64 RID: 7780
        public Font korean;

        // Token: 0x04001E65 RID: 7781
        public Font chineseSimp;

        // Token: 0x04001E66 RID: 7782
        public Font chinese;

        // Token: 0x04001E67 RID: 7783
        public Font polish;

        // Token: 0x04001E68 RID: 7784
        public Font portuguese;

        // Token: 0x04001E69 RID: 7785
        public Font spanishLatin;

        // Token: 0x04001E6A RID: 7786
        public Font turkish;
    }

    // Token: 0x020003A2 RID: 930
    [Serializable]
    public struct FontSetPro
    {
        // Token: 0x060017B9 RID: 6073 RVA: 0x00087A24 File Offset: 0x00085C24
        public TMP_FontAsset GetFontAtIndex(int index)
        {
            TMP_FontAsset tmp_FontAsset = null;
            if (index == 0)
            {
                tmp_FontAsset = this.english;
            }
            else if (index == 1)
            {
                tmp_FontAsset = this.french;
            }
            else if (index == 2)
            {
                tmp_FontAsset = this.italian;
            }
            else if (index == 3)
            {
                tmp_FontAsset = this.german;
            }
            else if (index == 4)
            {
                tmp_FontAsset = this.spanish;
            }
            else if (index == 5)
            {
                tmp_FontAsset = this.russian;
            }
            else if (index == 6)
            {
                tmp_FontAsset = this.japanese;
            }
            else if (index == 7)
            {
                tmp_FontAsset = this.korean;
            }
            else if (index == 8)
            {
                tmp_FontAsset = this.chineseSimp;
            }
            else if (index == 9)
            {
                tmp_FontAsset = this.chinese;
            }
            else if (index == 10)
            {
                tmp_FontAsset = this.polish;
            }
            else if (index == 11)
            {
                tmp_FontAsset = this.portuguese;
            }
            else if (index == 12)
            {
                tmp_FontAsset = this.spanishLatin;
            }
            else if (index == 13)
            {
                tmp_FontAsset = this.turkish;
            }
            if (tmp_FontAsset == null)
            {
                return this.english;
            }
            return tmp_FontAsset;
        }

        // Token: 0x060017BA RID: 6074 RVA: 0x00087B08 File Offset: 0x00085D08
        public Material[] GetFontMaterialSetAtIndex(int index)
        {
            Material[] array = null;
            if (index == 0)
            {
                array = this.englishFontMats;
            }
            else if (index == 1)
            {
                array = this.frenchFontMats;
            }
            else if (index == 2)
            {
                array = this.italianFontMats;
            }
            else if (index == 3)
            {
                array = this.germanFontMats;
            }
            else if (index == 4)
            {
                array = this.spanishFontMats;
            }
            else if (index == 5)
            {
                array = this.russianFontMats;
            }
            else if (index == 6)
            {
                array = this.japaneseFontMats;
            }
            else if (index == 7)
            {
                array = this.koreanFontMats;
            }
            else if (index == 8)
            {
                array = this.chineseSimpFontMats;
            }
            else if (index == 9)
            {
                array = this.chineseFontMats;
            }
            else if (index == 10)
            {
                array = this.polishFontMats;
            }
            else if (index == 11)
            {
                array = this.portugueseFontMats;
            }
            else if (index == 12)
            {
                array = this.spanishLatinFontMats;
            }
            else if (index == 13)
            {
                array = this.turkishFontMats;
            }
            if (array == null)
            {
                return this.englishFontMats;
            }
            return array;
        }

        // Token: 0x060017BB RID: 6075 RVA: 0x00087BE8 File Offset: 0x00085DE8
        public void UpdateMaterialSetAtIndex(int index, Material[] newMaterialSet)
        {
            Debug.Log("Updating material set for " + index.ToString() + " with " + newMaterialSet.ToString());
            if (index == 0)
            {
                this.englishFontMats = newMaterialSet;
                return;
            }
            if (index == 1)
            {
                this.frenchFontMats = newMaterialSet;
                return;
            }
            if (index == 2)
            {
                this.italianFontMats = newMaterialSet;
                return;
            }
            if (index == 3)
            {
                this.germanFontMats = newMaterialSet;
                return;
            }
            if (index == 4)
            {
                this.spanishFontMats = newMaterialSet;
                return;
            }
            if (index == 5)
            {
                this.russianFontMats = newMaterialSet;
                return;
            }
            if (index == 6)
            {
                this.japaneseFontMats = newMaterialSet;
                return;
            }
            if (index == 7)
            {
                this.koreanFontMats = newMaterialSet;
                return;
            }
            if (index == 8)
            {
                this.chineseSimpFontMats = newMaterialSet;
                return;
            }
            if (index == 9)
            {
                this.chineseFontMats = newMaterialSet;
                return;
            }
            if (index == 10)
            {
                this.polishFontMats = newMaterialSet;
                return;
            }
            if (index == 11)
            {
                this.portugueseFontMats = newMaterialSet;
                return;
            }
            if (index == 12)
            {
                this.spanishLatinFontMats = newMaterialSet;
                return;
            }
            if (index == 13)
            {
                this.turkishFontMats = newMaterialSet;
            }
        }

        // Token: 0x04001E6B RID: 7787
        public TMP_FontAsset english;

        // Token: 0x04001E6C RID: 7788
        public Material[] englishFontMats;

        // Token: 0x04001E6D RID: 7789
        public TMP_FontAsset french;

        // Token: 0x04001E6E RID: 7790
        public Material[] frenchFontMats;

        // Token: 0x04001E6F RID: 7791
        public TMP_FontAsset italian;

        // Token: 0x04001E70 RID: 7792
        public Material[] italianFontMats;

        // Token: 0x04001E71 RID: 7793
        public TMP_FontAsset german;

        // Token: 0x04001E72 RID: 7794
        public Material[] germanFontMats;

        // Token: 0x04001E73 RID: 7795
        public TMP_FontAsset spanish;

        // Token: 0x04001E74 RID: 7796
        public Material[] spanishFontMats;

        // Token: 0x04001E75 RID: 7797
        public TMP_FontAsset russian;

        // Token: 0x04001E76 RID: 7798
        public Material[] russianFontMats;

        // Token: 0x04001E77 RID: 7799
        public TMP_FontAsset japanese;

        // Token: 0x04001E78 RID: 7800
        public Material[] japaneseFontMats;

        // Token: 0x04001E79 RID: 7801
        public TMP_FontAsset korean;

        // Token: 0x04001E7A RID: 7802
        public Material[] koreanFontMats;

        // Token: 0x04001E7B RID: 7803
        public TMP_FontAsset chineseSimp;

        // Token: 0x04001E7C RID: 7804
        public Material[] chineseSimpFontMats;

        // Token: 0x04001E7D RID: 7805
        public TMP_FontAsset chinese;

        // Token: 0x04001E7E RID: 7806
        public Material[] chineseFontMats;

        // Token: 0x04001E7F RID: 7807
        public TMP_FontAsset polish;

        // Token: 0x04001E80 RID: 7808
        public Material[] polishFontMats;

        // Token: 0x04001E81 RID: 7809
        public TMP_FontAsset portuguese;

        // Token: 0x04001E82 RID: 7810
        public Material[] portugueseFontMats;

        // Token: 0x04001E83 RID: 7811
        public TMP_FontAsset spanishLatin;

        // Token: 0x04001E84 RID: 7812
        public Material[] spanishLatinFontMats;

        // Token: 0x04001E85 RID: 7813
        public TMP_FontAsset turkish;

        // Token: 0x04001E86 RID: 7814
        public Material[] turkishFontMats;

        // Token: 0x04001E87 RID: 7815
        public const int NUM_LANGUAGES = 13;
    }
}
