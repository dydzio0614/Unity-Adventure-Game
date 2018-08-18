using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class NpcHpBar : MonoBehaviour
{
    LivingObject target;
    Image image;

    // Use this for initialization
    void Start()
    {
        target = GetComponentInParent<LivingObject>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = target.RelativeHealth;
    }
}
