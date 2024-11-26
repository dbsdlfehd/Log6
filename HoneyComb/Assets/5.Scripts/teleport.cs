using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("��ǥ")]
	public Transform [] Pos; //  ��ǥ

	private Player player;				// player ��ũ��Ʈ ����
	private PlayerAction playerAction;  // PlayerAction ��ũ��Ʈ ����
	private ItemManager itemManager;    // ������ �Ŵ��� ��ũ��Ʈ ����
	private GoHomeManager goHomeManager;
	private DirectingCameraManager directingCameraManager;

	private void Start()
	{
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>(); // PlayerAction ��ũ��Ʈ ��������
		itemManager = FindObjectOfType<ItemManager>();                          // ItemManager ��ũ��Ʈ ��������
		player = playerAction.GetComponent<Player>().GetComponent<Player>();    // Player ��ũ��Ʈ ��������
		goHomeManager = FindObjectOfType<GoHomeManager>();
		directingCameraManager = FindObjectOfType<DirectingCameraManager>();
	}

	// �÷��̾�� �浹��
	private void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("ħ��� ����� ��");

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
		// Zoom-in ���� ����
		StartCoroutine(ZoomAndTeleport(collider));
	}

	private IEnumerator ZoomAndTeleport(Collider2D collider)
	{
		// Zoom-in ���� (1�� ���)
		directingCameraManager.ZoomIn();
		yield return new WaitForSeconds(1f);

		directingCameraManager.darkBg_On();

		yield return new WaitForSeconds(0.5f);

		// �÷��̾� ����
		collider.transform.position = Pos[0].position;  // �÷��̾� �ش� ��ġ�� ����
		Player.gameRound++;
		PrefabSpawner.alreadySpawnedEnemies = false;    // ���� ����� ���� �������� ���� ��ȯ�� ���������ϴ�.
		itemManager.RoundUp();                          // ���� ��ȸ 1�Ҹ� (��, ������ �϶�)

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