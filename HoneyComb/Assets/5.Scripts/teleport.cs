using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("좌표")]
	public Transform [] Pos; //  좌표

	private Player player;				// player 스크립트 변수
	private PlayerAction playerAction;  // PlayerAction 스크립트 변수
	private ItemManager itemManager;    // 아이템 매니저 스크립트 변수
	private GoHomeManager goHomeManager;
	private DirectingCameraManager directingCameraManager;

	private void Start()
	{
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>(); // PlayerAction 스크립트 가져오기
		itemManager = FindObjectOfType<ItemManager>();                          // ItemManager 스크립트 가져오기
		player = playerAction.GetComponent<Player>().GetComponent<Player>();    // Player 스크립트 가져오기
		goHomeManager = FindObjectOfType<GoHomeManager>();
		directingCameraManager = FindObjectOfType<DirectingCameraManager>();
	}

	// 플레이어와 충돌시
	private void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("침대와 닿았을 시");

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
		// Zoom-in 연출 시작
		StartCoroutine(ZoomAndTeleport(collider));
	}

	private IEnumerator ZoomAndTeleport(Collider2D collider)
	{
		// Zoom-in 연출 (1초 대기)
		directingCameraManager.ZoomIn();
		yield return new WaitForSeconds(1f);

		directingCameraManager.darkBg_On();

		yield return new WaitForSeconds(0.5f);

		// 플레이어 텔포
		collider.transform.position = Pos[0].position;  // 플레이어 해당 위치로 전송
		Player.gameRound++;
		PrefabSpawner.alreadySpawnedEnemies = false;    // 다음 라운드로 문을 열었으니 몬스터 소환이 가능해집니다.
		itemManager.RoundUp();                          // 라운드 기회 1소모 (단, 버프중 일때)

		if (itemManager.isNextRoundHpUp == true)
		{
			itemManager.HpUP();
		}

		if (itemManager.isCrunchMode == true)
		{
			itemManager.CrunchModeEye();
		}

		goHomeManager.isRoundStart = true;

		directingCameraManager.darkBg_Off();

		directingCameraManager.ZoomOut();
	}

}