using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardCategory
{//�ܺο��� ����
    public string name;//Hp, Mental, Ability, Material
    public List<GameObject> Cards;
    public Transform CardPosition;
}
public class CardProbability : MonoBehaviour
{
    public List<CardCategory> CardCategories;

    [Header("1�� �ڸ�")]
    public GameObject hpCardBackPrefab;
    public Transform FistCardPosition; 
    [Header("2�� �ڸ�")]
    public GameObject mentalCardBackPrefab;
    public Transform SecondCardPosition; 
    [Header("3�� �ڸ�")]
    public GameObject materialCardBackPrefab;
    public GameObject abilityCardBackPrefab;
    [Range(0, 100)]
    public float abilityCardProbability = 20f; // Inspector���� ���� ����
    public Transform thirdCardPosition; // 3�� ī�� ��ġ

    public void SpawnInitialCards(Transform firstPosition, Transform secondPosition, Transform thirdPosition)
    {
        Instantiate(hpCardBackPrefab, firstPosition.position, Quaternion.identity, firstPosition);
        Instantiate(mentalCardBackPrefab, secondPosition.position, Quaternion.identity, secondPosition);
        SpawnThirdCard();
    }
    public void SpawnThirdCard()//3�� �ڸ��� ī�� �޸� ������ ���� ����
    {
        GameObject cardPrefab = DetermineThirdCard();
        Instantiate(cardPrefab, thirdCardPosition.position, Quaternion.identity, thirdCardPosition);
    }

    private GameObject DetermineThirdCard()//3�� �ڸ� ī�� Ȯ��
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
