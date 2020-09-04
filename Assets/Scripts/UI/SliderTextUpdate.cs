using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SliderTextUpdate : MonoBehaviour {

    public Slider slider;
    public TMP_Text text;
    private GameManager gameManager;

    float value;
    public bool buying;
    public int max;
    public bool trade = false;

    public GameManager.Type itemType;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (trade) {
            GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), gameManager.currentlyBuyingFrom);

            if (buying) {
                max = gameManager.getMaxGoods(itemType, kingdom);
            }
            else {
                max = gameManager.getMaxPlayerGoods(itemType);
            }

            slider.maxValue = max;
        }
    }

    public void onLoad() {

        if (gameManager == null) {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        if (itemType == GameManager.Type.Wood) {
            slider.value = gameManager.woodValue;
        }

        if (itemType == GameManager.Type.Stone) {
            slider.value = gameManager.stoneValue;
        }

        if (itemType == GameManager.Type.Leather) {
            slider.value = gameManager.leatherValue;
        }
    }

    public void onValueChange(string s) {
        value = slider.value;
        text.text = value.ToString() + "%";
        
        switch (s) {
            case "Wood":
                gameManager.woodValue = (int) slider.value;
                break;
            case "Stone":
                gameManager.stoneValue = (int)slider.value;
                break;
            case "Leather":
                gameManager.leatherValue = (int)slider.value;
                break;
        }
    }

    public void onBuyOrSell() {
        text.text = (slider.value).ToString();
    }
}
