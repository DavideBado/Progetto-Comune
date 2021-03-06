﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : StateMachineBehaviour
{
    VFXController myVFXController;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("AnimAbility");
        myVFXController = animator.GetComponent<VFXController>();
        myVFXController.ActiveVFX(myVFXController.AbilityVFX);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myVFXController.DeactivateVFX(myVFXController.AbilityVFX);
    }
}
