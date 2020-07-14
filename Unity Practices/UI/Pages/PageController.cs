using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController Instance;

            private Hashtable _pages;
            private List<Page> _onList;
            private List<Page> _offList;

            public Page[] Pages;
            public PageType EntryPage;
            [HideInInspector] public PageType CurrentPage;
            
            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    _pages = new Hashtable();
                    _onList = new List<Page>();
                    _offList = new List<Page>();

                    RegisterAllPages();

                    CurrentPage = EntryPage;

                    if (EntryPage != PageType.None)
                    {
                        TurnPageOn(EntryPage);
                    }
                }
            }

            public void TurnPageOn(PageType type, bool closeCurrentPage = false, Action callback = null)
            {
                if (type == PageType.None)
                {
                    return;
                }

                if (PageExists(type) == false)
                {
                    return;
                }
                
                if (closeCurrentPage)
                {
                    TurnPageOff(CurrentPage, type);
                }
                else
                {
                    Page page = GetPage(type);
                    page.gameObject.SetActive(true);
                    page.SetState(true);
                    CurrentPage = type;
                }
                callback?.Invoke();
            }

            public void TurnPageOff(PageType offType, PageType onType = PageType.None, bool waitForExit = false)
            {
                if (offType == PageType.None)
                {
                    return;
                }

                if (PageExists(offType) == false)
                {
                    return;
                }

                Page offPage = GetPage(offType);

                if (offPage.gameObject.activeSelf)
                {
                    offPage.SetState(false);
                }

                if (waitForExit && offPage.UseAnimation)
                {
                    Page onPage = GetPage(onType);
                    StopCoroutine("WaitForPageExit");
                    StartCoroutine(WaitForPageExit(onPage, offPage));
                }
                else
                {
                    TurnPageOn(onType);
                }
            }

            private IEnumerator WaitForPageExit(Page onPage, Page offPage)
            {
                while (offPage.TargetState != Page.FLAG_NONE)
                {
                    yield return null;
                }
                TurnPageOn(onPage.Type);
            }

            public bool IsPageOn(PageType type)
            {
                if (PageExists(type) == false)
                {
                    return false;
                }

                return GetPage(type).IsOn;
            }

            private Page GetPage(PageType type)
            {
                if (PageExists(type) == false)
                {
                    return null;
                }

                return (Page)_pages[type];
            }

            private bool PageExists(PageType type)
            {
                return _pages.Contains(type);
            }

            private void RegisterAllPages()
            {
                foreach (Page page in Pages)
                {
                    RegisterPage(page);
                }
            }

            private void RegisterPage(Page page)
            {
                if (PageExists(page.Type))
                {
                    return;
                }

                _pages.Add(page.Type, page);
                TurnPageOff(page.Type);
            }
        }
    }
}
