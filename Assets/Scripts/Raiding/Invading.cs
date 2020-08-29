using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Invading : MonoBehaviour {

    public TMP_Text rym, galerd, jalonn, cobeth;
    private GameManager gameManager;

    public GameObject win;
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
        //
    }

    public void continueGame() {
        win.SetActive(false);
        actionMenu.setTask(false);
        actionMenu.openObjects.Remove(win);
        gameManager.won = false;
    }
}
