using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("��ǥ")]
	public Transform [] Pos; // �̵��ϰԵ� ��ǥ

	private PlayerAction playerAction;							// PlayerAction ��ũ��Ʈ
	private ItemManager itemManager;							// ItemManager ��ũ��Ʈ
	private GoHomeManager goHomeManager;						// GoHomeManager ��ũ��Ʈ
	private DirectingCameraManager directingCameraManager;      // DirectingCameraManager ��ũ��Ʈ
	private PrefabSpawner prefabSpawner;                        // PrefabSpawner ��ũ��Ʈ
	

	[Header("�� Ŭ���߾�?")]
	public bool isAlreadyClicked = false;

	private void Start()
	{
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>(); // PlayerAction ��ũ��Ʈ ��������
		itemManager = FindObjectOfType<ItemManager>();                          // ItemManager ��ũ��Ʈ ��������
		goHomeManager = FindObjectOfType<GoHomeManager>();                      // GoHomeManager ��ũ��Ʈ ��������	
		directingCameraManager = FindObjectOfType<DirectingCameraManager>();    // DirectingCameraManager ��ũ��Ʈ ��������
		prefabSpawner = FindObjectOfType<PrefabSpawner>();                      // PrefabSpawner ��ũ��Ʈ ��������
	}

	// �÷��̾�� �浹��
	private void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("ħ��� ����� ��");

		// playerAction ��ũ��Ʈ�� �������
		if (playerAction == null)
		{
			return;
		}

		// Pos ����ġ�� ���ҽ�
		if (Pos == null || Pos.Length == 0)
		{
			Debug.LogError("��ġ�� �Ҵ���� �ʾҽ��ϴ�.");
			return;
		}

		// Pos ����ġ�� ���ҽ�22
		foreach (var pos in Pos)
		{
			if (pos == null)
			{
				Debug.LogError("��ġ�� �Ҵ���� �ʾҽ��ϴ�.");
				return;
			}
		}

		// �÷��̾ ���ݿ��ΰ� False �Ͻ�
		if (!playerAction.isAtking)
		{
			// �÷��̾� �ݶ��̴��� �浹��
			if (collider.CompareTag("Player"))
			{
				// ��ġ ��ǥ�� 2�� �̻��Ͻ�
				if (Pos.Length >= 2)
				{
					// ���� �ϳ� �������� �Ѿ�ϴ�.
					int RANDOM_NUMBER = Random.Range(0, Pos.Length);
					collider.transform.position = Pos[RANDOM_NUMBER].position;
				}
				// ��ġ ��ǥ�� 1�� �Ͻ�
				else 
				{
					// �÷��̾ �̵���ŵ�ϴ�.
					MovePlayer(collider);
				}
			}
		}
		// �÷��̾� ���ݿ��ΰ� True �Ͻ�
		else
		{
		}
	}


	// �÷��̾� �̵� �Լ�
	public void MovePlayer(Collider2D collider)
	{
        if (!isAlreadyClicked)
        {
			// Ŭ�� ���� True
			isAlreadyClicked = true;

			// Zoom-in ���� ���� �ڷ���Ʈ ����
			StartCoroutine(ZoomAndTeleport(collider));
		}
	}

	// Zoom-in ���� ���� �ڷ���Ʈ �ڷ�ƾ
	private IEnumerator ZoomAndTeleport(Collider2D collider)
	{
		// Zoom-in ���� (1�� ���)
		directingCameraManager.ZoomIn();
		yield return new WaitForSeconds(1f);

		directingCameraManager.darkBg_On();

		yield return new WaitForSeconds(0.5f);

		// �÷��̾� ����
		collider.transform.position = Pos[0].position;  // �÷��̾� �ش� ��ġ�� ����

		// Ŭ�� ���� False
		isAlreadyClicked = false;

		Player.gameRound++;
		prefabSpawner.isSpawnned = false;    // ���� ����� ���� �������� ���� ��ȯ�� ���������ϴ�.
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