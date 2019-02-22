using System;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections;
using UnityEngine.EventSystems;

namespace ControlToolkit
{
	public class CancelEventArgs : System.EventArgs
	{
		public bool Cancel
		{
			get;
			set;
		}
	}
	
	public class ItemPlaceholder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public static EventHandler<CancelEventArgs> Activate;
		public static EventHandler Deactivate;
		
		public int Index;
		public Color HoverColor;
		public Color NormalColor;
		private Image m_background;
		
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			StartHover();
		}
		
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			StopHover();
		}
		
		public void StartHover() 
		{
			CancelEventArgs args = new CancelEventArgs();
			if(Activate != null)
			{
				Activate(this, args);
			}
			
			if(!args.Cancel)
			{
				if(m_background != null)
				{
					m_background.color = HoverColor;
				}
			}
		}
		
		public void StopHover() 
		{
			if(m_background != null)
			{
				m_background.color = NormalColor;
			}

			
			if(Deactivate != null)
			{
				Deactivate(this, System.EventArgs.Empty);
			}
		}
		
		public ItemContainer BindContainer(ItemContainer container, ItemPlaceholder placeholder)
		{
			ItemContainer movedContainer = GetComponentInChildren<ItemContainer>();
			
			container.transform.SetParent(gameObject.transform);
			container.transform.position = gameObject.transform.position;
			
			if(movedContainer != null && movedContainer != container)
			{
				placeholder.BindContainer(movedContainer, this);
			}
			
			return movedContainer;
		}
		
		// Use this for initialization
		private void Start () 
		{
			m_background = GetComponent<Image>();
			if(m_background != null)
			{
				m_background.color = NormalColor;
			}
		}
	
	}
}

