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
		// �������� �ƴҶ�
		if (playerAction.isAtking == false)
		{
			if (Pos.Length >= 2) // ��ǥ�� 2�� �̻��� ��� //
			{
				if (collider.CompareTag("Player"))
				{
					int RANDOM_NUMBER = Random.Range(0, 2); // ���� �Լ� �ߵ�
					collider.transform.position = Pos[RANDOM_NUMBER].position;
				}
			}
			else                // ��ǥ�� 1���� ��� //
			{
				if (collider.CompareTag("Player"))
				{
					MovePlayer(collider);
				}
			}
		}
		// �������� ��
		else if (playerAction.isAtking == true)
		{
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