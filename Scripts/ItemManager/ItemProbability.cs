using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemCategory
    //외부에서 
{
    public string name;//일반, 고급, 희귀
    public List<GameObject> items;//등급별로 각각 프리팹을 리스트로 받음
    public float probability;//확률
}

public class ItemProbability : MonoBehaviour
{
    public List<ItemCategory> categories;

    // 위치와 아이템 카테고리 인덱스를 기준으로 아이템을 생성하는 함수
    public GameObject SpawnItem(Vector3 position)
    {
        float totalProbability = 0;
        foreach (var category in categories)
        {
            totalProbability += category.probability;
        }

        float randomPoint = UnityEngine.Random.Range(0, totalProbability);//이렇게 써야 유니티개발에서 사용하는 랜덤 사용
        float currentProbability = 0;

        foreach (var category in categories)
        {
            currentProbability += category.probability;
            if (randomPoint <= currentProbability)
            {
                if (category.items.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, category.items.Count);
                    GameObject item = Instantiate(category.items[randomIndex], position, Quaternion.identity);
                    return item;
                }
            }
        }
        return null;
    }
}
