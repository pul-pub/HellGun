using TMPro;
using UnityEngine;
using YG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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

    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    private void Awake()
    {
        StaticVal.language = YandexGame.EnvironmentData.language;
        StaticVal.volMusic = YandexGame.savesData.volMusic;
        StaticVal.money = YandexGame.savesData.money;
        StaticVal.sens = YandexGame.savesData.sens;
        StaticVal.inv = YandexGame.savesData.inv;
        StaticVal.shoped = YandexGame.savesData.shoped;
        Tras();

        AudioListener.volume = StaticVal.volMusic;

        music.value = YandexGame.savesData.volMusic;
        sensMouse.value = YandexGame.savesData.sens;
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
        YandexGame.FullscreenShow();
        StaticVal.ammo = 300;
        for (int i = 0; i < StaticVal.gun.Length; i++)
        {
            StaticVal.gun[i].currentAmmos = StaticVal.gun[i].ammo;
        }
    }

    public void _LoadingScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void SettingVol()
    {
        StaticVal.volMusic = music.value;
        AudioListener.volume = StaticVal.volMusic;

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
        string[] _inv = _num.Split(',');
        StaticVal.inv[Int32.Parse(_inv[0])] = Int32.Parse(_inv[1]);
        Debug.Log(_inv[0]);
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

    public void Rewarded(int id)
    {
        StaticVal.money += 725;

        YandexGame.savesData.money = StaticVal.money;
        YandexGame.SaveProgress();
    }
}
