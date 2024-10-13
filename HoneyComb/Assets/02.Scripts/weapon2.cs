using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon2 : MonoBehaviour
{
    //원거리 공격용 스크립트
    //공격 구분짓기 위함
    [Header("주민등록번호")]
    public int id; //무기 or 공격 아이디
    public int prefabId; //프리펩 아이디

    [Header("공격력")]
    public int damage; //데미지

    [Header("관통력")]
    public int count; //관통 되는 수 // EX. 0 = 한개

    [Header("탄속")]
    public float speed; // 무기 속도

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
                //transform.Rotate(Vector3.back * speed * Time.deltaTime); //무기회전
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
                //speed = -150; //무기회전 속도
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
				FarATK.GetComponent<FarATK>().Init(damage, count, Vector3.zero); //count 자리 -1은 무한으로 관통
			}
        }

    }

    void Fire()
    {
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
