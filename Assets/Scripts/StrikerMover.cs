using UnityEngine;

public class StrikerMover : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private AIController AIController;
    private Rigidbody body;

    private Vector3 defaultPos;
    private MainGame mainGame;

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

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.identity;

        var newPos = body.position + speed * Time.deltaTime * direction;

        if (newPos.x < -Consts.FieldWidth / 2f + Consts.StrikerRadius)
            newPos = new Vector3(-Consts.FieldWidth / 2f + Consts.StrikerRadius, newPos.y, newPos.z);
        else if (newPos.x > Consts.FieldWidth / 2f - Consts.StrikerRadius)
            newPos = new Vector3(Consts.FieldWidth / 2f - Consts.StrikerRadius, newPos.y, newPos.z);

        if (newPos.z < -Consts.FieldHeight / 2f + Consts.StrikerRadius)
            newPos = new Vector3(newPos.x, newPos.y, -Consts.FieldHeight / 2f + Consts.StrikerRadius);
        else if (newPos.z > Consts.FieldHeight / 2f - Consts.StrikerRadius)
            newPos = new Vector3(newPos.x, newPos.y, Consts.FieldHeight / 2f - Consts.StrikerRadius);

        body.MovePosition(newPos);
    }

    public void Stop()
    {
        body.linearVelocity = Vector3.zero;
    }

    public void Restart(bool isPlayerWin) 
    {
        Restart();
    }
    public void Restart()
    {
        transform.position = defaultPos;
        transform.rotation = Quaternion.identity;
        body.linearVelocity = Vector3.zero;

        if (AIController)
            AIController.enabled = true;
    }

    private void OnDestroy()
    {
        mainGame.OnPlayStart -= Restart;
        mainGame.OnNewGameStart -= Restart;
    }
}
