using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class FinishLine : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI[] timesText = new TextMeshProUGUI[9];
    
    private bool isTimerRunning = false;
    private float elapsedTime = 0f;
    private float startTime;
    private float[] times = new float[9];

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer Text reference is missing on FinishLine script!");
        }
        for (int i = 0; i < 9; i++)
        {
            if (timesText[i] == null)
            {
                Debug.LogError($"Times Text reference at index {i} is missing on FinishLine script!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player");
        bool hasPlayerParent = other.transform.parent != null && other.transform.parent.CompareTag("Player");
        
        if (isPlayer || hasPlayerParent)
        {
            if (!isTimerRunning)
            {
                // Start the timer
                StartTimer();
            }
            else
            {
                // Stop the timer
                StopTimer();
            }
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerDisplay();
        }
    }

    private void StartTimer()
    {
        isTimerRunning = true;
        startTime = Time.time;
        elapsedTime = 0f;
    }

    private void StopTimer()
    {
        if (elapsedTime > 10)
        {
            isTimerRunning = false;
            
            // Add the new time and keep the array sorted
            for (int i = 0; i < times.Length; i++)
            {
                if (elapsedTime < times[i] || times[i] == 0)
                {
                    // Shift all elements after this position
                    for (int j = times.Length - 1; j > i; j--)
                    {
                        times[j] = times[j - 1];
                    }
                    times[i] = elapsedTime;
                    break;
                }
            }
            
            UpdateTimesDisplay();
            StartTimer();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(elapsedTime);
        }
    }

    private void UpdateTimesDisplay()
    {
        for (int i = 0; i < 9; i++)
        {
            if (times[i] != 0) {
                timesText[i].text = i + 1 + ". " + FormatTime(times[i]);
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}