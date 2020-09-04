using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;

public static class DataHandler {

    public static void save(GameManager gameManager) {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";
        FileStream stream  = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameManager);
        Debug.Log("SAVE");

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData load(GameManager gameManager) {
        string path = Application.persistentDataPath + "/player.dat";

        if (hasLoadedFile()) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            Debug.Log("LOAD");

            PlayerData data = (PlayerData) formatter.Deserialize(stream);

            gameManager.gold = data.gold;
            gameManager.year = data.year;
            gameManager.wood = data.wood;
            gameManager.stone = data.stone;
            gameManager.yearlyExpenses = data.yearlyExpenses;
            gameManager.taxPercent = data.taxPercent;
            gameManager.happiness = data.happiness;
            gameManager.popIncome = data.popIncome;
            gameManager.population = data.population;
            gameManager.leather = data.leather;
            gameManager.soldierCount = data.soldierCount;
            gameManager.soldierStrength = data.soldierStrength;
            gameManager.ownsBarracks = data.ownsBarracks;
            gameManager.ownsMarket = data.ownsMarket;
            gameManager.ownsTavern = data.ownsTavern;
            gameManager.ownsWatchtower = data.ownsWatchtower;
            gameManager.ownsGarrison = data.ownsGarrison;
            gameManager.ownsDruid = data.ownsDruid;
            gameManager.tradeRym = data.tradeRym;
            gameManager.tradeJalonn = data.tradeJalonn;
            gameManager.tradeCobeth = data.tradeCobeth;
            gameManager.tradeGalerd = data.tradeGalerd;
            gameManager.isAtWar = data.isAtWar;
            gameManager.warStatus = data.warStatus;
            gameManager.conqueredStatus = data.conqueredStatus;
            gameManager.puppetStates = data.puppetStates;

            gameManager.relationsWithCobeth = data.relationsWithCobeth;
            gameManager.relationsWithGalerd = data.relationsWithGalerd;
            gameManager.relationsWithJalonn = data.relationsWithJalonn;
            gameManager.relationsWithRym = data.relationsWithRym;

            gameManager.woodValue = data.woodValue;
            gameManager.stoneValue = data.stoneValue;
            gameManager.leatherValue = data.leatherValue;

            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            canvas.GetComponent<ResourceUI>().updateResourceText();

            foreach (GameManager.Kingdom k in gameManager.warStatus.Keys) {
                gameManager.updateTradeStatus(k);
            }
            

            stream.Close();

            return data;
        } else {
            Debug.Log("Save file not found.");
            return null;
        }
    }

    public static bool hasLoadedFile() {
        string path = Application.persistentDataPath + "/player.dat";

        if (File.Exists(path)) {
            return true;
        } else {
            return false;
        }
    }
}
