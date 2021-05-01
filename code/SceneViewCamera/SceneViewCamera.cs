using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SceneViewCamera : MonoBehaviour
{
	[SerializeField, Range(0.1f, 10f)]
	private float wheelSpeed = 2f;
	
	[SerializeField, Range(0.1f, 10f)]
	private float moveSpeed = 0.5f;
	
	[SerializeField, Range(0.1f, 10f)]
	private float rotateSpeed = 0.5f;
	
	private Vector3 preMousePos;
	private Vector3 vc = new Vector3 (0, 0, 0);
	
	private void Update()
	{
		MouseUpdate();
		return;
	}
	
	private void MouseUpdate()
	{
		float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if (scrollWheel != 0.0f)
			MouseWheel(scrollWheel);
		
		if (Input.GetMouseButtonDown(0) ||
		   Input.GetMouseButtonDown(1) ||
		   Input.GetMouseButtonDown(2))
			preMousePos = Input.mousePosition;
		
		MouseDrag(Input.mousePosition);
	}
	
	private void MouseWheel(float delta)
	{
		transform.position += transform.forward * delta * wheelSpeed;
	}
	
	private void MouseDrag(Vector3 mousePos)
	{
		Vector3 diff = mousePos - preMousePos;
		
		if (diff.magnitude < Vector3.kEpsilon)
			return;
		
		if (Input.GetMouseButton (2)) {
			Vector3 cp = transform.position;
			transform.Translate (-diff * Time.deltaTime * moveSpeed);
			vc += transform.position - cp;
		} else if (Input.GetMouseButton (0)) {
			CameraRotate (new Vector2 (-diff.y, diff.x) * rotateSpeed);
		}

		preMousePos = mousePos;
	}
	
	public void CameraRotate(Vector2 angle)
	{
		transform.RotateAround(vc, transform.right, angle.x);
		transform.RotateAround(vc, Vector3.up, angle.y);
	}
}
