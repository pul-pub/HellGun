using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    void Start()
    {
        Invoke("Del", 3f);
    }

    private void Del()
    {
        Destroy(gameObject);
    }
}
