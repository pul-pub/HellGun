using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInterface : MonoBehaviour
{
    [Header("GUI")]
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject opticsAim;
    [SerializeField] private GameObject hit;
    [SerializeField] private Slider progressBarPoint;
    [SerializeField] private Image bgPoint;
    [SerializeField] private Image imgPoint;
    [SerializeField] private Point point;
    [SerializeField] private Slider progressBarHealth;
    [Header("Enemy and Icon")]
    [SerializeField] private RectTransform[] iconEnemy;
    [SerializeField] private Transform[] enemyTransform;
    [Header("Weapons/Ammos for it")]
    [SerializeField] private TextMeshProUGUI textAmmosInInventory;
    [SerializeField] private TextMeshProUGUI textAmmos;
    [Header("Screens")]
    [SerializeField] private GameObject screenPause;
    [SerializeField] private Image screenWin;
    [SerializeField] private TextMeshProUGUI textForWinOrFail;
    [SerializeField] private TextMeshProUGUI textScores;

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
    }

    void Update()
    {
        SetPositionIcon();
        UpdateTextAmmos();
        progressBarHealth.value = _player.Health;

        if (Input.GetKeyDown(KeyCode.Tab) && !_isWinOrFail)
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

        if (bgPoint.color != point.color)
        {
            bgPoint.color = point.color;
            if (point.capturePoints[0] == WhoCapturingPoint.Enemy && imgPoint.color != Color.red)
            {
                imgPoint.color = Color.red;
            }
            else if (point.capturePoints[0] == WhoCapturingPoint.Player && imgPoint.color != Color.blue)
            {
                imgPoint.color = Color.blue;
            }
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

        if (StaticVal.scoreWithPoint == 10)
        {
            WinGame();
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
        if (StaticVal.language == "ru")
        {
            textForWinOrFail.text = "Миссия завершена.";
        }
        else
        {
            textForWinOrFail.text = "Mission complete.";
        }
        StartCoroutine(WinScreenOpen());
    }

    public void FailGame()
    {
        textScores.text = (StaticVal.moneyForBattle / 2).ToString() + " $";
        if (StaticVal.language == "ru")
        {
            textForWinOrFail.text = "Миссия не завершена.";
        }
        else
        {
            textForWinOrFail.text = "Mission not complete.";
        }
        StartCoroutine(WinScreenOpen());
    }

    IEnumerator WinScreenOpen()
    {
        _isWinOrFail = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;

        screenWin.gameObject.SetActive(true);
        for (int i = 0; i < 256; i += 2)
        {
            screenWin.color += new Color(0, 0, 0, 0.001f);
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

    public void UpdateTextAmmos()
    {
        if (StaticVal.inv[_player.NumberWeapon - 1] >= 0)
        {
            textAmmosInInventory.text = StaticVal.ammo.ToString();
            textAmmos.text = StaticVal.gun[StaticVal.inv[_player.NumberWeapon - 1]].currentAmmos.ToString();
            if (StaticVal.gun[StaticVal.inv[_player.NumberWeapon - 1]].currentAmmos == 0) textAmmos.color = Color.red;
            else textAmmos.color = Color.white;
        }
        else
        {
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
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
