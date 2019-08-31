using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// place on the Dialogue Panel that has all the Dialog UI. Make sure it has a CanvasGroup attached to it
// Takes the dialoge from a character and prints it out in the dialog box. Also places the name of the character and its image in the right spots

public class DialogeManager : MonoBehaviour
{
    public Image speakerImage;                              // image of where the character will go
    public Text dialogueText;                               // the text of where the dialog will be written
    public Text nameText;                                   // the text of the name
    public PlayerInteraction playerInteraction;

    public AudioClip[] typingClips;

    private Queue<string> sentences;
    bool isDialoging = false;
    CanvasGroup canvasGroup;
    AudioSource source;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        canvasGroup = GetComponent<CanvasGroup>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isDialoging) {
            if (Input.GetKeyDown(KeyCode.Space) && count != 0) {
                DisplayNextSentence();
            }
        }
    }

    /// <summary>
    /// Gets the Dialogue and sprite. Enqueues all the dialogue and places the image.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="imgage"></param>
    public void StartDialogue(Dialogue dialogue, Sprite imgage) { 
        canvasGroup.alpha = 1f;
        speakerImage.sprite = imgage;
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sent in dialogue.sentences) {
            sentences.Enqueue(sent);
            Debug.Log(sent);
            
        }
        DisplayNextSentence();
        
        
    }

    /// <summary>
    /// When pressed the next sentence in the dialoge will be shown
    /// </summary>
    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        source.Stop();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
       // isDialoging = true;
        count = 1;
    }

    IEnumerator TypeSentence(string sentence) {
        int choice = Random.Range(0, typingClips.Length - 1);
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            source.PlayOneShot(typingClips[choice]);
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingClips[choice].length);
            isDialoging = true;
        }
        
    }

    /// <summary>
    /// Turns off the the canvas
    /// </summary>
    void EndDialogue() {
        Debug.Log("End of convo");
        source.Stop();
        StopAllCoroutines();
        canvasGroup.alpha = 0f;
        isDialoging = false;
        count = 0;
        StartCoroutine(RedoInteraction());
       
    }

    IEnumerator RedoInteraction() {
        yield return new WaitForSeconds(1.5f);
        playerInteraction.RedoNPCInteraction();
    }
}
