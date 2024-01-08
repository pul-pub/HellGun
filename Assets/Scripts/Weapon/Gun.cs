using UnityEngine;


public class Gun
{
    public string name;
    public int moneys;

    public float dm;
    public int ammo;
    public float startTimeBtwShot;
    public bool opticalPricel;
    public float angelVertical;
    public Vector3 posHendsOnAim;

    public int currentAmmos = 0;

    public Gun(string name, int moneys,  float dm, int ammo, float startTimeBtwShot, bool opticalPricel, float angelVertical, int currentAmmos, Vector3 posHendsOnAim)
    {
        this.name = name;
        this.moneys = moneys;
        this.dm = dm;
        this.ammo = ammo;
        this.startTimeBtwShot = startTimeBtwShot;
        this.opticalPricel = opticalPricel;
        this.angelVertical = angelVertical;
        this.currentAmmos = currentAmmos;
        this.posHendsOnAim = posHendsOnAim;
    }

    public bool Shoot(UnityEngine.Object bullet, Transform parent, GameObject _pointStartRaycast, GameObject _pointStartRaycast2,
        TypeBullet _type)
    {
        currentAmmos--;
        Ray rayE = new Ray(_pointStartRaycast.transform.position, -_pointStartRaycast2.transform.right.normalized);
        Ray rayP = new Ray(_pointStartRaycast.transform.position, -_pointStartRaycast2.transform.right.normalized);

        if (_type == TypeBullet.Enemy && Physics.Raycast(rayE, out RaycastHit hitInfoE, 700f, LayerMask.GetMask("Enemy", "Ray")))
        {
            Collider col = hitInfoE.collider;

            GameObject obj = UnityEngine.Object.Instantiate(bullet, _pointStartRaycast.transform.position, _pointStartRaycast.transform.rotation, parent) as GameObject;
            obj.GetComponent<Bullet>().point = hitInfoE.point;

            if (col.GetComponents<EnemyAI>().Length > 0)
            {
                col.GetComponent<EnemyAI>().TakeDamage(dm);
                return true;
            }
        }
        else if (_type == TypeBullet.Player && Physics.Raycast(rayP, out RaycastHit hitInfoP, 700f, LayerMask.GetMask("Player", "Ray")))
        {
            Collider col = hitInfoP.collider;

            GameObject obj = UnityEngine.Object.Instantiate(bullet, _pointStartRaycast.transform.position, _pointStartRaycast.transform.rotation, parent) as GameObject;
            obj.GetComponent<Bullet>().point = hitInfoP.point;

            if (col.GetComponents<Player>().Length > 0)
            {
                col.GetComponent<Player>().TakeDamage(dm);
                return true;
            }
        }

        return false;

        /*
        RaycastHit[] hitInfo = Physics.RaycastAll(_pointStartRaycast2.transform.position, -_pointStartRaycast2.transform.right, 750f, _maskRaycast);

        GameObject obj = UnityEngine.Object.Instantiate(bullet, _pointStartRaycast.transform.position, _pointStartRaycast.transform.rotation, parent) as GameObject;
        obj.GetComponent<Bullet>().damage = _damage;
        if (hitInfo.Length > 0) obj.GetComponent<Bullet>().point = hitInfo[0].point;
        else obj.GetComponent<Bullet>().point = -_pointStartRaycast.transform.right.normalized * 700;
        if (hitInfo.Length != 0)
        {
            if (_enemy && hitInfo[0].collider.GetComponents<EnemyAI>().Length > 0)
            {
                hitInfo[0].collider.GetComponent<EnemyAI>().TakeDamage(_damage);
                return true;
            }
            else if (!_enemy && hitInfo[0].collider.GetComponents<Player>().Length > 0)
            {
                hitInfo[0].collider.GetComponent<Player>().TakeDamage(_damage);
            }
        }
        return false;
        */
    }

    public int Reload(int _ammos)
    {
        int reason = ammo - currentAmmos;
        int returnAmmos = _ammos;
        if (returnAmmos >= reason)
        {
            returnAmmos -= reason;
            currentAmmos = currentAmmos + reason;
        }
        else
        {
            currentAmmos = currentAmmos + returnAmmos;
            returnAmmos = 0;
        }
        return returnAmmos;
    }

    private Vector3 CalcucateRandom()
    {
        return new Vector3
        (
            Random.Range(-2, 2),
            Random.Range(-2, 2),
            Random.Range(-2, 2)
        );
    }
}
