using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour 
{
    public Transform player;
    public Transform point;
    public GameManager manager;

    private NavMeshAgent agent;
    private Animator anim;

    private float hp = 100;
    public bool onPlayer = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (hp <= 0) 
        {
            agent.isStopped = true;
            StaticVal.moneyForBattle += 70;
            Destroy(gameObject);
        }
        if (hp < 100) onPlayer = true;

        if (!onPlayer) AttackingPoint(3.9f);
        else AttackingPlayer(StaticVal.speedEnemy[StaticVal.levlEnemy]);

        if (agent.velocity != Vector3.zero) anim.SetBool("isRun", true);
        else anim.SetBool("isRun", false);
    }

    private void AttackingPoint(float _speed = 1f)
    {
        agent.speed = _speed;
        agent.destination = point.position;
    }

    private void AttackingPlayer(float _speed = 1f)
    {
        agent.speed = _speed;
        agent.SetDestination(player.position);
    }

    public void TakeDamage(float dm)
    {
        hp -= dm;
    }
}
