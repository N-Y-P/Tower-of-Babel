using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Player player;  //Player ��ũ��Ʈ
    public PlayerAni playerAni;
    public GameObject cardEvent;
    public CardProbability cardProbability;  // CardProbability ��ũ��Ʈ ����
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
        //cardEvent.SetActive(true);  // ī�� �̺�Ʈ Ȱ��ȭ
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
            player.CurrentHP = player.MaxHP;  // �ִ� ü������ ȸ��
        }
        else if (hpInfo.hp_recover > 0)
        {
            player.CurrentHP += hpInfo.hp_recover;  // ������ ��ġ��ŭ ü�� ȸ��
        }
        else if (hpInfo.hp_max_up > 0)
        {
            player.MaxHP += hpInfo.hp_max_up;  // �ִ� ü�� ����
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
            // �ݺ����� ����Ͽ� ������ ����(num)��ŭ �κ��丮�� �߰�
            for (int i = 0; i < materialInfo.num; i++)
            {
                InventoryManager.Instance.AddItem(materialInfo.item);
            }
        }
    }

}
