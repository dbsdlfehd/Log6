using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;
//using UnityEditor;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;               // ������ �� ������
    public Transform playerPos;             // �÷��̾� ��ġ

    public TalkManager talkManager;         // TalkManager ��ũ��Ʈ
    public RoomGenerator roomGenerator;     // RoomGenerator ��ũ��Ʈ

    [Header("�� ���� �̻� ����?")]
    public int OverRoom;

    [Header("���� �ڷ���Ʈ ������")]
    public GameObject StorePos;

	private List<GameObject> spawnedStoreTP = new List<GameObject>();   // ������ ���� �ڷ���Ʈ ��ü���� ��Ե� ����Ʈ ����

	[Header("���� ������ ������")]
    public GameObject[] reward_item_prf;
    private List<GameObject> spawnedItems = new List<GameObject>();     // ������ ���� ������ ��ü���� ��Ե� ����Ʈ ����

    [Header("���� �� ���� ��ȯ�� ���ΰ�?")]
    public float sec = 3.0f;

    [Header("�� ����� ���� ��� ��ȯ�� ���ΰ�?")]
    public int spawnCount = 5;

    [Header("���ΰ��� ��ȯ�� ��ġ")]
    public Transform[] Pos;

    [Header("���� ��ȯ�� ��ġ")]
    public Transform[] EnemySpawnPos;

    [Header("���� ������ ��ȯ�� ��ġ")]
    public Transform[] RewardItem_Pos;

    [Header("��ȯ�� ��ġ ������Ʈ��")]
    public GameObject[] PosObj;

	[Header("���� �ִٰ� ������ �����ҷ�?")]
	public float docBack_DelayTime;

    [Header("�� ��ȯ�� ���� ���� ����")]
    public float spawnOffset = 1.0f;

    [Header("�� ��ȯ�� ����� �Ÿ� ����")]
    public float checkDistance = 0.1f;  // ����� �Ÿ� ����

    [Header("�̹� �ѹ� ������?")]
    public bool isSpawnned = false;

    [Header("���� óġ�� ���� ���� ��Ÿ���ϴ�.")]
    public int RoomEnemyCount = 0;

    //static public bool alreadySpawnedEnemies = false; // ���� ��ȯ ����

    int temp_i = 0;

    private void Update()
    {
		// �� �濡�� �� ���� óġ��
		if (RoomEnemyCount == spawnCount)
		{
            // ���� �� óġ�� �ʱ�ȭ
            RoomEnemyCount = 0;

            // ���� ���� ����ŭ ���� ��
            if (Player.gameRound == OverRoom || Player.gameRound > OverRoom)
            {
                // ���� ���� ���� ����
                Vector3 leftPosition = Pos[temp_i].position + Vector3.left * 1.5f; // ��ǥ ���� �������� �̵�
                GameObject newItems = Instantiate(StorePos, leftPosition, Quaternion.identity); // ������ �ڷ���Ʈ�� ������ ����
				spawnedStoreTP.Add(newItems);
			}
			// �Ϲ� ���� ���� �� ��
			else
			{
                roomGenerator.RandomDoorGenerate(temp_i); // ���� �� ����
            }

            Debug.Log(temp_i);
            GameObject newItem = Instantiate(reward_item_prf[temp_i], RewardItem_Pos[temp_i].position, Quaternion.identity); // ���� ������ ����(������ ������, ��ȯ�� ��ġ, rotation)
			spawnedItems.Add(newItem);                                                                            // ������ ���� �������� ����Ʈ�� �߰�
		}

		// �÷��̾ �ش� �濡 �ý� && ���尡 ���� ���� �ƴ϶� �÷��̾ ���� ���带 �������� �� && �̹� �ѹ� ������?�� False�Ͻ�
		for (int i = 0; i < Pos.Length; i++)
        {
            if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !isSpawnned)
            {
                temp_i = i;         // ��ġ �����
                SpawnEnemies(i);    // �� ��ȯ
            }
        }
    }

    public void SpawnEnemies(int room) // �� ��ȯ
    {
        // �̹� �ѹ� ������? -> False �Ͻ�
        if (isSpawnned == false)
        {
            roomGenerator.DestroyDoor();                // �����Ǿ��� �� �ı�
            isSpawnned = true;                          // �̹� �ѹ� ������? -> True�� ����
            StartCoroutine(DelayedSpawn(sec, room));    // �� ��ȯ
            StartCoroutine(DocBack());                  // ���ΰ� ���� ����
        }
    }

    IEnumerator DocBack() // ��ٷȴٰ� �������
    {
		yield return new WaitForSeconds(docBack_DelayTime);
		talkManager.SoloTalk(); // ���� ���� ��Ȳ
	}

    IEnumerator DelayedSpawn(float delay, int room) // ��ٷȴٰ� �� ��ȯ���ִ� �ð��� �Լ�
    {
        yield return new WaitForSeconds(delay);     // N�� �ð� ���� (������Ʈ�� Sec �κ�)
        SpawnPrefabs(room);                         // ������ ��ȯ �Լ� ȣ��
    }

    public void SpawnPrefabs(int room) // ��ǥ�� �� ��ȯ
    {
        for (int i = 0; i < spawnCount; i++)
        {
            //Transform spawnTransform = Pos[room];                                               // ���� ��ȯ�� ��ġ
            //Vector3 position = spawnTransform.position + new Vector3(i * spawnOffset, 0, 0);    // ��ȯ�� ��ġ ���
            //Instantiate(prefab, position, Quaternion.identity);                                 // ������ ��ȯ

            Transform spawnTransform = EnemySpawnPos[room * 5 + i];
            Vector3 position = spawnTransform.position + new Vector3(spawnOffset, 0, 0);
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    public void DestroySpawnedObjects() // ������ ������ �� ���� TP ��ü ����
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();

        foreach(var item in spawnedStoreTP)
        {
			if (item != null)
			{
				Destroy(item);
			}
		}
		spawnedStoreTP.Clear();
	}
}
