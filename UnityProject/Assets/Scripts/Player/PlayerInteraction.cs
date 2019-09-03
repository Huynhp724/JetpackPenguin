using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place on the Pluck
// Gets the info of who Pluck is talking to and sets the button to press so you can talk to said npc.

public class PlayerInteraction : MonoBehaviour
{
    public bool canInteractWithNPC = false;                                // whether you can talk to the npc or not

    UIController uiController;
    NPCInteraction npcInInteraction = null;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
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

    public void RedoNPCInteraction() {
        canInteractWithNPC = true;
        uiController.SetSpeakingInteractionButton(true);
    }
}
