using UnityEngine;
using System.Collections;
using InControl;

public class CapsuleController : MonoBehaviour
{

    public float moveSpeed = 6f;
    public float rotationSpeed = 80f;

    private Rigidbody playerRigidbody;
    private Vector3 movement;
    private Vector3 rotation;

    void Awake ()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

	void Start ()
	{

	}
	
	void Update ()
	{
        
    }

    void FixedUpdate ()
    {
        InputDevice controller = InputManager.ActiveDevice;

        float leftHorizontal = controller.LeftStickX;
        float leftVertical = controller.LeftStickY;

        float rightHorizontal = controller.RightStickX;
        float rightVertical = controller.RightStickY;

        Move(leftHorizontal, leftVertical);
        if(rightHorizontal != 0f && rightVertical != 0) Rotate(rightHorizontal, rightVertical);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * moveSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Rotate(float h, float v)
    {
        rotation.Set(h, 0f, v);

        Quaternion newRotation = Quaternion.LookRotation(rotation);
        Quaternion currentRotation = Quaternion.Lerp(playerRigidbody.rotation, newRotation, rotationSpeed * Time.deltaTime);

        playerRigidbody.MoveRotation(currentRotation);
    }
}
