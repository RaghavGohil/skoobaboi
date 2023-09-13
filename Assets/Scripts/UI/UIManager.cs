/**
UIManager script is responsible for handling:
all the ui elements in the game
the pause ui in the game and it's state
set an action for the dive event
**/

using UnityEngine;
using System; // for the delegates
using System.Collections; // for the IEnum
using UnityEngine.SceneManagement;
using Game.Sound;
using Game.HelperFunctions;

namespace Game.UI
{
    public sealed class UIManager : MonoBehaviour
    {

        public static UIManager instance{get; private set;}

        public enum UIState
        {
            Def, // the default state
            Pause,
            Menu,
            Game,
            Dive,
            Settings,
            About,
            How,
            Shop,
            Lose,
            Win,
        }

        public UIState currentUIState {get; private set;}
    
        [SerializeField]
        GameObject menuUI,settingsUI,aboutUI,shopUI,howUI,gameUI,pauseUI,loseUI,winUI;
        [SerializeField]
        GameObject UIDisabler; // neat little trick to disable the ui components on start

        const float uiDisableTime = 2.5f;

        public bool hasPausedGame{get;private set;}

        public event Action diveEvent; // if 'event' is there then there is no need to add the getter and setter
        public event Action loseEvent; // TODO: stop the player from moving in the future when the player wins the game
        public event Action winEvent;

        // Start is called before the first frame update
        void Awake()
        {
            InitializeUI();
            
            if(instance == null)
                instance = this;
            else
                Destroy(instance);
        }

        void Start()
        {
            
            this.currentUIState = UIState.Menu;

            SwitchUIState(UIState.Menu);

            // we want to delay the ui interaction on start.
            StartCoroutine(EnableUIOnStart());

        }

        void Update()
        {
            if(GameManager.instance != null)
            {
                if(Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.gameState == GameManager.GameState.Dive)
                {
                    PauseUI();
                }
            }
        }

        void InitializeUI()
        {
            menuUI.SetActive(false);
            pauseUI.SetActive(false);
            settingsUI.SetActive(false);
            aboutUI.SetActive(false);
            shopUI.SetActive(false);
            howUI.SetActive(false);
            gameUI.SetActive(false);
            loseUI.SetActive(false);
            winUI.SetActive(false);
        }

        public void SwitchUIState(UIState state) // only the game manager can change this
        {

            this.currentUIState = state;

            InitializeUI(); // set all the ui components to false so that you can enable only one thing at once

            switch(currentUIState)
            {
                case UIState.Pause:
                    hasPausedGame = true;

                    pauseUI.SetActive(true);

                    GameManager.instance?.UpdateMouseState(); // if the game manager is there, then update the cursor state
                    
                    Time.timeScale = 0f;
                    if(AudioManager.instance != null)
                        AudioManager.instance.increasingAmbientVolume = false;
                    
                    break;
                case UIState.Menu:
                    menuUI.SetActive(true);
                    break;
                case UIState.Dive:
                    diveEvent?.Invoke(); // null propagator to check if diveEvent is null or not (if !null -> execute)
                    if (GameManager.instance != null)
                        Invoke("StartDiveUI", GameManager.instance.diveTransitionTime);
                    else
                        Helper.Log("GameManager is missing!",2);
                    break;
                case UIState.Settings:
                    settingsUI.SetActive(true);
                    break;
                case UIState.About:
                    aboutUI.SetActive(true);
                    break;
                case UIState.How:
                    howUI.SetActive(true);
                    break;
                case UIState.Shop:
                    shopUI.SetActive(true);
                    break;
                case UIState.Lose:
                    loseUI.SetActive(true);
                    break;
                case UIState.Win:
                    winUI.SetActive(true);
                    break;
                case UIState.Game:
                    gameUI.SetActive(true);
                    break;
                default:
                    this.currentUIState = UIState.Def;
                    break;
            }
        }
        
        #region public ui functions
            public void PlayButtonAudio() // Play the ui button sound
            {
                AudioManager.instance?.PlayInGame("uibuttonclick");
            }
            public void DiveUI()
            {
                SwitchUIState(UIState.Dive);
            }
            public void BackUI()
            {
                SwitchUIState(UIState.Menu);
            }
            public void SettingsUI()
            {
                SwitchUIState(UIState.Settings);
            }
            public void AboutUI()
            {
                SwitchUIState(UIState.About);
            }
            public void HowUI()
            {
                SwitchUIState(UIState.How);
            }
            public void ShopUI()
            {
                SwitchUIState(UIState.Shop);
            }
            public void LoseUI()
            {
                SwitchUIState(UIState.Lose);
            }
            public void WinUI()
            {
                SwitchUIState(UIState.Win);
            }
            public void ExitGame() // plugged in unity
            {
                Application.Quit();
            }

            //Hardcoded this as this wont change anyway

            public void RestartGame()
            {
                if(Time.timeScale == 0f)
                    Time.timeScale = 1f;
                SceneManager.LoadScene("MainGame");
            }

            public void Resume()
            {
                if(hasPausedGame)
                {
                    SwitchUIState(UIState.Game);
                    hasPausedGame = false;
                    Time.timeScale = 1f;
                    if(AudioManager.instance != null)
                        AudioManager.instance.increasingAmbientVolume = true;
                }
            }

            //

        #endregion

        #region private ui functions
        void PauseUI()
        {
            if(gameUI.activeSelf) // only activate if the game is on
                SwitchUIState(UIState.Pause);
        }
        #endregion

        void StartDiveUI()
        {
            AudioManager.instance?.PlayInGame("gameui");
            gameUI.SetActive(true);
        }

        IEnumerator EnableUIOnStart()
        {
            UIDisabler.SetActive(true); // disables starting gui!!!!!!!!
            yield return new WaitForSeconds(uiDisableTime);
            UIDisabler.SetActive(false); // enables starting gui!!!!!!!!!!
        }
    }
}
