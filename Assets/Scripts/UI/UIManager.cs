using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum LooseReasonState { Goal, Magnet, Klask }

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> looseReasons;

    [SerializeField]
    private FinishPanel gameOverPanel;
    [SerializeField]
    private FinishPanel roundOverPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject levelPanel;


    private void Start()
    {
        menuPanel.SetActive(true);
    }

    public IEnumerator ShowReason(LooseReasonState reason, Action callback = null) 
    {
        looseReasons[(int)reason].SetActive(true);

        yield return new WaitForSeconds(1f);

        foreach (var item in looseReasons)
            item.SetActive(false);

        if (callback != null)
            callback.Invoke();
    }

    public void ShowGameOverPanel(List<MainGame.Score> scores)
    {
        levelPanel.SetActive(false);
        gameOverPanel.ShowGameOver(scores);
    }

    public void ShowRoundOverPanel(bool isPlayerWin)
    {
        levelPanel.SetActive(false);
        roundOverPanel.ShowRoundOver(isPlayerWin);
    }

    public void StartMultiGame()
    {
    }
}
