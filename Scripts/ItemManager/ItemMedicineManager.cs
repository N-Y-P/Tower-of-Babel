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
    //ȸ�� ������ ��Ŭ�� �� �ش� ������ �� �� �Ҹ�
    //���ŷ�, ü�� ȸ��
    public void Medicine(ItemSlot itemSlot)
    {
        if (itemSlot.item == null || itemSlot.itemCount <= 0 || !(itemSlot.item is Medicine))
            return;

        Medicine medicine = itemSlot.item as Medicine;

        // ü�� �� ���ŷ� ����
        if (player != null)
        {
            player.CurrentHP += medicine.healthEffect;
            player.CurrentMental += medicine.mentalEffect;
        }

        // ������ ���� ����
        itemSlot.itemCount--;
        itemSlot.UpdateSlotUI(); // UI ������Ʈ

        // ������ ������ 0 ���ϸ� ������ ����
        if (itemSlot.itemCount <= 0)
        {
            Destroy(itemSlot.gameObject);
            toolPrefab.SetActive(false);
        }
    }
}
