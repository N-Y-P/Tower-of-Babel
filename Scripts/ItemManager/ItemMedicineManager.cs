using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMedicineManager : MonoBehaviour
{
    public static ItemMedicineManager Instance { get; private set; }

    public Player player;
    public GameObject toolPrefab;

    private void Awake()
    {
        Instance = this; 
    }
    //회복 아이템 우클릭 시 해당 아이템 한 개 소모
    //정신력, 체력 회복
    public void Medicine(ItemSlot itemSlot)
    {
        if (itemSlot.item == null || itemSlot.itemCount <= 0 || !(itemSlot.item is Medicine))
            return;

        Medicine medicine = itemSlot.item as Medicine;

        // 체력 및 정신력 적용
        if (player != null)
        {
            player.CurrentHP += medicine.healthEffect;
            player.CurrentMental += medicine.mentalEffect;
        }

        // 아이템 수량 감소
        itemSlot.itemCount--;
        itemSlot.UpdateSlotUI(); // UI 업데이트

        // 아이템 수량이 0 이하면 아이템 제거
        if (itemSlot.itemCount <= 0)
        {
            Destroy(itemSlot.gameObject);
            toolPrefab.SetActive(false);
        }
    }
}
