using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MainGame : MonoBehaviour
{
    [Serializable]
    public struct Score
    {
        public int player;
        public int enemy;

        public Score(int player, int enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }
    }

    public Action<bool> OnPlayStart;
    public Action OnNewGameStart;
    public Action<int, int, int> OnScoreChange;

    public int PlayerScore { get; private set; }
    public int EnemyScore { get; private set; }

    public int MagnetOnPlayerSrikerCount { get; private set; }
    public int MagnetOnEnemySrikerCount { get; private set; }

    private UIManager uiManager;
    public bool isGameOn;

    private List<Score> scores = new List<Score>();

    [SerializeField]
    private GameObject gamePanel;


    private void Awake()
    {
        Input.multiTouchEnabled = false;

        FindObjectOfType<BallMover>(true).Init(this);

        foreach (var item in FindObjectsOfType<StrikerMover>(true))
            item.Init(this);
        foreach (var item in FindObjectsOfType<MagnetConnector>(true))
            item.Init(this);
        foreach (var item in FindObjectsOfType<HoleTriger>(true))
            item.Init(this);
        foreach (var item in FindObjectsOfType<GoalTrigger>(true))
            item.Init(this);
        foreach (var item in FindObjectsOfType<PointMover>(true))
            item.Init(this);

        uiManager = FindObjectOfType<UIManager>();

        isGameOn = true;
    }

    public void IncreaseMagnetOnPlayerStrikerCount()
    {
        MagnetOnPlayerSrikerCount++;

        if (MagnetOnPlayerSrikerCount >= 2)
        {
            IncreaseEnemyPoints(LooseReasonState.Magnet);
        }
    }
    public void IncreaseMagnetOnEnemyStrikerCount()
    {
        MagnetOnEnemySrikerCount++;

        if (MagnetOnEnemySrikerCount >= 2)
        {
            IncreasePlayerPoints(LooseReasonState.Magnet);
        }
    }

    public void IncreaseEnemyPoints(LooseReasonState reason)
    {
        if (!isGameOn)
            return;

        EnemyScore++;
        OnScoreChange?.Invoke(scores.Count + 1, PlayerScore, EnemyScore);

        isGameOn = false;
        StartCoroutine(uiManager.ShowReason(reason, () =>
        {
            if (EnemyScore > Consts.MaxPoints)
                RoundOver();
            else
                StartPlay(false);
        }));
    }

    public void IncreasePlayerPoints(LooseReasonState reason)
    {
        if (!isGameOn)
            return;

        PlayerScore++;
        OnScoreChange?.Invoke(scores.Count + 1, PlayerScore, EnemyScore);

        isGameOn = false;
        StartCoroutine(uiManager.ShowReason(reason, () =>
        {
            if (PlayerScore > Consts.MaxPoints)
                RoundOver();
            else
                StartPlay(true);
        }));
    }

    public void StartPlay(bool isPlayerWin)
    {
        OnScoreChange?.Invoke(scores.Count + 1, PlayerScore, EnemyScore);
        OnPlayStart?.Invoke(isPlayerWin);
        isGameOn = true;
        gamePanel.SetActive(true);
        MagnetOnPlayerSrikerCount = 0;
        MagnetOnEnemySrikerCount = 0;
    }

    public void StartRound()
    {
        OnScoreChange?.Invoke(scores.Count + 1, 0, 0);
        OnPlayStart?.Invoke(PlayerScore > EnemyScore);
        isGameOn = true;
        gamePanel.SetActive(true);
        MagnetOnPlayerSrikerCount = 0;
        MagnetOnEnemySrikerCount = 0;
        PlayerScore = 0;
        EnemyScore = 0;
    }

    public void StartNewGame()
    {
        scores.Clear();

        isGameOn = true;
        gamePanel.SetActive(true);
        MagnetOnPlayerSrikerCount = 0;
        MagnetOnEnemySrikerCount = 0;
        PlayerScore = 0;
        EnemyScore = 0;

        OnScoreChange?.Invoke(1, 0, 0);
        OnNewGameStart?.Invoke();
    }

    public void RoundOver()
    {
        scores.Add(new Score(PlayerScore, EnemyScore));
        gamePanel.SetActive(false);

        if (scores.Count(f => f.player > f.enemy) >= Consts.MaxWins || scores.Count(f => f.player < f.enemy) >= Consts.MaxWins)
        {
            uiManager.ShowGameOverPanel(scores);
        }
        else
        {
            uiManager.ShowRoundOverPanel(PlayerScore > EnemyScore);
        }
    }
}
