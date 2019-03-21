using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCam : MonoBehaviour
{
    [Tooltip ("If the cursor is hidden.")]
    public bool showCursor = true;
    public Vector2 speed = new Vector2(120, 120);
    public float pitchMinLimit = -80, pitchMaxLimit = 80;

    private float x, y;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;                
        y = angles.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        x += Input.GetAxis("Mouse X") * speed.x * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * speed.y * Time.deltaTime;
        y = Mathf.Clamp(y, pitchMinLimit, pitchMaxLimit);
        transform.parent.rotation = Quaternion.Euler(0, x, 0);
        transform.localRotation = Quaternion.Euler(y, 0, 0);
	}
}
