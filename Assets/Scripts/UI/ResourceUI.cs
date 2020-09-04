using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour {

    public Slider slider1, slider2, slider3;
    public TMP_Text perc1, perc2, perc3;

    public GameObject resources;
    public TMP_Text woodNo, stoneNo, leatherNo;

    public int woodModifier, stoneModifier, leatherModifier;

    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        updateResourceText();
        StartCoroutine(showUI());
    }

    public IEnumerator showUI() {
        yield return new WaitForSeconds(1);
        resources.SetActive(true);
    }

    void Update() {
        
    }

    public int loadGoodsExpenses() {
        int woodPercent = getResourcePercent("WoodPercent");
        int leatherPercent = getResourcePercent("LeatherPercent");
        int stonePercent = getResourcePercent("StonePercent");

        float expense = (woodPercent * gameManager.woodCostPerPercent) + (leatherPercent * gameManager.leatherCostPerPercent) + (stonePercent * gameManager.stoneCostPerPercent);
        return (int) expense;
    }

    public void updateResourceText() {

        if (gameManager == null) {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        woodNo.text = gameManager.get("wood");
        stoneNo.text = gameManager.get("stone");
        leatherNo.text = gameManager.get("leather");
    }

    public void updateResources() {
        float population = gameManager.population;
        float woodPercent = getResourcePercent("WoodPercent");
        float leatherPercent = getResourcePercent("LeatherPercent");
        float stonePercent = getResourcePercent("StonePercent");

        float woodProduce = (population / woodModifier) * woodPercent;
        float stoneProduce = (population / stoneModifier) * stonePercent;
        float leatherProduce = (population / leatherModifier) * leatherPercent;

        gameManager.wood += woodProduce;
        gameManager.stone += stoneProduce;
        gameManager.leather += leatherProduce;

        gameManager.yearlyWoodProduce += (int) woodProduce;
        gameManager.yearlyStoneProduce += (int) stoneProduce;
        gameManager.yearlyLeatherProduce += (int) leatherProduce;
    }

    public void loadSliders() {
        int woodPercent = getResourcePercent("WoodPercent");
        int leatherPercent = getResourcePercent("LeatherPercent");
        int stonePercent = getResourcePercent("StonePercent");

        slider1.value = woodPercent / 100;
        slider2.value = leatherPercent / 100;
        slider3.value = stonePercent / 100;
    }

    public void setActive(bool b) {
        resources.SetActive(b);
    }

    public int getResourcePercent(string prefName) {
        return PlayerPrefs.GetInt(prefName);
    }
}
