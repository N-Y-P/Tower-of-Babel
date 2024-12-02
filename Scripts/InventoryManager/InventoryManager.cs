using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // 싱글톤 인스턴스


    public GameObject slotPrefab; // Slot 프리팹
    public Transform equipmentContainer; // 무기용 컨테이너
    public Transform medicineContainer; // 체력약용 컨테이너
    public Transform ingredientContainer; // 재료용 컨테이너

    private Dictionary<int, ItemSlot> itemSlots = new Dictionary<int, ItemSlot>(); // 아이템 ID와 ItemSlot의 매핑

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 게임 오브젝트 파괴 방지
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
        }
    }

    public void AddItem(Item item)
    {
        Transform container = GetContainer(item.itemType);
        if (itemSlots.ContainsKey(item.itemID))
        {
            // 이미 해당 아이템이 있으면 수량만 증가
            itemSlots[item.itemID].AddItemCount(1);
        }
        else
        {
            // 새 슬롯 생성
            GameObject slot = Instantiate(slotPrefab, GetContainer(item.itemType), false);
            ItemSlot newSlot = slot.GetComponent<ItemSlot>();
            newSlot.SetItem(item); // 아이템 설정
            newSlot.UpdateSlotUI(); // 슬롯 UI 초기화

            // 딕셔너리에 추가
            itemSlots.Add(item.itemID, newSlot);
            ReorderItems(container);
        }
    }

    // 아이템을 정렬하는 메서드
    private void ReorderItems(Transform container)
    {
        var items = container.GetComponentsInChildren<ItemSlot>().ToList();
        items.Sort((x, y) => x.item.itemID.CompareTo(y.item.itemID)); // Item ID 기준 정렬
        foreach (var item in items)
        {
            item.transform.SetAsLastSibling(); // 정렬된 순서대로 컨테이너에 재배치
        }
    }
    //개수 체크
    public int GetItemCount(Item item)
    {
        if (itemSlots.ContainsKey(item.itemID))
        {
            return itemSlots[item.itemID].itemCount;
        }
        return 0; // 아이템이 존재하지 않는 경우 0 반환
    }
    //재료 체크
    public bool CheckMaterials(ItemRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!itemSlots.ContainsKey(ingredient.item.itemID) || 
                itemSlots[ingredient.item.itemID].itemCount < ingredient.quantity)
            {
                return false;  // 필요한 재료가 부족하면 false 반환
            }
        }
        return true;  // 모든 재료가 충분하면 true 반환
    }
    //재료 소모 및 아이템 추가
    public void RemoveItem(Item item, int quantity)
    {
        if (itemSlots.ContainsKey(item.itemID))
        {
            ItemSlot slot = itemSlots[item.itemID];
            if (slot.itemCount >= quantity)
            {
                // 아이템 수량 감소
                slot.AddItemCount(-quantity);

                // 아이템 수량이 0이 되면 슬롯 제거
                if (slot.itemCount <= 0)
                {
                    Destroy(slot.gameObject); // 슬롯 게임 오브젝트 제거
                    itemSlots.Remove(item.itemID); // 딕셔너리에서 아이템 ID 제거
                }
            }
        }
    }
    // 아이템 유형에 따라 적절한 컨테이너를 반환
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
