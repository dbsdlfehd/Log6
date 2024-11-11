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


	public float sec = 3.0f;			// ���� �� ��ȯ �Ҳ���

	[Header("�� ��� ��ȯ")]
	public int spawnCount = 5;			// ��ȯ�� ���� ��

	[Header("��ȯ�� ��ġ")]
	public Transform[] Pos;				// ��ȯ�� ��ġ��

	public GameObject[] TP;				// �ڷ���Ʈ	

	public float spawnOffset = 1.0f;	// ��ȯ �� ���� (���ϴ� ��� ���� ����)
	public float checkDistance = 0.1f;  // ����� �Ÿ� ����
	public bool isSpawnned = false;
	public int RoomEnemyCount = 0;

	private void Update()
	{
		// �� ���� �� ���� óġ��
		if(RoomEnemyCount == spawnCount)
		{
            foreach (var tp in TP)
            {
				tp.SetActive(true);
			}
			isSpawnned = false;
			RoomEnemyCount = 0;
        }



		// room 1-1 ��ȯ
		if (Vector3.Distance(playerPos.position, Pos[0].position) < checkDistance)
		{
			SpawnEnemies(0);
		}

		//room 1-2 ��ȯ
		if(Vector3.Distance(playerPos.position, Pos[1].position) < checkDistance)
		{
			SpawnEnemies(1);
		}

		//room 2-1 ��ȯ
		if (Vector3.Distance(playerPos.position, Pos[2].position) < checkDistance)
		{
			SpawnEnemies(2);
		}

		//room 2-2 ��ȯ
		if (Vector3.Distance(playerPos.position, Pos[3].position) < checkDistance)
		{
			SpawnEnemies(3);
		}
	}

	public void HideTP()
    {
		foreach (var tp in TP)
		{
			tp.SetActive(false);
		}
	}

	public void SpawnEnemies(int room)
	{
		// �ѹ��� ����� if��
		if(isSpawnned == false)
		{
			isSpawnned = true;
			StartCoroutine(DelayedSpawn(sec,room));
			Debug.Log("���� ���Խ� ���ΰ� ���� ����");
			talkManager.SoloTalk(0); //���� ���� ��Ȳ = 0 
		}
	}
	
	IEnumerator DelayedSpawn(float delay, int room)
	{
		// 3�� ��ٸ�
		yield return new WaitForSeconds(delay);

		// ������ ��ȯ �Լ� ȣ��
		SpawnPrefabs(room);
	}


	// �������� ��ȯ�ϴ� �Լ�
	public void SpawnPrefabs(int room)
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
