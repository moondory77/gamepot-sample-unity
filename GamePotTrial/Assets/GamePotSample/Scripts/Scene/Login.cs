using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using GamePotUnity;

namespace GamePotSample
{
    public class Login : MonoBehaviour, IGamePot
    {
        [SerializeField]
        private GameObject rootContainer;

        [SerializeField]
        private GameObject loginButtonContainer;

        [SerializeField]
        private Text facebookButtonText;

        [SerializeField]
        private Text googleButtonText;

        [SerializeField]
        private Text guestButtonText;

        [SerializeField]
        public GameObject popupRoot;

        private Dictionary<string, string> libDictionary;

        private void Awake()
        {
            GamePot.initPlugin();
            GamePot.setListener(this);
        }

        private void Start()
        {

            ActiveLoginButton(false);
            CheckLibraryImported();
            ActiveLoginButton(true);

            //! Auto Login - Removed for Trial
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


        private void CheckLibraryImported()
        {
            foreach (Transform child in loginButtonContainer.transform)
            {
                GameObject each_btn = child.gameObject;
                if (LibraryInfo.libDictionary().ContainsKey(each_btn.name))
                {
                    string lib_value = LibraryInfo.libDictionary()[each_btn.name];
                    if (GamePot.isUseLibrary(lib_value) || each_btn.name.Equals("GamePotGuest"))
                    {
                        SwitchLoginButton(each_btn, true);
                        continue;
                    }
                }
                SwitchLoginButton(each_btn, false);
            }
        }

        private void SwitchLoginButton(GameObject loginButton, bool flag)
        {
            float opacity = flag ? 1.0f : 0.3f;
            var image = loginButton.transform.Find("Image").GetComponent<Image>();
            if (image != null)
            {
                var tmp_color = image.color;
                tmp_color.a = opacity;
                image.color = tmp_color;
            }

            var text = loginButton.transform.Find("Text").GetComponent<Text>();
            if (text != null)
            {
                var tmp_color = text.color;
                tmp_color.a = opacity;
                text.color = tmp_color;
            }

            var line = loginButton.transform.Find("Line").GetComponent<Image>();
            if (line != null)
            {
                var tmp_color = line.color;
                tmp_color.a = opacity;
                line.color = tmp_color;
            }

            var button = loginButton.GetComponent<Button>();
            if (button != null)
                button.interactable = flag;
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
        #endregion

        private void ActiveLoginButton(bool isActive)
        {
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
            PopupManager.ShowCustomPopup(popupRoot, "GamePotSamplePopup_BtnOne", "로그인", "로그인이 취소 되었습니다.");
        }

        public void onLoginSuccess(NUserInfo userInfo)
        {
            GamePotSettings.MemberInfo = userInfo;
            SceneManager.LoadSceneAsync("Main");
        }

        public void onLoginFailure(NError error)
        {
            PopupManager.ShowCustomPopup(popupRoot, "GamePotSamplePopup_BtnOne", "로그인", error.message);
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

    }
}