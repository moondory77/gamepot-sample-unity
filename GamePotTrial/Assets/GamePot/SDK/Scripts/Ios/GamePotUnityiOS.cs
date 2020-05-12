using UnityEngine;
using System.Runtime.InteropServices;
using System;
using GamePotUnity;

#if UNITY_IOS

namespace GamePotUnityiOS
{

    public class GamePotUnityPluginiOS
    {
        #region iOS DLL native functions
        [DllImport("__Internal")]
        private static extern void pluginVersionByUnity(String version);

        [DllImport("__Internal")]
        private static extern void joinChannelByUnity(String prevChannel);

        [DllImport("__Internal")]
        private static extern void leaveChannelByUnity(String prevChannel);

        [DllImport("__Internal")]
        private static extern void sendMessageByUnity(String prevChannel, String message);

        [DllImport("__Internal")]
        private static extern string getLastLoginTypeByUnity();

        [DllImport("__Internal")]
        private static extern void loginByUnity(string loginType);

        [DllImport("__Internal")]
        private static extern void deleteMemberByUnity();

        [DllImport("__Internal")]
        private static extern string getConfigByUnity(String key);

        [DllImport("__Internal")]
        private static extern string getConfigsByUnity();

        [DllImport("__Internal")]
        private static extern void purchaseByUnity(String productId, String uniqueId, String serverId, String playerId, String etc);

        [DllImport("__Internal")]
        private static extern void couponByUnity(String couponNumber, String userData);

        [DllImport("__Internal")]
        private static extern void setLanguageByUnity(int gameLanguage);

        [DllImport("__Internal")]
        private static extern int sendLocalPushByUnity(string sdate, string title, string text);

        [DllImport("__Internal")]
        private static extern void cancelLocalPushByUnity(int pushId);

        [DllImport("__Internal")]
        private static extern string getLinkedListByUnity();

        [DllImport("__Internal")]
        private static extern bool isLinkedByUnity(string linkType);

        [DllImport("__Internal")]
        private static extern void logoutByUnity();

        [DllImport("__Internal")]
        private static extern void createLinkingByUnity(string linkType);

        [DllImport("__Internal")]
        private static extern void deleteLinkingByUnity(string linkType);

        [DllImport("__Internal")]
        private static extern void addChannelByUnity(string linkType);

        [DllImport("__Internal")]
        private static extern void enableGameCenterByUnity(bool enable);

        [DllImport("__Internal")]
        private static extern string getPurchaseItemByUnity();

        [DllImport("__Internal")]
        private static extern void setPushByUnity(bool pushEnable);

        [DllImport("__Internal")]
        private static extern void setPushNightByUnity(bool pushEnable);

        [DllImport("__Internal")]
        private static extern void setPushAdByUnity(bool pushEnable);

        [DllImport("__Internal")]
        private static extern void setPushStateByUnity(bool pushEnable, bool nightPushEnable, bool adPushEnable);

        [DllImport("__Internal")]
        private static extern string getPushStatusByUnity();

        [DllImport("__Internal")]
        private static extern void showNoticeWebViewByUnity();

        [DllImport("__Internal")]
        private static extern void showCSWebViewByUnity();

        [DllImport("__Internal")]
        private static extern void showWebViewByUnity(string url);

        [DllImport("__Internal")]
        private static extern void showAppStatusPopupByUnity(string status);

        [DllImport("__Internal")]
        private static extern void showAgreeDialogByUnity(string info);

        [DllImport("__Internal")]
        private static extern void showTermsByUnity();

        [DllImport("__Internal")]
        private static extern void showPrivacyByUnity();

        [DllImport("__Internal")]
        private static extern void sendLogByUnity(string type, string errCode, string errMessage);

        [DllImport("__Internal")]
        private static extern void setLoggerUseridByUnity(string userid);

        [DllImport("__Internal")]
        private static extern void showNoticeByUnity();

        [DllImport("__Internal")]
        private static extern void showNoticeWithFlagByUnity(bool showTodayButton);

        [DllImport("__Internal")]
        private static extern void showFaqByUnity();

        [DllImport("__Internal")]
        private static extern bool characterInfoByUnity(string info);

        [DllImport("__Internal")]
        private static extern string getPushTokenByUnity();

        [DllImport("__Internal")]
        private static extern void showRefundByUnity();

