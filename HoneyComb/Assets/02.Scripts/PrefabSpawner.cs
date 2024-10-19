using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	// 소환할 프리팹
	public GameObject prefab;
	public GameObject player;
	public Transform playerPos;

	// 몇초 후 소환 할껀지
	public float sec = 3.0f;

	// 소환할 개수
	public int spawnCount = 5;

	// 소환할 위치들
	public Transform[] Pos;

	// 소환 간 간격 (원하는 경우 설정 가능)
	public float spawnOffset = 1.0f;

	public float checkDistance = 0.1f;  // 허용할 거리 오차

	private bool isSpawnned = false;
	void Start()
	{
		//SpawnPrefabs();
	}

	private void Update()
	{
		// Pos[0]과 player 사이의 거리가 일정 범위 안에 있는지 확인
		if (Vector3.Distance(playerPos.position, Pos[0].position) < checkDistance)
		{
			if(isSpawnned == false)
			{
				isSpawnned = true;
				Debug.Log("Player가 Pos[0] 위치에 있습니다!");
				StartCoroutine(DelayedSpawn(sec));

			}
			// 원하는 작업을 이곳에 추가
		}
	}

	IEnumerator DelayedSpawn(float delay)
	{
		// 3초 기다림
		yield return new WaitForSeconds(delay);

		// 프리팹 소환 함수 호출
		SpawnPrefabs();
	}


	// 프리팹을 소환하는 함수
	public void SpawnPrefabs()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			// 소환할 위치를 순환하며 계산 (배열 크기보다 더 많은 개수를 소환하려는 경우를 대비)
			Transform spawnTransform = Pos[i % Pos.Length];

			// 소환할 위치 계산 (Pos 배열의 위치에서 X축으로 offset을 더함)
			Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0);

			// 프리팹 소환
			Instantiate(prefab, position, Quaternion.identity);
		}
	}
}
