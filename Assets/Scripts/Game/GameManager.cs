using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Point")]
    public PontController pointA;
    [Header("Player")]
    public GameObject player;
    public TextMeshProUGUI scorePlayerText;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    private PlayerController playerController;
    private CamController camController;
    public int playerScore = 0;
    [Header("Enemy")]
    public Object enemy;
    public TextMeshProUGUI scoreEnemyText;
    public Transform enemyParent;
    public Vector3[] pos;
    private int enemyScore = 0;
    [Header("Other")]
    public Object apteca;
    public Object potrons;
    [Header("GUI")]
    public Slider progresBarPointA;
    public Image bgPointA;
    public Image imgPointA;
    public GameObject pricel;
    public GameObject ISShot;
    public GameObject winUI;
    public TextMeshProUGUI txtAmmo;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI winScore;
    public TextMeshProUGUI win;
    public bool isWin = false;
    [Header("Audio")]
    public AudioClip shooting;
    public AudioClip reloading;

    private float time = 1f;
    private float time2 = 20f;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        camController = player.GetComponentInChildren<CamController>();
        camController.gameManager = this;
        playerController.manager = this;
        SpawnEnemy((StaticVal.levlEnemy * 2) + 7);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camController.isPause = false;
        isWin = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (enemyParent.GetComponentsInChildren<EnemyAI>().Length == 0)
        {
            time2 -= Time.deltaTime;

            timer.gameObject.SetActive(true);
            if (StaticVal.language == "ru") timer.text = "До следующей волны: " + ((int)(time2)).ToString();
            else timer.text = "Until the next wave: " + ((int)(time2)).ToString();

            if (time2 < 0)
            {
                if (StaticVal.levlEnemy <= 9) StaticVal.levlEnemy++;
                SpawnEnemy((StaticVal.levlEnemy * 2) + 7);
                time2 = 20f;
            }
        }
        else timer.gameObject.SetActive(false);

        progresBarPointA.value = pointA.time / 100;
        bgPointA.color = pointA.color;
        if (pointA.capturePoints[0] == 'e') imgPointA.color = Color.red;
        else if (pointA.capturePoints[0] == 'p') imgPointA.color = Color.blue;
        else imgPointA.color = Color.white;

        time -= Time.deltaTime;
        if (pointA.capturePoints[0] != '0' && time <= 0)
        {
            time = 1;
            AddScore();
        }

        if ((playerController.hp <= 0 || playerScore >= 2000 || enemyScore >= 2000 || StaticVal.levlEnemy > 8) && !isWin)
        {
            camController.isPause = true;
            Cursor.lockState = CursorLockMode.None;
            winUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (playerController.hp <= 0 || enemyScore >= 2000) 
            {
                if (StaticVal.language == "ru") win.text = "Поражение.";
                else win.text = "Defeat.";
            }
            else
            {
                if (StaticVal.language == "ru") win.text = "Победа!";
                else win.text = "Win!";
            }
            Time.timeScale = 0.0f;
            if (StaticVal.language == "ru") winScore.text = "Ваша награда: " + ((playerScore - (playerScore % 5)) / 5).ToString() + "$";
            else winScore.text = "Your reward: " + ((playerScore - (playerScore % 5)) / 5).ToString() + "$";
            StaticVal.money += ((playerScore - (playerScore % 5)) / 5);
            PlayerPrefs.SetInt("money", StaticVal.money);
            isWin = true;
        }
        else
        {
            healthSlider.value = playerController.hp;
            healthText.text = playerController.hp.ToString();
        }
    }

    private void AddScore()
    {
        if (pointA.capturePoints[0] == 'p') playerScore++;
        else if (pointA.capturePoints[0] == 'e') enemyScore++;
        
        scorePlayerText.text = playerScore.ToString();
        scoreEnemyText.text = enemyScore.ToString();
    }
    private void SpawnEnemy(int _coutEnemy)
    {
        for (int i = 0; i < _coutEnemy; i++)
        {
            GameObject a = Instantiate(enemy, pos[Random.Range(0, pos.Length)], enemyParent.rotation, enemyParent) as GameObject;
            a.GetComponent<EnemyAI>().player = player.transform;
            a.GetComponent<EnemyAI>().point = pointA.transform;
            a.GetComponent<EnemyAI>().manager = this;
        }
    }
    public void Exit()
    {
        camController.isPause = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
}
