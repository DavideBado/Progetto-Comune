﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Drain : MonoBehaviour
{
    GameManager Manager;
    public Agent agent;
    bool imDrained = false;

    private void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        agent = GetComponent<Agent>();
    }
    public void Ability()
    {
        if (GetComponent<Agent>().Mana > 0 && GetComponent<Agent>().MyTurn && GetComponent<Agent>().PlayerType == 2 && GetComponent<Agent>().ImStunned == false && Manager.CanAttack == true)
        {
            if(GetComponent<LifeManager>().Life == 6)
            {
                ImFullButIWannaDrain();
            }

            if (GetComponent<LifeManager>().Life < 6)
            {
                INeedLifeDrain();
            }

            if (FindObjectOfType<PickUpsSpawner>().AllManaFull == true)
            {
                FindObjectOfType<GameManager>().PickUpTurnCount = 0;
                FindObjectOfType<PickUpsSpawner>().AllManaFull = false;
            }
            GetComponent<Agent>().Mana--;
            Manager.CanAttack = false;
            agent.DrainPS.SetActive(true);
            agent.StartDrain = Manager.Turn;
        }        
    }
    void ImFullButIWannaDrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);
            if (hit.transform.tag == "Player" && hit.transform != transform)
            {
                Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                hit.transform.GetComponent<LifeManager>().Life-=2;
                hit.transform.GetComponent<Agent>().imDrained = true;
                hit.transform.GetComponent<Agent>().StartDrain = Manager.Turn;
            }
        }
    }

    void INeedLifeDrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, 3))
        {
            Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);
            if (hit.transform.tag == "Player" && hit.transform != transform)
            {
                Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                if (GetComponent<LifeManager>().Life == 5)
                {
                    GetComponent<LifeManager>().Life++;
                }
                else if (GetComponent<LifeManager>().Life < 5)
                {
                    GetComponent<LifeManager>().Life += 2;
                }
                hit.transform.GetComponent<LifeManager>().Life -= 2;
                hit.transform.GetComponent<Agent>().imDrained = true;
                hit.transform.GetComponent<Agent>().StartDrain = Manager.Turn;
            }
        }
        if (Physics.Raycast(GetComponent<Agent>().RayLeft + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, 3))
        {
            Debug.DrawRay(GetComponent<Agent>().RayLeft + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);
            if (hit.transform.tag == "Player" && hit.transform != transform)
            {
                Debug.DrawRay(GetComponent<Agent>().RayLeft + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                GetComponent<LifeManager>().Life += 2;
                hit.transform.GetComponent<LifeManager>().Life -= 2;
                hit.transform.GetComponent<Agent>().imDrained = true;
                hit.transform.GetComponent<Agent>().StartDrain = Manager.Turn;
            }
        }
        if (Physics.Raycast(GetComponent<Agent>().RayRight + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, 3))
        {
            Debug.DrawRay(GetComponent<Agent>().RayRight + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.yellow);
            if (hit.transform.tag == "Player" && hit.transform != transform)
            {
                Debug.DrawRay(GetComponent<Agent>().RayRight + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.red);
                GetComponent<LifeManager>().Life += 2;
                hit.transform.GetComponent<LifeManager>().Life -= 2;
                hit.transform.GetComponent<Agent>().imDrained = true;
                hit.transform.GetComponent<Agent>().StartDrain = Manager.Turn;
            }
        }

    }

    public void Preview()
    {

        if (GetComponent<Agent>().MyTurn && GetComponent<Agent>().PlayerType == 2 && GetComponent<Agent>().ImStunned == false && Manager.CanAttack == true)

        {
            CleanPreview();
            float _lookX = GetComponent<Agent>().SavedlookAt.x;
            float _lookY = GetComponent<Agent>().SavedlookAt.z;
            Vector3 playerPosition = transform.position;

            List<CellPrefScript> cells = new List<CellPrefScript>();

            cells = FindObjectsOfType<CellPrefScript>().ToList();

            RaycastHit hit;

            if (GetComponent<LifeManager>().Life == 6)
            {
                if (Physics.Raycast(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(GetComponent<Agent>().RayCenter + new Vector3(0, 0.5f), GetComponent<Agent>().SavedlookAt * hit.distance, Color.black);
                    if (_lookX != 0)
                    {
                        CellsGreenInRay(hit.transform.position, cells, playerPosition);
                    }
                    else if (_lookY != 0)
                    {
                        CellsGreenInRay(hit.transform.position, cells, playerPosition);

                    }

                }
                else
                {

                    if (_lookX != 0)
                    {
                        if (_lookX > 0)
                        {
                            CellsGreenInRay(new Vector3(GetComponent<Agent>().configGrid.DimX, 0, transform.position.z), cells, playerPosition);
                        }
                        else if (_lookX < 0)
                        {
                            CellsGreenInRay(new Vector3(-1, 0, transform.position.z), cells, playerPosition);
                        }

                    }
                    else if (_lookY != 0)
                    {

                        if (_lookY < 0)
                        {
                            CellsGreenInRay(new Vector3(transform.position.x, 0, -1), cells, playerPosition);
                        }
                        else if (_lookY > 0)
                        {
                            CellsGreenInRay(new Vector3(transform.position.x, 0, GetComponent<Agent>().configGrid.DimY), cells, playerPosition);
                        }

                    }
                }
            }

            if (GetComponent<LifeManager>().Life < 6)
            {
                if (Physics.Raycast(transform.position + new Vector3(0, 0.5f), Vector3.forward, out hit, 4))
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition);
                }
                else
                {
                    CellsGreenInRay(new Vector3(transform.position.x, 0, (transform.position.z + 5)), cells, playerPosition);
                }

                if (Physics.Raycast(transform.position + new Vector3(0, 0.5f), Vector3.back, out hit, 4))
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition);
                }
                else
                {
                    CellsGreenInRay(new Vector3(transform.position.x, 0, (transform.position.z - 5)), cells, playerPosition);
                }

                if (Physics.Raycast(transform.position + new Vector3(0, 0.5f), Vector3.right, out hit, 4))
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition);
                }
                else
                {
                    CellsGreenInRay(new Vector3((transform.position.x + 5), 0, transform.position.z), cells, playerPosition);
                }

                if (Physics.Raycast(transform.position + new Vector3(0, 0.5f), Vector3.left, out hit, 4))
                {
                    CellsGreenInRay(hit.transform.position, cells, playerPosition);
                }
                else
                {
                    CellsGreenInRay(new Vector3((transform.position.x - 5), 0, transform.position.z), cells, playerPosition);
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
            cell.GetComponentInParent<CellPrefScript>().Color = cell.GetComponentInParent<CellPrefScript>().BaseColor;
        }
    }

    void CellsGreenInRay(Vector3 HitPosition, List<CellPrefScript> cells, Vector3 playerPosition)
    {
        foreach (CellPrefScript cell in cells)
        {
            if ((((playerPosition.x < cell.transform.position.x && cell.transform.position.x < HitPosition.x)

                ||

               (playerPosition.x > cell.transform.position.x && cell.transform.position.x > HitPosition.x)) &&


               (cell.transform.position.z == HitPosition.z)) ||



               (((playerPosition.z < cell.transform.position.z && cell.transform.position.z < HitPosition.z) ||
               (playerPosition.z > cell.transform.position.z && cell.transform.position.z > HitPosition.z)) &&
               (cell.transform.position.x == HitPosition.x)))
            {
                cell.GetComponentInParent<CellPrefScript>().Color = Color.green;
            }
        }
    }

}
