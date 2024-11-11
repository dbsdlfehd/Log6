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


	public float sec = 3.0f;			// 몇초 후 소환 할껀지

	[Header("적 몇마리 소환")]
	public int spawnCount = 5;			// 소환할 적의 수

	[Header("소환할 위치")]
	public Transform[] Pos;				// 소환할 위치들

	public GameObject[] TP;				// 텔레포트	

	public float spawnOffset = 1.0f;	// 소환 간 간격 (원하는 경우 설정 가능)
	public float checkDistance = 0.1f;  // 허용할 거리 오차
	public bool isSpawnned = false;
	public int RoomEnemyCount = 0;

	private void Update()
	{
		// 방 내에 적 전부 처치시
		if(RoomEnemyCount == spawnCount)
		{
            foreach (var tp in TP)
            {
				tp.SetActive(true);
			}
			isSpawnned = false;
			RoomEnemyCount = 0;
        }



		// room 1-1 소환
		if (Vector3.Distance(playerPos.position, Pos[0].position) < checkDistance)
		{
			SpawnEnemies(0);
		}

		//room 1-2 소환
		if(Vector3.Distance(playerPos.position, Pos[1].position) < checkDistance)
		{
			SpawnEnemies(1);
		}

		//room 2-1 소환
		if (Vector3.Distance(playerPos.position, Pos[2].position) < checkDistance)
		{
			SpawnEnemies(2);
		}

		//room 2-2 소환
		if (Vector3.Distance(playerPos.position, Pos[3].position) < checkDistance)
		{
			SpawnEnemies(3);
		}
	}

	public void HideTP()
    {
		foreach (var tp in TP)
		{
			tp.SetActive(false);
		}
	}

	public void SpawnEnemies(int room)
	{
		// 한번만 실행용 if문
		if(isSpawnned == false)
		{
			isSpawnned = true;
			StartCoroutine(DelayedSpawn(sec,room));
			Debug.Log("던전 진입시 주인공 독백 시작");
			talkManager.SoloTalk(0); //던전 진입 상황 = 0 
		}
	}
	
	IEnumerator DelayedSpawn(float delay, int room)
	{
		// 3초 기다림
		yield return new WaitForSeconds(delay);

		// 프리팹 소환 함수 호출
		SpawnPrefabs(room);
	}


	// 프리팹을 소환하는 함수
	public void SpawnPrefabs(int room)
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
