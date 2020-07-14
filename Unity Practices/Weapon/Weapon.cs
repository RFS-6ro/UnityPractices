using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //settings
    [Header("weapon settings")]
    [SerializeField] protected float _shootCooldown;
    protected float _timeBetweenShots;

    //cache
    protected Transform _transform;
    protected Player _player;
    protected WeaponInputController _selector;

    protected virtual void Awake()
    {
        _player = FindObjectOfType<Player>();
        _selector = FindObjectOfType<WeaponInputController>();
        if (_player == null || _selector == null)
        {
            throw new System.Exception("null reference in weapon");
        }
        _timeBetweenShots = 0.0f;
        _transform = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        if (_timeBetweenShots > 0)
            _timeBetweenShots -= Time.deltaTime;
    }

    public abstract void StartShoot();

    public abstract void StopShoot();

    public abstract void Reload();

    public abstract void Aim();
}
