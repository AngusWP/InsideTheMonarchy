using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

public class Raiding : MonoBehaviour {

    public TMP_Text title, coinText, woodText, leatherText, stoneText, relationText, rym, galerd, jalonn, cobeth, warText; // the kingdoms are the relation text objects in the raid menu.
    public GameObject raidObject, spoils, woodIcon, leatherIcon, stoneIcon, coinIcon, relationObject, failedText, warTextObj;
    public Button rymB, cobethB, jalonnB, galerdB;
    public bool failed = false;
    private ActionMenu actionMenu;
    private GameManager gameManager;
    private Canvas canvas;

    void Start() {
        gameManager = GetComponent<GameManager>();
        actionMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionMenu>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    void Update() {
        
    }

    public void updateRelationsOnRaidUI() {
        rym.text = gameManager.relationsWithRym + "%";
        cobeth.text = gameManager.relationsWithCobeth + "%";
        galerd.text = gameManager.relationsWithGalerd + "%";
        jalonn.text = gameManager.relationsWithJalonn + "%";
    }

    public void onLoad() {

        if (gameManager == null) return;

        if (gameManager.conqueredStatus[GameManager.Kingdom.Cobeth]) {
            cobethB.interactable = false;
        }

        if (gameManager.conqueredStatus[GameManager.Kingdom.Rym]) {
            rymB.interactable = false;
        }

        if (gameManager.conqueredStatus[GameManager.Kingdom.Galerd]) {
            galerdB.interactable = false;
        }

        if (gameManager.conqueredStatus[GameManager.Kingdom.Jalonn]) {
            jalonnB.interactable = false;
        }
    }

    public void onClick(string k) {

        GameManager.Kingdom kingdom = (GameManager.Kingdom)Enum.Parse(typeof(GameManager.Kingdom), k);

        spoils.SetActive(true);
        raidObject.SetActive(false);
        actionMenu.setTask(true);
        actionMenu.openObjects.Remove(raidObject);
        actionMenu.openObjects.Add(spoils);

        int wood = UnityEngine.Random.Range(25, 40);
        int stone = UnityEngine.Random.Range(20, 35);
        int leather = UnityEngine.Random.Range(7, 15);
        int gold = UnityEngine.Random.Range(70, 120);
        int relations = UnityEngine.Random.Range(30, 40);

        // make each random number relate to each kingdoms specific resources

        int failChance = 65 + ((int) (gameManager.soldierStrength - 1) * 100) + (int) ((gameManager.soldierCount / 100) * 2);
        // so soldier strength helps raid chances

        int roll = UnityEngine.Random.Range(1, 100);

        gameManager.decreaseRelations(kingdom, relations);
        int modifier = UnityEngine.Random.Range(3, 7);

        gameManager.soldierCount -= ((gameManager.soldierCount / 100) * modifier);

        if (roll >= failChance) {
            setIconStatus(false);
            title.text = "Defeat!";
            failed = true;

            return;
        }

        gameManager.gold += gold;
        gameManager.wood += wood;
        gameManager.stone += stone;
        gameManager.leather += leather;

        canvas.GetComponent<ResourceUI>().updateResourceText();

        woodText.text = wood.ToString();
        stoneText.text = stone.ToString();
        leatherText.text = leather.ToString();
        coinText.text = gold.ToString();
        relationText.text = "(-" + relations + " relations with " + kingdom.ToString() + ")";
    
        if (gameManager.getRelations(kingdom) <= 10) {
            gameManager.setAtWar(kingdom);
            gameManager.isAtWar = true;

            warTextObj.SetActive(true);
            warText.text = "You are now at war with " + kingdom + ".";
        }
    }

    public void setIconStatus(bool b) {
        coinIcon.SetActive(b);
        stoneIcon.SetActive(b);
        woodIcon.SetActive(b);
        relationObject.SetActive(b);
        leatherIcon.SetActive(b);

        failedText.SetActive(!b);
    }

    public void setButtonInteractable() {
        rymB.interactable = true;
        cobethB.interactable = true;
        jalonnB.interactable = true;
        galerdB.interactable = true;
    }
}
