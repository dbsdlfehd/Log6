using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon2 : MonoBehaviour
{
    //���Ÿ� ���ݿ� ��ũ��Ʈ
    //���� �������� ����
    [Header("�ֹε�Ϲ�ȣ")]
    public int id; //���� or ���� ���̵�
    public int prefabId; //������ ���̵�

    [Header("�̰� �Ǵ� ����;; ���ݷ�")]
    private int damage; //������

    [Header("�����")]
    public int count; //���� �Ǵ� �� // EX. 0 = �Ѱ�

    [Header("ź��")]
    public float speed; // ���� �ӵ�

	[Header("����")]
	public bool isBoss; // ���� �ӵ�

	//public int per;

	float timer;
    Enemy enemy;

    void Awake()
    {
		if (!isBoss)
		{
			damage = TableManager.Enemy1Atk; //������
		}
		else if (isBoss)
		{
			damage = TableManager.BossATK; //������
		}
		enemy = GetComponentInParent<Enemy>();
    }

    void Start()
    {
		Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                //transform.Rotate(Vector3.back * speed * Time.deltaTime); //����ȸ��
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
		Scan();
	}

    public void Init()
    {
        switch (id)
        {
            case 0:
                //speed = -150; //����ȸ�� �ӵ�
                Batch();
                break;
            default:
                speed = 0.8f;
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index > count; index++)
        {
            Transform FarATK = GameManager.Instance.pool.Get(prefabId).transform;
            FarATK.parent = transform;
            if(Vector3.zero != null)
            {
				FarATK.GetComponent<FarATK>().Init(damage, count, Vector3.zero); //count �ڸ� -1�� �������� ����
			}
        }

    }
	public Transform nearestTarget;
	public float scanRadius = 10f; // Ž�� �ݰ�
	public LayerMask targetLayer; // Ž�� ��� ���̾�


	void Scan()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius, targetLayer);

		float nearestDistance = float.MaxValue;
		nearestTarget = null;

		foreach (Collider col in colliders)
		{
			// Ư�� ������Ʈ�� ���� Collider�� ���͸�
			Player playerComp = col.GetComponent<Player>();
			if (playerComp == null) continue; // PlayerComponent�� ������ ����

			float distance = Vector3.Distance(transform.position, col.transform.position);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestTarget = col.transform; // Ÿ������ ����
			}
		}
	}

	void Fire()
	{
		//Debug.Log("�� �߻�");

		if (!enemy.scanner2.nearestTarget)
			return;

		Vector3 targetPos = enemy.scanner2.nearestTarget.position;
		Vector3 dir = targetPos - transform.position;
		dir = dir.normalized;

		Transform FarATK = GameManager.Instance.pool.Get(prefabId).transform;
		FarATK.position = transform.position;
		FarATK.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		FarATK.GetComponent<FarATK>().Init(damage, count, dir);
	}

}
