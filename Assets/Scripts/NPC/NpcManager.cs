using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcManager : MonoBehaviour
{
    public bool playerIsClose = false;

    const string npcDialogue = "Hi Player, welcome to my shop. Please have a look at what you need.";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && playerIsClose)
        {
            if (!DialogueManager.Instance.IsActiveInHierarchy()){
                DialogueManager.Instance.ToggleDialogue(true);
                StartCoroutine(ShowDialogue());
            }
        }
    }

    private IEnumerator ShowDialogue()
    {
        DialogueManager.Instance.dialogueText.text = "";
        foreach (char letter in npcDialogue.ToCharArray())
        {
            DialogueManager.Instance.dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ClearDialogue()
    {
        if(DialogueManager.Instance){
            DialogueManager.Instance.dialogueText.text = "";
            DialogueManager.Instance.ToggleDialogue(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) { 
        if (other.CompareTag("Player")) {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            playerIsClose = false;
            ClearDialogue();
        }
    }
}
