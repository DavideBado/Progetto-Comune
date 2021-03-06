﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using WindowsInput;
using System;

public class Agent : MonoBehaviour, ICharacter
{
    public string CharacterName, AbilityName, AbilityDescription;
    GameObject Graphic;
    InputSimulator m_InputSimulator = new InputSimulator();
    int m_OnEnableCounter = 0;
    public int SwitchIndex;
    [HideInInspector] public Transform AgentParent = null;
    [HideInInspector] public Vector3 AgentSpawnPosition;
    public int PlayerID;
    //************Variabili per test abilità***********
    public KeyCode BigD1, Drain2, Stun3, Venom4, Charge5, Whirlwind6;
    public int PlayerType = 0;
    public int CurrentID;
    //*************************************************
    public List<Sprite> _Sprites = new List<Sprite>();
    public List<Sprite> _InGameUIDamage = new List<Sprite>(); // Mettere gli sprite dal più danneggiato al meno, così che si possa accedere alla  lista con index = life-1
    public List<Sprite> LifeSprites = new List<Sprite>();
    public List<Sprite> LifeSpritesBench = new List<Sprite>(); 
    public List<Sprite> EnergySprites = new List<Sprite>();
    public List<Material> _Materials = new List<Material>();
    public Vector3 SavedlookAt, RayCenter, RayLeft, RayRight;
    public bool MyTurn, OnTheRoad = false;
    public int x, x2;
    public int y, y2;   
    public BaseGrid grid;
    public Collider BasicAtt;
    //public TMPro.TextMeshProUGUI Lifetext;
    public GameObject respawn;
    public GridConfigData configGrid;
    public RespawnController RespawnController;
    List<PlayerData> Players = new List<PlayerData>();
    List<Wall> Walls = new List<Wall>();
    GameManager GameManager;
    public bool RotUp = false, RotDown = false, RotLeft = false, RotRight = false, ImStunned = false, imDrained = false, StartDrain;
    public int Mana = 1;
    //public bool OhStunnedShit;
    public float AgentSpeed;
    Rigidbody rg;
    public GameObject StunPS, PoisonPS, DrainPS;
    int switcherIndex;
    public bool OnAttack;
    private int stunCounter;

    // ********** Cose per il menu *************
    public List<Sprite> Sprites
    {
        set
        {
            Sprites = _Sprites;
        }
    }

    public List<Material> Materials 
    { set
        {
            Materials = _Materials;
        }
            
    }
    //********************************************
    private void OnEnable()
    {
        AgentOnEnable(AgentParent, AgentSpawnPosition);
    }

    private void Start()
    {
        InStart();
        rg = GetComponent<Rigidbody>();
        if (GetComponentInChildren<AnimationController>() != null)
        {
            Graphic = GetComponentInChildren<AnimationController>().gameObject; 
        }
    }


    public void AgentOnEnable(Transform _parent, Vector3 _position)
    {
        transform.parent = _parent;
        transform.position = _position;
        x = (int)_position.x;
        x2 = (int)_position.x;
        y = (int)_position.z;
        y2 = (int)_position.z;
    }

    void InStart()
    {
        //FindObjectOfType<CounterPosition>().FindPlayers();
        UpdateReference();
        Spawn(); // Posiziona il giocatore
        FirstSaveXY(); // Salvo le coordinate della mia posizione
    }

    public void InUpdate()
    {
        //Debug.Log(name + " SpawnPoint:" + AgentSpawnPosition);
        Sicura();
		Movement(); // Muove il giocatore
        RayDirections();
        Stunname();
        ImDrained();
        Poisoned();
    }

    #region Start
    void UpdateReference()
    {
        GameManager = FindObjectOfType<GameManager>();
        grid = FindObjectOfType<BaseGrid>();
        configGrid = grid.ConfigData;
    }
    void FirstSaveXY()
    {
        // x2 && y2 Start = null, the next code lines change x2 && y2 != null
        x2 = x;
        y2 = y;
    }

