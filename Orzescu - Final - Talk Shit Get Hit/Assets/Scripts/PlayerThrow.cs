using UnityEngine;
using UnityEngine.Networking;

public class PlayerThrow : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";


    public GameObject dodgeballPrefab;
    public GameObject throwLocation;
    public GameObject throwDirection;
    public float forceForThrow;
    public float forceIncrease;

    public Camera playerCam;


    //Control what the raycast hits 
    public LayerMask mask;

    void Start()
    {
        forceIncrease = GameManager.instance.matchSettings.throwForceIncrease;
        if (playerCam == null)
        {
            Debug.LogError("PlayerThrow : No camera referenced");
            this.enabled = false;
        }
    }

    void Update()
    {
        //Debug.Log(ball.scoreIncrement);

        if (Input.GetButton("Fire1"))
        {
            forceForThrow += forceIncrease * Time.deltaTime;
            forceForThrow = Mathf.Clamp(forceForThrow, 0f, GameManager.instance.matchSettings.maximumThrowForce);
            Debug.DrawLine(playerCam.transform.position, playerCam.transform.forward, Color.blue);
            //Debug.Log(forceForThrow);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            ThrowBall(forceForThrow);
            Debug.Log("Threw ball with " + forceForThrow + " force.");
            forceForThrow = 0;
        }
    }

    //THIS IS ONLY EVER DONE ON THE CLIENT
    [Client]
    void ThrowBall(float _forceForThrow)
    {
        //Calculate throw direction here
        Vector3 dir = throwLocation.transform.position - throwDirection.transform.position;

        CmdPlayerThrewBall(transform.name, gameObject, throwLocation.transform.position, dir, _forceForThrow);

       
    }

    //COMMANDS, ARE METHODS THAT ARE ONLY CALLED ON THE SERVER

    [Command]
    void CmdPlayerThrewBall(string _playerID, GameObject playerObject, Vector3 throwOrigin, Vector3 throwDirection, float _newforceForThrow)
    {
        RpcThrowBall(transform.name ,playerObject, throwOrigin, throwDirection, _newforceForThrow);
        
    }

    [ClientRpc]
    void RpcThrowBall(string _playerID, GameObject playerObject, Vector3 throwOrigin, Vector3 throwDirection, float _newforceForThrow)
    {
        GameObject newBall = Instantiate(dodgeballPrefab, throwOrigin, playerObject.transform.rotation) as GameObject;
        Rigidbody newBallRigidbody = newBall.GetComponent<Rigidbody>();
        newBallRigidbody.AddForce(throwDirection * _newforceForThrow * -10);
        Ball ballInfo = newBall.GetComponent<Ball>();
        ballInfo.originPlayerTag = _playerID;
        newBall.name = _playerID + " Dodgeball";

    }


}



//OLD THROW BALL MECHANIC. MADE W/ RAYCASTS
//void ThrowBall()
//{
//    RaycastHit hit;

//    if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, ball.range, mask))
//    {
//        if (hit.collider.tag == PLAYER_TAG)
//        {
//            CmdPlayerGotHitByBall(hit.collider.name, ball.scoreIncrement);
//            CmdPlayerHitOtherPlayerWithBall(transform.name, ball.scoreIncrement);

//        }
//    }
//}