using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("��ǥ")]
	public Transform [] Pos; //  ��ǥ

	// �÷��̾�� �浹��
	private void OnTriggerEnter2D(Collider2D other)
    {

		if (Pos.Length>=2) // ��ǥ�� 2�� �̻��� ��� //
		{
			if (other.CompareTag("Player"))
			{
				int RANDOM_NUMBER = Random.Range(0, 2); // ���� �Լ� �ߵ�
				other.transform.position = Pos[RANDOM_NUMBER].position;
			}
		}
		else				// ��ǥ�� 1���� ��� //
		{
			if (other.CompareTag("Player"))
			{
				other.transform.position = Pos[0].position;
				Debug.Log("���� ����� �̵�");
				Player.gameRound++;
				PrefabSpawner.alreadySpawnedEnemies = false; // ���� ����� ���� �������� ���� ��ȯ�� ���������ϴ�.
			}
		}
    }
}