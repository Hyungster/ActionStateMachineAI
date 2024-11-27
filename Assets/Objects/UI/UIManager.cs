using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float scrollSpeed = 1;
    public float verticalOffset = 1;
    private int flip = 1;
    public float offset = 0.5f;

    private GameObject canvasGO;

    public TextMeshProUGUI templateStamp;
    public GameObject stampHolder;
    private RectTransform stampHolderTransform;
    public RectTransform stampStartPosition;

    public GameObject timeLine;

    public Character character;
    

    private void Awake()
    {
        character.stateEntered.AddListener(StampText);

        canvasGO = transform.GetChild(0).gameObject;

        templateStamp.gameObject.SetActive(false);
        stampHolderTransform = stampHolder.GetComponent<RectTransform>();
        //StartCoroutine(TimeLineCoroutine());
    }

    private void Update()
    {
        stampHolderTransform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0);
    }

    private void StampText(string text)
    {
        GameObject stamp = Instantiate(templateStamp.gameObject, stampHolder.transform);
        stamp.GetComponent<TextMeshProUGUI>().text = text;
        stamp.SetActive(true);
        stamp.transform.position = stampStartPosition.position;
        stamp.transform.position += new Vector3(0, verticalOffset * flip, 0);
        if (text.Equals("Scan")) stamp.transform.position -= new Vector3(this.offset, 0, 0);
        flip *= -1;
        Transform stampLine = stamp.transform.GetChild(0).transform;
        Vector3 offset = new Vector3(stampLine.localPosition.x, stampLine.localPosition.y, 0);
        if (flip > 0)
        {
            offset.y = -offset.y;
        }
        stampLine.localPosition = offset;
    }

    private IEnumerator TimeLineCoroutine()
    {
        float time = Time.time;
        int currentBeat = 0;
        for (; ; )
        {
            float scaledTime = time / character.beatDuration;
            int calculatedBeat = Mathf.FloorToInt((float)scaledTime);
            if (calculatedBeat > currentBeat)
            {
                SpawnTimeLine();
                currentBeat = calculatedBeat;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }
    private void SpawnTimeLine()
    {
        GameObject timeLineClone = Instantiate(timeLine, stampHolderTransform);
        timeLineClone.transform.position = stampStartPosition.position;
    }
}
