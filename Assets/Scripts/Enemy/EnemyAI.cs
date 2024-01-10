using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour 
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject pointStartRaycast;
    [SerializeField] private Transform parent;
    [SerializeField] private Object bullet;
    [SerializeField] private Transform player;
    [SerializeField] private Transform body;
    [SerializeField] private Transform hand;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    [SerializeField] private float health = 100;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    [SerializeField] private GameObject projectile;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private Animator _anim;
    private AudioSource _audioSource;
    private Gun _gun;
    private float _timeBtwShot = 0f;
    private bool isRelod = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _gun = new Gun("AKM", 0, 3f, 10, 0.15f, false, 1f, 10, new Vector3(-0.54f, -0.8f, 0));
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (!agent.isStopped)
        {
            _anim.SetBool("isRun", true);
        }
    }

    private void Patroling()
    {
        agent.isStopped = false;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;

        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.isStopped = true;

        //body.transform.LookAt(player.position - new Vector3(5, 3, 0));
        Vector3 vec = (player.position - transform.position);
        transform.rotation = Quaternion.LookRotation(vec.normalized);

        if (_timeBtwShot <= 0)
        {
            if (_gun.currentAmmos >= 1 && !isRelod)
            {
                _gun.Shoot(bullet, parent, pointStartRaycast, pointStartRaycast, TypeBullet.Player);

                _audioSource.Play();
                _timeBtwShot = _gun.startTimeBtwShot;
            }
            else
            {
                isRelod = true;
                Invoke("Reload", 2f);
            }
        }
        else if (_timeBtwShot > 0)
        {
            _timeBtwShot -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            player.GetComponent<PlayerInterface>().Kill();
            StaticVal.numEnemy--;
            StaticVal.moneyForBattle += 150;
            Destroy(gameObject);
        }
    }

    private void Reload()
    {
        _gun.Reload(100);
        isRelod = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
