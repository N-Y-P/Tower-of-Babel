using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 위치값 가져오기
    public PlayerAni playerAni; //이동 시 애니메이션
    public ClimbingStairs climbingStairs; // 계단 이동 로직
    private RoomInfo currentRoomInfo; // 현재 플레이어가 위치한 방 정보 저장
    public CameraMove cameraMove;
    float targetYRotation = 0;

    // 현재 플레이어의 방 정보를 반환하는 메서드
    public RoomInfo GetCurrentRoomInfo()
    {
        return currentRoomInfo;
    }
    public bool isPoint1;
    public Transform PlayerTransform { 
        get { return playerTransform; }
        private set { playerTransform = value; }
    }

    // 방 클릭 시 그 방의 point로 이동하는 메소드
    public void MovePlayerToRoom(GameObject room)
    {
        RoomInfo newRoomInfo = room.GetComponent<RoomInfo>();
        if (newRoomInfo == null)
        {
            Debug.LogError("RoomInfo 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        if (currentRoomInfo != null && currentRoomInfo.RoomNumber == newRoomInfo.RoomNumber)
        {
            Debug.Log("이미 해당 방에 있습니다: Room " + newRoomInfo.RoomNumber);
            return;
        }

        if (currentRoomInfo != null && currentRoomInfo.CurrenFloor != newRoomInfo.CurrenFloor)
        {
            StairInfo stair = climbingStairs.FindStair(currentRoomInfo.CurrenFloor, newRoomInfo.CurrenFloor);
            if (stair != null)
            {
                System.Action onComplete = () => {
                    MoveToFinalPosition(newRoomInfo);
                };

                if (currentRoomInfo.CurrenFloor < newRoomInfo.CurrenFloor)
                {
                    StartCoroutine(climbingStairs.ClimbUpStairs(stair, onComplete));
                }
                else
                {
                    StartCoroutine(climbingStairs.ClimbDownStairs(stair, onComplete));
                }
            }
            else
            {
                Debug.LogError("적절한 계단을 찾을 수 없습니다.");
            }
        }
        else
        {
            MoveToFinalPosition(newRoomInfo);
        }
    }
    // 최종 목적지로 플레이어를 이동시키는 메소드
    private void MoveToFinalPosition(RoomInfo roomInfo)
    {
        Vector3 targetPosition = CalculateTargetPosition(roomInfo);
        playerAni.MoveToPosition(targetPosition);
        currentRoomInfo = roomInfo;
        Debug.Log("방으로 이동 완료: " + roomInfo.RoomNumber);
        playerTransform.eulerAngles = new Vector3(
            playerTransform.eulerAngles.x, 
            targetYRotation, 
            playerTransform.eulerAngles.z);
    }

    // 방 위치를 계산하는 메소드
    private Vector3 CalculateTargetPosition(RoomInfo roomInfo)
    {
        // 현재 층 정보가 없는 경우 (처음 이동하는 경우)
        if (currentRoomInfo == null)
        {
            targetYRotation = 180;
            isPoint1 = true;
            return roomInfo.point1.position;
        }
        // 현재 층의 홀수/짝수 여부에 따라 다르게 처리
        bool isEvenFloor = currentRoomInfo.CurrenFloor % 2 == 0;

        // 같은 층 내에서의 방 이동 로직
        if (currentRoomInfo.CurrenFloor == roomInfo.CurrenFloor)
        {
            if (isEvenFloor) // 짝수 층
            {
                if (currentRoomInfo.RoomNumber < roomInfo.RoomNumber)
                {
                    targetYRotation = 0;
                    isPoint1 = false;
                    return roomInfo.point2.position;
                }
                else
                {
                    targetYRotation = 180;
                    isPoint1 = true;
                    return roomInfo.point1.position;
                }
            }
            else // 홀수 층
            {
                if (currentRoomInfo.RoomNumber < roomInfo.RoomNumber)
                {
                    targetYRotation = 180;
                    isPoint1 = true;
                    return roomInfo.point1.position;
                }
                else
                {
                    targetYRotation = 0;
                    isPoint1 = false;
                    return roomInfo.point2.position;
                }
            }
        }
        else // 다른 층으로 이동하는 경우
        {
            if (isEvenFloor)//현재 층이 짝수//홀수 층으로 올라갈 때//방 번호가 커짐
            {
                if(currentRoomInfo.RoomNumber < roomInfo.RoomNumber)
                {
                    targetYRotation = 180;
                    isPoint1 = true;
                    return roomInfo.point1.position;
                }
                else//홀수 층으로 내려갈 때//방 번호가 작아짐
                {
                    targetYRotation = 0;
                    isPoint1 = false;
                    return roomInfo.point2.position;
                } 
            }
            else
            {//현재 층이 홀수//짝수 층으로 올라갈 때//방 번호가 커짐
                if (currentRoomInfo.RoomNumber < roomInfo.RoomNumber)
                {
                    targetYRotation = 0;
                    isPoint1 = false;
                    return roomInfo.point2.position;
                }
                else
                {
                    targetYRotation = 180;
                    isPoint1 = true;
                    return roomInfo.point1.position;
                }
            }
        }
    }
}

