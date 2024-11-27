using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float offset = 20;

    private GameObject canvasGO;

    public Character character;
    private string currentText = "";
    public TextMeshProUGUI stateTextTemplate;
    private GameObject templateGO;
    private RectTransform[] stateRects = new RectTransform[10];
    private RectTransform[] rectTransforms = new RectTransform[10];
    private List<string> statesEntered = new();

    private void Awake()
    {
        character.stateEntered.AddListener(PushTextListener);

        canvasGO = stateTextTemplate.transform.parent.gameObject;
        templateGO = stateTextTemplate.gameObject;
        templateGO.SetActive(false);
        
    }

    private void Update()
    {
        if (statesEntered.Count > 0)
        {

        }
    }

    private void PushTextListener(string text)
    {
        Debug.Log("ahahah");
        statesEntered.Add(text);
    }

    private IEnumerator PushText(string text)
    {
        GameObject go = Instantiate(templateGO, templateGO.transform.position, Quaternion.identity);
        go.transform.SetParent(canvasGO.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = text;
        go.name = text;
        go.SetActive(true);
        yield return null;
    }
}
