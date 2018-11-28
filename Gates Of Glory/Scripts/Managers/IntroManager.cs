using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour 
{

    public Animator mainCamAnim;

    public Button playButton;
    public Button quitButton;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void StartGameButton()
    {
        mainCamAnim.SetTrigger("StartGame");
        quitButton.GetComponent<Animator>().SetTrigger("Pressed");
    }

    public void QuitGameButton()
    {
        mainCamAnim.SetTrigger("QuitGame");
        playButton.GetComponent<Animator>().SetTrigger("Pressed");
    }

    public void AnimationEventStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AnimationEventQuitGame()
    {
        Application.Quit();
    }
}
