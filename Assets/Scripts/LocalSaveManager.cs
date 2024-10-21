using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Oniboogie;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LocalSaveManager : SingletonMono<LocalSaveManager> // 싱글톤
{
    [SerializeField]
    private LocalSaveData localSave;    // 로컬 세이브 데이터

    public LocalSaveData LocalSave { get => localSave; }
    public bool IsLocalDataLoadComplete { get; private set; }   // 첫 한번 로드가 끝났는지 여부    

    private string LocalSavePath
    {
        get
        {
            return Path.Combine(Application.dataPath, "LocalSave.json");
        }
    }

    private void Awake()
    {
        if (Instance == this)
            DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Save Local Data")]  // 컴포넌트를 우클릭 했을 때 메뉴에 추가
    public void SaveLocalData()
    {
        if(localSave == null)
            localSave = new LocalSaveData();

        // LocalSaveData타입을 JObject로 직렬화
        JObject jobj = JObject.FromObject(localSave);

        // 저장할 수 있게 string json으로 변환
        string json = JsonConvert.SerializeObject(jobj);
        // 파일 저장
        File.WriteAllText(LocalSavePath, json);
    }

    [ContextMenu("Load Local Data")]
    public void LoadLocalData()
    {
        // 저장 파일을 생성한 적 없거나 지워진 경우
        if(!File.Exists(LocalSavePath))
        {
            SaveLocalData();
            IsLocalDataLoadComplete = true;
            return;
        }

        var loadedJobj = JObject.Parse(File.ReadAllText(LocalSavePath));
        // 어떤 이유에서든 Jobject로 parse에 실패한 경우
        if(loadedJobj == null)
        {
            SaveLocalData();
            IsLocalDataLoadComplete = true;
            return;
        }

        // LocalSaveData 타입으로 역직렬화
        localSave = loadedJobj.ToObject<LocalSaveData>();
        IsLocalDataLoadComplete = true;
    }
}

#if UNITY_EDITOR
public static class LocalSaveManagerMenu
{    
    [MenuItem("Oniboogie/Save Local Data")]  // 에디터 최상단에 메뉴 아이템으로 추가
    public static void SaveLocalData()
    {
        LocalSaveManager.Instance.SaveLocalData();
    }

    [MenuItem("Oniboogie/Load Local Data")]
    public static void LoadLocalData()
    {
        LocalSaveManager.Instance.LoadLocalData();
    }
}
#endif
