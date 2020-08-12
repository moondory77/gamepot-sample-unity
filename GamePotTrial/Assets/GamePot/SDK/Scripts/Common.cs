using System.Collections;
using UnityEngine;

public class NCommon
{

    public enum LoginType
    {
        NONE,
        GOOGLE,
        GOOGLEPLAY,
        FACEBOOK,
        NAVER,
        GAMECENTER,
        TWITTER,
        LINE,
        APPLE,
        GUEST,
        THIRDPARTYSDK
    }

    public enum ChannelType
    {
        GOOGLEPLAY,
        GOOGLE,
        FACEBOOK,
        NAVER,
        GAMECENTER,
        TWITTER,
        LINE,
        APPLE,
        THIRDPARTYSDK
    }

    public enum LinkingType
    {
        GOOGLEPLAY,
        GAMECENTER,
        GOOGLE,
        FACEBOOK,
        NAVER,
        TWITTER,
        LINE,
        APPLE,
        THIRDPARTYSDK
    }

    public enum GameOrientation
    {
        portrait = 1,
        landscape = 2,
    }

    public enum GameLanguage
    {
        KOREAN = 0,
        ENGLISH = 1,
        CHINESE_CN = 2,
        CHINESE_TW = 3,
        GERMAN = 4,
        JAPANESE = 5
    }

    // -------- RESPONSE RESULT STATE --------
    public enum ResultLogin
    {
        SUCCESS,
        CANCELLED,
        FAILED,
        NEED_UPDATE,
        MAINTENANCE,
        APP_CLOSE
    }

    public enum ResultPurchase
    {
        SUCCESS,
        CANCELLED,
        FAILED
    }

    public enum ResultLinking
    {
        SUCCESS,
        CANCELLED,
        FAILED
    }

}