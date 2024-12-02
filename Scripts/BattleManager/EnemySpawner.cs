using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public FloorEnemyProbability[] floorProbabilities; // �ν����Ϳ��� ������ ���� Ȯ��
    public GameObject SpawnEnemy(Transform enemyPosition, int floor)
    {
        foreach (var floorProb in floorProbabilities)
        {
            if (floor >= floorProb.startFloor && floor <= floorProb.endFloor)
            {
                float roll = Random.value * 100; // 0�� 100 ������ ��
                float cumulative = 0f;

                foreach (var enemyProb in floorProb.enemies)
                {
                    cumulative += enemyProb.probability;
                    if (roll < cumulative)
                    {
                        if (enemyProb.enemyPrefab != null)
                        {
                            // ���� Enemy ��ġ�� �ڽ����� ����
                            GameObject newEnemy = Instantiate(enemyProb.enemyPrefab, enemyPosition.position, Quaternion.identity, enemyPosition);
                            return newEnemy;
                        }
                        else
                        {
                            return null; // ���� ��Ÿ���� ����
                        }
                    }
                }
                break; // Ȯ���� �´� ���� �������� �ʾҴٸ�, �� ���� ����
            }
        }
        return null; // �ش� ���� ���� ���� ���� �Ǵ� �� ���� Ȯ�� ������ ���
    }
}
