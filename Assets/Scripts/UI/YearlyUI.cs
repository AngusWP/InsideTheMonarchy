using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YearlyUI : MonoBehaviour {

    public TMP_Text year, gold, tax, expense, happiness, population, newGold, soldierCount, wood, stone, leather;
    private GameManager gameManager;
    public GameObject yearlyReview;
    private StatsUI statsUI;
    private YearlyEvents yearlyEvents;
    private CameraMove cameraMove;
    private ResourceUI resourceUI;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        yearlyEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<YearlyEvents>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        statsUI = GetComponent<StatsUI>();
        resourceUI = GetComponent<ResourceUI>();
    }

    public void updateUI(float oldGold) {
        year.text = gameManager.get("year");
        gold.text = oldGold.ToString();
        tax.text = gameManager.get("tax");
        expense.text = gameManager.get("expense");
        happiness.text = gameManager.get("happiness");
        population.text = gameManager.get("population");
        newGold.text = gameManager.get("gold");
        soldierCount.text = gameManager.get("soldier");
        wood.text = gameManager.get("wood") + " (" + (int) gameManager.yearlyWoodProduce + ")";
        stone.text = gameManager.get("stone") + " (" + (int) gameManager.yearlyStoneProduce + ")";
        leather.text = gameManager.get("leather") + " (" + (int) gameManager.yearlyLeatherProduce + ")";
    }

    public void onClick() {
        yearlyReview.SetActive(false);
        yearlyEvents.handleEvents();
        yearlyEvents.checkCivilStatus();
        GetComponent<Transition>().fadeIn();
        cameraMove.moveUp();
        gameManager.paused = false;
        gameManager.resetYearlyProduce();
        StartCoroutine(statsUI.showUI());
        StartCoroutine(resourceUI.showUI());
    }
}
