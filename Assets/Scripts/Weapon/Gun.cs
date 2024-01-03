using System;
using UnityEngine;

[Serializable]
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

    public bool Shoot(UnityEngine.Object bullet, Transform parent, float _damage, GameObject _pointStartRaycast, LayerMask _maskRaycast, string _tag, bool _enemy)
    {
        currentAmmos--;
        RaycastHit[] hitInfo = Physics.RaycastAll(_pointStartRaycast.transform.position, -_pointStartRaycast.transform.right, 750f, _maskRaycast);
        RaycastHit[] hitWithBullet = Physics.RaycastAll(_pointStartRaycast.transform.position, -_pointStartRaycast.transform.right, 750f);
        GameObject obj = UnityEngine.Object.Instantiate(bullet, _pointStartRaycast.transform.position, _pointStartRaycast.transform.rotation, parent) as GameObject;
        obj.GetComponent<Bullet>().damage = _damage;
        if (hitWithBullet.Length > 0) obj.GetComponent<Bullet>().point = hitWithBullet[0].point;
        else obj.GetComponent<Bullet>().point = -_pointStartRaycast.transform.right.normalized * 700;
        if (hitInfo.Length != 0)
        {
            if (_enemy) hitInfo[0].collider.GetComponent<EnemyAI>().TakeDamage(_damage);
            else hitInfo[0].collider.GetComponent<Player>().TakeDamage(_damage);
            return true;
        }
        return false;
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
}
