using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;            // 생성할 적 프리팹
    public GameObject player;
    public Transform playerPos;

    public TalkManager talkManager;
    public RoomGenerator roomGenerator;

    [Header("몇 라운드 이상 상점?")]
    public int OverRoom;

    [Header("상점 TP 객체")]
    public GameObject StorePos;         // 상점 위치들
    private GameObject spawnedStoreTP;  // 생성된 상점 객체 참조

    [Header("아이템들")]
    public GameObject[] items;          // 공격, 최대 체력, 재화, 구슬
    private List<GameObject> spawnedItems = new List<GameObject>(); // 생성된 아이템들

    public float sec = 3.0f;             // 몇초 후 소환할 시간

    [Header("적 몇마리 소환")]
    public int spawnCount = 5;           // 소환할 적의 수

    [Header("소환할 위치")]
    public Transform[] Pos;             // 소환할 위치들

    [Header("소환할 위치 오브젝트들")]
    public GameObject[] PosObj;         // 소환할 위치 오브젝트들

    //public GameObject[] TP;             // 텔레포트들

    public float spawnOffset = 1.0f;    // 소환 간 간격
    public float checkDistance = 0.1f;  // 허용할 거리 오차
    public bool isSpawnned = false;
    public int RoomEnemyCount = 0;

    static public bool alreadySpawnedEnemies = false; // 몬스터 소환 조건

    int temp_i = 0;

    private void Update()
    {
        // 적 전부 처치시 다음으로 갈 랜덤 문 생성
        if (RoomEnemyCount == spawnCount)
        {
            isSpawnned = false;
            RoomEnemyCount = 0;

            // 플레이어의 라운드 진행 수가 5 또는 5 이상일 때
            if (Player.gameRound == OverRoom || Player.gameRound > OverRoom)
            {
                // 상점 가는 텔포 생성
                Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // 좌표 기준 왼쪽으로 이동
                spawnedStoreTP = Instantiate(StorePos, leftPosition, Quaternion.identity); // 생성된 텔레포트를 변수에 저장
            }
            else
            {
                roomGenerator.RandomDoorGenerate(temp_i); // 랜덤 문 생성
            }

            // 아이템 생성
            GameObject newItem = Instantiate(items[temp_i], Pos[temp_i].position, Quaternion.identity);
            spawnedItems.Add(newItem); // 생성된 아이템을 리스트에 추가
        }

        // 플레이어가 해당 방에 올시 적 소환 && 라운드가 진행 중이 아니라 플레이어가 다음 라운드를 선택했을 때
        for (int i = 0; i < Pos.Length; i++)
        {
            if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !alreadySpawnedEnemies)
            {
                temp_i = i;         // 위치 저장용
                SpawnEnemies(i);
            }
        }
    }

    //public void HideTP() // 다시 텔레포트를 숨기는 함수
    //{
    //    foreach (var tp in TP)
    //    {
    //        tp.SetActive(false);
    //    }
    //}

    public void SpawnEnemies(int room) // 적 소환 함수
    {
        if (isSpawnned == false) // 한번만 실행용 if문
        {
            alreadySpawnedEnemies = true;
            roomGenerator.DestroyDoor(); // 생성된 문 삭제
            isSpawnned = true;
            StartCoroutine(DelayedSpawn(sec, room));
            talkManager.SoloTalk(); // 던전 진입 상황
        }
    }

    IEnumerator DelayedSpawn(float delay, int room) // 기다렸다가 적 소환해주는 시간차 함수
    {
        yield return new WaitForSeconds(delay);
        SpawnPrefabs(room); // 프리팹 소환 함수 호출
    }

    public void SpawnPrefabs(int room) // 좌표에 적 소환
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnTransform = Pos[room]; // 적을 소환할 위치
            Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0); // 소환할 위치 계산
            Instantiate(prefab, position, Quaternion.identity); // 프리팹 소환
        }
    }

    public void DestroySpawnedObjects() // 생성된 아이템 및 상점 TP 객체 제거
    {
        // 생성된 아이템 제거
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();

        // 생성된 상점 텔레포트 제거
        if (spawnedStoreTP != null)
        {
            Destroy(spawnedStoreTP);
            spawnedStoreTP = null;
        }

        Debug.Log("생성된 아이템과 상점 텔레포트가 제거되었습니다.");
    }
}
