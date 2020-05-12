using System.Collections.Generic;
using UnityEngine;
using Realtime.LITJson;
using System;

#if UNITY_IOS
using GamePotUnityiOS;
#elif UNITY_ANDROID
using GamePotUnityAOS;
#endif

namespace GamePotUnity
{
    public class GamePot
    {
        // 2020.2.3 Added support delegate callback function (2.0.3)
        // 2020.3.26 V2.1.0 released
        // 2020.3.31 ShowWebView (with callback)

        public static string DefaultProjectId = "fbee0f1c-3329-421f-b966-0385a0dbf515";
        public static string UnityPluginVersion = "2.1.0";

        public static NCommon.LoginType getLastLoginType()
        {
            string result = "";

#if UNITY_IOS
                result = GamePotUnityPluginiOS.getLastLoginType();
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getLastLoginType();
#elif UNITY_EDITOR
            Debug.Log("GamePot - UNITY EDITOR getLastLoginType always returns NONE");
#endif

            switch (result)
            {
                case "GOOGLE":
                    return NCommon.LoginType.GOOGLE;
                case "GOOGLEPLAY":
                    return NCommon.LoginType.GOOGLEPLAY;
                case "FACEBOOK":
                    return NCommon.LoginType.FACEBOOK;
                case "NAVER":
                    return NCommon.LoginType.NAVER;
                case "GAMECENTER":
                    return NCommon.LoginType.GAMECENTER;
                case "TWITTER":
                    return NCommon.LoginType.TWITTER;
                case "LINE":
                    return NCommon.LoginType.LINE;
                case "APPLE":
                    return NCommon.LoginType.APPLE;
                case "GUEST":
                    return NCommon.LoginType.GUEST;
                case "THIRDPARTYSDK":
                    return NCommon.LoginType.THIRDPARTYSDK;
            }

            return NCommon.LoginType.NONE;
        }

        public static string getMemberId()
        {
            if (GamePotSettings.MemberInfo == null)
                return "";

            return GamePotSettings.MemberInfo.memberid;
        }

        public static string getMemberName()
        {
            if (GamePotSettings.MemberInfo == null)
                return "";

            return GamePotSettings.MemberInfo.name;
        }

        public static string getMemberEmail()
        {
            if (GamePotSettings.MemberInfo == null)
                return "";

            return GamePotSettings.MemberInfo.email;
        }

        public static string getMemberProfileUrl()
        {
            if (GamePotSettings.MemberInfo == null)
                return "";

            return GamePotSettings.MemberInfo.profileUrl;
        }

        public static string getMemberSocialId()
        {
            if (GamePotSettings.MemberInfo == null)
                return "";

            return GamePotSettings.MemberInfo.userid;
        }

        public static string getLanguage()
        {
            string result = "";
#if UNITY_IOS
                result = "ko-KR";
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getLanguage();
#elif UNITY_EDITOR
            Debug.Log("GamePot - UNITY EDITOR getLanguage always returns empty string");
#endif
            return result;
        }

        public static List<NLinkingInfo> getLinkedList()
        {
            string result = "";
#if UNITY_IOS
                result = GamePotUnityPluginiOS.getLinkedList();
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getLinkedList();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR always returns empty linked list");
            }
#endif

            JsonData data = JsonMapper.ToObject(result);
            List<NLinkingInfo> itemData = new List<NLinkingInfo>();
            if (data.IsArray || data.IsObject)
            {
                foreach (JsonData item in data)
                {
                    Debug.Log("GamePot::getLinkedList-" + item["provider"]);

                    NLinkingInfo info = new NLinkingInfo();
                    if (item["provider"].ToString() == "google")
                        info.provider = NCommon.LinkingType.GOOGLE;
                    else if (item["provider"].ToString() == "facebook")
                        info.provider = NCommon.LinkingType.FACEBOOK;
                    else if (item["provider"].ToString() == "naver")
                        info.provider = NCommon.LinkingType.NAVER;
                    else if (item["provider"].ToString() == "googleplay")
                        info.provider = NCommon.LinkingType.GOOGLEPLAY;
                    else if (item["provider"].ToString() == "gamecenter")
                        info.provider = NCommon.LinkingType.GAMECENTER;
                    else if (item["provider"].ToString() == "line")
                        info.provider = NCommon.LinkingType.LINE;
                    else if (item["provider"].ToString() == "twitter")
                        info.provider = NCommon.LinkingType.TWITTER;
                    else if (item["provider"].ToString() == "apple")
                        info.provider = NCommon.LinkingType.APPLE;
                    else if (item["provider"].ToString() == "thirdpartysdk")
                        info.provider = NCommon.LinkingType.THIRDPARTYSDK;
                    itemData.Add(info);
                }
            }
            return itemData;
        }

