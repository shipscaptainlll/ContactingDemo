using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour, IContactable
{
    [SerializeField] private ConversationManager _conversationManager;
    [SerializeField] private List<Dialogue> _startDialogues;
    [SerializeField] private LookAtPlayer _lookAtPlayer;
    [SerializeField] private Transform _lookAtCharacter;

    private List<Dialogue> dialoguesNotYetShown;
    private Dialogue lastShownDialogue = null;

    private void Start()
    {
        ResetDialogues();
    }

    public void GetContacted()
    {

        if (dialoguesNotYetShown.Count == 0)
        {
            ResetDialogues();
            if (_startDialogues.Count > 2 && lastShownDialogue != null)
            {
                dialoguesNotYetShown.Remove(lastShownDialogue);
            }
        }

        Dialogue selectedDialogue = GetRandomDialogue();

        _conversationManager.InitiateConversation(selectedDialogue, _lookAtPlayer, _lookAtCharacter);
        lastShownDialogue = selectedDialogue;
    }

    private Dialogue GetRandomDialogue()
    {
        int randomIndex = Random.Range(0, dialoguesNotYetShown.Count);
        Dialogue selectedDialogue = dialoguesNotYetShown[randomIndex];

        dialoguesNotYetShown.RemoveAt(randomIndex);
        return selectedDialogue;
    }

    private void ResetDialogues()
    {
        dialoguesNotYetShown = new List<Dialogue>(_startDialogues);
    }
}
