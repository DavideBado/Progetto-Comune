﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Charge : MonoBehaviour
{
    int power = 0;
    bool attackCheck = false;
    float Timer;
    GameManager Manager;
    private bool onAttack;

    private void Start()
    {
        Timer = 1f;
        Manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        InUpdate();
        if (onAttack == true)
        {
            GetComponent<Agent>().OnAttack = true;
            Timer -= Time.deltaTime;
            Manager.TimerOn = false;
            Manager.UpdateTilesMat();
            NewPreview(Manager.CellAttackMaterial);
            if (Timer <= 0)
            {
                onAttack = false;
                Manager.TimerOn = true;
                Manager.CleanTiles();
                Manager.UpdateTilesMat();
                Timer = 1f;
            }

        }
        
    }

    void InUpdate()
    {
        if (attackCheck == true)
        {
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if ((Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, 1f)) && hit.transform != transform)
        {
            if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<LifeManager>().Life -= power;
            }
            attackCheck = false;
            Manager.Pause = false;
            Manager.TimerOn = true;

        }            
    }
    public void Ability()
    {
        if (GetComponent<Agent>().Mana > 0 && GetComponent<Agent>().MyTurn && GetComponent<Agent>().PlayerType == 5 && GetComponent<Agent>().ImStunned == false && Manager.CanAttack == true && Manager.Pause == false)
        {            
            GetComponentInChildren<AnimationController>().Ability();
            RaycastHit hit;
            onAttack = true;
            if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, Mathf.Infinity))

            {
                
                if (hit.transform.position.x == transform.position.x)
                {
                    int dist = (GetComponent<Agent>().y - (int)hit.transform.position.z);

                    if (dist < 0)
                    {
                        GetComponent<Agent>().y = (int)hit.transform.position.z - 1;
                    }
                    else if (dist > 0)
                    {
                        GetComponent<Agent>().y = (int)hit.transform.position.z + 1;
                    }
                    Manager.TimerOn = false;
                    GetComponent<Agent>().OnTheRoad = true;
                    GetComponent<Agent>().AgentSpeed = 5;
                }

                else
                    if (hit.transform.position.z == transform.position.z)
                {
                    int dist = (GetComponent<Agent>().x - (int)hit.transform.position.x);

                    if (dist < 0)
                    {
                        GetComponent<Agent>().x = (int)hit.transform.position.x - 1;
                    }
                    else if (dist > 0)
                    {
                        GetComponent<Agent>().x = (int)hit.transform.position.x + 1;
                    }
                    Manager.TimerOn = false;
                    GetComponent<Agent>().OnTheRoad = true;
                    GetComponent<Agent>().AgentSpeed = 5;
                }

                Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);

                if (hit.transform.tag == "Player" && hit.transform != transform)

                {
                    int dist = (int)Vector3.Distance(transform.position, hit.transform.position);
                    Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                    if (dist <= 5)
                    {
                        power = 1;
                    }
                    else if (dist > 5)
                    {
                        power = 2;
                    }
                    attackCheck = true;
                }

            }
            else
            {
                float _lookX = GetComponent<Agent>().SavedlookAt.x;
                float _lookY = GetComponent<Agent>().SavedlookAt.z;

                if (_lookX != 0)
                {
                    if (_lookX < 0)
                    {
                        GetComponent<Agent>().x = 0;
                    }
                    else if (_lookX > 0)
                    {
                        GetComponent<Agent>().x = GetComponent<Agent>().configGrid.DimX;
                    }
                }
                else if (_lookY != 0)
                {
                    if (_lookY < 0)
                    {
                        GetComponent<Agent>().y = 0;
                    }
                    else if (_lookY > 0)
                    {
                        GetComponent<Agent>().y = GetComponent<Agent>().configGrid.DimY;
                    }
                }
            }                   

            GetComponent<Agent>().Mana--;
            Manager.CanAttack = false;
        }

    }

    //public void Preview()
    //{
    //    Manager.UpdateTilesMat();
    //    if (GetComponent<Agent>().MyTurn && GetComponent<Agent>().ImStunned == false && Manager.CanAttack == true && Manager.Pause == false)

    //    {
    //        //CleanPreview();
    //        Material PrevMaterial = FindObjectOfType<CellPrefScript>().Materials[3];
    //        NewPreview(PrevMaterial);
    //    }
    //}

    void NewPreview(Material _material)
    {                         
            float _lookX = GetComponent<Agent>().SavedlookAt.x;
            float _lookY = GetComponent<Agent>().SavedlookAt.z;
            Vector3 playerPosition = transform.position;

            List<CellPrefScript> cells = new List<CellPrefScript>();

            cells = FindObjectsOfType<CellPrefScript>().ToList();

            RaycastHit hit;

            if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.black);
                if (_lookX != 0)
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition, _lookX, hit.transform.GetComponent<Agent>(), _material);
                }
                else if (_lookY != 0)
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition, _lookY, hit.transform.GetComponent<Agent>(), _material);

                }
                
            }
            else
            {

                if (_lookX != 0)
                {
                    if (_lookX > 0)
                    {
                        CellsGreenInRay(new Vector3(GetComponent<Agent>().configGrid.DimX, 0,transform.position.z), cells, playerPosition, _lookX, null, _material);
                    }
                    else if (_lookX < 0)
                    {
                        CellsGreenInRay(new Vector3(-1, 0, transform.position.z), cells, playerPosition, _lookX, null, _material);
                    }
                }
                else if (_lookY != 0)
                {

                    if (_lookY < 0)
                    {
                        CellsGreenInRay(new Vector3(transform.position.x, 0, -1), cells, playerPosition, _lookY, null, _material);
                    }
                    else if (_lookY > 0)
                    {
                        CellsGreenInRay(new Vector3(transform.position.x, 0, GetComponent<Agent>().configGrid.DimY), cells, playerPosition, _lookY, null, _material);
                    }
                }
            }    
    }

    public void CleanPreview()
    {
        List<CellPrefScript> cells = new List<CellPrefScript>();

        cells = FindObjectsOfType<CellPrefScript>().ToList();

        foreach (CellPrefScript cell in cells)
        {
            cell.GetComponent<MeshRenderer>().material = cell.Materials[0];
        }
        Manager.UpdateTilesMat();
    }

    void CellsGreenInRay(Vector3 HitPosition, List<CellPrefScript> cells, Vector3 playerPosition, float Look, Agent _agent, Material _material)
    {
        foreach (CellPrefScript cell in cells)
        {
            int dist = (int)Vector3.Distance(transform.position, cell.transform.position);
            if (_agent != null)
            {
                if ((((playerPosition.x < cell.transform.position.x && cell.transform.position.x <= HitPosition.x)

                  ||

                 (playerPosition.x > cell.transform.position.x && cell.transform.position.x >= HitPosition.x)) &&


                 (Mathf.Round(cell.transform.position.z) == Mathf.Round(HitPosition.z))) ||



                 (((playerPosition.z < cell.transform.position.z && cell.transform.position.z <= HitPosition.z) ||
                 (playerPosition.z > cell.transform.position.z && cell.transform.position.z >= HitPosition.z)) &&
                 (Mathf.Round(cell.transform.position.x) == Mathf.Round(HitPosition.x))))
                {
                    if (dist >= 0 && dist <= 5)
                    {
                        cell.GetComponent<MeshRenderer>().material = _material;
                    }
                    else if (dist > 5)
                    {
                        cell.GetComponent<MeshRenderer>().material = cell.Materials[1];
                    }

                }
            }
            else if ((((playerPosition.x < cell.transform.position.x && cell.transform.position.x < HitPosition.x)

                  ||

                 (playerPosition.x > cell.transform.position.x && cell.transform.position.x > HitPosition.x)) &&


                 (Mathf.Round(cell.transform.position.z) == Mathf.Round(HitPosition.z))) ||



                 (((playerPosition.z < cell.transform.position.z && cell.transform.position.z < HitPosition.z) ||
                 (playerPosition.z > cell.transform.position.z && cell.transform.position.z > HitPosition.z)) &&
                 (Mathf.Round(cell.transform.position.x) == Mathf.Round(HitPosition.x))))
            {
                if (dist >= 0 && dist <= 5)
                {
                    cell.GetComponent<MeshRenderer>().material = _material;
                }
                else if (dist > 5)
                {
                    cell.GetComponent<MeshRenderer>().material = cell.Materials[1];
                }

            }
        }
    }
}

