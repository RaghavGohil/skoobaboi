/**
@RaghavGohil on github.

some conventions to follow while reading the program:
public variables and user-defined types are written with the PascalCase
private, protected variables and function params are written with the snakeCase
constants and readonly variables are written in MACROCASE
**/

using UnityEngine;
using System;

public enum GameState // We want to access the gamestate everywhere.
{
    gameStart,
    startedDiving,
    isDiving,
    lose,
    win,
}

public sealed class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    // Game state handling:

    [HideInInspector]
    public GameState CurrentGameState;

    // Events:

    public static event Action GameStartAction, StartedDivingAction, IsDivingAction, LoseAction, WinAction;
    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        // manager is singleton
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    void Start()
    {
        CurrentGameState = GameState.gameStart;
    }

    public void SwitchGameState(GameState newState)
    {

        CurrentGameState = newState;

        switch(newState)
        {
            case GameState.gameStart:
                GameStartAction?.Invoke();
                break;
            case GameState.startedDiving:
                StartedDivingAction?.Invoke();
                break;
            case GameState.isDiving:
                IsDivingAction?.Invoke();
                break;
            case GameState.lose:
                LoseAction?.Invoke();
                break;
            case GameState.win:
                WinAction?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

}
