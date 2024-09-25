using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 dirVec = target.position - rigid.position;
        Debug.Log($"{dirVec.x.ToString("F0")}, {dirVec.y.ToString("F0")}");


        //player��ǥ - npc��ǥ
        int X = Math.Abs(Mathf.RoundToInt(dirVec.x));
        int Y = Math.Abs(Mathf.RoundToInt(dirVec.y));

        //������ ������
        if (X <= SenserRangeX && Y <= SenserRangeY)
        {
            //���󰣴�.
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }
    
    void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
