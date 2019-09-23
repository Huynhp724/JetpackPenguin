using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


// Place on the Pluck
// Gets the info of who Pluck is talking to and sets the button to press so you can talk to said npc.

public class PlayerInteraction : MonoBehaviour
{
    public bool canInteractWithNPC = false;                                // whether you can talk to the npc or not

    UIController uiController;
    NPCInteraction npcInInteraction = null;
    Player player;
    PlayerController playerController;
    Rigidbody rb;



    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteractWithNPC)
        {
            if ((player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.Q)) &&
                (playerController.GetCurrentState() != PlayerController.State.Dashing) && 
                (playerController.GetCurrentState() != PlayerController.State.Flapping) && (playerController.onGround))
            {
                EnablePlayerController(false);
                npcInInteraction.SendDialogeInfo();
                uiController.SetSpeakingInteractionButton(false);
                canInteractWithNPC = false;
                // make sure to freeze movement while Dialoging with other npcs
            }
        }
    }

    /// <summary>
    /// sets if you can talk and sets the info of the npc character you are talking to
    /// </summary>
    /// <param name="interact"></param>
    /// <param name="npc"></param>
    public void SetInteract(bool interact, NPCInteraction npc)
    {
        canInteractWithNPC = interact;
        npcInInteraction = npc;
    }

    public void EnablePlayerController(bool enable) {
        if (!enable)
        {
            //rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            playerController.enabled = enable;
            
        }
        else {
            //rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            playerController.enabled = enable;
            
        }
    }

    public void RedoNPCInteraction() {
        //canInteractWithNPC = true;
        //uiController.SetSpeakingInteractionButton(true);
    }
}
