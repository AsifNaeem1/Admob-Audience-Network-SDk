# Amob, Audience Network & Unity Ads in Signgle Package

Package Version\
Google Mobile Ads Unity Plugin v3.16\
Google Play services 17.2.0\
Google Mobile Ads iOS SDK 7.42.0\
Unity Jar Resolver 1.2.102.0\
Audience Network 5.2.0\
Note: Use Gradle build system to build apk

Test Note: Tested in Unity 2018.3.0f2 for andriod plateform. Stable release

#### Check List for For Andriod App

- [ ]  Drag AdsManager in scene. Add Admob, Audience Network and Unity ID 	for desire plateform.
- [ ]  Add your AdMob app ID to the AndroidManifest.xml file in the
Assets/Plugins/Android/GoogleMobileAdsPlugin directory of your Unity app, using the <meta-data> tag as shown below. You can find your app ID in the AdMob UI. For android:value insert your own AdMob app ID in quotes
- [ ]  In AdsManager.cs Start() lines are commented to test with DemoScene. 	Each function call is manual. Uncomment these lines before use.
- [ ]  Uncheck Test Ads before final release.



#### Check List for App Store App
- [ ] App bundle in plist.cs file
- [ ] Add InApp Purchases in Binary Build
  
 Common Error & Solution Xcode
  clang: error: linker command failed with exit code 1.
  Iss error ki detail b ni milti. Ni pta chalta k kis waja say arha ha
  
  Solution: DISABLE BITCODE:
  In Xcode Under the 'Build Settings' tab there is a 'Build Options' tab. Change the 'Enable Bitcode' part to 'No'
  https://answers.unity.com/questions/819445/xcode-clang-error-linker-command-failed-with-exit.html
  
  
