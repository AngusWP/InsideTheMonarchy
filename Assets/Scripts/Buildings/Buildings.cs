using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {

    private GameManager gameManager;
    public Canvas canvas;
    private BuildingUI buildingUI;
    int gold;
    
    //make a sound effect for buying building, and not being able to buy building

    void Start() {
        gameManager = GetComponent<GameManager>();
        buildingUI = canvas.GetComponent<BuildingUI>();
    }

    void Update() {
        
    }

    public void onBarracksClick() {
        gold = (int) gameManager.gold;

        if (gold >= gameManager.barracksCost) {
            gameManager.gold -= gameManager.barracksCost;
            gameManager.ownsBarracks = true;
            buildingUI.barracks.interactable = false;
            gameManager.soldierStrength += 0.50f;
        }
    }

    public void onTavernClick() {
        gold = (int) gameManager.gold;


        if (gold >= gameManager.tavernCost) {
            gameManager.gold -= gameManager.tavernCost;
            gameManager.ownsTavern = true;
            buildingUI.tavern.interactable = false;
        }
    }

    public void onMarketClick() {
        gold = (int)gameManager.gold;


        if (gold >= gameManager.marketCost) {
            gameManager.gold -= gameManager.marketCost;
            gameManager.ownsMarket = true;
            buildingUI.market.interactable = false;
            gameManager.happiness += 15;

            if (gameManager.happiness > 100) {
                gameManager.happiness = 100;
            }
        }
    }

    public void onWatchtowerClick() {
        gold = (int)gameManager.gold;


        if (gold >= gameManager.watchtowerCost) {
            gameManager.gold -= gameManager.watchtowerCost;
            gameManager.ownsWatchtower = true;
            buildingUI.watchtower.interactable = false;
        }
    }

    public void onGarrisonClick() {
        gold = (int)gameManager.gold;


        if (gold >= gameManager.garrisonCost) {
            gameManager.gold -= gameManager.garrisonCost;
            gameManager.ownsGarrison = true;
            buildingUI.garrison.interactable = false;
            gameManager.soldierStrength += 0.10f;
        }
    }

    public void onDruidClick() {
        gold = (int)gameManager.gold;


        if (gold >= gameManager.druidCost) {
            gameManager.gold -= gameManager.druidCost;
            gameManager.ownsDruid = true;
            buildingUI.druid.interactable = false;
        }
    }
}
