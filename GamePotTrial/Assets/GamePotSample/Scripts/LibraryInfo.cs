using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LibraryInfo
{
    private static Dictionary<string, string> _libDictionary = null;

    public static Dictionary<string, string> libDictionary()
    {
        if (_libDictionary == null)
        {
            _libDictionary = new Dictionary<string, string>();
            // _libDictionary.Add("GamePotGoogle", "io.gamepot.channel.google.signin.GamePotGoogleSignin");
            // _libDictionary.Add("GamePotFacebook", "io.gamepot.channel.facebook.GamePotFacebook");
            _libDictionary.Add("GamePotNaver", "io.gamepot.channel.naver.GamePotNaver");
            _libDictionary.Add("GamePotLine", "io.gamepot.channel.line.GamePotLine");
            _libDictionary.Add("GamePotTwitter", "io.gamepot.channel.twitter.GamePotTwitter");
            _libDictionary.Add("GamePotGuest", "");

        }
        return _libDictionary;
    }
}