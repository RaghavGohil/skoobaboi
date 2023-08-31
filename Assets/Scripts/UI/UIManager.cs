using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Game.Sound;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {

        public static UIManager instance;

        [SerializeField]
        GameObject mainUI,settingsUI,aboutUI,shopUI,howUI,gameUI,UIDisabler,pauseUI;
        [SerializeField][Tooltip("On the start the UI will be disabled")]
        float disableTime;

        public bool hasPausedGame{get;private set;}
        public bool canAccessPause{get;private set;}

        // Start is called before the first frame update
        void Awake()
        {
            StartCoroutine(EnableUIOnStart());
            InitializeUI();
        }

        void Start()
        {
            instance = this;
        }

        void InitializeUI()
        {
            mainUI.SetActive(true);
            pauseUI.SetActive(false);
            settingsUI.SetActive(false);
            aboutUI.SetActive(false);
            shopUI.SetActive(false);
            howUI.SetActive(false);
            gameUI.SetActive(false);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && GameManagerOld.instance.isDiving)
            {
                PauseUI();
            }
        }

        // buttons
        public void PlayButtonAudio()
        {
            AudioManager.instance.PlayInGame("uibuttonclick");
        }

        public void BackUI()
        {
            InitializeUI();
        }
        
        public void DiveUI()
        {
            mainUI.SetActive(false);
            GameManagerOld.instance.startedDiving = true;
            Invoke("StartDiveUI",GameManagerOld.instance.diveUIStartTime);
        }
        public void SettingsUI()
        {
            mainUI.SetActive(false);
            settingsUI.SetActive(true);
        }
        public void AboutUI()
        {
            mainUI.SetActive(false);
            aboutUI.SetActive(true);
        }
        public void ShopUI()
        {
            mainUI.SetActive(false);
            shopUI.SetActive(true);
        }
        public void HowUI()
        {
            mainUI.SetActive(false);
            howUI.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void YouLoseUI()
        {
            print("YouLose");
        }

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
            UIDisabler.SetActive(true);
            yield return new WaitForSeconds(disableTime);
            UIDisabler.SetActive(false);
        }

    }
}
