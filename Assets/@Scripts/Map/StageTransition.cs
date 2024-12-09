using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StageTransition : MonoBehaviour
{
    public List<Stage> Stages = new List<Stage>();
    public Stage CurrentStage { get; set; }
    public int CurrentStageIndex { get; set; } = -1;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Stages.Count > 0)
            return;

        foreach (Transform child in transform)
        {
            Stage stage = child.GetComponent<Stage>();
            if (stage != null)
                Stages.Add(stage);
        }
    }

    public void SetInfo()
    {
        int currentMapIndex = 0;

        for (int i = 0; i < Stages.Count; i++)
        {
            Stages[i].SetInfo(i);
            currentMapIndex = i;
        }

        OnMapChanged(currentMapIndex);
    }

    public void CheckMapChanged(Vector3 position)   // 플레이어가 현재 스테이지를 벗어난 경우 스테이지 전환을 처리
    {
        if (CurrentStage.IsPointInStage(position) == false)
        {
            int stageIndex = GetStageIndex(position);
            if (stageIndex != -1)
                OnMapChanged(stageIndex);
        }
    }

    int GetStageIndex(Vector3 position)
    {
        for (int i = 0; i < Stages.Count; i++)
        {
            if (Stages[i].IsPointInStage(position))
                return i;
        }

        return -1;
    }

    public void OnMapChanged(int newMapIndex)
    {
        CurrentStageIndex = newMapIndex;
        CurrentStage = Stages[CurrentStageIndex];
        if (CurrentStage.transform.name == "03_dungeon")
            Managers.Sound.Play(ESound.Bgm, "BGM_boss");

        LoadMapsAround(newMapIndex);
        UnloadOtherMaps(newMapIndex);
    }

    void LoadMapsAround(int mapIndex)   // 현재 스테이지와 인접한 스테이지를 로드
    {
        // 이전, 현재, 다음 맵을 로드
        for (int i = mapIndex - 1; i <= mapIndex + 1; i++)
        {
            if (i > -1 && i < Stages.Count)
            {
                Debug.Log($"{i} Stage Load -> {Stages[i].name}");
                Stages[i].LoadStage();
            }
        }
    }

    void UnloadOtherMaps(int mapIndex)  // 현재 스테이지와 인접하지 않은 스테이지를 언로드
    {
        for (int i = 0; i < Stages.Count; i++)
        {
            if (i < mapIndex - 1 || i > mapIndex + 1)
            {
                Debug.Log($"{i} Stage UnLoad -> {Stages[i].name}");
                Stages[i].UnLoadStage();
            }
        }
    }
}
