using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Sound;
using Game.HelperFunctions;
using static Game.UI.UIManager;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; // this instance can be accessed anywhere!

    public enum GameState // all the states that the game has (state machine)
    {
        Def,
        Menu,
        StartedDive, // initialize systems (including animation , camera sync) so that dive is possible (AS IN THE STARTEDDIVING STATE)
        Dive, // actually the game part (AS IN THE ISDIVING STATE)
        Shop,
        Lose,
        Win,
    }

    public GameState gameState { get; private set; }

    #region Game variables.. change this however you want
    
    public float diveTransitionTime { get; private set; }

    [SerializeField]
    GameObject character, lobbyCamera, playerCamera, player;
    [SerializeField]
    GameObject bounds;
    public Vector3 cameraRotation;
    #endregion

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        // helpers
        Helper.canLog = true;
        Helper.canDrawDebugRays = false;
        Helper.canDrawDebugSpheres = true;

        InitializeSystem();
    }

    public void InitializeSystem()
    {
        //set the observers
        SetObservers();

        //set the game state:
        this.gameState = GameState.Menu;

        //instance var
        diveTransitionTime = 4.0f;
        character.SetActive(true);
        bounds.SetActive(false);
        lobbyCamera.SetActive(true);
        playerCamera.SetActive(false);
        AudioManager.instance.PlayInGame("abovewater");

        UpdateMouseState(); // set the mouse to accessible first
    }

    void SwitchGameState(GameState gameState)
    {
        this.gameState = gameState;

        switch (gameState)
        {
            case GameState.Menu:
                break;
            
            case GameState.StartedDive:

                AudioManager.instance.increasingAmbientVolume = true;
                FishGenerator.instance.SpawnFishes();
                character.transform.GetComponent<Animator>().Play("Dive");
                lobbyCamera.GetComponent<Animator>().Play("Dive");
                StartCoroutine(ChangeGameStateToDive());
                Invoke("ActivateBoundsAndFog", 0.8f);
                break;
            
            case GameState.Dive:

                UpdateMouseState();

                character.SetActive(false);
                
                //camera syncing
                Vector3 playerPos = lobbyCamera.transform.position;
                cameraRotation = lobbyCamera.transform.eulerAngles;
                lobbyCamera.SetActive(false);
                player.transform.position = playerPos;
                playerCamera.SetActive(true);
                
                break;
            
            case GameState.Shop:
                break;
            case GameState.Lose:
                break;
            case GameState.Win:
                break;
            default:
                this.gameState = GameState.Def;
                break;
        }
    }

    #region Needed for diving part
    void ActivateBoundsAndFog()
    {
        bounds.SetActive(true);
        RenderSettings.fog = true;
    }

    IEnumerator ChangeGameStateToDive()
    {
        yield return new WaitForSeconds(diveTransitionTime);
        SwitchGameState(GameState.Dive);
    } 
    #endregion

    public void UpdateMouseState()
    {
        if (gameState == GameState.Dive)
        {
            if (!UIManager.instance.hasPausedGame)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    void SetObservers()
    {
        UIManager.instance.diveEvent += ChangeGameStateToStartedDive;
    }

    void OnDestroy() // unsub to all the delegates
    {
        UIManager.instance.diveEvent -= ChangeGameStateToStartedDive;
    }

    #region delegate functions to be added/removed in game
    void ChangeGameStateToStartedDive()
    {
        SwitchGameState(GameState.StartedDive);
    }
    #endregion
}
