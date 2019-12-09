using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;


public class ShitTalker : NetworkBehaviour {

    [Header("Shit Talk JSON Info")]
    string path; //String to path where JSON is
    string jsonString; //string to hold raw data from JSON

    private int randomShitTalkLine;
    private int shitTalkListLength;

    public ShitTalker localPlayerShitTalker;
    public ShitTalk localShitTalk;


    private string[] shitTalkLines;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            shitTalkListLength = localShitTalk.shitTalkLines.Length;
            randomShitTalkLine = Random.Range(0, shitTalkListLength);
            //Debug.Log("Random # = " + randomShitTalkLine);
            TalkSomeShit(randomShitTalkLine);
        }
    }

    void Awake()
    {
        SetShitTalking();
    }

    public void SetShitTalking()
    {
        localPlayerShitTalker = GetComponent<ShitTalker>();
        //Debug.Log(localPlayerShitTalker);
        localShitTalk.shitTalkLines = localPlayerShitTalker.AssignPlayerShitTalk();
        //Debug.Log(localShitTalk.shitTalkLines);

    }

    public string[] AssignPlayerShitTalk()
    {
        path = Application.streamingAssetsPath + "/ShitTalkLines.json";
        jsonString = File.ReadAllText(path);
        ShitTalk playerShitTalk = JsonUtility.FromJson<ShitTalk>(jsonString); //Take data from json, map it to shittalk class, then place it on playerShitTalk.

        //localShitTalk = playerShitTalk;

        //Debug.Log("Shit talk lines for " + gameObject.transform.name + " ");
        Debug.Log(playerShitTalk.shitTalkLines);

        //shitTalkArrayLength = playerShitTalk.shitTalkLines.Length;

        for (int i = 0; i < playerShitTalk.shitTalkLines.Length; i++)
        {
            //Debug.Log(playerShitTalk.shitTalkLines[i].ToString());
        }

        return playerShitTalk.shitTalkLines;
    }

    [Client]
    void TalkSomeShit(int _randomShitTalkLine)
    {
        string ShitTalkLine = localShitTalk.shitTalkLines[_randomShitTalkLine];
        CmdPlayerTalkedSomeShit(transform.name, ShitTalkLine);
    }

    [Command]
    void CmdPlayerTalkedSomeShit(string _playerID, string lineOfShitTalk)
    {
        Debug.Log(transform.name + " Started talking shit!");
        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.RpcSetShitTalk(lineOfShitTalk);
        _player.IncreaseMultiplier();
        _player.DecreaseSpeed();
    }



}

[System.Serializable]
public class ShitTalk
{
    public string[] shitTalkLines;
}
