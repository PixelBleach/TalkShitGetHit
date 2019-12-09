using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class Ball : NetworkBehaviour {

    //public string name = "Dodgeball";

    public string originPlayerTag;

    public int scoreIncrement = 50;
    public float range = 25;
    public bool hasBounced = false;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I bounced on something already. Ball out of play.");
        if (collision.gameObject.tag != "Player")
        {
            hasBounced = true;
        }
    }



}
