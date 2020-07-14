using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extentions;

[RequireComponent(typeof(BoxCollider2D))]
public class MeleeWeapon : Weapon
{
    [Header("melee weapon settings")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _power;

    public override void Aim() { }

    public override void Reload() { }

    public override void StartShoot() { }

    public override void StopShoot() { }
}
