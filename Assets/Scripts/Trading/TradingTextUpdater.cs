using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TradingTextUpdater : MonoBehaviour {

    private GameManager gameManager;
    public TMP_Text relations, tradeStatus, woodCost, stoneCost, leatherCost;

    public bool changed = false;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        updatePrice(GetComponent<TradeListener>().kingdom);
        updateText(GetComponent<TradeListener>().kingdom);
    }

    void Update() {
        if (gameManager.isGamePaused()) {
            if (changed) return;
            updatePrice(GetComponent<TradeListener>().kingdom); // this only runs once.
            changed = true;
        }
    }

    public void updateText(GameManager.Kingdom kingdom) {

        if (gameManager == null) {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        relations.text = gameManager.getRelations(kingdom) + "%";
        tradeStatus.text = gameManager.getTradeStatus(kingdom);
    }

    public void updatePrice(GameManager.Kingdom kingdom) {
        woodCost.text = gameManager.getCost(GameManager.Type.Wood, kingdom).ToString();
        stoneCost.text = gameManager.getCost(GameManager.Type.Stone, kingdom).ToString();
        leatherCost.text = gameManager.getCost(GameManager.Type.Leather, kingdom).ToString();
    }
}
