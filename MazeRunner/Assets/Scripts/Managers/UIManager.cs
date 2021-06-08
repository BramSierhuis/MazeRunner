using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Script to show/hide panels and to update values
public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject inGameUI;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject finishedUI;
    [SerializeField]
    private TextMeshProUGUI scoreTxt;
    [SerializeField]
    private TextMeshProUGUI coinsTxt;
    [SerializeField]
    private TextMeshProUGUI coinsLeftTxt;
    [SerializeField]
    private TextMeshProUGUI finalScoreTxt;
    #endregion

    #region Singleton
    [HideInInspector]
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    #region ShowPanels
    public void ShowMenu(bool active)
    {
        mainMenu.SetActive(active);
    }

    public void ShowInGameUI(bool active)
    {
        inGameUI.SetActive(active);
    }

    public void ShowGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    public void ShowFinishedUI(bool active)
    {
        finishedUI.SetActive(active);
    }
    #endregion

    #region Update Values
    public void SetScore(int score)
    {
        scoreTxt.text = score.ToString();
    }

    public void SetCoins(int coins)
    {
        coinsTxt.text = coins.ToString();
    }

    public void SetCoinsLeft(int coins)
    {
        coinsLeftTxt.text = coins.ToString();
    }

    public void SetFinalScore(int score)
    {
        finalScoreTxt.text = score.ToString();
    }
    #endregion
}
