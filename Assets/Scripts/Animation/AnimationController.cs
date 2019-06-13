﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{
    public LifeManager Enemy;
    GameManager gameManager;
    Agent agent;
    Animator animator;

    #region Actions
    public Action Move, Attack, Ability, Stun, Damage, Happy, Death, Idle;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        agent = GetComponentInParent<Agent>();
        animator = GetComponent<Animator>();
    }

    #region Funzioni MonoBehaviour
    private void OnEnable()
    {
        Death += DeathSetAnim;
        Happy += HappySetAnim;
        Move += MoveSetAnim;
        Idle += IdleSetAnim;
        Stun += StunSetAnim;
        Attack += AttackSetAnim;
        Ability += AbilitySetAnim;
        Damage += DamageSetAnim;
    }
    private void OnDisable()
    {
        Death -= DeathSetAnim;
        Happy -= HappySetAnim;
        Move -= MoveSetAnim;
        Idle -= IdleSetAnim;
        Stun -= StunSetAnim;
        Attack -= AttackSetAnim;
        Ability -= AbilitySetAnim;
        Damage -= DamageSetAnim;
    }
    #endregion

    #region Funzioni che settano i trigger
    private void MoveSetAnim()
    {
        animator.SetTrigger("AnimWalk");
    }
    private void HappySetAnim()
    {
        animator.SetTrigger("AnimHappyGeek");
    }
    private void AttackSetAnim()
    {
        animator.SetTrigger("AnimAttack");
    }
    private void AbilitySetAnim()
    {
        animator.SetTrigger("AnimAbility");
    }
    private void StunSetAnim()
    {
        animator.SetTrigger("AnimStun");
    }
    private void DeathSetAnim()
    {
        animator.SetTrigger("AnimPlayDeathStranding");
    }
    private void IdleSetAnim()
    {
        animator.ResetTrigger("AnimWalk");
        animator.SetTrigger("AnimIdle");
    }
    private void DamageSetAnim()
    {
        animator.SetTrigger("AnimDamage");
        if(agent.ImStunned)
        {
            Stun();
        }
    }
    #endregion

    #region Funzioni per animation events
    public void Idle_Animation()
    {
        Idle();
    }

    public void DamageEnemy()
    {
        Enemy.Damage();
        Idle();
    }

    public void DieNow()
    {
        gameManager.EndGameCheck(agent.PlayerID, agent.gameObject);
    }

    #endregion
}
