using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region 1. Panel System
    [Header("Panels")]
    public GameObject InGamePanel;
    public GameObject PausePanel;
    
    private void Start()
    {
        InitializePanels();
        SetupButtons();
    }
    
    private void InitializePanels()
    {
        InGamePanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
        InGamePanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        InGamePanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    #endregion

    #region 2. Button System
    [Header("Buttons")]
    public Button PauseBtn;
    public Button ResumeBtn;
    public Button MenuBtn;
    
    [Header("Scene")]
    public int menuSceneIndex = 0;
    
    private void SetupButtons()
    {
        PauseBtn.onClick.AddListener(PauseGame);
        ResumeBtn.onClick.AddListener(ResumeGame);
        MenuBtn.onClick.AddListener(GoToMenu);
        
        SetupAudioSystem();
    }
    
    private void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneIndex);
    }
    #endregion

    #region 3. Audio System
    [Header("Audio Settings")]
    public bool playHoverSound = true;
    public bool playClickSound = true;
    
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    [Header("Audio Sources")]
    public AudioSource audioSource;
    public AudioSource musicAudioSource;
    
    private bool sfxEnabled;
    private bool musicEnabled;
    
    private void SetupAudioSystem()
    {
        LoadAudioSettings();
        AddClickSounds();
        AddHoverEvents();
    }
    
    private void LoadAudioSettings()
    {
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        UpdateMusicState();
    }
    
    private void UpdateMusicState()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.mute = !musicEnabled;
        }
    }
    
    private void AddClickSounds()
    {
        PauseBtn.onClick.RemoveAllListeners();
        ResumeBtn.onClick.RemoveAllListeners();
        MenuBtn.onClick.RemoveAllListeners();
        
        PauseBtn.onClick.AddListener(() => { PlayClickSound(); PauseGame(); });
        ResumeBtn.onClick.AddListener(() => { PlayClickSound(); ResumeGame(); });
        MenuBtn.onClick.AddListener(() => { PlayClickSound(); GoToMenu(); });
    }
    
    private void AddHoverEvents()
    {
        AddHoverEventToButton(PauseBtn);
        AddHoverEventToButton(ResumeBtn);
        AddHoverEventToButton(MenuBtn);
    }
    
    private void AddHoverEventToButton(Button button)
    {
        var eventTrigger = button.gameObject.GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        }
        
        var entry = new UnityEngine.EventSystems.EventTrigger.Entry();
        entry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { PlayHoverSound(); });
        eventTrigger.triggers.Add(entry);
    }
    
    private void PlayHoverSound()
    {
        if (playHoverSound && sfxEnabled && hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
    
    private void PlayClickSound()
    {
        if (playClickSound && sfxEnabled && clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
    #endregion
}