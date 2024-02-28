using CharacterNamespace;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Core;
using Unity.VisualScripting;
using UnityEngine;

public class BuffItem : MonoBehaviour
{
    public ItemType MyType;
    private MeshRenderer myMeshRenderer;
    private Coroutine startCoroutine = null;
    private Vector3 startPosition;
    private ItemAudioSource itemAudioSource;
    private Coroutine destroyCoroutine;

    [SerializeField] private GameObject itemAudioSourcePrefab;

    private void OnEnable()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        itemAudioSource = Instantiate(itemAudioSourcePrefab, transform).GetComponent<ItemAudioSource>();
        startPosition = transform.position;
    }
    private void Update()
    {
        if (GameManager.IsGameStart && startCoroutine == null)
        {
            StartCoroutine(Floating());
            startCoroutine = StartCoroutine(LifeTimer());
        }
    }

    private IEnumerator Floating()
    {
        var dir = Vector3.up;
        var time = 0.0f;
        while (true)
        {
            transform.position += dir * Time.deltaTime * 0.5f;
            transform.Rotate(Vector3.up, Time.deltaTime * 100.0f);
            time += Time.deltaTime;
            if(time > 1.0f)
            {
                time = 0.0f;
                dir = -dir;
            }
            yield return null;
        }
    }

    private IEnumerator LifeTimer()
    {
        var time = 0.0f;

        while(time < 20.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        var myMats = myMeshRenderer.materials;
        Color[] colors = new Color[myMats.Length];
        for(int i = 0; i < myMats.Length; i++)
        {
            colors[i] = myMats[i].color;
        }
        
        while(true)
        {
            for(int i = 0; i < colors.Length; i++)
            {
                var delta = Mathf.Clamp(colors[i].a / 50.0f, 0.005f, 0.1f);
                colors[i].a -= delta;
                myMeshRenderer.materials[i].color = colors[i];
            }

            if (colors[0].a < 0.01f)
            {
                break;
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(PlayerControl.Instance.gameObject == other.gameObject)
        {
            if(destroyCoroutine == null)
            {
                GetComponent<MeshRenderer>().enabled = false;
                PlayerControl.Instance.GetBuff(MyType);
                itemAudioSource.PlaySound(SoundType.ItemPickUp);
                destroyCoroutine = StartCoroutine(DestroyDelay());
            }
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
