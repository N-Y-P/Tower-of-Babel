using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Action사용


public class ClimbingStairs : MonoBehaviour
{
    public PlayerAni playerAni;
    public CameraMove cameraMove;
    // 계단을 오르는 로직

    // 플레이어가 계단을 오르는 메소드
    public IEnumerator ClimbUpStairs(StairInfo stair, Action onComplete)
    {
        Vector3[] positions = new Vector3[stair.Positions.Length];
        float[] rotations = new float[stair.Positions.Length]; // 회전값 배열

        // 계단 오르기 또는 내리기에 따라 초기 회전값 설정
        rotations[0] = stair.StartFloor % 2 == 0 ? 0 : 180; // 시작 층에 따라 초기 회전값 설정
        rotations[1] = rotations[0] == 180 ? 0 : 180; // 다음 인덱스에서 회전값 변경

        for (int i = 0; i < stair.Positions.Length; i++)
        {
            positions[i] = stair.Positions[i].position;
        }

        yield return playerAni.StartCoroutine(playerAni.MoveAlongPath(positions, rotations, onComplete));
    }

    // 플레이어가 계단을 내려가는 메소드
    public IEnumerator ClimbDownStairs(StairInfo stair, Action onComplete)
    {
        Vector3[] positions = new Vector3[stair.Positions.Length];
        float[] rotations = new float[stair.Positions.Length]; // 회전값 배열

        // 계단 내려가기에 따라 초기 회전값 설정
        rotations[1] = stair.StartFloor % 2 == 0 ? 180 : 0; // 시작 층에 따라 초기 회전값 설정
        rotations[2] = rotations[1] == 180 ? 0 : 180; // 다음 인덱스에서 회전값 변경

        for (int i = 0; i < stair.Positions.Length; i++)
        {
            positions[i] = stair.Positions[i].position;
        }

        // 회전 배열도 역순으로 설정해야 합니다.
        Array.Reverse(positions);  // 위치 배열을 역순으로 만듭니다.
        Array.Reverse(rotations);  // 회전값 배열도 역순으로 설정

        yield return playerAni.StartCoroutine(playerAni.MoveAlongPath(positions, rotations, onComplete));
    }
    
    // 적절한 계단 정보 찾기
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
        return null; // 적절한 계단을 찾지 못했을 경우 null 반환
    }
    
}
