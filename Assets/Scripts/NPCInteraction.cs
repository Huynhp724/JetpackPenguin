using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Put on an object with a trigger that is childed to the main npc parent
// Script deals with dialoging with the player.

public class NPCInteraction : MonoBehaviour
{
    public string playerTag;                                    // tag of Pluck
    public float rotationSpeed;                                 // speed the npc will rotate with speaking or done speaking to you
    public Sprite faceImage;                                    // image of the npc character. Will appear in the dialog box
    public int dialoguePriorityNumber = 0;                      // int that chooses which dialogue the npc will say


    UIController uiController;
    DialogeManager dialogueManager;
    GameObject parentObject, targetObject;
    bool rotateTowardsPlayer;
    Quaternion rotationCheck, previousRotation;
    Dialogue[] dialogueOptions;

    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        dialogueManager = FindObjectOfType<DialogeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        dialogueOptions = GetComponentsInParent<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateTowardsPlayer)
        {

            if (RotateTowards())
            {
                rotateTowardsPlayer = true;
            }
            else
            {
                rotateTowardsPlayer = false;
            }
        }
        else {
            if (previousRotation != null) {
                parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, previousRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    /// <summary>
    /// When Pluck enter trigger turns on button that needs to be pressed to speak and the npc rotates toward pluck
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log(other.name);
            uiController.SetSpeakingInteractionButton(true);

            PlayerInteraction interact = other.GetComponent<PlayerInteraction>();
            interact.SetInteract(true, this);

            previousRotation = parentObject.transform.rotation;
            Debug.Log(previousRotation);
            rotateTowardsPlayer = true;
            targetObject = other.gameObject;


        }
    }

    /// <summary>
    /// When out of trigger button imgage turns off and npc rotates back to original rotation
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log(other.name);
            uiController.SetSpeakingInteractionButton(false);

            PlayerInteraction interact = other.GetComponent<PlayerInteraction>();
            interact.SetInteract(false, null);

            rotateTowardsPlayer = false;

            //parentObject.transform.rotation = previousRotation;
        }
    }


    /// <summary>
    /// Rotates the npc character towards Pluck
    /// </summary>
    /// <returns></returns>
    bool RotateTowards()
    {
        rotationCheck = Quaternion.LookRotation(targetObject.transform.position - parentObject.transform.position);

        parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, rotationCheck, rotationSpeed * Time.deltaTime);

        

        if (Quaternion.Angle(parentObject.transform.rotation, rotationCheck) < 0.1f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SendDialogeInfo() {
        dialogueManager.StartDialogue(dialogueOptions[dialoguePriorityNumber], faceImage);
    }
}
