using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor.iOS.Xcode.Custom;
using System.Text.RegularExpressions;
using UnityEditor.iOS.Xcode.Custom.Extensions;

public static class XCodePostProcessSDK
{
	#if UNITY_EDITOR && UNITY_IOS
	[PostProcessBuildAttribute(0)]

	public static void OnPostProcessBuild (BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget != BuildTarget.iOS) {
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
		PBXProject pbxProject = new PBXProject ();
		pbxProject.ReadFromFile (projectPath);
		string targetGuid = pbxProject.TargetGuidByName ("Unity-iPhone");

		// Add Property
		pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
		pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
		pbxProject.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
		pbxProject.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
		pbxProject.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
		pbxProject.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks/**");
		pbxProject.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Pods/**");

		// Add Default Framework

		// IGAWorksCore
		AddLibToProject (pbxProject, targetGuid, "libxml2.tbd");
		pbxProject.AddFrameworkToProject(targetGuid, "iAd.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "UIKit.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "CoreText.framework", false);

		// Adbrix
		pbxProject.AddFrameworkToProject(targetGuid, "MessageUI.framework", false);

		// NaverCafe
		pbxProject.AddFrameworkToProject(targetGuid, "AVKit.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "AVFoundation.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "MediaPlayer.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "CoreMedia.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "AssetsLibrary.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "ImageIO.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "QuartzCore.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "ReplayKit.framework", true);

		// NaverCafe & IGAWorksCore
		pbxProject.AddFrameworkToProject(targetGuid, "MobileCoreServices.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);

		// NaverCafe & GamePot
		pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", false);

		// Facebook & Google SignIn
		pbxProject.AddFrameworkToProject(targetGuid, "SafariServices.framework", false);

		// Adjust
		pbxProject.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
    	pbxProject.AddFrameworkToProject(targetGuid, "UserNotifications.framework", false);
		
		// Google
    	pbxProject.AddFrameworkToProject(targetGuid, "AuthenticationServices.framework", false);
    	pbxProject.AddFrameworkToProject(targetGuid, "LocalAuthentication.framework", false);

		AddLibToProject (pbxProject, targetGuid, "libz.tbd");

        //Unity 2019.03.x 이후 버전에서의 BuildTarget 재맵핑
#if UNITY_2019_3_OR_NEWER
        targetGuid = pbxProject.TargetGuidByName("UnityFramework");

        pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        pbxProject.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        pbxProject.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        pbxProject.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        // Google
        pbxProject.AddFrameworkToProject(targetGuid, "LocalAuthentication.framework", false);
        // Facebook & Google SignIn
        pbxProject.AddFrameworkToProject(targetGuid, "SafariServices.framework", false);

        //Target 원상복귀
        targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // Add Bundle  explicitly
        AddBundle(pbxProject, targetGuid, pathToBuiltProject, "GamePot.bundle");
        AddBundle(pbxProject, targetGuid, pathToBuiltProject, "GoogleSignIn.bundle");
        AddBundle(pbxProject, targetGuid, pathToBuiltProject, "NaverAuth.bundle");
        AddBundle(pbxProject, targetGuid, pathToBuiltProject, "NaverCafeSDK.bundle");
#endif

		const string frameworkPath = "Frameworks/Plugins/IOS/Frameworks/";

		const string twitterCoreFrameworkName = "TwitterCore.framework";
		if(Directory.Exists(Application.dataPath + "/Plugins/IOS/Frameworks/"+twitterCoreFrameworkName))
		{
			PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, targetGuid, pbxProject.AddFile(frameworkPath+twitterCoreFrameworkName,frameworkPath+twitterCoreFrameworkName));
		}

		const string lineFrameworkName = "LineSDK.framework";
		if(Directory.Exists(Application.dataPath + "/Plugins/IOS/Frameworks/"+lineFrameworkName))
		{
			PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, targetGuid, pbxProject.AddFile(frameworkPath+lineFrameworkName,frameworkPath+lineFrameworkName));
		}

		const string twitterKitFrameworkName = "TwitterKit.framework";
		if(Directory.Exists(Application.dataPath + "/Plugins/IOS/Frameworks/"+twitterKitFrameworkName))
		{
			PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, targetGuid, pbxProject.AddFile(frameworkPath+twitterKitFrameworkName,frameworkPath+twitterKitFrameworkName));
		}


		const string lineObjCFrameworkName = "LineSDKObjC.framework";
		if(Directory.Exists(Application.dataPath + "/Plugins/IOS/Frameworks/"+lineObjCFrameworkName))
		{
			PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, targetGuid, pbxProject.AddFile(frameworkPath+lineObjCFrameworkName,frameworkPath+lineObjCFrameworkName));
		}

		const string naverThirdPartyLoginFrameworkName = "NaverThirdPartyLogin.framework";
		if(Directory.Exists(Application.dataPath + "/Plugins/IOS/Frameworks/"+naverThirdPartyLoginFrameworkName))
		{
			PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, targetGuid, pbxProject.AddFile(frameworkPath+naverThirdPartyLoginFrameworkName,frameworkPath+naverThirdPartyLoginFrameworkName));
		}

		pbxProject.RemoveFileFromBuild(targetGuid, "TwitterCore.framework");
		pbxProject.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");


		//GamePot Config Key
		if (File.Exists (pathToBuiltProject + "/GamePotConfig-Info.plist")) {
			File.Delete (pathToBuiltProject + "/GamePotConfig-Info.plist");
		}

		if (File.Exists (pathToBuiltProject + "/GoogleService-Info.plist")) {
			File.Delete (pathToBuiltProject + "/GoogleService-Info.plist");
		}

		File.Copy(Application.dataPath + "/Plugins/IOS/GamePotConfig-Info.plist", pathToBuiltProject + "/GamePotConfig-Info.plist");
		pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile("GamePotConfig-Info.plist", "GamePotConfig-Info.plist"));

		File.Copy(Application.dataPath + "/Plugins/IOS/GoogleService-Info.plist", pathToBuiltProject + "/GoogleService-Info.plist");
		pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));

		pbxProject.WriteToFile(projectPath);

		// Apply settings
		File.WriteAllText (projectPath, pbxProject.WriteToString ());

		// Info.plist file Setting
		var plistPath = Path.Combine (pathToBuiltProject, "Info.plist");
		var gamePotPath = Path.Combine (pathToBuiltProject, "GamePotConfig-Info.plist");
		var plist = new PlistDocument ();
		var gamePotPlist = new PlistDocument ();

		plist.ReadFromFile (plistPath);
		gamePotPlist.ReadFromFile (gamePotPath);

		PlistElementDict dict = plist.root.AsDict();
		PlistElementDict gameDict = gamePotPlist.root.AsDict();

		// Add LSApplicationQueriesSchemes
		PlistElementArray querisesSchemesArray = dict.CreateArray ("LSApplicationQueriesSchemes");
		querisesSchemesArray.AddString ("navercafe");
		querisesSchemesArray.AddString ("naversearchapp");
		querisesSchemesArray.AddString ("naversearchthirdlogin");
		querisesSchemesArray.AddString ("fbapi");
		querisesSchemesArray.AddString ("fb-messenger-share-api");
		querisesSchemesArray.AddString ("fbauth2");
		querisesSchemesArray.AddString ("fbshareextension");
		querisesSchemesArray.AddString ("lineauth2");
		querisesSchemesArray.AddString ("twitter");
		querisesSchemesArray.AddString ("twitterauth");

		// Add URL Scheme
		var array = plist.root.CreateArray ("CFBundleURLTypes");

        if (gameDict.values.ContainsKey("gamepot_naver_urlscheme") && gameDict["gamepot_naver_urlscheme"].AsString().Equals("") != true)
        {
            var urlDict = array.AddDict();
            urlDict.SetString("CFBundleURLName", "");
            urlDict.CreateArray("CFBundleURLSchemes").AddString(gameDict["gamepot_naver_urlscheme"].AsString());
        }

        if (gameDict.values.ContainsKey("gamepot_facebook_app_id") && gameDict["gamepot_facebook_app_id"].AsString().Equals("") != true)
        {
            var urlDict = array.AddDict();
            urlDict.SetString("CFBundleURLName", "");
            urlDict.CreateArray("CFBundleURLSchemes").AddString("fb" + gameDict["gamepot_facebook_app_id"].AsString());
        }

		if (gameDict.values.ContainsKey("gamepot_google_url_schemes") && gameDict ["gamepot_google_url_schemes"].AsString ().Equals ("") != true) {
			var urlDict = array.AddDict ();
			urlDict.SetString ("CFBundleURLName", "");
			urlDict.CreateArray ("CFBundleURLSchemes").AddString (gameDict ["gamepot_google_url_schemes"].AsString ());
		}

		if (gameDict.values.ContainsKey("gamepot_line_url_schemes") && gameDict ["gamepot_line_url_schemes"].AsString ().Equals ("") != true) {
			var urlDict = array.AddDict ();
			urlDict.SetString ("CFBundleURLName", "");
			urlDict.CreateArray ("CFBundleURLSchemes").AddString (gameDict ["gamepot_line_url_schemes"].AsString ());
		}

		if (gameDict.values.ContainsKey ("gamepot_twitter_consumerkey") && gameDict ["gamepot_twitter_consumerkey"].AsString ().Equals ("") != true) {
			var urlDict = array.AddDict ();
			urlDict.SetString ("CFBundleURLName", "");
			urlDict.CreateArray ("CFBundleURLSchemes").AddString ("twitterkit-" + gameDict ["gamepot_twitter_consumerkey"].AsString ());
		}

		if (gameDict.values.ContainsKey("gamepot_naver_urlscheme") && gameDict ["gamepot_naver_urlscheme"].AsString ().Equals ("") != true) {
			var urlDict = array.AddDict ();
			urlDict.SetString ("CFBundleURLName", "");
			urlDict.CreateArray ("CFBundleURLSchemes").AddString (gameDict ["gamepot_naver_urlscheme"].AsString ());
		}
		// Apply editing settings to Info.plist
		plist.WriteToFile (plistPath);
	}

	private static void AddBundle(PBXProject project, string targetGuid, string pathToBuiltProject, string bundle)
	{
		var unityPath = "/Plugins/IOS/Bundle/" + bundle;
		var fullUnityPath = Application.dataPath + unityPath;

		var bundlePath = "Bundle/" + bundle;
		var fullBundlePath = Path.Combine(pathToBuiltProject, bundlePath);

		CopyAndReplaceDirectory (fullUnityPath, fullBundlePath);

		var bundleFileGuid = project.AddFile (bundlePath, bundlePath, PBXSourceTree.Source);
		project.AddFileToBuild(targetGuid, bundleFileGuid);
	}

	static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
	{
		string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
		inst.AddFileToBuild(targetGuid, fileGuid);
	}

	private static void AddExternalFramework(PBXProject project, string targetGuid, string pathToBuiltProject, string framework) {
		var unityPath = "/Plugins/IOS/Frameworks/" + framework;
		var fullUnityPath = Application.dataPath + unityPath;

		var frameworkPath = "Frameworks/" + framework;
		var fullFrameworkPath = Path.Combine(pathToBuiltProject, frameworkPath);

		CopyAndReplaceDirectory (fullUnityPath, fullFrameworkPath);

		var frameworkFileGuid = project.AddFile (frameworkPath, frameworkPath, PBXSourceTree.Source);

		project.AddFileToBuild (targetGuid, frameworkFileGuid);

		project.AddFrameworkToProject (targetGuid, framework, false);
	}

	static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);

		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		string[] exclude = new string[] {"^.*.meta$", "^.*.mdown^", "^.*.pdf$"};
		string regexExclude = string.Format( @"{0}", string.Join( "|", exclude) );
		foreach (string file in Directory.GetFiles(srcPath)) {

			if( Regex.IsMatch(file, regexExclude ) ) {
				continue;
			}

			File.Copy (file, Path.Combine (dstPath, Path.GetFileName (file)));
		}

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}
	#endif
}

