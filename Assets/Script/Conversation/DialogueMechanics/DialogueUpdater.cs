using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUpdater : MonoBehaviour
{
    [SerializeField] private Text _outcomeLine;
    [SerializeField] private Text _variant1Line;
    [SerializeField] private Text _variant2Line;
    [SerializeField] private Text _variant3Line;
    [SerializeField] private Text _returnOption;
    [SerializeField] private float _typingSpeed;
    [SerializeField] private ConversationManager _conversationManager;
    List<AudioSource> _soudns = new List<AudioSource>();
    private Coroutine _typeTextCoroutine;


    public event Action<Dialogue> DialogueTriggered = delegate { };

    private void Start()
    {
        _conversationManager.ConversationFinished += StopTyping;
        _soudns.Add(SoundManager.instance.FindSound("type_0"));
        _soudns.Add(SoundManager.instance.FindSound("type_1"));
        _soudns.Add(SoundManager.instance.FindSound("type_2"));
        _soudns.Add(SoundManager.instance.FindSound("type_3"));
        _soudns.Add(SoundManager.instance.FindSound("type_4"));
        _soudns.Add(SoundManager.instance.FindSound("type_5"));
        _soudns.Add(SoundManager.instance.FindSound("type_6"));
        _soudns.Add(SoundManager.instance.FindSound("type_7"));
    }

    private void StopTyping()
    {
        if (_typeTextCoroutine != null)
        {
            StopCoroutine(_typeTextCoroutine);
            _typeTextCoroutine = null;
        }
    }

    public void UpdateNextDialogue(Dialogue nextDialogue)
    {
        DialogueTriggered.Invoke(nextDialogue);

        if (nextDialogue.Variant1Dialogue == null)
        {
            _variant1Line.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _variant1Line.transform.parent.gameObject.SetActive(true);
            _variant1Line.text = nextDialogue.Variant1TextLocalized;
        }

        if (nextDialogue.Variant2Dialogue == null)
        {
            _variant2Line.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _variant2Line.transform.parent.gameObject.SetActive(true);
            _variant2Line.text = nextDialogue.Variant2TextLocalized;
        }

        if (nextDialogue.Variant3Dialogue == null)
        {
            _variant3Line.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _variant3Line.transform.parent.gameObject.SetActive(true);
            _variant3Line.text = nextDialogue.Variant3TextLocalized;
        }

        if (nextDialogue.ReturnDisabled)
        {
            _returnOption.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _returnOption.transform.parent.gameObject.SetActive(true);
        }



        StopTyping();
        TypeOutcome(nextDialogue);
    }

    private void TypeOutcome(Dialogue nextDialogue)
    {
        string textToShow = nextDialogue.OutcomeString;

        _typeTextCoroutine = StartCoroutine(TypeText(textToShow));
    }

    private IEnumerator TypeText(string textToShow)
    {


        StringBuilder outcomyy = new StringBuilder();
        int indexer = 0;

        foreach (char letter in textToShow)
        {
            outcomyy.Append(letter);

            _outcomeLine.text = outcomyy.ToString();

            if (indexer % 3f == 0)
            {
                int soundIndex = UnityEngine.Random.Range(0, _soudns.Count);

                _soudns[soundIndex].Play();
            }
            indexer++;
            yield return new WaitForSeconds(_typingSpeed);
        }

        _typeTextCoroutine = null;

    }

}
