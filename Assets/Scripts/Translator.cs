using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language list", fileName = "Translator")]
public class Translator : ScriptableObject
{
    public string[] keys;
    [Header("RU")]
    public string[] valueRu;
    [Header("EN")]
    public string[] valueEn;

    private Dictionary<string, string> ruText;
    private Dictionary<string, string> enText;

    public string Translating(string key)
    {
        if (ruText == null || enText == null)
        {
            ruText = new Dictionary<string, string>();
            enText = new Dictionary<string, string>();

            for (int i = 0; i < keys.Length; i++)
            {
                ruText.Add(keys[i], valueRu[i]);
                enText.Add(keys[i], valueEn[i]);
            }
        }

        if (StaticVal.language == "ru")
        {
            return ruText[key];
        }
        else
        {
            return enText[key];
        }
    }
}
