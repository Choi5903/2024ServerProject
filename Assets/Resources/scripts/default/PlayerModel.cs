using UnityEngine;
using System.Collections.Generic;

public class PlayerModel
{
    public string playerName;
    public int currentStage;  // 탐사 단계
    private Dictionary<int, StageData> stageData;

    // 두 개의 인자를 받는 생성자 추가
    public PlayerModel(string playerName, int currentStage)
    {
        this.playerName = playerName;
        this.currentStage = currentStage;
        this.stageData = new Dictionary<int, StageData>();

        // 각 단계 정보 초기화 (설명과 이미지 설정)
        for (int i = 1; i <= 20; i++)
        {
            stageData[i] = new StageData
            {
                name = "Stage " + i,
                description = GetStageDescription(i), // 각 단계의 설명을 반환하는 메서드 사용
                riskLevel = Random.Range(1, 21),
                progress = 0f,
                image = Resources.Load<Sprite>("StageImages/Stage" + i)  // 이미지 경로는 Resources 폴더에 위치해야 합니다.
            };
        }
    }

    // 각 단계에 대한 설명을 반환하는 메서드
    public string GetStageDescription(int stage)
    {
        switch (stage)
        {
            case 1: return "Stage 1: The beginning of your journey. You start with basic resources and must prepare for the challenges ahead.";
            case 2: return "Stage 2: Initial exploration. You encounter your first set of obstacles and make the first steps toward progress.";
            case 3: return "Stage 3: Expanding your horizons. New opportunities arise, but so do greater risks.";
            case 4: return "Stage 4: The first real challenge. You face a series of tough decisions that could define your path forward.";
            case 5: return "Stage 5: You encounter a powerful opponent. Victory here will propel you forward, but defeat could set you back.";
            case 6: return "Stage 6: The terrain grows more difficult. You must adapt your strategy to survive.";
            case 7: return "Stage 7: Midway point. Resources are running low, and every decision counts.";
            case 8: return "Stage 8: The risk of failure grows. Do you push forward, or take a step back to reassess?";
            case 9: return "Stage 9: A breakthrough. You discover new technology or allies to help you reach your goal.";
            case 10: return "Stage 10: The halfway point. You're starting to feel the weight of your journey, but you're also seeing the rewards of your efforts.";
            case 11: return "Stage 11: Major setback. An unexpected obstacle forces you to rethink your approach.";
            case 12: return "Stage 12: New discoveries. You encounter an ancient artifact or clue that changes the direction of your exploration.";
            case 13: return "Stage 13: The first major battle. Victory will bring great rewards, but defeat could cost you dearly.";
            case 14: return "Stage 14: The environment grows more hostile. Resources are scarce, and the stakes are higher than ever.";
            case 15: return "Stage 15: An unexpected ally. A new character joins you, bringing valuable skills or knowledge.";
            case 16: return "Stage 16: The final stretch. You're getting closer to your goal, but the challenges grow exponentially.";
            case 17: return "Stage 17: Almost there. You can see the end in sight, but the final obstacles will test your resolve.";
            case 18: return "Stage 18: Final preparations. Gather everything you need for the final push.";
            case 19: return "Stage 19: The final battle. Everything you've worked for is on the line.";
            case 20: return "Stage 20: The end of your journey. You've reached the final stage, but the challenges here are unlike any you've faced before.";
            default: return "Unknown Stage: This stage is not yet defined.";
        }
    }

    // 각 단계의 이름
    public string GetStageName(int stage)
    {
        return stageData[stage].name;
    }

    // 각 단계의 설명
    public string GetStageDescriptionByStage(int stage)
    {
        return stageData[stage].description;
    }

    // 각 단계의 이미지를 반환
    public Sprite GetStageImage(int stage)
    {
        return stageData[stage].image;
    }

    public int GetRiskLevel(int stage)
    {
        return stageData[stage].riskLevel;
    }

    public float GetProgress(int stage)
    {
        return stageData[stage].progress;
    }

    // StageData 클래스는 각 단계에 대한 정보를 담고 있습니다.
    public class StageData
    {
        public string name;
        public string description;
        public int riskLevel;
        public float progress;
        public Sprite image;
    }
}
