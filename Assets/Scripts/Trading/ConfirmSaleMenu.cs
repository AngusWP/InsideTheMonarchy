using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class ConfirmSaleMenu : MonoBehaviour {

    private GameManager gameManager;
    public GameObject buy1, buy2, buy3, sell1, sell2, sell3;
    public TMP_Dropdown dropdown;
    public GameObject coverBuy, coverSell, confirmSale, warningText, noGoldText;

    public TMP_Text costText, incomeText;

    public int minRelationBoost, maxRelationBoost;

    public TMP_Text relationJalonn, relationGalerd, relationRym, relationCobeth;

    private ActionMenu actionMenu;
    private Canvas canvas;

    public bool buying = true;
    
    void Start() {
        canvas = GetComponent<Canvas>();
        actionMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionMenu>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        coverSell.SetActive(true);
    }

    void Update() {
        
    }

    public void onSliderChange() {
        GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), gameManager.currentlyBuyingFrom);

        if (buying) {
            Slider buyS1, buyS2, buyS3;

            buyS1 = buy1.GetComponentInChildren<Slider>();
            buyS2 = buy2.GetComponentInChildren<Slider>();
            buyS3 = buy3.GetComponentInChildren<Slider>();
            
            int c = getSliderValue(buyS1, true) + getSliderValue(buyS2, true) + getSliderValue(buyS3, true);
            int percent = ((c / 100) * gameManager.taxFromTrade);

            if (gameManager.getTradeStatus(kingdom) == "Taxed Goods") {
                c += percent;
            }

            costText.text = "Cost: " + c;

        } else {
            Slider sellS1, sellS2, sellS3;

            sellS1 = sell1.GetComponentInChildren<Slider>();
            sellS2 = sell2.GetComponentInChildren<Slider>();
            sellS3 = sell3.GetComponentInChildren<Slider>();

            int i = getSliderValue(sellS1, false) + getSliderValue(sellS2, false) + getSliderValue(sellS3, false);
            int percent = ((i / 100) * gameManager.taxFromTrade);

            if (gameManager.getTradeStatus(kingdom) == "Taxed Goods") {
                i -= percent;
            }

            incomeText.text = "Income: " + i;
        }
    }

    public int getSliderCount(GameObject obj) {
        // so get the slider amount
        Slider s = obj.GetComponentInChildren<Slider>();
        return (int) s.value;
    }

    public void resetTradeSliders() {
        Slider buyS1 = buy1.GetComponentInChildren<Slider>();
        Slider buyS2 = buy2.GetComponentInChildren<Slider>();
        Slider buyS3 = buy3.GetComponentInChildren<Slider>();
        Slider sellS1 = sell1.GetComponentInChildren<Slider>();
        Slider sellS2 = sell2.GetComponentInChildren<Slider>();
        Slider sellS3 = sell3.GetComponentInChildren<Slider>();

        buyS1.value = 0;
        buyS2.value = 0;
        buyS3.value = 0;
        sellS1.value = 0;
        sellS2.value = 0;
        sellS3.value = 0;
    }

    private int getSliderValue(Slider s, bool buy) {
        GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), gameManager.currentlyBuyingFrom);
        SliderTextUpdate sliderTextUpdate = s.GetComponentInParent<SliderTextUpdate>();

        int max = s.GetComponentInParent<SliderTextUpdate>().max;

        if (buy) {
            return (int) s.value * gameManager.getCost(sliderTextUpdate.itemType, kingdom);
        } else {
            return gameManager.getCost(sliderTextUpdate.itemType, kingdom) * (int) s.value;
        }

    }

    public void onConfirmBuy() {

        if (gameManager.tradesLeft == 0) {
            warningText.SetActive(true);
            return;
        }

        if (buying) {
            if (costText.text == "Cost: 0") {
                return;
            }

            int cost = int.Parse(costText.text.Split(':')[1]);

            if (cost > gameManager.gold) {
                noGoldText.SetActive(true);
                return;
            }

            gameManager.gold -= cost;
            int woodAmount = getSliderCount(buy1);
            int stoneAmount = getSliderCount(buy2);
            int leatherAmount = getSliderCount(buy3);

            gameManager.wood += woodAmount;
            gameManager.stone += stoneAmount;
            gameManager.leather += leatherAmount;

        } else {
            if (incomeText.text == "Income: 0") {
                return;
            }

            int income = int.Parse(incomeText.text.Split(':')[1]);

            gameManager.gold += income;
            int woodAmount = getSliderCount(sell1);
            int stoneAmount = getSliderCount(sell2);
            int leatherAmount = getSliderCount(sell3);

            gameManager.wood -= woodAmount;
            gameManager.stone -= stoneAmount;
            gameManager.leather -= leatherAmount;
        }

        gameManager.tradesLeft -= 1;
        GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), gameManager.currentlyBuyingFrom);
        canvas.GetComponent<ResourceUI>().updateResourceText();
        gameManager.increaseRelations(kingdom, UnityEngine.Random.Range(minRelationBoost, maxRelationBoost));
        updateRelationText(kingdom);

        if (buying) {
            coverSell.SetActive(false);
        }
        else {
            coverBuy.SetActive(false);
        }

        resetTradeSliders();
        costText.text = "Cost: 0";
        dropdown.value = 0;
        coverSell.SetActive(true);
        incomeText.text = "Income: 0";

        confirmSale.SetActive(false);
        actionMenu.openObjects.Remove(confirmSale);
        actionMenu.active = false;
        actionMenu.taskOpen = false;
        gameManager.currentlyBuyingFrom = "";
    }

    public void setMaxValues(GameManager.Kingdom kingdom) {
        Slider b1, b2, b3, s1, s2, s3;

        b1 = buy1.GetComponentInChildren<Slider>();
        b2  = buy2.GetComponentInChildren<Slider>();
        b3 = buy3.GetComponentInChildren<Slider>();
        s1 = sell1.GetComponentInChildren<Slider>();
        s2 = sell2.GetComponentInChildren<Slider>();
        s3  = sell3.GetComponentInChildren<Slider>();

        b1.maxValue = gameManager.getMaxGoods(GameManager.Type.Wood, kingdom);
        b2.maxValue = gameManager.getMaxGoods(GameManager.Type.Stone, kingdom);
        b3.maxValue = gameManager.getMaxGoods(GameManager.Type.Leather, kingdom);
        s1.maxValue = gameManager.getMaxPlayerGoods(GameManager.Type.Wood);
        s2.maxValue = gameManager.getMaxPlayerGoods(GameManager.Type.Stone);
        s3.maxValue = gameManager.getMaxPlayerGoods(GameManager.Type.Leather);
    }

    public void updateRelationText(GameManager.Kingdom kingdom) {
        switch (kingdom) {
            case GameManager.Kingdom.Rym:
                relationRym.text = gameManager.getRelations(kingdom) + "%";
                break;

            case GameManager.Kingdom.Galerd:
                relationGalerd.text = gameManager.getRelations(kingdom) + "%";
                break;

            case GameManager.Kingdom.Cobeth:
                relationCobeth.text = gameManager.getRelations(kingdom) + "%";
                break;

            case GameManager.Kingdom.Jalonn:
                relationJalonn.text = gameManager.getRelations(kingdom) + "%";
                break;
        }
    }

    public void onValueChanged() {
        if (dropdown.value == 0) {
            coverBuy.SetActive(false);
            coverSell.SetActive(true);
            buying = true;
        } else {
            coverBuy.SetActive(true);
            coverSell.SetActive(false);
            buying = false;
        }
    }

}
