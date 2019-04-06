﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCharacter : MonoBehaviour
{
    public GameObject Loading;  

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Q))
        {
            FindObjectOfType<SelectMenu>().RemoveCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            FindObjectOfType<SelectMenu>().RemoveCharacter(2);
        }

        if(Loading.activeInHierarchy == true && Input.GetKeyDown(KeyCode.A))
        {
            GameObject Bottone = FindObjectOfType<OnClick>().gameObject;
            Bottone.GetComponent<OnClick>().LoadByIndex(1);
        }
    }
}