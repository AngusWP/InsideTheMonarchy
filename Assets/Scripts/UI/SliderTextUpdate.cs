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

    public void onValueChange() {
        value = slider.value;
        text.text = value.ToString() + "%";
        PlayerPrefs.SetInt(slider.gameObject.name + "Percent", (int) value);
    }

    public void onBuyOrSell() {
        text.text = (slider.value).ToString();
    }
}
