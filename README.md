# RealTimeTranslator
### Screen text translator `alpha` 

<!-- ![image](https://user-images.githubusercontent.com/35491968/202774868-cf49cf39-bd54-468e-a01e-df46ec8e8b1a.png) -->
![image](https://user-images.githubusercontent.com/35491968/202775116-b17ab47e-7e63-4c42-aa2f-5f0a6416284b.png)

### Now supports:
- Recognition of Russian, English and Japanese text;
- Recognized text translation via Google Translate;
- Automatic and manual translation;
- History of translations;
- Basic settings: languages, recognition settings, UI.

### Installation
Before running the app you have to download neccessary trained models from this repository https://github.com/tesseract-ocr/tessdata and place them in a specific folder (`C:\RTT_Data\TrainedData` by default).  
For now **only** 3 languages are supported for recognition. _(it's pretty easy to fix but I'm too lazy for it~)_

### Usage
There are 3 windows.

#### Main window
![image](https://user-images.githubusercontent.com/35491968/202778601-b45f75d3-9c20-4a54-b3b2-fe3d32d1846e.png)

You have to put this window around the text you want to be translated, like on the screenshot above.

The buttons mean:
- `A` - Auto mode (automatically translates the selected text when it changes);
- `L` - Recognize the last recognized area _(used for debug)_
- `T` - Translate the selected area manually;
- `R` - Only recognize the selected area (like `T` but without translating);
- `X` - Exit the app.

#### Translated text window



There is a settings window with "Threshold" setting which is one of the main settings. You have to adjust this value manually for better recognition. Read more here: https://docs.opencv.org/4.x/db/d8e/tutorial_threshold.html

---

### Описание функций  
###### (квадратными скобками обозначены кнопки в главном окне)
#### [L] - распознать текст на последней выделенной области (функция будет убрана или доработана).
#### [T] - перевести текст с выделенной области
#### [R] - распознать текст с выделенной области (без перевода)
#### [ X ] - выход из программы
#### Двойной клик по заголовку - действия, аналогичные нажатию на [T]
#### Клавиша ~ (тильда) - аналогично [T]
