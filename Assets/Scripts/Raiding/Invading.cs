using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Invading : MonoBehaviour {

    public TMP_Text rym, galerd, jalonn, cobeth, invadeF, invadeS, textS, textF;
    private GameManager gameManager;

    public int invadeModifier;

    public GameObject invade, win, invadeSuccess, invadeFail;
    private ActionMenu actionMenu;

    void Start() {
        gameManager = GetComponent<GameManager>();
        actionMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionMenu>();
    }

    void Update() {
        
    }

    public void updateRelationsOnInvadeUI() {
        rym.text = gameManager.relationsWithRym + "%";
        cobeth.text = gameManager.relationsWithCobeth + "%";
        galerd.text = gameManager.relationsWithGalerd + "%";
        jalonn.text = gameManager.relationsWithJalonn + "%";
    }

    public void onClick(string k) {
        GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), k);
        gameManager.setAtWar(kingdom);

        int chance = -10;
        gameManager.invadePause = true;

        float modifier = gameManager.soldierCount;
        modifier = (gameManager.soldierCount * gameManager.soldierStrength);

        chance += (int) ((modifier / 100) * invadeModifier);
        int roll = UnityEngine.Random.Range(1, 100);

        Debug.Log(chance);
        Debug.Log(roll);

        invade.SetActive(false);

        if (chance >= roll) {
            invadeSuccess.SetActive(true);
            gameManager.conqueredStatus[kingdom] = true;
            gameManager.warStatus[kingdom] = false;

            textS.text = "Your forces have stormed the keep of " + kingdom.ToString() +  ", and have taken the land. Your subjects congratulate you on your victory. " +
                "There is a cost in victory though, as you have lost good men to claim these lands.";


            bool war = false;

            foreach (GameManager.Kingdom kin in gameManager.warStatus.Keys) {
                    if (gameManager.warStatus[kin]) {
                    war = true;
                }
            }

            if (!war) {
                gameManager.isAtWar = false;
            }

            gameManager.puppetStates++;

            float loss = ((gameManager.soldierCount / 100) * UnityEngine.Random.Range(30, 55));

            Debug.Log(loss);

            if (gameManager.ownsGarrison || gameManager.ownsBarracks) {
                loss -= (loss * ((gameManager.soldierStrength - 1)));
                Debug.Log("Post Calculation: " + loss);
            }

            gameManager.soldierCount -= loss;

        } else {
            invadeFail.SetActive(true);
            textF.text = "Your forces were unable to overcome " + kingdom.ToString() + " 's defenses. You have lost a large majority of soldiers, and your people's happiness has dropped.";
            gameManager.soldierCount = ((gameManager.soldierCount / 100) * UnityEngine.Random.Range(15, 25)); // keep 15 to 25% 
            gameManager.decreaseRelations(kingdom, 100);

        }

        gameManager.updateTradeStatus(kingdom);
    }

    public void closeInvasionSuccess() {
        invadeSuccess.SetActive(false);
        actionMenu.openObjects.Remove(invadeSuccess);
        actionMenu.setTask(false);
        gameManager.invadePause = false;
    }

    public void closeInvasionFail() {
        invadeFail.SetActive(false);
        actionMenu.openObjects.Remove(invadeFail);
        actionMenu.setTask(false);
        gameManager.invadePause = false;
    }

    public void continueGame() {
        win.SetActive(false);
        actionMenu.setTask(false);
        actionMenu.openObjects.Remove(win);
        gameManager.won = false;
        gameManager.invadePause = false;
    }
}
