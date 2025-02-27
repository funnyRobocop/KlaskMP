using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelPanel : MonoBehaviour
{

    [SerializeField]
    private int roundNumber;

    [SerializeField]
    private TextMeshProUGUI label;


    private MainGame mainGame;

    private void Awake()
    {
        mainGame = FindObjectOfType<MainGame>();

        mainGame.OnScoreChange += UpdateView;
    }

    public void UpdateView(int round, int player, int enemy)
    {
        label.text = string.Format("Round {0}\n{1} : {2}", round, player, enemy);
    }

    private void OnDestroy()
    {
        mainGame.OnScoreChange -= UpdateView;
    }
}
