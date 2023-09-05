using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Game.Sound;

namespace Game.UI
{
    public sealed class UIManager : MonoBehaviour
    {

        public static UIManager instance{get; private set;}

        public enum UIState
        {
            Def, // the default state
            Pause,
            Main,
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
        GameObject mainUI,settingsUI,aboutUI,shopUI,howUI,gameUI,pauseUI,loseUI,winUI;
        [SerializeField]
        GameObject UIDisabler; // neat little trick to disable the ui components on start

        [SerializeField][Header("On the start the UI will be disabled")]
        
        internal readonly float uiDisableTime = 2.5f;

        public bool hasPausedGame{get;private set;}
        public bool canAccessPause{get;private set;}

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
            // at the start of the scene, we want the main panel to be active.
            currentUIState = UIState.Def;
            SwitchUIState(UIState.Main);

            // we want to delay the ui interaction on start.
            StartCoroutine(EnableUIOnStart());
        }

        void InitializeUI()
        {
            mainUI.SetActive(false);
            pauseUI.SetActive(false);
            settingsUI.SetActive(false);
            aboutUI.SetActive(false);
            shopUI.SetActive(false);
            howUI.SetActive(false);
            gameUI.SetActive(false);
            loseUI.SetActive(false);
            winUI.SetActive(false);
        }

        void Update()
        {
            #region Do the input manager thing here
                if(Input.GetKeyDown(KeyCode.Escape) && GameManagerOld.instance.isDiving)
                {
                    PauseUI();
                }
            #endregion
        }

        void SwitchUIState(UIState state)
        {

            currentUIState = state;

            InitializeUI(); // set all the ui components to false so that you can enable only one thing at once

            switch(currentUIState)
            {
                case UIState.Pause:
                    pauseUI.SetActive(true);
                    break;
                case UIState.Main:
                    mainUI.SetActive(true);
                    break;
                case UIState.Dive:
                    if(GameManagerOld.instance != null)
                    {
                        GameManagerOld.instance.startedDiving = true;
                        Invoke("StartDiveUI",GameManagerOld.instance.diveUIStartTime);
                    }
                    else
                        Debug.LogError("Unable to get the game manager script!");
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
                    currentUIState = UIState.Def;
                    break;
            }
        }

        // buttons
        #region Look later
            public void PlayButtonAudio()
            {
                AudioManager.instance.PlayInGame("uibuttonclick");
            }
        #endregion
        
        
        
        #region public ui functions
            public void DiveUI()
            {
                SwitchUIState(UIState.Dive);
            }
            public void BackUI()
            {
                SwitchUIState(UIState.Main);
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
        #endregion

        void PauseUI()
        {
            hasPausedGame = !hasPausedGame;
            pauseUI.SetActive(!pauseUI.activeSelf);
            gameUI.SetActive(!gameUI.activeSelf);
            GameManagerOld.instance.UpdateMouseState();
            if(hasPausedGame)
            {
                Time.timeScale = 0f;
                AudioManager.instance.increasingAmbientVolume = false;
            }
            else
            {
                Time.timeScale = 1f;
                AudioManager.instance.increasingAmbientVolume = true;
            }
        }

        public void RestartGame()
        {
            if(Time.timeScale == 0f)
                Time.timeScale = 1f;
            SceneManager.LoadScene("MainGame");
        }

        public void Resume()
        {
            PauseUI();
        }

        void StartDiveUI()
        {
            AudioManager.instance.PlayInGame("gameui");
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
