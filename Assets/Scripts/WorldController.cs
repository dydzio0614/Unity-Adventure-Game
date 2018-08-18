using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{
    bool chieftainVisited;
    bool banditsKilled;

    public GameObject banditQuestGiver;
    public GameObject alchemist;

    GameObject banditForestManager;
    GameObject mountainCampManager;
    GameObject banditHeadquartersManager;
    // Use this for initialization
    void Start() {
        banditForestManager = GameObject.FindGameObjectWithTag("Bandits");
        mountainCampManager = GameObject.FindGameObjectWithTag("MountainCamp");
        banditHeadquartersManager = GameObject.FindGameObjectWithTag("BanditHeadquarters");
        StartCoroutine(CheckForest());
        StartCoroutine(CheckMountain());
        StartCoroutine(CheckHeadquarters());
    }
	
	// Update is called once per frame
	/*void Update()
    {
        
	}*/

    IEnumerator CheckForest()
    {
        while(banditForestManager.transform.childCount > 0)
        {
            yield return new WaitForSeconds(1);
        }
        banditQuestGiver.GetComponent<InteractiveHumanoid>().DialogOptions.Add(new DialogOption()
        {
            option = "Forest bandits do not exist anymore. Does that solve bandit problem?",
            response = "Incredible! I admire your action and you did us a huge favor, but our situation is not much better. They originally came from direction that is currently in front of me, and forest was just one outpost. There must be some mountain passage they are using right now. I will not stop you from investigating. Frenig told me you lost your memory, it seems you were a great warrior before that happened.",
            target = banditQuestGiver.GetComponent<InteractiveHumanoid>(),
            isTemporary = true
        });
        Debug.Log("Bandits killed!");
    }

    IEnumerator CheckMountain()
    {
        while (mountainCampManager.transform.childCount > 0)
        {
            yield return new WaitForSeconds(1);
        }
        banditQuestGiver.GetComponent<InteractiveHumanoid>().DialogOptions.Add(new DialogOption()
        {
            option = "I found small group of bandits near the mountains. I took care of them",
            response = "Great job, our position is safer with each bandit killed.",
            target = banditQuestGiver.GetComponent<InteractiveHumanoid>(),
            isTemporary = true
        });
        Debug.Log("Mountain Bandits killed!");
    }

    IEnumerator CheckHeadquarters()
    {
        while (banditHeadquartersManager.transform.childCount > 0)
        {
            yield return new WaitForSeconds(1);
        }
        banditQuestGiver.GetComponent<InteractiveHumanoid>().DialogOptions.Add(new DialogOption()
        {
            option = "I found and destroyed bandit headquarters, their commander was a knight in black armor!",
            response = "I never expected things to go that way. You are now a hero here. We all are now free, you can go and seek your past, or be honorable member of our village. Now you can go rest, we will feast later and have plenty of time to talk about you.",
            target = banditQuestGiver.GetComponent<InteractiveHumanoid>(),
            isTemporary = true
        });
        Debug.Log("Bandit Headquarters destroyed!");
    }

    public void StartBanditMission() //forest
    {
        StartCoroutine(StartBanditMissionInternal());
    }

    IEnumerator StartBanditMissionInternal()
    {
        foreach (Transform child in banditForestManager.transform)
        {
            child.gameObject.SetActive(true);
            yield return null;
        }
    }

    public void StartBanditBaseMission()
    {
        StartCoroutine(StartBanditBaseMissionInternal());
    }

    IEnumerator StartBanditBaseMissionInternal()
    {
        foreach (Transform child in banditHeadquartersManager.transform)
        {
            child.gameObject.SetActive(true);
            yield return null;
        }
    }
}
