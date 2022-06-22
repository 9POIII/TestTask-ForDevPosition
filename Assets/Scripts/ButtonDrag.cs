using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ButtonDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform m_RectTransform;
    private Camera m_MainCamera;
    private float m_xStartButtonCoord, m_yStartButtonCoord;
    
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool m_Available;
    [SerializeField] private Grid m_Grid;
    [SerializeField] private int m_ButtonNumber;
    
    public Building buildingPrefab;

    private void Awake()
    {
        m_xStartButtonCoord = gameObject.GetComponent<RectTransform>().localPosition.x;
        m_yStartButtonCoord = gameObject.GetComponent<RectTransform>().localPosition.y;
        m_RectTransform = GetComponent<RectTransform>();
        m_Grid.grid = new Building[m_Grid.gridSize.x, m_Grid.gridSize.y];
        m_MainCamera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);

            x = Mathf.RoundToInt(worldPosition.x);
            y = Mathf.RoundToInt(worldPosition.z);

            m_Available = true;

            if (x < 0 || x > m_Grid.gridSize.x - buildingPrefab.size.x) m_Available = false;
            if (y < 0 || y > m_Grid.gridSize.y - buildingPrefab.size.y) m_Available = false;

            if (m_Available && m_Grid.IsPlaceTaken(x, y, m_ButtonNumber)) m_Available = false;    
            
            if (m_Available)
            {
                m_Grid.PlaceFlyingBuilding(x, y, m_ButtonNumber);
                Instantiate(buildingPrefab, new Vector3(x, 0, y), Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().localPosition =
                    new Vector3(m_xStartButtonCoord, m_yStartButtonCoord, 0);
            }
        }
    }
}