        [DllImport("__Internal")]
        private static extern void sendPurchaseByThirdPartySDKByUnity(string productId, string transactionId, string currency, double price, string store, string paymentId, string uniqueId);

        [DllImport("__Internal")]
        private static extern void loginByThirdPartySDKByUnity(string userId);
        #endregion


        public static void initPlugin()
        {
            if (GamePotEventListener.s_instance != null) return;

            Debug.Log("GamePot - Creating GamePot iOS Native Bridge Receiver");
            new GameObject("GamePotiOSManager", typeof(GamePotEventListener));
            pluginVersion(GamePot.UnityPluginVersion);

        }

        public static void setListener(IGamePot inter)
        {
            Debug.Log("GamePotEventListener::setListener");
            if (GamePotEventListener.s_instance != null)
            {
                GamePotEventListener.s_instance.setListener(inter);
            }
            else
            {
                Debug.LogError("GamePot - GamePotEventListener instance is NULL");
            }
        }

        public static void pluginVersion(String version)
        {
#if UNITY_IOS
                pluginVersionByUnity(version);
#else
            {
                Debug.Log("GamePot -  pluginVersion not supported");
            }
#endif
        }

        //////////////////////
        // Common API
        //////////////////////

        //////////////////////
        // Chat API
        //////////////////////

        public static void joinChannel(String prevChannel)
        {
#if UNITY_IOS
                joinChannelByUnity(prevChannel);
#else
            {
                Debug.Log("GamePot -  UNITY EDITOR joinChannel not supported");
            }
#endif
        }

        public static void leaveChannel(String prevChannel)
        {
#if UNITY_IOS
                leaveChannelByUnity(prevChannel);
#else
            {
                Debug.Log("GamePot - UNITY EDITOR leaveChannel not supported");
            }
#endif
        }

        public static void sendMessage(String prevChannel, String message)
        {
#if UNITY_IOS
            {
                Debug.Log("chatMessage - prevChannel :  " + prevChannel + " / message : " + message);
                sendMessageByUnity(prevChannel, message);
            }
#else
            {
                Debug.Log("GamePot - UNITY EDITOR sendMessage not supported");
            }
#endif
        }

        public static string getLastLoginType()
        {
#if UNITY_IOS
            {
                return getLastLoginTypeByUnity();
            }
#else
            {
                Debug.Log("GamePot - UNITY EDITOR getLastLoginType always returns NONE");
                return "";
            }
#endif
        }

