using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Invading : MonoBehaviour {

    public TMP_Text rym, galerd, jalonn, cobeth, invadeF, invadeS;
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
