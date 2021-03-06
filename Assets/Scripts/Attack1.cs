﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack1 : MonoBehaviour
{
    public GameManager Manager;
    float Timer;

    private void Start()
    {
        
        Manager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"/* && Manager.CanAttack == true*/) // Se è in collisione con un player e può attaccare
        {
            //other.transform.DOShakePosition(0.5f, 0.6f, 10, 45);
            other.GetComponent<LifeManager>().DamageAmount = 1;
            other.GetComponent<LifeManager>().Enemy = GetComponent<Agent>();
            other.GetComponent<LifeManager>().BaseAttack = true;
            GetComponentInChildren<AnimationController>().Enemy = other.GetComponent<LifeManager>();

            this.GetComponent<Collider>().enabled = false; // Spegni il collider di attacco
            /*Manager.CanAttack = false;*/ // Non posso più attaccare

           
        }
    }

    private void Update()
    {
        if(GetComponent<Collider>().enabled == true)
        {
            Timer += Time.deltaTime;
            if (Timer >= 0.5f)
            {
                Manager.CanAttack = false; // Non posso più attaccare
            }
        }
    }



}
