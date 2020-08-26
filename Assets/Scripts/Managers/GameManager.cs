using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float year = 1;
    public float gold = 500;
    public float yearlyExpenses = 50;
    public int taxPercent = 5;
    public float happiness = 75;
    public float population;
    public float popIncome = 20;
    public float soldierCount = 30;
    public bool paused;
    public float tradesLeft;
    private float originalTrades;

    public string currentlyBuyingFrom;

    public float happinessPopulationModifier, populationModifier;
    public int gracePeriodYear;

    public enum Kingdom { Rym, Jalonn, Cobeth, Galerd }
    public enum Type { Wood, Stone, Leather };

    public float secondsBetweenYears = 10;
    public float woodCostPerPercent, stoneCostPerPercent, leatherCostPerPercent;
    public float wood = 50;
    public float leather = 10;
    public float stone = 20;

    public int minWoodRandom, maxWoodRandom, minStoneRandom, maxStoneRandom, minLeatherRandom, maxLeatherRandom;

    public bool gracePeriod = true;

    public Dictionary<Kingdom, bool> warStatus = new Dictionary<Kingdom, bool>();
    public Dictionary<Kingdom, bool> conqueredStatus = new Dictionary<Kingdom, bool>();

    public float soldierPrice;
    public float soldierExpense;
    private float originalSeconds;
    public GameObject canvas;
    public GameObject yearlyReview;
    public int yearlyWoodProduce, yearlyStoneProduce, yearlyLeatherProduce;
    public bool taxChanged = false;
    public bool transition = true;
    public bool isAtWar = false;

    public int relationsWithRym, relationsWithJalonn, relationsWithGalerd, relationsWithCobeth;

    public int rymStone, rymWood, rymLeather, jalonnWood, jalonnStone, jalonnLeather, cobethStone, cobethWood, cobethLeather, galerdWood, galerdStone, galerdLeather;

    List<List<int>> allGoods = new List<List<int>>();
    public List<int> rymGoodPrices;
    public List<int> jalonnGoodPrices;
    public List<int> galerdGoodPrices;
    public List<int> cobethGoodPrices;

    private StatsUI statsUI;
    private ResourceUI resourceUI;
    private CameraMove cameraMove;

    public GameObject cobethTrade, rymTrade, jalonnTrade, galerdTrade;
    List<GameObject> tradeObjects = new List<GameObject>();

    public int tradeRym, tradeJalonn, tradeCobeth, tradeGalerd; // 0 = fine, 1 = tax, 2 = war;
    public int taxFromTrade;

    public int barracksCost, tavernCost, marketCost, watchtowerCost, garrisonCost, druidCost;
    public bool ownsBarracks = false, ownsTavern = false, ownsMarket = false, ownsWatchtower = false, ownsGarrison = false, ownsDruid = false;

    public bool drought = false, plague = false;

    public float soldierStrength;

    void Start() {
        allGoods.Add(rymGoodPrices);
        allGoods.Add(jalonnGoodPrices);
        allGoods.Add(galerdGoodPrices);
        allGoods.Add(cobethGoodPrices);

        tradeObjects.Add(cobethTrade);
        tradeObjects.Add(jalonnTrade);
        tradeObjects.Add(rymTrade);
        tradeObjects.Add(galerdTrade);

        warStatus.Add(Kingdom.Cobeth, false);
        warStatus.Add(Kingdom.Jalonn, false);
        warStatus.Add(Kingdom.Galerd, false);
        warStatus.Add(Kingdom.Rym, false);

        conqueredStatus.Add(Kingdom.Cobeth, false);
        conqueredStatus.Add(Kingdom.Jalonn, false);
        conqueredStatus.Add(Kingdom.Galerd, false);
        conqueredStatus.Add(Kingdom.Rym, false);

        originalTrades = tradesLeft;
        originalSeconds = secondsBetweenYears;
        statsUI = canvas.GetComponent<StatsUI>();
        resourceUI = canvas.GetComponent<ResourceUI>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>(); 

        if (loadedSavedGame()) {
            updateRelations();
            loadGoodsPrices();

        } else {
            generateGoodPrices();
            setSliderValues();
        }
    }

    public void setSliderValues() {
        PlayerPrefs.SetInt("WoodPercent", 15);
        PlayerPrefs.SetInt("StonePercent", 15);
        PlayerPrefs.SetInt("LeatherPercent", 15);
    }

    public bool hasBuilt(string s) {

        if (s == "barracks") {
            return ownsBarracks;
        }

        if (s == "bavern") {
            return ownsTavern;
        }

        if (s == "market") {
            return ownsMarket;
        }

        if (s == "watchtower") {
            return ownsWatchtower;
        }

        if (s == "garrison") {
            return ownsGarrison;
        }

        if (s == "druid") {
            return ownsDruid;
        }

        return false;
    }

    public void loadGoodsPrices() {
            //like from player prefs
    }

    public int getMaxPlayerGoods(Type t) {

        string type = t.ToString();

        if (type == "Wood") {
            return (int) wood;
        }

        if (type == "Stone") {
            return (int) stone;
        }

        if (type == "Leather") {
            return (int) leather;
        }

        return 0;
    }

    public int getCost(Type t, Kingdom kingdom) {

        string type = t.ToString();

        switch (kingdom) {
            case GameManager.Kingdom.Rym:
                if (type == "Wood") {
                    return rymGoodPrices[0];
                }

                if (type == "Stone") {
                    return rymGoodPrices[1];
                }

                if (type == "Leather") {
                    return rymGoodPrices[2];
                }
                break;

            case GameManager.Kingdom.Galerd:
                if (type == "Wood") {
                    return galerdGoodPrices[0];
                }

                if (type == "Stone") {
                    return galerdGoodPrices[1];
                }

                if (type == "Leather") {
                    return galerdGoodPrices[2];
                }
                break;

            case GameManager.Kingdom.Cobeth:
                if (type == "Wood") {
                    return cobethGoodPrices[0];
                }

                if (type == "Stone") {
                    return cobethGoodPrices[1];
                }

                if (type == "Leather") {
                    return cobethGoodPrices[2];
                }
                break;

            case GameManager.Kingdom.Jalonn:
                if (type == "Wood") {
                    return jalonnGoodPrices[0];
                }

                if (type == "Stone") {
                    return jalonnGoodPrices[1];
                }

                if (type == "Leather") {
                    return jalonnGoodPrices[2]; 
                }
                break;
        }

        return 0;
    }

    public int getMaxGoods(Type t, Kingdom kingdom) {

        string type = t.ToString();

        switch (kingdom) {
            case GameManager.Kingdom.Rym:
                if (type == "Wood") {
                    return rymWood;
                }

                if (type == "Stone") {
                    return rymStone;
                }

                if (type == "Leather") {
                    return rymLeather;
                }
                break;

            case GameManager.Kingdom.Galerd:
                if (type == "Wood") {
                    return galerdWood;
                }

                if (type == "Stone") {
                    return galerdStone;
                }

                if (type == "Leather") {
                    return galerdLeather;
                }
                break;

            case GameManager.Kingdom.Cobeth:
                if (type == "Wood") {
                    return cobethWood;
                }

                if (type == "Stone") {
                    return cobethStone;
                }

                if (type == "Leather") {
                    return cobethLeather;
                }
                break;

            case GameManager.Kingdom.Jalonn:
                if (type == "Wood") {
                    return jalonnWood;
                }

                if (type == "Stone") {
                    return jalonnStone;
                }

                if (type == "Leather") {
                    return jalonnLeather;
                }
                break;
        }

        return 0;
    }

    public string getTradeStatus(Kingdom kingdom) {
        updateTradeStatus(kingdom);

        switch (kingdom) {
            case Kingdom.Jalonn:
                if (tradeJalonn == 0) {
                    return "Accepting Trade";
                } else if (tradeJalonn == 1) {
                    return "Taxed Goods";
                } else {
                    return "At War";
                }
            case Kingdom.Rym:
                if (tradeRym == 0) {
                    return "Accepting Trade";
                }
                else if (tradeRym == 1) {
                    return "Taxed Goods";
                }
                else {
                    return "At War";
                }
            case Kingdom.Cobeth:
                if (tradeCobeth == 0) {
                    return "Accepting Trade";
                }
                else if (tradeCobeth == 1) {
                    return "Taxed Goods";
                }
                else {
                    return "At War";
                }
            case Kingdom.Galerd:
                if (tradeGalerd == 0) {
                    return "Accepting Trade";
                }
                else if (tradeGalerd == 1) {
                    return "Taxed Goods";
                }
                else {
                    return "At War";
                }
        }

        return "";
    }

    public bool loadedSavedGame() {
        return false;
    }

    public void generateGoodPrices() {
        foreach (List<int> list in allGoods) {
            int wood = Random.Range(minWoodRandom, maxWoodRandom);
            int stone = Random.Range(minStoneRandom, maxStoneRandom);
            int leather = Random.Range(minLeatherRandom, maxLeatherRandom);

            list.Clear();
            list.Add(wood);
            list.Add(stone);
            list.Add(leather);
        }
    }

    public void updateRelations() {
        relationsWithRym = PlayerPrefs.GetInt("RymRelation");
        relationsWithJalonn = PlayerPrefs.GetInt("JalonnRelation");
        relationsWithCobeth = PlayerPrefs.GetInt("CobethRelation");
        relationsWithGalerd = PlayerPrefs.GetInt("GalerdRelation");
    }

    public void increaseRelations(Kingdom kingdom, int amount) {
        switch (kingdom) {
            case Kingdom.Jalonn:
                relationsWithJalonn += amount;
                if (relationsWithJalonn > 100)  relationsWithJalonn = 100;
                break;
            case Kingdom.Rym:
                relationsWithRym += amount;
                if (relationsWithRym > 100) relationsWithRym = 100;
                break;
            case Kingdom.Cobeth:
                relationsWithCobeth += amount;
                if (relationsWithCobeth > 100) relationsWithCobeth = 100;
                break;
            case Kingdom.Galerd:
                relationsWithGalerd += amount;
                if (relationsWithGalerd > 100) relationsWithGalerd = 100;
                break;
        }

        updateTradeStatus(kingdom);
    }

    public void updateTradeStatus(Kingdom kingdom) {
        int relations = getRelations(kingdom);

        switch (kingdom) {
            case Kingdom.Jalonn:
                if (atWar(kingdom)) {
                    tradeJalonn = 2;
                    return;
                }

                if (relations <= 20) {
                    tradeJalonn = 1;
                    return;
                }

                tradeJalonn = 0;

                break;
            case Kingdom.Rym:
                if (atWar(kingdom)) {
                    tradeRym = 2;
                    return;
                }

                if (relations <= 20) {
                    tradeRym = 1;
                    return;
                }

                tradeRym = 0;
                break;
            case Kingdom.Cobeth:
                if (atWar(kingdom)) {
                    tradeCobeth = 2;
                    return;
                }

                if (relations <= 20) {
                    tradeCobeth = 1;
                    return;
                }

                tradeCobeth = 0;
                break;
            case Kingdom.Galerd:
                if (atWar(kingdom)) {
                    tradeGalerd = 2;
                    return;
                }

                if (relations <= 20) {
                    tradeGalerd = 1;
                    return;
                }

                tradeGalerd = 0;
                break;
        }
    }

    public bool atWar(Kingdom kingdom) {
        return false;
    }

    public void decreaseRelations(Kingdom kingdom, int amount) {
        switch (kingdom) {
            case Kingdom.Jalonn:
                relationsWithJalonn -= amount;
                if (relationsWithJalonn < 0) relationsWithJalonn = 0;
                break;
            case Kingdom.Rym:
                relationsWithRym -= amount;
                if (relationsWithRym < 0) relationsWithRym = 0;
                break;
            case Kingdom.Cobeth:
                relationsWithCobeth -= amount;
                if (relationsWithCobeth < 0) relationsWithCobeth = 0;
                break;
            case Kingdom.Galerd:
                relationsWithGalerd -= amount;
                if (relationsWithGalerd < 0) relationsWithGalerd = 0;
                break;
        }

        updateTradeStatus(kingdom);
    }

    public void saveRelations() {
        PlayerPrefs.SetInt("RymRelation", relationsWithRym);
        PlayerPrefs.SetInt("JalonnRelation", relationsWithJalonn);
        PlayerPrefs.SetInt("CobethRelation", relationsWithCobeth);
        PlayerPrefs.SetInt("GalerdRelation", relationsWithGalerd);
    }

    public float calculateTaxReturns() {
        float tax = ((population * popIncome) * taxPercent) / 100;

        if (ownsMarket) {
            tax += ((tax / 100) * 25);
        }

        return tax;
    }

    public bool isGamePaused() {
        return paused;
    }

    public int getRelations(Kingdom kingdom) {
        if (kingdom == Kingdom.Jalonn) {
            return relationsWithJalonn;
        }

        if (kingdom == Kingdom.Galerd) {
            return relationsWithGalerd;
        }

        if (kingdom == Kingdom.Cobeth) {
            return relationsWithCobeth;
        }

        if (kingdom == Kingdom.Rym) {
            return relationsWithRym;
        }

        return 0;
    }

    void Update() {

        if (paused) return;

        if (secondsBetweenYears > 0) {
            secondsBetweenYears -= Time.deltaTime;
        } else {
            goToNextYear();
        }
    }

    public void goToNextYear() {
        paused = true;
        taxChanged = false;
        cameraMove.moveDown();
        statsUI.setActive(false);
        resourceUI.setActive(false);
        tradesLeft = originalTrades;

        canvas.GetComponent<ConfirmSaleMenu>().warningText.SetActive(false);
        canvas.GetComponent<ConfirmSaleMenu>().noGoldText.SetActive(false);
        canvas.GetComponent<ConfirmSaleMenu>().resetTradeSliders();
        // just incase. ^

        canvas.GetComponent<Transition>().fadeOut();


        foreach (GameObject obj in tradeObjects) {
            if (obj.GetComponent<TradeListener>().started) {
                obj.GetComponent<TradingTextUpdater>().updateText(obj.GetComponent<TradeListener>().kingdom);
            }
        }

        float oldGold = gold;
        float soldierCost = (soldierCount * soldierExpense);

        if (ownsBarracks) soldierCost = ((soldierCost / 100) * 90);

        yearlyExpenses += soldierCost;

        generateGoodPrices();
        resourceUI.updateResources();
        resourceUI.updateResourceText();
        yearlyExpenses += resourceUI.loadGoodsExpenses();

        gold -= yearlyExpenses;
        gold += calculateTaxReturns();
        updateHappiness();

        if (plague) {
            soldierStrength += ((soldierStrength / 100) * 10);
            plague = false;
        }

        if (drought) {
            soldierStrength += ((soldierStrength / 100) * 50);
            drought = false;
        }

        if (gold < 0) {
            Debug.Log("DEBT");
            happiness -= 15;

            if (happiness < 0) {
                happiness = 0;
            }
        }

        population += (int) (happiness / happinessPopulationModifier * populationModifier);

        GetComponent<Raiding>().setButtonInteractable();

        canvas.GetComponent<YearlyUI>().updateUI(oldGold);
        year++;

        if (year >= gracePeriodYear) gracePeriod = true;

        StartCoroutine(showUI());

        saveRelations();
        secondsBetweenYears = originalSeconds;
    }

    public void updateHappiness() {
        int happinessChange = 1;

        switch (taxPercent) { // 0 , 5, 10, 20 , 35
            case 0:
                happinessChange = Random.Range(15, 20);
                break;
            case 5:
                happinessChange = Random.Range(10, 15);
                break;
            case 10:
                happinessChange = Random.Range(1, 5);
                break;
            case 20:
                happinessChange = Random.Range(-10, -5);
                break;
            case 35:
                happinessChange = Random.Range(-20, -10);
                break;
        }

        happiness += happinessChange;

        if (ownsTavern) {

            if (happinessChange >= 0) { // positive
                happinessChange += ((happinessChange / 100) * 50);
            } else { // negative
                happinessChange -= ((happinessChange / 100) * 50);
            }
        }
       

        if (happiness > 100) happiness = 100;
        if (happiness < 0) happiness = 0;
    }


    public void resetYearlyProduce() {
        yearlyWoodProduce = 0;
        yearlyStoneProduce = 0;
        yearlyLeatherProduce = 0;
    }

    public IEnumerator showUI() {
        yield return new WaitForSeconds(1f);
        yearlyReview.SetActive(true);
    }

    public string get(string s) {
        switch (s) {
            case "year":
                return year.ToString();
            case "gold":
                return ((int) gold).ToString();
            case "tax":
                return ((int) calculateTaxReturns()).ToString();
            case "happiness":
                return ((int) happiness).ToString() + "%";
            case "population":
                return ((int) population).ToString();
            case "expense":
                return ((int) yearlyExpenses).ToString();
            case "soldier":
                return ((int) soldierCount).ToString();
            case "wood":
                return ((int) wood).ToString();
            case "stone":
                return ((int) stone).ToString();
            case "leather":
                return ((int) leather).ToString();
        }

        return "0";
    }
}