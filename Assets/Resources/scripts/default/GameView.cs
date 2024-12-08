using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하려면 이 네임스페이스가 필요합니다.
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    // UI 요소들 (TextMeshProUGUI로 변경)
    public TMP_Text playerNameText;    // TextMeshPro로 변경
    public TMP_Text stageNameText;     // TextMeshPro로 변경
    public TMP_Text stageDescriptionText; // TextMeshPro로 변경
    public Image stageImage;
    public TMP_Text explorationLogText; // TextMeshPro로 변경
    public TMP_Text nextAreaText;      // TextMeshPro로 변경
    public Slider progressSlider;

    public Button exploreButton;

    // UI 업데이트 메서드
    public void UpdatePlayerInfo(string playerName, string stageName, Sprite stageSprite, string description, string logText, string nextArea, int currentStage)
    {
        playerNameText.text = playerName;
        stageNameText.text = stageName;
        stageDescriptionText.text = description;

        // 단계에 맞는 이미지를 리소스 폴더에서 불러오기
        string imagePath = "StageImages/Stage" + currentStage; // "StageImages"는 Resources 폴더 내에 저장된 폴더 이름
        stageSprite = Resources.Load<Sprite>(imagePath); // 해당 경로에서 이미지 불러오기

        // 이미지가 존재할 경우에만 설정
        if (stageSprite != null)
        {
            stageImage.sprite = stageSprite;
        }
        else
        {
            Debug.LogWarning($"Stage image not found for stage {currentStage}");
        }

        explorationLogText.text = logText;
        nextAreaText.text = nextArea;
    }

    public void SetProgress(float progress)
    {
        progressSlider.value = progress;
    }

    // 탐사 버튼 클릭 리스너 설정
    public void SetExploreButtonListener(UnityEngine.Events.UnityAction action)
    {
        exploreButton.onClick.AddListener(action);
    }
}
