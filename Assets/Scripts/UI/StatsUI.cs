using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour {

    public TMP_Text gold, year, soldiers, happiness, population;
    public GameObject stats;
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(showUI());
    }

    public IEnumerator showUI() {
        yield return new WaitForSeconds(1);
        stats.SetActive(true);
    }

    void Update() {
        year.text = gameManager.get("year");
        gold.text = gameManager.get("gold");
        happiness.text = gameManager.get("happiness");
        population.text = gameManager.get("population");
        soldiers.text = gameManager.get("soldier");
    }

    public void setActive(bool b) {
        stats.SetActive(b);
    }
}