        public static void initPlugin()
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.initPlugin();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.initPlugin();
#elif UNITY_EDITOR
            {
                if (GamePotEventListener.s_instance == null)
                {
                    Debug.Log("GamePot - Creating GamePot Native Bridge Receiver");
                    new GameObject("GamePotManager", typeof(GamePotEventListener));
                }
            }
#endif
        }

        public static void setListener(IGamePot inter)
        {
            if (GamePotEventListener.s_instance != null)
            {
                GamePotEventListener.s_instance.setListener(inter);
            }
            else
            {
                Debug.LogError("GamePotEventListener instance is null");
            }
        }

        public static int sendLocalPush(DateTime sendDate, string title, string message)
        {
#if UNITY_IOS
                return GamePotUnityPluginiOS.sendLocalPush(sendDate.ToString("yyyy-MM-dd HH:mm:ss"), title, message);
#elif UNITY_ANDROID
            return GamePotUnityPluginAOS.sendLocalPush(sendDate, title, message);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLocalPush always return -1");
            }
            return -1;
#endif
        }


        public static bool isLinked(string linkType)
        {
#if UNITY_IOS
                return GamePotUnityPluginiOS.isLinked(linkType);
#elif UNITY_ANDROID
            return GamePotUnityPluginAOS.isLinked(linkType);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR always returns isLinked false");
            }
            return false;
#endif
        }

        public static void cancelLocalPush(string pushId)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.cancelLocalPush(Int32.Parse(pushId));
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.cancelLocalPush(Int32.Parse(pushId));
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR cancelLocalPush not supported");
            }
            return;
#endif
        }

        public static void login(NCommon.LoginType loginType)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.login(loginType);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.login(loginType);
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

        /// <summary>
        /// Login (callback delegate)
        /// </summary>
        /// <param name="loginType">Login Type</param>
        /// <param name="callback"></param>
        public static void login(NCommon.LoginType loginType, GamePotCallbackDelegate.CB_Login cbLogin)
        {
            GamePotEventListener.cbLogin = cbLogin;
            login(loginType);
        }

        public static void deleteMember()
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.deleteMember();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.deleteMember();
#elif UNITY_EDITOR         
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
#endif
        }

        /// <summary>
        /// Delete Member (callback delegate)
        /// </summary>
        /// <param name="cbDeleteMember"></param>
        public static void deleteMember(GamePotCallbackDelegate.CB_Common cbDeleteMember)
        {
            GamePotEventListener.cbDeleteMember = cbDeleteMember;
            deleteMember();
        }

        public static void logout()
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.logout();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.logout();
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

        /// <summary>
        /// Logout (callback delegate)
        /// </summary>
        /// <param name="cbPurchase"></param>
        public static void logout(GamePotCallbackDelegate.CB_Common cbLogout)
        {
            GamePotEventListener.cbLogout = cbLogout;
            logout();
        }


        public static string getConfig(string key)
        {
            string result = "";
#if UNITY_IOS
                result = GamePotUnityPluginiOS.getConfig(key);
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getConfig(key);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getConfig always returns empty");
            }
#endif
            return result;
        }

        public static string getConfigs()
        {
            string result = "";
#if UNITY_IOS
                result = GamePotUnityPluginiOS.getConfigs();
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getConfigs();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getConfigs always returns empty");
            }
