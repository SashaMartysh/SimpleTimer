using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SmartLocalization;


public static class StaticTimerManager
{
    public static int totalSeconds;
    public static int setSeconds;    // количество секунд, которое выбрано вручную
    public static int setMinutes;    // количество минут, которое выбрано вручную

    // Заготовки для локализации надписей на кнопках на разные языки
    public static string localizedExit = "Exit";
    public static string localizedStart = "Start";
    public static string localizedRestart = "Restart";
    public static string localizedPause = "Pause";
    public static string localizedMin = "min";
    public static string localizedSec = "sec";
    public static string localizedCancel = "Cancel";
    public static string language = "English";          // English, Ukrainian, Russian

    public static void StartTimer()
    {
        totalSeconds = setMinutes*60 + setSeconds;
        SceneManager.LoadScene("ProgressTimerScreen");
        Debug.Log("Timer was started for " +setMinutes + " minutes and "+setSeconds +" seconds (total " + totalSeconds +" seconds)");
    }

}
