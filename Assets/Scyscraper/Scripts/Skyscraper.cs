using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skyscraper : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Transform floorPrefab;
    [SerializeField] Transform firstFloor;
    [SerializeField] float floorMoovingSpeedMin = .5f;
    [SerializeField] float floorMoovingSpeedMax = 1.5f;
    [SerializeField] Transform shanksWrap;
    [SerializeField] Transform trashWrap;
    [SerializeField] Transform canvas;
    [SerializeField] Transform perfectPrefab;

    [Header("Particle System")]
    [SerializeField] ParticleSystem blueSmoke;
    [SerializeField] ParticleSystem pinkSmoke;
    [SerializeField] ParticleSystem redSmoke;
    [SerializeField] ParticleSystem goldenSmoke;
    [SerializeField] ParticleSystem multicolorSmoke;

    [Header("Materials")]
    [SerializeField] Material blue;
    [SerializeField] Material pink;
    [SerializeField] Material red;
    [SerializeField] Material gold;

    [Header("Audio")]
    [SerializeField] AudioSource gameOver;
    [SerializeField] AudioSource tap;
#pragma warning restore 0649

    public float perfectDistance = .125f;
    public float PerfectDistanceDefault { get; private set; }
    public FloorColorSet currentColorSet = FloorColorSet.Blue;
    public bool builtLock;

    public enum State { ReadyToBuild, UnderBuild, Built }
    public enum FloorColorSet { Blue, Pink, Red, Gold, MultiColor }

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
    private Dictionary<FloorColorSet, Material[]> materialSets;
    private Dictionary<FloorColorSet, ParticleSystem> smokeSet;
    private float currentFloorMoovingSpeed;
    private int bestScore;

    public readonly float FloorHeight = 0.5f;
    #endregion

    #region Properties

    public int FloorsCount { get; private set; } = 0;
    public State CurrentState { get; private set; }
    public bool Cheat => cheat;
    public bool HasCheatActiveOnce => hasCheatActiveOnce;
    public static Skyscraper Instance { get; private set; }
    #endregion

    #region Events

    public delegate void MethodContainer();
    public event MethodContainer OnPerfectTap;
    public event MethodContainer OnGameStart;

    public delegate void MethodContainer2(int arg1, bool arg2);
    public event MethodContainer2 OnGameOver;
    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        Instance = this;

        PerfectDistanceDefault = perfectDistance;

        CurrentState = State.ReadyToBuild;
        statesActions = new Dictionary<State, Action>()
        {
            { State.ReadyToBuild, ReadyToBuildAction },
            { State.UnderBuild,   UnderBuildAction },
            { State.Built,        BuiltAction }
        };

        previousFloor = firstFloor;

        materialSets = new Dictionary<FloorColorSet, Material[]>
        {
            { FloorColorSet.Blue, new Material[] { blue } },
            { FloorColorSet.Pink, new Material[] { pink } },
            { FloorColorSet.Red, new Material[] { red } },
            { FloorColorSet.Gold, new Material[] { gold } },
            { FloorColorSet.MultiColor, new Material[] { blue, pink, red, gold } }
        };

        smokeSet = new Dictionary<FloorColorSet, ParticleSystem>
        {
            { FloorColorSet.Blue, blueSmoke },
            { FloorColorSet.Pink, pinkSmoke },
            { FloorColorSet.Red,  redSmoke },
            { FloorColorSet.Gold, goldenSmoke },
            { FloorColorSet.MultiColor, multicolorSmoke }
        };
    }

    void Update()
    {
        statesActions[CurrentState].Invoke();
    }
    #endregion

    #region Private Functions

    private void ReadyToBuildAction()
    {
        builtLock = true;
    }

    private void UnderBuildAction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Cheat click
            MakeCurrentPositionCorrect();
            ProcessTap();
            hasCheatActiveOnce = true;
        }

        UpdateCurrentFloorPosition();
        time += Time.deltaTime;
    }

    private void BuiltAction()
    {
        if (builtLock) return;

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
        if (distance < perfectDistance)
        {
            // Perfect tap
            MakeCurrentPositionCorrect();
            Instantiate(smokeSet[currentColorSet], previousFloor.transform);
            CreatePerfectMarkIfCanvasExists();
            OnPerfectTap();
        }

        Transform shank;
        PutCurrentFloor(out shank);
        if (shank == null)
        {
            CurrentState = State.Built;
            gameOver.Play();
            OnGameOver(FloorsCount, FloorsCount > bestScore);
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
        currentFloor = InstantiateFloor(transform);
        currentFloor.name = string.Format("Floor_{0}", FloorsCount + 1);
        currentFloor.localScale = new Vector3(previousFloor.localScale.x, currentFloor.localScale.y, previousFloor.localScale.z);
        currentFloorPosition = Vector3.Scale(previousFloor.localPosition, Vector3.one - moveDirection) + moveDirection * floorPrefab.localScale.x;
        currentFloorPosition.y = previousFloor.localPosition.y + previousFloor.localScale.y / 2f + currentFloor.localScale.y / 2f;
        spawnPosition = currentFloorPosition;
        currentFloor.localPosition = currentFloorPosition;
        Material[] materialSet = materialSets[currentColorSet];
        currentFloor.GetComponent<MeshRenderer>().material = materialSet[UnityEngine.Random.Range(0, materialSet.Length)];
        if (FloorsCount % 10 == 0) currentFloorMoovingSpeed = UnityEngine.Random.Range(floorMoovingSpeedMin, floorMoovingSpeedMax);
    }

    private Transform InstantiateFloor(Transform parent)
    {
        return Instantiate(floorPrefab, parent);
    }

    private void UpdateCurrentFloorPosition()
    {
        if (currentFloor == null) return;
        currentFloorPosition = spawnPosition - moveDirection * Mathf.PingPong(time * currentFloorMoovingSpeed, floorPrefab.localScale.x * 2f);
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
            shank = InstantiateFloor(shanksWrap);
            shank.name = "Shank_" + (FloorsCount + 1);
            shank.localScale = shankScale;
            shank.localPosition = shankPosition;
            shank.GetComponent<MeshRenderer>().material = mat;
        }
        else shank = null;

        if (isTrashExists)
        {
            Material mat = currentFloor.GetComponent<MeshRenderer>().material;
            trash = InstantiateFloor(trashWrap);
            trash.name = "Trash_" + (FloorsCount + 1);
            trash.localScale = trashScale;
            trash.localPosition = trashPosition;
            trash.GetComponent<MeshRenderer>().material = mat;
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
    #endregion

    #region Public Funcions

    public void Tap()
    {
        if (CurrentState == State.ReadyToBuild)
        {
            CurrentState = State.UnderBuild;
            CreateNewFloor();
            OnGameStart();
            bestScore = BestScore.Instance.Value;
            return;
        }

        if (CurrentState != State.UnderBuild) return;
        if (cheat) MakeCurrentPositionCorrect();
        ProcessTap();
    }

    public void SwitchCheatState(UnityEngine.UI.Graphic graphic)
    {
        cheat = !cheat;
        if (graphic != null)
            graphic.color = (cheat) ? Color.green : Color.red;
        hasCheatActiveOnce = true;
    }

    public void SetCurrentState(State state)
    {
        CurrentState = state;
    }
#endregion
}
