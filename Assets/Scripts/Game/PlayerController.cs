using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    public GameObject ray;
    public Object boom;
    public GameObject boomParent;
    public GameObject rig;
    public GameObject pso;
    public GameObject[] guns;
    public GameManager manager;

    private float speed = 4;
    private float runSpeed = 7;
    private float gravity = -9.81f;
    public float hp = 100f;

    private CharacterController controller;
    private Animator anim;
    private CamController cam;
    private Camera camera;
    ////////-------------Удaлить для Яндекса---------------/////////
    private AudioSource audioPl;
    ////-----------------Удалить для яндекса-----------------///////

    private int gun = 1;
    private bool flagGun = true;
    private bool isReload = false;
    private bool onPricel = false;
    private float timeBtwShot;
    private float velocety;
    private Vector3 moveDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<CamController>();
        anim = GetComponentInChildren<Animator>();
        camera = GetComponentInChildren<Camera>();
        //------------------------------------------------
        audioPl = GetComponentInChildren<AudioSource>();
        //------------------------------------------------
    }

    private void FixedUpdate()
    {
        if (controller.isGrounded && velocety < 0) velocety = -2;

        if ((moveDirection.x != 0 || moveDirection.z != 0) && Input.GetKey(KeyCode.LeftShift))
        {
            Move(moveDirection, runSpeed);
        }
        else if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Move(moveDirection, speed);
        }

        DoGravity();
    }

    private void Update()
    {
        // Прицеливание
        if (Input.GetMouseButtonDown(1))
        {
            onPricel = true;
            manager.pricel.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            onPricel = false;
            manager.pricel.SetActive(true);
        }

        // Анимация если оружие просто и прицеле
        SetGun();
        if (flagGun)
        {
            if (onPricel && !StaticVal.gun[StaticVal.inv[gun - 1]].opticalPricel)
            {
                anim.SetBool("onGunOn", true);
                anim.SetBool("onGun", false);
                rig.SetActive(true);
                pso.SetActive(false);
                camera.fieldOfView = 60f;
                Close(StaticVal.inv[gun - 1]);
            }
            else if (onPricel && StaticVal.gun[StaticVal.inv[gun - 1]].opticalPricel)
            {
                Close(100);
                rig.SetActive(false);
                camera.fieldOfView = 30f;
                pso.SetActive(true);
            }
            else
            {
                anim.SetBool("onGunOn", false);
                anim.SetBool("onGun", true);
                rig.SetActive(true);
                pso.SetActive(false);
                camera.fieldOfView = 60f;
                Close(StaticVal.inv[gun - 1]);
            }
        }
        else
        {
            rig.SetActive(true);
            pso.SetActive(false);
            camera.fieldOfView = 60f;
            if (onPricel)
            {
                anim.SetBool("onGunOn", false);
                anim.SetBool("onGun", false);
            }
            else
            {
                anim.SetBool("onGunOn", false);
                anim.SetBool("onGun", false);
            }
            Close(100);
        }

        // Выстрел
        if (timeBtwShot <= 0)
        {
            if (Input.GetMouseButton(0) && flagGun == true && StaticVal.inv[gun - 1] >= 0 && StaticVal.gun[StaticVal.inv[gun - 1]].currentAmmos >= 1 && !cam.isPause && !manager.isWin)
            {
                if (onPricel) anim.SetTrigger("isShotOn");
                else anim.SetTrigger("isShot");
                timeBtwShot = StaticVal.gun[StaticVal.inv[gun - 1]].startTimeBtwShot;
                if (StaticVal.gun[StaticVal.inv[gun - 1]].Shot(Random.Range(7, 14), ray, LayerMask.GetMask("Ray"), "Enemy", true))
                {
                    manager.ISShot.SetActive(true);
                    Invoke("IconShotClose", 1f);
                }
                cam.UpRot(StaticVal.gun[StaticVal.inv[gun - 1]].angelVertical);
                audioPl.clip = manager.shooting;
                audioPl.Play();
            }
        }
        else
        {
            timeBtwShot -= Time.deltaTime;
        }

        // Перезарядка
        if (Input.GetKeyDown(KeyCode.R) && StaticVal.ammo >= 1 && StaticVal.gun[StaticVal.inv[gun - 1]].currentAmmos != StaticVal.gun[StaticVal.inv[gun - 1]].ammo && isReload == false && flagGun && !cam.isPause && !manager.isWin)
        {
            isReload = true;
            audioPl.clip = manager.reloading;
            audioPl.Play();
            Invoke("Reload", 1f);
        }

        // Пишем количество потронов\объём обоймы\потронов в обойме
        if (StaticVal.inv[gun - 1] >= 0 && flagGun) manager.txtAmmo.text = StaticVal.ammo.ToString() + " / " + StaticVal.gun[StaticVal.inv[gun - 1]].ammo.ToString() + " / " + StaticVal.gun[StaticVal.inv[gun - 1]].currentAmmos.ToString();
        else manager.txtAmmo.text = "-- / -- / --";

        moveDirection = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        // Проверка на близость врага
        RaycastHit[] hit = Physics.CapsuleCastAll(transform.position, transform.position, 25f, new Vector3(3f, 3f, 3f));
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Enemy"))
            {
                hit[i].collider.gameObject.GetComponent<EnemyAI>().onPlayer = true;
            }
        }

        hit = Physics.CapsuleCastAll(transform.position, transform.position, 1f, new Vector3(3f, 3f, 3f));
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Enemy"))
            {
                // Создаём взрыв на месте врага и уничтожаем его
                hp -= 7f;
                Instantiate(boom, boomParent.transform).GetComponent<Transform>().position = hit[i].collider.transform.position;
                Destroy(hit[i].collider.gameObject);
            }
            if (hit[i].collider.CompareTag("Apt"))
            {
                if (hp + 50 >= 100) hp = 100;
                else hp += 50;
                Destroy(hit[i].collider.gameObject);
            }
            if (hit[i].collider.CompareTag("Pot"))
            {
                StaticVal.ammo += 64 + (StaticVal.levlEnemy * 8);
                Destroy(hit[i].collider.gameObject);
            }
        }
    }    
    
    private void Move(Vector3 direction, float speedObj)
    {
        controller.Move(((transform.right * (direction.z * speedObj)) + (transform.forward * (direction.x * speedObj))) * Time.fixedDeltaTime);
    }
    private void DoGravity()
    {
        velocety = gravity * Time.fixedDeltaTime;
        controller.Move(transform.up * velocety);
    }

    public void SetGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))     //при нажетии на кнопку оружия 1
        {
            if (gun != 1) gun = 1;
            else if (gun == 1 && flagGun == true) flagGun = false;
            else if (gun == 1 && flagGun == false) flagGun = true;
            if (StaticVal.inv[0] < 0) flagGun = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))        //при нажетии на кнопку оружия 2
        {
            if (gun != 2) gun = 2;
            else if (gun == 2 && flagGun == true) flagGun = false;
            else if (gun == 2 && flagGun == false) flagGun = true;
            if (StaticVal.inv[1] < 0) flagGun = false;
        }
    }
    private void Reload()
    {
        StaticVal.ammo = StaticVal.gun[StaticVal.inv[gun - 1]].Reload(StaticVal.ammo);
        isReload = false;
    }
    private void Close(int _id)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (i == _id) guns[i].SetActive(true);
            else guns[i].SetActive(false);
        }
    }
    private void IconShotClose()
    {
        manager.ISShot.SetActive(false);
    }
    public void TakeDamage(float dm)
    {
        hp -= dm;
    }
}
