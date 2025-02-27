using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTriger : MonoBehaviour
{

    private MainGame mainGame;
    private Rigidbody strikerBody;

    public void Init(MainGame mainGame)
    {
        this.mainGame = mainGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Striker"))
        {
            strikerBody = other.GetComponent<Rigidbody>();
            strikerBody.isKinematic = false;
            strikerBody.linearVelocity = Vector3.zero;
            strikerBody.constraints = RigidbodyConstraints.None;

            other.GetComponent<StrikerMover>().enabled = false;

            StartCoroutine(Klask());
        }

        if (other.tag.Contains("Ball"))
        {
            var otherBody = other.GetComponent<Rigidbody>();
            otherBody.constraints = RigidbodyConstraints.None;
            otherBody.linearVelocity = new Vector3(otherBody.linearVelocity.x /2f, 0f, otherBody.linearVelocity.z / 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Striker"))
        {
            var body = other.GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.linearVelocity = Vector3.zero;
            body.constraints = RigidbodyConstraints.FreezeRotation;

            strikerBody = null;

            other.GetComponent<StrikerMover>().enabled = true;

            StartCoroutine(OnTriggerExitCrtn(other));
        }

        if (other.tag.Contains("Ball"))
        {
            StartCoroutine(OnTriggerExitCrtn(other));
        }
    }

    private IEnumerator Klask()
    {
        yield return new WaitForSeconds(1f);

        if (strikerBody != null && strikerBody.linearVelocity == Vector3.zero)
        {
            if (strikerBody.tag.Contains("Player"))
            {
                mainGame.IncreaseEnemyPoints(LooseReasonState.Klask);
                strikerBody.GetComponent<StrikerMover>().Restart(false);
            }
            else
            {
                mainGame.IncreasePlayerPoints(LooseReasonState.Klask);
                strikerBody.GetComponent<StrikerMover>().Restart(true);
            }
        }
    }

    private IEnumerator OnTriggerExitCrtn(Collider other)
    {
        yield return new WaitForSeconds(0.1f);

        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
}
