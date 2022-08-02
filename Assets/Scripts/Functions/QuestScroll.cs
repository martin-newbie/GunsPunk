using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScroll : MonoBehaviour
{
    List<QuestContainer> questsList = new List<QuestContainer>();
    public QuestContainer containerPrefab;
    public Transform content;

    public Transform maxpos;
    public Transform minpos;

    private void Awake()
    {
        foreach (var item in QuestManager.Instance.Quests)
        {
            QuestContainer temp = Instantiate(containerPrefab, content);
            temp.Init(item);
            questsList.Add(temp);
        }

    }

    private void Start()
    {
        StartCoroutine(RemoveOrder());
    }

    IEnumerator RemoveOrder()
    {
        yield return null;
        content.GetComponent<ContentSizeFitter>().enabled = false;
        content.GetComponent<VerticalLayoutGroup>().enabled = false;
    }

    private void Update()
    {
        foreach (var item in questsList)
        {
            item.gameObject.SetActive(item.transform.position.y >= minpos.position.y && item.transform.position.y <= maxpos.position.y);
        }
    }
}
