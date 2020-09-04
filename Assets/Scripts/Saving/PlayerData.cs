using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData  {

    public float year;
    public float gold;
    public float yearlyExpenses;
    public int taxPercent;
    public float happiness;
    public float population;
    public float popIncome;
    public float soldierCount;
    public float wood;
    public float leather;
    public float stone;
    public bool ownsBarracks, ownsTavern, ownsMarket, ownsWatchtower, ownsGarrison, ownsDruid;
    public int tradeRym, tradeJalonn, tradeCobeth, tradeGalerd; // 0 = fine, 1 = tax, 2 = war;
    public float soldierStrength;
    public bool isAtWar;
    public Dictionary<GameManager.Kingdom, bool> warStatus = new Dictionary<GameManager.Kingdom, bool>();
    public Dictionary<GameManager.Kingdom, bool> conqueredStatus = new Dictionary<GameManager.Kingdom, bool>();
    public int puppetStates;
    public int woodValue, stoneValue, leatherValue;

    public int relationsWithRym, relationsWithJalonn, relationsWithGalerd, relationsWithCobeth;

    public PlayerData(GameManager gameManager) {
        year = gameManager.year;
        gold = gameManager.gold;
        yearlyExpenses = gameManager.yearlyExpenses;
        taxPercent = gameManager.taxPercent;
        happiness = gameManager.happiness;
        population = gameManager.population;
        popIncome = gameManager.popIncome;
        soldierCount = gameManager.soldierCount;
        wood = gameManager.wood;
        stone = gameManager.stone;
        leather = gameManager.leather;
        ownsBarracks = gameManager.ownsBarracks;
        ownsTavern = gameManager.ownsTavern;
        ownsMarket = gameManager.ownsMarket;
        ownsWatchtower = gameManager.ownsWatchtower;
        ownsGarrison = gameManager.ownsGarrison;
        ownsDruid = gameManager.ownsDruid;
        tradeRym = gameManager.tradeRym;
        tradeJalonn = gameManager.tradeJalonn;
        tradeCobeth = gameManager.tradeCobeth;
        tradeGalerd = gameManager.tradeGalerd;
        soldierStrength = gameManager.soldierStrength;
        isAtWar = gameManager.isAtWar;
        warStatus = gameManager.warStatus;
        conqueredStatus = gameManager.conqueredStatus;
        puppetStates = gameManager.puppetStates;
        relationsWithRym = gameManager.relationsWithRym;
        relationsWithCobeth = gameManager.relationsWithCobeth;
        relationsWithGalerd = gameManager.relationsWithGalerd;
        relationsWithJalonn = gameManager.relationsWithJalonn;
        woodValue = gameManager.woodValue;
        stoneValue = gameManager.stoneValue;
        leatherValue = gameManager.leatherValue;
    }
}
