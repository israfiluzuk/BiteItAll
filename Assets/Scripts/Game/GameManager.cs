using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField] Text scoreText;
    [SerializeField] Button buttonRestart;
    [SerializeField] GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0";
        if (panel.activeInHierarchy)
            panel.gameObject.SetActive(false);

        buttonRestart.gameObject.SetActive(false);
        buttonRestart.transform.localScale = Vector3.zero;
        buttonRestart.onClick.AddListener(() => RestartGame());
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void UpdadeScore(int score = 0, int multiplier = 1)
    {
        score *= multiplier;
        scoreText.text = score.ToString();
    }

    internal void Restart()
    {
        panel.gameObject.SetActive(true);
        buttonRestart.gameObject.SetActive(true);
        buttonRestart.transform.DOScale(1,.2f);
    }
}
