using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour {

    Player player;
    Image image;

	// Use this for initialization
	void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update()
    {
        image.fillAmount = player.RelativeHealth;
	}
}
