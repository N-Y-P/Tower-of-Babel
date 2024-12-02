using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemCategory
    //�ܺο��� 
{
    public string name;//�Ϲ�, ���, ���
    public List<GameObject> items;//��޺��� ���� �������� ����Ʈ�� ����
    public float probability;//Ȯ��
}

public class ItemProbability : MonoBehaviour
{
    public List<ItemCategory> categories;

    // ��ġ�� ������ ī�װ� �ε����� �������� �������� �����ϴ� �Լ�
    public GameObject SpawnItem(Vector3 position)
    {
        float totalProbability = 0;
        foreach (var category in categories)
        {
            totalProbability += category.probability;
        }

        float randomPoint = UnityEngine.Random.Range(0, totalProbability);//�̷��� ��� ����Ƽ���߿��� ����ϴ� ���� ���
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
