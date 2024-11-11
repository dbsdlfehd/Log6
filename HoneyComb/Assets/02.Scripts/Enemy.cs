using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("�ӵ�")]
    public float speed;
	private Rigidbody2D rb;//�߷�
	private Player player;//�÷��̾�
	private PlayerAction playerAction;
	private PrefabSpawner prefabSpawner;

    private Rigidbody2D target;

	[Header("���� ����")]
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;

	[Header("ü��")]
	public int maxHP; // �ִ� ü�� ����
    public int nowHP; // ���� ü�� ����

	[Header("�̰� ���� ����")]
    public Scanner scanner;
    public Scanner2 scanner2;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

	//ü�¹� ������ 
	[SerializeField]					// private�� ������ �ܺο��� ������ �� �ְ� �ٲ���
	private GameObject prfHpBar;		// ������ ü�¹�

	RectTransform bghp_bar;				// bghp_bar ��ο� ��� ü�¹�
	Image hp_bar;						// hp_bar ���� ü�¹�

	public float height = 1.7f;			// ü�¹� Y ����

    private void SetEnemyStatus(int _maxHP)
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = FindObjectOfType<Player>();// ������ ����ߵ� (�ʱ�ȭ)
		SetEnemyStatus(100);				// ü�� ��ġ ����

		// prfHpBar �������� �̿��� canvas���ٰ� ü�¹� ����.
		bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar����
		hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar�� �ڽ� ������Ʈ ������Ʈ ��������
	}

	void Update()
	{
		// ī�޶� ���� ���� ü�¹� ��ǥ ��ġ ����
		Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
		bghp_bar.position = _hpBarPos; // �ش� ��ǥ�� ��ġ �����ϱ�

		hp_bar.fillAmount = (float)nowHP / (float)maxHP; // ü�� ��ġ �����ϱ�
	}

	void Awake()
    {
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>(); //�ٰŸ� ���ݿ� ��ĵ
        scanner2 = GetComponent<Scanner2>(); //���Ÿ� ���ݿ� ��ĵ
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
		prefabSpawner = FindAnyObjectByType<PrefabSpawner>();
	}

	void FixedUpdate()
    {
		if(player.EnmeyDown == true)
        {
			EnemyDead();
			//GetComponent<Image>();     // ü�¹�(��) �ı�
			//GetComponent<RectTransform>();   // ü�¹�(���) �ı�
			//Destroy(gameObject); // �� (�ڱ��ڽ�) �ı�
		}

		Vector2 direction = player.transform.position - transform.position;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//������ ������
		{
			//���󰣴�.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//���� ������ �ø����ִ� ��
			if(direction.x > 0)
			{
				spriter.flipX = false;
			}
			else if(direction.x < 0)
			{
				spriter.flipX = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            nowHP = nowHP - player.Atk;
		}
        if(nowHP < 0)							   // ü���� 0���� ������
        {
			EnemyDead();
		}
	}

	void EnemyDead()
    {
		nowHP = 0;
		Destroy(gameObject);
		prefabSpawner.RoomEnemyCount++;        // �� ���� Ƚ�� 1 �þ
		Destroy(bghp_bar.gameObject);          // ü�¹� ����
	}
}
