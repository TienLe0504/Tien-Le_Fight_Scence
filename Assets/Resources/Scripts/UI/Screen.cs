using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Screen : MonoBehaviour
{
    [Header("UI - General")]
    public Image backgroundScreen;
    public ScreenController controller;

    [Header("UI - Buttons")]
    public Button btnPlayGame;
    public Button btnContinue;
    public Button btnHomeScreen;
    public Button btnBigModel;

    [Header("UI - Player Info")]
    public TextMeshProUGUI txtGameMode;
    public TextMeshProUGUI txtLevelNumber;
    public Image avatarPlayer;
    public Image healthPlayer;

    [Header("UI - Effects")]
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI goText;
    public TextMeshProUGUI txtTitleGame;
    public Image BGShawdow;



    private void Awake()
    {
        TurnOffUI();
        btnContinue.onClick.AddListener(() => PlayGameContinue());
        btnPlayGame.onClick.AddListener(() => StartGame());
        btnHomeScreen.onClick.AddListener(() => HomeScreen());
        btnBigModel.onClick.AddListener(() => StartModeWith25Models());
    }
    public void HomeScreen()
    {
        TurnOffUI();
        backgroundScreen.gameObject.SetActive(true);
        controller.HomeScreen();
    }
    public void TurnOffUI()
    {
        BGShawdow.gameObject.SetActive(false);
        TurnOffInfoPlayer();
        readyText.gameObject.SetActive(false);
        goText.gameObject.SetActive(false);
    }

    public void TurnOffInfoPlayer()
    {
        txtGameMode.gameObject.SetActive(false);
        avatarPlayer.gameObject.SetActive(false);
    }

    public void TurnOnUI()
    {
        txtGameMode.gameObject.SetActive(true);
        avatarPlayer.gameObject.SetActive(true);
        UpdateHealthUI();
        ShowReadyGoSequence();
    }
    public void StartModeWith25Models()
    {
        controller.StartModeWith25Models();
        StartGame();
    }
    public void ShowReadyGoSequence()
    {
        readyText.gameObject.SetActive(true);
        PlayTextEffect(readyText, () =>
        {
            readyText.gameObject.SetActive(false);
            goText.gameObject.SetActive(true);
            PlayTextEffect(goText, () =>
            {
                goText.gameObject.SetActive(false);
                controller.ActivateCombatPhase();
            });
        });
    }

    private void PlayTextEffect(TextMeshProUGUI textObj, TweenCallback onComplete)
    {
        textObj.transform.localScale = Vector3.zero;
        textObj.alpha = 1f;

        textObj.transform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                textObj.DOFade(0f, 0.2f)
                    .SetEase(Ease.Linear)
                    .OnComplete(onComplete);
            });
    }
    public void TurnOnBGShadow()
    {
        BGShawdow.gameObject.SetActive(true);
    }
    public void UpdateHealthUI()
    {
        healthPlayer.fillAmount = 1;
    }
    public void UpdateHealthTakeDamage(float damagePerCent)
    {
        healthPlayer.fillAmount -= damagePerCent;
    }
    public void StartGame()
    {
        BGShawdow.gameObject.SetActive(false);
        backgroundScreen.gameObject.SetActive(false);
        controller.StartGame();
    }
    public void PlayGameContinue()
    {
        BGShawdow.gameObject.SetActive(false);
        controller.PlayGameContinue();
    }
    public void EffectUI(DataUI data)
    {
        txtGameMode.text = CONST.GAME_MOD +" " + data.gameMode;
        txtLevelNumber.text = CONST.LEVEL_MOD +" " + data.level.ToString();

        RectTransform rt = txtLevelNumber.GetComponent<RectTransform>();
        RectTransform rtGM = txtGameMode.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        rt.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rt.DOScale(1f, 0.15f).SetEase(Ease.InQuad);
            });
        txtGameMode.alpha = 0;
        Vector2 originalPos = rtGM.anchoredPosition;
        rtGM.anchoredPosition = originalPos + new Vector2(-100f, 0); 
        rtGM.DOAnchorPos(originalPos, 0.4f).SetEase(Ease.OutCubic);
        txtGameMode.DOFade(1f, 0.4f).SetEase(Ease.Linear);
    }
    public void SetTextTitle(string tile)
    {
        txtTitleGame.text = tile;
        txtTitleGame.transform.localScale = Vector3.zero;
        txtTitleGame.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
    public void ShowButtonContinue(Button btnContinueGame)
    {
        btnContinueGame.gameObject.SetActive(true);
        BGShawdow.gameObject.SetActive(true);
        Color c = BGShawdow.color;
        c.a = 0;
        BGShawdow.color = c;
        BGShawdow.DOFade(0.1f, 0.2f);
        RectTransform rtBtn = btnContinueGame.GetComponent<RectTransform>();
        rtBtn.localScale = Vector3.zero;
        rtBtn.DOScale(1.1f, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                rtBtn.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
            });
    }

}
