using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("좌표")]
	public Transform [] Pos; //  좌표

	private Player player;				// player 스크립트
	private PlayerAction playerAction;	// PlayerAction 스크립트
	private ItemManager itemManager;	// 아이템 매니저 스크립트
	private GoHomeManager goHomeManager;

	private void Start()
	{
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>(); // PlayerAction 스크립트 찾기
		itemManager = FindObjectOfType<ItemManager>();                          // 아이템 매니저 스크립트 찾기
		player = playerAction.GetComponent<Player>().GetComponent<Player>();
		goHomeManager = FindObjectOfType<GoHomeManager>();
	}

	// 플레이어와 충돌시
	private void OnTriggerEnter2D(Collider2D collider)
	{
		// Ensure playerAction is assigned
		if (playerAction == null)
		{
			//Debug.LogError("playerAction is not assigned!");
			return;
		}

		// Ensure Pos array is valid
		if (Pos == null || Pos.Length == 0)
		{
			Debug.LogError("Pos array is not initialized or empty!");
			return;
		}

		foreach (var pos in Pos)
		{
			if (pos == null)
			{
				Debug.LogError("An element in the Pos array is null!");
				return;
			}
		}

		// Not attacking
		if (!playerAction.isAtking)
		{
			if (collider.CompareTag("Player"))
			{
				if (Pos.Length >= 2) // Two or more positions
				{
					int RANDOM_NUMBER = Random.Range(0, Pos.Length); // Random index within bounds
					collider.transform.position = Pos[RANDOM_NUMBER].position;
				}
				else // Single position
				{
					MovePlayer(collider);
				}
			}
		}
		else // Attacking logic
		{
			// Your logic for when attacking
		}
	}


	public void MovePlayer(Collider2D collider)
    {
		collider.transform.position = Pos[0].position;
		//Debug.Log("다음 라운드로 이동");
		Player.gameRound++;
		PrefabSpawner.alreadySpawnedEnemies = false;	// 다음 라운드로 문을 열었으니 몬스터 소환이 가능해집니다.
		itemManager.RoundUp();                          // 라운드 기회 1소모 (단, 버프중 일때)

		if(itemManager.isNextRoundHpUp == true)
		{
			itemManager.HpUP();
		}

		if(itemManager.isCrunchMode == true)
		{
			itemManager.CrunchModeEye();
		}

		goHomeManager.isRoundStart = true;
	}
}