using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.IngameUI
{
    public class PlayerUI : MonoBehaviour
    {
        public Player player;

        public Button standardActionButton;
        public Button moveActionButton;
        public Button minorActionButton;
        public Button actionPointButton;
        public Button freeActionButton;
        public Button cancelActionButton;

        public Text choosenStandardAction;
        public Text choosenMoveAction;
        public Text choosenMinorAction;
        public Text choosenFreeAction;
        public Text choosenActionPoint;
        public Text choosenOptionalAction;
        public Text cancelActionText;
        public Text confirmActionText;

        public Dropdown standardActionDropdown;
        public Dropdown moveActionDropdown;
        public Dropdown minorActionDropdown;
        public Dropdown freeActionDropdown;
        public Dropdown actionPointDropdown;
        public Dropdown optionalActionDropdown;

        public Button confirmAction;

        public string currentAction;
        public bool blockPlayerUI;
        void Start()
        {

        }
        void Update()
        {
            var isActing = player.isUsingStandardAction || player.isUsingMoveAction || player.isUsingMinorAction || player.isUsingFreeAction || player.isUsingActionPoint;
            standardActionButton.interactable = player.standardActionCount > 0 && !blockPlayerUI && !isActing;
            standardActionDropdown.interactable = player.isUsingStandardAction && !blockPlayerUI;
            choosenStandardAction.enabled = player.isUsingStandardAction && !blockPlayerUI;

            moveActionButton.interactable = player.moveActionCount > 0 && !blockPlayerUI && !isActing;
            moveActionDropdown.interactable = player.isUsingMoveAction && !blockPlayerUI;
            choosenMoveAction.enabled = player.isUsingMoveAction && !blockPlayerUI;

            minorActionButton.interactable = player.minorActionCount > 0 && !blockPlayerUI && !isActing;
            minorActionDropdown.interactable = player.isUsingMinorAction && !blockPlayerUI;
            choosenMinorAction.enabled = player.isUsingMinorAction && !blockPlayerUI;

            freeActionButton.interactable = !blockPlayerUI && !isActing;
            freeActionDropdown.interactable = player.isUsingFreeAction && !blockPlayerUI;
            choosenFreeAction.enabled = player.isUsingFreeAction && !blockPlayerUI;

            actionPointButton.interactable = !blockPlayerUI && !isActing && player.actionPoint > 0;
            actionPointDropdown.interactable = player.isUsingActionPoint && !blockPlayerUI;
            choosenActionPoint.enabled = player.isUsingActionPoint && !blockPlayerUI;

            optionalActionDropdown.interactable = player.isUsingStandardAction && !blockPlayerUI;
            choosenOptionalAction.enabled = player.isUsingStandardAction && !blockPlayerUI;

            cancelActionButton.interactable = isActing && !blockPlayerUI;
            cancelActionText.enabled = isActing && !blockPlayerUI;


            confirmAction.interactable = isActing && !blockPlayerUI;
            confirmActionText.enabled = isActing && !blockPlayerUI;



            #region ConfirmButton position

            if (player.isUsingStandardAction)
            {
                confirmAction.GetComponent<RectTransform>().transform.localPosition = new Vector3(-373, 167, 0);
                cancelActionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(-290, 167, 0);
            }
            if (player.isUsingMoveAction)
            {
                confirmAction.GetComponent<RectTransform>().transform.localPosition = new Vector3(-201, 208, 0);
                cancelActionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(-118, 208, 0);
            }
            if (player.isUsingMinorAction)
            {
                confirmAction.GetComponent<RectTransform>().transform.localPosition = new Vector3(-35, 208, 0);
                cancelActionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(47, 208, 0);
            }
            if (player.isUsingFreeAction)
            {
                confirmAction.GetComponent<RectTransform>().transform.localPosition = new Vector3(134, 208, 0);
                cancelActionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(218, 208, 0);
            }
            if (player.isUsingActionPoint)
            {
                confirmAction.GetComponent<RectTransform>().transform.localPosition = new Vector3(300, 208, 0);
                cancelActionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(383, 208, 0);
            }
            #endregion
        }

        public void ConfirmAction()
        {
            if (player.isUsingStandardAction)
            {
                var choosedAction = choosenStandardAction.text.ToLower();
                //TODO: abrir opções de standard action
                switch (choosedAction)
                {
                    case "basic attack":
                        //TODO: carregar lista de alvos possiveis;
                        break;
                    case "use power":
                        //TODO: carregar lista de poderes e lista de alvos possiveis;
                        break;
                    case "transform in move":
                        player.moveActionCount++;
                        break;
                }
                player.standardActionCount--;
                player.isUsingStandardAction = false;
                currentAction = null;
            }
            if (player.isUsingMoveAction)
            {
                //TODO: abrir opções de move action
                var choosedAction = choosenMoveAction.text.ToLower();
                //TODO: abrir opções de standard action
                switch (choosedAction)
                {
                    case "walk":
                        player.isMoving = true;
                        break;

                    case "run":
                        player.isMoving = true;
                        player.maxMoves = player.speed +2;
                        break;
                    case "shift":
                        player.isMoving = true;
                        player.maxMoves = 1;
                        break;
                    case "use power":
                        player.isMoving = true;
                        //TODO: carregar lista de poderes usaveis
                        break;
                }
                player.moveActionCount--;
                player.isUsingMoveAction = false;
                currentAction = null;

            }
            if (player.isUsingMinorAction)
            {
                var choosedAction = choosenMinorAction.text.ToLower();
                switch(choosedAction)
                {
                    case "use potion":
                        //TODO: carregar opção só se houver potion no inventário
                        //TODO: carregar lista de potions
                        break;
                    case "skill check":
                        //TODO: carregar todas as skills, mesmo que não haja ação programada.
                        break;
                    case "use power":
                        //TODO: carregar lista de poderes usaveis.
                        break;
                    case "store r. hand":
                        break;
                    case "store l. hand":
                        break;
                }
                player.minorActionCount--;
                player.isUsingMinorAction = false;
                currentAction = null;
            }
            if (player.isUsingFreeAction)
            {
                player.isUsingFreeAction = false;
                currentAction = null;
                //TODO: abrir opções de free action
            }
            if (player.isUsingActionPoint)
            {
                var choosedAction = choosenActionPoint.text.ToLower();
                switch(choosedAction)
                {
                    case "standard action":
                        player.standardActionCount++;
                        break;
                    case "move action":
                        player.moveActionCount++;
                        break;
                    case "minor action":
                        player.minorActionCount++;
                        break;
                }

                player.actionPoint--;
                player.isUsingActionPoint = false;
                currentAction = null;
            }
        }

        public void CancelAction()
        {
            player.isUsingMinorAction = false;
            player.isUsingStandardAction = false;
            player.isUsingMoveAction = false;
            player.isUsingActionPoint = false;
            player.isUsingFreeAction = false;
        }
        public void UseStandardAction()
        {
            player.isUsingStandardAction = true;
            currentAction = "Current action: Standard";
        }
        public void UseMoveAction()
        {
            player.isUsingMoveAction = true;
            currentAction = "Current action: Move";
        }
        public void UseMinorAction()
        {
            player.isUsingMinorAction = true;
            currentAction = "Current action: Minor";
        }
        public void UseFreeAction()
        {
            player.isUsingFreeAction = true;
            currentAction = "Current action: Free";
        }
        public void UseActionPoint()
        {
            player.isUsingActionPoint = true;
            currentAction = "Current action: Action point";
        }

    }
}
