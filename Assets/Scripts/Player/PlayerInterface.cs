using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using YG;

public class PlayerInterface : MonoBehaviour
{
    public Translator translator;
    [Header("Texture")]
    [SerializeField] private Sprite[] imgWeapon;
    [SerializeField] private Image icon1gun;
    [SerializeField] private Image icon2gun;
    [Header("GUI")]
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject opticsAim;
    [SerializeField] private GameObject hit;
    [SerializeField] private Slider progressBarPoint;
    [SerializeField] private Image bgPoint;
    [SerializeField] private Image imgPoint;
    [SerializeField] private Point point;
    [SerializeField] private Slider progressBarHealth;
    [SerializeField] private TextMeshProUGUI textForPlayer; 
    [Header("Enemy and Icon")]
    [SerializeField] private RectTransform[] iconEnemy;
    [SerializeField] private Transform[] enemyTransform;
    [Header("Weapons/Ammos for it")]
    [SerializeField] private TextMeshProUGUI textAmmosInInventory;
    [SerializeField] private TextMeshProUGUI textAmmos;
    [Header("Screens")]
    [SerializeField] private GameObject screenPause;
    [SerializeField] private TextMeshProUGUI contline;
    [SerializeField] private TextMeshProUGUI exit;
    [SerializeField] private TextMeshProUGUI exitWin;
    [SerializeField] private Image screenWin;
    [SerializeField] private TextMeshProUGUI textForWinOrFail;
    [SerializeField] private TextMeshProUGUI textPause;
    [SerializeField] private TextMeshProUGUI textScores;
    [Header("Medic")]
    [SerializeField] private TextMeshProUGUI textMedic;
    [SerializeField] private Slider sliderMedic;
    [Header("Settings")]
    [SerializeField] private Slider music;
    [SerializeField] private Slider sensMouse;
    [SerializeField] private TextMeshProUGUI headerSettings;
    [SerializeField] private TextMeshProUGUI butSettings;
    [SerializeField] private TextMeshProUGUI sens;
    [SerializeField] private TextMeshProUGUI vol;
    [SerializeField] private TextMeshProUGUI back;

    private Camera _cam;
    private Player _player;
    private bool _isHit = false;
    private float timer = 1f;
    public bool _isPause = false;
    public bool _isWinOrFail = false;

    private void Awake()
    {
        _cam = Camera.main;
        _player = GetComponent<Player>();
        _isHit = false;
        _isPause = false;
        _isWinOrFail = false;

        SetWeaponImg();
        StartCoroutine(Messages("task"));
        AwakeTranslet();

        music.value = StaticVal.volMusic;
        sensMouse.value = StaticVal.sens;
    }

