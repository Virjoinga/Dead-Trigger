using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComponentGadgets : MonoBehaviour
{
	public Transform HandR;

	protected AgentHuman Owner;

	private AgentActionUseItem AgentActionUseItem;

	public Dictionary<E_ItemID, Item> Gadgets { get; protected set; }

	private void Awake()
	{
		Owner = GetComponent<AgentHuman>();
		BlackBoard blackBoard = Owner.BlackBoard;
		blackBoard.ActionHandler = (BlackBoard.AgentActionHandler)Delegate.Combine(blackBoard.ActionHandler, new BlackBoard.AgentActionHandler(HandleAction));
		Gadgets = new Dictionary<E_ItemID, Item>();
	}

	private void Activate()
	{
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		Item item = null;
		List<E_ItemID> list = new List<E_ItemID>();
		foreach (PPIItemData item2 in playerPersistentInfo.EquipList.Items)
		{
			if (item2.ID != 0 && item2.Count > 0)
			{
				if (Gadgets.ContainsKey(item2.ID))
				{
					Debug.LogError(" Gadgets is already in the inventory " + item2.ID);
					continue;
				}
				ItemSettings itemSettings = ItemSettingsManager.Instance.Get(item2.ID);
				item = new Item(Owner, item2.ID, (item2.Count <= itemSettings.MaxCountInMisson) ? item2.Count : itemSettings.MaxCountInMisson);
				Gadgets.Add(item2.ID, item);
				list.Add(item2.ID);
			}
		}
		GuiHUD.Instance.SetGadgets(list);
	}

	private void LateUpdate()
	{
		if (AgentActionUseItem != null && AgentActionUseItem.Throw)
		{
			Item gadget = GetGadget(Owner.BlackBoard.Desires.Gadget);
			if (gadget == null)
			{
				AgentActionUseItem.SetFailed();
				return;
			}
			gadget.Use(HandR.position, Owner.BlackBoard.FireDir);
			AgentActionUseItem = null;
		}
		foreach (KeyValuePair<E_ItemID, Item> gadget2 in Gadgets)
		{
			gadget2.Value.Update();
		}
	}

	private void Deactivate()
	{
		foreach (KeyValuePair<E_ItemID, Item> gadget in Gadgets)
		{
			gadget.Value.Destroy();
		}
		Gadgets.Clear();
		AgentActionUseItem = null;
	}

	public void AddGadget(E_ItemID newItem, int count = 1)
	{
		List<E_ItemID> list = new List<E_ItemID>();
		foreach (KeyValuePair<E_ItemID, Item> gadget in Gadgets)
		{
			list.Add(gadget.Key);
		}
		if (Gadgets.ContainsKey(newItem))
		{
			Debug.LogError(" Gadgets is already in the inventory " + newItem);
			return;
		}
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get(newItem);
		Item value = new Item(Owner, newItem, (count <= itemSettings.MaxCountInMisson) ? count : itemSettings.MaxCountInMisson);
		Gadgets.Add(newItem, value);
		list.Add(newItem);
		GuiHUD.Instance.SetGadgets(list);
		GuiHUD.Instance.ShowMessage(GuiHUD.E_MessageType.Console, TextDatabase.instance[3000500] + "  " + TextDatabase.instance[itemSettings.Name], false, 7f);
	}

	public void RemoveGadget(E_ItemID oldItem)
	{
		Item value;
		Gadgets.TryGetValue(oldItem, out value);
		if (value != null)
		{
			value.Destroy();
		}
		Gadgets.Remove(oldItem);
		List<E_ItemID> list = new List<E_ItemID>();
		foreach (KeyValuePair<E_ItemID, Item> gadget in Gadgets)
		{
			list.Add(gadget.Key);
		}
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get(oldItem);
		GuiHUD.Instance.SetGadgets(list);
		GuiHUD.Instance.ShowMessage(GuiHUD.E_MessageType.Console, TextDatabase.instance[3000505] + " " + TextDatabase.instance[itemSettings.Name] + " " + TextDatabase.instance[3000510], false, 7f);
	}

	public void HandleAction(AgentAction action)
	{
		if (!action.IsFailed() && action is AgentActionUseItem)
		{
			AgentActionUseItem = action as AgentActionUseItem;
		}
	}

	public bool IsGadgetAvailableForUse(E_ItemID id)
	{
		if (Gadgets.ContainsKey(id) && Gadgets[id].IsAvailableForUse())
		{
			return Gadgets[id].IsAvailableForUse();
		}
		return false;
	}

	public bool IsGadgetAvailableWithBehaviour(E_ItemBehaviour behaviour)
	{
		foreach (KeyValuePair<E_ItemID, Item> gadget in Gadgets)
		{
			if (gadget.Value.Settings.ItemBehaviour == behaviour && gadget.Value.IsAvailableForUse())
			{
				return true;
			}
		}
		return false;
	}

	public Item GetGadgetAvailableWithBehaviour(E_ItemBehaviour behaviour)
	{
		foreach (KeyValuePair<E_ItemID, Item> gadget in Gadgets)
		{
			if (gadget.Value.Settings.ItemBehaviour == behaviour && gadget.Value.IsAvailableForUse())
			{
				return gadget.Value;
			}
		}
		return null;
	}

	public Item GetGadget(E_ItemID id)
	{
		if (Gadgets.ContainsKey(id))
		{
			return Gadgets[id];
		}
		return null;
	}
}