#endif
            return result;
        }

        public static void coupon(string couponNumber)
        {
            coupon(couponNumber, "");
        }

        public static void coupon(string couponNumber, string userData)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.coupon(couponNumber, userData);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.coupon(couponNumber, userData);
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

        /// <summary>
        /// Coupon (callback delegate)
        /// </summary>
        /// <param name="couponNumber"></param>
        /// <param name="cbCoupon">Callback function</param>
        public static void coupon(string couponNumber, GamePotCallbackDelegate.CB_Common cbCoupon)
        {
            GamePotEventListener.cbCoupon = cbCoupon;
            coupon(couponNumber, "");
        }

        /// <summary>
        /// Coupon (callback delegate)
        /// </summary>
        /// <param name="couponNumber"></param>
        /// <param name="userData"></param>
        /// <param name="cbCoupon">Callback function</param>
        public static void coupon(string couponNumber, string userData, GamePotCallbackDelegate.CB_Common cbCoupon)
        {
            GamePotEventListener.cbCoupon = cbCoupon;
            coupon(couponNumber, userData);
        }


        public static void setLanguage(NCommon.GameLanguage gameLanguage)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.setLanguage((int)gameLanguage);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setLanguage((int)gameLanguage);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setLanguage not supported");
            }
#endif
        }

        public static void purchase(string productId)
        {
            purchase(productId, "", "", "", "");
        }

        public static void purchase(string productId, string uniqueId)
        {
            purchase(productId, uniqueId, "", "", "");
        }

        public static void purchase(string productId, string uniqueId, string serverId, string playerId, string etc)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.purchase(productId, uniqueId, serverId, playerId, etc);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.purchase(productId, uniqueId, serverId, playerId, etc);
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


        /// <summary>
        /// Purchase (callback delegate)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="cbPurchase"></param>
        public static void purchase(string productId, GamePotCallbackDelegate.CB_Purchase cbPurchase)
        {
            GamePotEventListener.cbPurchase = cbPurchase;
            purchase(productId, "", "", "", "");
        }

        /// <summary>
        /// Purchase (callback delegate)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="uniqueId"></param>
        /// <param name="cbPurchase"></param>
        public static void purchase(string productId, string uniqueId, GamePotCallbackDelegate.CB_Purchase cbPurchase)
        {
            GamePotEventListener.cbPurchase = cbPurchase;
            purchase(productId, uniqueId);
        }

        /// <summary>
        /// Purchase (callback delegate)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="uniqueId"></param>
        /// <param name="serverId"></param>
        /// <param name="playerId"></param>
        /// <param name="etc"></param>
        /// <param name="cbPurchase"></param>

        public static void purchase(string productId, string uniqueId, string serverId, string playerId, string etc, GamePotCallbackDelegate.CB_Purchase cbPurchase)
        {
            GamePotEventListener.cbPurchase = cbPurchase;
            purchase(productId, uniqueId, serverId, playerId, etc);
        }



        public static NPurchaseItem[] getPurchaseItems()
        {
            string result = "";
#if UNITY_IOS
                result = GamePotUnityPluginiOS.getPurchaseItems();
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getPurchaseItems();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getPurchaseItems always returns empty");
            }
