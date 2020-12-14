using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skyscraper : MonoBehaviour
{
	[SerializeField] Transform floorPrefab = null;
	[SerializeField] Transform firstFloor = null;
	[SerializeField] float floorMoovingSpeed = 1f;
	[SerializeField] Transform shanksWrap = null;
	[SerializeField] Transform trashWrap = null;

	public enum State { ReadyToBuild, UnderBuild, Built }

	private Dictionary<State, Action> statesActions;

	private Transform currentFloor;
	private Transform previousFloor;
	private Vector3 currentFloorPosition;
	private Vector3 moveDirection = Vector3.forward;
	private float time;
	private Vector3 spawnPosition;

	public int FloorsCount { get; private set; } = 0;
	public State CurrentState { get; private set; }

	public static Skyscraper Instance { get; private set; }
	public static readonly float FloorHeight = 1.5f;

	void Awake()
	{
		Instance = this;

		CurrentState = State.ReadyToBuild;
		statesActions = new Dictionary<State, Action>()
		{
			{ State.ReadyToBuild, ReadyToBuildAction },
			{ State.UnderBuild,   UnderBuildAction },
			{ State.Built, 		  BuiltAction }
		};

		previousFloor = firstFloor;
	}

	void Update()
	{
		statesActions[CurrentState].Invoke();
	}


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
		if (Input.GetMouseButtonDown(0) || Input.touches.Any(x => x.phase == TouchPhase.Began))
		{
			Transform shank;
			PutCurrentFloor(out shank);
			if (shank == null)
			{
				CurrentState = State.Built;
				FMODUnity.RuntimeManager.PlayOneShot("event:/Fail");
				return;
			}
			FMODUnity.RuntimeManager.PlayOneShot("event:/Floor");
			FloorsCount++;
			previousFloor = shank;
			moveDirection = (moveDirection == Vector3.right) ? Vector3.forward : Vector3.right;
			time = 0f;
			CreateNewFloor();
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


	private void CreateNewFloor()
	{
		currentFloor = Instantiate(floorPrefab, transform);
		currentFloor.name = string.Format("Floor_{0}", FloorsCount + 1);
		currentFloor.localScale = previousFloor.localScale;
		currentFloorPosition = Vector3.Scale(previousFloor.localPosition, Vector3.one - moveDirection) + moveDirection * floorPrefab.localScale.x;
		currentFloorPosition.y += floorPrefab.localScale.y;
		spawnPosition = currentFloorPosition;
		currentFloor.localPosition = currentFloorPosition;
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
			shank = Instantiate(floorPrefab, shanksWrap);
			shank.name = "Shank_" + (FloorsCount + 1);
			shank.localScale = shankScale;
			shank.localPosition = shankPosition;
		}
		else shank = null;

		if (isTrashExists)
		{
			trash = Instantiate(floorPrefab, trashWrap);
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
}
