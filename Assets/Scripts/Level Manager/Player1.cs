﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using UnityEngine.UI;
using System.Linq;

public class Player1 : Agent
{
    private void Update()
    {
        TextUpdate();    
        InUpdate();
        TurnUpdate();
    }

   void TurnUpdate()
    {
        MyTurn = GameObject.Find("GameManager").GetComponent<GameManager>().Turn;
    }

    void TextUpdate()
    {
        Lifetext.text = "P1 Life:" + GetComponent<LifeManager>().Life.ToString(); // Life on screen

    }   
    
}
