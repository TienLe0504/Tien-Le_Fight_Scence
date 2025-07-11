using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public Screen screenUI;
    public bool isBigModel = false;
    private void OnEnable()
    {
        ListenerManager.Instance.AddListener<float>(EventType.TakeDamage, TackDamage);
        ListenerManager.Instance.AddListener(EventType.TurnOnUI, TurnOnUI);
        ListenerManager.Instance.AddListener<DataUI>(EventType.dataUI, RecieveData);
        ListenerManager.Instance.AddListener(EventType.winGame, WinGame);
        ListenerManager.Instance.AddListener(EventType.loseGame, LoseGame);
    }
    private void OnDisable()
    {
        if (ListenerManager.Instance != null)
        {
            ListenerManager.Instance.RemoveListener<float>(EventType.TakeDamage, TackDamage);
            ListenerManager.Instance.RemoveListener(EventType.TurnOnUI, TurnOnUI);
            ListenerManager.Instance.RemoveListener<DataUI>(EventType.dataUI, RecieveData);
            ListenerManager.Instance.RemoveListener(EventType.winGame, WinGame);
            ListenerManager.Instance.RemoveListener(EventType.loseGame, LoseGame);
        }
    }
    public void StartGame()
    {
        if (isBigModel) { 
            isBigModel = false;
            ListenerManager.Instance.TriggerEvent(EventType.StartGameWith25Models);
        }
        ListenerManager.Instance.TriggerEvent(EventType.StartGame);
    }
    public void TackDamage(float damageAmount)
    {
        float baseHealth = LevelGenerator.Instance.playerBaseHealth;
        float healthPercent = damageAmount / baseHealth;
        screenUI.UpdateHealthTakeDamage(healthPercent);
    }
    public void TurnOnUI()
    {
        screenUI.TurnOnUI();
    }
    public void RecieveData(DataUI data)
    {
        screenUI.EffectUI(data);
    }
    public void WinGame()
    {
        screenUI.TurnOffInfoPlayer();
        EffectButton();
        screenUI.SetTextTitle(CONST.VICTORY);
    }

    private void EffectButton()
    {
        screenUI.ShowButtonContinue(screenUI.btnContinue);
        screenUI.ShowButtonContinue(screenUI.btnHomeScreen);
    }

    public void LoseGame()
    {
        screenUI.TurnOffInfoPlayer();
        EffectButton();
        screenUI.SetTextTitle(CONST.DEFEAT);
    }
    public void PlayGameContinue()
    {
        ListenerManager.Instance.TriggerEvent(EventType.continueGame);
    }
    public void HomeScreen()
    {
        ListenerManager.Instance.TriggerEvent(EventType.HomeScreen);
    }
    public void ActivateCombatPhase()
    {
        ListenerManager.Instance.TriggerEvent(EventType.ReadyGo);
    }
    public void StartModeWith25Models()
    {
        isBigModel = true;
    }
}
