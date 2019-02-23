using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ControlToolkit;
using UnityEngine.SceneManagement;

public class DemoPlayer
{
	public string Name
	{
		get;
		set;
	}

	public bool IsComputer
	{
		get;
		set;
	}
}

public class DemoScene : MonoBehaviour {

	private int m_index;


	[SerializeField] Text TxtOutput;
	[SerializeField] GameObject PnlSelectPlayer;
	[SerializeField] ArrangeItemsControl ArrangeItemsControl;

	void Start () {
		ArrangeItemsControl.ItemDataBinding += OnItemDataBinding;
		ArrangeItemsControl.ItemsArranged += OnItemsArranged;
		ArrangeItemsControl.ItemAdd += OnItemsAdd;
		ArrangeItemsControl.ItemRemoving += OnItemRemoving;
		ArrangeItemsControl.ItemRemoved += OnItemRemoved;

		IList items = new ArrayList();
		items.Add(new DemoPlayer { Name = "Alessandra" });
		items.Add(new DemoPlayer { Name = "Bot 457", IsComputer = true });
		items.Add(new DemoPlayer { Name = "Player 256" });
		items.Add(null);
		items.Add(new DemoPlayer { Name = "Victor Mdvd" });
		items.Add(null);
		items.Add(new DemoPlayer { Name = "Killing machine", IsComputer = true });

		
		ArrangeItemsControl.Items = items;
		DebugOutput("ITEMS ADDED");
	}
	
	void OnItemDataBinding(object sender, ItemDataBindingEventArgs args)
	{
		DemoPlayer dataItem = args.Item as DemoPlayer;
		if(dataItem != null)
		{
			Text text = args.ItemPresenter.GetComponentInChildren<Text>();
			text.text = dataItem.Name;


			Image image = args.ItemPresenter.GetComponentInChildren<Image>();
			if(dataItem.IsComputer)
			{
				Sprite sprite = Resources.Load<Sprite>("computerSmall");  
				image.sprite = sprite;
			}
			else
			{
				Sprite sprite =  Resources.Load<Sprite>("playerSmall");  
				image.sprite = sprite;
			}
		}
	}

	void OnItemsArranged(object sender, System.EventArgs args)
	{
		DebugOutput("ITEMS ARRANGED");
	}

	void OnItemsAdd(object sender, ItemAddEventArgs args)
	{
		m_index = args.Index;
		PnlSelectPlayer.SetActive(true);
		ArrangeItemsControl.gameObject.SetActive(false);

	}

	void OnItemRemoving(object sender, CancelEventArgs args)
	{

	}

	void OnItemRemoved(object sender, ItemRemovedEventArgs args)
	{
		DebugOutput("ITEM REMOVED");
	}
	
	public void OnAddClick(string who)
	{
		PnlSelectPlayer.SetActive(false);
		ArrangeItemsControl.gameObject.SetActive(true);

		DemoPlayer demoPlayer = new DemoPlayer();
		switch(who)
		{
		case "Player":
			demoPlayer.Name = "Player " + Random.Range(0, 999);
			break;
		case "Easy":
			demoPlayer.Name = "Easy Bot " + Random.Range(0, 999);
			demoPlayer.IsComputer = true;
			break;
		case "Medium":
			demoPlayer.Name = "Normal Bot " + Random.Range(0, 999);
			demoPlayer.IsComputer = true;
			break;
		case "Hard":
			demoPlayer.Name = "Hard Bot " + Random.Range(0, 999);
			demoPlayer.IsComputer = true;
			break;
		}

		IList items = ArrangeItemsControl.Items;
		items[m_index] = demoPlayer;
		ArrangeItemsControl.Items = items;

		DebugOutput("ITEM ADDED");

	}

	void DebugOutput(string action)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(string.Format("{0}: {1}", action, System.Environment.NewLine));
		for(int i = 0; i < ArrangeItemsControl.Items.Count; ++i)
		{
			DemoPlayer demo = ArrangeItemsControl.Items[i] as DemoPlayer;
			if(demo != null)
			{
				sb.Append( string.Format("{0}. {1} {2}", i, demo.Name, System.Environment.NewLine));
			}
			else
			{
				sb.Append( string.Format("{0}. {1} {2}", i, "Empty", System.Environment.NewLine));
			}
		}
		
		TxtOutput.text = sb.ToString();
	}

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }


}
