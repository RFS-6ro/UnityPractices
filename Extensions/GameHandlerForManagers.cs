using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandlerForManagers : MonoBehaviour
{
    private List<Coroutine> coroutines;

    public void StartCoroutine(IEnumerator routine, out Coroutine coroutine)
    {
        coroutine = StartCoroutine(routine);

        if (coroutines == null)
        {
            coroutines = new List<Coroutine>();
        }

        coroutines.Add(coroutine);
    }

    public new bool StopCoroutine(Coroutine coroutine)
    {
        if (coroutines != null)
        {
            if (coroutines.Contains(coroutine))
            {
                coroutines.Remove(coroutine);
                StopCoroutine(coroutine);

                return true;
            }
        }

        return false;
    }
}
