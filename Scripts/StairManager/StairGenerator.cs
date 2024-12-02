using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StairGenerator : MonoBehaviour
{
    public GameObject stair1Prefab;  // Stair1 ������
    public GameObject stair2Prefab;  // Stair2 ������
    public Vector3 stair1StartPos;   // Stair1 ���� ��ġ
    public Vector3 stair2StartPos;   // Stair2 ���� ��ġ
    public float stairHeight = 2.9f; // ��� ���� ���� ����

    void Start()
    {
        GenerateStairs();
    }

    void GenerateStairs()
    {
        int totalStairsCount = 65; // 1������ 66������ 65���� ��� �ʿ�
        int startFloor = 1;
        int endFloor = 2;
        float currentXOffset = 0f;

        int currentFloor = 2;

        for (int i = 0; i < totalStairsCount; i++)
        {
            GameObject stairPrefab;
            Vector3 position;

            if (i % 2 == 0) // ¦�� �ε���: stair2
            {
                if (startFloor >= 21 && startFloor < 41)
                {
                    currentXOffset = 3.98f; // 21������ 40������ x������ 2��ŭ �̵�
                }
                else if (startFloor >= 41)
                {
                    currentXOffset = 7.96f; // 41������ x������ 4��ŭ �̵�
                }
                stairPrefab = stair2Prefab;
                position = new Vector3(stair2StartPos.x + currentXOffset, 
                    stair2StartPos.y + i * stairHeight, 
                    stair2StartPos.z);
            }
            else // Ȧ�� �ε���: stair1
            {
                stairPrefab = stair1Prefab;
                // ù ��° stair1 ���� ��ġ ����: i == 1�� ���� stairHeight�� �߰����� ����
                position = new Vector3(stair1StartPos.x,
                    stair1StartPos.y + (i == 1 ? 0 : (i - 1) * stairHeight),
                    stair1StartPos.z);
            }

            // ��� �ν��Ͻ� ����
            GameObject stairInstance = Instantiate(stairPrefab, position, Quaternion.identity, this.transform);
            TMP_Text floorText = stairInstance.GetComponentInChildren<TMP_Text>();
            if (floorText != null)
            {
                floorText.text = currentFloor.ToString(); // �� �� ������Ʈ
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

