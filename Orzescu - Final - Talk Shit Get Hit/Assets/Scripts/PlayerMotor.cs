using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    public Camera playerCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float camRotationY;
    private float currentCameraRotationY = 0f;

    public float cameraRotationLimit = 85f;

    private float animVerti;
    private float animHoriz;

    Animator anim;
    private Rigidbody rigidBody;

    void Start()
    {

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

    }

    //Gets a movement vector for this script
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void AnimateMovement(float _animVerti, float _animHoriz)
    {
        animVerti = _animVerti;
        animHoriz = _animHoriz;
    }

    //gets a rotational vector for this script
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _camRotationY)
    {
        camRotationY = _camRotationY;
    }

    //Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //take the velocity vector we passed to this script, and use it to move the player via rigidbody.moveposition()
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rigidBody.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
            anim.SetFloat("InputX", animHoriz);
            anim.SetFloat("InputZ", animVerti);
        } else
        {
            anim.SetFloat("InputX", 0f);
            anim.SetFloat("InputZ", 0f);
        }
    }

    //perform rotation from the rotation vector we passed the script
    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {
            rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(rotation));
            if (playerCamera != null)
            {
                currentCameraRotationY -= camRotationY;
                currentCameraRotationY = Mathf.Clamp(currentCameraRotationY, -cameraRotationLimit, cameraRotationLimit);

                playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotationY, 0f, 0f);
            }
        }
    }



}
