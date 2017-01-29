using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequence : MonoBehaviour {
    public EnemySpawnManager enemySpawner;
    public EnemySpawningSet[] levelData;
    public CrowdCreator crowdCreator;
    public GoalTracker goalTracker;
    public LevelWinToast levelWinToast;
    public LevelWinToast gameWinToast;
    public LevelWinToast lostToast;
    public NextLevelBeginToast beginToast;
    public ActionButtons actionButtons;
    int levelIndex = 0;
    bool isWinning = false;
    bool hasGameBegun = false;
    public GameObject tutorial;

	void Awake () {
        crowdCreator.LostEvent += LevelLost;
        goalTracker.levelWon += LevelWon;
	}

    private void LevelLost()
    {
        lostToast.RunToast(ResetGame);
    }

    private void LevelWon()
    {
        if (hasGameBegun == false)
            BeginGame();
        else
            StartWinSequence();
    }

    private void StartWinSequence()
    {
        if (isWinning)
            return;

        isWinning = true;
        enemySpawner.ClearEnemies();

        //TODO: Should probably disable buttons here...
        levelWinToast.RunToast(WinToastComplete);
    }

    private void BeginGame()
    {
        tutorial.SetActive(false);
        hasGameBegun = true;
        enemySpawner.SetUpNextLevel(levelData[levelIndex]);
        StartBeginToast();
    }

    private void WinToastComplete()
    {
        levelIndex++;
        if (levelIndex >= levelData.Length)
            WinGame();
        else
            SetUpNextLevel();
    }

    private void SetUpNextLevel()
    {
        enemySpawner.SetUpNextLevel(levelData[levelIndex]);
        LeanTween.delayedCall(1.0f, StartBeginToast);
    }

    private void StartBeginToast()
    {
        actionButtons.SetLevel(levelIndex + 1);
        beginToast.RunToast(FinishedBeginLevelToast);
    }

    private void FinishedBeginLevelToast()
    {
        enemySpawner.BeginLevel();
        crowdCreator.MakeUnitSpawner(crowdCreator.GetPercievedCrowdCenter());
        isWinning = false;
        //TODO: Should probably enable buttons here...
    }

    private void WinGame()
    {
        gameWinToast.RunToast(FinishedGameWinToast);
    }

    private void FinishedGameWinToast()
    {
        LeanTween.delayedCall(5.0f, ResetGame);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
