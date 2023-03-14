using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GoogleDriveBridge;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : UIPanel
{
    [SerializeField]
    private InputField _emailInput;

    [SerializeField]
    private InputField _codeInput;

    [SerializeField]
    private TextMeshProUGUI _sendText;

    [SerializeField]
    private Button _loginbutton;

    private EventTrigger _trigger;

    private bool _canSendCode;

    public void Awake()
    {
        _trigger = _sendText.gameObject.GetComponent<EventTrigger>();
        if (!_trigger)
        {
            _trigger = _sendText.gameObject.AddComponent<EventTrigger>();
        }
    }
    public override async UniTask InitAsync()
    {
        await base.InitAsync();

        DisableInput();
        
        CanvasGroup.alpha = 0;
        await CanvasGroup.DOFade(1, 3).SetEase(Ease.InOutCubic);

        _canSendCode = true;
        EnableInput();
    }

    public override async UniTask CloseAsync()
    {
        DisableInput();
        await CanvasGroup.DOFade(0, 1).SetEase(Ease.InOutCubic);
    }

    public override void EnableInput()
    {
        var callbackEntry = new EventTrigger.Entry();
        callbackEntry.callback.AddListener(OnSendClicked);

        _trigger.triggers.Add(callbackEntry);
        _loginbutton.interactable = true;
    }

    public override void DisableInput()
    {
        _trigger.triggers.Clear();
        _loginbutton.interactable = false;
    }


    private void OnSendClicked(BaseEventData eventData)
    {
        if (!_canSendCode)
            return;

        SendVerificationCode().Forget();
    }

    private void OnLoginClicked()
    {
        Login().Forget();
    }

    private async UniTask SendVerificationCode()
    {
        _canSendCode = false;

        GoogleDrive.Instance.SendVerificationCode(_emailInput.text).Forget();
        
        await CountTimeToAllowResend();

        _canSendCode = true;
    }

    private async UniTask Login()
    {
        DisableInput();
        var result = await GoogleDrive.Instance.Login(_emailInput.text, _codeInput.text);
        if (result.result)
        {
            //setup player info
            PlayerInfo.Instance.Email = _emailInput.text;

            //setup google sheet
            GoogleDrive.Instance.UserSheetID = result.sid;

            List<Row> list = new List<Row>();
            Row v1 = new Row();
            v1.cellValues.Add("zack");
            v1.cellValues.Add("5");
            v1.cellValues.Add("100");

            Row v2 = new Row();
            v2.cellValues.Add("not zack");
            v2.cellValues.Add("500");
            v2.cellValues.Add("10");

            list.Add(v1);
            list.Add(v2);


            await GoogleDrive.Instance.AddRows("PlayerInfo", list, true);

            await UIManager.Instance.RemoveAsync<LoginPanel>();
            UIManager.Instance.PushAsync<DataPanel>().Forget();
        }
        else
        {
            EnableInput();
        }
    }

    private async UniTask CountTimeToAllowResend()
    {
        int resendCounter = 60;

        while (resendCounter >= 0)
        {
            resendCounter -= 1;
            _sendText.text = $"resend in {resendCounter}s";

            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
        }

        _sendText.text = "send";
    }
    
}
