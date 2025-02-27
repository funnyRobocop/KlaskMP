using UnityEngine;

public class AIController : MonoBehaviour
{
    private StrikerMover mover;
    private MainGame mainGame;

    private enum State { Protect, Attack }

    private State state { get; set; }

    private float tick;
    //private float stuckTick;
    private float stateChangeTime;
    private Rigidbody body;

    [SerializeField]
    private BallMover ballMover;
    [SerializeField]
    private Transform enemyGoal;
    [SerializeField]
    private Transform enemyGoalProtectPoint;
    [SerializeField]
    private Transform playerGoal;

    private void Awake()
    {
        mover = GetComponent<StrikerMover>();
        mainGame = FindObjectOfType<MainGame>();
        body = GetComponent<Rigidbody>();

        mainGame.OnPlayStart += (isPlayerWin) => 
        {
            if (isPlayerWin)
                ForceSetProtect();
            else
                ForceSetAttack();
        };

        mainGame.OnNewGameStart += () =>
        {
            if (ballMover.OnEnemySide())
                ForceSetAttack();
            else
                ForceSetProtect();
        };
    }
    private void Start()
    {
        if (ballMover.OnEnemySide())
            ForceSetAttack();
        else
            ForceSetProtect();
    }

    public void ForceSetProtect()
    {
        tick = 0;
        state = State.Protect;
        stateChangeTime = int.MaxValue;
    }

    public void ForceSetAttack()
    {
        tick = 0;
        state = State.Attack;
        stateChangeTime = Consts.MaxAIStateChangeTime * 2;
    }

    private void FixedUpdate()
    {
        if (!mainGame.isGameOn)
            return;

        if (stateChangeTime == int.MaxValue && ballMover.OnEnemySide())
            stateChangeTime = 0;

        /*if (state == State.Attack && (body.velocity.x < 0.1f || body.velocity.z < 0.1f))
            stuckTick += Time.fixedDeltaTime;
        else
            stuckTick = 0f;

        if (stuckTick > 1f && stateChangeTime < Consts.MaxAIStateChangeTime)
        {
            state = State.Protect;
            stuckTick = 0f;
        }*/
            
        tick += Time.fixedDeltaTime;

        Vector3 movement = Vector3.zero;

        if (state == State.Attack)
        {
            movement = Vector3.ClampMagnitude(new Vector3(ballMover.transform.position.x - transform.position.x, 0f, ballMover.transform.position.z + 0.2f - transform.position.z).normalized,
                Random.Range(0, Consts.MaxStrikerSpeed));
        } 
        else if (state == State.Protect)
        {
            enemyGoal.LookAt(ballMover.transform.position);

            if (Vector3.Distance(transform.position, enemyGoalProtectPoint.position) > 1f)
            {
                movement = Vector3.ClampMagnitude(new Vector3(enemyGoalProtectPoint.position.x - transform.position.x, 0f, enemyGoalProtectPoint.position.z - transform.position.z).normalized,
                    Random.Range(0, Consts.MaxStrikerSpeed));
            }
        }

        mover.Move(movement);

        if (tick < stateChangeTime)
            return;

        tick = 0;
        stateChangeTime = Random.Range(1f, Consts.MaxAIStateChangeTime);

        var rnd = Random.Range(0, 3);
        state = rnd == 0 ? State.Attack : State.Protect;
    }
}
