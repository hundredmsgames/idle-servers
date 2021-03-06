using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

namespace ControlToolkit
{

    public class ItemDataBindingEventArgs : System.EventArgs
    {
        public object Item
        {
            get;
            private set;
        }

        public GameObject ItemPresenter
        {
            get;
            private set;
        }

        public bool CanDrag
        {
            get;
            set;
        }

        public ItemDataBindingEventArgs(object item, GameObject itemPresenter)
        {
            Item = item;
            ItemPresenter = itemPresenter;
            CanDrag = true;
        }
    }

    public class ArrangeItemsControl : MonoBehaviour
    {
        [SerializeField] GameObject EmptyPresenterPrefab;
        [SerializeField] GameObject ItemPresenterPrefab;
        [SerializeField] GameObject ItemContainerPrefab;
        [SerializeField] AudioSource ItemDropSound;
        [SerializeField] AudioSource ItemAddSound;

        public event EventHandler<ItemDataBindingEventArgs> ItemDataBinding;
        public event EventHandler<ItemDataBindingEventArgs> EmptyDataBinding;

        public event EventHandler ItemsArranged;

        private ItemPlaceholder m_toPlaceholder;
        private ItemPlaceholder m_fromPlaceholder;
        private ItemPlaceholder[] m_placeholders;

        private int m_maxItemsCount = int.MaxValue;
        public int MaxItemsCount
        {
            get { return m_maxItemsCount; }
            set
            {
                m_maxItemsCount = value;
                if (m_maxItemsCount < 0)
                {
                    m_maxItemsCount = 0;
                }
                EnableDisablePlaceholders();
            }
        }

        private void EnableDisablePlaceholders()
        {
            if (m_placeholders != null)
            {
                for (int i = 0; i < Math.Min(m_placeholders.Length, m_maxItemsCount); ++i)
                {
                    m_placeholders[i].gameObject.SetActive(true);
                }

                for (int i = m_maxItemsCount; i < m_placeholders.Length; ++i)
                {
                    m_placeholders[i].gameObject.SetActive(false);
                }
            }
        }

        private GameObject m_dragSurface;
        private bool m_isDragging;

        private IList m_items;
        public IList Items
        {
            get { return m_items; }
            set
            {
                m_items = value;
                DataBind();
            }
        }


        private bool m_interactable = true;
        public bool Interactable
        {
            get { return m_interactable; }
            set
            {
                if (m_interactable != value)
                {
                    m_interactable = value;
                    foreach (ItemContainer container in GetComponentsInChildren<ItemContainer>())
                    {
                        container.Interactable = value;
                    }
                }
            }
        }

        public void SetupItem(ItemPlaceholder itemPlaceholder)
        {
            //when we setup a Item this function handles the setup
        }

        private void DataBind()
        {
            if (m_placeholders == null)
            {
                return;
            }

            if (m_items != null)
            {
                for (int i = m_items.Count; i < Math.Min(m_placeholders.Length, MaxItemsCount); ++i)
                {
                    m_items.Add(null);
                }

                int count = Math.Min(MaxItemsCount, Math.Min(m_placeholders.Length, m_items.Count));
                for (int i = 0; i < count; ++i)
                {
                    ItemPlaceholder placeholder = m_placeholders[i];
                    GameObject container = (GameObject)Instantiate(ItemContainerPrefab);
                    container.transform.SetParent(placeholder.gameObject.transform, false);
                    ItemContainer itemContainer = container.GetComponent<ItemContainer>();

                    //not sure if we should do this way
                    // GameContoller.Instance.ItemContainers.Add(itemContainer);
                    Debug.Log("ArrangeItemsControl::Databind::Buras� �al���yor");

                    if (itemContainer == null)
                    {
                        itemContainer = container.AddComponent<ItemContainer>();
                    }

                    itemContainer.Interactable = m_interactable;

                    object item = m_items[i];
                    if (item == null)
                    {
                        GameObject presenter = (GameObject)Instantiate(EmptyPresenterPrefab);
                        presenter.transform.SetParent(container.transform, false);
                        presenter.transform.SetAsFirstSibling();

                        if (EmptyDataBinding != null)
                        {
                            ItemDataBindingEventArgs args = new ItemDataBindingEventArgs(null, presenter);
                            EmptyDataBinding(this, args);
                            itemContainer.CanDrag = args.CanDrag;
                        }
                        else
                        {
                            itemContainer.CanDrag = false;
                        }
                    }
                    else
                    {
                        GameObject presenter = (GameObject)Instantiate(ItemPresenterPrefab);
                        presenter.transform.SetParent(container.transform, false);
                        presenter.transform.SetAsFirstSibling();

                        if (container != null)
                        {
                            if (ItemDataBinding != null)
                            {
                                ItemDataBindingEventArgs args = new ItemDataBindingEventArgs(item, presenter);
                                ItemDataBinding(this, args);

                                itemContainer.CanDrag = args.CanDrag;
                            }
                            else
                            {
                                itemContainer.CanDrag = true;
                            }
                        }
                    }

                }
            }
        }

