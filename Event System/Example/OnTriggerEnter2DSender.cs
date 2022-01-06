using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extentions;

[RequireComponent(typeof(Collider2D))]
public class OnTriggerEnter2DSender : TriggerCommand
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>() != null)
        {
            if (isTriggered && oneShot)
                return;
            Send();
        }
    }
}
