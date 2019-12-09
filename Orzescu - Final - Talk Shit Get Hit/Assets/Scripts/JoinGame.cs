using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : NetworkBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    public Text status;

    public GameObject roomListItemPrefab;

    public Transform roomListParent;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        networkManager.matchMaker.ListMatches(0,20,"",false,0,0,OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        if (matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        ClearRoomList();

        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGameObject = Instantiate(roomListItemPrefab);
            _roomListItemGameObject.transform.SetParent(roomListParent);

            roomList.Add(_roomListItemGameObject);
        }

        if (roomList.Count == 0)
        {
            status.text = "No active rooms at the moment.";
        }

    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }



}
