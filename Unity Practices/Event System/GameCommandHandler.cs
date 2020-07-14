using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extentions;
using System;

[SelectionBase]
[RequireComponent(typeof(GameCommandReciever))]
public abstract class GameCommandHandler : MonoBehaviour
{
    [SerializeField] protected SendGameCommand _sender;

    public TriggerCommandsType command;
    public bool isOneShot = false;
    public float cooldown = 0f;
    protected bool isTriggered = false;
    public float startTime = 0f;
    public float startDelay = 0f;

    protected virtual void Awake()
    {
        gameObject.HandleComponent<GameCommandReciever>((Component) => Component.Register(command, this));
    }

    private void Execute()
    {
        if (startDelay > 0)
        {
            StartCoroutine(Perform(startDelay, PerformInteraction));
        }
        else
        {
            PerformInteraction();
        }
    }

    public virtual void OnInteraction()
    {
        if (isOneShot && isTriggered) return;
        isTriggered = true;
        
        if (cooldown > 0)
        {
            if (Time.time > startTime + cooldown)
            {
                startTime = Time.time + startDelay;
                Execute();
            }
        }
        else
        {
            Execute();
        }
    }

    public abstract void PerformInteraction();

    protected IEnumerator Perform(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
