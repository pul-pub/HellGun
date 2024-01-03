using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    [Header("Weapon")]
    [SerializeField] private GameObject[] pullWeapons;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject pointStartRaycast;
    [SerializeField] private Transform parent;
    [SerializeField] private Object bullet;
    [Header("Body")]
    [SerializeField] private GameObject hands;
    [Header("Audio")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private CharacterController _controller;
    private PlayerInterface _playerInterface;
    private Camera _camera;
    private CameraContoller _cameraContoller;
    private ParticleSystem[] _particleSystems;
    private AudioSource _audioSource;
    private Animator _anim;
    private float _velocety;
    private Vector3 _moveDirection;
    private int _numGun;
    private float _timeBtwShot = 1f;
    private bool _flagGun = true;
    private bool _isReload = false;
    private bool _onAim = false;
    private float _hp = 100;

    private readonly float _gravity = -9.81f;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _controller = GetComponent<CharacterController>();
        _playerInterface = GetComponent<PlayerInterface>();
        _anim = hands.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main;
        _cameraContoller = GetComponentInChildren<CameraContoller>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _numGun = 1;
        _flagGun = false;
        ClosePullGun(StaticVal.inv[_numGun - 1]);
    }

    void Update()
    {
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
                if (StaticVal.gun[StaticVal.inv[_numGun - 1]].Shoot(bullet, parent, Random.Range(7, 14), pointStartRaycast, LayerMask.GetMask("Enemy"),
                    "Enemy", true))
                {
                    _playerInterface.Hiting();
                }
                
                _cameraContoller.Recoil(StaticVal.gun[StaticVal.inv[_numGun - 1]].angelVertical);
                _audioSource.clip = shootClip;
                _audioSource.Play();
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
            _audioSource.clip = reloadClip;
            _audioSource.Play();
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jamp(200));
        }
    }

    private void FixedUpdate()
    {
        if (_controller.isGrounded && _velocety < 0)
        {
            _velocety = -2;
        }

        if ((_moveDirection.x != 0 || _moveDirection.z != 0) && Input.GetKey(KeyCode.LeftShift))
        {
            Move(_moveDirection, speedRun);
        }
        else if (_moveDirection.x != 0 || _moveDirection.z != 0)
        {
            Move(_moveDirection, speed);
        }

        DoGravity();
    }

    IEnumerator Jamp(float _force)
    {
        for (int i = 0; i < 30; i++)
        {
            _controller.Move(transform.up * (_force / 30) * Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Move(Vector3 _direction, float _speedObj)
    {
        _controller.Move(((transform.right * (_direction.z * _speedObj)) + (transform.forward * (_direction.x * _speedObj))) * Time.fixedDeltaTime);
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
            else if (_numGun == 1 && _flagGun == true)
            {
                _flagGun = false;
            }
            else if (_numGun == 1 && _flagGun == false)
            {
                _flagGun = true;
            }
            if (StaticVal.inv[0] < 0)
            {
                _flagGun = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_numGun != 2)
            {
                _numGun = 2;
                _flagGun = true;
            }
            else if (_numGun == 2 && _flagGun == true)
            {
                _flagGun = false;
            }
            else if (_numGun == 2 && _flagGun == false)
            {
                _flagGun = true;
            }
            if (StaticVal.inv[1] < 0)
            {
                _flagGun = false;
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
