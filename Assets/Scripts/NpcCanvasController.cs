using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NpcCanvasController : MonoBehaviour
{
    /*[SerializeField]
    private GameObject HPBar;
    [SerializeField]
    private GameObject HPBarFrame;
    [SerializeField]
    private GameObject NameText;*/

    GameObject mainCamera;

    Text nameText;

    Image healthBar;

    LivingObject targetNPC;

    // Use this for initialization
    void Start()
    {
        IEnumerable<Image> imageList = GetComponentsInChildren<Image>();

        if (imageList != null)
        {
            foreach (Image image in imageList)
                if (image.type == Image.Type.Filled)
                    healthBar = image;
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        targetNPC = GetComponentInParent<LivingObject>();
        nameText = GetComponentInChildren<Text>();
        if(nameText != null)
            nameText.text = targetNPC.LivingObjectName;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar != null)
            healthBar.fillAmount = targetNPC.RelativeHealth;

        transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - mainCamera.transform.forward * 100 - transform.position);
    }
}
