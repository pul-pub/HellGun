using UnityEngine;
using UnityEngine.AI;

public class Enemy
{
    private NavMeshAgent _agent;
    private Gun _gun;
    private float _speed;

    public Enemy(NavMeshAgent navMeshAgent, Gun gun, float speed)
    {
        _agent = navMeshAgent;
        _gun = gun;
        _speed = speed;
    }

    public void MoveTo(Vector3 _position)
    {
        _agent.speed = _speed;
        _agent.SetDestination(_position);
    }

    public Quaternion RotToObject(Vector3 _myPosition, Vector3 _objectPosition)
    {
        if (!_agent.Raycast(_objectPosition, out NavMeshHit hit))
        {
            Vector3 vec = _myPosition - _objectPosition;
            return Quaternion.LookRotation(-vec);
        }
        return new Quaternion(0, 0, 0, 0);
    }

    public void Shoot(GameObject _pointStartRaycast)
    {
        /*
        if (_gun.Shot(Random.Range(7, 14), _pointStartRaycast, LayerMask.GetMask("Ray"), "Pl", false))
        {
            Debug.Log("Ya!");
        }
        */
    }

}
