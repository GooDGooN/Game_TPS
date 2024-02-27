using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    protected PlayerControl player;
    protected UIDialogControl dialogControl;
    protected float interactDistance = 3.0f;
    public List<string> MyDialog = new List<string>();
    public string NPCName = string.Empty;
    public Coroutine LeaveCoroutine = null;
    public SkinnedMeshRenderer[] MySkinnedMesh;

    public Animator MyAnimator { get => myAnimator; }
    protected Animator myAnimator
    {
        get
        {
            var comp = GetComponent<Animator>();
            if (comp == null)
            {
                comp = GetComponentInChildren<Animator>();
            }
            return comp;
        }
    }
    public CapsuleCollider MyCapsuleCollider { get => myCapsuleCollider; }
    protected CapsuleCollider myCapsuleCollider
    {
        get
        {
            var comp = GetComponent<CapsuleCollider>();
            if (comp == null)
            {
                comp = GetComponentInChildren<CapsuleCollider>();
            }
            return comp;
        }
    }

    protected virtual void Start()
    {
        dialogControl = UIDialogControl.Instance;
        player = FindObjectOfType<PlayerControl>();
    }

    public string this[int i]
    {
        get
        {
            if(i < MyDialog.Count)
            {
                return MyDialog[i];
            }
            else
            {
                return MyDialog[MyDialog.Count - 1];
            }
        }
    }

    protected virtual void Update()
    {
        if (dialogControl.TargetConversationTarget != this || dialogControl.InConversation)
        {
            foreach(var mesh in MySkinnedMesh)
            {
                var material = mesh.materials.Where(i => i.name == "OutlineMaterial (Instance)").ToArray();
                if(material.Length > 0)
                {
                    material[0].SetFloat("_OutlineThinkness", 0.0f);
                }
            }
        }
        else
        {
            foreach (var mesh in MySkinnedMesh)
            {
                var material = mesh.materials.Where(i => i.name == "OutlineMaterial (Instance)").ToArray();
                if (material.Length > 0)
                {
                    material[0].SetFloat("_OutlineThinkness", 0.025f);
                }
            }
        }
    }

    protected Vector3 LookPlayerSlowly(Transform targetTransform)
    {
        var targetnormal = (player.transform.position - targetTransform.position).normalized;
        var dot = Vector3.Dot(targetTransform.forward, targetnormal);
        if (1.0f - dot > 0.05f)
        {
            var rot = Quaternion.LookRotation(player.transform.position - targetTransform.position).eulerAngles;
            var result = rot - targetTransform.eulerAngles;

            result.x = Mathf.Abs(result.x) > 180.0f ? -(result.x % 180.0f) : result.x;
            result.y = Mathf.Abs(result.y) > 180.0f ? -(result.y % 180.0f) : result.y;
            result.z = Mathf.Abs(result.z) > 180.0f ? -(result.z % 180.0f) : result.z;
            targetTransform.eulerAngles += result.normalized * 250.0f * Time.deltaTime;
        }
        else
        {
            targetTransform.eulerAngles = Quaternion.LookRotation(player.transform.position - targetTransform.position).eulerAngles;
        }
        return targetTransform.eulerAngles;
    }

    public virtual void EndDialog() { }
}
