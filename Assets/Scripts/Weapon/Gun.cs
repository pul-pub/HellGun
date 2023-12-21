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

    public int currentAmmos = 0;

    public Gun(string name, int moneys,  float dm, int ammo, float startTimeBtwShot, bool opticalPricel, float angelVertical, int currentAmmos)
    {
        this.name = name;
        this.moneys = moneys;
        this.dm = dm;
        this.ammo = ammo;
        this.startTimeBtwShot = startTimeBtwShot;
        this.opticalPricel = opticalPricel;
        this.angelVertical = angelVertical;
        this.currentAmmos = currentAmmos;
    }

    public bool Shot(float _damage, GameObject _pointStartRaycast, LayerMask _maskRaycast, string _tag, bool _enemy)
    {
        currentAmmos--;
        RaycastHit[] hitInfo = Physics.RaycastAll(_pointStartRaycast.transform.position, _pointStartRaycast.transform.forward, 750f, _maskRaycast);
        if (hitInfo.Length != 0)
        {
            if (hitInfo[0].collider.CompareTag(_tag))
            {
                if (_enemy) hitInfo[0].collider.GetComponent<EnemyAI>().TakeDamage(_damage);
                else hitInfo[0].collider.GetComponent<Player>().TakeDamage(_damage);
                return true;
            }
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
