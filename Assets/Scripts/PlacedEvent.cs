using UnityEngine;
using System.Collections;

public enum EventTarget
{
    PlayerOnly,
    Enemies,
    AllLivingObjects
}

public class PlacedEvent : MonoBehaviour
{
    public EventTarget Target;
    public string Message;
    public float MessageTime = 5;
    public bool deleteAfterUse;
    public GameObject cameraMarker;

    UIController UI;
    MainCamera mainCamera;
    
	// Use this for initialization
	void Start()
    {
        UI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
    }
	
    /*
	void Update()
    {
	
	}*/

    void OnTriggerEnter(Collider other)
    {
        if(Target == EventTarget.AllLivingObjects)
        {
        }
        else if(Target == EventTarget.Enemies)
        {
        }
        else if(Target == EventTarget.PlayerOnly)
        {
            if(other.GetComponent<Player>() != null)
            {
                if(!string.IsNullOrEmpty(Message))
                {
                    UI.DisplayMessage(Message, MessageTime);
                }
                if(cameraMarker != null)
                {
                    mainCamera.ViewPoint(cameraMarker.transform.position);
                }

                if (deleteAfterUse)
                    Destroy(gameObject);
            }
        }
    }
}
