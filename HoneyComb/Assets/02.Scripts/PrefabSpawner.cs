using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject prefab;			// 생성할 적 프리펩
	public GameObject player;
	public Transform playerPos;



	public TalkManager talkManager;

	public RoomGenerator roomGenerator;


	[Header("몇 라운드 이상 상점?")]
	public int OverRoom;

	[Header("상점 TP 객체")]
	public GameObject StorePos;		// 상점 위치들

	[Header("아이템들")]
	public GameObject[] items;			// 공격, 최대 체력, 재화, 구슬

	public float sec = 3.0f;			// 몇초 후 소환 할껀지

	[Header("적 몇마리 소환")]
	public int spawnCount = 5;			// 소환할 적의 수

	[Header("소환할 위치")]
	public Transform[] Pos;             // 소환할 위치들

	[Header("소환할 위치 오브젝트들")]
	public GameObject[] PosObj;			// 소환할 위치 오브젝트들

	public GameObject[] TP;				// 텔레포트 들	

	public float spawnOffset = 1.0f;	// 소환 간 간격 (원하는 경우 설정 가능)
	public float checkDistance = 0.1f;  // 허용할 거리 오차
	public bool isSpawnned = false;
	public int RoomEnemyCount = 0;

	static public bool alreadySpawnedEnemies = false; // 몬스터 소환 조건


	int temp_i = 0;
	private void Update()
	{
		// 적 전부 처치시 다음으로 갈 랜덤 문 생성
		if(RoomEnemyCount == spawnCount)
		{
			// 모든 텔포 생성
			//         foreach (var tp in TP)
			//         {
			//	tp.SetActive(true);
			//}
			isSpawnned = false;
			RoomEnemyCount = 0;

			// 플레이어의 라운드 진행 수가 5 또는 5 이상 일 때
			if (Player.gameRound == OverRoom || Player.gameRound > OverRoom)
			{
				// 상점 가는 텔포 생성
				Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // 첫 번째 객체의 왼쪽으로 1.5 단위 이동
				Instantiate(StorePos, leftPosition, Quaternion.identity);		   // 상점 텔포를 생성 좌표 기준 왼쪽에 추가
			}
			else
			{
				roomGenerator.RandomDoorGenerate(temp_i); // 랜덤 문 생성
			}


			// 아이템 생성 (프리펩 아이템, 생성될 위치, 회전값)
			Instantiate(items[temp_i], Pos[temp_i].position, Quaternion.identity);

			//Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // 첫 번째 객체의 왼쪽으로 1.5 단위 이동
			//Instantiate(items[0], leftPosition, Quaternion.identity); // 생명령 기본 지급 

		}

		// 플레이어가 해당 방에 올시 적 소환 && 라운드가 진행중이 아니라 플레이어가 다음라운드를 선택했을 때
		for (int i = 0; i < Pos.Length; i++ )
		{
			if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !alreadySpawnedEnemies)
			{
				temp_i = i;			// 위치 저장용
				SpawnEnemies(i);
			}
		}
	}

	public void HideTP() // 다시 텔레포트를 숨기는 함수
    {
		foreach (var tp in TP)
		{
			tp.SetActive(false);
		}
	}

	public void SpawnEnemies(int room) // 적 소환 함수
	{
		// 한번만 실행용 if문
		if (isSpawnned == false)
		{
			alreadySpawnedEnemies = true;
			roomGenerator.DestroyDoor(); // 생성된 문 삭제
			isSpawnned = true;
			StartCoroutine(DelayedSpawn(sec,room));
			//Debug.Log("던전 진입시 주인공 독백 시작");
			talkManager.SoloTalk(); //던전 진입 상황 = 0
		}
	}
	
	IEnumerator DelayedSpawn(float delay, int room) // 기다렸다가 적 소환해주는 시간차 함수
	{
		// 3초 기다림
		yield return new WaitForSeconds(delay);

		// 프리팹 소환 함수 호출
		SpawnPrefabs(room);
	}

	public void SpawnPrefabs(int room) // 좌표에 적 소환
	{
		for (int i = 0; i < spawnCount; i++)
		{
			// 적을 소환할 위치
			Transform spawnTransform = Pos[room];

			// 소환할 위치 계산 (Pos 배열의 위치에서 X축으로 offset을 더함)
			Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0);

			// 프리팹 소환
			Instantiate(prefab, position, Quaternion.identity);
		}
	}
}
