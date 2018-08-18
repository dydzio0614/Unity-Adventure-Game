using UnityEngine;
using System.Collections;

public class Arrow : Weapon
{
    float flyTime = 3.8f;
    bool flying;
    float speed = 30;

    Rigidbody arrowRigidbody;

    void Update()
    {
        if(flying) // redundant?
        {
            if (flyTime > 0)
            {
                //transform.Translate(new Vector3(0.3f, 0, 0.245f) * Time.deltaTime * speed);
                //transform.Translate(new Vector3(0.5f, 0, 0) * Time.deltaTime * speed);
                
                flyTime -= Time.deltaTime;
            }
            else
                Destroy(gameObject);
        }
    }

    public override void Attack() //move to Start() and dont call Attack ?
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        transform.position = parent.transform.position + parent.transform.forward + new Vector3(0, 1.25f, 0); //assign player position + offset
        transform.rotation = parent.transform.rotation;

        if(parent.tag == "Player") // can use if(parent.GetComponent<Player>() != null) too
        {
            transform.Rotate(new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-2f, 2f), 0));
        }

        //transform.Rotate(0, -51, 0); //FOR OLD PREFAB
        //transform.Rotate(0, -90, 0); //put additional rotation so arrow is positioned towards view
        flying = true;
        arrowRigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
