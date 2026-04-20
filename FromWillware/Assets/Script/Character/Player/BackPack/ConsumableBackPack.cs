using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBackPack : BackPack,ISaveable
{
    // Start is called before the first frame update
    public List<ItemStack> Items = new List<ItemStack>();

    private ItemPickup nearbyItem;
    private ItemBar itemBar;
    private ItemPickup itemPickup;
    
    void Start()
    {
        itemBar = GetComponent<ItemBar>();
        //Items = new List<ItemStack>(new ItemStack[MaxSize]);
    }

    // Update is called once per frame
    

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearbyItem = other.GetComponent<ItemPickup>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearbyItem = null;
        }
    }

    void Update()
    {
        if (nearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            if (AddItem(nearbyItem.ItemData))
            {
                Destroy(nearbyItem.gameObject); // 拾取后消失
                nearbyItem = null;
            }
               
           
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            AddToItemBar(0,0);
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveFromItemBar(0);
        }
    }
    
    public bool AddItem(Item item)
    {
        // 1. 先堆叠
        foreach (var stack in Items)
        {
            if (stack != null && stack.item == item && stack.CurrentCount < item.MaxCount)
            {
                stack.CurrentCount++;
                return true;
            }
            else
            {
                Debug.Log("The item is MaxCount");
                return false;
            }
        }

        // 2. 新建
        if (Items.Count < MaxSize)
        {
            Items.Add(new ItemStack(item, 1));
            return true;
        }
        else
        {
            Debug.Log("背包满");
            return false;
        }
    }

    //整理背包
    void TidyBackPack()
    {
        List<ItemStack> newList = new List<ItemStack>();

        // 1️⃣ 收集所有非空物品
        foreach (var stack in Items)
        {
            if (stack != null)
            {
                newList.Add(stack);
            }
        }

        // 2️⃣ 补空位
        while (newList.Count < MaxSize)
        {
            newList.Add(null);
        }

        // 3️⃣ 替换
        Items = newList;
    }
    
    public void RemoveItem(int index, int amount = 1)
    {
        if (Items[index] == null) return;

        Items[index].CurrentCount -= amount;

        if (Items[index].CurrentCount <= 0)
        {
            Items[index] = null;
            TidyBackPack();
        }
    }

    public void AddToItemBar(int BackIndex,int BarIndex)
    {
        if (Items[BackIndex].BarIndex != -1)
        {
            Debug.Log("Is already in bar");
            return;
        }

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null && Items[i].BarIndex == BarIndex)
            {
                RemoveFromItemBar(i);
                break;
            }
        }
        Items[BackIndex].BarIndex = BarIndex;
        itemBar.Items[BarIndex] =  Items[BackIndex];
    }

    public void RemoveFromItemBar(int BackIndex)
    {
        if (Items[BackIndex].BarIndex == -1)
        {
            Debug.Log("Is not in bar");
            return;
        }
        itemBar.Items[Items[BackIndex].BarIndex] =  null;
        Items[BackIndex].BarIndex = -1;
       
    }
    
    public object CaptureState()
    {
        InventoryData data = new InventoryData();
        data.items = new List<InventoryItemData>();

        foreach (var stack in Items)
        {
            if (stack == null || stack.item == null) continue;

            data.items.Add(new InventoryItemData
            {
                itemID = stack.item.Name,
                count = stack.CurrentCount,
                barIndex = stack.BarIndex
            });
        }

        return JsonUtility.ToJson(data);
    }
    
    public void RestoreState(object state)
    {
        string json = (string)state;
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);

        Items.Clear();

        foreach (var itemData in data.items)
        {
            Item item = ItemDatabase.Instance.GetItemByID(itemData.itemID);

            if (item == null)
            {
                Debug.LogWarning("找不到物品: " + itemData.itemID);
                continue;
            }

            ItemStack stack = new ItemStack(item, itemData.count);
            stack.BarIndex = itemData.barIndex;

            Items.Add(stack);
        }

        // ⭐ 恢复快捷栏（关键！）
        //RestoreItemBar();
    }
    
    public string GetUniqueID()
    {
        return "ConsumableBackPack";
    }
}



[System.Serializable]
public class InventoryItemData
{
    public string itemID;
    public int count;
    public int barIndex;
}

[System.Serializable]
public class InventoryData
{
    public List<InventoryItemData> items;
}
