using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Adv : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void ShowAdv();

    [DllImport("__Internal")]
    public static extern void ShowReward();

    // Fullscreen
    public void OnOpen()
    {
        AudioListener.volume = 0;
        Time.timeScale = 0;
        StaticVal.onAd = true;
    }

    public void OnClose()
    {
        AudioListener.volume = 1;
        Time.timeScale = 1;
        StaticVal.onAd = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void OnError()
    {
        OnClose();
    }

    public void OnOffline()
    {
        OnClose();
    }

    // Reward
    public void OnOpenReward()
    {
        AudioListener.volume = 0;
    }

    public void OnRewarded()
    {
        StaticVal.money += 50;
        PlayerPrefs.SetInt("money", StaticVal.money);
    }

    public void OnCloseReward()
    {
        AudioListener.volume = 1;
    }

    public void OnErrorReward()
    {
        OnCloseReward();
    }
}
