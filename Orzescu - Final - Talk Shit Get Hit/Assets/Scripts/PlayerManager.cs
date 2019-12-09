using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{

    public int totalScore = 0;



    //Pushes variable data to all the other clients
    [SyncVar]
    public float currentSpeed;
    [SyncVar]
    public float currentMultiplier;
    [SyncVar]
    public int currentScore = 0;
    [SyncVar]
    public string currentShitTalkLine;

    //Shit Talk Canvas Elements
    public Canvas shitTalkCanvas;
    public Text shitTalkText;

    //UI Canvas Elements
    public Text player1ScoreText;
    public Text player2ScoreText;


    public Quaternion initialPlayerForward;

    void Awake()
    {
        SetDefaults();
        AssignShitTalkUI();
    }



    public void SetDefaults()
    {
        totalScore = 0;
        currentScore = 0;
        currentMultiplier = 1;
        currentSpeed = GameManager.instance.matchSettings.basePlayerMoveSpeed;

    }

    public void DecreaseScore(int _amount)
    {
        currentScore -= (int)(_amount * currentMultiplier);
        Debug.Log(transform.name + " now has " + currentScore + " score.");
    }

    public void IncreaseScore(int _amount)
    {
        currentScore += (int)(_amount * currentMultiplier);
        Debug.Log(transform.name + " now has " + currentScore + " score.");
    }

    public void ResetMultiplier()
    {
        currentMultiplier = 1;
    }

    public void ResetMoveSpeed()
    {
        currentSpeed = GameManager.instance.matchSettings.basePlayerMoveSpeed;
    }

    public void DecreaseSpeed()
    {
        currentSpeed -= GameManager.instance.matchSettings.moveSpeedDecay;
        if (currentSpeed < GameManager.instance.matchSettings.minimumPlayerMoveSpeed)
        {
            currentSpeed = GameManager.instance.matchSettings.minimumPlayerMoveSpeed;
        }
    }

    public void IncreaseMultiplier()
    {
        currentMultiplier *= GameManager.instance.matchSettings.scoreMultiplier;
        if (currentMultiplier > GameManager.instance.matchSettings.maximumMultiplier)
        {
            currentMultiplier = GameManager.instance.matchSettings.maximumMultiplier;
        }
    }

    [ClientRpc]
    public void RpcSetShitTalk(string shitTalkLine)
    {
        currentShitTalkLine = shitTalkLine;
        Debug.Log(transform.name + "'s Current Shit talk line is : " + currentShitTalkLine);
        shitTalkText.text = currentShitTalkLine;
    }

    void Start()
    {
        initialPlayerForward = transform.localRotation;
        player1ScoreText = GameObject.Find("Player 1ScoreText").GetComponent<Text>();
        player2ScoreText = GameObject.Find("Player 2ScoreText").GetComponent<Text>();


    }

    // Update is called once per frame
    void Update()
    {
        shitTalkCanvas.transform.rotation = initialPlayerForward;

        if (transform.name == "Player 1")
        {
            player1ScoreText.text = transform.name + " Score: " + currentScore + "   ";
        } 
        if (transform.name == "Player 2")
        {
            player2ScoreText.text = transform.name + " Score: " + currentScore + "   ";
        }
        
    }

    public void AssignShitTalkUI()
    {
        if (transform.Find("ShitTalkCanvas") != null)
        {
            shitTalkCanvas = transform.Find("ShitTalkCanvas").GetComponent<Canvas>();

            if (shitTalkCanvas.transform.Find("ShitTalkText").GetComponent<Text>() != null)
            {
                shitTalkText = shitTalkCanvas.transform.Find("ShitTalkText").GetComponent<Text>();
            }
            else {
                Debug.LogError("Could not find text child in Shit talk Canvas.");
            }

        }
        else
        {
            Debug.LogError("SHIT TALKER: Could not find canvas child on player prefab.");
        }
    }

    //Stuff Dealing with balls and score

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("BALL HIT ON PLAYER!");
            Ball localBallComponent = collision.gameObject.GetComponent<Ball>();

            if (gameObject.name != localBallComponent.originPlayerTag && localBallComponent.hasBounced == false)
            {
                IHitSomeoneWithABall(localBallComponent.originPlayerTag, localBallComponent.scoreIncrement);
                IGotHitByABall(gameObject.name, localBallComponent.scoreIncrement);
                localBallComponent.hasBounced = true;
            } else
            {
                localBallComponent.hasBounced = false;
            }


        }
    }

    [Client]
    void IGotHitByABall(string _playerID, int _scoreIncrement)
    {
        CmdPlayerGotHitByBall(_playerID, _scoreIncrement);
    }

    [Client]
    void IHitSomeoneWithABall(string _playerID, int _scoreIncrement)
    {
        CmdPlayerHitOtherPlayerWithBall(_playerID, _scoreIncrement);
    }

    [Command]
    void CmdPlayerHitOtherPlayerWithBall(string _playerID, int _scoreIncrement)
    {
        Debug.Log(transform.name + " hit someone with a ball!");
        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.IncreaseScore(_scoreIncrement);
        _player.ResetMoveSpeed();
        _player.ResetMultiplier();
    }

    [Command]
    void CmdPlayerGotHitByBall(string _playerID, int _scoreIncrement)
    {
        Debug.Log(_playerID + " Has been hit by a ball!");
        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.DecreaseScore(_scoreIncrement);
        _player.ResetMoveSpeed();
        _player.ResetMultiplier();

    }



}
