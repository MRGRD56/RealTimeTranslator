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

After installing the trained models you should <a href="https://github.com/MRGRD56/RealTimeTranslator/releases">download the latest release</a>, unzip the archive and run `RealTimeTranslator.exe`.

### Usage

#### Main window
![image](https://user-images.githubusercontent.com/35491968/202778601-b45f75d3-9c20-4a54-b3b2-fe3d32d1846e.png)

You have to put this window around the text you want to be translated, like on the screenshot above.

The buttons mean:
- `A` - <ins>A</ins>uto mode (automatically translates the selected text when it changes);
- `L` - Recognize the <ins>l</ins>ast recognized area _(used for debug)_
- `T` - <ins>T</ins>ranslate the selected area manually;
- `R` - Only <ins>r</ins>ecognize the selected area (like `T` but without translating);
- `X` - Exit the app.

Also you can manually translate the selected text by pressing `~` on your keyboard or double clicking the title of the main window. 

#### Translated text window
![image](https://user-images.githubusercontent.com/35491968/202781158-a8f7a560-5609-416a-a249-16584900cb63.png)

You can resize and move it however and wherever you want. You can scroll this window to see the old translated text. The original (recognized) and the translated text are displayed.

#### Settings window
![image](https://user-images.githubusercontent.com/35491968/202781828-8079f358-c184-4a92-86fb-0d5697a158cd.png)

_(yes it looks ugly and is not completely in English yet)_

This window is minimized by default.  
There is the "Threshold" setting which is one of the main settings. You have to adjust this value manually for better recognition. Learn more here: https://docs.opencv.org/4.x/db/d8e/tutorial_threshold.html

---

See also: https://github.com/MRGRD56/textractor-translator
