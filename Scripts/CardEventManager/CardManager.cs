using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Player player;  //Player 스크립트
    public PlayerAni playerAni;
    public GameObject cardEvent;
    public CardProbability cardProbability;  // CardProbability 스크립트 참조
    public bool cardSelected = false;
    public Transform firstCardPosition;
    public Transform secondCardPosition;
    public Transform thirdCardPosition;

    void Update()
    {
        if (player.CurrentExp >= player.ExpRequired)
        {
            CardEventStart();
        }
    }

    public void CardEventStart()
    {
        //cardEvent.SetActive(true);  // 카드 이벤트 활성화
        playerAni.Moveable = false;
        cardProbability.SpawnInitialCards(firstCardPosition, secondCardPosition, thirdCardPosition);
        player.CurrentExp = 0;
        cardSelected = false;
    }
    public void ResetCardEvent()
    {
        DestroyAllCards(firstCardPosition);
        DestroyAllCards(secondCardPosition);
        DestroyAllCards(thirdCardPosition);
        //cardEvent.SetActive(false);
        playerAni.Moveable = true;
    }

    void DestroyAllCards(Transform cardPosition)
    {
        foreach (Transform child in cardPosition)
        {
            Destroy(child.gameObject);
        }
    }
    public void ApplyCardEffect(CardInfo cardInfo)
    {
        switch (cardInfo.cardType)
        {
            case CardInfo.CardType.Health:
                ApplyHealthEffect(cardInfo as HpCardInfo);
                break;
            case CardInfo.CardType.Mental:
                ApplyMentalEffect(cardInfo as MentalCardInfo);
                break;
            case CardInfo.CardType.Ability:
                ApplyAbilityEffect(cardInfo as AbilityCardInfo);
                break;
            case CardInfo.CardType.Material:
                ApplyMaterialEffect(cardInfo as MaterialCardInfo);
                break;
        }
    }

    private void ApplyHealthEffect(HpCardInfo hpInfo)
    {
        if (hpInfo.hp_full)
        {
            player.CurrentHP = player.MaxHP;  // 최대 체력으로 회복
        }
        else if (hpInfo.hp_recover > 0)
        {
            player.CurrentHP += hpInfo.hp_recover;  // 지정된 수치만큼 체력 회복
        }
        else if (hpInfo.hp_max_up > 0)
        {
            player.MaxHP += hpInfo.hp_max_up;  // 최대 체력 증가
        }
    }
    private void ApplyMentalEffect(MentalCardInfo mentalCardInfo)
    {
        if(mentalCardInfo.mental_full)
        {
            player.CurrentMental = 0;
        }
        else if(mentalCardInfo.mental_recover > 0)
        {
            player.CurrentMental -= mentalCardInfo.mental_recover;
        }
        else if(mentalCardInfo.mental_max_up > 0)
        {
            player.MaxMental += mentalCardInfo.mental_max_up;
        }
    }
    private void ApplyAbilityEffect(AbilityCardInfo abilityCardInfo)
    {
        if(abilityCardInfo.basic_power > 0)
        {
            player.BasicCapability += abilityCardInfo.basic_power;
        }
        else if(abilityCardInfo.accuracyrate_Increase > 0)
        {
            player.AccuracyRate += abilityCardInfo.accuracyrate_Increase;
        }
    }
    private void ApplyMaterialEffect(MaterialCardInfo materialInfo)
    {
        if (materialInfo != null && InventoryManager.Instance != null)
        {
            // 반복문을 사용하여 아이템 수량(num)만큼 인벤토리에 추가
            for (int i = 0; i < materialInfo.num; i++)
            {
                InventoryManager.Instance.AddItem(materialInfo.item);
            }
        }
    }

}
