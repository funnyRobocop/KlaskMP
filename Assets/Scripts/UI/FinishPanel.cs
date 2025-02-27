using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject loosePanel;
    [SerializeField]
    private List<LevelPanel> levelPanel;


    public void ShowGameOver(List<MainGame.Score> scores)
    {
        var isPlayerWin = scores.Count(f => f.player > f.enemy) >= Consts.MaxWins;

        gameObject.SetActive(true);
        winPanel.SetActive(isPlayerWin);
        loosePanel.SetActive(!isPlayerWin);

        foreach (var item in levelPanel)
            item.gameObject.SetActive(false);

        for (int i = 0; i < scores.Count; i++)
        {
            levelPanel[i].gameObject.SetActive(true);
            levelPanel[i].UpdateView(i + 1, scores[i].player, scores[i].enemy);
        }
    }

    public void ShowRoundOver(bool isPlayerWin)
    {
        gameObject.SetActive(true);
        winPanel.SetActive(isPlayerWin);
        loosePanel.SetActive(!isPlayerWin);
    }
}
