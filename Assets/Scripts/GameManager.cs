using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isGameOver;

    [SerializeField]
    private bool _isMultiPlayerMode;

    [SerializeField]
    private GameObject _pauseMenu;

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
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
