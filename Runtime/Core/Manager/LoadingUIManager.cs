using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUIManager : MonoBehaviour
{
    public static LoadingUIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _percentageText;

    [Header("Animation")]
    [SerializeField] private float _fadeSpeed = 3f;
    [SerializeField] private float _progressSmoothSpeed = 5f;

    private List<AsyncOperation> _operations = new();
    private float _currentProgress = 0f;
    private bool _isVisible = false;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        HideImmediate();
    }

    public void BeginLoading()
    {
        _operations.Clear();
        _isVisible = true;
        _currentProgress = 0f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void TrackOperation(AsyncOperation op)
    {
        _operations.Add(op);
    }

    private void Update()
    {
        HandleFade();

        if (_operations.Count == 0) return;

        float totalProgress = _operations.Sum(op => op.progress);
        float targetProgress = totalProgress / _operations.Count;

        // Smooth interpolation
        _currentProgress = Mathf.Lerp(_currentProgress, targetProgress, Time.deltaTime * _progressSmoothSpeed);
        _progressBar.value = _currentProgress;

        int percentage = Mathf.RoundToInt(_currentProgress * 100f);
        _percentageText.text = $"{percentage}%";

        if (_operations.All(op => op.isDone))
        {
            _isVisible = false;
            _operations.Clear();
        }
    }

    private void HandleFade()
    {
        float targetAlpha = _isVisible ? 1f : 0f;
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, targetAlpha, Time.deltaTime * _fadeSpeed);

        if (_canvasGroup.alpha == 0f)
            _canvasGroup.blocksRaycasts = false;
    }

    private void HideImmediate()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _progressBar.value = 0f;
        _percentageText.text = "0%";
    }
}
