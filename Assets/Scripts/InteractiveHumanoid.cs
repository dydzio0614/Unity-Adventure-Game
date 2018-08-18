using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveHumanoid : LivingHumanoid {

    public List<Vector3> PathPoints;
    public List<DialogOption> DialogOptions;

    Vector3 SelectedPoint;
    bool walking;

	// Use this for initialization
	void Start()
    {
        foreach (DialogOption dialogOption in DialogOptions)
        {
            dialogOption.target = this;
        }

        PathPoints.Add(transform.position);

        humanAnimator = GetComponent<Animator>(); //TODO: remove this boilerplate code
        objectController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update()
    {
        if (PathPoints.Count > 1)
        {
            if (!walking)
            {
                if (Random.Range(0, 5000) < 10)
                {
                    SelectedPoint = PathPoints[Random.Range(0, PathPoints.Count)];

                    walking = true;
                    transform.rotation = Quaternion.LookRotation(SelectedPoint - transform.position);
                }
            }
            else if (Vector3.Distance(transform.position, SelectedPoint) > 3)
            {
                MoveForward(Speed);
            }
            else
            {
                humanAnimator.SetBool("run", false);
                walking = false;
            }
        }
	}
}
