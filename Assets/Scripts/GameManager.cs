using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using static EmreUtils;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Tilemaps & Tiles")]
    [SerializeField] private Tilemap tileMap_Obstacles;
    [SerializeField] private Tilemap tileMap_Ground;
    [SerializeField] private Tilemap tileMap_Temp;
    [SerializeField] private TileBase groundTile;

    [Header("Preview Buildings")]
    [SerializeField] private GameObject BarracksPreview;
    [SerializeField] private GameObject HangarPreview;
    [SerializeField] private GameObject PowerPlantPreview;
    [SerializeField] private GameObject HeadquartersPreview;

    [Header("Actual Buildings")]
    [SerializeField] private GameObject BarracksActual;
    [SerializeField] private GameObject HangarActual;
    [SerializeField] private GameObject PowerPlantActual;
    [SerializeField] private GameObject HeadquartersActual;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image info_Portrait;
    [SerializeField] private TMPro.TextMeshProUGUI info_Header;
    [SerializeField] private TMPro.TextMeshProUGUI info_Description;
    [SerializeField] private UnityEngine.UI.Button Unit1Btn;
    [SerializeField] private UnityEngine.UI.Button Unit2Btn;
    [SerializeField] private UnityEngine.UI.Image Unit1BtnImg;
    [SerializeField] private UnityEngine.UI.Image Unit2BtnImg;
    [SerializeField] private Sprite RobinPortrait;
    [SerializeField] private Sprite LyndaPortrait;
    [SerializeField] private Sprite TankPortrait;
    [SerializeField] private Sprite GarbageTruckPortrait;

    [Header("Other")]
    [SerializeField] private Transform boxSelectPreview;


    private bool buildable;
    private bool currentlyBuilding;

    private GameObject previewBuilding;
    private List<Vector3Int> cellsToUnpaintLater = new List<Vector3Int>();
    private List<Vector3Int> cellsToDeleteLater = new List<Vector3Int>();
    private Vector3 boxSelectStartPosition;
    private List<GameObject> currentlySelectedUnits = new List<GameObject>();
    GameObject selected;

    void Start()
    {
        currentlyBuilding = false;
        buildable = true;
        boxSelectPreview.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click down event
        {
            boxSelectStartPosition = EmreUtils.GetMouseWorldPos();
            boxSelectPreview.gameObject.SetActive(true);

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                ClearCurrentlySelectedUnits();
            }

            if (currentlyBuilding && !EmreUtils.IsMouseOverUI() && buildable) //place building
            {
                currentlyBuilding = false;
                UnpaintCells();

                switch (previewBuilding.GetComponentInParent<Building>().GetBuildingType())
                {
                    case Building.BuildingType.Barracks:
                        Instantiate(BarracksActual, previewBuilding.transform.position, Quaternion.identity);
                        break;
                    case Building.BuildingType.PowerPlant:
                        Instantiate(PowerPlantActual, previewBuilding.transform.position, Quaternion.identity);
                        break;
                    case Building.BuildingType.Headquarters:
                        Instantiate(HeadquartersActual, previewBuilding.transform.position, Quaternion.identity);
                        break;
                    case Building.BuildingType.Hangar:
                        Instantiate(HangarActual, previewBuilding.transform.position, Quaternion.identity);
                        break;
                }
                GameObject.Destroy(previewBuilding);
                AstarPath.active.Scan();

                Unselect();
            }
            else if(!currentlyBuilding)
            {
                RaycastHit2D[] results = Physics2D.RaycastAll(GetMouseWorldPos(), new Vector3(0, 0, -1), 1000f); //raycast at mouse
                bool foundSomething = false;
                foreach (var item in results)
                {
                    if (item.transform.tag == "Unit" || item.transform.tag == "Building")
                    {
                        currentlySelectedUnits.Add(item.transform.gameObject);
                        item.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        SetSelected(item.transform.gameObject);
                        foundSomething = true;
                        break;
                    }

                }
                if (!foundSomething)
                {
                    ClearCurrentlySelectedUnits();
                    Unselect();
                }
            }

            UpdateBoxSelectPreview();
        }

        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)) //mouse dragging
        {
            if (Input.GetMouseButton(0))
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    ClearCurrentlySelectedUnits();
                }

                Collider2D[] boxSelectedUnfiltered = Physics2D.OverlapAreaAll(boxSelectStartPosition, GetMouseWorldPos()); // "units" layer as layer mask
                if (boxSelectedUnfiltered.Length > 0)
                {
                    foreach (var unit in boxSelectedUnfiltered)
                    {
                        if (unit.gameObject.layer == 8 && !currentlySelectedUnits.Contains(unit.gameObject)) // if Units layer
                        {
                            currentlySelectedUnits.Add(unit.gameObject);
                            unit.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // left click up event
        {
            boxSelectPreview.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(1) && !currentlyBuilding && !EmreUtils.IsMouseOverUI()) //right click down event (only used for commanding units)
        {
            if (currentlySelectedUnits.Count > 0)
            {
                foreach (GameObject unit in currentlySelectedUnits)
                {
                    Vector2 randomPointsInCircle = UnityEngine.Random.insideUnitCircle * 2;

                    unit.GetComponent<AIPath>().destination = new Vector2(GetMouseWorldPos().x + randomPointsInCircle.x, GetMouseWorldPos().y + randomPointsInCircle.y);
                    unit.GetComponent<AIPath>().SearchPath();
                }
            }
        }

        UpdateBoxSelectPreview();
    }

    #region Main

    private void PreviewBuilding()
    {
        if (currentlyBuilding && previewBuilding != null)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition = new Vector2(Mathf.Round(mouseWorldPosition.x) - .5f, Mathf.Round(mouseWorldPosition.y) - .5f);

            if (previewBuilding.GetComponentInParent<Building>().GetBuildingType() == Building.BuildingType.PowerPlant)
            {
                previewBuilding.transform.position = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y - .5f);
            }
            else
            {
                previewBuilding.transform.position = mouseWorldPosition;
            }

            Vector3Int CellPosition = tileMap_Obstacles.WorldToCell(mouseWorldPosition);
            TraverseCells(mouseWorldPosition, CellPosition);
        }
        else
        {
            CancelInvoke("PreviewBuilding");
        }
    }

    private void TraverseCells(Vector2 mouseWorldPosition, Vector3Int pivotCell)
    {
        UnpaintCells();
        DeleteCells();

        buildable = true;

        if (currentlyBuilding && previewBuilding != null)
        {
            var occupiedCells = previewBuilding.GetComponentInParent<Building>().OccupiedCells;
            foreach (var cell in occupiedCells)
            {
                PaintTile(new Vector3Int(pivotCell.x + cell.x, pivotCell.y + cell.y, 0));
            }
        }
    }

    private void PaintTile(Vector3Int cellPosition)//Paint tiles red or green, depending on if the tile is null in "Obstacles" tilemap or not
    {
        TileBase obstacleTile = tileMap_Obstacles.GetTile(cellPosition); //Get reference to the actual obstacle tile using cell position
        TileBase groundTile = tileMap_Ground.GetTile(cellPosition); //Get reference to the actual ground tile using cell position

        if (obstacleTile != null) //if there is obstacle at this tile
        {
            tileMap_Obstacles.SetColor(cellPosition, Color.red);
            cellsToUnpaintLater.Add(cellPosition);
            buildable = false;
        }
        else //if null, there is no obstacle at this tile
        {
            Vector3 tempCellToWorld = tileMap_Ground.CellToWorld(cellPosition); //convert back to world pos from cell pos
            tempCellToWorld = new Vector3(tempCellToWorld.x + .5f, tempCellToWorld.y + .5f); //re-adjust world position (unsnap)

            if (!AstarPath.active.GetNearest(new Vector3(tempCellToWorld.x, tempCellToWorld.y, 0)).node.Walkable) //if there is building at this tile
            {
                tileMap_Temp.SetTile(cellPosition, groundTile);
                tileMap_Temp.SetColor(cellPosition, new Color(1, 0, 0, .7f));
                cellsToDeleteLater.Add(cellPosition);
                buildable = false;
            }
            else
            {
                tileMap_Ground.SetColor(cellPosition, Color.green);
                cellsToUnpaintLater.Add(cellPosition);
            }
        }
    }

    #endregion

    #region Helpers

    private void SetSelected(GameObject input)
    {
        selected = input;

        Unit1Btn.gameObject.SetActive(false);
        Unit2Btn.gameObject.SetActive(false);

        if (input.tag == "Unit")
        {
            switch (input.GetComponent<UnitBase>().GetUnitType().ToString())
            {
                case "Lynda":
                    info_Portrait.sprite = input.GetComponent<Lynda>().GetPortraitSprite();
                    break;
                case "Robin":
                    info_Portrait.sprite = input.GetComponent<Robin>().GetPortraitSprite();
                    break;
                case "Tank":
                    info_Portrait.sprite = input.GetComponent<Tank>().GetPortraitSprite();
                    break;
                case "GarbageTruck":
                    info_Portrait.sprite = input.GetComponent<GarbageTruck>().GetPortraitSprite();
                    break;
            }
            info_Header.text = input.GetComponent<UnitBase>().GetUnitType().ToString();
            info_Description.text = input.GetComponent<UnitBase>().GetUnitDescription().ToString();
        }
        else
        {
            info_Portrait.sprite = input.GetComponent<SpriteRenderer>().sprite;
            info_Header.text = input.GetComponent<Building>().GetBuildingType().ToString();
            info_Description.text = input.GetComponent<Building>().GetBuildingDescription();

            switch (input.GetComponent<Building>().GetBuildingType())
            {
                case Building.BuildingType.Barracks:
                    Unit1Btn.gameObject.SetActive(true);
                    Unit1BtnImg.sprite = RobinPortrait;
                    Unit2Btn.gameObject.SetActive(true);
                    Unit2BtnImg.sprite = LyndaPortrait;
                    break;

                case Building.BuildingType.Hangar:
                    Unit1Btn.gameObject.SetActive(true);
                    Unit1BtnImg.sprite = GarbageTruckPortrait;
                    Unit2Btn.gameObject.SetActive(true);
                    Unit2BtnImg.sprite = TankPortrait;
                    break;
            }
        }

        info_Portrait.gameObject.SetActive(true);
    }

    private void Unselect()
    {
        if (!EmreUtils.IsMouseOverUI())
        {
            try
            {
                selected.GetComponent<SpriteRenderer>().color = Color.white; //inside try because selected might be already null
            }
            catch (Exception)
            {

            }
            info_Portrait.gameObject.SetActive(false);
            info_Header.text = "";
            info_Description.text = "";

            Unit1Btn.gameObject.SetActive(false);
            Unit2Btn.gameObject.SetActive(false);
        }
    }

    private void UpdateBoxSelectPreview()
    {
        Vector3 boxSelectLowerLeft = new Vector3(
            Mathf.Min(boxSelectStartPosition.x, GetMouseWorldPos().x),
            Mathf.Min(boxSelectStartPosition.y, GetMouseWorldPos().y));
        Vector3 boxSelectUpperRight = new Vector3(
            Mathf.Max(boxSelectStartPosition.x, GetMouseWorldPos().x),
            Mathf.Max(boxSelectStartPosition.y, GetMouseWorldPos().y));

        boxSelectPreview.position = boxSelectLowerLeft;
        boxSelectPreview.localScale = boxSelectUpperRight - boxSelectLowerLeft;
    }

    #endregion

    #region Cleaners

    private void UnpaintCells()
    {
        if (cellsToUnpaintLater.Count > 0)
        {
            TileBase tile;

            foreach (var cell in cellsToUnpaintLater)
            {
                tile = tileMap_Obstacles.GetTile(cell);

                if (tile != null) //if the tile is in Obstacles tilemap
                {
                    tileMap_Obstacles.SetColor(cell, Color.white);
                }
                else //if the tile is in Ground tilemap
                {
                    tileMap_Ground.SetColor(cell, Color.white);
                }
            }

            cellsToUnpaintLater.Clear();
        }
    }

    private void DeleteCells()
    {
        if (cellsToDeleteLater.Count > 0)
        {
            foreach (var cellPosition in cellsToDeleteLater)
            {
                tileMap_Temp.SetTile(cellPosition, null);
            }
            cellsToDeleteLater.Clear();
        }
    }

    private void ClearCurrentlySelectedUnits()
    {
        foreach (var unit in currentlySelectedUnits)
        {
            unit.GetComponent<SpriteRenderer>().color = Color.white;
        }
        currentlySelectedUnits.Clear();
    }

    #endregion

    #region Building Spawners

    public void SpawnBarracks()
    {
        if (previewBuilding != null)
        {
            GameObject.Destroy(previewBuilding);
        }

        currentlyBuilding = true;

        Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previewBuilding = Instantiate(BarracksPreview, tempPos, Quaternion.identity);
        InvokeRepeating("PreviewBuilding", 0, 0.03f);
    }

    public void SpawnHangar()
    {
        if (previewBuilding != null)
        {
            GameObject.Destroy(previewBuilding);
        }

        currentlyBuilding = true;

        Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previewBuilding = Instantiate(HangarPreview, tempPos, Quaternion.identity);
        InvokeRepeating("PreviewBuilding", 0, 0.03f);
    }

    public void SpawnPowerPlant()
    {
        if (previewBuilding != null)
        {
            GameObject.Destroy(previewBuilding);
        }

        currentlyBuilding = true;

        Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previewBuilding = Instantiate(PowerPlantPreview, tempPos, Quaternion.identity);
        InvokeRepeating("PreviewBuilding", 0, 0.03f);
    }

    public void SpawnHeadquarters()
    {
        if (previewBuilding != null)
        {
            GameObject.Destroy(previewBuilding);
        }

        currentlyBuilding = true;

        Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previewBuilding = Instantiate(HeadquartersPreview, tempPos, Quaternion.identity);
        InvokeRepeating("PreviewBuilding", 0, 0.03f);
    }

    #endregion

    #region UI

    public void ProduceUnit1()
    {
        if (selected.TryGetComponent<Barracks>(out Barracks B))
        {
            selected.GetComponent<Barracks>().ProduceUnit(1);
        }
        else
        {
            selected.GetComponent<Hangar>().ProduceUnit(1);
        }
    }

    public void ProduceUnit2()
    {
        if (selected.TryGetComponent<Barracks>(out Barracks B))
        {
            selected.GetComponent<Barracks>().ProduceUnit(2);
        }
        else
        {
            selected.GetComponent<Hangar>().ProduceUnit(2);
        }
    }

    #endregion
}