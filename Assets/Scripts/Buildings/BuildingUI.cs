using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {

    public Button barracks, tavern, market, watchtower, garrison, druid;
    public TMP_Text barracksCost, tavernCost, marketCost, watchtowerCost, garrisonCost, druidCost;

    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        loadCost();
        checkBought();
    }

    void Update() {
        
    }

    public void loadCost() {
        barracksCost.text = gameManager.barracksCost.ToString();
        tavernCost.text = gameManager.tavernCost.ToString();
        marketCost.text = gameManager.marketCost.ToString();
        watchtowerCost.text = gameManager.watchtowerCost.ToString();
        garrisonCost.text = gameManager.garrisonCost.ToString();
        druidCost.text = gameManager.druidCost.ToString();
    }

    public void checkBought() {

        if (gameManager == null) {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        Debug.Log("0");

        if (gameManager.hasBuilt("barracks")) {
            barracks.interactable = false;
        }

        if (gameManager.hasBuilt("tavern")) {
            Debug.Log("1");
            tavern.interactable = false;
        }

        if (gameManager.hasBuilt("market")) {
            market.interactable = false;
        }

        if (gameManager.hasBuilt("watchtower")) {
            watchtower.interactable = false;
        }

        if (gameManager.hasBuilt("garrison")) {
            garrison.interactable = false;
        }

        if (gameManager.hasBuilt("druid")) {
            druid.interactable = false;
        }
    }
}
