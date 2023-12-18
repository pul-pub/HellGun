using UnityEngine;
using UnityEngine.UI;

public class PontController : MonoBehaviour
{
    public float time = 30f;
    public Color color = Color.white;
    private bool onPoint = false;
    private bool player = false;
    private bool enemy = false;

    private float startTime = 30f;
    public char[] capturePoints = { '0' };
    private char kto = 'n';
    private char capturing = 'n';

    private void Update()
    {
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(10.5f, 0.5f, 10.5f));
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.CompareTag("Pl"))
            {
                if (capturing != 'p' && !enemy) capturing = 'p';
                //kto = 'p';
                color = Color.blue;
                player = true;
                
                continue;
            }
            else if (col[i].gameObject.CompareTag("Enemy"))
            {
                if (capturing != 'e' && !player) capturing = 'e';
                //kto = 'e';
                color = Color.red;
                enemy = true;
                continue;
            }
        }

        if (player && enemy)
        {
            onPoint = false;
            color = Color.yellow;
        }
        else if (player || enemy)
        {
            onPoint = true;
            if (capturing != kto)
            {
                time = startTime;
            }
            if (enemy) kto = 'e';
            else if (player) kto = 'p';
        }
        else
        {
            onPoint = false;
            kto = '0';
            time = startTime;
        }

        player = false;
        enemy = false;

        if (onPoint)
        {
            if (capturePoints[0] == '0')
            {
                if (time <= 0)
                {
                    time = startTime;
                    capturePoints[0] = kto;
                    if (kto == 'p') GetComponent<Image>().color = Color.blue;
                    else GetComponent<Image>().color = Color.red;
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
            else if (capturePoints[0] == 'e' && kto == 'p')
            {
                if (time <= 0)
                {
                    time = startTime;
                    capturePoints[0] = kto;
                    GetComponent<Image>().color = Color.blue;
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
            else if (capturePoints[0] == 'p' && kto == 'e')
            {
                if (time <= 0)
                {
                    time = startTime;
                    capturePoints[0] = kto;
                    GetComponent<Image>().color = Color.red;
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }
    }
}
