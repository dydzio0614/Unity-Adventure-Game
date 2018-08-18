using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject DialogueWindow;
    [SerializeField]
    private GameObject MessageWindow;
    [SerializeField]
    private GameObject GameplayInterface;
    [SerializeField]
    private GameObject StatWindow;
    [SerializeField]
    private GameObject InventoryWindow;
    [SerializeField]
    private GameObject QuickMessage;

    private WorldController worldController;

    private MainCamera mainCamera;

    /// <summary>
    /// Child scrollable container of InventoryWindow, that fits size to its contents.
    /// </summary>
    private GameObject InventoryList;
    /// <summary>
    /// Child Text area of InventoryWindow, near Inventory Item List designed to show highlighted item details.
    /// </summary>
    private GameObject ItemDetails;

    //read only properties used to determine state of UI togglable windows
    public bool dialogueWindowActive { get { return DialogueWindow.activeSelf; } }
    public bool statWindowActive { get { return StatWindow.activeSelf; } }
    public bool inventoryWindowActive { get { return InventoryWindow.activeSelf; } }

    void Start () {
        worldController = GameObject.FindGameObjectWithTag("WorldController").GetComponent<WorldController>();
        InventoryList = InventoryWindow.transform.GetChild(0).GetChild(0).GetChild(0).gameObject; //this code assumes proper item order in editor
        ItemDetails = InventoryWindow.transform.GetChild(1).gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
	}

    public void ToggleDialogueWindow()
    {
        if (DialogueWindow.activeSelf)
        {
            DialogueWindow.SetActive(false);
            foreach(Transform child in DialogueWindow.transform) //special loop iterating thru child objects
            {
                Destroy(child.gameObject);
            }

            if (MessageWindow.activeSelf)
                MessageWindow.SetActive(false);
        }
        else
            DialogueWindow.SetActive(true);
    }

    public void ToggleStatWindow(string info)
    {
        if (StatWindow.activeSelf)
        {
            StatWindow.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            StatWindow.GetComponentInChildren<Text>().text = info;
            StatWindow.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ToggleInventoryWindow() //merge with DialogueWindow?
    {
        if (InventoryWindow.activeSelf)
        {
            InventoryWindow.SetActive(false);
            DeleteItems();
        }
        else
            InventoryWindow.SetActive(true);
    }

    /// <summary>
    /// Displays message in rectangle on the upper part of game window
    /// </summary>
    /// <param name="message">Text to display</param>
    /// <param name="displayTime">Amount of time for message window to last</param>
    public void DisplayMessage(string message, float displayTime)
    {
        StartCoroutine(DisplayMessageInternal(message, displayTime));
    }

    IEnumerator DisplayMessageInternal(string message, float displayTime)
    {
        MessageWindow.SetActive(true);
        MessageWindow.transform.GetChild(0).GetComponent<Text>().text = message;
        yield return new WaitForSeconds(displayTime);
        MessageWindow.SetActive(false);
        Debug.Log("event routine end");
    }

    public void AddDialogue(DialogOption data)
    {
        GameObject dialogueOption = Instantiate(Resources.Load<GameObject>("DialogueOption"));
        dialogueOption.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = data.option;
        dialogueOption.GetComponent<RectTransform>().SetParent(DialogueWindow.GetComponent<RectTransform>());
        dialogueOption.GetComponent<Button>().onClick.AddListener(() => DialogueClicked(data)); //cannot assign event handler from scene object in button prefab so do it here
    }

    public void AddItem(InventoryItem data, int offsetIndex)
    {
        GameObject inventoryOption = Instantiate(Resources.Load<GameObject>("ItemOption"));
        inventoryOption.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = data.Name;
        inventoryOption.GetComponent<RectTransform>().SetParent(InventoryList.GetComponent<RectTransform>());
        inventoryOption.GetComponent<Button>().onClick.AddListener(() => data.Use());
    }

    public void DeleteItems()
    {
        foreach (Transform child in InventoryList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayItemDetails(InventoryItem data)
    {
        ItemDetails.GetComponent<Text>().text = data.ToString();
    }

    /// <summary>
    /// Deletes old (if possible) and adds new dialogue that ends conversation at the end of the list.
    /// </summary>
    public void RefreshEndDialogue()
    {
        DeleteDialogueButton("END");

        DialogOption data = new DialogOption() { option = "END" };
        GameObject dialogueOption = Instantiate(Resources.Load<GameObject>("DialogueOption"));
        dialogueOption.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = data.option;
        dialogueOption.GetComponent<RectTransform>().SetParent(DialogueWindow.GetComponent<RectTransform>());
        dialogueOption.GetComponent<Button>().onClick.AddListener(() => DialogueClicked(data));
    }

    /// <summary>
    /// Deletes all active dualogue options that match exactly provided text.
    /// </summary>
    /// <param name="text">Dialogue option text to search for.</param>
    public void DeleteDialogueButton(string text)
    {
        foreach(Transform child in DialogueWindow.transform)
        {
            if (child.gameObject.transform.GetChild(0).GetComponent<Text>().text == text)
                Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Display small font text at the center of the screen.
    /// </summary>
    /// <param name="text">Text to display.</param>
    /// <param name="duration">Time in seconds indicating how long the message lasts.</param>
    public void DisplayQuickMessage(string text, float duration)
    {
        StartCoroutine(DisplayQuickMessageInternal(text, duration));
    }

    private IEnumerator DisplayQuickMessageInternal(string text, float duration)
    {
        QuickMessage.SetActive(true);
        QuickMessage.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(duration);
        QuickMessage.SetActive(false);
    }

    /// <summary>
    /// Methodcalled on dialogue button click. Processes all additional message events.
    /// </summary>
    /// <param name="data">Dialogue data associated with dialogue button.</param>
    public void DialogueClicked(DialogOption data)
    {
        if (data.option == "END")
        {
            ToggleDialogueWindow();
            return;
        }

        if (!MessageWindow.activeSelf)
            MessageWindow.SetActive(true);
        MessageWindow.transform.GetChild(0).GetComponent<Text>().text = data.response;


        //FRENIG
        if(data.option == "What is going on? I recently woke up, and met two hostile people on my way here.")
        {
            InteractiveHumanoid villageChief = worldController.banditQuestGiver.GetComponent<InteractiveHumanoid>();
            DialogOption Dialogue = new DialogOption() { option = "I had spoken to Frenig, and he said you can tell me more about bandits",
                response = "Yes. It is simple, they came out of nowhere like three weeks ago from the east and started raiding our village. We managed to defend for now, but they grew stronger and also captured the forest. Nobody who went there has yet returned.", target = villageChief, isTemporary = true };
            villageChief.DialogOptions.Add(Dialogue);
        }

        //KUKAZU
        else if (data.option == "What are you doing here?")
        {
            InteractiveHumanoid alchemist = worldController.alchemist.GetComponent<InteractiveHumanoid>();
            DialogOption Dialogue = new DialogOption()
            {
                option = "How can I help you?",
                response = "I need red mushrooms. I will not gather them myself, as I am busy with my research, and bandits make things even harder. I will give you small healing potion for each one you bring to me.",
                target = alchemist,
                isTemporary = true
            };
            alchemist.DialogOptions.Add(Dialogue);
            AddDialogue(Dialogue);
            RefreshEndDialogue();
        }

        else if (data.option == "How can I help you?")
        {
            InteractiveHumanoid alchemist = worldController.alchemist.GetComponent<InteractiveHumanoid>();
            DialogOption Dialogue = new DialogOption()
            {
                option = "I want to trade mushrooms",
                response = "Let's see... I will give you a healing potion for each mushroom you bring here.",
                target = alchemist,
                isTemporary = false
            };
            alchemist.DialogOptions.Add(Dialogue);
            AddDialogue(Dialogue);
            RefreshEndDialogue();
        }

        else if (data.option == "I want to trade mushrooms")
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            int firstMushroomIndex = player.Inventory.FindIndex(x => x.Name.ToLower() == "red mushroom");
            if(firstMushroomIndex != -1)
            {
                player.Inventory.RemoveAt(firstMushroomIndex);
                FoodItem potion = ScriptableObject.CreateInstance<FoodItem>();
                potion.Name = "Small healing potion";
                potion.Owner = player.gameObject;
                potion.HealAmount = 2;
                potion.Description = "A healing potion I received from Kukazu";
                player.Inventory.Add(potion);
                //player.Inventory.Add(new FoodItem() { Name = "Small healing potion", Owner = player.gameObject, HealAmount = 2, Description = "A healing potion I received from Kukazu" });
            }
        }

        //VILLAGE CHIEF
        else if (data.option == "I had spoken to Frenig, and he said you can tell me more about bandits")
        {
            mainCamera.ViewPoint(GameObject.FindGameObjectWithTag("ForestCameraMarker").transform.position);
            DialogOption Dialogue = new DialogOption() { option = "I want to help. I will try to kill the bandits.", response = "WHAT? Are you serious? Currently nobody of us can help you, as direct attack would be a suicide in our opinion. Feel free to scout though, and be careful.", target = data.target, isTemporary = true };
            data.target.DialogOptions.Add(Dialogue);
            AddDialogue(Dialogue);
            RefreshEndDialogue();
        }

        else if (data.option == "I want to help. I will try to kill the bandits.")
        {
            worldController.StartBanditMission();
        }

        else if (data.option == "Forest bandits do not exist anymore. Does that solve bandit problem?")
        {
            worldController.StartBanditBaseMission();
        }

        else if (data.option == "I found and destroyed bandit headquarters, their commander was a knight in black armor!")
        {
            DisplayQuickMessage("You won the game! You can now continue playing and explore the world if you want.", 60);
        }

        if (data.isTemporary)
        {
            data.target.DialogOptions.Remove(data);
            DeleteDialogueButton(data.option);
        }
    }
}
