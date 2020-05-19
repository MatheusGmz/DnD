using Assets.Scripts.IngameUI;
using Assets.Scripts.UnitScript;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public bool isUsingStandardAction;
    public bool isUsingMoveAction;
    public bool isUsingMinorAction;

    public PlayerUI playerUI;

    public Actor actor;

    public Text actingNow;

    public Text currentAction;
    public Text standardActionCounter;
    public Text moveActionCounter;
    public Text minorActionCounter;
    public Text actionPoint;

    public List<string> initiativeOrder;

    int listPosition = 0;
    public bool isPlayerTurn;

    void Start()
    {
        var playerList = FindObjectsOfType<Player>();
        foreach (var player in playerList)
        {
            initiativeOrder.Add(player.name);
        }
        var enemyList = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemyList)
        {
            initiativeOrder.Add(enemy.name);
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(playerUI.currentAction))
        {
            currentAction.text = playerUI.currentAction;
        }
        else
        {
            currentAction.text = "Current action: None";
        }
        var currentActorName = initiativeOrder[listPosition];
        var currentActor = GameObject.Find(currentActorName);
        actingNow.text = "Acting now: " + currentActorName;
        var isPlayer = currentActor.GetComponent<Player>() != null;
        if (isPlayer)
        {
            standardActionCounter.text = "Standard Action: " + currentActor.GetComponent<Player>().standardActionCount.ToString();
            moveActionCounter.text = "Move Action: " + currentActor.GetComponent<Player>().moveActionCount.ToString();
            minorActionCounter.text = "Minor Action: " + currentActor.GetComponent<Player>().minorActionCount.ToString();
            actionPoint.text = "Action point: " + currentActor.GetComponent<Player>().actionPoint.ToString();

            actor.player = currentActor.GetComponent<Player>();
            isPlayerTurn = true;
            playerUI.blockPlayerUI = false;

        }
        else
        {
            actor.enemy = currentActor.GetComponent<Enemy>();
            isPlayerTurn = false;
            playerUI.blockPlayerUI = true;
        }
    }
    public void NextTurn()
    {
        listPosition++;
        if (listPosition >= initiativeOrder.Count)
        {
            listPosition = 0;
        }
        var actor = GameObject.Find(initiativeOrder[listPosition]).GetComponent<Actor>();
        actor.standardActionCount = 1;
        actor.moveActionCount = 1;
        actor.minorActionCount = 1;
    }

}
