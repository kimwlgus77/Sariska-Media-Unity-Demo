using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExternalTextureSecond : MonoBehaviour
{
    private AndroidJavaObject mGLTexCtrl;
    [SerializeField] private RawImage image;
    [SerializeField] private RawImage remoteImage;
    private int mTextureId;
    private int mWidth;
    private int mHeight;
    private Texture2D texture2D;
    private Texture2D remoteTexture2D;
    private IntPtr _nativeTexturePointer;
    //private Token tokenInstance;
    
    private void Awake()
    {
        AndroidJavaClass androidWebViewApiClass = new AndroidJavaClass("io.sariska.sariskamediaunityplugin.SariskaMediaUnityPlugin");
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        mGLTexCtrl = androidWebViewApiClass.CallStatic<AndroidJavaObject>("Instance", currentActivityObject);
        //tokenInstance = TokenAPIHelp.GetSessionToken("dipak");
        String token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNmYjc1MTJjZjgzYzdkYTRjMjM0Y2QzYWEyYWViOTUzMGNlZmUwMDg1YzRiZjljYzgwY2U5YmQ5YmRiNjA3ZjciLCJ0eXAiOiJKV1QifQ.eyJjb250ZXh0Ijp7InVzZXIiOnsiaWQiOiIwa3hwYmp6OSIsIm5hbWUiOiJVbml0eVVzZXIifSwiZ3JvdXAiOiIxIn0sInN1YiI6InVhdG5jb2U1djcybG5vaGxud2dxdjgiLCJyb29tIjoiKiIsImlhdCI6MTY1MDYzNTA2NSwibmJmIjoxNjUwNjM1MDY1LCJpc3MiOiJzYXJpc2thIiwiYXVkIjoibWVkaWFfbWVzc2FnaW5nX2NvLWJyb3dzaW5nIiwiZXhwIjoxNjUwODA3ODY1fQ.fuTQzwvIAaUAoq8zTrhMnunIUmJopct7g8vU08UHM7ScpwcR-umm602e4qJLtoNuptAMCfPPrj7It33yMHtkveCpmojxOy1ZUkpWLki-aZn0aE1y4ayktHfuig3CZCgzyXpBMX13lvG1PfQa0Z9wtOTYtM6vA7l4KuOHsdwgls-TcnqOgY4zi40HWp1d1OsdgddnmzdTxDmqJzqHIihmaCfR1h7bC8ywm2R6-6PKp2fYqPu1-xQKvuDvs7ETUyqMPDhuIQcVjBJIy3Uzlq2DGVfgglpl2BF3TimSKjctK1JwTvMkdXndBI60NBqtHc-JRstAszo_Hm4pzF06BtpErA";
        mGLTexCtrl.Call("setupOpenGL", token);
    }

    // Start is called before the first frame update
    void Start()
    {
        remoteTexture2D = new Texture2D(1280, 800, TextureFormat.ARGB32, false)
        { filterMode = FilterMode.Point };

        remoteTexture2D.Apply();
        remoteImage.texture = remoteTexture2D;
        _nativeTexturePointer = remoteTexture2D.GetNativeTexturePtr();

        texture2D = new Texture2D(1280, 800, TextureFormat.ARGB32, false);
        image.texture = texture2D;
        BindTexture(_nativeTexturePointer);
    }

    private void BindTexture(IntPtr remoteTexturePointer)
    {
        mTextureId = mGLTexCtrl.Call<int>("getStreamTextureID", remoteTexturePointer.ToInt32());
        if (mTextureId == 0) {
            Debug.Log("Texture ID value is zero");
        }
        mWidth = mGLTexCtrl.Call<int>("getStreamTextureWidth");
        mHeight = mGLTexCtrl.Call<int>("getStreamTextureHeight");
        Debug.Log("Done getting width and height");
        
        Texture2D nativeTexture = Texture2D.CreateExternalTexture(
                mWidth, mHeight,
                TextureFormat.ARGB32,
                false, false,
                (IntPtr)mTextureId);

        texture2D.UpdateExternalTexture(nativeTexture.GetNativeTexturePtr());
        //Update texture data
        
        mGLTexCtrl.Call("setupLocalStream");
    }

    // Update is called once per frame
    void Update()
    {
        if(mGLTexCtrl == null)
        {
            return;
        }
        
    }
}
