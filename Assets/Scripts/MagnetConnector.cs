using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetConnector : MonoBehaviour
{

    private Rigidbody body;
    private MainGame mainGame;
    private Vector3 defaultPos;
    private GameObject lastFakeMagnet;


    public void Init(MainGame mainGame)
    {
        this.mainGame = mainGame;
        mainGame.OnPlayStart += Restart;
        mainGame.OnNewGameStart += Restart;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        defaultPos = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Striker"))
        {
            gameObject.SetActive(false);

            if (collision.gameObject.tag.Contains("Player"))
                mainGame.IncreaseMagnetOnPlayerStrikerCount();
            else
                mainGame.IncreaseMagnetOnEnemyStrikerCount();

            if (lastFakeMagnet != null && mainGame.MagnetOnPlayerSrikerCount < 2)
            {
                lastFakeMagnet.GetComponent<MeshRenderer>().enabled = true;
                lastFakeMagnet.GetComponent<MeshCollider>().isTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FakeMagnet")
        {
            lastFakeMagnet = other.gameObject;
        }
    }

    private void Restart(bool isPlayerWin)
    {
        Restart();
    }

    private void Restart()
    {
        gameObject.SetActive(true);

        transform.position = defaultPos;
        transform.rotation = Quaternion.identity;
        body.linearVelocity = Vector3.zero;

        if (lastFakeMagnet != null)
        {
            lastFakeMagnet.GetComponent<MeshRenderer>().enabled = false;
            lastFakeMagnet.GetComponent<MeshCollider>().isTrigger = true;
        }

        lastFakeMagnet = null;
    }

    private void OnDestroy()
    {
        mainGame.OnPlayStart -= Restart;
        mainGame.OnNewGameStart -= Restart;
    }
}
