﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

using GamePotUnity;

namespace GamePotSample
{
    public class Login : MonoBehaviour, IGamePot
    {
        [SerializeField]
        private GameObject loginButtonContainer;

        [SerializeField]
        private Text facebookButtonText;

        [SerializeField]
        private Text googleButtonText;

        [SerializeField]
        private Text guestButtonText;

        [SerializeField]
        private GameObject bi;

        [SerializeField]
        public GameObject popupRoot;

        // [SerializeField]
        // private Text projectIdText;

        [SerializeField]
        private InputField projectidInput;

        private void Awake()
        {
            GamePot.initPlugin();
            GamePot.setListener(this);
        }

        private void Start()
        {
            ActiveLoginButton(false);
            projectidInput.text = GamePot.getProjectID();
            
            GameObject btn_containter =  transform.Find("LoginButtonContainer").gameObject;
            foreach (Transform child in btn_containter.transform)
            {
                if (child.name != "GuestButton")
                {
                    Button btn = child.GetComponentInChildren<Button>();
                    if (btn != null)
                    {
                        btn.interactable = false;
                    }
                }
            }
            ActiveLoginButton(true);

            // Auto Login - Removed for Trial
            // NCommon.LoginType type = GamePot.getLastLoginType();
            // if (type != NCommon.LoginType.NONE && type != NCommon.LoginType.THIRDPARTYSDK)
            // {
            //     GamePot.login(type);
            // }
            // else
            // {
            //     ActiveLoginButton(true);
            // }
        }

        #region UIButton.Click
        public void ClickAppleIDLoginButton()
        {
            GamePot.login(NCommon.LoginType.APPLE);
        }

        public void ClickGameCenterLoginButton()
        {
            GamePot.login(NCommon.LoginType.GAMECENTER);
        }

        public void ClickGoogleLoginButton()
        {
            GamePot.login(NCommon.LoginType.GOOGLE);
        }

        public void ClickFacebookLoginButton()
        {
            GamePot.login(NCommon.LoginType.FACEBOOK);
        }

        public void ClickNaverLoginButton()
        {
            GamePot.login(NCommon.LoginType.NAVER);
        }

        public void ClickLineLoginButton()
        {
            GamePot.login(NCommon.LoginType.LINE);
        }

        public void ClickTwitterLoginButton()
        {
            GamePot.login(NCommon.LoginType.TWITTER);
        }

        public void ClickGuestLoginButton()
        {
            GamePot.login(NCommon.LoginType.GUEST);
        }

        public void Click3rdPartyLoginButton(InputField input)
        {
            GamePot.loginByThirdPartySDK(input.text);
        }

        public void ClickAgreeButton()
        {
            NAgreeInfo info = new NAgreeInfo();
            info.theme = "green";
            info.showNightPush = true;
            GamePot.showAgreeDialog(info);
        }

        public void ClickProjectIDButton(InputField input)
        {
            string result = input.text;
            projectidInput.text = result;
            GamePot.setProjectID(result);
            PopupManager.ShowCommonPopup(popupRoot, PopupManager.CommonPopupType.SMALL_SIZE, "ProjectID", "변경된 Project ID : " + result + "\n\n 앱을 재시작 해주세요.", "확인", () => { Application.Quit(); });
        }

        public void ClickProjectResetButton()
        {
            projectidInput.text = GamePot.DefaultProjectId;
            ClickProjectIDButton(projectidInput);
        }

        #endregion

        private void ActiveLoginButton(bool isActive)
        {
            bi.SetActive(isActive);
            loginButtonContainer.SetActive(isActive);
        }

        // GamePot Interface
        public void onAppClose()
        {
            Application.Quit();
        }

        public void onNeedUpdate(NAppStatus status)
        {
            GamePot.showAppStatusPopup(status.ToJson());
        }

        public void onMainternance(NAppStatus status)
        {
            GamePot.showAppStatusPopup(status.ToJson());
        }

        public void onLoginCancel()
        {
            PopupManager.ShowCommonPopup(popupRoot, PopupManager.CommonPopupType.SMALL_SIZE, "로그인", "로그인이 취소 되었습니다.", "확인", () => { });
        }

        public void onLoginSuccess(NUserInfo userInfo)
        {
            GamePotSettings.MemberInfo = userInfo;
            SceneManager.LoadSceneAsync("main");
        }

        public void onLoginFailure(NError error)
        {
            PopupManager.ShowCommonPopup(popupRoot, PopupManager.CommonPopupType.SMALL_SIZE, "로그인", error.message, "확인", () => { });
        }

        public void onDeleteMemberSuccess()
        {
        }

        public void onDeleteMemberFailure(NError error)
        {
        }

        public void onLogoutSuccess()
        {
        }

        public void onLogoutFailure(NError error)
        {
        }

        public void onCouponSuccess()
        {
        }

        public void onCouponFailure(NError error)
        {
        }

        public void onPurchaseSuccess(NPurchaseInfo purchaseInfo)
        {
        }

        public void onPurchaseFailure(NError error)
        {
        }

        public void onPurchaseCancel()
        {
        }

        public void onCreateLinkingSuccess(NUserInfo userInfo)
        {
        }

        public void onCreateLinkingFailure(NError error)
        {
        }

        public void onCreateLinkingCancel()
        {
        }

        public void onDeleteLinkingSuccess()
        {
        }

        public void onDeleteLinkingFailure(NError error)
        {
        }

        public void onPushSuccess()
        {
        }

        public void onPushFailure(NError error)
        {
        }

        public void onPushNightSuccess()
        {
        }

        public void onPushNightFailure(NError error)
        {
        }

        public void onPushAdSuccess()
        {
        }

        public void onPushAdFailure(NError error)
        {
        }

        public void onPushStatusSuccess()
        {
        }

        public void onPushStatusFailure(NError error)
        {
        }

        public void onAgreeDialogSuccess(NAgreeResultInfo info)
        {
            Debug.Log("onAgreeDialogSuccess - " + info.agree + ", " + info.agreeNight);
        }

        public void onAgreeDialogFailure(NError error)
        {
            Debug.Log("onAgreeDialogFailure - " + error);
        }

        public void onReceiveScheme(string scheme)
        {
        }

        public void onLoadAchievementSuccess(List<NAchievementInfo> info)
        {
        }

        public void onLoadAchievementFailure(NError error)
        {
        }

        public void onLoadAchievementCancel()
        {
        }

        public void onWebviewClose(string result)
        {
            Debug.Log("gamepot webview return : " + result);
        }

        public void onResetForChangedProject(string projectid)
        {
            PopupManager.ShowCommonPopup(popupRoot, PopupManager.CommonPopupType.SMALL_SIZE, "start with", "start with project id : " + projectid, "확인", () => { });
        }

    }
}