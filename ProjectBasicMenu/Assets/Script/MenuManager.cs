using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainPanel;
    public GameObject SettingPanel;
    
    [Header("Buttons")]
    public Button playButton;
    public Button openSettingButton;
    public Button closeSettingButton;
    public Button quitButton;
    
    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle effectToggle;
    
    [Header("Scene")]
    public int gameSceneIndex;
    
    [Header("Audio Settings")]
    public bool playHoverSound = true;
    public bool playClickSound = true;
    
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    [Header("Audio Source")]
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    
    private void Start()
    {
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
        
        LoadSettings();
        
        playButton.onClick.AddListener(() => { PlayClickSound(); PlayGame(); });
        openSettingButton.onClick.AddListener(() => { PlayClickSound(); OpenSettings(); });
        closeSettingButton.onClick.AddListener(() => { PlayClickSound(); CloseSettings(); });
        quitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });
        
        musicToggle.onValueChanged.AddListener((bool value) => { PlayClickSound(); OnMusicToggleChanged(value); });
        effectToggle.onValueChanged.AddListener((bool value) => { PlayClickSound(); OnSFXToggleChanged(value); });
        
        AddHoverEvents();
    }
    
    private void LoadSettings()
    {
        bool sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        
        effectToggle.isOn = sfxEnabled;
        musicToggle.isOn = musicEnabled;
        
        UpdateAudioSourcesMute();
    }
    
    private void OnMusicToggleChanged(bool value)
    {
        PlayerPrefs.SetInt("MusicEnabled", value ? 1 : 0);
        PlayerPrefs.Save();
        UpdateAudioSourcesMute();
    }
    
    private void OnSFXToggleChanged(bool value)
    {
        PlayerPrefs.SetInt("SFXEnabled", value ? 1 : 0);
        PlayerPrefs.Save();
        UpdateAudioSourcesMute();
    }
    
    private void UpdateAudioSourcesMute()
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.mute = !effectToggle.isOn;
        }
        
        if (musicAudioSource != null)
        {
            musicAudioSource.mute = !musicToggle.isOn;
        }
    }
    
    private void AddHoverEvents()
    {
        AddHoverEventToButton(playButton);
        AddHoverEventToButton(openSettingButton);
        AddHoverEventToButton(closeSettingButton);
        AddHoverEventToButton(quitButton);
        
        AddHoverEventToToggle(musicToggle);
        AddHoverEventToToggle(effectToggle);
    }
    
    private void AddHoverEventToButton(Button button)
    {
        if (button == null) return;
        
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }
        
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { PlayHoverSound(); });
        trigger.triggers.Add(pointerEnter);
    }
    
    private void AddHoverEventToToggle(Toggle toggle)
    {
        if (toggle == null) return;
        
        EventTrigger trigger = toggle.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = toggle.gameObject.AddComponent<EventTrigger>();
        }
        
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { PlayHoverSound(); });
        trigger.triggers.Add(pointerEnter);
    }
    
    private void PlayHoverSound()
    {
        if (playHoverSound && hoverSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(hoverSound);
        }
    }
    
    private void PlayClickSound()
    {
        if (playClickSound && clickSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clickSound);
        }
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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}