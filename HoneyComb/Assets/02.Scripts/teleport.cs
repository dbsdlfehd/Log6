using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("��ǥ")]
	public Transform [] Pos; //  ��ǥ

	private Player player;				// player ��ũ��Ʈ
	private PlayerAction playerAction;	// PlayerAction ��ũ��Ʈ
	private ItemManager itemManager;	// ������ �Ŵ��� ��ũ��Ʈ
	private GoHomeManager goHomeManager;

	private void Start()
	{
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>(); // PlayerAction ��ũ��Ʈ ã��
		itemManager = FindObjectOfType<ItemManager>();                          // ������ �Ŵ��� ��ũ��Ʈ ã��
		player = playerAction.GetComponent<Player>().GetComponent<Player>();
		goHomeManager = FindObjectOfType<GoHomeManager>();
	}

	// �÷��̾�� �浹��
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
		//Debug.Log("���� ����� �̵�");
		Player.gameRound++;
		PrefabSpawner.alreadySpawnedEnemies = false;	// ���� ����� ���� �������� ���� ��ȯ�� ���������ϴ�.
		itemManager.RoundUp();                          // ���� ��ȸ 1�Ҹ� (��, ������ �϶�)

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