        private void Activate(ItemPlaceholder placeholder)
        {
            if (!CanHandleEvent(placeholder))
            {
                return;
            }
            m_toPlaceholder = placeholder;

        }

        private void Deactivate(ItemPlaceholder placeholder)
        {
            if (!CanHandleEvent(placeholder))
            {
                return;
            }

            if (m_toPlaceholder == placeholder)
            {
                m_toPlaceholder = null;
            }
        }

        private void BeginDrag(ItemContainer container)
        {
            if (!CanHandleEvent(container))
            {
                return;
            }


            m_fromPlaceholder = container.transform.parent.GetComponent<ItemPlaceholder>();

            if (m_fromPlaceholder != null)
            {
                m_isDragging = true;

                container.transform.SetParent(m_dragSurface.transform);

                m_fromPlaceholder.StartHover();

            }
        }

        private void Drop(ItemContainer container)
        {
            if (!CanHandleEvent(container))
            {
                return;
            }

            if (!m_isDragging)
            {
                return;
            }

            m_isDragging = false;

            if (m_toPlaceholder == null)
            {
                m_toPlaceholder = m_fromPlaceholder;
            }

            ItemContainer displacedContainer = m_toPlaceholder.GetComponentInChildren<ItemContainer>();
            container.transform.SetParent(m_toPlaceholder.transform);
            container.MoveTo(m_toPlaceholder.transform.position);

            if (displacedContainer != null && displacedContainer != container)
            {
                displacedContainer.transform.SetParent(m_fromPlaceholder.transform);
                displacedContainer.MoveTo(m_fromPlaceholder.transform.position);

                if (ItemDropSound != null)
                {
                    ItemDropSound.Play();
                }
            }


            m_toPlaceholder.StopHover();

            if (m_items == null)
            {
                return;
            }

            if (displacedContainer == null)
            {
                return;
            }


            int fromIndex = IndexOf(displacedContainer);
            int toIndex = IndexOf(container);

            object tmp = Items[fromIndex];
            Items[fromIndex] = Items[toIndex];
            Items[toIndex] = tmp;

            if (ItemsArranged != null)
            {
                ItemsArranged(this, EventArgs.Empty);
            }

            m_fromPlaceholder = null;
        }




        private int IndexOf(ItemContainer container)
        {
            ItemPlaceholder placeholder = container.transform.parent.GetComponent<ItemPlaceholder>();
            if (placeholder == null)
            {
                if (m_isDragging)
                {
                    placeholder = m_fromPlaceholder;
                }
            }

            int count = Math.Min(MaxItemsCount, Math.Min(m_placeholders.Length, m_items.Count));
            for (int i = 0; i < count; ++i)
            {
                if (m_placeholders[i] == placeholder)
                {
                    return i;
                }
            }

            return -1;
        }

        private void OnBeginDrag(object s, EventArgs e)
        {
            BeginDrag((ItemContainer)s);
        }

        private void OnDrop(object s, EventArgs e)
        {
            Drop((ItemContainer)s);
        }


        private void OnDeactivate(object s, EventArgs e)
        {
            Deactivate((ItemPlaceholder)s);
        }

        private void OnActivate(object s, CancelEventArgs e)
        {
            ItemPlaceholder placeholder = (ItemPlaceholder)s;
            if (!CanHandleEvent(placeholder))
            {
                return;
            }
            if (m_isDragging)
            {
                Activate(placeholder);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Start()
        {
            m_dragSurface = new GameObject();
            m_dragSurface.name = "DragSurface";
            m_dragSurface.transform.SetParent(transform);
            RectTransform rectTransform = m_dragSurface.AddComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            CanvasGroup canvasGroup = m_dragSurface.AddComponent<CanvasGroup>();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = false;

            m_placeholders = GetComponentsInChildren<ItemPlaceholder>().OrderBy(p => p.Index).ToArray();
            EnableDisablePlaceholders();
            DataBind();
        }

        private void OnEnable()
        {
            ItemContainer.BeginDrag += OnBeginDrag;
            ItemContainer.Drop += OnDrop;
            ItemPlaceholder.Deactivate += OnDeactivate;
            ItemPlaceholder.Activate += OnActivate;
        }

        private void OnDisable()
        {
            ItemContainer.BeginDrag -= OnBeginDrag;
            ItemContainer.Drop -= OnDrop;
            ItemPlaceholder.Deactivate -= OnDeactivate;
            ItemPlaceholder.Activate -= OnActivate;
        }

        private bool CanHandleEvent(ItemPlaceholder placeholder)
        {
            return placeholder.transform.IsChildOf(transform);
        }

        private bool CanHandleEvent(ItemContainer container)
        {
            return container.transform.IsChildOf(transform);
        }
    }
}
