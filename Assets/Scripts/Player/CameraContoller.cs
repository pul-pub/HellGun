using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float _mouseX;
    private float _mouseY;

    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * StaticVal.sens * Time.deltaTime;
        _mouseY += Input.GetAxis("Mouse Y") * StaticVal.sens * Time.deltaTime;

        player.Rotate(_mouseX * new Vector3(0, 1, 0));
        _mouseY = Mathf.Clamp(_mouseY, -90, 90);
        transform.localEulerAngles = new Vector3(-_mouseY, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public void Recoil(float _angel)
    {
        _mouseY += _angel;
    }
}
