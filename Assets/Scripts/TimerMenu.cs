using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SmartLocalization;
using UnityEngine.SceneManagement;


public class TimerMenu : MonoBehaviour
{
    // ссылки на текст на кнопках выбора минут и секунд (set in Inspector)
    public GameObject MinutesShow;
    public GameObject SecondsShow;
    // эти две переменные показывают, не нажал ли человек кнопку выбора минут или секунд
    bool selectingMinutes = false;
    bool selectingSeconds = false;
    // эта переменная показывает, нажал ли человек кнопку просмотра информации о программе и авторе
    bool showingInfo = false;

    string info =
        "Simple Timer (v1.1 from januar 1, 2017)\nAuthor: Sasha Martysh (Ukraine, Dnipr)\nMy Contacts:\ne-mail: dnepr.sa@mail.ru\nmobile phone: +380959058675\n\nThank you for using my program!";
    // эта переменная используется для хранения выбранного числа при выборе минут и секунд вручную
    byte selectedNumber = 0;
    // это мой стиль для кнопок при выборе минут и секунд вручную
    GUIStyle btnStyle;
    GUIStyle lblStyle;

    public GameObject btn_1st_PredeclaredText;
    public GameObject btn_2nd_PredeclaredText;
    public GameObject btn_3rd_PredeclaredText;
    public GameObject btn_4th_PredeclaredText;
    public GameObject btn_5th_PredeclaredText;
    int predeclared_1st_time;
    int predeclared_2nd_time;
    int predeclared_3rd_time;
    int predeclared_4th_time;
    int predeclared_5th_time;

    public Texture2D myPhoto;

    // ссылка на текст на кнопках (для локализации)
    public GameObject btn_ExitText;
    public GameObject btn_StartText;

    public static GameObject InstanceOfThisCanvas ;
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        if (StaticTimerManager.language == "English")
            LanguageManager.Instance.ChangeLanguage("en");
        else if (StaticTimerManager.language == "Russian")
            LanguageManager.Instance.ChangeLanguage("ru");
        else LanguageManager.Instance.ChangeLanguage("uk");
        RefreshlocalizedText();
        //DontDestroyOnLoad(gameObject);
        if (InstanceOfThisCanvas == null)
            InstanceOfThisCanvas = gameObject;
        else Destroy(gameObject);
        PredeclareSomeButtons();
        
