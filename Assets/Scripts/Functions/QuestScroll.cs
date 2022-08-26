using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestScroll : MonoBehaviour
{
    List<QuestContainer> questsList = new List<QuestContainer>();
    public QuestContainer containerPrefab;
    public Transform content;

    public Transform maxpos;
    public Transform minpos;

    private void Awake()
    {
        StartCoroutine(WaitUntilQuestExist());
    }

    IEnumerator WaitUntilQuestExist()
    {
        while (QuestManager.Instance.Quests == null || QuestManager.Instance.Quests.Count <= 0)
        {
            yield return null;
        }

        foreach (var item in QuestManager.Instance.Quests)
        {
            QuestContainer temp = Instantiate(containerPrefab, content);
            temp.Init(item);
            questsList.Add(temp);
        }

        var quests = from quest in questsList
                     orderby quest.data.curProgress / quest.data.maxProgress descending
                     select quest;

        var questList = quests.ToList();
        var already = questsList.FindAll((item) => !item.data.isRewardAble);

        foreach (var item in already)
        {
            questList.Remove(item);
        }

        int idx = 0;
        foreach (var item in questList)
        {
            item.transform.SetSiblingIndex(idx);
            idx++;
        }

        foreach (var item in already)
        {
            item.transform.SetAsLastSibling();
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
