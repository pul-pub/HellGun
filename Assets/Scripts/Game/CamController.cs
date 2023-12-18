using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField]
    private GameObject pausUI;
    private float mouseX;
    private float mouseY;
    public bool isPause = false;
    public GameManager gameManager;

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * StaticVal.sens * Time.deltaTime;
        mouseY += Input.GetAxis("Mouse Y") * StaticVal.sens * Time.deltaTime;

        player.Rotate(mouseX * new Vector3(0, 1, 0));
        mouseY = Mathf.Clamp(mouseY, -90, 90);
        transform.localEulerAngles = new Vector3(-mouseY, transform.localEulerAngles.y, transform.localEulerAngles.z);

        if (Input.GetKeyDown(KeyCode.Tab) && !gameManager.isWin)
        {
            if (isPause) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPause = false;
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPause = true;
    }

    public void UpRot(float _scatter)
    {
        mouseY += _scatter;
    }
}
