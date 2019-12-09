using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    Camera sceneCamera;

    void Start()
    {
        //Check to see if this object is not controlled by the player, if it's not, go through and disable it from the player's client
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();

        } else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //Create playerUI

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = transform.name + "UI";

        }
    }

    public override void OnStartClient()
    {

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(_netID, _player);



    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);

        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);

    }


}
