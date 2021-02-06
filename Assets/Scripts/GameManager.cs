using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isGameOver;

    [SerializeField]
    private bool _isMultiPlayerMode;

    [SerializeField]
    private GameObject _pauseMenu;

    private Animator _pauseMenuAnimator;

    private void Start()
    {
        _pauseMenuAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenu.SetActive(true);
            _pauseMenuAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public bool IsMultiPlayerMode()
    {
        return _isMultiPlayerMode;
    }

    public void ResumePlay()
    {
        _pauseMenuAnimator.SetBool("isPaused", false);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
