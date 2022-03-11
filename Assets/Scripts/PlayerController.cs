using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpImpulsion = 5;
    [SerializeField] private bool isMovingSound = false;

    [Header("References")]
    [SerializeField] private new Camera camera = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private MouseLook mouseLook = null;

    [Header("Pickup")]
    [SerializeField] private float pickupDistance = 3;
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private Transform hand = null;

    [Header("Interact")]
    [SerializeField] private float interactDistance = 3;
    [SerializeField] private LayerMask interactMask;

    [Header("Crouching References")]
    [SerializeField] private CapsuleCollider standingCollider = null;
    [SerializeField] private CapsuleCollider crouchingCollider = null;
    [SerializeField] private Transform standingCameraPosition = null;
    [SerializeField] private Transform crouchingCameraPosition = null;

    [Header("UIReferences")]
    [SerializeField] private GameObject _hud = null;

    private TimeManager timeManager;

    public GameObject pickup = null;

    private bool isCrouched = false;

    public bool isOnStair = false;

    public bool isInConduit = false;

    private IInteractable interactableInRange = null;

    private bool isDead = false;

    public bool isInteractingWithScreen = false;

    public bool isInvestigationOpened = false;

    private GameObject _cursor = null;

    // Start is called before the first frame update
    void Start()
    {
        _cursor = GameObject.Find("Cursor");
        _hud = GameObject.Find("HUD");
        timeManager = FindObjectOfType<TimeManager>();
        playerMovement.OnCharacterLanded += PlayerLanded;
    }

    // Update is called once per frame
    void Update()
    {
        // Nouvelle façon de déplacer le joueur à partir de la vidéo de Brackey
        // Le code de la rotation du joueur par rapport à la souris a été déplacé vers MouseLook
        float x;
        float z;
        if (!isInvestigationOpened)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
        }
        else
        {
            x = 0;
            z = 0;
        }

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        //Désactive la crosshair si le joueur est en mode focus
        if (isInteractingWithScreen)
            _cursor.SetActive(false);
        else
            _cursor.SetActive(true);

        if (isDead == false && !isInteractingWithScreen)
        {
            if(timeManager.multiplier < 0.05f)
                playerMovement.Move(move * speed); //Evite que le joueur augmente trop sa vitesse lorsque le lerp est proche de 0
            else
                playerMovement.Move(move * speed / Time.timeScale);
        }
        else
        {
            playerMovement.Move(new Vector3(0,0,0));
        }
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.2f && isMovingSound == false && move != new Vector3(0,0,0))
        {
            if (playerMovement.isGrounded == true)
            {
                if (isCrouched)
                {
                    FindObjectOfType<SoundManager>().Stop("Walk");
                    FindObjectOfType<SoundManager>().Stop("WalkStair");
                    if(!isInConduit)
                    {
                        FindObjectOfType<SoundManager>().Play("Crawl", 0f);
                        FindObjectOfType<SoundManager>().Stop("CrawlConduit");
                    }
                    else
                    {
                        FindObjectOfType<SoundManager>().Play("CrawlConduit", 0f);
                        FindObjectOfType<SoundManager>().Stop("Crawl");


                    }


                }
                else if (isOnStair)
                {
                    FindObjectOfType<SoundManager>().Stop("Walk");
                    FindObjectOfType<SoundManager>().Stop("Crawl");
                    FindObjectOfType<SoundManager>().Play("WalkStair", 0f);
                    FindObjectOfType<SoundManager>().Stop("CrawlConduit");

                }
                else
                {
                    FindObjectOfType<SoundManager>().Stop("WalkStair");
                    FindObjectOfType<SoundManager>().Stop("Crawl");
                    FindObjectOfType<SoundManager>().Play("Walk", 0f);
                    FindObjectOfType<SoundManager>().Stop("CrawlConduit");


                }

            }
            else
            {
                FindObjectOfType<SoundManager>().Stop("Crawl");
                FindObjectOfType<SoundManager>().Stop("WalkStair");
                FindObjectOfType<SoundManager>().Stop("Walk");
                FindObjectOfType<SoundManager>().Stop("CrawlConduit");

            }
            isMovingSound = true;
        }
        else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0.2f && isMovingSound == true)
        {
            FindObjectOfType<SoundManager>().Stop("Crawl");
            FindObjectOfType<SoundManager>().Stop("WalkStair");
            FindObjectOfType<SoundManager>().Stop("Walk");
            FindObjectOfType<SoundManager>().Stop("CrawlConduit");

            isMovingSound = false;
        }
        if (playerMovement.isGrounded == false)
        {
            FindObjectOfType<SoundManager>().Stop("Crawl");
            FindObjectOfType<SoundManager>().Stop("WalkStair");
            FindObjectOfType<SoundManager>().Stop("Walk");
            FindObjectOfType<SoundManager>().Stop("CrawlConduit");

            isMovingSound = false;
        }

        CheckInteractable();

        if (!isDead && !isInvestigationOpened && !isInteractingWithScreen)
        {
            if (Input.GetButtonDown("Jump"))
            {
                playerMovement.Jump(jumpImpulsion);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                if (isCrouched)
                {
                    camera.transform.position = standingCameraPosition.position;
                    standingCollider.enabled = true;
                    crouchingCollider.enabled = false;
                    isCrouched = false;
                    isMovingSound = false;
                }
                else
                {
                    camera.transform.position = crouchingCameraPosition.position;
                    standingCollider.enabled = false;
                    crouchingCollider.enabled = true;
                    isCrouched = true;
                    isMovingSound = false;
                }
            }
        }

        if (!isInvestigationOpened)
        {
            // Utilise l'item porté
            if (Input.GetButtonDown("Interact"))
            {
                if (interactableInRange != null)
                {
                    interactableInRange.Interact(pickup, this);
                }
                else if (pickup != null)
                {
                    /*if (pickup.GetComponent<FoodType>().Foodtype == "Eat")
                    {
                        FindObjectOfType<SoundManager>().ChangePitch("Eat");
                        FindObjectOfType<SoundManager>().Play("Eat");
                    }
                    else if (pickup.GetComponent<FoodType>().Foodtype == "Drink")
                    {
                        FindObjectOfType<SoundManager>().ChangePitch("Drink");
                        FindObjectOfType<SoundManager>().Play("Drink");
                    }*/
                    //Destroy(pickup);
                    //pickup = null;
                }
            }

            // Item nécessitant que le joueur presse longtemps le bouton interaction
            if (Input.GetButton("Interact"))
            {
                if (interactableInRange != null)
                {
                    interactableInRange.InteractHolding(pickup, this);
                }
            }

            // Le joueur relâche le boutton d'interaction
            if (Input.GetButtonUp("Interact"))
            {
                if (interactableInRange != null)
                {
                    interactableInRange.StopInteractHolding(pickup, this);
                }
            }
        }

        /*
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            //Change buttons to be interactables
            if (hit.collider.tag == "Button" )
            {
                if (timeManager.multiplier !=0)
                {
                    if (Input.GetKeyDown("e") == true)
                    {
                        hit.collider.GetComponent<Button>().clicked = true;
                    }
                }

            }

        }*/
        if (!isInvestigationOpened)
        {
            if (pickup == null)
            {
                if (Input.GetButtonDown("Pickup"))
                {
                    TryPickupItem();
                }
            }


            //Lache l'item
            if (pickup != null)
            {
                if (Input.GetButtonDown("Drop"))
                {
                    pickup.transform.parent = null;
                    pickup = null;
                }
            }
        }


        if (Input.GetButtonDown("OpenInvestigation"))
        {
            OpenInvestigationPressed();
        }

        if(Input.GetKeyDown(KeyCode.Equals))
        {
            timeManager.RestartLoop();
        }

        if(!isInvestigationOpened)
        {
            if (Input.GetButtonDown("StopTime"))
            {
                timeManager.StopTimePressed();
            }

            if (Input.GetButtonDown("SlowTime"))
            {
                timeManager.SlowTimePressed();
            }

            if (Input.GetButtonDown("NormalTime"))
            {
                timeManager.NormalTimePressed();
            }

            if (Input.GetButtonDown("SpeedTime"))
            {
                timeManager.SpeedTimePressed();
            }

            if (Input.GetButtonDown("RewindTime"))
            {
                timeManager.RewindTimePressed();
            }
        }


        // A RETIRER c'est pour couper le son du lore lors de présentation du build
        if (Input.GetKeyDown("r"))
            FindObjectOfType<SoundManager>().CheckLoreSound();

        // A RETIRER sert à activer l'écran de débuging
        if (Input.GetKeyDown("m"))
        {
            GameObject DebugScreen = _hud.transform.Find("DebugScreen").gameObject;
            DebugScreen.SetActive(!DebugScreen.activeInHierarchy);
        }

        // A RETIRER sert à changer rapidement le temps ingame
        if (!isInvestigationOpened)
        {
            if (Input.GetKeyDown("6"))
                timeManager.currentLoopTime -= 60;
            if (Input.GetKeyDown("7"))
                timeManager.currentLoopTime += 60;
        }
    }

    public void ReleaseHeldObject()
    {
        pickup = null;
    }

    private void OpenInvestigationPressed()
    {
        GameObject investigationPanel = _hud.transform.Find("InvestigationPanel").gameObject;
        if (investigationPanel)
        {
            investigationPanel.SetActive(!investigationPanel.activeInHierarchy);
            mouseLook.ChangeCursorLockMode(!investigationPanel.activeInHierarchy);
            isInvestigationOpened = (investigationPanel.activeInHierarchy);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().Play(investigationPanel.activeInHierarchy ? "InvestigationOpen" : "InvestigationClose" , 0);
        }
    }

    private void PlayerLanded(float fallenDistance)
    {

    }

    private void TryPickupItem()
    {
        RaycastHit hit;
        if (timeManager.IsTimeStopped == false )
        {
            if (Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hit, pickupDistance, pickupMask))
            {
                //Prend l'item si ce dernier n'est pas accroché à un ObjectReceiver ou s'il n'y a pas de Rewind en cours
                //if (hit.collider.gameObject.transform.parent == false || !hit.collider.gameObject.transform.parent.CompareTag("ObjectReceiver"))
                //{
                    if (timeManager.rewindManager.isRewinding == false)
                    {
                        pickup = hit.collider.gameObject;
                        pickup.transform.parent = hand;
                        pickup.transform.position = hand.position;
                    }
                //}
            }
        }
    }

    private void CheckInteractable()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hit, interactDistance, interactMask))
        {
            IInteractable foundInteractable = hit.collider.GetComponent<IInteractable>();
            if(interactableInRange != foundInteractable)
            {
                if (interactableInRange != null)
                {
                    interactableInRange.PlayerHoverEnd();
                }

                interactableInRange = foundInteractable;
                interactableInRange.PlayerHoverStart();
            }
        }
        else 
        {
            if (interactableInRange != null)
            {
                interactableInRange.PlayerHoverEnd();
            }
            interactableInRange = null;
        }
    }




    // A réutiliser pour le nouveau système de loop
    IEnumerator Death()
    {
        // Empêche le joueur de bouger afin de simuler une "animation de mort" pendant la durée du son de mort et relance la loop une fois le son terminé.
        isDead = true;
        yield return new WaitForSeconds(FindObjectOfType<SoundManager>().FindSoundClip("Death").length);
        timeManager.RestartLoop();
        yield return null;
    }

    private void Die()
    {
        // Lance la couroutine de mort du joueur
        FindObjectOfType<SoundManager>().Play("Death", 0f);
        StartCoroutine("Death");
    }

    public void EnterStair()
    {
        isOnStair = true;
        isMovingSound = false;
    }
    public void ExitStair()
    {
        isOnStair = false;
        isMovingSound = false;
    }

    public void EnterConduit()
    {
        isInConduit = true;
        isMovingSound = false;
    }
    public void ExitConduit()
    {
        isInConduit = false;
        isMovingSound = false;
    }
}
