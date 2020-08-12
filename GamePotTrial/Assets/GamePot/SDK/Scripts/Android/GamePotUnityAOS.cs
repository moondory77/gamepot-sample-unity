using UnityEngine;

namespace GamePotUnityAOS
{
    public class GamePotUnityPluginAOS
    {
        private static bool _initialized = false;
        private static AndroidJavaClass mGamePotClass = null;

        //Native Android Class Name
        protected const string GAMEPOT_CLASS_NAME = "io.gamepot.unity.plugin.GamePotUnityPlugin";

        public static void initPlugin()
        {
            if (GamePotEventListener.s_instance != null) return;

#if UNITY_ANDROID
            {
                // NATIVE LIBRARY
                try
                {
                    mGamePotClass = new AndroidJavaClass(GAMEPOT_CLASS_NAME + "$SDK");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex);
                }

                if (mGamePotClass != null)
                {
                    _initialized = true;
                }
                else
                {
                    Debug.LogError("GamePotUnityPluginAOS  FAIL!!!");
                }
            }
#endif

            if (GamePotEventListener.s_instance == null)
            {
                Debug.Log("GamePot - Creating GamePot Android Native Bridge Receiver");
                new GameObject("GamePotAndroidManager", typeof(GamePotEventListener));
            }
        }


        public static void setListener(IGamePot inter)
        {
            Debug.Log("GamePotUnityPluginAOS::setListener");
            if (GamePotEventListener.s_instance != null)
            {
                GamePotEventListener.s_instance.setListener(inter);
            }
            else
            {
                Debug.LogError("GamePot - GamePotEventListener instance is NULL");
            }
        }

        //////////////////////
        // Common API
        //////////////////////
        public static void setLanguage(int gameLanguage)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setLanguage", gameLanguage);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setLanguage not supported");
            }
#endif
        }

        //////////////////////
        // Channel API
        //////////////////////
        public static int sendLocalPush(System.DateTime sendDate, string title, string message)
        {
#if UNITY_ANDROID
            {
                int result = -1;
                if (mGamePotClass != null)
                    result = mGamePotClass.CallStatic<int>("sendLocalPush", sendDate.ToString("yyyy-MM-dd HH:mm:ss"), title, message);
                return result;
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLocalPush always return -1");
                return -1;
            }
#else
            return -1;
#endif
        }

        public static void cancelLocalPush(int pushId)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic<bool>("cancelLocalPush", pushId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR cancelLocalPush not supported");
                return;
            }
#endif
        }

        public static string getLanguage()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getLanguage");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getLanguage always returns empty string");
                return "";
            }
#else
            return "";
#endif
        }

        public static string getLastLoginType()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getLastLoginType");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getLastLoginType always returns NONE");
                return "";
            }
#else
            return "";
#endif
        }

        public static void login(NCommon.LoginType loginType)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("login", loginType.ToString());
            }
#elif UNITY_EDITOR
            {
                GamePotEventListener listener = GamePotEventListener.s_instance;
                if (loginType == NCommon.LoginType.GUEST)
                {
                    Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK login with GUEST. It does not communicate with server.");
                    // Temporary user info for UnityEditor Development mode
                    if (listener != null)
                    {
                        NUserInfo userInfo = new NUserInfo
                        {
                            memberid = "UE-" + SystemInfo.deviceUniqueIdentifier,
                            userid = "UE-" + SystemInfo.deviceUniqueIdentifier,
                            name = "UnityEditor",
                            token = "UnityEditorTempToken"
                        };

                        GamePotSettings.MemberInfo = userInfo;
                        listener.onLoginSuccess(userInfo.ToJson());
                    }
                    else
                    {
                        Debug.LogError("GamePot UnityEditor listener is NULL");
                    }
                }
                else
                {
                    Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK login cancelled");
                    listener.onLoginCancel();
                }
            }
#endif
        }

        public static void deleteMember()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("deleteMember");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK deleteMember always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onDeleteMemberSuccess();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void logout()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("logout");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK logout always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onLogoutSuccess("");
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static string getConfig(string key)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getConfig", key);
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getConfig always returns empty");
                return "";
            }
#else
            return "";
#endif
        }

        public static string getConfigs()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getConfigs");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getConfigs always returns empty");
                return "";
            }
#else
            return "";
#endif
        }

        public static void coupon(string couponNumber, string userData)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("coupon", couponNumber, userData);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK coupon always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onCouponSuccess("");
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void purchase(string productId, string uniqueId, string serverId, string playerId, string etc)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("purchase", productId, uniqueId, serverId, playerId, etc);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK purchase always cancelled");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onPurchaseCancel();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static string getPurchaseItems()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getPurchaseItems");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getPurchaseItems always returns empty");
                return "";
            }
#else
        return "";
#endif
        }

        public static bool isLinked(string linkType)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<bool>("isLinked", linkType);
                }
                return false;
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR always returns isLinked false");
                return false;
            }
#else
            return false;      
#endif
        }

        public static string getLinkedList()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getLinkedList");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR always returns empty linked list");
                return "";
            }
#else
            return "";
#endif
        }

        public static void createLinking(NCommon.LinkingType linkType)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("createLinking", linkType.ToString());
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK createLinking always cancelled");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onCreateLinkingCancel("");
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void deleteLinking(NCommon.LinkingType linkType)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("deleteLinking", linkType.ToString());
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK deleteLinking always faulure");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onDeleteLinkingFailure("");
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void addChannel(NCommon.ChannelType channelType)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("addChannel", channelType.ToString());
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR addChannel not supported");
            }
