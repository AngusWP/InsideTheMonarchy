using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeListener : MonoBehaviour {

    private GameManager gameManager;
    public GameManager.Kingdom kingdom;
    private TradingTextUpdater tradingTextUpdater;
    public bool started = false;

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
    }

    public void updatePrices(GameManager.Kingdom k) {
        tradingTextUpdater.updatePrice(k);
    }
}
