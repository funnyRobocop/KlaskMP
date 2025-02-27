using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{

    private Rigidbody body;
    private bool isOut;
    private Vector3 defaultPos;
    private MainGame mainGame;

    public void Init(MainGame mainGame)
    {
        this.mainGame = mainGame;
        mainGame.OnPlayStart += Restart;
        mainGame.OnNewGameStart += Restart;
    }

    public bool OnEnemySide()
    {
        return transform.position.z >= 0;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.solverIterations = 255;
        defaultPos = transform.position;
    }

    private void FixedUpdate()
    {
        var newPos = body.position;

        if (newPos.y >= 1.75f)
        {
            if (newPos.x < -Consts.FieldWidth / 2f + Consts.BallRadius || newPos.x > Consts.FieldWidth / 2f - Consts.BallRadius ||
                newPos.z < -Consts.FieldHeight / 2f + Consts.BallRadius || newPos.z > Consts.FieldHeight / 2f - Consts.BallRadius)
            {
                isOut = true;
                Debug.Log("Ball is out");

                mainGame.StartPlay(newPos.z < 0);
            }
            
            return;
        }

        if (isOut)
            return;

        if (newPos.x < -Consts.FieldWidth / 2f + Consts.BallRadius)
            newPos = new Vector3(-Consts.FieldWidth / 2f + Consts.BallRadius, newPos.y, newPos.z);
        else if (newPos.x > Consts.FieldWidth / 2f - Consts.BallRadius)
            newPos = new Vector3(Consts.FieldWidth / 2f - Consts.BallRadius, newPos.y, newPos.z);

        if (newPos.z < -Consts.FieldHeight / 2f + Consts.BallRadius)
            newPos = new Vector3(newPos.x, newPos.y, -Consts.FieldHeight / 2f + Consts.BallRadius);
        else if (newPos.z > Consts.FieldHeight / 2f - Consts.BallRadius)
            newPos = new Vector3(newPos.x, newPos.y, Consts.FieldHeight / 2f - Consts.BallRadius);

        if (body.position != newPos)
            body.position = newPos;
    }

    private void Restart(bool isPlayerWin)
    {
        transform.position = Consts.BallStartPos[isPlayerWin ? Random.Range(0,2) : Random.Range(2, 4)];

        transform.rotation = Quaternion.identity;
        body.linearVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.FreezePositionY;
        body.Sleep();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (body.IsSleeping())
            return;

        if (other.CompareTag("EnemyStriker") || other.CompareTag("PlayerStriker"))
            body.Sleep();
    }

    public void Restart()
    {
        transform.position = Consts.BallStartPos[Random.Range(0, 4)];

        transform.rotation = Quaternion.identity;
        body.linearVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.FreezePositionY;
        body.Sleep();
    }

    private void OnDestroy()
    {
        mainGame.OnPlayStart -= Restart;
        mainGame.OnNewGameStart -= Restart;
    }
}
