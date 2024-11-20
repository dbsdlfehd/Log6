using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("좌표")]
	public Transform [] Pos; //  좌표

	// 플레이어와 충돌시
	private void OnTriggerEnter2D(Collider2D other)
    {

		if (Pos.Length>=2) // 좌표가 2개 이상일 경우 //
		{
			if (other.CompareTag("Player"))
			{
				int RANDOM_NUMBER = Random.Range(0, 2); // 랜덤 함수 발동
				other.transform.position = Pos[RANDOM_NUMBER].position;
			}
		}
		else				// 좌표가 1개일 경우 //
		{
			if (other.CompareTag("Player"))
			{
				other.transform.position = Pos[0].position;
				Debug.Log("다음 라운드로 이동");
				Player.gameRound++;
				PrefabSpawner.alreadySpawnedEnemies = false; // 다음 라운드로 문을 열었으니 몬스터 소환이 가능해집니다.
			}
		}
    }
}