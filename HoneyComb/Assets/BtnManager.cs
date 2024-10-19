using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    public GameObject tip;
    private bool isOpened = false;
    public void OnAndOFF()
    {
        if(isOpened == false)
        {
            tip.SetActive(true);
            isOpened = true;
        }
        else if(isOpened == true)
        {
            tip.SetActive(false);
            isOpened = false;
        }
	}

}
