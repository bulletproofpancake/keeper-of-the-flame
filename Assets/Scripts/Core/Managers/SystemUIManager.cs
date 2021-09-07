using System.Collections;
using UnityEngine;
using TMPro;

public class SystemUIManager : MonoBehaviour
{
    [Header("Transitions")]
    [SerializeField] private Animator transitions;
    
    [Header("Game Canvas")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject instructionsUI;
    [SerializeField] private GameObject gameEndUI;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
    }


    private void Start()
    {
        mainMenuUI.SetActive(true);
        instructionsUI.SetActive(false);
    }

    private void OnGameStart()
    {
        mainMenuUI.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(ToggleGame());
    }

    private IEnumerator ToggleGame()
    {
        AudioManager.Instance.Play("Click");
        StartCoroutine(TransitionFade());
        yield return new WaitForSeconds(1f);
        GameManager.Instance.StartGame();
    }

    public void ShowInstructions()
    {
        StartCoroutine(ToggleInstructions());
    }

    private IEnumerator ToggleInstructions()
    {
        AudioManager.Instance.Play("Click");
        StartCoroutine(TransitionFade());
        yield return new WaitForSeconds(1f);
        mainMenuUI.SetActive(false);
        instructionsUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(ToggleMainMenu());
    }

    private IEnumerator ToggleMainMenu()
    {
        AudioManager.Instance.Play("Click");
        StartCoroutine(TransitionFade());
        yield return new WaitForSeconds(1f);
        FindObjectOfType<LevelManager>().UnloadLevel();
        mainMenuUI.SetActive(true);
        instructionsUI.SetActive(false);
        gameEndUI.SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.Play("Click");
        Application.Quit();
    }

    private IEnumerator TransitionFade()
    {
        transitions.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        transitions.SetTrigger("FadeIn");

    }
    
}
