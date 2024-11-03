using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerHpShow PlayerHpShow;

	public PrefabSpawner prefabSpawner;// �� ���� ��ũ��Ʈ

	public _Object[] npcObjects = new _Object[2]; // NPC

	[Header("���� Ƚ��")]
    public int DeadCount = 0;
	public TextMeshProUGUI DeadCount_UI;

	public Transform player;            // �÷��̾� ��ġ
	public Transform Home;              // �� ��ġ
	public Transform DeadPoint;         // ���� ��ġ

	public bool isDead = false;
    public GameObject Dead_set;         // ���� ����

    [Header("ü��")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("����")]
	public int Atk;
	public TextMeshProUGUI Atk_UI;

	[Header("�ӵ�")]
    public float minPos;
    public float maxPos;
    public RectTransform pass;
    public int atkNum;
    

	private void Start()
	{
		nowHP = maxHP;
		PlayerHpShow = GetComponent<PlayerHpShow>();
	}
    private void Update()
    {
        if(isDead == true && Input.GetKeyDown(KeyCode.Space)) // �÷��̾� ���� ����
        {
			nowHP = maxHP;							// �� ȸ��
            isDead = false;							// �� ����
			player.position = Home.position;		// �� ��ġ�� ����
			Dead_set.SetActive(false);              // ���� ���� ���ֱ�

			foreach (var npc in npcObjects)			// NPC ��ǳ�� �����ֱ�
			{
				npc.isDialogged = true;
			}
		}

		if (isDead == true)							// ������ ��ǥ ����
			player.position = DeadPoint.position;

		HP_UI.text = nowHP.ToString();    // ���� ü�� ��ġ
		Atk_UI.text = Atk.ToString();				// ���ݷ� ��ġ
		DeadCount_UI.text = "���� Ƚ�� : " + DeadCount.ToString();
	}

    void OnTriggerEnter2D(Collider2D collision) // ��� ���Ÿ� ����
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        if (nowHP < 0) // ����
		{
			Dead();
		}
    }

    public void Dead()
    {
        DeadCount++;						  // ���� Ƚ�� �߰��ϱ�

		isDead = true;
        Dead_set.SetActive(true);
		player.position = DeadPoint.position; // ���� ��ġ �̵�
		prefabSpawner.isSpawnned = false;
	}
}
