using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : PersistentSingleton<PauseManager>
{
    [SerializeField] private PausableRuntimeSetSO _pausableRTS;
    [SerializeField] private InputActionReference _pause;
    private bool _isPaused = false;

    private void OnEnable()
    {
        _isPaused = false;
        _pause.action.started += TogglePause;
        _pause.action.Enable();
    }

    private void OnDisable()
    {
        _pause.action.started -= TogglePause;
        _pause.action.Disable();
    }


    [ContextMenu("Pause")]
    public void RequestPause()
    {
        _isPaused = true; 
        foreach (var pausable in _pausableRTS.Items)
        {
            Time.timeScale = 0f;
            pausable?.OnPaused();
        }
    }

    [ContextMenu("Resume")]
    public void ReleasePause()
    {
        _isPaused = false;
        foreach (var pausable in _pausableRTS.Items)
        {
            Time.timeScale = 1f;
            pausable?.OnResumed();
        }
    }

    public void TogglePause()
    {
        if (_isPaused)
            ReleasePause();
        else
            RequestPause();
    }

    private void TogglePause(InputAction.CallbackContext obj)
    {
        TogglePause();
    }
}