        public static void login(NCommon.LoginType loginType)
        {
#if UNITY_IOS
            {
                loginByUnity(loginType.ToString());
            }
#else
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
#if UNITY_IOS
            {
                deleteMemberByUnity();
            }
#else
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

        public static string getConfig(String key)
        {
#if UNITY_IOS
            {
                return getConfigByUnity(key);
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
#if UNITY_IOS
            {
                return getConfigsByUnity();
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

        public static void purchase(String productId, String uniqueId, String serverId, String playerId, String etc)
        {
#if UNITY_IOS
            {
                purchaseByUnity(productId, uniqueId, serverId, playerId, etc);
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

        public static void coupon(String couponNumber, String userData)
        {
#if UNITY_IOS
            {
                couponByUnity(couponNumber, userData);
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

        public static void setLanguage(int gameLanguage)
        {
#if UNITY_IOS
            {
                setLanguageByUnity(gameLanguage);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setLanguage not supported");
            }
#endif
        }

        public static int sendLocalPush(string sdate, string title, string text)
        {
#if UNITY_IOS
            {
                return sendLocalPushByUnity(sdate, title, text);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sending local push not supported");
                return -1;
            }
#else
		return -1;
#endif
        }

        public static void cancelLocalPush(int pushId)
        {
#if UNITY_IOS
            {
                cancelLocalPushByUnity(pushId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR cancelLocalPush not supported");
                return;
            }
#endif
        }




        public static string getLinkedList()
        {
#if UNITY_IOS
            {
                return getLinkedListByUnity();
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

        public static bool isLinked(string linkType)
        {
#if UNITY_IOS
            {
                return isLinkedByUnity(linkType);
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

        public static void logout()
        {
#if UNITY_IOS
            {
                logoutByUnity();
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

        public static void createLinking(NCommon.LinkingType linkingType)
        {
#if UNITY_IOS
            {
                createLinkingByUnity(linkingType.ToString());
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

        public static void deleteLinking(NCommon.LinkingType linkingType)
        {
#if UNITY_IOS
            {
                deleteLinkingByUnity(linkingType.ToString());
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR DUMMY CALLBACK deleteLinking always failure");
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
#if UNITY_IOS
            {
                addChannelByUnity(channelType.ToString());
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR addChannel not supported");
            }
#endif
        }

        public static void enableGameCenter(bool enable)
        {
#if UNITY_IOS
            {
                enableGameCenterByUnity(enable);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR enableGameCenter not supported");
            }
#endif
        }

        public static string getPurchaseItems()
        {
#if UNITY_IOS
            {
                return getPurchaseItemByUnity();
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

        // TODO: push
        public static void setPush(bool pushEnable)
        {
#if UNITY_IOS
            {
                setPushByUnity(pushEnable);
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
#if UNITY_IOS
            {
                setPushNightByUnity(pushEnable);
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
#if UNITY_IOS
            {
                setPushAdByUnity(pushEnable);
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
#if UNITY_IOS
            {
                setPushStateByUnity(pushEnable, nightPushEnable, adPushEnable);
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
#if UNITY_IOS
            {
                return getPushStatusByUnity();
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
#if UNITY_IOS
            {
                showNoticeWebViewByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNoticeWebView not supported");
            }
#endif
        }

        public static void showCSWebView()
        {
#if UNITY_IOS
            {
                showCSWebViewByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showCSWebView not supported");
            }
#endif
        }

        public static void showWebView(string url)
        {
#if UNITY_IOS
            {
                showWebViewByUnity(url);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showWebView not supported");
            }
#endif
        }

        public static void showAppStatusPopup(string status)
        {
#if UNITY_IOS
            {
                showAppStatusPopupByUnity(status);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showAppStatusPopup not supported");
            }
#endif
        }

        public static void showAgreeDialog(string info)
        {
#if UNITY_IOS
            {
                showAgreeDialogByUnity(info);
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

        public static void showTerms()
        {
#if UNITY_IOS
            {
                showTermsByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showTerms not supported");
            }
#endif
        }

        public static void showPrivacy()
        {
#if UNITY_IOS
            {
                showPrivacyByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showPrivacy not supported");
            }
#endif
        }

        public static void sendLog(string type, string errCode, string errMessage)
        {
#if UNITY_IOS
            {
                sendLogByUnity(type, errCode, errMessage);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void setLoggerUserid(string userid)
        {
#if UNITY_IOS
            {
                setLoggerUseridByUnity(userid);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setLoggerUserid not supported");
            }
#endif
        }

        public static void showNotice()
        {
#if UNITY_IOS
            {
                showNoticeByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNotice not supported");
            }
#endif
        }

        public static void showNotice(bool showTodayButton)
        {
#if UNITY_IOS
            {
                showNoticeWithFlagByUnity(showTodayButton);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNotice not supported");
            }
#endif
        }

        public static void showFaq()
        {
#if UNITY_IOS
            {
                showFaqByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showFaq not supported");
            }
#endif
        }

        public static bool characterInfo(string info)
        {
#if UNITY_IOS
            {
                return characterInfoByUnity(info);
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

        public static string getPushToken()
        {
#if UNITY_IOS
            {
                return getPushTokenByUnity();
            }
#else
            {
                Debug.Log("GamePot - UNITY EDITOR getPushToken always returns emprt string");
                return "";
            }
#endif
        }

        public static void showRefund()
        {
#if UNITY_IOS
            {
                showRefundByUnity();
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showRefund not supported");
            }
#endif
        }

        public static void sendPurchaseByThirdPartySDK(string productId, string transactionId, string currency, double price, string store, string paymentId, string uniqueId)
        {
#if UNITY_IOS
            {
                sendPurchaseByThirdPartySDKByUnity(productId, transactionId, currency, price, store, paymentId, uniqueId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showRefund not supported");
            }
#endif
        }

        public static void loginByThirdPartySDK(string userId)
        {
#if UNITY_IOS
            {
                loginByThirdPartySDKByUnity(userId);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showRefund not supported");
            }
#endif
        }
    }
    
}
#endif
