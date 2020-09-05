using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ActionMenu : MonoBehaviour {

    private GameManager gameManager;
    public GameObject actionMenu;
    public bool active = false;

    public GameObject dropdown, canvas, inputField, inputObject, resourceMenu, confirmSale, tradeObject, buildObject, raidObject, spoilsObject, invadeObject;
    public TMP_Text infoTextConfirm, relationNo;

    public Button rym, galerd, jalonn, cobeth;

    private Raiding raiding;
    private Invading invading;
    private BuildingUI buildingUI;
    private TradeListener tradeListener;

    public List<GameObject> openObjects;
    public bool taskOpen = false;

    void Start() {
        openObjects = new List<GameObject>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        raiding = gameManager.GetComponent<Raiding>();
        invading = gameManager.GetComponent<Invading>();
        tradeListener = tradeObject.GetComponentInChildren<TradeListener>();
    }

    public void goBackToMenu() {
        SceneManager.LoadScene(0);
    }

    void Update() {

        if (gameManager.isGamePaused()) {

            if (active) {
                foreach (GameObject obj in openObjects) {
                    obj.SetActive(false);
                }

                setTask(false);
            }

            gameManager.currentlyBuyingFrom = "";
            return;
        }

        if (taskOpen) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            active = !active;
            actionMenu.SetActive(active);

            if (active) {
                openObjects.Add(actionMenu);
            } else {
                openObjects.Remove(actionMenu);
            }
        }
    }

    public void changeTaxes() {

        setupDropdown("Manage Taxes", new List<string> { "0%", "5%", "10%", "20%", "35%" }, "WARNING: The higher your taxes, the lower your people's happiness will be.", confirmTaxes);
        TMP_Dropdown drop = canvas.transform.Find("Taxes").transform.Find("Drop").GetComponent<TMP_Dropdown>();

        switch (gameManager.taxPercent) {
            case 0:
                drop.value = 0;
                break;
            case 5:
                drop.value = 1;
                break;
            case 10:
                drop.value = 2;
                break;
            case 20:
                drop.value = 3;
                break;
            default:
                drop.value = 4;
                break;
        }

        dropdown.SetActive(true);
        setTask(true);
        removeActionMenu();
        openObjects.Add(dropdown);
    }

    public void removeActionMenu() {
        actionMenu.SetActive(false);
        openObjects.Remove(actionMenu);
    }

    public void recruitSoliders() {

        setupInputField("Recruit Soldiers", "Info: Having a higher amount of soliders increases yearly expenses.", confirmRecruitment, true, changeValue);

        inputField.SetActive(true);
        removeActionMenu();
        openObjects.Add(inputField);
        setTask(true);
    }

    public void manageResources() {
        foreach (SliderTextUpdate text in resourceMenu.GetComponentsInChildren<SliderTextUpdate>()) {
            text.onLoad();
        }
        resourceMenu.SetActive(true);
        removeActionMenu();
        openObjects.Add(resourceMenu);
        setTask(true);
     }

     public void setTask(bool b) {
        active = b;
        taskOpen = b;
    }

    public void openTradeMenu() {
        tradeListener.onLoad();
        tradeObject.SetActive(true);
        openObjects.Add(tradeObject);
        setTask(true);
        removeActionMenu();
    }

    public void openBuildMenu() {
        buildingUI = canvas.GetComponent<BuildingUI>();
        buildingUI.checkBought();
        buildObject.SetActive(true);
        openObjects.Add(buildObject);
        setTask(true);
        removeActionMenu();
    }

    public void openRaidMenu() {
        raiding.onLoad();
        raiding.updateRelationsOnRaidUI();
        raidObject.SetActive(true);
        openObjects.Add(raidObject);
        setTask(true);
        removeActionMenu();
    }

    public void openInvadeMenu() {
        invading.updateRelationsOnInvadeUI();
        invading.updateButtons(rym, galerd, jalonn, cobeth);
        invadeObject.SetActive(true);
        openObjects.Add(invadeObject);
        setTask(true);
        removeActionMenu();
    }

    public void tradeExitClick() {
        setTask(false);
        openObjects.Remove(tradeObject);
        tradeObject.SetActive(false);
    }

    public void raidExitClick() {
        setTask(false);
        openObjects.Remove(raidObject);
        raidObject.SetActive(false);
    }

    public void invadeExitClick() {
        setTask(false);
        openObjects.Remove(invadeObject);
        invadeObject.SetActive(false);
    }

    public void spoilsExitClick() {
        setTask(false);
        openObjects.Remove(spoilsObject);

        if (raiding.failed) {
            raiding.setIconStatus(true);
            raiding.failed = false;
            raiding.title.text = "Spoils";
        }

        spoilsObject.SetActive(false);
    }

    public void buildingExitClick() {
        setTask(false);
        openObjects.Remove(buildObject);
        buildObject.SetActive(false);
    }

    public void confirmResources() {
        resourceMenu.SetActive(false);
        openObjects.Remove(resourceMenu);
        setTask(false);
    }

    public void confirmRecruitment() {
        setTask(false);
        inputField.SetActive(false);
        openObjects.Remove(inputField);
        string input = inputObject.GetComponent<TMP_InputField>().text;
        TMP_Text warning = canvas.transform.Find("Soldiers").transform.Find("InfoText").GetComponent<TMP_Text>();

        if (input == "") {
            openObjects.Remove(inputField);
            setTask(false);
            inputField.SetActive(false);
            return;
        }

        if (input.Contains("-")) {
            warning.text = "You must enter a positive number.";
            return;
        }

        int amountOfSoldiers = int.Parse(input);
        inputObject.GetComponent<TMP_InputField>().text = "0";
        int price = amountOfSoldiers * (int) gameManager.soldierPrice;

        if (price > gameManager.gold) {
            warning.text = "You cannot afford this.";
            return;
        }

        gameManager.gold -= price;
        gameManager.soldierCount += amountOfSoldiers;
    }

    public void onKingdomClick(string s) {
        tradeObject.SetActive(false);

        openObjects.Remove(tradeObject);
        openObjects.Add(confirmSale);

        openSaleMenu((GameManager.Kingdom) Enum.Parse(typeof(GameManager.Kingdom), s));
    }

    public void openSaleMenu(GameManager.Kingdom kingdom) {
        gameManager.currentlyBuyingFrom = kingdom.ToString();
        canvas.GetComponent<ConfirmSaleMenu>().setMaxValues(kingdom);
        confirmSale.SetActive(true);
        infoTextConfirm.text = "You are currently trading with " + kingdom.ToString() + ".";
        int woodCost = gameManager.getCost(GameManager.Type.Wood, kingdom);
        int stoneCost = gameManager.getCost(GameManager.Type.Stone, kingdom);
        int leatherCost = gameManager.getCost(GameManager.Type.Leather, kingdom);

        relationNo.text = gameManager.getRelations(kingdom) + "%";
    }

    public void onBackClickTrade() {
        confirmSale.SetActive(false);
        tradeObject.SetActive(true);
        canvas.GetComponent<ConfirmSaleMenu>().noGoldText.SetActive(false); // just incase.
        canvas.GetComponent<ConfirmSaleMenu>().warningText.SetActive(false);
        canvas.GetComponent<ConfirmSaleMenu>().resetTradeSliders();
        gameManager.currentlyBuyingFrom = "";
    }

    public void changeValue() {
        TMP_Text cost = canvas.transform.Find("Soldiers").transform.Find("CostInfo").GetComponent<TMP_Text>();
        string input = inputObject.GetComponent<TMP_InputField>().text;

        if (input == "") {
            cost.text = "";
            return;
        }

        int amountOfSoldiers = int.Parse(input);
        cost.text = (amountOfSoldiers * (int)gameManager.soldierPrice).ToString();
    }

    public void setupInputField(string t, string warn, UnityEngine.Events.UnityAction onClick, bool cost, UnityEngine.Events.UnityAction onValueChange) {
        canvas.transform.Find("Soldiers").transform.Find("Title").GetComponent<TMP_Text>().text = t;
        TMP_InputField inputF = inputObject.GetComponent<TMP_InputField>();
        canvas.transform.Find("Soldiers").transform.Find("InfoText").GetComponent<TMP_Text>().text = warn;
        Button confirm = canvas.transform.Find("Soldiers").transform.Find("Confirm").GetComponent<Button>();

        if (cost) {
            inputF.onValueChanged.AddListener(delegate { onValueChange(); });
        }
    }

    public void setupDropdown(string t, List<string> options, string warn, UnityEngine.Events.UnityAction onClick) {
        TMP_Text title = canvas.transform.Find("Taxes").transform.Find("Title").GetComponent<TMP_Text>();
        TMP_Dropdown drop = canvas.transform.Find("Taxes").transform.Find("Drop").GetComponent<TMP_Dropdown>();
        TMP_Text warning = canvas.transform.Find("Taxes").transform.Find("InfoText").GetComponent<TMP_Text>();
        Button confirm = canvas.transform.Find("Taxes").transform.Find("Confirm").GetComponent<Button>();

        title.text = t;
        drop.ClearOptions();
        drop.AddOptions(options);
        confirm.onClick.AddListener(onClick);

        warning.text = warn;
    }


    public void confirmTaxes() {
        TMP_Dropdown drop = canvas.transform.Find("Taxes").transform.Find("Drop").GetComponent<TMP_Dropdown>();
        int percent = int.Parse(drop.options[drop.value].text.Split('%')[0]);
        gameManager.taxPercent = percent;
        gameManager.taxChanged = true;
        openObjects.Remove(dropdown);
        setTask(false);
        dropdown.SetActive(false);
    }

}
