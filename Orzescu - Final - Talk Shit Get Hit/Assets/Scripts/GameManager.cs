using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 game manager in scene. ");
        } else
        {
            instance = this;
        }
    }



    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    //Create a dictionary to store all the player's w/ their id's in the game
	public static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

    //Take player ID's w/ their comp
    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static PlayerManager GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    void Start()
    {
        instance = this;
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string _playerID in players.Keys)
    //    {
    //        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}


    #endregion
}
