using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TrainBot : MonoBehaviour
{
    public GameObject TestBot;
	public GameObject prefab;

	public Transform Pos;// ��ȯ�� ��ġ��

	public void Respawn()
	{
		StartCoroutine(DelayedSpawn(2));
	}
	IEnumerator DelayedSpawn(float delay)
	{
		// delay������ŭ�� �� ��ٸ�
		yield return new WaitForSeconds(delay);

		// ������ ��ȯ �Լ� ȣ��
		SpawnPrefabs();
	}


	void SpawnPrefabs()
	{
		// ��ȯ�� ��ġ�� ��ȯ�ϸ� ��� (�迭 ũ�⺸�� �� ���� ������ ��ȯ�Ϸ��� ��츦 ���)
		Transform spawnTransform = Pos;

		// ��ȯ�� ��ġ ��� (Pos �迭�� ��ġ���� X������ offset�� ����)
		Vector3 position = spawnTransform.position + new Vector3(0, 0, 0);

		// ������ ��ȯ
		Instantiate(prefab, position, Quaternion.identity);
	}
}
