
using UnityEngine;

public class PointMover : MonoBehaviour
{
    private MainGame mainGame;

    [SerializeField]
    private bool isPlayer;

    public void Init(MainGame mainGame)
    {
        this.mainGame = mainGame;

        mainGame.OnPlayStart += Move;
    }

    private void Move(bool isPlayerWin)
    {
        if (isPlayer)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                Consts.PointsPosZ[mainGame.PlayerScore]);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                -Consts.PointsPosZ[mainGame.EnemyScore]);
        }
    }

    private void OnDestroy()
    {
        mainGame.OnPlayStart -= Move;
    }
}
