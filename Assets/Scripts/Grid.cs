using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private bool m_PopUpPanelBool;
    
    public Vector2Int gridSize = new Vector2Int(20, 20);
    public Building[,] grid;
    
    [SerializeField] private ButtonDrag[] m_ButtonDrag;
    [SerializeField] private GameObject m_PopUpPanel;
    [SerializeField] private Text m_BuildingText;
    
    
    private void Awake()
    {
        grid = new Building[gridSize.x, gridSize.y];
        m_PopUpPanel.SetActive(false);
        m_PopUpPanelBool = true;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        if (m_PopUpPanelBool)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
 
            if (Physics.Raycast (ray, out hit))
            {
                m_PopUpPanel.SetActive(true);
                if (hit.collider.gameObject.CompareTag("Plane"))
                {
                    m_BuildingText.text = "There is no building";
                    m_PopUpPanelBool = false;
                }
                else
                {
                    m_BuildingText.text = "Building name: " + hit.collider.gameObject.name;
                    m_PopUpPanelBool = false;
                }
            }
        }
    }

    public void ExitButton()
    {
        m_PopUpPanel.SetActive(false);
        m_PopUpPanelBool = true;
    }


    public bool IsPlaceTaken(int placeX, int placeY, int numberReturn)
    {
        for (int x = 0; x < m_ButtonDrag[numberReturn].buildingPrefab.size.x; x++)
        {
            for (int y = 0; y < m_ButtonDrag[numberReturn].buildingPrefab.size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    public void PlaceFlyingBuilding(int placeX, int placeY, int numberReturn)
    {
        for (int x = 0; x < m_ButtonDrag[numberReturn].buildingPrefab.size.x; x++)
        {
            for (int y = 0; y < m_ButtonDrag[numberReturn].buildingPrefab.size.y; y++)
            {
                grid[placeX + x, placeY + y] = m_ButtonDrag[numberReturn].buildingPrefab;
            }
        }
    }
}