    void Spawn()
    {
        AgentSpeed = GameManager.Speed;
        //Mana = 1;
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);      
    }
    #endregion

    #region Update

    void Sicura()
	{
		if (MyTurn == false) // Se non è il tuo turno
		{
			BasicAtt.enabled = false; // Metti via le armi
		}
	}
	
    public void Movement() // Muove il giocatore
    {
        if (grid) // Check if we have a grid
        {
            if (checkIfPosEmpty() && ImStunned == false) // Now if the cell is free
            { // Move the player && save x && y
                BasicAtt.enabled = false; // Assicurati di avere le armi nel fodero
                 // Spostati verso la casella selezionata alla velocità di Speed unità al secondo
                transform.position = Vector3.MoveTowards(transform.position, grid.GetWorldPosition(transform.position, x, y),
                AgentSpeed * Time.deltaTime);
                if (!OnAttack)
                {
                    GetComponentInChildren<AnimationController>().Move(); 
                }
                if (transform.position == grid.GetWorldPosition(transform.position, x, y)) // Se hai raggiunto la tua destinazione
                {
                    GameManager.CleanTiles();
                    GameManager.UpdateTilesMat();
                    AgentSpeed = FindObjectOfType<GameManager>().Speed;
                    if (!OnAttack)
                    {
                        GetComponentInChildren<AnimationController>().Idle(); 
                    }
                    // Salva le coordinate della posizione attuale
                    x2 = x;
                    y2 = y;
                    OnTheRoad = false;
                    //GameManager.TimerOn = true;
                }
            }
            else // Load the old values of x && y
            {
                x = x2;
                y = y2;
                OnTheRoad = false;               
                //GameManager.TimerOn = true;
            }
        }
    }

    void RayDirections()
    {
        if(RotUp == true)
        {
            SavedlookAt = new Vector3(0, 0, 1);
            RayLeft = transform.position + new Vector3(-1, 0, 0);
            RayCenter = transform.position + new Vector3(0, 0, 0);
            RayRight = transform.position + new Vector3(1, 0, 0);
        }
        if(RotLeft == true)
        {
            SavedlookAt = new Vector3(-1, 0, 0);
            RayLeft = transform.position + new Vector3(0, 0, -1);
            RayCenter = transform.position + new Vector3(0, 0, 0);
            RayRight = transform.position + new Vector3(0, 0, 1);
        }
        if(RotDown == true)
        {
            SavedlookAt = new Vector3(0, 0, -1);
            RayLeft = transform.position + new Vector3(-1, 0, 0);
            RayCenter = transform.position + new Vector3(0, 0, 0);
            RayRight = transform.position + new Vector3(1, 0, 0);
        }
        if(RotRight == true)
        {
            SavedlookAt = new Vector3(1, 0, 0);
            RayLeft = transform.position + new Vector3(0, 0, 1);
            RayCenter = transform.position + new Vector3(0, 0, 0);
            RayRight = transform.position + new Vector3(0, 0, -1);
        }
    }

    void Stunname()
    {
        if(ImStunned == true)
        {
            StunPS.SetActive(true);
        }
        else
        {
            StunPS.SetActive(false);
        }
        if (ImStunned == true && MyTurn == true)
        {
                ImStunned = false;
            //OhStunnedShit = true;
        }
        //if (OhStunnedShit == true && MyTurn == false)
        //{
        //    OhStunnedShit = false;
        //}
    }
    #endregion



    public bool checkIfPosEmpty() // This check if the cell is free
    {
        GetAllInterestingData(); // Trova tutti gli oggetti che potrebbero interferire con il movimento
        foreach (var item in Players)
        {
            if (grid.GetWorldPosition(transform.position, x, y) == item.transform.position) // Controlla se la cella è occupata da un altro giocatore
            {
                return false;
            }
        }
        foreach (var item in Walls)
        {
            if (grid.GetWorldPosition(transform.position, x, y) == item.transform.position) // Controlla se la cella è occupata da un muro
            {
                return false;
            }
        }
        return true; // Se invece è libera posso andarci
    }

    public void GetAllInterestingData()
    {
        // Creo una lista di tutti i Player presenti nel livello.
        Players = FindObjectsOfType<PlayerData>().ToList();

        // Creo una lista di tutti i Wall presenti nel livello.
        Walls = FindObjectsOfType<Wall>().ToList();
    }

    public void Rotation()
    {
        Vector3 lookAt = grid.GetWorldPosition(transform.position, x, y);
        
        float AngleRad = Mathf.Atan2(lookAt.x - this.transform.position.x, lookAt.z - this.transform.position.z);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        transform.rotation = Quaternion.Euler(0, AngleDeg, 0);

        GameManager.CleanTiles();
        GameManager.UpdateTilesMat();
    }
	
    #region Input

	public void Up() //Trasforma la chiamata del manager in una richiesta di movimento
	{

		if (y < (configGrid.DimY - 1) && OnTheRoad == false && MyTurn == true && ImStunned == false && GameManager.Pause == false)
        {
            RotDown = false; RotLeft = false; RotRight = false;
			OnTheRoad = true;
			y++;						
            Rotation();
            if (RotUp == false)
			{
                if (PlayerID == 1)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W); 
                }
                else if (PlayerID == 2)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.UP);
                }
                y--;
				RotUp = true;
			}
            GameManager.GetComponent<Pointer>().PointerOff();
		}
	}
	public void Left()
	{
		if (x > 0 && OnTheRoad == false && MyTurn == true && ImStunned == false && GameManager.Pause == false)
        {
            RotUp = false; RotDown = false; RotRight = false;
			OnTheRoad = true;
			x--;			
            Rotation();
            if (RotLeft == false)
			{
                if (PlayerID == 1)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_A);
                }
                else if (PlayerID == 2)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);
                }
                x++;
				RotLeft = true;
			}
            GameManager.GetComponent<Pointer>().PointerOff();
        }
	}
	public void Down()
	{
		if (y > 0 && OnTheRoad == false && MyTurn == true && ImStunned == false && GameManager.Pause == false)
        {
            RotUp = false; RotLeft = false; RotRight = false;
			OnTheRoad = true;
			y--;			
            Rotation();
            if (RotDown == false)
			{
                if (PlayerID == 1)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_S);
                }
                else if (PlayerID == 2)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.DOWN);
                }
                y++;
				RotDown = true;
			}
            GameManager.GetComponent<Pointer>().PointerOff();
        }
	}
	public void Right()
	{
		if (x < (configGrid.DimX - 1) && OnTheRoad == false && MyTurn == true && ImStunned == false && GameManager.Pause == false)
        {
            RotUp = false; RotDown = false; RotLeft = false;
			OnTheRoad = true;
			x++;
            Rotation();
            if (RotRight == false)
			{
                if (PlayerID == 1)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_D);
                }
                else if (PlayerID == 2)
                {
                    m_InputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.RIGHT);
                }
                x--;
				RotRight = true;
			}
            GameManager.GetComponent<Pointer>().PointerOff();
        }
	}

	public void BasicAttack()
	{
		if (MyTurn == true && ImStunned == false && GameManager.CanAttack == true && GameManager.Pause == false) // Se è il mio turno
		{
            GetComponentInChildren<AnimationController>().Attack();
            OnTheRoad = true;
			GameManager.CanAttack = false;
            //BasicAtt.enabled = true; // Attiva il collider di attacco     

            RaycastHit hit;
            if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, 1))
            {
                Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);
                if (hit.transform.tag == "Player" && hit.transform != transform)
                {
                    OnAttack = true;
                    Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                    //hit.transform.DOShakePosition(0.5f, 0.4f, 10, 45);
                    hit.transform.GetComponent<LifeManager>().DamageAmount = 1;
                    hit.transform.GetComponent<LifeManager>().Enemy = GetComponent<Agent>();
                    hit.transform.GetComponent<LifeManager>().BaseAttack = true;
                    GetComponentInChildren<AnimationController>().Enemy = hit.transform.GetComponent<LifeManager>();
                }
            }

            /*Graphic.*/transform.DOMove(transform.position + SavedlookAt * 0.2f, 0.3f)
                .SetAutoKill();

        }
		else BasicAtt.enabled = false;
	}

    public void Poisoned()
    {
        if(GetComponentInChildren<Poison>() != null)
        {
            PoisonPS.SetActive(true);
        }
        else
        {
            PoisonPS.SetActive(false);
        }
    }

    public void Switch_Up()
    {
        //SwitchIndex++;
        //if (switcherIndex >= 2)
        //    switcherIndex = 0;
        if (MyTurn == true && GameManager.TimerOn == true && ImStunned == false /*&& OhStunnedShit == false*/)
        {
            GameManager.Switcher(PlayerID, 0, gameObject, RotUp, RotDown, RotRight, RotLeft);
        }
    }

    public void Switch_Down()
    {
        //SwitchIndex--;
        //if (switcherIndex < 0)
        //    switcherIndex = 1;
        if (MyTurn == true && GameManager.TimerOn == true && ImStunned == false /*&& OhStunnedShit == false*/)
        {
            GameManager.Switcher(PlayerID, 1, gameObject, RotUp, RotDown, RotRight, RotLeft);
        }
    }    

    #endregion

    public void TeleportMe()
    {
        if (GameManager.TimerOn == true && MyTurn == true)
        {
            GameManager.ActivatePortal(); 
        }
    }
    public void ImDrained()
    {        
        if (imDrained == true && StartDrain == GameManager.Turn)
        {
            DrainPS.SetActive(true);
        }
        else 
        { if (StartDrain != GameManager.Turn)
            {
                imDrained = false;
                DrainPS.SetActive(false);
            }
        }
    }
}