#endif
            NPurchaseItem[] itemData = JsonMapper.ToObject<NPurchaseItem[]>(result);
            return itemData;
        }


        public static void createLinking(NCommon.LinkingType linkType)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.createLinking(linkType);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.createLinking(linkType);
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

        /// <summary>
        /// Create Linking (callback delegate)
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="cbCreateLinking">Callback Function</param>
        public static void createLinking(NCommon.LinkingType linkType, GamePotCallbackDelegate.CB_CreateLinking cbCreateLinking)
        {
            GamePotEventListener.cbCreateLinking = cbCreateLinking;
            createLinking(linkType);
        }


        public static void deleteLinking(NCommon.LinkingType linkType)
        {
#if UNITY_IOS
                GamePotUnityPluginiOS.deleteLinking(linkType);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.deleteLinking(linkType);
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

        /// <summary>
        /// Delete Linking (callback delegate)
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="cbDeleteLinking">Callback Function</param>
        public static void deleteLinking(NCommon.LinkingType linkType, GamePotCallbackDelegate.CB_Common cbDeleteLinking)
        {
            GamePotEventListener.cbDeleteLinking = cbDeleteLinking;
            deleteLinking(linkType);
        }

        public static void setPushStatus(bool pushEnable)
        {
            Debug.Log("[GPUnity][Call] setPushStatus : " + pushEnable);

#if UNITY_IOS
                GamePotUnityPluginiOS.setPush(pushEnable);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setPush(pushEnable);
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

        /// <summary>
        /// Push Enable
        /// </summary>
        /// <param name="pushEnable"></param>
        /// <param name="cbPushEnable">Callback Function</param>
        public static void setPushStatus(bool pushEnable, GamePotCallbackDelegate.CB_Common cbPushEnable)
        {
            GamePotEventListener.cbPushEnable = cbPushEnable;
            setPushStatus(pushEnable);
        }

        public static void setPushNightStatus(bool nightPushEnable)
        {
            Debug.Log("[GPUnity][Call] setPushNightStatus : " + nightPushEnable);
#if UNITY_IOS
                GamePotUnityPluginiOS.setPushNight(nightPushEnable);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setPushNight(nightPushEnable);
#elif UNITY_EDITOR
            {
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

        /// <summary>
        /// Night Push Enable
        /// </summary>
        /// <param name="nightPushEnable"></param>
        /// <param name="cbPushNightEnable">Callback Function</param>
        public static void setPushNightStatus(bool nightPushEnable, GamePotCallbackDelegate.CB_Common cbPushNightEnable)
        {
            GamePotEventListener.cbPushNightEnable = cbPushNightEnable;
            setPushNightStatus(nightPushEnable);
        }

        public static void setPushADStatus(bool adPushEnable)
        {
            Debug.Log("[GPUnity][Call] setPushADStatus : " + adPushEnable);
#if UNITY_IOS
                GamePotUnityPluginiOS.setPushAd(adPushEnable);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setPushAd(adPushEnable);
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

        /// <summary>
        /// Set Push AD Status
        /// </summary>
        /// <param name="adPushEnable"></param>
        /// <param name="cbPushADEnable">Callback Function</param>
        public static void setPushADStatus(bool adPushEnable, GamePotCallbackDelegate.CB_Common cbPushADEnable)
        {
            GamePotEventListener.cbPushADEnable = cbPushADEnable;
            setPushADStatus(adPushEnable);
        }



        public static void setPushStatus(bool pushEnable, bool nightPushEnable, bool adPushEnable)
        {
            Debug.Log("[GPUnity][Call] setPush : " + pushEnable + " NightPush : " + nightPushEnable + " adPush : " + adPushEnable);
#if UNITY_IOS
                GamePotUnityPluginiOS.setPushState(pushEnable, nightPushEnable, adPushEnable);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setPushState(pushEnable, nightPushEnable, adPushEnable);
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

        /// <summary>
        /// Set Push Status at once
        /// </summary>
        /// <param name="pushEnable"></param>
        /// <param name="nightPushEnable"></param>
        /// <param name="adPushEnable"></param>
        /// <param name="cbPushStatusEnable">Callback Function</param>
        public static void setPushStatus(bool pushEnable, bool nightPushEnable, bool adPushEnable, GamePotCallbackDelegate.CB_Common cbPushStatusEnable)
        {
            GamePotEventListener.cbPushStatusEnable = cbPushStatusEnable;
            setPushStatus(pushEnable, nightPushEnable, adPushEnable);
        }


        public static NPushInfo getPushStatus()
        {
            Debug.Log("[GPUnity][Call] getPushStatus");
            string result = "";
#if UNITY_IOS
                result = GamePotUnityPluginiOS.getPushStatus();
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.getPushStatus();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR getPushStatus always returns empty");
            }
#endif
            Debug.Log("[GPUnity][Call] getPushStatus result : " + result);
            NPushInfo pushInfo = JsonMapper.ToObject<NPushInfo>(result);
            return pushInfo;
        }

        public static void showNoticeWebView()
        {
            Debug.Log("[GPUnity][Call] showNoticeWebView");
#if UNITY_IOS
                GamePotUnityPluginiOS.showNoticeWebView();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showNoticeWebView();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNoticeWebView not supported");
            }
#endif
        }

        public static void showWebView(string url, GamePotCallbackDelegate.CB_ShowWebView cbShowWebView)
        {
            GamePotEventListener.cbShowWebView = cbShowWebView;
            showWebView(url);
        }

        public static void showWebView(string url)
        {
            Debug.Log("[GPUnity][Call] showWebView url : " + url);
#if UNITY_IOS
            {
                GamePotEventListener listener = GamePotEventListener.s_instance;
                GamePotUnityPluginiOS.showWebView(url);
                // IOS doesn't return Any Callback
                listener.onWebviewClose("");
            }
#elif UNITY_ANDROID
            {
                GamePotUnityPluginAOS.showWebView(url);
            }
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showWebView not supported");
            }
#endif
        }

        public static void showCSWebView()
        {
            Debug.Log("[GPUnity][Call] showCSWebView");
#if UNITY_IOS
                GamePotUnityPluginiOS.showCSWebView();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showCSWebView();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showCSWebView not supported");
            }
#endif
        }

        public static void showAppStatusPopup(string status)
        {
            Debug.Log("[GPUnity][Call] showAppStatusPopup - " + status);
#if UNITY_IOS
                GamePotUnityPluginiOS.showAppStatusPopup(status);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showAppStatusPopup(status);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showAppStatusPopup not supported");
            }
#endif
        }

        public static void showAgreeDialog()
        {
            NAgreeInfo info = null;
            showAgreeDialog(info);
        }

        public static void showAgreeDialog(NAgreeInfo info)
        {
            Debug.Log("[GPUnity][Call] showAgreeDialog");
#if UNITY_IOS
                GamePotUnityPluginiOS.showAgreeDialog(info != null ? info.ToJson().ToString() : null);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showAgreeDialog(info != null ? info.ToJson().ToString() : null);
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

        /// <summary>
        /// Show Agree Popup
        /// </summary>
        /// <param name="cbShowAgree">Callback Function</param>
        public static void showAgreeDialog(GamePotCallbackDelegate.CB_ShowAgree cbShowAgree)
        {
            GamePotEventListener.cbShowAgree = cbShowAgree;
            showAgreeDialog(null, cbShowAgree);
        }

        /// <summary>
        /// Show Agree Popup
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cbShowAgree">Callback Function</param>
        public static void showAgreeDialog(NAgreeInfo info, GamePotCallbackDelegate.CB_ShowAgree cbShowAgree)
        {
            GamePotEventListener.cbShowAgree = cbShowAgree;
            showAgreeDialog(info);
        }



        public static void setVoidBuilder(NVoidInfo info)
        {
            Debug.Log("[GPUnity][Call] setVoidBuilder");

#if UNITY_IOS
                Debug.Log("GamePot - iOS setVoidBuilder not supported");
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setVoidBuilder(info != null ? info.ToJson().ToString() : null);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR setVoidBuilder not supported");
            }
#endif
        }

        public static void showTerms()
        {
            Debug.Log("[GPUnity][Call] showTerms");
#if UNITY_IOS
                GamePotUnityPluginiOS.showTerms();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showTerms();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showTerms not supported");
            }
#endif
        }

        public static void showPrivacy()
        {
            Debug.Log("[GPUnity][Call] showPrivacy");
#if UNITY_IOS
                GamePotUnityPluginiOS.showPrivacy();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showPrivacy();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showPrivacy not supported");
            }
