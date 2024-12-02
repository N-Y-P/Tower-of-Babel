using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� ��ġ�� ��������
    public PlayerAni playerAni; //�̵� �� �ִϸ��̼�
    public ClimbingStairs climbingStairs; // ��� �̵� ����
    private RoomInfo currentRoomInfo; // ���� �÷��̾ ��ġ�� �� ���� ����
    public CameraMove cameraMove;
    float targetYRotation = 0;

    // ���� �÷��̾��� �� ������ ��ȯ�ϴ� �޼���
    public RoomInfo GetCurrentRoomInfo()
    {
        return currentRoomInfo;
    }
    public bool isPoint1;
    public Transform PlayerTransform { 
        get { return playerTransform; }
        private set { playerTransform = value; }
    }

    // �� Ŭ�� �� �� ���� point�� �̵��ϴ� �޼ҵ�
    public void MovePlayerToRoom(GameObject room)
    {
        RoomInfo newRoomInfo = room.GetComponent<RoomInfo>();
        if (newRoomInfo == null)
        {
            Debug.LogError("RoomInfo ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        if (currentRoomInfo != null && currentRoomInfo.RoomNumber == newRoomInfo.RoomNumber)
        {
            Debug.Log("�̹� �ش� �濡 �ֽ��ϴ�: Room " + newRoomInfo.RoomNumber);
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
                Debug.LogError("������ ����� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            MoveToFinalPosition(newRoomInfo);
        }
    }
    // ���� �������� �÷��̾ �̵���Ű�� �޼ҵ�
    private void MoveToFinalPosition(RoomInfo roomInfo)
    {
        Vector3 targetPosition = CalculateTargetPosition(roomInfo);
        playerAni.MoveToPosition(targetPosition);
        currentRoomInfo = roomInfo;
        Debug.Log("������ �̵� �Ϸ�: " + roomInfo.RoomNumber);
        playerTransform.eulerAngles = new Vector3(
            playerTransform.eulerAngles.x, 
            targetYRotation, 
            playerTransform.eulerAngles.z);
    }

    // �� ��ġ�� ����ϴ� �޼ҵ�
    private Vector3 CalculateTargetPosition(RoomInfo roomInfo)
    {
        // ���� �� ������ ���� ��� (ó�� �̵��ϴ� ���)
        if (currentRoomInfo == null)
        {
            targetYRotation = 180;
            isPoint1 = true;
            return roomInfo.point1.position;
        }
        // ���� ���� Ȧ��/¦�� ���ο� ���� �ٸ��� ó��
        bool isEvenFloor = currentRoomInfo.CurrenFloor % 2 == 0;

        // ���� �� �������� �� �̵� ����
        if (currentRoomInfo.CurrenFloor == roomInfo.CurrenFloor)
        {
            if (isEvenFloor) // ¦�� ��
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
            else // Ȧ�� ��
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
        else // �ٸ� ������ �̵��ϴ� ���
        {
            if (isEvenFloor)//���� ���� ¦��//Ȧ�� ������ �ö� ��//�� ��ȣ�� Ŀ��
            {
                if(currentRoomInfo.RoomNumber < roomInfo.RoomNumber)
                {
                    targetYRotation = 180;
                    isPoint1 = true;
                    return roomInfo.point1.position;
                }
                else//Ȧ�� ������ ������ ��//�� ��ȣ�� �۾���
                {
                    targetYRotation = 0;
                    isPoint1 = false;
                    return roomInfo.point2.position;
                } 
            }
            else
            {//���� ���� Ȧ��//¦�� ������ �ö� ��//�� ��ȣ�� Ŀ��
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

