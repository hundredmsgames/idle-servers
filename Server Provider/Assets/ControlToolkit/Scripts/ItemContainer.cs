using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace ControlToolkit
{
    public class ItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static EventHandler BeginDrag;
        public static EventHandler Drop;

        public static EventHandler<CancelEventArgs> Removing;
        public static EventHandler Removed;

        public static EventHandler Add;

        public bool hasItem = false;

        // ADDED
        public delegate void ClickHandler(ItemContainer container, PointerEventData pointerEventData);
        public event ClickHandler OnClick;

        public float MoveDuration = 0.2f;
        private float m_beginMoveT;

        private bool m_animate;

        private bool m_drag;
        private CanvasGroup m_canvasGroup;
        private Vector3 m_position;
        private Vector3 m_startPosition;

        private bool m_interactable = true;

        private bool m_canDrag = true;

        public bool Interactable
        {
            get { return m_interactable; }
            set
            {
                m_interactable = value;

                if (m_canvasGroup != null)
                {
                    m_canvasGroup.interactable = value;
                }
            }
        }
        public bool IsClickable
        {
            get { return OnClick != null; }
        }

        public bool CanDrag
        {
            get { return m_canDrag; }
            set
            {
                m_canDrag = value;
            }
        }



        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("ItemContainer::OnPointerDown");

            if (!m_canDrag || !hasItem)
            {
                return;
            }

            if (!m_interactable)
            {
                return;
            }

            m_animate = false;
            m_drag = true;
            m_canvasGroup.ignoreParentGroups = false;

            m_position = Input.mousePosition;

            if (BeginDrag != null)
            {
                BeginDrag(this, System.EventArgs.Empty);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log("ItemContainer::OnPointerUp");

            // ADDED 
            if (OnClick != null)
                OnClick(this, eventData);

            if (!m_canDrag)
            {
                return;
            }
            if (!m_interactable)
            {
                return;
            }

            m_drag = false;
            m_canvasGroup.ignoreParentGroups = true;

            if (Drop != null)
            {
                Drop(this, System.EventArgs.Empty);
            }
        }


        public void MoveTo(Vector3 position)
        {
            if (m_drag)
            {
                return;
            }

            m_animate = true;
            m_startPosition = transform.position;
            m_position = position;
            m_beginMoveT = Time.time;
        }

        // Use this for initialization
        private void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            if (m_canvasGroup != null)
            {
                m_canvasGroup.interactable = m_interactable;
            }

        }

        // Update is called once per frame
        private void Update()
        {

            if (m_drag)
            {
                transform.position += (Input.mousePosition - m_position);
                m_position = Input.mousePosition;
            }
            else if (m_animate)
            {
                float t = (Time.time - m_beginMoveT) / MoveDuration;
                if (t >= 1.0f)
                {
                    t = 1.0f;
                    m_animate = false;
                }

                transform.position = m_startPosition + (m_position - m_startPosition) * t;
            }
        }


    }
}

