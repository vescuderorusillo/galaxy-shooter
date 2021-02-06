using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    private int _bestScore;
    
    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _scoreText.text = "Score: " + 0;
        _bestScoreText.text = "Best: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void CheckForBestScore(int currentScore)
    {
        if (currentScore > _bestScore)
        {
            _bestScore = currentScore;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            _bestScoreText.text = "Best: " + _bestScore;
        }
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0)
        {
            _livesImg.sprite = _liveSprites[currentLives];
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = string.Empty;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnResumeButtonClick()
    {
        _gameManager.ResumePlay();
    }

    public void OnBackToMainMenuButtonClicked()
    {
        _gameManager.OpenMainMenu();
    }
}
