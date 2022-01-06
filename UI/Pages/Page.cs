using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameCore.UI
{
    public class Page : MonoBehaviour
    {
        [SerializeField] protected bool _allowMultipleInstances = false;
        [SerializeField] protected PageType _type;
        [SerializeField] private bool _isLoaded = false;

        private bool _isLoading = false;

        public bool AllowMultipleInstances => _allowMultipleInstances;
        public PageType Type => _type;
        public bool IsLoaded => _isLoaded;
        public bool IsLoading => _isLoading;

        public event Action OnLoadEvent;
        public event Action OnUnloadEvent;

        protected virtual void Start()
        {
            if (IsLoaded) //hard code initialization support
            {
                Load();
            }
            else
            {
                Unload();
            }
        }

    #region load
        public void Load()
        {
            _isLoading = true;
            StartCoroutine(LoadCoroutine());
        }

        private IEnumerator LoadCoroutine()
        {
            yield return LoadAnimation();
            _isLoaded = true;
            _isLoading = false;
            OnLoad();
            OnLoadEvent?.Invoke();
        }

        protected virtual IEnumerator LoadAnimation()
        {
            yield return null;
            transform.DOScale(1f, 0f);
        }

        protected virtual void OnLoad() { }
    #endregion

    #region unload
        public void Unload() => StartCoroutine(UnloadCoroutine());

        private IEnumerator UnloadCoroutine()
        {
            yield return UnloadAnimation();
            OnUnload();
            OnUnloadEvent?.Invoke();
            _isLoaded = false;
        }

        protected virtual IEnumerator UnloadAnimation()
        {
            yield return null;
            transform.DOScale(0f, 0f);
        }

        protected virtual void OnUnload() { }
    #endregion
    }
}
