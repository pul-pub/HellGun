using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaineMenu : MonoBehaviour
{
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
        if (PlayerPrefs.GetString("flag") != "t")
        {
            PlayerPrefs.SetInt("money", StaticVal.money);
            PlayerPrefs.SetFloat("music", StaticVal.volMusic);
            PlayerPrefs.SetFloat("sens", StaticVal.sens);
            PlayerPrefs.SetString("flag", "t");
        }
        else
        {
            StaticVal.money = PlayerPrefs.GetInt("money");
            StaticVal.volMusic = PlayerPrefs.GetFloat("music");
            StaticVal.sens = PlayerPrefs.GetFloat("sens");
            StaticVal.inv[0] = PlayerPrefs.GetInt("inv0");
            StaticVal.inv[1] = PlayerPrefs.GetInt("inv1");

            for (int i = 0; i < StaticVal.shoped.Length; i++)
            {
                StaticVal.shoped[i] = PlayerPrefs.GetInt("shop" + i.ToString());
            }
        }
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
        //Adv.ShowAdv();
        //Adv.ShowAdv();
        StaticVal.ammo = 200;
        StaticVal.levlEnemy = 0;
        for (int i = 0; i < StaticVal.gun.Length; i++)
        {
            StaticVal.gun[i].currentAmmos = 0;
        }
    }
    public void SettingVol()
    {
        StaticVal.volMusic = music.value;
        PlayerPrefs.SetFloat("music", StaticVal.volMusic);
    }
    public void SetSens()
    {
        StaticVal.sens = sensMouse.value;
        PlayerPrefs.SetFloat("sens", StaticVal.sens);
    }
    public void Shoped(int _id)
    {
        if (StaticVal.money - StaticVal.gun[_id].moneys >= 0)
        {
            StaticVal.money -= StaticVal.gun[_id].moneys;
            StaticVal.shoped[_id] = _id;
            PlayerPrefs.SetInt("shop" + _id.ToString(), _id);
        }
        else Debug.Log("No");
    }
    public void SetGun(string _num)
    {
        if (_num == "01")////
        {
            StaticVal.inv[0] = 1;
            PlayerPrefs.SetInt("inv0", 1);
        }
        else if (_num == "02")
        {
            StaticVal.inv[0] = 2;
            PlayerPrefs.SetInt("inv0", 2);
        }
        else if (_num == "03")
        {
            StaticVal.inv[0] = 3;
            PlayerPrefs.SetInt("inv0", 3);
        }
        else if (_num == "04")/////////
        {
            StaticVal.inv[0] = 4;
            PlayerPrefs.SetInt("inv0", 4);
        }
        else if (_num == "11")/////////
        {
            StaticVal.inv[1] = 1;
            PlayerPrefs.SetInt("inv1", 1);
        }
        else if (_num == "12")
        {
            StaticVal.inv[1] = 2;
            PlayerPrefs.SetInt("inv1", 2);
        }
        else if (_num == "13")
        {
            StaticVal.inv[1] = 3;
            PlayerPrefs.SetInt("inv1", 3);
        }
        else if (_num == "14") /////
        {
            StaticVal.inv[1] = 4;
            PlayerPrefs.SetInt("inv1", 4);
        }
    }
}
