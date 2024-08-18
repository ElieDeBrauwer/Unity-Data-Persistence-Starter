using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;


public class ScoreAndDataManager : MonoBehaviour
{

    public static ScoreAndDataManager Instance;
    public string PlayerName;
    public int TopScore;
    public string TopScoreName;
    private TMP_InputField txtInput;

    private void Awake()
    {
        LoadSaveData();
        txtInput = GameObject.FindGameObjectWithTag("input_name").GetComponent<TMP_InputField>();
        txtInput.text = PlayerName;
        updateTopScoreView();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void updateTopScoreView()
    {
        TMP_Text txtTopScore = GameObject.FindGameObjectWithTag("top_score").GetComponent<TMP_Text>();
        txtTopScore.text = "Top Score: " + TopScore + " by " + TopScoreName;
    }

    public void pushTopScore(int score)
    {
        if  (score > TopScore)
        {
            TopScore = score;
            TopScoreName = PlayerName;
            SaveSaveData();
        }
    }
    public void StartNew()
    {
        SceneManager.LoadScene("main");
    }

    public void Exit()
    {
        SaveSaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }



    [System.Serializable]
    class SaveData
    {
        public int TopScore;
        public string TopScoreName;
        public string PlayerName;
    }


    public void SaveName()
    {
        Debug.Log(txtInput.text);
        name = txtInput.text;
        PlayerName = name;
        SaveSaveData();
        
    }

    public void SaveSaveData()
    {
        string savePath = Application.persistentDataPath + "saveFile.json";
        SaveData data = new SaveData();
        data.PlayerName = PlayerName;
        data.TopScore = TopScore;
        data.TopScoreName = TopScoreName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadSaveData()
    {
        string savePath = Application.persistentDataPath + "saveFile.json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            PlayerName = data.PlayerName;
            TopScore = data.TopScore;
            TopScoreName = data.TopScoreName;
        }
    }
}
