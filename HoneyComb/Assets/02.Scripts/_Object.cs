using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Object : MonoBehaviour
{
    public int id;//NPC번호

    //public bool isNPC;//NPC인지 확인하는 건데 필요 없는듯

    //NPC별 횟차별 대화 횟수
    public int EachTalkCount = 0;
    public int TempEachTalkCount = 0;

    //테스트용 이름 변수
    public new string name;

    //NPC별 죽은뒤 대화 확인용 bool형 변수
	public bool isDeadUp = false;
	public bool isUpGraded = false;

    public int PlayerDead;
    public int tempPlayerDead = 0;
}
