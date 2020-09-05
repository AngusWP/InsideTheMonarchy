using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;

public class YearlyEvents : MonoBehaviour {

    //here we manage random events every year (raid, plague, drought etc)
    //need to balance 

    private GameManager gameManager;
    public GameObject eventMenu;
    private ActionMenu actionMenu;
    public TMP_Text title, info;
    public GameObject canvas;

    public GameObject invasionSuccess;
    public TMP_Text invasionSuccessInfo;

    public GameObject invasionFail;
    public TMP_Text invasionFailInfo;

    public int eventChance;
    public int raidPopPercentMin, raidPopPercentMax, raidGoodsPercentMin, raidGoodsPercentMax, plaguePopPercentMin, plaguePopPercentMax;

    public enum Event { Raid, Plague, Drought };

    void Start() {
        gameManager = GetComponent<GameManager>();
        actionMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionMenu>();
    }

    public void handleEvents() {
        int chance = UnityEngine.Random.Range(1, 100);

        if (!(eventChance >= chance)) return;

        int typeChance = UnityEngine.Random.Range(1, 100);
        Event e = Event.Raid;

        if (typeChance >= 50) {
            e = Event.Raid;
        } else if (typeChance <= 49 && typeChance >= 25) {
            e = Event.Plague;
        } else if (typeChance <= 24 && typeChance >= 0) {
            e = Event.Drought;
        }

        if (gameManager.isAtWar) {
            bool conflict = false;
            
            foreach (GameManager.Kingdom k in gameManager.warStatus.Keys) {

                if (conflict) {
                    return;
                }

                if (gameManager.warStatus[k] == true) {

                    int invChance = 20;
                    int random = UnityEngine.Random.Range(1, 100);
                    

                    if (invChance >= random) {

                        if (!conflict) conflict = true;

                        int c = 70;
                        if (gameManager.ownsGarrison) c -= 45;
                        if (gameManager.ownsWatchtower) c -= 15;

                        if (c >= UnityEngine.Random.Range(1, 100)) {
                            invasionSuccess.SetActive(true);
                            invasionSuccessInfo.text = "Unfortunately your Kingdom has been overrun and taken by " + k.ToString() + ". Spikes with your family's heads litter the walls of your keep. Your reign is over.";

                            actionMenu.openObjects.Add(invasionSuccess);
                            actionMenu.setTask(true);
                        } else {
                            invasionFail.SetActive(true);
                            invasionFailInfo.text = k.ToString() + " have tried to conquer your Kingdom! Luckily, your defenses pulled through. Although you have lost alot of resources, your Kingdom has managed to pull through.";

                            gameManager.soldierCount -= ((gameManager.soldierCount / 100) * UnityEngine.Random.Range(20, 40));
                            gameManager.wood -= ((gameManager.wood / 100) * UnityEngine.Random.Range(10, 20));
                            gameManager.stone -= ((gameManager.stone / 100) * UnityEngine.Random.Range(10, 20));
                            gameManager.leather -= ((gameManager.leather / 100) * UnityEngine.Random.Range(10, 20));
                            gameManager.gold -= ((gameManager.gold / 100) * UnityEngine.Random.Range(10, 20));

                            actionMenu.openObjects.Add(invasionFail);
                            actionMenu.setTask(true);
                        }
                    }

                }
            }
        }

        if (e == Event.Raid) {
            if (gameManager.ownsWatchtower) {
                int escapeChance = UnityEngine.Random.Range(1, 100);
        
                if (escapeChance <= 30) {
                    UnityEngine.Debug.Log("Saved by Watchtower.");
                    return;
                }
            }
        }

        if (e == Event.Plague) {
            if (gameManager.ownsDruid) {
                int escapeChance = UnityEngine.Random.Range(1, 100);

                if (escapeChance <= 20) {
                    UnityEngine.Debug.Log("Saved by Druid.");
                    return;
                }
            }
        }

        actionMenu.openObjects.Add(eventMenu);
        actionMenu.setTask(true);

        runEvent(e);
        title.text = getCardInfo(e)[0];
        info.text = getCardInfo(e)[1] + "\n" + getCardInfo(e)[2];

        StartCoroutine(showUI());
    }

    public void checkCivilStatus() {
        if (gameManager.happiness < 15) {
            UnityEngine.Debug.Log("CIVIL WAR CHANCE");
        }
    }

    public void closeInvasionFail() {
        actionMenu.openObjects.Remove(invasionFail);
        actionMenu.setTask(false);
    }

    public void runEvent(Event e) {
        switch (e) {
            case Event.Raid:
                float minus = (gameManager.population / 100);
                int modifier = UnityEngine.Random.Range(raidPopPercentMin, raidPopPercentMax);

                gameManager.population -= (minus * modifier);
                int modifier1 = UnityEngine.Random.Range(raidGoodsPercentMin, raidGoodsPercentMax) / 100;

                gameManager.wood -= (gameManager.wood * (1 - modifier1));
                gameManager.stone -= (gameManager.stone * (1 - modifier1));
                gameManager.leather -= (gameManager.leather * (1 - modifier1));

                canvas.GetComponent<ResourceUI>().updateResourceText(); // think this is done?
                break;
            case Event.Plague:
                gameManager.plague = true;
                float m = (gameManager.population / 100);
                int mod = UnityEngine.Random.Range(plaguePopPercentMin, plaguePopPercentMax);
                float soldier = (gameManager.soldierCount / 100);


                gameManager.population -= (m * mod);
                gameManager.soldierCount -= (soldier * mod);
                gameManager.soldierStrength = gameManager.soldierStrength / 2;
                gameManager.happiness -= 10;

                break;
            case Event.Drought:
                //make buildings 20% more expensive
                gameManager.drought = true;
                break;
        }

        if (gameManager.population < 0) {
            gameManager.population = 0;
        }

        if (gameManager.soldierCount < 0) {
            gameManager.soldierCount = 0;
        }

        if (gameManager.happiness < 0) {
            gameManager.happiness = 0;
        }
    }

    public List<string> getCardInfo(Event e) {
        List<string> list = new List<string>();
        
        switch (e) {
            case Event.Raid:
                list.Add("You have been raided!");
                list.Add("Your villages have raided by bandits! Some of your peasants have lost their life, and the bandits have stolen any goods they could find!");
                list.Add("You have lost some resources.");
                break;
            case Event.Plague:
                list.Add("There has been a devastating plague!");
                list.Add("A plague, rumoured to have come from a bat, has wiped out lots of villages around your Kingdom! Many lives have been lost.");
                list.Add("Your kingdom's happiness has fallen by 10%.");
                break;
            case Event.Drought:
                list.Add("A drought strikes your Kingdom!");
                list.Add("There hasn't been any water for months. Any building upgrades will now cost 20% more, as your workers are exhausted and thirsty.");
                list.Add("Your army's strength has fallen by 50% for this year.");
                break;
        }

        return list;
    }

    IEnumerator showUI() {
        yield return new WaitForSeconds(1f);
        eventMenu.SetActive(true);   
    }
}
