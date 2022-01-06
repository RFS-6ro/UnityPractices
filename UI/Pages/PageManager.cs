using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.UI
{
    public class PageManager
    {
        private static Dictionary<PageType, List<Page>> _onPages;
        private static Dictionary<PageType, List<Page>> _offPages;

        private static Transform _parent;

        public static bool IsLoading => false;
        public static bool Loaded => false;

        private static void Initialize()
        {
            _parent = GameObject.FindObjectOfType<Canvas>().transform;
            Page[] pages = _parent.GetComponentsInChildren<Page>();
            _onPages = new Dictionary<PageType, List<Page>>();
            _offPages = new Dictionary<PageType, List<Page>>();

            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i].Type == PageType.ManualManage)
                {
                    continue;
                }

                if (_offPages.ContainsKey(pages[i].Type))
                {
                    _offPages[pages[i].Type].Add(pages[i]);
                }
                else
                {
                    _offPages.Add(pages[i].Type, new List<Page>() { pages[i] });
                    _onPages.Add(pages[i].Type, new List<Page>());
                }
            }
        }

        public static Page LoadPage(PageType type, PageLoadType loadType = PageLoadType.Additive)
        {
            CheckInitialization();

            if (loadType == PageLoadType.Single)
            {
                foreach (KeyValuePair<PageType, List<Page>> currentPages in _onPages)
                {
                    foreach (var page in currentPages.Value)
                    {
                        UnloadPage(page);
                    }
                }
            }

            Page loadingPage;

            if (_offPages.ContainsKey(type))
            {
                if (_onPages[type].Count == 1 && _onPages[type].First().AllowMultipleInstances == false)
                {
                    return _onPages[type].First();
                }

                loadingPage = _offPages[type].FirstOrDefault();

                if (loadingPage == null)
                {
                    loadingPage = AssetUtilities.GetNewPageInstance(type);
                    if (loadingPage == null)
                    {
                        throw new System.Exception("Non prefab page was not added to scene");
                    }
                    loadingPage.transform.parent = _parent;
                }
                else
                {
                    _offPages[type].Remove(loadingPage);
                }
            }
            else
            {
                loadingPage = AssetUtilities.GetNewPageInstance(type);
                if (loadingPage == null)
                {
                    throw new System.Exception("Non prefab page was not added to scene, type == " + type);
                }
                loadingPage.transform.parent = _parent;
                _offPages.Add(type, new List<Page>());
                _onPages.Add(type, new List<Page>());
            }

            if (Random.Range(0, 100) < 20 &&
                type != PageType.Collection &&
                type != PageType.Color &&
                type != PageType.TopView &&
                type != PageType.Painting)
            {
                Interstitial.Instance.ShowInterstitial();
            }

            _onPages[type].Add(loadingPage);
            loadingPage.Load();
            return loadingPage;
        }

        public static void UnloadPage(Page page)
        {
            CheckInitialization();

            page.Unload();

            if (_onPages.ContainsKey(page.Type) &&
                _onPages[page.Type].Contains(page) &&
                _offPages[page.Type].Contains(page) == false)
            {
                _onPages[page.Type].Remove(page);
                _offPages[page.Type].Add(page);
            }
        }

        private static void CheckInitialization()
        {
            if (_onPages == null)
            {
                Initialize();
            }
        }
    }
}
