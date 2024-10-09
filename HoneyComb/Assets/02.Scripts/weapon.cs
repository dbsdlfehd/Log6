using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    //�ٰŸ� ���ݿ� ��ũ��Ʈ
    //���� �������� ����
    public int id; //���� or ���� ���̵�
    public int prefabId; //������ ���̵�
    public int damage; //������
    public int count; //���� �Ǵ� ��
    public float speed; // ����

    //public int per;

    float timer;
    Enemy enemy;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void start()
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
                speed = 0.5f; //����
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index > count; index++)
        {
            Transform FarATK = GameManager.Instance.pool.Get(prefabId).transform;
            FarATK.parent = transform;
            FarATK.GetComponent<FarATK>().Init(damage, count, Vector3.zero); //count �ڸ� -1�� �������� ����
        }

    }

    void Fire()
    {
        if (!enemy.scanner.nearestTarget)
            return;

        Vector3 targetPos = enemy.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform FarATK = GameManager.Instance.pool.Get(prefabId).transform;
        FarATK.position = transform.position;
        FarATK.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        FarATK.GetComponent<FarATK>().Init(damage, count, dir);
    }
}
