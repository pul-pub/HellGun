using UnityEngine;
using TMPro;

public class Yandex : MonoBehaviour
{
    [SerializeField] private bool inAwake = false;
    [SerializeField] private TextMeshProUGUI[] textMesh;
    [SerializeField] private string[] textRU;
    [SerializeField] private string[] textEN;

    private void Awake()
    {
        if (inAwake)
        {
            if (Application.absoluteURL.Contains(".ru"))
            {
                StaticVal.language = "ru";
            }
            else
            {
                StaticVal.language = "en";
            }
        }
        if (StaticVal.language == "ru") for (int i = 0; i < textMesh.Length; i++) textMesh[i].text = textRU[i];
        else for (int i = 0; i < textMesh.Length; i++) textMesh[i].text = textEN[i];
    }
}
