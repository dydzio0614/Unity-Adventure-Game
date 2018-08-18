using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    private Transform player;

    public bool FirstPerson { get; set; }
    public bool FollowingPlayer { get; set; }
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FollowingPlayer = true;
	}
	
	// Update is called once per frame
	void LateUpdate()
    {
        if (FollowingPlayer && player != null)
        {
            transform.position = player.position + player.up * 2 + player.forward * -3 + (FirstPerson == true ? transform.forward * 3.25f : Vector3.zero);

            //transform.rotation.eulerAngles.Set(10, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(10, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
        }
    }

    public void ViewPoint(Vector3 target)
    {
        StartCoroutine(ViewPointInternal(target));
    }

    IEnumerator ViewPointInternal(Vector3 target)
    {
        FollowingPlayer = false;
        transform.rotation = Quaternion.LookRotation(target - transform.position);
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.6f * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        FollowingPlayer = true;
    }
}
