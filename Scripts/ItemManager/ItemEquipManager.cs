using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class ItemEquipManager : MonoBehaviour
{
    public static ItemEquipManager Instance { get; private set; }

    public Player player;
    public SkeletonAnimation skeletonAnimation; // Spine �ִϸ��̼� ������Ʈ
    [Header("��� ���� UI")]
    public TMP_Text equippedItemName; // ���� ������ �̸� �ؽ�Ʈ
    public Image equippedItemImage; // ���� ������ �̹���
    public TMP_Text equippedItemDescription; // ���� ������ ���� �ؽ�Ʈ
    [Header("ȿ����")]
    public bool isEquipped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipItem(Item item)
    {
        // Equipment Ÿ������ ĳ���� �õ�
        Equipment equipment = item as Equipment;//�̷��� �ؾ� weaponSkin�� �� �� ����
        player.EquipWeapon(equipment); // �÷��̾ ���� ����
        isEquipped = true;

        skeletonAnimation.skeleton.SetSkin(equipment.weaponSkin);
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.skeleton);

        equippedItemName.text = item.itemName;
        equippedItemImage.sprite = item.itemImage;
        //equippedItemDescription.text = item.itemDescription;
        equippedItemDescription.text = 
            $"������: {equipment.minDamage + player.BasicCapability} - {equipment.maxDamage + player.BasicCapability}";
        // ���� ���� �߰� (��: �÷��̾� ���� ����)
    }
}
