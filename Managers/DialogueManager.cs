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
    private Dictionary<string, DialogueData> dialogueDictionary; // JSON에서 로드한 대화 데이터를 저장할 딕셔너리
    private Dictionary<(string questId, string requiredQuestState), List<DialogueData.Choice>> choiceIndex; // 퀘스트들의 정보를 저장할 딕셔너리

    private int currentLineIndex = 0;
    private int currentDialogueId = 1; // 현재 대화의 ID
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
            DontDestroyOnLoad(gameObject); // 게임 전체에서 유지되도록
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

            // 선택지 인덱싱 초기화
            choiceIndex = new Dictionary<(string, string), List<DialogueData.Choice>>();

            // 선택지 인덱싱 시작
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
            Debug.LogError("파일을 찾지 못함: " + filePath);
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
                    currentDialogueId = dialogueId; // 현재 대화의 ID 저장
                    characterNameText.text = currentDialogue.name;

                    string fullLine = dialogue.lines[currentLineIndex].line;
                    StopAllCoroutines();  // 이전에 실행 중이던 코루틴이 있으면 중지
                    StartCoroutine(Typing(fullLine));  // 새로운 코루틴 시작

                    // 대화의 마지막 라인인 경우에만 선택지 UI 표시
                    if (currentLineIndex == dialogue.lines.Length - 1) // 마지막 라인인지 확인
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
                    Debug.LogError("유효하지 않은 라인 인덱스: " + lineIndex);
                }
            }
            else
            {
                Debug.LogError("해당 ID에 맞는 대화를 찾을 수 없음: " + dialogueId);
            }
        }
        else
        {
            Debug.LogError("해당 캐릭터 이름에 맞는 대화를 찾을 수 없음: " + characterName);
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
                DisplayDialogue(currentDialogue.name, currentDialogueId, currentLineIndex); // 현재 대화의 인덱스와 다음 라인 인덱스 사용
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
        choiceBox.SetActive(true);  // 선택지 UI 활성화
        nextIcon.gameObject.SetActive(false);

        // 기존 선택지 버튼을 모두 제거
        foreach (Transform child in choiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // 선택지 버튼 생성
        foreach (var choice in choices)
        {
            if (choice.isVisible) // isVisible이 true인 경우에만 버튼 활성화
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
        DisplayDialogue(currentDialogue.name, nextDialogueId); // 다음 대화 표시

        switch (action)
        {
            case "OpenShop":
                ShopManager.instance.ShowShop();
                break;
            case "StartQuest":
                if (questId != null)
                {
                    GameEventsManager.instance.questEvents.StartQuest(questId);
                    Debug.Log("퀘스트 시작: " + questId);

                    // 퀘스트 시작 선택지 숨김
                    SetChoiceVisibility(questId, "CAN_START", false);
                }
                break;
            case "FinishQuest":
                if (questId != null)
                {
                    currentQuestId = questId;
                    Debug.Log("퀘스트 완료: " + questId);

                    // 퀘스트 완료 선택지 숨김
                    SetChoiceVisibility(questId, "CAN_FINISH", false);
                }
                break;
            default:
                Debug.LogWarning("알 수 없는 액션: " + action);
                break;
        }

        HideChoices(); // 선택지 숨김
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
            interactions.DialougeEnd(); // 대화 종료 처리
        }

        if (currentQuestId != null)
        {
            GameEventsManager.instance.questEvents.FinishQuest(currentQuestId);
            Debug.Log("퀘스트 완료!");
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
            // 화살표를 처음 위치로 되돌림
            nextIcon.gameObject.SetActive(true);

            nextIcon.DOKill();

            // 애니메이션 시작 전에 화살표 위치를 초기 위치로 설정
            nextIcon.localPosition = nextIconInitialPosition;

            // 그 위치에서 Y축으로 -10만큼 이동
            nextIcon.DOLocalMoveY(nextIconInitialPosition.y - 10f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}