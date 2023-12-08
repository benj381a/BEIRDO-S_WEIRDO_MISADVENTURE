using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO.Ports;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Extensions
{
    public struct _
    {
        public static void If(Dictionary<bool,Action> d)
        {
            for (int i = 0; i < d.Count; i++)
            {
                if (d.Keys.ToArray()[i])
                {
                    d.Values.ToArray()[i]();
                }
            }
        }
        public static void elif(Dictionary<bool, Action> d)
        {
            for (int i = 0; i < d.Count; i++)
            {
                if (d.Keys.ToArray()[i])
                {
                    d.Values.ToArray()[i]();
                    break;
                }
            }
        }
    }
    /*public struct Serial
    {

        public static SerialPort sp = new SerialPort();

        public static void Init(int baudrate = 9600, int readTimeout = 100, int writeTimeout = 100)
        {
            int y = -300;
            SerialPort sp1 = new SerialPort("COM0", baudrate);
            sp1.ReadTimeout = readTimeout;
            sp1.WriteTimeout = writeTimeout;

            GameObject eventSystem = new GameObject("SP_EventSystem");

            if (EventSystem.current == null)
            {
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            GameObject newCanvas = new GameObject("SP_Canvas");
            Canvas c = newCanvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.sortingOrder = 1000;
            newCanvas.AddComponent<CanvasScaler>();
            newCanvas.AddComponent<GraphicRaycaster>();

            GameObject view = new GameObject("SP_View");
            view.AddComponent<CanvasRenderer>();
            view.transform.SetParent(newCanvas.transform);
            view.AddComponent<RectTransform>();
            view.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            view.GetComponent<RectTransform>().anchorMax = Vector2.one;
            view.GetComponent<RectTransform>().pivot = Vector2.one * 0.5f;
            view.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            ScrollRect view_Rect = view.AddComponent<ScrollRect>();
            view_Rect.horizontal = false;
            view_Rect.vertical = true;

            GameObject _UIContent = new GameObject("SP_Content");
            _UIContent.AddComponent<CanvasRenderer>();
            _UIContent.transform.SetParent(view.transform);
            _UIContent.AddComponent<RectTransform>();
            _UIContent.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            _UIContent.GetComponent<RectTransform>().anchorMax = Vector2.one;
            _UIContent.GetComponent<RectTransform>().pivot = Vector2.one * 0.5f;
            _UIContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            c = _UIContent.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.overrideSorting = true;
            c.sortingOrder = 1001;
            _UIContent.AddComponent<GraphicRaycaster>();
            view_Rect.content = _UIContent.GetComponent<RectTransform>();

            GameObject scrollbar = new GameObject("SP_Scrollbar");
            scrollbar.AddComponent<CanvasRenderer>();
            scrollbar.transform.SetParent(view.transform);
            scrollbar.AddComponent<RectTransform>();
            scrollbar.GetComponent<RectTransform>().anchorMin = Vector2.right;
            scrollbar.GetComponent<RectTransform>().anchorMax = Vector2.one;
            scrollbar.GetComponent<RectTransform>().pivot = Vector2.one;
            scrollbar.GetComponent<RectTransform>().localScale = new Vector2(20, 17);
            Scrollbar scrollbar_bar = scrollbar.AddComponent<Scrollbar>();
            view_Rect.verticalScrollbar = scrollbar_bar;

            GameObject slidingArea = new GameObject("SP_Sliding_Area");
            slidingArea.AddComponent<CanvasRenderer>();
            slidingArea.transform.SetParent(scrollbar.transform);
            slidingArea.AddComponent<RectTransform>();
            slidingArea.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            slidingArea.GetComponent<RectTransform>().anchorMax = Vector2.one;
            slidingArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            slidingArea.GetComponent<RectTransform>().localScale = Vector2.one * 10;
            slidingArea.GetComponent<RectTransform>().position = Vector2.one * 10;

            GameObject handle = new GameObject("SP_Handle");
            handle.AddComponent<CanvasRenderer>();
            handle.transform.SetParent(slidingArea.transform);
            handle.AddComponent<RectTransform>();
            handle.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            handle.GetComponent<RectTransform>().anchorMax = Vector2.one;
            handle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            handle.GetComponent<RectTransform>().localScale = Vector2.one * -10;
            handle.GetComponent<RectTransform>().position = Vector2.one * -10;

            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.clear;
            colorBlock.highlightedColor = Color.clear;
            colorBlock.pressedColor = Color.clear;
            colorBlock.selectedColor = Color.clear;
            colorBlock.disabledColor = Color.clear;

            scrollbar_bar.targetGraphic = handle.AddComponent<Image>();
            scrollbar_bar.colors = colorBlock;
            scrollbar_bar.handleRect = handle.GetComponent<RectTransform>();


            foreach (string str in SerialPort.GetPortNames())
            {
                TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
                GameObject button = TMP_DefaultControls.CreateButton(resources);
                button.name = "SP_Button_" + str;
                button.GetComponent<Button>().onClick.AddListener(() => {
                    sp1.PortName = str;
                    sp1.Open();
                    sp = sp1;
                    GameObject.Destroy(newCanvas);
                    GameObject.Destroy(eventSystem);
                });
                button.GetComponentInChildren<TMP_Text>().text = str;
                button.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
                button.GetComponentInChildren<TMP_Text>().fontSizeMax = 1000;
                button.transform.SetParent(_UIContent.transform);
                button.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                button.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 200);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, y, 0);
                y -= 250;
            }
        }
    }*/
}
