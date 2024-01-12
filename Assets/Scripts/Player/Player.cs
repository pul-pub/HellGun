using System.Collections;
using UnityEngine;

public enum TypeBullet { Enemy, Player };

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float forceJamp;
    [SerializeField] private float hightGroundCheck;
    [SerializeField] private LayerMask maskGround;
    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    [Header("Weapon")]
    [SerializeField] private GameObject[] pullWeapons;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject pointStartRaycast;
    [SerializeField] private GameObject pointStartRaycast2;
    [SerializeField] private Transform parent;
    [SerializeField] private Object bullet;
    [Header("Body")]
    [SerializeField] private GameObject hands;
    [Header("Audio")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private CharacterController _controller;
    private Rigidbody rb;
    private PlayerInterface _playerInterface;
    private Camera _camera;
    private CameraContoller _cameraContoller;
    private ParticleSystem[] _particleSystems;
    //private AudioSource _audioSource;
    private Animator _anim;
    private float _velocety;
    private Vector3 _moveDirection;
    private int _numGun;
    private float _timeBtwShot = 1f;
    public bool _flagGun = true;
    public bool _flagMedic = false;
    public int numMedic = 5;
    private bool _isReload = false;
    private bool _onAim = false;
    private float _hp = 100;
    public float timer = 5f;
    private bool grounded;

    private readonly float _gravity = -9.81f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _controller = GetComponent<CharacterController>();
        _playerInterface = GetComponent<PlayerInterface>();
        _anim = hands.GetComponent<Animator>();
       // _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main;
        _cameraContoller = GetComponentInChildren<CameraContoller>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _numGun = 1;
        numMedic = 5;
        _flagMedic = false;
        _flagGun = true;
        ClosePullGun(StaticVal.inv[_numGun - 1]);
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, hightGroundCheck, maskGround);
        if (!_playerInterface._isPause && !_playerInterface._isWinOrFail)
        {
            if (Input.GetKey(KeyCode.Alpha3) && _hp < 100 && numMedic > 0)
            {
                _flagGun = false;
                _flagMedic = true;
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    _hp += 32;
                    if (_hp > 100)
                    {
                        _hp = 100;
                    }
                    timer = 5f;
                    _flagMedic = false;
                    numMedic--;
                }
            }
            else
            {
                timer = 5f;
            }

            _moveDirection = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

            if (Input.GetMouseButtonDown(1))
            {
                _onAim = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                _onAim = false;
            }

            SetGun();
            SetTypeAim();

            if (_timeBtwShot <= 0)
            {
                if (Input.GetMouseButton(0) && _flagGun == true &&
                    StaticVal.inv[_numGun - 1] >= 0 && StaticVal.gun[StaticVal.inv[_numGun - 1]].currentAmmos >= 1 && !_playerInterface._isPause &&
                    !_playerInterface._isWinOrFail)
                {
                    _particleSystems[Random.Range(0, _particleSystems.Length)].Play();
                    if (StaticVal.gun[StaticVal.inv[_numGun - 1]].Shoot(bullet, parent, pointStartRaycast, pointStartRaycast2, TypeBullet.Enemy))
                    {
                        _playerInterface.Hiting();
                    }

                    _cameraContoller.Recoil(StaticVal.gun[StaticVal.inv[_numGun - 1]].angelVertical);
                    //_audioSource.clip = shootClip;
                    //_audioSource.Play();
                    _timeBtwShot = StaticVal.gun[StaticVal.inv[_numGun - 1]].startTimeBtwShot;
                }
            }
            else if (_timeBtwShot > 0)
            {
                _timeBtwShot -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.R) && StaticVal.ammo >= 1 &&
                StaticVal.gun[StaticVal.inv[_numGun - 1]].currentAmmos != StaticVal.gun[StaticVal.inv[_numGun - 1]].ammo && _isReload == false && _flagGun &&
                !_playerInterface._isPause && !_playerInterface._isWinOrFail)
            {
                _isReload = true;
                _anim.SetTrigger("Reload");
                //_audioSource.clip = reloadClip;
                //_audioSource.Play();
                StartCoroutine(Reload());
            }
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                Jamp(forceJamp);
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, hightGroundCheck, maskGround);
        if (!_playerInterface._isPause && !_playerInterface._isWinOrFail)
        {
            /*
            if (_controller.isGrounded && _velocety < 0)
            {
                _velocety = -2;
            }
            */
            if ((_moveDirection.x != 0 || _moveDirection.z != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                Move(_moveDirection, speedRun);
            }
            else if (_moveDirection.x != 0 || _moveDirection.z != 0)
            {
                Move(_moveDirection, speed);
            }
            else if (_moveDirection.x == 0 && _moveDirection.z == 0 && grounded)
            {
                rb.velocity = Vector3.zero;
            }

            //DoGravity();
        }
    }

    private void Jamp(float _force)
    {
        rb.AddForce(transform.up * _force, ForceMode.Impulse);
    }

    private void Move(Vector3 _direction, float _speedObj)
    {
        //_controller.Move(((transform.right * (_direction.z * _speedObj)) + (transform.forward * (_direction.x * _speedObj))) * Time.fixedDeltaTime);      
        Vector3 vec = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(vec.magnitude > 10)
        {
            
        }
        else
        {
            rb.AddForce(((transform.right * (_direction.z * _speedObj)) + (transform.forward * (_direction.x * _speedObj))) * 10);
        }
    }

    private void DoGravity()
    {
        _velocety = _gravity * Time.fixedDeltaTime;
        _controller.Move(transform.up * _velocety);
    }

    private void SetGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_numGun != 1)
            {
                _numGun = 1;
                _flagGun = true;
            }
            if (StaticVal.inv[0] < 0)
            {
                _flagGun = false;
            }
            else
            {
                _flagGun = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_numGun != 2)
            {
                _numGun = 2;
                _flagGun = true;
            }
            if (StaticVal.inv[1] < 0)
            {
                _flagGun = false;
            }
            else
            {
                _flagGun = true;
            }
        }

        if (_flagGun)
        {
            _anim.SetBool("noGun", false);
        }
        else
        {
            _anim.SetBool("noGun", true);
        }
    }

    private void SetTypeAim()
    {
        if (_flagGun)
        {
            if (_onAim && !StaticVal.gun[StaticVal.inv[_numGun - 1]].opticalPricel)
            {
                hands.transform.localPosition = StaticVal.gun[StaticVal.inv[_numGun - 1]].posHendsOnAim;
                hands.SetActive(true);
                _playerInterface.SetAim(false);
                _camera.fieldOfView = 60f;
                ClosePullGun(StaticVal.inv[_numGun - 1]);
            }
            else if (_onAim && StaticVal.gun[StaticVal.inv[_numGun - 1]].opticalPricel)
            {
                hands.SetActive(false);
                _playerInterface.SetAim(false, true);
                _camera.fieldOfView = 30f;
                ClosePullGun(100);
            }
            else
            {
                hands.transform.localPosition = new Vector3(-0.2f, -1, 0);
                hands.SetActive(true);
                _playerInterface.SetAim(true);
                _camera.fieldOfView = 60f;
                ClosePullGun(StaticVal.inv[_numGun - 1]);
            }
        }
        else
        {
            hands.transform.localPosition = new Vector3(-0.2f, -1, 0);
            hands.SetActive(true);
            _playerInterface.SetAim(true, false);
            _camera.fieldOfView = 60f;
            ClosePullGun(100);
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2f);
        StaticVal.ammo = StaticVal.gun[StaticVal.inv[_numGun - 1]].Reload(StaticVal.ammo);
        _isReload = false;
        _playerInterface.UpdateTextAmmos();
    }

    private void ClosePullGun(int _idGun)
    {
        if (_idGun == 100)
        {
            store.SetActive(false);
        }
        else
        {
            store.SetActive(true);
        }

        for (int i = 0; i < pullWeapons.Length; i++)
        {
            if (i == _idGun)
            {
                pullWeapons[i].SetActive(true);
            }
            else
            {
                pullWeapons[i].SetActive(false);
            }
        }
    }

    public void TakeDamage(float _damage)
    {
        _hp -= _damage;
    }

    public float Health
    {
        get { return _hp; }
    }

    public int NumberWeapon
    {
        get { return _numGun; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pointStartRaycast.transform.position, -pointStartRaycast.transform.right.normalized * 700);
    }
}
