using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeListener : MonoBehaviour {

    private GameManager gameManager;
    public GameManager.Kingdom kingdom;
    private TradingTextUpdater tradingTextUpdater;
    public bool started = false;
    public Button rymB, cobethB, jalonnB, galerdB;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        tradingTextUpdater = GetComponent<TradingTextUpdater>();
        started = true; 
    }

    void Update() {
        
    }

    private void OnEnable() {

        if (tradingTextUpdater == null) {
            tradingTextUpdater = GetComponent<TradingTextUpdater>();
        }

        tradingTextUpdater.updateText(kingdom);

        if (gameManager.warStatus[GameManager.Kingdom.Cobeth] || gameManager.conqueredStatus[GameManager.Kingdom.Cobeth]) {
            cobethB.interactable = false;
        }

        if (gameManager.warStatus[GameManager.Kingdom.Rym] || gameManager.conqueredStatus[GameManager.Kingdom.Rym]) {
            rymB.interactable = false;
        }

        if (gameManager.warStatus[GameManager.Kingdom.Galerd] || gameManager.conqueredStatus[GameManager.Kingdom.Galerd]) {
            galerdB.interactable = false;
        }

        if (gameManager.warStatus[GameManager.Kingdom.Jalonn] || gameManager.conqueredStatus[GameManager.Kingdom.Jalonn]) {
            jalonnB.interactable = false;
        }
    }

    public void updatePrices(GameManager.Kingdom k) {
        tradingTextUpdater.updatePrice(k);
    }
}
