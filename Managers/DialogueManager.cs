using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject talkBox;
    private Dictionary<string, DialogueData> dialogueDictionary; // JSON���� �ε��� ��ȭ �����͸� ������ ��ųʸ�
    private Dictionary<(string questId, string requiredQuestState), List<DialogueData.Choice>> choiceIndex; // ����Ʈ���� ������ ������ ��ųʸ�

    private int currentLineIndex = 0;
    private int currentDialogueId = 1; // ���� ��ȭ�� ID
    private DialogueData currentDialogue;

    public GameObject choiceBox;
    public GameObject choiceButtonPrefab;

    public GameObject ShopCanvas;

    public RectTransform nextIcon;
    private Vector3 nextIconInitialPosition;

    private string currentQuestId = null;

    private bool isChoicesActive = false;
    private bool isTyping = false;
    private bool isDialogueComplete = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ��ü���� �����ǵ���
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadDialogue();
        nextIconInitialPosition = nextIcon.localPosition;
    }

    void LoadDialogue()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "dialogue.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            dialogueDictionary = JsonConvert.DeserializeObject<Dictionary<string, DialogueData>>(json);

            // ������ �ε��� �ʱ�ȭ
            choiceIndex = new Dictionary<(string, string), List<DialogueData.Choice>>();

            // ������ �ε��� ����
            foreach (var characterDialogue in dialogueDictionary.Values)
            {
                foreach (var dialogue in characterDialogue.dialogues.Values)
                {
                    if (dialogue.choices != null)
                    {
                        foreach (var choice in dialogue.choices)
                        {
                            if (!string.IsNullOrEmpty(choice.questId) && !string.IsNullOrEmpty(choice.requiredQuestState))
                            {
                                var key = (choice.questId, choice.requiredQuestState);
                                if (!choiceIndex.ContainsKey(key))
                                {
                                    choiceIndex[key] = new List<DialogueData.Choice>();
                                }
                                choiceIndex[key].Add(choice);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("������ ã�� ����: " + filePath);
        }
    }

    public void DisplayDialogue(string characterName, int dialogueId = 1, int lineIndex = 0)
    {
        if (dialogueDictionary.TryGetValue(characterName, out currentDialogue))
        {
            if (currentDialogue.dialogues.TryGetValue(dialogueId.ToString(), out DialogueData.Dialogue dialogue))
            {
                if (lineIndex >= 0 && lineIndex < dialogue.lines.Length)
                {
                    talkBox.gameObject.SetActive(true);
                    currentLineIndex = lineIndex;
                    currentDialogueId = dialogueId; // ���� ��ȭ�� ID ����
                    characterNameText.text = currentDialogue.name;

                    string fullLine = dialogue.lines[currentLineIndex].line;
                    StopAllCoroutines();  // ������ ���� ���̴� �ڷ�ƾ�� ������ ����
                    StartCoroutine(Typing(fullLine));  // ���ο� �ڷ�ƾ ����

                    // ��ȭ�� ������ ������ ��쿡�� ������ UI ǥ��
                    if (currentLineIndex == dialogue.lines.Length - 1) // ������ �������� Ȯ��
                    {
                        if (dialogue.choices != null && dialogue.choices.Length > 0)
                        {
                            isChoicesActive = true;
                            ShowChoices(dialogue.choices);
                        }
                        else
                        {
                            isChoicesActive = false;
                            HideChoices();
                        }
                    }
                }
                else
                {
                    Debug.LogError("��ȿ���� ���� ���� �ε���: " + lineIndex);
                }
            }
            else
            {
                Debug.LogError("�ش� ID�� �´� ��ȭ�� ã�� �� ����: " + dialogueId);
            }
        }
        else
        {
            Debug.LogError("�ش� ĳ���� �̸��� �´� ��ȭ�� ã�� �� ����: " + characterName);
        }
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            return;
        } 
        else if (isDialogueComplete)
        {
            nextIcon.gameObject.SetActive(false);

            if (currentDialogue != null && currentLineIndex + 1 < currentDialogue.dialogues[currentDialogueId.ToString()].lines.Length)
            {
                currentLineIndex++;
                DisplayDialogue(currentDialogue.name, currentDialogueId, currentLineIndex); // ���� ��ȭ�� �ε����� ���� ���� �ε��� ���
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void EnableQuestChoice(string questId, QuestState questState)
    {
        var key = (questId, questState.ToString());
        if (choiceIndex.TryGetValue(key, out var choices))
        {
            foreach (var choice in choices)
            {
                choice.isVisible = true;
            }
        }
    }

    public void ShowChoices(DialogueData.Choice[] choices)
    {
        choiceBox.SetActive(true);  // ������ UI Ȱ��ȭ
        nextIcon.gameObject.SetActive(false);

        // ���� ������ ��ư�� ��� ����
        foreach (Transform child in choiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // ������ ��ư ����
        foreach (var choice in choices)
        {
            if (choice.isVisible) // isVisible�� true�� ��쿡�� ��ư Ȱ��ȭ
            {
                CreateChoiceButton(choice);
            }
        }
    }

    private void CreateChoiceButton(DialogueData.Choice choice)
    {
        Button choiceButton = Instantiate(choiceButtonPrefab, choiceBox.transform).GetComponent<Button>();

        choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

        choiceButton.onClick.RemoveAllListeners();
        choiceButton.onClick.AddListener(() => HandleChoice(choice.nextDialogueId, choice.action, choice.questId));
    }

    private void HideChoices()
    {
        choiceBox.SetActive(false);
    }

    private void HandleChoice(int nextDialogueId, string action, string questId = null)
    {
        DisplayDialogue(currentDialogue.name, nextDialogueId); // ���� ��ȭ ǥ��

        switch (action)
        {
            case "OpenShop":
                ShopManager.instance.ShowShop();
                break;
            case "StartQuest":
                if (questId != null)
                {
                    GameEventsManager.instance.questEvents.StartQuest(questId);
                    Debug.Log("����Ʈ ����: " + questId);

                    // ����Ʈ ���� ������ ����
                    SetChoiceVisibility(questId, "CAN_START", false);
                }
                break;
            case "FinishQuest":
                if (questId != null)
                {
                    currentQuestId = questId;
                    Debug.Log("����Ʈ �Ϸ�: " + questId);

                    // ����Ʈ �Ϸ� ������ ����
                    SetChoiceVisibility(questId, "CAN_FINISH", false);
                }
                break;
            default:
                Debug.LogWarning("�� �� ���� �׼�: " + action);
                break;
        }

        HideChoices(); // ������ ����
    }

    private void SetChoiceVisibility(string questId, string requiredQuestState, bool isVisible)
    {
        var key = (questId, requiredQuestState);
        if (choiceIndex.TryGetValue(key, out var choices))
        {
            foreach (var choice in choices)
            {
                choice.isVisible = isVisible;
            }
        }
    }

    private void EndDialogue()
    {
        if (isChoicesActive) return;

        characterNameText.text = "";
        dialogueText.text = "";
        Interactions interactions = FindObjectOfType<Interactions>();
        if (interactions != null)
        {
            interactions.DialougeEnd(); // ��ȭ ���� ó��
        }

        if (currentQuestId != null)
        {
            GameEventsManager.instance.questEvents.FinishQuest(currentQuestId);
            Debug.Log("����Ʈ �Ϸ�!");
            currentQuestId = null;
        }

        talkBox.gameObject.SetActive(false);
    }

   public void TMPDOText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }

    IEnumerator Typing(string dialogueLine)
    {
        isTyping = true;
        isDialogueComplete = false;
        nextIcon.gameObject.SetActive(false);
        dialogueText.text = dialogueLine;

        AudioManager.instance.PlaySFX("TypingSound");

        TMPDOText(dialogueText, 0.5f);

        yield return new WaitForSeconds(1f);

        AudioManager.instance.StopSFX("TypingSound");

        isTyping = false;
        isDialogueComplete = true;

        if (!choiceBox.activeSelf)
        {
            // ȭ��ǥ�� ó�� ��ġ�� �ǵ���
            nextIcon.gameObject.SetActive(true);

            nextIcon.DOKill();

            // �ִϸ��̼� ���� ���� ȭ��ǥ ��ġ�� �ʱ� ��ġ�� ����
            nextIcon.localPosition = nextIconInitialPosition;

            // �� ��ġ���� Y������ -10��ŭ �̵�
            nextIcon.DOLocalMoveY(nextIconInitialPosition.y - 10f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}