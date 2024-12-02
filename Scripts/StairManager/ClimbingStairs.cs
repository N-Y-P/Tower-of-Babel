using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Action���


public class ClimbingStairs : MonoBehaviour
{
    public PlayerAni playerAni;
    public CameraMove cameraMove;
    // ����� ������ ����

    // �÷��̾ ����� ������ �޼ҵ�
    public IEnumerator ClimbUpStairs(StairInfo stair, Action onComplete)
    {
        Vector3[] positions = new Vector3[stair.Positions.Length];
        float[] rotations = new float[stair.Positions.Length]; // ȸ���� �迭

        // ��� ������ �Ǵ� �����⿡ ���� �ʱ� ȸ���� ����
        rotations[0] = stair.StartFloor % 2 == 0 ? 0 : 180; // ���� ���� ���� �ʱ� ȸ���� ����
        rotations[1] = rotations[0] == 180 ? 0 : 180; // ���� �ε������� ȸ���� ����

        for (int i = 0; i < stair.Positions.Length; i++)
        {
            positions[i] = stair.Positions[i].position;
        }

        yield return playerAni.StartCoroutine(playerAni.MoveAlongPath(positions, rotations, onComplete));
    }

    // �÷��̾ ����� �������� �޼ҵ�
    public IEnumerator ClimbDownStairs(StairInfo stair, Action onComplete)
    {
        Vector3[] positions = new Vector3[stair.Positions.Length];
        float[] rotations = new float[stair.Positions.Length]; // ȸ���� �迭

        // ��� �������⿡ ���� �ʱ� ȸ���� ����
        rotations[1] = stair.StartFloor % 2 == 0 ? 180 : 0; // ���� ���� ���� �ʱ� ȸ���� ����
        rotations[2] = rotations[1] == 180 ? 0 : 180; // ���� �ε������� ȸ���� ����

        for (int i = 0; i < stair.Positions.Length; i++)
        {
            positions[i] = stair.Positions[i].position;
        }

        // ȸ�� �迭�� �������� �����ؾ� �մϴ�.
        Array.Reverse(positions);  // ��ġ �迭�� �������� ����ϴ�.
        Array.Reverse(rotations);  // ȸ���� �迭�� �������� ����

        yield return playerAni.StartCoroutine(playerAni.MoveAlongPath(positions, rotations, onComplete));
    }
    
    // ������ ��� ���� ã��
    public StairInfo FindStair(int currentFloor, int targetFloor)
    {
        StairInfo[] allStairs = FindObjectsOfType<StairInfo>();
        foreach (StairInfo stair in allStairs)
        {
            if ((stair.StartFloor == currentFloor && stair.EndFloor == targetFloor) ||
                (stair.StartFloor == targetFloor && stair.EndFloor == currentFloor))
            {
                return stair;
            }
        }
        return null; // ������ ����� ã�� ������ ��� null ��ȯ
    }
    
}