#endif
        }

        public static void showNotice()
        {
            showNotice(true);
        }

        public static void showNotice(bool showTodayButton)
        {
            Debug.Log("[GPUnity][Call] showNotice");
#if UNITY_IOS
                GamePotUnityPluginiOS.showNotice(showTodayButton);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showNotice(showTodayButton);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showNotice not supported");
            }
#endif
        }

        public static void showFaq()
        {
            Debug.Log("[GPUnity][Call] showFaq");
#if UNITY_IOS
                GamePotUnityPluginiOS.showFaq();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showFaq();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showFaq not supported");
            }
#endif
        }



        public static void d(string errCode, string errMessage)
        {
            Debug.Log("[GPUnity][Call] d");
#if UNITY_IOS
                GamePotUnityPluginiOS.sendLog("d", errCode, errMessage);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.sendLog("d", errCode, errMessage);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void i(string errCode, string errMessage)
        {
            Debug.Log("[GPUnity][Call] i");
#if UNITY_IOS
                GamePotUnityPluginiOS.sendLog("i", errCode, errMessage);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.sendLog("i", errCode, errMessage);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void w(string errCode, string errMessage)
        {
            Debug.Log("[GPUnity][Call] w");
#if UNITY_IOS
                GamePotUnityPluginiOS.sendLog("w", errCode, errMessage);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.sendLog("w", errCode, errMessage);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void e(string errCode, string errMessage)
        {
            Debug.Log("[GPUnity][Call] e");
#if UNITY_IOS
                GamePotUnityPluginiOS.sendLog("e", errCode, errMessage);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.sendLog("e", errCode, errMessage);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void setLoggerUserid(string userid)
        {
            Debug.Log("[GPUnity][Call] setLoggerUserid");
#if UNITY_IOS
                GamePotUnityPluginiOS.setLoggerUserid(userid);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.setLoggerUserid(userid);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendLog not supported");
            }
#endif
        }

        public static void showAchievement()
        {
            Debug.Log("[GPUnity][Call] showAchievement");
#if UNITY_ANDROID
            GamePotUnityPluginAOS.showAchievement();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - showAchievement not supported");
            }
#endif
        }

        public static void showLeaderboard()
        {
            Debug.Log("[GPUnity][Call] showLeaderboard");

#if UNITY_ANDROID
            GamePotUnityPluginAOS.showLeaderboard();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - showLeaderboard not supported");
            }
#endif
        }

        public static void unlockAchievement(string achievementId)
        {
            Debug.Log("[GPUnity][Call] unlockAchievement - " + achievementId);
#if UNITY_ANDROID
            GamePotUnityPluginAOS.unlockAchievement(achievementId);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  unlockAchievement not supported");
            }
#endif
        }

        public static void incrementAchievement(string achievementId, string count)
        {
            Debug.Log("[GPUnity][Call] incrementAchievement - " + achievementId + ", " + count);
#if UNITY_ANDROID
            GamePotUnityPluginAOS.incrementAchievement(achievementId, count);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  incrementAchievement not supported");
            }
