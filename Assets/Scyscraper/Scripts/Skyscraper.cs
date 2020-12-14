using System;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : MonoBehaviour
{
	[SerializeField] Transform floorPrefab = null;
	[SerializeField] Transform firstFloor = null;
	[SerializeField] float floorMoovingSpeed = 1f;
	[SerializeField] Transform shanksWrap = null;
	[SerializeField] Transform trashWrap = null;

	public enum State { ReadyToBuild, UnderBuild, Built }

	public State currentState;
	private Dictionary<State, Action> statesActions;

	private Transform currentFloor;
	private Transform previousFloor;
	private Vector3 currentFloorPosition;
	private Vector3 moveDirection = Vector3.forward;
	private float timeOffset;

	private int height = 0;

	public static Skyscraper Instance { get; private set; }

	void Awake()
	{
		Instance = this;

		currentState = State.ReadyToBuild;
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
		statesActions[currentState].Invoke();
	}


	private void ReadyToBuildAction()
	{
		if (Input.GetMouseButtonDown(0))
		{
			currentState = State.UnderBuild;
			CreateNewFloor();
		}
	}

	private void UnderBuildAction()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Transform shank;
			PutCurrentFloor(out shank);
			if (shank == null)
			{
				print("Game Over");
				currentState = State.Built;
				return;
			}
			else height++;
			previousFloor = shank;
			moveDirection = (moveDirection == Vector3.right) ? Vector3.forward : Vector3.right;
			CreateNewFloor();
		}
		UpdateCurrentFloorPosition();
	}

	private void BuiltAction()
	{
	}


	private void CreateNewFloor()
	{
		timeOffset = UnityEngine.Random.Range(0f, 10f);
		currentFloor = Instantiate(floorPrefab, transform);
		currentFloor.name = string.Format("Floor_{0}", height + 1);
		currentFloor.localScale = previousFloor.localScale;
		currentFloorPosition =
			Vector3.Scale(previousFloor.localPosition, Vector3.one - moveDirection)
			+ moveDirection * floorPrefab.localScale.x * Mathf.Sin((Time.time + timeOffset) * floorMoovingSpeed);
		currentFloorPosition.y += floorPrefab.localScale.y;

		currentFloor.localPosition = currentFloorPosition;
	}

	private void UpdateCurrentFloorPosition()
	{
		if (currentFloor == null) return;
		float newCoordinate = Mathf.Sin((Time.time + timeOffset) * floorMoovingSpeed);
		currentFloorPosition.Scale(Vector3.one - moveDirection);
		currentFloorPosition += Vector3.Scale(floorPrefab.localScale, moveDirection) * newCoordinate;
		currentFloor.localPosition = currentFloorPosition;
	}

	private void PutCurrentFloor(out Transform shank)
	{
		Transform trash;
		DivideCurrentFloor(out shank, out trash);
		if (trash != null)
			trash.gameObject.AddComponent<Rigidbody>().mass = 5f;
	}

	private void DivideCurrentFloor(out Transform shank, out Transform trash)
	{
		Vector3 shankScale, trashScale;
		CalculateShankAndTrashScales(out shankScale, out trashScale);

		Vector3 shankPosition, trashPosition;
		CalculateShankAndTrashPositions(out shankPosition, out trashPosition);

		bool isShankExists = Vector3.Scale(shankScale, moveDirection).magnitude > 0f;
		bool isTrashExists = Vector3.Scale(trashScale, moveDirection).magnitude > 0f;

		if (isShankExists)
		{
			shank = Instantiate(floorPrefab, shanksWrap);
			shank.name = "Shank_" + (height + 1);
			shank.localScale = shankScale;
			shank.localPosition = shankPosition;
		}
		else shank = null;

		if (isTrashExists)
		{
			trash = Instantiate(floorPrefab, trashWrap);
			trash.name = "Trash_" + (height + 1);
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
}
