using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeManager : MonoBehaviour
{
    // Ư�� ��ü�� �����ϱ� ���� ����
    [Header("����")]
    public GameObject targetObject; // ������ ��� ��ü
    public GameObject bossPrefab;   // ���� ������ ���� ������
    public Transform bossSpawnPoint; // ������ ������ ��ġ


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

    void MovePlayerToHome()
    {
        if (player != null && homePosition != null && isPlayerMovable == false)
        {
            // 2-1. �÷��̾ �ٷ� ��ǥ�� �̵�
            player.transform.position = homePosition.position;

            // (����������) ȸ���� �̵��� ��ġ�� ȸ���� ����
            player.transform.rotation = homePosition.rotation;

            Debug.Log("�÷��̾ ������ ��ǥ�� �̵��߽��ϴ�.");

            // �÷��̾� ���� �ʱ�ȭ 
            playerScript.SetPlayerDefaultStatus();

            isPlayerMovable = true;

            // ��Ȱ
            playerScript.RespawnPlayer();
        }
        else
        {
            Debug.LogWarning("�÷��̾ HomePosition�� �������� �ʾҽ��ϴ�.");
        }
    }

    void SpawnNewBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            // ���� ����
            GameObject newBoss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            targetObject = newBoss; // ���ο� ������ targetObject�� ����

            Debug.Log("���ο� ������ �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("BossPrefab�̳� BossSpawnPoint�� �������� �ʾҽ��ϴ�.");
        }
    }

}