#endif
        }

        public static void submitScoreLeaderboard(string leaderBoardId, string leaderBoardScore)
        {
            Debug.Log("[GPUnity][Call] submitScoreLeaderboard - " + leaderBoardId + ", " + leaderBoardScore);
#if UNITY_ANDROID
            GamePotUnityPluginAOS.submitScoreLeaderboard(leaderBoardId, leaderBoardScore);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - submitScoreLeaderboard not supported");
            }
#endif
        }

        public static void loadAchievement()
        {
            Debug.Log("[GPUnity][Call] loadAchievement");
#if UNITY_ANDROID
            GamePotUnityPluginAOS.loadAchievement();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  loadAchievement not supported");
            }
#endif
        }

        public static void purchaseThirdPayments(string productId)
        {
            purchaseThirdPayments(productId, "");
        }

        public static void purchaseThirdPayments(string productId, string uniqueId)
        {
            Debug.Log("[GPUnity][Call] purchaseThirdPayments - " + productId + ", " + uniqueId);
#if UNITY_ANDROID
            GamePotUnityPluginAOS.purchaseThirdPayments(productId, uniqueId);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  purchaseThirdPayments not supported");
            }
#endif
        }

        /// <summary>
        /// Purchase Thrid Party Payments
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="cbPurchase">Callback Function</param>
        public static void purchaseThirdPayments(string productId, GamePotCallbackDelegate.CB_Purchase cbPurchase)
        {
            GamePotEventListener.cbPurchase = cbPurchase;
            purchaseThirdPayments(productId, "");
        }

        /// <summary>
        /// Purchase Thrid Party Payments
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="uniqueId"></param>
        /// <param name="cbPurchase">Callback Function</param>
        public static void purchaseThirdPayments(string productId, string uniqueId, GamePotCallbackDelegate.CB_Purchase cbPurchase)
        {
            GamePotEventListener.cbPurchase = cbPurchase;
            purchaseThirdPayments(productId, uniqueId);
        }



        public static NPurchaseItem[] getPurchaseThirdPaymentsItems()
        {
            Debug.Log("[GPUnity][Call] getPurchaseThirdPaymentsItems");
            string result = "";

#if UNITY_ANDROID
            result = GamePotUnityPluginAOS.getPurchaseThirdPaymentsItems();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  getPurchaseThirdPaymentsItems always returns empty");
            }