    void Update()
    {
        SetPositionIcon();
        UpdateTextAmmos();
        progressBarHealth.value = _player.Health;

        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) && !_isWinOrFail)
        {
            if (_isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (point.time != 30f)
        {
            progressBarPoint.value = point.time / 100;
        }
        else
        {
            progressBarPoint.value = 1;
        }

        if (point.capturePoints[0] == WhoCapturingPoint.Enemy)
        {
            imgPoint.color = Color.red;
        }
        else if (point.capturePoints[0] == WhoCapturingPoint.Player)
        {
            imgPoint.color = Color.blue;
        }

        if (bgPoint.color != point.color)
        {
            bgPoint.color = point.color;
        }

        if (point.time >= 30f && point.capturePoints[0] == WhoCapturingPoint.Player && timer <= 0f && !_isPause && !_isWinOrFail)
        {
            StaticVal.scoreWithPoint++;
            timer = 1f;
        }
        else if (timer > 0f && !_isPause && !_isWinOrFail)
        {
            timer -= Time.deltaTime;
        }

        if ((StaticVal.scoreWithPoint >= 100 || StaticVal.numEnemy <= 0) && !_isWinOrFail)
        {
            if (StaticVal.scoreWithPoint >= 120)
            {
                StaticVal.moneyForBattle += 575;
            }
            WinGame();
        }
        else if (_player.Health <= 0f && !_isWinOrFail)
        {
            FailGame();
        }
    }

    private void AwakeTranslet()
    {
        contline.text = translator.Translating("contline");
        exit.text = translator.Translating("toMenu");
        exitWin.text = translator.Translating("toMenu");
        textPause.text = translator.Translating("pause");

        headerSettings.text = translator.Translating("settings");
        butSettings.text = translator.Translating("settings");
        sens.text = translator.Translating("sens");
        vol.text = translator.Translating("vol");
        back.text = translator.Translating("back");
    }

    public void Kill()
    {
        StartCoroutine(Messages("kill"));
    }

    private void SetWeaponImg()
    {
        if (StaticVal.inv[0] < 0)
        {
            icon1gun.color = new Color(255, 255, 255, 0);
        }
        else
        {
            icon1gun.color = new Color(255, 255, 255, 255);
            icon1gun.sprite = imgWeapon[StaticVal.inv[0]];
        }

        if (StaticVal.inv[1] < 0)
        {
            icon2gun.color = new Color(255, 255, 255, 0);
        }
        else
        {
            icon2gun.color = new Color(255, 255, 255, 255);
            icon2gun.sprite = imgWeapon[StaticVal.inv[1]];
        }
    }

    private void SetPositionIcon()
    {
        for (int i = 0; i < iconEnemy.Length; i++)
        {
            if (enemyTransform[i] == null)
            {
                iconEnemy[i].gameObject.SetActive(false);
            }
            else
            {
                iconEnemy[i].position = _cam.WorldToScreenPoint(enemyTransform[i].position);

                if (iconEnemy[i].localPosition.z < 0)
                {
                    iconEnemy[i].gameObject.SetActive(false);
                }
                else
                {
                    iconEnemy[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetAim(bool _isColseAim, bool _isOptical = false)
    {
        aim.SetActive(_isColseAim);
        opticsAim.SetActive(_isOptical);
    }

    public void Hiting()
    {
        if (_isHit == false)
        {
            StartCoroutine(Hited());
        }
    }

    IEnumerator Messages(string key)
    {
        textForPlayer.text = translator.Translating(key);
        textForPlayer.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= 100; i++)
        {
            textForPlayer.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Hited()
    {
        _isHit = true;
        hit.SetActive(true);
        yield return new WaitForSeconds(1f);
        hit.SetActive(false);
        _isHit = false;
    }

    public void WinGame()
    {
        textScores.text = StaticVal.moneyForBattle.ToString() + " $";
        textForWinOrFail.text = translator.Translating("win");
        _isWinOrFail = true;
        StartCoroutine(WinScreenOpen());
    }

    public void FailGame()
    {
        StaticVal.moneyForBattle = StaticVal.moneyForBattle / 2;
        textScores.text = StaticVal.moneyForBattle.ToString() + " $";
        _isWinOrFail = true;
        textForWinOrFail.text = translator.Translating("fail");
        StartCoroutine(WinScreenOpen());
    }

    IEnumerator WinScreenOpen()
    {
        _isWinOrFail = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;

        screenWin.gameObject.SetActive(true);
        for (int i = 0; i < 100; i++)
        {
            screenWin.color += new Color(0, 0, 0, 0.02f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        screenPause.SetActive(false);
        Time.timeScale = 1.0f;
        _isPause = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        screenPause.SetActive(true);
        Time.timeScale = 0.0f;
        _isPause = true;
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

    public void UpdateTextAmmos()
    {
        if (_player._flagGun)
        {
            sliderMedic.gameObject.SetActive(false);
            textAmmosInInventory.text = StaticVal.ammo.ToString();
            textAmmos.text = StaticVal.gun[StaticVal.inv[_player.NumberWeapon - 1]].currentAmmos.ToString();
            if (StaticVal.gun[StaticVal.inv[_player.NumberWeapon - 1]].currentAmmos == 0) textAmmos.color = Color.red;
            else textAmmos.color = Color.white;
        }
        else if (_player._flagMedic)
        {
            sliderMedic.gameObject.SetActive(true);
            textMedic.text = translator.Translating("medic");
            sliderMedic.value = 5f - _player.timer;
            textAmmosInInventory.text = "--";
            textAmmos.text = _player.numMedic.ToString();
        }
        else
        {
            sliderMedic.gameObject.SetActive(false);
            textAmmosInInventory.text = "--";
            textAmmos.text = "--";
        }
    }

    public void Exit()
    {
        _isPause = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StaticVal.money += StaticVal.moneyForBattle;
        StaticVal.moneyForBattle = 0;
        screenPause.SetActive(false);
        screenWin.color -= new Color(0, 0, 0, 1);
        screenWin.gameObject.SetActive(false);
        YandexGame.savesData.money = StaticVal.money;
        YandexGame.SaveProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
}
