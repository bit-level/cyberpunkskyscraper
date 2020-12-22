using UnityEngine;

public class HologramCube : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField]
	private float moovingAmplitude = 1f;
	[SerializeField]
	private float moovingSpeed = 1f;
	[SerializeField]
	private float rotationSpeed = 1f;
#pragma warning restore 0649	

	private Vector3 _position;
	private float _startYCoord;

	private void Awake()
	{
		_position = transform.position;
		_startYCoord = _position.y;
	}

	private void FixedUpdate()
	{
		_position.y = _startYCoord + Mathf.Sin(Time.time * moovingSpeed) * moovingAmplitude; 
		transform.position = _position;

		transform.Rotate(Vector3.up * rotationSpeed, Space.World);
	}	
}
