using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    private MainGame mainGame;

    [SerializeField]
    private AIController AIController;

    public void Init(MainGame mainGame)
    {
        this.mainGame = mainGame;
        this.mainGame.OnPlayStart += (f) =>
        {
            if (AIController != null)
                AIController.enabled = true;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Ball"))
        {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            StartCoroutine(Goal());

            if (AIController != null)
                AIController.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Ball"))
        {
            other.transform.localPosition = new Vector3(other.transform.localPosition.x, 0.75f, other.transform.localPosition.z);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            StopAllCoroutines();

            if (AIController != null)
                AIController.enabled = true;
        }
    }

    private IEnumerator Goal()
    {
        yield return new WaitForSeconds(1f);

        if (tag.Contains("Player"))
            mainGame.IncreaseEnemyPoints(LooseReasonState.Goal);
        else
            mainGame.IncreasePlayerPoints(LooseReasonState.Goal);
    }
}
