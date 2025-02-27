using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NovelManager : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI novelText;
    public Image portraitImage;
    public Image novelWindow;
    public Button nextButton;
    public Button prevButton;

    private List<Novel> novel;
    private int currentIndex = 0;

    void Start()
    {
        LoadNovel();
        ShowNovel(0);
    }

    void LoadNovel()
    {
        // Загружаем JSON-файл из папки Resources
        TextAsset novelJson = Resources.Load<TextAsset>("Novel"); // Файл должен быть в "Assets/Resources/Novel.json"
        if (novelJson != null)
        {
            NovelData data = JsonUtility.FromJson<NovelData>(novelJson.text);
            novel = data.novel;
        }
        else
        {
            Debug.LogError("Не удалось загрузить Novel.json!");
        }
    }

    public void ShowNovel(int index)
    {
        if (index < 0 || index >= novel.Count)
            return;

        currentIndex = index;
        Novel currentData = novel[currentIndex];

        characterName.text = currentData.name;
        novelText.text = currentData.text;
        // novelWindow.color = GetColorFromString(currentData.windowColor);

        // Устанавливаем портрет
        Sprite portrait = Resources.Load<Sprite>("Characters/" + currentData.spriteName);
        if (portrait != null)
        {
            portraitImage.sprite = portrait;
        }

        // Меняем шрифт в зависимости от значения в JSON
        ChangeFont(currentData.font);

        // Обновляем доступность кнопок
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < novel.Count - 1;
    }

    private void ChangeFont(string fontName)
    {
        string fontFile;

        // Используем switch для выбора языка
        switch (fontName)
        {
            case "anime":
                fontFile = "Anime Ace v3 SDF";
                break;
            default:
                fontFile = "Asinastra SDF";
                break;
        }
        // Загружаем шрифт из папки Resources
        TMP_FontAsset newFont = Resources.Load<TMP_FontAsset>("Fonts/" + fontFile); // Путь к шрифтам в Resources/Fonts

        // Если шрифт найден, применяем его
        if (newFont != null)
        {
            novelText.font = newFont;
        }
        else
        {
            Debug.LogError("Шрифт не найден: " + fontFile);
        }
    }

    public void NovelNext()
    {
        if (currentIndex < novel.Count - 1)
        {
            ShowNovel(currentIndex + 1);
        }
    }

    public void NovelPrev()
    {
        if (currentIndex > 0)
        {
            ShowNovel(currentIndex - 1);
        }
    }

    private Color GetColorFromString(string color)
    {
        switch (color.ToLower())
        {
            case "blue": return Color.blue;
            case "red": return Color.red;
            case "green": return Color.green;
            default: return Color.white;
        }
    }
}
