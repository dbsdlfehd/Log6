using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject prefab;			// ������ �� ������
	public GameObject player;
	public Transform playerPos;



	public TalkManager talkManager;

	public RoomGenerator roomGenerator;


	[Header("�� ���� �̻� ����?")]
	public int OverRoom;

	[Header("���� TP ��ü")]
	public GameObject StorePos;		// ���� ��ġ��

	[Header("�����۵�")]
	public GameObject[] items;			// ����, �ִ� ü��, ��ȭ, ����

	public float sec = 3.0f;			// ���� �� ��ȯ �Ҳ���

	[Header("�� ��� ��ȯ")]
	public int spawnCount = 5;			// ��ȯ�� ���� ��

	[Header("��ȯ�� ��ġ")]
	public Transform[] Pos;             // ��ȯ�� ��ġ��

	[Header("��ȯ�� ��ġ ������Ʈ��")]
	public GameObject[] PosObj;			// ��ȯ�� ��ġ ������Ʈ��

	public GameObject[] TP;				// �ڷ���Ʈ ��	

	public float spawnOffset = 1.0f;	// ��ȯ �� ���� (���ϴ� ��� ���� ����)
	public float checkDistance = 0.1f;  // ����� �Ÿ� ����
	public bool isSpawnned = false;
	public int RoomEnemyCount = 0;

	static public bool alreadySpawnedEnemies = false; // ���� ��ȯ ����


	int temp_i = 0;
	private void Update()
	{
		// �� ���� óġ�� �������� �� ���� �� ����
		if(RoomEnemyCount == spawnCount)
		{
			// ��� ���� ����
			//         foreach (var tp in TP)
			//         {
			//	tp.SetActive(true);
			//}
			isSpawnned = false;
			RoomEnemyCount = 0;

			// �÷��̾��� ���� ���� ���� 5 �Ǵ� 5 �̻� �� ��
			if (Player.gameRound == OverRoom || Player.gameRound > OverRoom)
			{
				// ���� ���� ���� ����
				Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // ù ��° ��ü�� �������� 1.5 ���� �̵�
				Instantiate(StorePos, leftPosition, Quaternion.identity);		   // ���� ������ ���� ��ǥ ���� ���ʿ� �߰�
			}
			else
			{
				roomGenerator.RandomDoorGenerate(temp_i); // ���� �� ����
			}


			// ������ ���� (������ ������, ������ ��ġ, ȸ����)
			Instantiate(items[temp_i], Pos[temp_i].position, Quaternion.identity);

			//Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // ù ��° ��ü�� �������� 1.5 ���� �̵�
			//Instantiate(items[0], leftPosition, Quaternion.identity); // ����� �⺻ ���� 

		}

		// �÷��̾ �ش� �濡 �ý� �� ��ȯ && ���尡 �������� �ƴ϶� �÷��̾ �������带 �������� ��
		for (int i = 0; i < Pos.Length; i++ )
		{
			if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !alreadySpawnedEnemies)
			{
				temp_i = i;			// ��ġ �����
				SpawnEnemies(i);
			}
		}
	}

	public void HideTP() // �ٽ� �ڷ���Ʈ�� ����� �Լ�
    {
		foreach (var tp in TP)
		{
			tp.SetActive(false);
		}
	}

	public void SpawnEnemies(int room) // �� ��ȯ �Լ�
	{
		// �ѹ��� ����� if��
		if (isSpawnned == false)
		{
			alreadySpawnedEnemies = true;
			roomGenerator.DestroyDoor(); // ������ �� ����
			isSpawnned = true;
			StartCoroutine(DelayedSpawn(sec,room));
			//Debug.Log("���� ���Խ� ���ΰ� ���� ����");
			talkManager.SoloTalk(); //���� ���� ��Ȳ = 0
		}
	}
	
	IEnumerator DelayedSpawn(float delay, int room) // ��ٷȴٰ� �� ��ȯ���ִ� �ð��� �Լ�
	{
		// 3�� ��ٸ�
		yield return new WaitForSeconds(delay);

		// ������ ��ȯ �Լ� ȣ��
		SpawnPrefabs(room);
	}

	public void SpawnPrefabs(int room) // ��ǥ�� �� ��ȯ
	{
		for (int i = 0; i < spawnCount; i++)
		{
			// ���� ��ȯ�� ��ġ
			Transform spawnTransform = Pos[room];

			// ��ȯ�� ��ġ ��� (Pos �迭�� ��ġ���� X������ offset�� ����)
			Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0);

			// ������ ��ȯ
			Instantiate(prefab, position, Quaternion.identity);
		}
	}
}
