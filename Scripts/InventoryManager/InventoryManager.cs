using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // �̱��� �ν��Ͻ�


    public GameObject slotPrefab; // Slot ������
    public Transform equipmentContainer; // ����� �����̳�
    public Transform medicineContainer; // ü�¾�� �����̳�
    public Transform ingredientContainer; // ���� �����̳�

    private Dictionary<int, ItemSlot> itemSlots = new Dictionary<int, ItemSlot>(); // ������ ID�� ItemSlot�� ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ������Ʈ �ı� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� �ı�
        }
    }

    public void AddItem(Item item)
    {
        Transform container = GetContainer(item.itemType);
        if (itemSlots.ContainsKey(item.itemID))
        {
            // �̹� �ش� �������� ������ ������ ����
            itemSlots[item.itemID].AddItemCount(1);
        }
        else
        {
            // �� ���� ����
            GameObject slot = Instantiate(slotPrefab, GetContainer(item.itemType), false);
            ItemSlot newSlot = slot.GetComponent<ItemSlot>();
            newSlot.SetItem(item); // ������ ����
            newSlot.UpdateSlotUI(); // ���� UI �ʱ�ȭ

            // ��ųʸ��� �߰�
            itemSlots.Add(item.itemID, newSlot);
            ReorderItems(container);
        }
    }

    // �������� �����ϴ� �޼���
    private void ReorderItems(Transform container)
    {
        var items = container.GetComponentsInChildren<ItemSlot>().ToList();
        items.Sort((x, y) => x.item.itemID.CompareTo(y.item.itemID)); // Item ID ���� ����
        foreach (var item in items)
        {
            item.transform.SetAsLastSibling(); // ���ĵ� ������� �����̳ʿ� ���ġ
        }
    }
    //���� üũ
    public int GetItemCount(Item item)
    {
        if (itemSlots.ContainsKey(item.itemID))
        {
            return itemSlots[item.itemID].itemCount;
        }
        return 0; // �������� �������� �ʴ� ��� 0 ��ȯ
    }
    //��� üũ
    public bool CheckMaterials(ItemRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!itemSlots.ContainsKey(ingredient.item.itemID) || 
                itemSlots[ingredient.item.itemID].itemCount < ingredient.quantity)
            {
                return false;  // �ʿ��� ��ᰡ �����ϸ� false ��ȯ
            }
        }
        return true;  // ��� ��ᰡ ����ϸ� true ��ȯ
    }
    //��� �Ҹ� �� ������ �߰�
    public void RemoveItem(Item item, int quantity)
    {
        if (itemSlots.ContainsKey(item.itemID))
        {
            ItemSlot slot = itemSlots[item.itemID];
            if (slot.itemCount >= quantity)
            {
                // ������ ���� ����
                slot.AddItemCount(-quantity);

                // ������ ������ 0�� �Ǹ� ���� ����
                if (slot.itemCount <= 0)
                {
                    Destroy(slot.gameObject); // ���� ���� ������Ʈ ����
                    itemSlots.Remove(item.itemID); // ��ųʸ����� ������ ID ����
                }
            }
        }
    }
    // ������ ������ ���� ������ �����̳ʸ� ��ȯ
    private Transform GetContainer(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.Equipment:
                return equipmentContainer;
            case Item.ItemType.Medicine:
                return medicineContainer;
            case Item.ItemType.Ingredient:
                return ingredientContainer;
            default:
                return null;
        }
    }
}
