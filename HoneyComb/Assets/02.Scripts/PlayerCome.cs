using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCome : MonoBehaviour
{
	PrefabSpawner prefabSpawner;
	private void OnTriggerEnter(Collider other)
	{
		// "Player" �±װ� ���� ������Ʈ���� Ȯ��
		if (other.CompareTag("Player"))
		{
			// ������ ��ȯ
			//prefabSpawner.SpawnPrefabs();
		}
	}

}
