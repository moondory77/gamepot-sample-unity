using System.Collections.Generic;
using UnityEngine;
using Realtime.LITJson;

public class GamePotEventListener : MonoBehaviour
{
    public static GamePotEventListener s_instance;

    // --- INTERFACE ---
    public IGamePot GamePotInterface = null;

    // --- DELEGATE ---
    public static GamePotCallbackDelegate.CB_Common cbAppClose = null;
    public static GamePotCallbackDelegate.CB_Login cbLogin = null;
    public static GamePotCallbackDelegate.CB_Common cbDeleteMember = null;
    public static GamePotCallbackDelegate.CB_Common cbLogout = null;
    public static GamePotCallbackDelegate.CB_Common cbCoupon = null;
    public static GamePotCallbackDelegate.CB_Purchase cbPurchase = null;
    public static GamePotCallbackDelegate.CB_CreateLinking cbCreateLinking = null;
    public static GamePotCallbackDelegate.CB_Common cbDeleteLinking = null;
    public static GamePotCallbackDelegate.CB_Common cbPushEnable = null;
    public static GamePotCallbackDelegate.CB_Common cbPushNightEnable = null;
    public static GamePotCallbackDelegate.CB_Common cbPushADEnable = null;
    public static GamePotCallbackDelegate.CB_Common cbPushStatusEnable = null;
    public static GamePotCallbackDelegate.CB_ShowAgree cbShowAgree = null;
    public static GamePotCallbackDelegate.CB_ReceiveScheme cbReceiveScheme = null;
    public static GamePotCallbackDelegate.CB_ShowWebView cbShowWebView = null;