        // при появлении меню сразу показываем, какое последнее время было выбрано для таймера
        MinutesShow.GetComponent<Text>().text = StaticTimerManager.setMinutes + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
        SecondsShow.GetComponent<Text>().text = StaticTimerManager.setSeconds + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
        // делаем настройку стиля текста для кнопок и надписей
        SetupGUIStyles();
    }

    // делаем настройку стиля текста для кнопок и надписей
    private void SetupGUIStyles()
    {
        btnStyle = new GUIStyle("Button");
        btnStyle.normal.textColor = Color.white;
        btnStyle.alignment = TextAnchor.MiddleCenter;
        lblStyle = new GUIStyle("Label");
        lblStyle.alignment = TextAnchor.LowerCenter;
        lblStyle.normal.textColor = Color.white;
        //размер шрифта в надписи изменяется динамически в зависимости от высоты экрана, чтоб всегда быть удобочитаемым
        lblStyle.fontSize = (int) (50*(Screen.height/(float) 900));
        Debug.Log("LABEL. FontSize: " + lblStyle.fontSize + "   Font Style: " + lblStyle.fontStyle + "   Font: " +
                  lblStyle.font);
        //размер шрифта в кнопках изменяется динамически в зависимости от ширины экрана, чтоб всегда помещаться в кнопках
        btnStyle.fontSize = (int) (100*(Screen.width/(float) 1600));
        Debug.Log("BUTTON. FontSize: " + btnStyle.fontSize + "   Font Style: " + btnStyle.fontStyle + "   Font: " +
                  btnStyle.font);
    }

    // Тут можно (и нужно) задать параметры для предустановленных таймеров
    private void PredeclareSomeButtons()
    {
        predeclared_1st_time = 10;
        predeclared_2nd_time = 30;
        predeclared_3rd_time = 60;
        predeclared_4th_time = 300;
        predeclared_5th_time=600;
        btn_1st_PredeclaredText.GetComponent<Text>().text = "10\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
        btn_2nd_PredeclaredText.GetComponent<Text>().text = "30\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
        btn_3rd_PredeclaredText.GetComponent<Text>().text = "1\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
        btn_4th_PredeclaredText.GetComponent<Text>().text = "5\n"+ LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
        btn_5th_PredeclaredText.GetComponent<Text>().text = "10\n"+ LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
    }

    // ============================================================================================
    #region К Н О П К И   В   М Е Н Ю
    public void selectMinutes()
    {
        // нажата кнопка чтоб выбирать минуты для таймера
        selectingMinutes = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void selectSeconds()
    {
        selectingSeconds = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void StartTimer()
    {
        StaticTimerManager.StartTimer();
    }
    // следующие пять кнопок служат для запуска предустановленных наиболее типичных по времени таймеров (1 мин, 5 мин, и т.д.)
    public void setTimerBy_1st_Button()
    {
        StaticTimerManager.setMinutes = 0;
        StaticTimerManager.setSeconds = predeclared_1st_time;
        StartTimer();
    }
    public void setTimerBy_2nd_Button()
    {
        StaticTimerManager.setMinutes = 0;
        StaticTimerManager.setSeconds = predeclared_2nd_time;
        StartTimer();
    }
    public void setTimerBy_3rd_Button()
    {
        StaticTimerManager.setMinutes = 0;
        StaticTimerManager.setSeconds = predeclared_3rd_time;
        StartTimer();
    }
    public void setTimerBy_4th_Button()
    {
        StaticTimerManager.setMinutes = 0;
        StaticTimerManager.setSeconds = predeclared_4th_time;
        StartTimer();
    }
    public void setTimerBy_5th_Button()
    {
        StaticTimerManager.setMinutes = 0;
        StaticTimerManager.setSeconds = predeclared_5th_time;
        StartTimer();
    }
    // эта кнопка показывает информацию об авторе проекта (т.е. обо мне)
    public void ShowInfo()
    {
        showingInfo = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void ExitApplication()
    {
        Application.Quit();
    }

    // кнопки переключения языков
    public void selectEnglish()
    {
        LanguageManager.Instance.ChangeLanguage("en");
        StaticTimerManager.language = "English";
        RefreshlocalizedText();
        //SceneManager.LoadScene("TimerMenu");
    }
    public void selectUkrainian()
    {
        LanguageManager.Instance.ChangeLanguage("uk");
        StaticTimerManager.language = "Ukrainian";
        RefreshlocalizedText();
        // Debug.Log(LanguageManager.HasInstance);
        //SceneManager.LoadScene("TimerMenu");
    }
    public void selectRussian()
    {
        LanguageManager.Instance.ChangeLanguage("ru");
        StaticTimerManager.language = "Russian";
        RefreshlocalizedText();
        //SceneManager.LoadScene("TimerMenu");
    }
    #endregion
    // ============================================================================================
    // ============================================================================================
    #region G U I
    void OnGUI()
    {
        // если нажат вопросительный значок в углу экрана, то показывать мое фото и информацию обо мне, а также кнопку возврата
        if(showingInfo == true)
             DrawMyPhotoAndInfo();
        // если нажата кнопка чтоб выбирать минуты или секунды для таймера, то отображаем 60 кнопок на экране 
        if (selectingMinutes || selectingSeconds)
            DrawNumberButtons();
    }

    // Показывает мое фото и информацию обо мне и программе, а также кнопку возврата к меню
    void DrawMyPhotoAndInfo()
    {
      //  myPhoto.Apply();
        GUI.skin.box.normal.background = myPhoto;
        GUI.Box(new Rect(10, 10, Screen.height/2/5*6, Screen.height/2), GUIContent.none);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), info, lblStyle);
        if (GUI.Button(new Rect((Screen.width - (Screen.height / 6 * 2) - 10), 10, Screen.height / 6 * 2, Screen.height / 6), LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedExit), btnStyle))
        {
            showingInfo = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    // рисует много кнопок на экране для выбора времени
    void DrawNumberButtons()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width/12, Screen.height/7), "0", btnStyle))
        {
            selectedNumber = 0;
            SetMinutesOrSeconds(selectedNumber);
        }
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 0; j <= 5; j++)
            {
                Vector2 position = new Vector2(Screen.width / 11 * i, Screen.height / 6 * j);
                Vector2 size = new Vector2(Screen.width / 11.5f, Screen.height / 6.5f);
                if (GUI.Button(new Rect(position, size), (j * 10 + i).ToString(), btnStyle))
                {
                    selectedNumber = (byte)(j * 10 + i);
                    SetMinutesOrSeconds(selectedNumber);
                }
            }
        }
    }

    void SetMinutesOrSeconds(byte selectedNumber)
    {
        if (selectingMinutes == true)
                    {
                        selectingMinutes = false;
                        MinutesShow.GetComponent<Text>().text = selectedNumber + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
                        StaticTimerManager.setMinutes = selectedNumber;
                    }
        if (selectingSeconds == true)
                    {
                        selectingSeconds = false;
                        SecondsShow.GetComponent<Text>().text = selectedNumber + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
                        StaticTimerManager.setSeconds = selectedNumber;
                    }
        Debug.Log(selectedNumber.ToString() + " was pressed");
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    #endregion
    // ============================================================================================


    void RefreshlocalizedText()
    {
        MinutesShow.GetComponent<Text>().text = StaticTimerManager.setMinutes + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedMin);
        SecondsShow.GetComponent<Text>().text = StaticTimerManager.setSeconds + "\n" + LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedSec);
        PredeclareSomeButtons();
        btn_StartText.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedStart);
        btn_ExitText.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(StaticTimerManager.localizedExit);
    }
}
                        