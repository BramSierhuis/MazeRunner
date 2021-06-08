using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField]
    private TMP_InputField width;   //The width of the maze
    [SerializeField]
    private TMP_InputField height;  //The height of the maze
    [SerializeField]
    private DifficultySetting[] difficultySettings; //All the possible difficulties
    [SerializeField]
    private GameObject playerPrefab; //The player

    private DifficultySetting difficulty = null; //Current difficulty
    private int score; //Current score
    private int coins; //Coins left
    private GameObject player; //The spawned player
    private Camera mainCam; //The main (top-down) camera
    private bool alive = false; //Tracks if the player is still alive
    private bool hasInputError = true; //Stops the user from pressing play
    #endregion

    #region Singleton
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    private void Start()
    {
        //Set the main cam instantly, because it gets disabled once the player spawns
        mainCam = Camera.main;
    }

    private void Update()
    {
        //Cehck if game completed, show the approppriate screen
        if (alive)
        {
            if (coins < 1)
                Finish();
            else if (score < 1)
                GameOver();
        }
    }

    //Get called from the game over or finished menu, lets the user play again
    public void Replay()
    {
        UIManager.Instance.ShowMenu(true);
        UIManager.Instance.ShowGameOverUI(false);
        UIManager.Instance.ShowFinishedUI(false);
    }

    //Gets called when the user presses play. Start the game.
    public void StartGame()
    {
        //Make sure the width and height are valid
        if (hasInputError)
            return;

        //If the user hasn't selected a difficulty, set easy as default
        if (difficulty == null)
            difficulty = difficultySettings[(int)Difficulty.Easy];

        //Show the InGame UI, hide the menu
        UIManager.Instance.ShowMenu(false);
        UIManager.Instance.ShowInGameUI(true);

        //Generate the maze
        RecursiveBacktrackerGenerator.Instance.Generate(int.Parse(width.text), int.Parse(height.text));

        //Spawn the player
        Vector3 startPos = new Vector3(RecursiveBacktrackerGenerator.Instance.GetEntrance().x, 0.38f, RecursiveBacktrackerGenerator.Instance.GetEntrance().y);
        player = Instantiate(playerPrefab, startPos, Quaternion.identity);

        //Set the score and start the timer
        score = RecursiveBacktrackerGenerator.Instance.GetSurface() / 2 + 10;
        TimeManager.Instance.StartTimer();
        alive = true;

        //Lock the cursor for first person movement
        Cursor.lockState = CursorLockMode.Locked;

        //Switch sound
        SoundsPlayer.Instance.PlayIngameMusic();
    }

    #region Private helper methods
    //Gets called when the player collects all coins
    private void Finish()
    {
        EndGame();

        //Play the win sound, show finished UI
        SoundsPlayer.Instance.PlayFinish();
        UIManager.Instance.ShowInGameUI(false);
        UIManager.Instance.SetFinalScore(score);
        UIManager.Instance.ShowFinishedUI(true);
    }

    //Gets called when the players score is 0
    private void GameOver()
    {
        EndGame();

        //Play the gameover sound, show gameover UI
        SoundsPlayer.Instance.PlayGameOver();
        UIManager.Instance.ShowInGameUI(false);
        UIManager.Instance.SetCoinsLeft(coins);
        UIManager.Instance.ShowGameOverUI(true);
    }

    //Gets called when the player wins or dies
    private void EndGame()
    {
        //Stop the timer
        TimeManager.Instance.StopTimer();

        //Unlock cursor, remove player and maze, set player as dead
        alive = false;
        Cursor.lockState = CursorLockMode.None;
        Destroy(player);
        RemoveMaze();

        //As the player is removed, so is the FirstPerson camera, so enable the main cam again
        mainCam.enabled = true;

        //Play background music
        SoundsPlayer.Instance.PlayMenuMusic();
    }

    //Remove all objects that make up the maze
    private void RemoveMaze()
    {
        //Add all items off the maze to a list
        List<GameObject> mazeItems = new List<GameObject>();
        mazeItems.AddRange(GameObject.FindGameObjectsWithTag("Coin"));
        mazeItems.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        mazeItems.Add(GameObject.FindGameObjectWithTag("Ground"));

        //Remove all objects in the list
        foreach(GameObject item in mazeItems)
        {
            Destroy(item);
        }

        //Reset all values of the maze
        RecursiveBacktrackerGenerator.Instance.Reset();
    }
    #endregion

    #region Getters and Setters
    //Update the score and the UI
    public void SetScore(int score)
    {
        this.score = score;

        //Update UI
        UIManager.Instance.SetScore(score);

        //Change the pitch so sound keeps getting faster
        //Uses an inversely proportional function
        SoundsPlayer.Instance.SetInGamePitch(1f + 1f / (score + 5f));
    }

    public int GetScore()
    {
        return score;
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficultySettings[difficulty];
    }

    public DifficultySetting GetDifficulty()
    {
        return difficulty;
    }

    public void SetCoins(int coins)
    {
        this.coins = coins;

        //Update the ui
        UIManager.Instance.SetCoins(coins);
    }

    public void LowerCoins(int amount)
    {
        coins -= amount;

        //Update the UI
        UIManager.Instance.SetCoins(coins);
    }

    public void SetInputError(bool state)
    {
        hasInputError = state;
    }
    #endregion
}