#endif
        }

        public static void setPush(bool pushEnable)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setPush", pushEnable);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK setPush always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onPushSuccess();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void setPushNight(bool pushEnable)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setPushNightStatus", pushEnable);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK setPushNight always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onPushNightSuccess();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void setPushAd(bool pushEnable)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setPushAdStatus", pushEnable);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK setPushAd always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onPushAdSuccess();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void setPushState(bool pushEnable, bool nightPushEnable, bool adPushEnable)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setPushStatus", pushEnable, nightPushEnable, adPushEnable);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK setPushState always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    GamePotEventListener.s_instance.onPushStatusSuccess();
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static string getPushStatus()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getPushStatus");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getPushStatus always returns empty");
                return "";
            }
#else
		return "";
#endif
        }

        public static void showNoticeWebView()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showNoticeWebView");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNoticeWebView not supported");
            }
#endif
        }

        public static void showWebView(string url)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showWebView", url);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showWebView not supported");
            }
#endif
        }

        public static void showCSWebView()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showCSWebView");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showCSWebView not supported");
            }
#endif
        }

        public static void showAppStatusPopup(string status)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showAppStatusPopup", status);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showAppStatusPopup not supported");
            }
#endif
        }

        public static void showAgreeDialog(string info)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showAgreeDialog", info);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK showAgreeDialog always success");
                // Temporary user info for UnityEditor Development mode
                if (GamePotEventListener.s_instance != null)
                {
                    string successResultJson = "{\"agree\":true,\"agreeNight\":true}";
                    GamePotEventListener.s_instance.onAgreeDialogSuccess(successResultJson);
                }
                else
                {
                    Debug.LogError("GamePot UnityEditor listener is NULL");
                }
            }
#endif
        }

        public static void setVoidBuilder(string info)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setVoidBuilder", info);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setVoidBuilder not supported");
            }
#endif
        }

        public static void showTerms()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showTerms");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showTerms not supported");
            }
#endif
        }

        public static void showPrivacy()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showPrivacy");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showPrivacy not supported");
            }
#endif
        }

        public static void showNotice()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showNotice");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNotice not supported");
            }
#endif
        }

        public static void showNotice(bool showTodayButton)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showNotice", showTodayButton);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNotice not supported");
            }
#endif
        }

        public static void showFaq()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showFaq");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showFaq not supported");
            }
#endif
        }

        public static void setLoggerUserid(string userid)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("setLoggerUserid", userid);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setLoggerUserid not supported");
            }
#endif
        }

        public static void sendLog(string type, string errCode, string errMessage)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("sendLog", type, errCode, errMessage);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void showAchievement()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showAchievement");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showAchievement not supported");
            }
#endif
        }

        public static void showLeaderboard()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showLeaderboard");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showLeaderboard not supported");
            }
#endif
        }

        public static void unlockAchievement(string achievementId)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("unlockAchievement", achievementId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR unlockAchievement not supported");
            }
#endif
        }

        public static void incrementAchievement(string achievementId, string count)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("incrementAchievement", achievementId, count);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR incrementAchievement not supported");
            }
#endif
        }

        public static void submitScoreLeaderboard(string leaderBoardId, string leaderBoardScore)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("submitScoreLeaderboard", leaderBoardId, leaderBoardScore);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR submitScoreLeaderboard not supported");
            }
#endif
        }

        public static void loadAchievement()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("loadAchievement");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR loadAchievement not supported");
            }
#endif
        }

        public static void purchaseThirdPayments(string productId, string uniqueId)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("purchaseThirdPayments", productId, uniqueId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR purchaseThirdPayments not supported");
            }
#endif
        }

        public static string getPurchaseThirdPaymentsItems()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getPurchaseThirdPaymentsItems");
                }
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getPurchaseThirdPaymentsItems always returns empty");
                return "";
            }
#else
		return "";
#endif
        }

        public static bool characterInfo(string data)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<bool>("characterInfo", data);
                }
                return false;
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR characterInfo always returns false");
                return false;
            }
#else
		return false;
#endif
        }

        public static string getFCMToken()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<string>("getFCMToken");
                }
                Debug.Log("GamePot -  GamePotUnityPluginAOS not initialized!!");
                return "";
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getFCMToken always returns empty string");
                return "";
            }
#else
		return "";
#endif
        }

        public static void showRefund()
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("showRefund");
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showRefund not supported");
            }
#endif
        }

        public static void sendPurchaseByThirdPartySDK(string productId, string transactionId, string currency, double price, string store, string paymentId, string uniqueId)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("sendPurchaseByThirdPartySDK", productId, transactionId, currency, price, store, paymentId, uniqueId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendPurchaseByThirdPartySDK not supported");
            }
#endif
        }

        public static void loginByThirdPartySDK(string userId)
        {
#if UNITY_ANDROID
            {
                if (mGamePotClass != null)
                    mGamePotClass.CallStatic("loginByThirdPartySDK", userId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR loginByThirdPartySDK not supported");
            }
#endif
        }

        public static bool isUseLibrary(string libraryName)
        {
#if UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR isUseLibrary not supported");
                return false;
            }
#elif UNITY_ANDROID
            {
                if (mGamePotClass != null)
                {
                    return mGamePotClass.CallStatic<bool>("isUseLibrary", libraryName);
                }
                else
                { 
                    return false;
                }
            }
#endif
        }

    }
}