using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeManager : MonoBehaviour
{
    // Ư�� ��ü�� �����ϱ� ���� ����
    [Header("����")]
    public GameObject targetObject; // ������ ��� ��ü

    [Header("���� ������")]
    public GameObject bossPrefab;   // ���� ������ ���� ������
    public Transform bossSpawnPoint; // ������ ������ ��ġ


    public GameObject GameOverImg;  // ���� ���� ����

    [Header("�� ��ġ")]
    public Transform homePosition;  // �̵��� ��ǥ

    // �÷��̾ ����
    [Header("�÷��̾�")]
    public GameObject player;
    public Player playerScript;

    public bool isPlayerMovable = false;

    void Update()
    {
        // ���� óġ��
        // 1. ��� ��ü�� null(�ı��ǰų� ������ ã�� �� ����) �������� Ȯ��
        if (targetObject == null)
        {
            isPlayerMovable = false;
            // 2. �÷��̾ Ư�� ��ǥ(homePosition)�� �̵�
            MovePlayerToHome();
            SpawnNewBoss();
        }
    }
    public bool isRoundStart = false;

	void MovePlayerToHome()
    {
        if (player != null && homePosition != null && isPlayerMovable == false && isRoundStart == true )
        {

            // ��Ȱ
            StartCoroutine(RespawnPlayerStart());
        }
        else
        {
            //Debug.LogWarning("�÷��̾ HomePosition�� �������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator RespawnPlayerStart()
    {
		//GameOverImg.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		// 2-1. �÷��̾ �ٷ� ��ǥ�� �̵�
		//player.transform.position = homePosition.position;

		// (����������) ȸ���� �̵��� ��ġ�� ȸ���� ����
		//player.transform.rotation = homePosition.rotation;

		//Debug.Log("�÷��̾ ������ ��ǥ�� �̵��߽��ϴ�.");

		// �÷��̾� ���� �ʱ�ȭ 
		//playerScript.StatDefaultPlayer();

		isPlayerMovable = true;

		Debug.Log("������ �������ϴ�.");
		playerScript.RespawnPlayer();
	}

    void SpawnNewBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
			// ���� ����
			GameObject newBoss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            MonsterHP bossHP = newBoss.GetComponent<MonsterHP>();

            if (bossHP != null)
            {
                // MonsterHP ������Ʈ�� Ư�� �Լ� ȣ��
                bossHP.SetDefault(); // Ư���Լ�()�� MonsterHP ��ũ��Ʈ�� �Լ� �̸����� ��ü
            }
            else
            {
                Debug.LogError("MonsterHP ������Ʈ�� ã�� �� �����ϴ�.");
            }


            targetObject = newBoss; // ���ο� ������ targetObject�� ����
		}
        else
        {
            Debug.LogWarning("BossPrefab�̳� BossSpawnPoint�� �������� �ʾҽ��ϴ�.");
        }
    }
}
