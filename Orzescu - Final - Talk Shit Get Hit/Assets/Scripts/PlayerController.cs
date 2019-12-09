using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    public float speed;
    public float lookSensitivity = 4;
    public bool isCrouching;

    private PlayerMotor playerMotor;
    private PlayerManager playerManager;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerManager = GetComponent<PlayerManager>();

    }

	void Update () {

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Get Axis from 0-1 to get what direction the player wants player to move
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //Get component vectors
        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        //Combine vomponent vectors into final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * playerManager.currentSpeed;

        //Call function from "PlayerMotor" to move the player
        playerMotor.Move(velocity);
        playerMotor.AnimateMovement(moveZ, moveX);

        //Calculate rotation as a 3D vector (turning the player left and right)
        float _yRotation = Input.GetAxisRaw("Mouse X");

        //Create rotation Vector
        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //Appy the rotation
        playerMotor.Rotate(_rotation);


        //Calculate rotation as a 3D vector (turning the player left and right)
        float _yCamRotation = Input.GetAxisRaw("Mouse Y");

        //Create rotation Vector
        float _CamRotation = _yCamRotation* lookSensitivity;

        //Appy the rotation
        playerMotor.RotateCamera(_CamRotation);

        //Press to talk some shit


    }

}