    private void Awake()
    {
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void setListener(IGamePot v)
    {
        Debug.Log("GamePotEventListener::setListener()");
        GamePotInterface = v;
    }

    public void onAppClose()
    {
        Debug.Log("GamePotEventListener::onAppClose()");


        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.APP_CLOSE, null, null);
            cbLogin = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onAppClose();
        }
    }

    public void onNeedUpdate(string result)
    {
        Debug.Log("GamePotEventListener::onNeedUpdate-" + result);
        NAppStatus status = JsonMapper.ToObject<NAppStatus>(result);
        if (!string.IsNullOrEmpty(status.resultPayload))
        {
            status.userInfo = JsonMapper.ToObject<NUserInfo>(status.resultPayload);
        }

        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.NEED_UPDATE, null, status);
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onNeedUpdate(status);
        }
    }

    public void onMainternance(string result)
    {
        Debug.Log("GamePotEventListener::onMainternance-" + result);
        NAppStatus status = JsonMapper.ToObject<NAppStatus>(result);
        status.userInfo = null;

        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.NEED_UPDATE, null, status);
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onMainternance(status);
        }
    }

    public void onLoginSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onLoginSuccess-" + result);
        NUserInfo userInfo = JsonMapper.ToObject<NUserInfo>(result);

        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.SUCCESS, userInfo);
            cbLogin = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onLoginSuccess(userInfo);
            }
        }
    }
    public void onLoginCancel()
    {
        Debug.Log("GamePotEventListener::onLoginCancel()");
        if (GamePotInterface != null)
            GamePotInterface.onLoginCancel();

        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.CANCELLED);
            cbLogin = null;
        }
    }

    public void onLoginFailure(string result)
    {
        Debug.Log("GamePotEventListener::onLoginFailure()-" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbLogin != null)
        {
            cbLogin(NCommon.ResultLogin.FAILED, null, null, error);
            cbLogin = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onLoginFailure(error);
            }
        }
    }

    public void onDeleteMemberFailure(string result)
    {
        Debug.Log("GamePotEventListener::onDeleteMemberFailure() - " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbDeleteMember != null)
        {
            cbDeleteMember(false, error);
            cbDeleteMember = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onDeleteMemberFailure(error);
            }
        }
    }

    public void onDeleteMemberSuccess()
    {
        Debug.Log("GamePotEventListener::onDeleteMemberSuccess()");

        if (cbDeleteMember != null)
        {
            cbDeleteMember(true);
            cbDeleteMember = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onDeleteMemberSuccess();
        }
    }

    public void onLogoutFailure(string result)
    {
        Debug.Log("GamePotEventListener::onLogOutFailure() - " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbLogout != null)
        {
            cbLogout(false, error);
            cbLogout = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onLogoutFailure(error);
            }
        }
    }

    public void onLogoutSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onLogOutSuccess()");

        if (cbLogout != null)
        {
            cbLogout(true);
            cbLogout = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onLogoutSuccess();
        }
    }


    public void onCouponSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onCouponSuccess() " + result);

        if (cbCoupon != null)
        {
            cbCoupon(true);
            cbCoupon = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onCouponSuccess();
        }
    }

    public void onCouponFailure(string result)
    {
        Debug.Log("GamePotEventListener::onCouponFailure() : " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbCoupon != null)
        {
            cbCoupon(false, error);
            cbCoupon = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onCouponFailure(error);
            }
        }
    }

    public void onPurchaseSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onPurchaseSuccess()");
        NPurchaseInfo purchaseInfo = JsonMapper.ToObject<NPurchaseInfo>(result);

        if (cbPurchase != null)
        {
            cbPurchase(NCommon.ResultPurchase.SUCCESS, purchaseInfo);
            cbPurchase = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPurchaseSuccess(purchaseInfo);
            }
        }
    }

    public void onPurchaseFailure(string result)
    {
        Debug.Log("GamePotEventListener::onPurchaseFailure() - " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbPurchase != null)
        {
            cbPurchase(NCommon.ResultPurchase.FAILED, null, error);
            cbPurchase = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPurchaseFailure(error);
            }
        }
    }

    public void onPurchaseCancel()
    {
        Debug.Log("GamePotEventListener::onPurchaseCancel()");

        if (cbPurchase != null)
        {
            cbPurchase(NCommon.ResultPurchase.CANCELLED);
            cbPurchase = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onPurchaseCancel();
        }
    }


    public void onCreateLinkingCancel(string result)
    {
        Debug.Log("GamePotEventListener::onCreateLinkCancel()" + result);

        if (cbCreateLinking != null)
        {
            cbCreateLinking(NCommon.ResultLinking.CANCELLED);
            cbCreateLinking = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onCreateLinkingCancel();
        }
    }

    public void onCreateLinkingSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onCreateLinkSuccess() - " + result);
        NUserInfo userInfo = JsonMapper.ToObject<NUserInfo>(result);

        if (cbCreateLinking != null)
        {
            cbCreateLinking(NCommon.ResultLinking.SUCCESS, userInfo);
            cbCreateLinking = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onCreateLinkingSuccess(userInfo);
            }
        }
    }

    public void onCreateLinkingFailure(string result)
    {
        Debug.Log("GamePotEventListener::onCreateLinkFailure() - " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbCreateLinking != null)
        {
            cbCreateLinking(NCommon.ResultLinking.FAILED, null, error);
            cbCreateLinking = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onCreateLinkingFailure(error);
            }
        }
    }
    public void onDeleteLinkingSuccess()
    {
        Debug.Log("GamePotEventListener::onDeleteLinkSuccess()");

        if (cbDeleteLinking != null)
        {
            cbDeleteLinking(true);
            cbDeleteLinking = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onDeleteLinkingSuccess();
        }
    }

    public void onDeleteLinkingFailure(string result)
    {
        Debug.Log("GamePotEventListener::onDeleteLinkFailure() - " + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbDeleteLinking != null)
        {
            cbDeleteLinking(false, error);
            cbDeleteLinking = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onDeleteLinkingFailure(error);
            }
        }
    }

    public void onPushSuccess()
    {
        Debug.Log("GamePotEventListener::onPushSuccess()");

        if (cbPushEnable != null)
        {
            cbPushEnable(true);
            cbPushEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onPushSuccess();
        }
    }

    public void onPushFailure(string result)
    {
        Debug.Log("GamePotEventListener::onPushFailure()" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbPushEnable != null)
        {
            cbPushEnable(false, error);
            cbPushEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPushFailure(error);
            }
        }
    }

    public void onPushNightSuccess()
    {
        Debug.Log("GamePotEventListener::onPushNightSuccess()");

        if (cbPushNightEnable != null)
        {
            cbPushNightEnable(true);
            cbPushNightEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onPushNightSuccess();
        }
    }

    public void onPushNightFailure(string result)
    {
        Debug.Log("GamePotEventListener::onPushNightFailure()" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbPushNightEnable != null)
        {
            cbPushNightEnable(false, error);
            cbPushNightEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPushNightFailure(error);
            }
        }
    }

    public void onPushAdSuccess()
    {
        Debug.Log("GamePotEventListener::onPushAdSuccess()");

        if (cbPushADEnable != null)
        {
            cbPushADEnable(true);
            cbPushADEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onPushAdSuccess();
        }
    }

    public void onPushAdFailure(string result)
    {
        Debug.Log("GamePotEventListener::onPushAdFailure()" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbPushADEnable != null)
        {
            cbPushADEnable(false, error);
            cbPushADEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPushAdFailure(error);
            }
        }
    }

    public void onPushStatusSuccess()
    {
        Debug.Log("GamePotEventListener::onPushStatusSuccess()");

        if (cbPushStatusEnable != null)
        {
            cbPushStatusEnable(true);
            cbPushStatusEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
                GamePotInterface.onPushStatusSuccess();
        }
    }

    public void onPushStatusFailure(string result)
    {
        Debug.Log("GamePotEventListener::onPushStatusFailure()" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbPushStatusEnable != null)
        {
            cbPushStatusEnable(false, error);
            cbPushStatusEnable = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onPushStatusFailure(error);
            }
        }
    }

    public void onAgreeDialogSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onAgreeDialogSuccess() - " + result);
        NAgreeResultInfo resultInfo = JsonMapper.ToObject<NAgreeResultInfo>(result);

        if (cbShowAgree != null)
        {
            cbShowAgree(true, resultInfo);
            cbShowAgree = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onAgreeDialogSuccess(resultInfo);
            }
        }
    }

    public void onAgreeDialogFailure(string result)
    {
        Debug.Log("GamePotEventListener::onAgreeDialogFailure()" + result);
        NError error = JsonMapper.ToObject<NError>(result);

        if (cbShowAgree != null)
        {
            cbShowAgree(false, null, error);
            cbShowAgree = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onAgreeDialogFailure(error);
            }
        }
    }

    public void onReceiveScheme(string scheme)
    {
        Debug.Log("GamePotEventListener::onReceiveScheme()" + scheme);
        if (GamePotInterface != null)
        {
            GamePotInterface.onReceiveScheme(scheme);
        }
    }

    public void onLoadAchievementSuccess(string result)
    {
        Debug.Log("GamePotEventListener::onLoadAchievementSuccess()" + result);
        if (GamePotInterface != null)
        {
            List<NAchievementInfo> resultInfo = JsonMapper.ToObject<List<NAchievementInfo>>(result);
            GamePotInterface.onLoadAchievementSuccess(resultInfo);
        }
    }

    public void onLoadAchievementFailure(string result)
    {
        Debug.Log("GamePotEventListener::onLoadAchievementFailure()" + result);
        if (GamePotInterface != null)
        {
            NError error = JsonMapper.ToObject<NError>(result);
            GamePotInterface.onLoadAchievementFailure(error);
        }
    }

    public void onLoadAchievementCancel()
    {
        Debug.Log("GamePotEventListener::onLoadAchievementCancel()");
        if (GamePotInterface != null)
            GamePotInterface.onLoadAchievementCancel();
    }

    public void onWebviewClose(string result)
    {
        Debug.Log("GamePotEventListener::onWebviewClose()" + result);
        if (cbShowWebView != null)
        {
            cbShowWebView(result);
            cbShowWebView = null;
        }
        else
        {
            if (GamePotInterface != null)
            {
                GamePotInterface.onWebviewClose(result);
            }
        }
    }

}
