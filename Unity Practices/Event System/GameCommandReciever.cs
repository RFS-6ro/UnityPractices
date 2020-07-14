using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommandReciever : MonoBehaviour
{
    private Dictionary<TriggerCommandsType, List<Action>> _handlers = new Dictionary<TriggerCommandsType, List<Action>>();

    public void Recieve(TriggerCommandsType command)
    {
        List<Action> callbacks = null;
        if (_handlers.TryGetValue(command, out callbacks))
        {
            foreach (var methodToInvoke in callbacks)
            {
                methodToInvoke();
            }
        }
    }

    public void Register(TriggerCommandsType command, GameCommandHandler handler)
    {
        List<Action> callbacks = null;
        if (!_handlers.TryGetValue(command, out callbacks))
        {
            callbacks = _handlers[command] = new List<Action>();
        }
        callbacks.Add(handler.OnInteraction);
    }

    public void Remove(TriggerCommandsType command, GameCommandHandler handler)
    {
        _handlers[command].Remove(handler.OnInteraction);
    }
}
