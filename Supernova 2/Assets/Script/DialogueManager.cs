using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public void Awake() => instance = this;

    private List<StartDialogOnTrigger> dialogueTriggers = new List<StartDialogOnTrigger>();

    public void AddDialogueTrigger(StartDialogOnTrigger dialogueTriggerToAdd)
    {
        dialogueTriggers.Add(dialogueTriggerToAdd);
    }

    public bool IsFreezingDialogueOpen()
    {
        bool frozen = false;
        for(int i = 0; i < dialogueTriggers.Count; i++)
        {
            if (dialogueTriggers[i].IsFreezingAndOpen()) frozen = true;
        }
        return frozen;
    }
}