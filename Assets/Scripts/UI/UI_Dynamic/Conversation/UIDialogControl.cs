using CharacterNamespace;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIDialogControl : Singleton<UIDialogControl>
{
    #region UI
    public TMP_Text ChatText;
    public GameObject InteractText;
    public GameObject ChatBackground;
    public GameObject[] ToggleObjs; // crosshair, icons, status ...
    #endregion

    #region CONVERSATION
    private string currentText;
    private int stringIndex = 0;
    private int dialogNum = 0;
    public bool InConversation = false;

    public NPCBase CurrentConversationTarget;
    public List<NPCBase> NearNPCs = new List<NPCBase>();
    #endregion

    public bool StopConversation = false;
    private Coroutine currentCoroutine = null;

    private void Update()
    {
        if(!StopConversation)
        {
            InteractNPC();
        }
        ToggleSet();
    }

    private void ToggleSet()
    {
        if (InConversation)
        {
            ChatBackground.SetActive(true);
            foreach (GameObject obj in ToggleObjs)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in ToggleObjs)
            {
                obj.SetActive(true);
            }
            ChatBackground.SetActive(false);

            InteractText.SetActive(NearNPCs.Count > 0 && !StopConversation);
        }
    }

    private void InteractNPC()
    {
        if (InConversation)
        {
            if (GameSystem.GetKeyPressed(KeyInputs.Interact))
            {
                dialogNum++;
                if (dialogNum < CurrentConversationTarget.MyDialog.Count)
                {
                    SetText(CurrentConversationTarget[dialogNum]);
                }
                else if (dialogNum >= CurrentConversationTarget.MyDialog.Count)
                {
                    CurrentConversationTarget.EndDialog();
                    CurrentConversationTarget = null;
                    InConversation = false;
                    dialogNum = 0;
                    return;
                }
            }
            
            if(GameSystem.GetKeyPressed(KeyInputs.Escape))
            {
                CurrentConversationTarget = null;
                InConversation = false;
                dialogNum = 0;
                return;
            }
        }

        if (GameSystem.GetKeyPressed(KeyInputs.Interact) && !InConversation)
        {
            if (NearNPCs.Count > 0)
            {
                var targetNpc = NearNPCs[0];
                if (NearNPCs.Count > 1)
                {
                    var playerBodyTransform = PlayerControl.Instance.PlayerBody.transform;
                    for (int i = 1; i < NearNPCs.Count; i++)
                    {
                        var normal1 = targetNpc.transform.position - playerBodyTransform.position;
                        var normal2 = NearNPCs[i].transform.position - playerBodyTransform.position;
                        normal1.y = normal2.y = playerBodyTransform.position.y;

                        var targetNPCdotValue = Vector3.Dot(playerBodyTransform.forward, normal1.normalized);
                        var testNPCdotValue = Vector3.Dot(playerBodyTransform.forward, normal2.normalized);
                        var targetNPCDist = 1.0f - Vector3.Distance(playerBodyTransform.position.normalized, targetNpc.transform.position.normalized);
                        var testNPCDist = 1.0f - Vector3.Distance(playerBodyTransform.position.normalized, NearNPCs[i].transform.position.normalized);

                        if(targetNPCdotValue + targetNPCDist < testNPCdotValue + testNPCDist)
                        {
                            targetNpc = NearNPCs[i];
                        }
                    }
                }
                CurrentConversationTarget = targetNpc;
                InConversation = true;
                SetText(CurrentConversationTarget[dialogNum]);
            }
        }
    }

    private IEnumerator DrawingText()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (stringIndex < currentText.Length)
            {
                ChatText.text += currentText[stringIndex++];
            }
            else
            {
                break;
            }
        }
    }

    public void SetText(string text, bool isContinue = false)
    {
        Cleartext();
        currentCoroutine = StartCoroutine(DrawingText());
        stringIndex = 0;
        ChatText.text = isContinue ? currentText : "";
        currentText = text;
    }

    private void Cleartext()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = null;
    }
}
