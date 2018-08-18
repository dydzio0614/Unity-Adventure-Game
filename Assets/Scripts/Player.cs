using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : LivingHumanoid
{  
    public float RotateSpeed;
    public GameObject ArrowPrefab;
    public GameObject EquippedArrow;
    public List<InventoryItem> Inventory;

    private bool isShooting; //variable disallowing multiple shooting colliding with each other
    private bool isSliding;

    private MainCamera mainCamera;
    private UIController UI;

    private GameObject rightHand;
    private GameObject leftHand;

    public GameObject RightHand
    {
        get { return rightHand; }
    }

    public GameObject LeftHand
    {
        get { return leftHand; }
    }

    public System.Action Attack;

    void Start()
    {
        foreach (InventoryItem item in Inventory)
            item.Owner = gameObject;
        UI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        objectController = GetComponent<CharacterController>();
        humanAnimator = GetComponent<Animator>();
        EquippedArrow.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
        leftHand = GameObject.FindGameObjectWithTag("LeftHand");
        Attack = RangedAttack;
    }

	void Update()
    {
        Vector3 movementVector = Vector3.zero;
        Vector3 rotationVector = Vector3.zero;

        if (!UI.dialogueWindowActive && !UI.statWindowActive && !UI.inventoryWindowActive && !isDead)
        {
            movementVector = transform.forward * Input.GetAxis("Vertical") * Speed * Time.deltaTime; //process movement keys
            rotationVector = new Vector3(0, Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X"), 0) * RotateSpeed;
        }
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red);
        

        if (objectController.isGrounded)
        {
            RaycastHit terrainInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out terrainInfo, 10)) //slide detection
            {
                float test = Vector3.Dot(terrainInfo.normal, Vector3.up);
                if (test < 0.7f && objectController.isGrounded)
                {
                    isSliding = true;
                    movementVector = new Vector3(terrainInfo.normal.x, 0, terrainInfo.normal.z) * 4 * Time.deltaTime;
                }
                else
                    isSliding = false;
            }



            if (Input.GetKey(KeyCode.Space) && !isSliding) //jump
            {
                Jump(JumpHeight);
            }

            if ((Input.GetKeyDown(KeyCode.LeftControl)) && !isShooting) //attack
            {
                Attack();         
            }

            if (Input.GetKeyDown(KeyCode.T)) //talk
            {
                Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * 3, Color.red, 1);
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out hitInfo, 3))
                {
                    Debug.Log(hitInfo.collider.gameObject);
                    InteractiveHumanoid target = hitInfo.collider.gameObject.GetComponent<InteractiveHumanoid>();

                    if (target != null && !UI.dialogueWindowActive)
                    {                      
                        foreach (DialogOption option in target.DialogOptions)
                            UI.AddDialogue(option);

                        UI.RefreshEndDialogue();
                        UI.ToggleDialogueWindow();
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.BackQuote)) //open console
        {
            Debug.Log("jest!!!");
        }

        if(Input.GetKeyDown(KeyCode.B)) //open stat window
        {
            UI.ToggleStatWindow(statsVerbal);
        }

        if(Input.GetKeyDown(KeyCode.P)) //view switch
        {
            mainCamera.FirstPerson = !mainCamera.FirstPerson;
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!UI.inventoryWindowActive)
            {
                RefreshInventoryItems();
               /* foreach (InventoryItem item in Inventory)
                    UI.AddItem(item);*/
            }
            UI.ToggleInventoryWindow();
        }

        transform.Rotate(rotationVector); //rotate character depending on input

        if (movementVector != Vector3.zero) //move character depending on input
        {
            humanAnimator.SetBool("run", true);
            objectController.Move(movementVector); //treat coordinates as local and transform to global equivalents
        }
        else
            humanAnimator.SetBool("run", false);
        
	}

    public void RefreshInventoryItems()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            UI.AddItem(Inventory[i], i);
        }
    }

    public void RangedAttack()
    {
        StartCoroutine(Shoot());
    }

    public void MeleeAttack()
    {
        GetComponentInChildren<Weapon>().Attack();
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        EquippedArrow.SetActive(true);

        humanAnimator.SetTrigger("shoot");

        yield return new WaitForSeconds(0.6f);

        EquippedArrow.SetActive(false);

        Arrow arrow = Instantiate(ArrowPrefab).GetComponent<Arrow>();
        arrow.parent = gameObject;
        arrow.Attack();
        yield return new WaitForSeconds(0.4f);
        isShooting = false;
    }
}
