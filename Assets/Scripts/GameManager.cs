using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : Singleton<GameManager>
{
    //==============================================================================================================
    #region PUBLIC
    //==============================================================================================================

    /// <summary>
    /// All data stored by the game.
    /// </summary>
    public ScS_GameManager gameData;

    /// <summary>
    /// Input of the game.
    /// </summary>
    public Cursor_Controller cursorInputActions;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region REFERENCE
    //==============================================================================================================

    [SerializeField] private FMODUnity.EventReference onClickAudio;
    [SerializeField] private FMODUnity.EventReference onMouseOverAudio;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE
    //==============================================================================================================

    private string path_Cursor_Normal = "Template/Cursor/Normal_Cursor";
    private string path_Cursor_Click = "Template/Cursor/Click_Cursor";

    private Vector2 offset_Cursor = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private Texture2D cursor_Normal;
    private Texture2D cursor_Click;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region MONOBEHAVIOUR
    //==============================================================================================================

    private void Awake()
    {
        // set gameData reference.
        gameData = ScS_GameManager.Instance;
        cursorInputActions = new();
        cursorInputActions.Enable();
        LoadSettings();
        SetInputAction();
        GetCursorTexture();

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PUBLIC FONCTION
    //==============================================================================================================

    public void OnMouseOverAudio() { FMODUnity.RuntimeManager.PlayOneShot(onMouseOverAudio); }

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE FONCTION
    //==============================================================================================================

    /// <summary>
    /// Get cursor texture.
    /// </summary>
    private void GetCursorTexture()
    {
        cursor_Normal = Resources.Load<Texture2D>(path_Cursor_Normal);
        cursor_Click = Resources.Load<Texture2D>(path_Cursor_Click);

        Cursor.SetCursor(cursor_Normal, offset_Cursor, cursorMode);
    }
    /// <summary>
    /// Set input action to their action.
    /// </summary>
    private void SetInputAction()
    {
        cursorInputActions.Cursor.CursorClick.started += ctx => OnClickStarted();
        cursorInputActions.Cursor.CursorClick.canceled += ctx => OnClickCanceled();
    }
    private void OnClickStarted() { if (Cursor.visible) Cursor.SetCursor(cursor_Click, offset_Cursor, cursorMode); FMODUnity.RuntimeManager.PlayOneShot(onClickAudio); }
    private void OnClickCanceled() { if (Cursor.visible) Cursor.SetCursor(cursor_Normal, offset_Cursor, cursorMode); }

    /// <summary>
    /// Load settings from player pref.
    /// </summary>
    private void LoadSettings()
    {
        if (gameData.gameSettingsParameters.resolution == null) gameData.gameSettingsParameters = new();

        int _fullscreen = PlayerPrefs.GetInt("Fullscreen");
        if (_fullscreen == 1) gameData.gameSettingsParameters.fullscreen = true;
        else gameData.gameSettingsParameters.fullscreen = false;

        gameData.gameSettingsParameters.resolution = PlayerPrefs.GetString("Resolution");

        int _masterVolumeIsOff = PlayerPrefs.GetInt("MasterVolumeIsOff");
        if (_masterVolumeIsOff == 1) gameData.gameSettingsParameters.masterVolumeIsOff = true;
        else gameData.gameSettingsParameters.masterVolumeIsOff = false;
        gameData.gameSettingsParameters.masterVolume = PlayerPrefs.GetFloat("MasterVolume");

        int _musicVolumeIsOff = PlayerPrefs.GetInt("MusicVolumeIsOff");
        if (_musicVolumeIsOff == 1) gameData.gameSettingsParameters.musicVolumeIsOff = true;
        else gameData.gameSettingsParameters.musicVolumeIsOff = false;
        gameData.gameSettingsParameters.musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        int _soundVolumeIsOff = PlayerPrefs.GetInt("SoundVolumeIsOff");
        if (_soundVolumeIsOff == 1) gameData.gameSettingsParameters.soundVolumeIsOff = true;
        else gameData.gameSettingsParameters.soundVolumeIsOff = false;
        gameData.gameSettingsParameters.soundVolume = PlayerPrefs.GetFloat("SoundVolume");
    }
    /// <summary>
    /// Save settings on player pref.
    /// </summary>
    private void SaveSettings()
    {
        if (gameData.gameSettingsParameters.fullscreen) PlayerPrefs.SetInt("Fullscreen", 1);
        else PlayerPrefs.SetInt("Fullscreen", 0);

        PlayerPrefs.SetString("Resolution", gameData.gameSettingsParameters.resolution);

        if (gameData.gameSettingsParameters.masterVolumeIsOff) PlayerPrefs.SetInt("MasterVolumeIsOff", 1);
        else PlayerPrefs.SetInt("MasterVolumeIsOff", 0);
        PlayerPrefs.SetFloat("MasterVolume", gameData.gameSettingsParameters.masterVolume);

        if (gameData.gameSettingsParameters.musicVolumeIsOff) PlayerPrefs.SetInt("MusicVolumeIsOff", 1);
        else PlayerPrefs.SetInt("MusicVolumeIsOff", 0);
        PlayerPrefs.SetFloat("MusicVolume", gameData.gameSettingsParameters.musicVolume);

        if (gameData.gameSettingsParameters.soundVolumeIsOff) PlayerPrefs.SetInt("SoundVolumeIsOff", 1);
        else PlayerPrefs.SetInt("SoundVolumeIsOff", 0);
        PlayerPrefs.SetFloat("SoundVolume", gameData.gameSettingsParameters.soundVolume);

        PlayerPrefs.Save();
    }

    #endregion
    //==============================================================================================================
}