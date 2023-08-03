using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : ThunderStrike_Controller
{
    public void Start()
    {
        Destroy(gameObject, 2);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
