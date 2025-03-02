using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NovelManager : MonoBehaviour
{
    public GameManager gameManager; // Ссылка на GameManager
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI novelText;
    public Image portraitImage;
    public Image novelWindow;
    public Button nextButton;
    public Button prevButton;

    private List<Novel> novel;
    private int currentIndex = 0;

    public void StartNovel(string novelName, int novelStartIndex)
    {
        Debug.Log($"StartNovel {novelName} {novelStartIndex}");
        LoadNovel(novelName); // Название файла новеллы, по умолчанию Novel_start
        ShowNovel(novelStartIndex); // С какой позиции начать, по умолчанию 0
    }

    private void LoadNovel(string novelName)
    {
        // Загружаем JSON-файл из папки Resources
        TextAsset novelJson = Resources.Load<TextAsset>("Novels/" + novelName); // Файл должен быть в "Assets/Resources/Novels/{novelName}.json"
        if (novelJson == null)
        {
            Debug.LogError($"Не удалось загрузить {novelName}.json!");
            return;
        }

        NovelData data = JsonUtility.FromJson<NovelData>(novelJson.text);
        novel = data.novel;
    }

    private void ShowNovel(int index)
    {
        if (index < 0 || index >= novel.Count)
        {
            Debug.LogError($"ShowNovel: Указан не верный index ({index}) для отображения новеллы!");
            return;
        }

        currentIndex = index;
        Novel currentData = novel[currentIndex];

        characterName.text = currentData.name;
        novelText.text = currentData.text;

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
        nextButton.interactable = currentIndex < novel.Count;
    }

    private void ChangeFont(string fontName)
    {
        string fontFile = GetFontFile(fontName);


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
        else
        {
            gameManager.NovelFinished(); // Сообщаем GameManager, что новелла закончилась
        }
    }


    public void NovelPrev()
    {
        if (currentIndex > 0)
        {
            ShowNovel(currentIndex - 1);
        }
    }

    private string GetFontFile(string font)
    {
        switch (font)
        {
            case "anime": return "Anime Ace v3 SDF";
            case "default": return "Asinastra SDF";
            default: return "Asinastra SDF";
        }
    }
}
