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
    public SkeletonAnimation skeletonAnimation; // Spine 애니메이션 컴포넌트
    [Header("장비 장착 UI")]
    public TMP_Text equippedItemName; // 장착 아이템 이름 텍스트
    public Image equippedItemImage; // 장착 아이템 이미지
    public TMP_Text equippedItemDescription; // 장착 아이템 설명 텍스트
    [Header("효과음")]
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
        // Equipment 타입으로 캐스팅 시도
        Equipment equipment = item as Equipment;//이렇게 해야 weaponSkin을 쓸 수 있음
        player.EquipWeapon(equipment); // 플레이어에 무기 장착
        isEquipped = true;

        skeletonAnimation.skeleton.SetSkin(equipment.weaponSkin);
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.skeleton);

        equippedItemName.text = item.itemName;
        equippedItemImage.sprite = item.itemImage;
        //equippedItemDescription.text = item.itemDescription;
        equippedItemDescription.text = 
            $"데미지: {equipment.minDamage + player.BasicCapability} - {equipment.maxDamage + player.BasicCapability}";
        // 장착 로직 추가 (예: 플레이어 스탯 변경)
    }
}
