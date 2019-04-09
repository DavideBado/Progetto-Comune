﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{    
    Agent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<Agent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        animator.SetBool("OnMove", agent.OnTheRoad);
    }
}