using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardCategory
{//외부에서 조절
    public string name;//Hp, Mental, Ability, Material
    public List<GameObject> Cards;
    public Transform CardPosition;
}
public class CardProbability : MonoBehaviour
{
    public List<CardCategory> CardCategories;

    [Header("1번 자리")]
    public GameObject hpCardBackPrefab;
    public Transform FistCardPosition; 
    [Header("2번 자리")]
    public GameObject mentalCardBackPrefab;
    public Transform SecondCardPosition; 
    [Header("3번 자리")]
    public GameObject materialCardBackPrefab;
    public GameObject abilityCardBackPrefab;
    [Range(0, 100)]
    public float abilityCardProbability = 20f; // Inspector에서 조절 가능
    public Transform thirdCardPosition; // 3번 카드 위치

    public void SpawnInitialCards(Transform firstPosition, Transform secondPosition, Transform thirdPosition)
    {
        Instantiate(hpCardBackPrefab, firstPosition.position, Quaternion.identity, firstPosition);
        Instantiate(mentalCardBackPrefab, secondPosition.position, Quaternion.identity, secondPosition);
        SpawnThirdCard();
    }
    public void SpawnThirdCard()//3번 자리에 카드 뒷면 프리팹 랜덤 스폰
    {
        GameObject cardPrefab = DetermineThirdCard();
        Instantiate(cardPrefab, thirdCardPosition.position, Quaternion.identity, thirdCardPosition);
    }

    private GameObject DetermineThirdCard()//3번 자리 카드 확률
    {
        float roll = Random.Range(0f, 100f);
        if (roll < abilityCardProbability)
        {
            return abilityCardBackPrefab;
        }
        else
        {
            return materialCardBackPrefab;
        }
    }
    
}
