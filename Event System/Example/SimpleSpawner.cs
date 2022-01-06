using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleSpawner : GameCommandHandler
{
    public bool activate = false;
    public SendGameCommand OnStartCommand, OnStopCommand;

    [SerializeField] protected GameObject _objectToSpawn;

    [SerializeField] protected Vector3 _spawnPosition;
    [SerializeField] protected Quaternion _spawnRotation;
    
    [SerializeField] protected float _timeBetweenShots = 0f;
    
    public override void PerformInteraction()
    {
        activate = true;
        if (OnStartCommand != null) OnStartCommand.Send();
        
        SpawnOnce();
    }

    private void SpawnOnce()
    {
        if (_objectToSpawn != null)
            Instantiate(_objectToSpawn, _spawnPosition, _spawnRotation);
    }
}
