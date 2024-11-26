using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;
//using UnityEditor;

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
    //private GameObject spawnedStoreTP;  // ������ ���� ��ü ����
	private List<GameObject> spawnedStoreTP = new List<GameObject>();

	[Header("������ �����۵�")]
    public GameObject[] reward_item_prf;          // ����, �ִ� ü��, ��ȭ, ����
    private List<GameObject> spawnedItems = new List<GameObject>(); // ������ �����۵�

    public float sec = 3.0f;             // ���� �� ��ȯ�� �ð�

    [Header("�� ��� ��ȯ")]
    public int spawnCount = 5;

    [Header("���ΰ��� ��ȯ�� ��ġ")]
    public Transform[] Pos;

    [Header("���� ������ ��ȯ�� ��ġ")]
    public Transform[] RewardItem_Pos;

    [Header("��ȯ�� ��ġ ������Ʈ��")]
    public GameObject[] PosObj;

	[Header("���� ���� �ð�")]
	public float docBack_DelayTime;

	public float spawnOffset = 1.0f;    // ��ȯ �� ����
    public float checkDistance = 0.1f;  // ����� �Ÿ� ����
    public bool isSpawnned = false;
    public int RoomEnemyCount = 0;

    static public bool alreadySpawnedEnemies = false; // ���� ��ȯ ����

    int temp_i = 0;

    private void Update()
    {
		// �� �濡�� �� ���� óġ��
		if (RoomEnemyCount == spawnCount)
		{
            isSpawnned = false;
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

		// (�÷��̾ �ش� �濡 �ý� && ���尡 ���� ���� �ƴ϶� �÷��̾ ���� ���带 �������� ��) -> �� ��ȯ
		for (int i = 0; i < Pos.Length; i++)
        {
            if (Vector3.Distance(playerPos.position, Pos[i].position) < checkDistance && !alreadySpawnedEnemies)
            {
                temp_i = i;         // ��ġ �����
                SpawnEnemies(i);
            }
        }
    }

    public void SpawnEnemies(int room) // �� ��ȯ �Լ�
    {
        if (isSpawnned == false) // �ѹ��� ����� if��
        {
            //Debug.Log("���� ��ȯ�մϴ�.");
            alreadySpawnedEnemies = true;
            roomGenerator.DestroyDoor(); // ������ �� ����
            isSpawnned = true;
            StartCoroutine(DelayedSpawn(sec, room));
            StartCoroutine(DocBack());   // ���ΰ� ���� ����
        }
    }

    IEnumerator DocBack() // ��ٷȴٰ� �������
    {
		yield return new WaitForSeconds(docBack_DelayTime);
		talkManager.SoloTalk(); // ���� ���� ��Ȳ
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
