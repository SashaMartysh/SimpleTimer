using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SmartLocalization;

public class TimerManager : MonoBehaviour
{
    // показывает в секундах, сколько было вначале и сколько осталось
    int timerTotalSecondsLenght = 10;
    int timerTotalSecondsRemain;
    // показывает отдельно в минутах и секундах, сколько осталось (нужно для надписи)
    int timerRemainSeconds;
    int timerRemainMinutes;
    string timerRemain;
    bool halfTimeMustBeDinged = false; // показывает, нужен ли звук посередеине таймера, возвещающий о прошествии половины времени

    Color color = Color.green;

    bool TimerIsPaused = false;  // показывает, идет ли таймер или он на паузе

    AudioSource secondTick;
    public AudioClip SecondTickSound;

    public GameObject btn_CancelText;
    public GameObject btn_PauseText;
    public GameObject btn_RestartText;

	// Use this for initialization
	void Start ()
	{
	    Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (StaticTimerManager.language == "English")
            LanguageManager.Instance.ChangeLanguage("en");
        else if (StaticTimerManager.language == "Russian")
            LanguageManager.Instance.ChangeLanguage("ru");
        else LanguageManager.Instance.ChangeLanguage("uk");
	    btn_CancelText.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedCancel);
        btn_PauseText.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedPause);
        btn_RestartText.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedRestart);

        secondTick = gameObject.AddComponent<AudioSource>();
	    secondTick.clip = SecondTickSound;
	    secondTick.playOnAwake = false;
       // secondTick.clip = Resources.Load("Assets/Windows_Ding_Calligraphy.wav") as AudioClip;    // Странно, почему-то не работает

	    timerTotalSecondsRemain = StaticTimerManager.totalSeconds;
        timerTotalSecondsLenght = StaticTimerManager.totalSeconds;
	    if (timerTotalSecondsRemain > 30)
	        halfTimeMustBeDinged = true;                 // звук в середине времени нужен и еще не прозвучал
        ConvertTotalSecondsToMinutesAndSeconds();
        StartCoroutine(second_counting());
    }

    // отсчитывает секунды
    IEnumerator second_counting()
    {
        while (timerTotalSecondsRemain > 0)
        {
            yield return new WaitForSeconds(1);
            timerTotalSecondsRemain--;
            if (timerTotalSecondsRemain <= 9 && timerTotalSecondsRemain >= 0)
                    secondTick.Play();
            if (halfTimeMustBeDinged == true && timerTotalSecondsRemain <= timerTotalSecondsLenght/2)
            {
                halfTimeMustBeDinged = false;
                secondTick.Play();
            }
            ConvertTotalSecondsToMinutesAndSeconds();
            if (timerTotalSecondsRemain > timerTotalSecondsLenght/2)
                color = Color.Lerp(Color.yellow, Color.green, (float)(timerTotalSecondsRemain - (timerTotalSecondsLenght / 2)) / (float)(timerTotalSecondsLenght / 2));
            else
                color = Color.Lerp(Color.red, Color.yellow, (float)timerTotalSecondsRemain / (float)(timerTotalSecondsLenght / 2));

        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TimerMenu");
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle("Label");
        style.fontSize = (int)(200 * (Screen.height / (float)900));
        style.alignment = TextAnchor.MiddleCenter;
        if (TimerIsPaused == true)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 3), LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedPause), style);
        else GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 3), timerRemain, style);
        if (timerTotalSecondsRemain > 0 )
        DrawQuad(new Rect(0, Screen.height / 3, (float)timerTotalSecondsRemain/(float)timerTotalSecondsLenght*Screen.width, Screen.height / 3), color);
    }

    // рисует полоску таймера на экране
    void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }
    public void CancelTimer()
    {
        SceneManager.LoadScene("TimerMenu");
    }

    public void RestartTimer()
    {
        StaticTimerManager.StartTimer();
    }

    public void pauseUnpause()
    {
        if (TimerIsPaused == false)
        {
            TimerIsPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            TimerIsPaused = false;
            Time.timeScale = 1;
        }
    }

    void ConvertTotalSecondsToMinutesAndSeconds()
    {
        timerRemainMinutes = timerTotalSecondsRemain / 60;
        timerRemainSeconds = timerTotalSecondsRemain % 60;
        if (timerRemainMinutes == 0)
            timerRemain = timerRemainSeconds + " " + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
        else
            timerRemain = timerRemainMinutes + " " + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin) +"  "+ timerRemainSeconds + " " + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);

    }

}
