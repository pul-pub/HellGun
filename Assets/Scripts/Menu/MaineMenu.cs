using TMPro;
using UnityEngine;
using YG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaineMenu : MonoBehaviour
{
    [SerializeField] private Translator translator;
    [SerializeField] private string[] key;
    [SerializeField] private TextMeshProUGUI[] text;
    public Slider music;
    public Slider sensMouse;
    public TextMeshProUGUI money;
    public GameObject IsShoped1;
    public GameObject IsShoped2;
    public GameObject IsShoped3;
    public GameObject IsShoped4;
    public GameObject IsShoped11;
    public GameObject IsShoped22;
    public GameObject IsShoped33;
    public GameObject IsShoped44;

    private void Awake()
    {
        StaticVal.language = YG.YandexGame.lang;
        Tras();

    }

    private void Update()
    {
        money.text = StaticVal.money.ToString() + " $";

        for (int i = 0; i < StaticVal.shoped.Length; i++)
        {
            if (StaticVal.shoped[i] == 1)
            {
                IsShoped1.SetActive(false);
                IsShoped11.SetActive(true);
            }
            else if (StaticVal.shoped[i] == 2)
            {
                IsShoped2.SetActive(false);
                IsShoped22.SetActive(true);
            }
            else if (StaticVal.shoped[i] == 3)
            {
                IsShoped3.SetActive(false);
                IsShoped33.SetActive(true);
            }
            else if (StaticVal.shoped[i] == 4)
            {
                IsShoped4.SetActive(false);
                IsShoped44.SetActive(true);
            }
        }
    }

    public void Play()
    {
        YG.YandexGame.FullscreenShow();
        StaticVal.ammo = 200;
        StaticVal.levlEnemy = 0;
        for (int i = 0; i < StaticVal.gun.Length; i++)
        {
            StaticVal.gun[i].currentAmmos = StaticVal.gun[i].ammo;
        }
    }

    public void _LoadingScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void SettingVol()
    {
        StaticVal.volMusic = music.value;

        YandexGame.savesData.volMusic = StaticVal.volMusic;
        YandexGame.SaveProgress();
    }

    public void SetSens()
    {
        StaticVal.sens = sensMouse.value;

        YandexGame.savesData.sens = StaticVal.sens;
        YandexGame.SaveProgress();
    }

    public void Shoped(int _id)
    {
        if (StaticVal.money - StaticVal.gun[_id].moneys >= 0)
        {
            StaticVal.money -= StaticVal.gun[_id].moneys;
            StaticVal.shoped[_id] = _id;
        }
        else Debug.Log("No");

        YandexGame.savesData.money = StaticVal.money;
        YandexGame.savesData.shoped = StaticVal.shoped;
        YandexGame.SaveProgress();
    }
    public void SetGun(string _num)
    {
        StaticVal.inv[_num[0]] = _num[1];

        YandexGame.savesData.inv = StaticVal.inv;
        YandexGame.SaveProgress();
    }

    private void Tras()
    {
        for (int i = 0; i < text.Length; i++)
        {
            text[i].text = translator.Translating(key[i]);
        }
    }

    public void OnRewarded()
    {
        StaticVal.money += 725;


        YandexGame.savesData.money = StaticVal.money;
        YandexGame.SaveProgress();
    }
}
