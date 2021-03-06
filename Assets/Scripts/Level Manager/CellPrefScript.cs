﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GridSystem;
using System;

public class CellPrefScript : Cell
{
    [NonSerialized]
    public GameObject MyCell;
    public List<Material> Materials;
    List<Agent> agents = new List<Agent>();
    int area;
    GameManager m_GameManager;
    GridArea m_Area;
   
    public ItemData GetData()
    {
        ItemData itemData = new ItemData()
        {
            GridPosition = transform.position,
            ItemType = ItemData.Type.Player,
        };

        return itemData;
    }

    public CellPrefScript(int _x, int _y, Vector3 _worldPosition)
    {      
        x = _x;
        z = _y;
        worldPosition = _worldPosition;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        m_GameManager = FindObjectOfType<GameManager>();
        m_GameManager.CleanTiles += CleanTile;
        CleanTile();
    }

    private void OnDisable()
    {
        m_GameManager.CleanTiles -= CleanTile;

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<PlayerData>() != null)
    //    {
    //        other.transform.parent = transform.parent;
    //        //if (other.GetComponent<Agent>().PlayerID == 1)
    //        //{
    //        //    GetComponent<MeshRenderer>().material = Materials[1];
    //        //}
    //        //else if (other.GetComponent<Agent>().PlayerID == 2)
    //        //{
    //        //    GetComponent<MeshRenderer>().material = Materials[2];
    //        //}
    //    }
    //    //else
    //    //{
    //    //    GetComponent<MeshRenderer>().material = Materials[0];
    //    //}
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<PlayerData>() != null)
    //    {
    //        GetComponent<MeshRenderer>().material = Materials[0];

    //    }
    //}

    public void CleanTile()
    {
        if (m_Area == null)
        {
            m_Area = GetComponentInParent<GridArea>();
        }
        FindPlayers();

        bool m_agentHere = false;
       
        foreach (Agent _agent in agents)
        {
            if(_agent.transform.position == transform.position)
            {
				_agent.transform.parent = m_Area.transform;
				m_agentHere = true;
                if(_agent.PlayerID == 1)
                {
                    GetComponent<MeshRenderer>().material = Materials[1];
                }
                else if (_agent.PlayerID == 2)
                {
                    GetComponent<MeshRenderer>().material = Materials[2];
                }
            }            
        }
        if (m_agentHere == false)
        {
            GetComponent<MeshRenderer>().material = Materials[0];
        }
    }

    void FindPlayers()
    {
        agents = FindObjectsOfType<Agent>().ToList();
    }
}