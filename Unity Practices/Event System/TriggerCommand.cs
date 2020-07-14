using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCommand : SendGameCommand
{
    protected void Reset()
    {
        var collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
            collider2D.isTrigger = true;
    }
}
