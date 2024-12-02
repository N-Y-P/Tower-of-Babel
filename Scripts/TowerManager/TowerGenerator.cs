using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public static List<int> AngelRoomNumbers { get; private set; }


    public GameObject[] LowerroomPrefabs; // 방 프리팹 배열
    public GameObject angelRoomPrefab; // 천사방 프리팹
    public List<int> angelRoomNumbers; // 천사방이 생성될 방 번호 목록
    public int floors = 66;          // 층 수
    public float roomSpacing = 1.0f;  // 방 사이의 추가 간격
    public float roomup = 2.9f;  // 방 사이의 추가 간격
    int roomnum = 0;
    public EnemySpawner enemySpawner; // 적 생성
    Vector3 lastRoomPosition; // 이전 층의 마지막 방의 위치 저장

    void Awake()
    {
        AngelRoomNumbers = angelRoomNumbers; // 천사방 번호 목록을 정적 프로퍼티에 할당
    }

    void Start()
    {
        GenerateTower();
    }
    void GenerateTower()
    {
        for (int floor = 0; floor < floors; floor++)
        {
            int roomsPerFloor = RoomsPerFloor(floor); // 층별 방의 수 결정
            bool isDirectionReversed = floor % 2 != 0; // 방향 반전

            Vector3 startPosition = lastRoomPosition; // 층 시작 위치는 이전 층의 마지막 방 위치

            if (floor > 0 && !isDirectionReversed)
            {
                startPosition.x -= (roomsPerFloor - 1) * roomSpacing; // 새 층 시작 위치 조정
            }

            for (int room = 0; room < roomsPerFloor; room++)
            {
                int index = isDirectionReversed ? room : roomsPerFloor - 1 - room;
                Vector3 position = new Vector3(startPosition.x + index * roomSpacing, floor * roomup, 0);
                roomnum++;

                GameObject roomPrefab = SelectRoomPrefab(roomnum); // 천사방 또는 일반 방 선택
                GameObject roomInstance = Instantiate(roomPrefab, position, Quaternion.identity);

                RoomInfo roomInfo = roomInstance.GetComponent<RoomInfo>() ?? roomInstance.AddComponent<RoomInfo>();
                roomInfo.RoomNumber = roomnum;
                roomInfo.CurrenFloor = floor + 1;
                if (!angelRoomNumbers.Contains(roomnum))// 천사방이 아닌 경우에만 적 생성
                {
                    if (roomInfo.Enemy != null) // 방 프리팹에 설정된 Enemy 위치가 존재하는지 확인
                    {
                        GameObject enemy = enemySpawner.SpawnEnemy(roomInfo.Enemy, floor + 1);  
                        // Enemy 위치를 인자로 전달
                        if (enemy != null)
                        {
                            EnemyAni enemyAni = enemy.GetComponent<EnemyAni>();
                            if (enemyAni != null)
                            {
                                enemyAni.roomNum = roomnum; // 적이 생성된 방 번호 설정
                                roomInfo.hasEnemy = true;  // 방에 적이 존재함을 표시
                            }
                        }
                    }
                }

                if (room == roomsPerFloor - 1)
                {
                    lastRoomPosition = position; // 마지막 방 위치 업데이트
                }
            }
        }

    }
    // 방 번호에 따라 프리팹 선택
    GameObject SelectRoomPrefab(int roomNumber)
    {
        if (angelRoomNumbers.Contains(roomNumber))
        {
            return angelRoomPrefab; // 천사방 프리팹 반환
        }
        else
        {
            return LowerroomPrefabs[Random.Range(0, LowerroomPrefabs.Length)]; // 일반 방 프리팹 무작위 선택
        }
    }
    int RoomsPerFloor(int floor)
    {
        if (floor < 20) return 5;
        else if (floor < 40) return 4;
        else return 3;
    }
}
