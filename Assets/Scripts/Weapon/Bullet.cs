using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float startVelocety = 200f;
    [SerializeField] private LayerMask maskWall, maskPlayer, maskEnemy;
    public float damage = 10f;
    public Vector3 point;
    private Rigidbody rb;
    private float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 vec = point - transform.position;
        transform.rotation = Quaternion.LookRotation(-vec);
        rb.AddForce(-transform.forward * startVelocety);

        timer = lifeTime;
    }

    private void Update()
    {
        bool l = Physics.CheckBox(transform.position, new Vector3(0.075f, 0.075f, 1f), transform.rotation, LayerMask.GetMask("Ray", "Player", "Enemy"));
        timer -= Time.deltaTime;
        if (timer < 0 || l)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(0.075f, 0.075f, 1f));
    }

    /*
    private void Update()
    {
        RaycastHit[] onWall = Physics.RaycastAll(transform.position, transform.forward, 1f, maskWall);
        RaycastHit[] onPlayer = Physics.RaycastAll(transform.position, transform.forward, 1f, maskPlayer);
        RaycastHit[] onEnemy = Physics.RaycastAll(transform.position, transform.forward, 1f, maskEnemy);

        if (onWall.Length > 0)
        {
            Destroy(gameObject);
        }
        else if (onEnemy.Length > 0)
        {
            onEnemy[0].collider.gameObject.GetComponent<EnemyAI>().TakeDamage(50f);
            Destroy(gameObject);
        }
        else if (onPlayer.Length > 0)
        {
            //player.TakeDamage(10f);
            Destroy(gameObject);
        }
    }
    */
}
