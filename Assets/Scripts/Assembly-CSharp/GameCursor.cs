using UnityEngine;

// A single owner keeps cursor visibility and capture in sync with desktop UI.
[DefaultExecutionOrder(10000)]
public sealed class GameCursor : MonoBehaviour
{
    private static GameCursor s_Instance;
    private bool m_Focused;
    private bool m_ApplicationPaused;
    private bool m_WasGameplay;
    private bool m_ReleasedManually;

    public static bool IsCaptured { get { return Cursor.lockState == CursorLockMode.Locked; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (s_Instance == null) new GameObject("Desktop cursor").AddComponent<GameCursor>();
#endif
    }

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this) { Destroy(gameObject); return; }
        s_Instance = this;
        DontDestroyOnLoad(gameObject);
        m_Focused = Application.isFocused;
        Apply();
    }

    public static void Refresh()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (s_Instance != null) s_Instance.Apply();
#endif
    }

    public static void ToggleCapture()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (s_Instance == null) return;
        s_Instance.m_ReleasedManually = !s_Instance.m_ReleasedManually;
        s_Instance.Apply();
#endif
    }

    private void LateUpdate() { Apply(); }

    private void Apply()
    {
        bool gameplay = Game.Instance != null && Game.Instance.GameState == E_GameState.Game &&
                        Mission.Instance != null && !Mission.Instance.MissionIsEnding && Player.Instance != null;
        if (gameplay != m_WasGameplay)
        {
            m_ReleasedManually = false;
            m_WasGameplay = gameplay;
        }
        bool capture = gameplay && m_Focused && !m_ApplicationPaused && !m_ReleasedManually;
        SetCursor(capture);
    }

    private static void SetCursor(bool capture)
    {
#if UNITY_EDITOR
        if (capture)
        {
            // GameView disables this permission on Escape independently of the
            // requested Cursor.lockState. Restore it when returning to gameplay.
            UnityEditor.Unsupported.SetAllowCursorLock(true);
            UnityEditor.Unsupported.SetAllowCursorHide(true);
        }
#endif
        CursorLockMode mode = capture ? CursorLockMode.Locked : CursorLockMode.None;
        if (Cursor.lockState != mode) Cursor.lockState = mode;
        // Unlocking does not reliably make a previously hidden cursor visible.
        if (Cursor.visible != !capture) Cursor.visible = !capture;
    }

    private void OnApplicationFocus(bool focused) { m_Focused = focused; Apply(); }
    private void OnApplicationPause(bool paused) { m_ApplicationPaused = paused; Apply(); }
    private void OnDestroy()
    {
        if (s_Instance != this) return;
        s_Instance = null;
        SetCursor(false);
    }
}
