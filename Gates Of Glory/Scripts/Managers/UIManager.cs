using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private Camera mainCam;
    [HideInInspector]
    public PreBuiltCastleRoom[] prebuiltCastleRooms;

    private Animator mainCamAnim;

    [Header("Pause")]
    public GameObject pausePanel;
    private bool canPause = true;

    [Header("Wave")]
    public TextMeshProUGUI waveText;
    public Image waveHealthFill;
    public TextMeshProUGUI waveHealthText;

    [Header("Timeline Directors")]
    public PlayableDirector startGameTLDirector;
    public PlayableDirector gameOverTLDirector;

    [Header("Not Enough Gold Icon")]
    public GameObject notEnoughGoldIcon;
    public float notEnoughGoldIconDisplayTime;
    private float notEnoughGoldDisplayTimer;

    [Header("Other")]
    public GameObject placeObjectUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        mainCam = Camera.main;
        mainCamAnim = mainCam.GetComponent<Animator>();
        prebuiltCastleRooms = FindObjectsOfType<PreBuiltCastleRoom>();

        if (GameManager.instance.showStartGameAnimation)
        {
            StartCoroutine(StartGame());
        }
        else
        {
            //gameOverAnimator.enabled = false;
            canPause = true;

            startGameTLDirector.enabled = false;

            GameManager.instance.gameState = GameManager.GameState.Playing;

            WaveManager.instance.NextWave();
        }
    }

    private void Update()
    {
        if (notEnoughGoldDisplayTimer > 0)
        {
            notEnoughGoldDisplayTimer -= Time.deltaTime;
            notEnoughGoldIcon.SetActive(true);
            notEnoughGoldIcon.transform.position = Input.mousePosition;

            if (CursorManager.instance.CursorVisibilityStandard)
            {
                Cursor.visible = false;
            }
        }
        else
        {
            notEnoughGoldDisplayTimer = 0;
            notEnoughGoldIcon.SetActive(false);
            Cursor.visible = CursorManager.instance.CursorVisibilityStandard;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            PauseButton();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTimeScale();
        }
    }

    private IEnumerator StartGame()
    {
        GameManager.instance.gameState = GameManager.GameState.Cinematic;

        Cursor.visible = false;
        CursorManager.instance.ToggleCursorObject();
        canPause = false;

        CameraManager mainCamManager = mainCam.GetComponent<CameraManager>();
        mainCamManager.enabled = false;

        mainCamAnim.enabled = true;

        yield return new WaitForSeconds(11);

        mainCamAnim.enabled = false;
        mainCamManager.enabled = true;
        canPause = true;

        startGameTLDirector.enabled = false;

        if (CursorManager.instance.CursorVisibilityStandard)
        {
            Cursor.visible = true;
        }
        CursorManager.instance.ToggleCursorObject();

        GameManager.instance.gameState = GameManager.GameState.Playing;

        yield return new WaitForSeconds(1);

        WaveManager.instance.NextWave();
        GameManager.instance.showStartGameAnimation = false;
    }

    public IEnumerator GameOver()
    {
        CastleUpgradeManager.instance.CloseAllUI(null);
        canPause = false;
        GameManager.instance.gameState = GameManager.GameState.Cinematic;

        CameraManager mainCamManager = mainCam.GetComponent<CameraManager>();
        mainCamManager.enabled = false;

        gameOverTLDirector.Play();

        yield return new WaitForSeconds(1.2f);

        CursorManager.instance.ToggleCursorObject();
        mainCamAnim.enabled = true;
        mainCam.fieldOfView = 60;

        yield return new WaitForSeconds(9f);

        Time.timeScale = 0;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseButton()
    {
        if (!canPause)
        {
            return;
        }

        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            GameManager.instance.gameState = GameManager.GameState.Paused;

            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            GameManager.instance.gameState = GameManager.GameState.Playing;

            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void DisplayNotEnoughGoldIcon()
    {
        notEnoughGoldDisplayTimer = notEnoughGoldIconDisplayTime;
    }

    private void ToggleTimeScale()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 4;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
