using UnityEngine.UI;
using UnityEngine;

public enum WhoCapturingPoint { Enemy, Player, Null };

public class Point : MonoBehaviour
{
    public WhoCapturingPoint[] capturePoints = { WhoCapturingPoint.Null };
    public float time = 30f;
    public Color color = Color.white;

    private bool _onPoint = false;
    private bool _player = false;
    private bool _enemy = false;
    private float _startTime = 30f;
    private WhoCapturingPoint _who = WhoCapturingPoint.Null;
    private WhoCapturingPoint _capturing = WhoCapturingPoint.Null;

    private void Update()
    {
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(10.5f, 0.5f, 10.5f));
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.CompareTag("Pl"))
            {
                if (_capturing != WhoCapturingPoint.Player && !_enemy) _capturing = WhoCapturingPoint.Player;
                //kto = 'p';
                color = Color.blue;
                _player = true;

                continue;
            }
            else if (col[i].gameObject.CompareTag("Enemy"))
            {
                if (_capturing != WhoCapturingPoint.Enemy && !_player) _capturing = WhoCapturingPoint.Enemy;
                //kto = 'e';
                color = Color.red;
                _enemy = true;
                continue;
            }
        }

        if (_player && _enemy)
        {
            _onPoint = false;
            color = Color.yellow;
        }
        else if (_player || _enemy)
        {
            _onPoint = true;
            if (_capturing != _who)
            {
                time = _startTime;
            }
            if (_enemy) _who = WhoCapturingPoint.Enemy;
            else if (_player) _who = WhoCapturingPoint.Player;
        }
        else
        {
            color = Color.white;
            _onPoint = false;
            _who = WhoCapturingPoint.Null;
            time = _startTime;
        }

        _player = false;
        _enemy = false;

        if (_onPoint)
        {
            if (capturePoints[0] == WhoCapturingPoint.Null)
            {
                if (time <= 0)
                {
                    time = _startTime;
                    capturePoints[0] = _who;
                    if (_who == WhoCapturingPoint.Player) GetComponent<Image>().color = Color.blue;
                    else GetComponent<Image>().color = Color.red;
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
            else if (capturePoints[0] == WhoCapturingPoint.Enemy && _who == WhoCapturingPoint.Player)
            {
                if (time <= 0)
                {
                    time = _startTime;
                    capturePoints[0] = _who;
                    GetComponent<Image>().color = Color.blue;
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
            else if (capturePoints[0] == WhoCapturingPoint.Player && _who == WhoCapturingPoint.Enemy)
            {
                if (time <= 0)
                {
                    time = _startTime;
                    capturePoints[0] = _who;
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
