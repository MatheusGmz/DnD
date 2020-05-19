using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UnitScript
{
    public class Actor : MonoBehaviour
    {
        public int initiativeValue = 5;
        public Text counter;
        public Text standardAction;
        public Text moveAction;
        public Text minorAction;
        public Player player;
        public Enemy enemy;

        public int standardActionCount = 1;
        public int moveActionCount = 1;
        public int minorActionCount = 1;
        public int actionPoint = 1;
        private void Start()
        {
            
        }
        public void UseStandardAction()
        {
            standardAction.text = "Standard Action: Used.";
            standardActionCount--;
        }
        public void UseMoveAction()
        {
            moveAction.text = "Move Action: Used.";
            moveActionCount--;
            if (player != null)
            {
                player.isMoving = true;
            }
        }
        public void UseMinorAction()
        {
            minorAction.text = "Minor Action: Used.";
            minorActionCount--;
        }

        public void UseActionPoint()
        {
            actionPoint--;
        }

    }
}
