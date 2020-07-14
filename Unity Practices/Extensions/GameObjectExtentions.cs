using System;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace Extentions
{
    static class GameObjectExtentions
    {
        public static void HandleComponent<T>(this GameObject gameObject, Action<T> handler)
        {
            var component = gameObject.GetComponent<T>();
            if (component != null)
                handler?.Invoke(component);
        }

        public static bool ContainsComponent<T>(this GameObject gameObject)
        {
            return gameObject.GetComponent<T>() != null ? true : false;
        }

        public static void HandleComponent<T>(this Transform transform, Action<T> handler)
        {
            var component = transform.GetComponent<T>();
            if (component != null)
                handler?.Invoke(component);
        }

        public static bool ContainsComponent<T>(this Transform transform)
        {
            return transform.GetComponent<T>() != null ? true : false;
        }

        public static bool ContainsDelegate(this Delegate[] delegates, Delegate @delegate)
        {
            if (delegates.Contains(@delegate))
            {
                return true;
            }

            return false;
        }

        public static bool IsPartOfPlayer(this GameObject gameObject, out GameObject playerObject)
        {
            Transform Object = gameObject.transform;

            while (Object.parent != null)
            {
                if (!Object.ContainsComponent<Player>())
                {
                    playerObject = Object.gameObject;
                    return true;
                }

                Object = Object.parent;
            }

            if (!Object.ContainsComponent<Player>())
            {
                playerObject = Object.gameObject;
                return true;
            }

            playerObject = null;
            return false;
        }

        public static bool IsPartOfPlayer(this Transform gameObject, out Transform playerTransform)
        {
            Transform Object = gameObject;

            while (Object.parent != null)
            {
                if (!Object.ContainsComponent<Player>())
                {
                    playerTransform = Object;
                    return true;
                }

                Object = Object.parent;
            }

            if (!Object.ContainsComponent<Player>())
            {
                playerTransform = Object;
                return true;
            }


            playerTransform = null;
            return false;
        }

        public static void SetAnimationOffset(this Animator animator, out Coroutine coroutine, float seconds = 0f)
        {
            if (seconds == 0f)
            {
                seconds = UnityEngine.Random.Range(0f, 1f);
            }

            UnityEngine.Object.FindObjectOfType<GameHandlerForManagers>().StartCoroutine(AnimationOffset(animator, seconds), out coroutine);
        }

        private static IEnumerator AnimationOffset(Animator animator, float value)
        {
            animator.enabled = false;
            yield return new WaitForSeconds(value);
            animator.enabled = true;
        }

        public static uint Concat(this uint defaultNumber, uint NumberToConcat, bool ConcatToEnd)
        {
            if (ConcatToEnd)
            {
                return Concat(defaultNumber, NumberToConcat);
            }
            else
            {
                return Concat(NumberToConcat, defaultNumber);
            }
        }

        private static uint Concat(uint a, uint b)
        {
            uint pow = 1;

            while (pow < b)
            {
                pow = ((pow << 2) + pow) << 1;
            }

            return a * pow + b;
        }
    }
}
