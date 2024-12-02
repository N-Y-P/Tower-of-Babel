using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public FloorEnemyProbability[] floorProbabilities; // 인스펙터에서 설정할 층별 확률
    public GameObject SpawnEnemy(Transform enemyPosition, int floor)
    {
        foreach (var floorProb in floorProbabilities)
        {
            if (floor >= floorProb.startFloor && floor <= floorProb.endFloor)
            {
                float roll = Random.value * 100; // 0과 100 사이의 값
                float cumulative = 0f;

                foreach (var enemyProb in floorProb.enemies)
                {
                    cumulative += enemyProb.probability;
                    if (roll < cumulative)
                    {
                        if (enemyProb.enemyPrefab != null)
                        {
                            // 적을 Enemy 위치의 자식으로 생성
                            GameObject newEnemy = Instantiate(enemyProb.enemyPrefab, enemyPosition.position, Quaternion.identity, enemyPosition);
                            return newEnemy;
                        }
                        else
                        {
                            return null; // 적이 나타나지 않음
                        }
                    }
                }
                break; // 확률에 맞는 적이 생성되지 않았다면, 적 생성 없음
            }
        }
        return null; // 해당 층에 대한 정보 없음 또는 적 생성 확률 범위를 벗어남
    }
}
