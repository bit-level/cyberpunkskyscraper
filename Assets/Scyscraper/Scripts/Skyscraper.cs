using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Skyscraper : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField] Transform floorPrefab;
    [SerializeField] Transform firstFloor;
    [SerializeField] float floorMoovingSpeed = 1f;
    [SerializeField] Transform shanksWrap;
    [SerializeField] Transform trashWrap;
    [SerializeField] ParticleSystem smoke;
    [SerializeField] Transform canvas;
    [SerializeField] Transform perfectPrefab;
    [SerializeField] EventSystem eventSystem;

    [Header("Audio")]
    [SerializeField] AudioSource gameOver;
    [SerializeField] AudioSource tap;
#pragma warning restore 0649

    public enum State { ReadyToBuild, UnderBuild, Built }

    #region Fields

    private Dictionary<State, Action> statesActions;
    private Transform currentFloor;
    private Transform previousFloor;
    private Vector3 currentFloorPosition;
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 spawnPosition;
    private float time;
    private bool cheat;
    private bool hasCheatActiveOnce;

    public readonly float FloorHeight = 0.5f;
    #endregion

    #region Properties

    public int FloorsCount { get; private set; } = 0;
    public State CurrentState { get; private set; }
    public bool Cheat => cheat;
    public bool HasCheatActiveOnce => hasCheatActiveOnce;
    public static Skyscraper Instance { get; private set; }
    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        Instance = this;

        CurrentState = State.ReadyToBuild;
        statesActions = new Dictionary<State, Action>()
        {
            { State.ReadyToBuild, ReadyToBuildAction },
            { State.UnderBuild,   UnderBuildAction },
            { State.Built,        BuiltAction }
        };

        previousFloor = firstFloor;
    }

    void Update()
    {
        statesActions[CurrentState].Invoke();
        if (cheat) hasCheatActiveOnce = true;
    }
    #endregion


    #region Functions

    private void ReadyToBuildAction()
    {
        if (Input.GetMouseButtonDown(0) || Input.touches.Any(x => x.phase == TouchPhase.Began))
        {
            CurrentState = State.UnderBuild;
            CreateNewFloor();
        }
    }

    private void UnderBuildAction()
    {
        if (!cheat && (Input.GetMouseButtonDown(0) || Input.touches.Any(x => x.phase == TouchPhase.Began)))
        {
            if (!eventSystem.IsPointerOverGameObject())
                ProcessTap();
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButton(1))
        {
            // Cheat tap

            if (!eventSystem.IsPointerOverGameObject())
            {
                MakeCurrentPositionCorrect();
                ProcessTap();
            }
        }
#endif
        else if (Input.GetMouseButtonDown(0) || Input.touches.Any(x => x.phase == TouchPhase.Began))
        {
            if (!eventSystem.IsPointerOverGameObject())
            {
                MakeCurrentPositionCorrect();
                ProcessTap();
            }
        }

        UpdateCurrentFloorPosition();
        time += Time.deltaTime;
    }

    private void BuiltAction()
    {
        if (Input.GetMouseButtonDown(0) || Input.touches.Any(x => x.phase == TouchPhase.Began))
        {
            ClearData();
            CurrentState = State.ReadyToBuild;
        }
    }

    private void MakeCurrentPositionCorrect()
    {
        Vector3 correctPosition = previousFloor.localPosition;
        correctPosition.y = currentFloor.localPosition.y;

        currentFloor.localPosition = correctPosition;
    }

    private void ProcessTap()
    {
        float distance = GetDistanceBetweenCurrentAndPrevious2D();
        if (distance < .125f)
        {
            MakeCurrentPositionCorrect();
            Instantiate(smoke, previousFloor.transform);
            CreatePerfectMarkIfCanvasExists();
        }

        Transform shank;
        PutCurrentFloor(out shank);
        if (shank == null)
        {
            CurrentState = State.Built;
            gameOver.Play();
            return;
        }
        tap.Play();
        FloorsCount++;
        previousFloor = shank;
        moveDirection = (moveDirection == Vector3.right) ? Vector3.forward : Vector3.right;
        time = 0f;
        CreateNewFloor();
    }

    private float GetDistanceBetweenCurrentAndPrevious2D()
    {
        Vector3 c = currentFloor.localPosition;
        Vector3 p = previousFloor.localPosition;

        Vector2 current = new Vector2(c.x, c.z);
        Vector2 previous = new Vector2(p.x, p.z);

        return (current - previous).magnitude;
    }

    private void CreatePerfectMarkIfCanvasExists()
    {
        if (canvas != null)
        {
            Vector2 touchPosition;
#if UNITY_EDITOR
            touchPosition = Input.mousePosition;
#else
			touchPosition = Input.touches[0].position;
#endif
            touchPosition.x -= Screen.width / 2f;
            touchPosition.y -= Screen.height / 2f;

            Transform perfect = Instantiate(perfectPrefab, canvas);
            perfect.localPosition = touchPosition;
        }
    }

    private void CreateNewFloor()
    {
        float texTiling = (previousFloor.localScale.x + previousFloor.localScale.z) / 10f;
        currentFloor = InstantiateFloor(transform, UnityEngine.Random.Range(-20f, 20f), texTiling);
        currentFloor.name = string.Format("Floor_{0}", FloorsCount + 1);
        currentFloor.localScale = new Vector3(previousFloor.localScale.x, currentFloor.localScale.y, previousFloor.localScale.z);
        currentFloorPosition = Vector3.Scale(previousFloor.localPosition, Vector3.one - moveDirection) + moveDirection * floorPrefab.localScale.x;
        currentFloorPosition.y = previousFloor.localPosition.y + previousFloor.localScale.y / 2f + currentFloor.localScale.y / 2f;
        spawnPosition = currentFloorPosition;
        currentFloor.localPosition = currentFloorPosition;
    }

    private Transform InstantiateFloor(Transform parent, float textureOffset, float textureTiling)
    {
        var floor = Instantiate(floorPrefab, parent);
        var mr = floor.GetComponent<MeshRenderer>();
        mr.material.SetFloat("texture_offset", textureOffset);
        mr.material.SetFloat("texture_tiling", textureTiling);
        return floor;
    }

    private void UpdateCurrentFloorPosition()
    {
        if (currentFloor == null) return;

        currentFloorPosition = spawnPosition - moveDirection * Mathf.PingPong(time * floorMoovingSpeed, floorPrefab.localScale.x * 2f);

        currentFloor.localPosition = currentFloorPosition;
    }

    private void PutCurrentFloor(out Transform shank)
    {
        Transform trash;
        DivideCurrentFloor(out shank, out trash);
        if (trash != null)
        {
            var rb = trash.gameObject.AddComponent<Rigidbody>();
            rb.mass = trash.localScale.x * trash.localScale.y * trash.localScale.z;
        }
    }

    private void DivideCurrentFloor(out Transform shank, out Transform trash)
    {
        Vector3 shankScale, trashScale;
        CalculateShankAndTrashScales(out shankScale, out trashScale);

        bool isShankExists = Vector3.Scale(shankScale, moveDirection).magnitude > 0f;
        bool isTrashExists = Vector3.Scale(trashScale, moveDirection).magnitude > 0f;

        Vector3 shankPosition = Vector3.zero;
        Vector3 trashPosition = Vector3.zero;

        if (isShankExists)
            CalculateShankAndTrashPositions(out shankPosition, out trashPosition);
        else
            CalculateTrashPosition(out trashPosition);

        if (isShankExists)
        {
            Material mat = currentFloor.GetComponent<MeshRenderer>().material;
            shank = InstantiateFloor(shanksWrap, mat.GetFloat("texture_offset"), mat.GetFloat("texture_tiling"));
            shank.name = "Shank_" + (FloorsCount + 1);
            shank.localScale = shankScale;
            shank.localPosition = shankPosition;
        }
        else shank = null;

        if (isTrashExists)
        {
            Material mat = currentFloor.GetComponent<MeshRenderer>().material;
            trash = InstantiateFloor(trashWrap, mat.GetFloat("texture_offset"), mat.GetFloat("texture_tiling"));
            trash.name = "Trash_" + (FloorsCount + 1);
            trash.localScale = trashScale;
            trash.localPosition = trashPosition;
        }
        else trash = null;

        Destroy(currentFloor.gameObject);
    }

    private void CalculateShankAndTrashScales(out Vector3 shankScale, out Vector3 trashScale)
    {
        Vector3 shift = Vector3.Scale(currentFloor.localPosition - previousFloor.localPosition, moveDirection);
        float currentFloorScale = Vector3.Scale(currentFloor.localScale, moveDirection).magnitude;

        shankScale = Vector3.Scale(currentFloor.localScale, Vector3.one - moveDirection);
        trashScale = Vector3.Scale(currentFloor.localScale, Vector3.one - moveDirection);

        if (shift.magnitude == 0f)
            shankScale += moveDirection * currentFloorScale;
        else if (shift.magnitude > currentFloorScale)
            trashScale += moveDirection * currentFloorScale;
        else
        {
            shankScale += moveDirection * (currentFloorScale - shift.magnitude);
            trashScale += moveDirection * shift.magnitude;
        }
    }

    private void CalculateShankAndTrashPositions(out Vector3 shankPosition, out Vector3 trashPosition)
    {
        Vector3 shift = Vector3.Scale(currentFloor.localPosition - previousFloor.localPosition, moveDirection);
        float currentFloorScale = Vector3.Scale(currentFloor.localScale, moveDirection).magnitude;

        shankPosition = previousFloor.localPosition + shift / 2f;
        shankPosition.y = currentFloor.localPosition.y;

        trashPosition = shankPosition + shift.normalized * currentFloorScale / 2f;
    }

    private void CalculateTrashPosition(out Vector3 trashPosition)
    {
        trashPosition = currentFloor.localPosition;
    }

    private void ClearData()
    {
        DestroyAllChilds(shanksWrap);
        DestroyAllChilds(trashWrap);
        FloorsCount = 0;
        previousFloor = firstFloor;
        time = 0f;
    }

    private void DestroyAllChilds(Transform parent)
    {
        List<GameObject> childs = new List<GameObject>();
        for (int i = 0; i < parent.childCount; ++i)
            childs.Add(parent.GetChild(i).gameObject);
        foreach (GameObject child in childs)
            Destroy(child);
    }

    public void SwitchCheatState(UnityEngine.UI.Graphic graphic)
    {
        cheat = !cheat;
        if (graphic != null)
            graphic.color = (cheat) ? Color.green : Color.red;
    }
    #endregion
}
