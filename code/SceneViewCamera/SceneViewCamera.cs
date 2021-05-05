using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SceneViewCamera : MonoBehaviour
{
	[SerializeField, Range(0.1f, 10f)]
	private float wheelRatio = 1.5f;
	
	[SerializeField, Range(0.1f, 10f)]
	private float rotateSpeed = 0.5f;
	
	private Vector3 preMousePos;
	public float dstToLook = 5;

	private Camera cam;

	private void Update()
	{
		MouseUpdate();
	}
	
	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private Vector3 getLookPos()
	{
		Vector3 p = transform.position;
		Vector3 f = transform.forward;
		return p + f * dstToLook;
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
		float dstAfter = dstToLook * Mathf.Pow(wheelRatio, delta);
		float mv = dstToLook - dstAfter;
		transform.position += transform.forward * mv;
		dstToLook = dstAfter;
	}

	private void MouseDrag(Vector3 mousePos)
	{
		Vector3 diff = mousePos - preMousePos;
		
		if (diff.magnitude < Vector3.kEpsilon)
			return;
		
		if (Input.GetMouseButton (2)) {
			float fov = cam.fieldOfView;
			float moveSpeed = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * dstToLook;
			Vector3 cp = transform.position;
			transform.Translate (-diff * Time.deltaTime * moveSpeed);
		} else if (Input.GetMouseButton (0)) {
			CameraRotate (new Vector2 (-diff.y, diff.x) * rotateSpeed);
		}

		preMousePos = mousePos;
	}
	
	public void CameraRotate(Vector2 angle)
	{
		Vector3 look = getLookPos();
		transform.RotateAround(look, transform.right, angle.x);
		transform.RotateAround(look, Vector3.up, angle.y);
	}
}
