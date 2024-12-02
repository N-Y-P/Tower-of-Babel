using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StairGenerator : MonoBehaviour
{
    public GameObject stair1Prefab;  // Stair1 프리팹
    public GameObject stair2Prefab;  // Stair2 프리팹
    public Vector3 stair1StartPos;   // Stair1 시작 위치
    public Vector3 stair2StartPos;   // Stair2 시작 위치
    public float stairHeight = 2.9f; // 계단 간의 높이 간격

    void Start()
    {
        GenerateStairs();
    }

    void GenerateStairs()
    {
        int totalStairsCount = 65; // 1층부터 66층까지 65개의 계단 필요
        int startFloor = 1;
        int endFloor = 2;
        float currentXOffset = 0f;

        int currentFloor = 2;

        for (int i = 0; i < totalStairsCount; i++)
        {
            GameObject stairPrefab;
            Vector3 position;

            if (i % 2 == 0) // 짝수 인덱스: stair2
            {
                if (startFloor >= 21 && startFloor < 41)
                {
                    currentXOffset = 3.98f; // 21층부터 40층까지 x축으로 2만큼 이동
                }
                else if (startFloor >= 41)
                {
                    currentXOffset = 7.96f; // 41층부터 x축으로 4만큼 이동
                }
                stairPrefab = stair2Prefab;
                position = new Vector3(stair2StartPos.x + currentXOffset, 
                    stair2StartPos.y + i * stairHeight, 
                    stair2StartPos.z);
            }
            else // 홀수 인덱스: stair1
            {
                stairPrefab = stair1Prefab;
                // 첫 번째 stair1 생성 위치 조정: i == 1일 때는 stairHeight를 추가하지 않음
                position = new Vector3(stair1StartPos.x,
                    stair1StartPos.y + (i == 1 ? 0 : (i - 1) * stairHeight),
                    stair1StartPos.z);
            }

            // 계단 인스턴스 생성
            GameObject stairInstance = Instantiate(stairPrefab, position, Quaternion.identity, this.transform);
            TMP_Text floorText = stairInstance.GetComponentInChildren<TMP_Text>();
            if (floorText != null)
            {
                floorText.text = currentFloor.ToString(); // 층 수 업데이트
            }
            currentFloor++;
            StairInfo stairInfo = stairInstance.GetComponent<StairInfo>();
            if (stairInfo != null)
            {
                stairInfo.StartFloor = startFloor;
                stairInfo.EndFloor = endFloor;
            }
            startFloor++;
            endFloor++;
        }
    }
}

