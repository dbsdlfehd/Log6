using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	// ��ȯ�� ������
	public GameObject prefab;
	public GameObject player;
	public Transform playerPos;

	// ���� �� ��ȯ �Ҳ���
	public float sec = 3.0f;

	// ��ȯ�� ����
	public int spawnCount = 5;

	// ��ȯ�� ��ġ��
	public Transform[] Pos;

	// ��ȯ �� ���� (���ϴ� ��� ���� ����)
	public float spawnOffset = 1.0f;

	public float checkDistance = 0.1f;  // ����� �Ÿ� ����

	private bool isSpawnned = false;
	void Start()
	{
		//SpawnPrefabs();
	}

	private void Update()
	{
		// Pos[0]�� player ������ �Ÿ��� ���� ���� �ȿ� �ִ��� Ȯ��
		if (Vector3.Distance(playerPos.position, Pos[0].position) < checkDistance)
		{
			if(isSpawnned == false)
			{
				isSpawnned = true;
				Debug.Log("Player�� Pos[0] ��ġ�� �ֽ��ϴ�!");
				StartCoroutine(DelayedSpawn(sec));

			}
			// ���ϴ� �۾��� �̰��� �߰�
		}
	}

	IEnumerator DelayedSpawn(float delay)
	{
		// 3�� ��ٸ�
		yield return new WaitForSeconds(delay);

		// ������ ��ȯ �Լ� ȣ��
		SpawnPrefabs();
	}


	// �������� ��ȯ�ϴ� �Լ�
	public void SpawnPrefabs()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			// ��ȯ�� ��ġ�� ��ȯ�ϸ� ��� (�迭 ũ�⺸�� �� ���� ������ ��ȯ�Ϸ��� ��츦 ���)
			Transform spawnTransform = Pos[i % Pos.Length];

			// ��ȯ�� ��ġ ��� (Pos �迭�� ��ġ���� X������ offset�� ����)
			Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0);

			// ������ ��ȯ
			Instantiate(prefab, position, Quaternion.identity);
		}
	}
}