#endif

            NPurchaseItem[] itemData = JsonMapper.ToObject<NPurchaseItem[]>(result);
            return itemData;
        }

        public static bool characterInfo(GamePotSendLogCharacter info)
        {
            Debug.Log("[GPUnity][Call] characterInfo");
            bool result = false;

            if (info == null)
            {
                Debug.Log("GamePotSendLogCharacter is null");
                return false;
            }

#if UNITY_IOS
                result = GamePotUnityPluginiOS.characterInfo(info.toString());
#elif UNITY_ANDROID
            result = GamePotUnityPluginAOS.characterInfo(info.toString());
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - characterInfo always returns false");
            }
#endif
            return result;
        }


        public static string getPushToken()
        {
            Debug.Log("[GPUnity][Call] getFCMToken");
            string token = "";
#if UNITY_IOS
                token = GamePotUnityPluginiOS.getPushToken();
#elif UNITY_ANDROID
            token = GamePotUnityPluginAOS.getFCMToken();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot -  getFCMToken always returns empty string");
            }
#endif
            return token;
        }


        public static void showRefund()
        {
            Debug.Log("[GPUnity][Call] showRefund");
#if UNITY_IOS
                GamePotUnityPluginiOS.showRefund();
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.showRefund();
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR showRefund not supported");
            }
#endif
        }

        public static void sendPurchaseByThirdPartySDK(string productId, string transactionId, string currency, double price, string store, string paymentId, string uniqueId)
        {
            Debug.Log("[GPUnity][Call] sendPurchaseByThirdPartySDK");
#if UNITY_IOS
                GamePotUnityPluginiOS.sendPurchaseByThirdPartySDK(productId, transactionId, currency, price, store, paymentId, uniqueId);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.sendPurchaseByThirdPartySDK(productId, transactionId, currency, price, store, paymentId, uniqueId);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR sendPurchaseByThirdPartySDK not supported");
            }
#endif
        }


        public static void loginByThirdPartySDK(string userId)
        {
            Debug.Log("[GPUnity][Call] loginByThirdPartySDK");
#if UNITY_IOS
                GamePotUnityPluginiOS.loginByThirdPartySDK(userId);
#elif UNITY_ANDROID
            GamePotUnityPluginAOS.loginByThirdPartySDK(userId);
#elif UNITY_EDITOR
            {
                Debug.Log("GamePot - UNITY EDITOR loginByThirdPartySDK not supported");
            }
#endif
        }

        public static void setProjectID(string projectid)
        {
            Debug.Log("[GPUnity][Call] setProjectID");
#if UNITY_ANDROID
                GamePotUnityPluginAOS.setProjectID(projectid);
#else
            {
                Debug.Log("GamePot -  setProjectID not supported");
            }
#endif
        }

        public static string getProjectID()
        {
            Debug.Log("[GPUnity][Call] getProjectID");
            string result = "";

#if UNITY_ANDROID
                result = GamePotUnityPluginAOS.getProjectID();
#else
            {
                Debug.Log("GamePot - getProjectID always returns false");
            }
#endif
            return result;
        }
    }
}