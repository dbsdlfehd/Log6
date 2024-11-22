using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;            // ������ �� ������
    public GameObject player;
    public Transform playerPos;

    public TalkManager talkManager;
    public RoomGenerator roomGenerator;

    [Header("�� ���� �̻� ����?")]
    public int OverRoom;

    [Header("���� TP ��ü")]
    public GameObject StorePos;         // ���� ��ġ��
    private GameObject spawnedStoreTP;  // ������ ���� ��ü ����

    [Header("�����۵�")]
    public GameObject[] items;          // ����, �ִ� ü��, ��ȭ, ����
    private List<GameObject> spawnedItems = new List<GameObject>(); // ������ �����۵�

    public float sec = 3.0f;             // ���� �� ��ȯ�� �ð�

    [Header("�� ��� ��ȯ")]
    public int spawnCount = 5;           // ��ȯ�� ���� ��

    [Header("��ȯ�� ��ġ")]
    public Transform[] Pos;             // ��ȯ�� ��ġ��

    [Header("��ȯ�� ��ġ ������Ʈ��")]
    public GameObject[] PosObj;         // ��ȯ�� ��ġ ������Ʈ��

    //public GameObject[] TP;             // �ڷ���Ʈ��

    public float spawnOffset = 1.0f;    // ��ȯ �� ����
    public float checkDistance = 0.1f;  // ����� �Ÿ� ����
    public bool isSpawnned = false;
    public int RoomEnemyCount = 0;

    static public bool alreadySpawnedEnemies = false; // ���� ��ȯ ����

    int temp_i = 0;

    private void Update()
    {
        // �� ���� óġ�� �������� �� ���� �� ����
        if (RoomEnemyCount == spawnCount)
        {
            isSpawnned = false;
            RoomEnemyCount = 0;

            // �÷��̾��� ���� ���� ���� 5 �Ǵ� 5 �̻��� ��
            if (Player.gameRound == OverRoom || Player.gameRound > OverRoom)
            {
                // ���� ���� ���� ����
                Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // ��ǥ ���� �������� �̵�
                spawnedStoreTP = Instantiate(StorePos, leftPosition, Quaternion.identity); // ������ �ڷ���Ʈ�� ������ ����
            }
            else
            {
                roomGenerator.RandomDoorGenerate(temp_i); // ���� �� ����
            }

            // ������ ����
            GameObject newItem = Instantiate(items[temp_i], Pos[temp_i].position, Quaternion.identity);
            spawnedItems.Add(newItem); // ������ �������� ����Ʈ�� �߰�
        }

        // �÷��̾ �ش� �濡 �ý� �� ��ȯ && ���尡 ���� ���� �ƴ϶� �÷��̾ ���� ���带 �������� ��
        for (int i = 0; i < Pos.Length; i++)
        {
            if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !alreadySpawnedEnemies)
            {
                temp_i = i;         // ��ġ �����
                SpawnEnemies(i);
            }
        }
    }

    //public void HideTP() // �ٽ� �ڷ���Ʈ�� ����� �Լ�
    //{
    //    foreach (var tp in TP)
    //    {
    //        tp.SetActive(false);
    //    }
    //}

    public void SpawnEnemies(int room) // �� ��ȯ �Լ�
    {
        if (isSpawnned == false) // �ѹ��� ����� if��
        {
            alreadySpawnedEnemies = true;
            roomGenerator.DestroyDoor(); // ������ �� ����
            isSpawnned = true;
            StartCoroutine(DelayedSpawn(sec, room));
            talkManager.SoloTalk(); // ���� ���� ��Ȳ
        }
    }

    IEnumerator DelayedSpawn(float delay, int room) // ��ٷȴٰ� �� ��ȯ���ִ� �ð��� �Լ�
    {
        yield return new WaitForSeconds(delay);
        SpawnPrefabs(room); // ������ ��ȯ �Լ� ȣ��
    }

    public void SpawnPrefabs(int room) // ��ǥ�� �� ��ȯ
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnTransform = Pos[room]; // ���� ��ȯ�� ��ġ
            Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0); // ��ȯ�� ��ġ ���
            Instantiate(prefab, position, Quaternion.identity); // ������ ��ȯ
        }
    }

    public void DestroySpawnedObjects() // ������ ������ �� ���� TP ��ü ����
    {
        // ������ ������ ����
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();

        // ������ ���� �ڷ���Ʈ ����
        if (spawnedStoreTP != null)
        {
            Destroy(spawnedStoreTP);
            spawnedStoreTP = null;
        }

        Debug.Log("������ �����۰� ���� �ڷ���Ʈ�� ���ŵǾ����ϴ�.");
    }
}
