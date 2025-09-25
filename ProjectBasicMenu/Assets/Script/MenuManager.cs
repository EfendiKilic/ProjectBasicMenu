using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainPanel;
    public GameObject SettingPanel;
    [Space(5)]
    
    [Header("Buttons")]
    public Button playButton;
    public Button openSettingButton;
    public Button closeSettingButton;
    public Button quitButton;
    [Space(5)]
    
    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle effectToggle;
    [Space(5)] 
    
    [Header("Scene")]
    public int gameSceneIndex;
    
    private void Start()
    {
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
        
        playButton.onClick.AddListener(PlayGame);
        openSettingButton.onClick.AddListener(OpenSettings);
        closeSettingButton.onClick.AddListener(CloseSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    private void OpenSettings()
    {
        MainPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }
    
    private void CloseSettings()
    {
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
    
}
