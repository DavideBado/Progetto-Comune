﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using UnityEngine.UI;

public class PositionTester1 : MonoBehaviour {
    bool _Round, OnTheRoad = false;
    public int x, x2;
    public int y, y2;  
    public BaseGrid grid;
    int MaxAct;
    public Collider ColliderBasicAtt;
    public Text Lifetext;
    public GameObject respawn;
    public GridConfigData configGrid;

    private void Start()
    {
        InStart();             
    }
    // Update is called once per frame
    void Update()
    {
        InUpdate();        
    }

    void InStart()
    {       
        Spawn(); // Posizionati nella tua posizione iniziale
        FirstSaveXY(); // Salva le coordinate della tua posizione
    }

    void InUpdate()
    {
        RoundCheck(); // Controlla se è il tuo turno
        TextUpdate(); // Aggiorna i testi a schermo
        Sicura(); // Evita attacchi scorretti
        Movement(); // Sposta il player
    }

    #region START
    void FirstSaveXY()
    {
        // x2 && y2 Start = null, the next code lines change x2 && y2 != null
        x2 = x;
        y2 = y;
    }

    void Spawn()
    {
        transform.position = respawn.transform.position; // Posiziona il giocatore nella posizione di partenza
    }
    #endregion

    #region UPDATE
    void RoundCheck()
    {
        _Round = GameObject.Find("GameManager").GetComponent<GameManager>().Round; // Controlla i round contati dal manager
    }

    void TextUpdate() // Aggiorna i testi a schermo
    {
        Lifetext.text = "P2 Life:" + GetComponent<LifeManager>().Life.ToString(); // Life on screen
    }

    void Sicura() // Assicura che il collider per l'attacco base non sia attivo durante il turno avversario
    {
        if (_Round == true)
        {

            ColliderBasicAtt.enabled = false;
        }
    }

    void Movement()
    {
        if (grid) // Check if we have a grid
        {

            if (checkIfPosEmpty()) // Now if the cell is free
            { // Move the player && save x && y
                ColliderBasicAtt.enabled = false; // Assicurati che l'attacco base non sia attivo

                // Muove il player verso la casella selezionata alla velocità di Speed unità/s
                transform.position = Vector3.MoveTowards(transform.position, grid.GetWorldPosition(x, y),
                GameObject.Find("GameManager").GetComponent<GameManager>().Speed * Time.deltaTime);
                if (transform.position == grid.GetWorldPosition(x, y)) // Se sono arrivato a destinazione
                {
                    // Aggiorna i valori di x e y salvati
                    x2 = x;
                    y2 = y;
                    OnTheRoad = false;
                }
            }
            else // Se invece non mi sono potuto spostare
            { // Carica i vecchi valori di x e y
                x = x2;
                y = y2;
            }
        }
    }
    #endregion

    #region INPUT // Ascoltano le chiamate del inputmanager
    public void Up() //Trasforma la chiamata del manager in una richiesta di movimento
    {
        if (y < (configGrid.DimY - 1) && OnTheRoad == false && _Round == false)
        {
            y++;
            OnTheRoad = true;
        }
    }
    public void Left()
    {
        if (x > 0 && OnTheRoad == false && _Round == false)
        {
            x--;
            OnTheRoad = true;
        }
    }
    public void Down()
    {
        if (y > 0 && OnTheRoad == false && _Round == false)
        {
            y--;
            OnTheRoad = true;
        }
    }
    public void Right()
    {
        if (x < (configGrid.DimX - 1) && OnTheRoad == false && _Round == false)
        {
            x++;
            OnTheRoad = true;
        }
    }
    public void BasicAttack()
    {
        if (_Round == false) // Se è il mio turno
        {
            ColliderBasicAtt.enabled = true; // Attiva il collider di attacco
        }
        else ColliderBasicAtt.enabled = false;
    }
    #endregion 

    public bool checkIfPosEmpty() // This check if the cell is free
    {
        GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("Player"); // Trova tutti i player
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == (grid.GetWorldPosition(x, y) + new Vector3(0, 4))) // Se uno di essi si trova nella casella dove voglio andare
                return false; // Non posso anarci
        }
        return true; // Se invece è libera posso andarci
    }
}
