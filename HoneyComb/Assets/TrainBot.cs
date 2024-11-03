using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TrainBot : MonoBehaviour
{
    public GameObject TestBot;
	public GameObject prefab;

	public Transform Pos;// 소환할 위치들

	public void Respawn()
	{
		StartCoroutine(DelayedSpawn(2));
	}
	IEnumerator DelayedSpawn(float delay)
	{
		// delay변수만큼의 초 기다림
		yield return new WaitForSeconds(delay);

		// 프리팹 소환 함수 호출
		SpawnPrefabs();
	}


	void SpawnPrefabs()
	{
		// 소환할 위치를 순환하며 계산 (배열 크기보다 더 많은 개수를 소환하려는 경우를 대비)
		Transform spawnTransform = Pos;

		// 소환할 위치 계산 (Pos 배열의 위치에서 X축으로 offset을 더함)
		Vector3 position = spawnTransform.position + new Vector3(0, 0, 0);

		// 프리팹 소환
		Instantiate(prefab, position, Quaternion.identity);
	}
}
