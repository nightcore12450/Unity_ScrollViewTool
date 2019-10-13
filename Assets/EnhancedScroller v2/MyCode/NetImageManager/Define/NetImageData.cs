using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 描述：
/// 功能：
/// 作者：yoyohan
/// 创建时间：2019-06-08 14:51:02
/// </summary>
public class NetImageMonoScript : MonoBehaviour
{

}

public class NetImageData
{
    public string url;

    public Texture2D texture2D;
    private Sprite _sprite;
    private Sprite _sprite_GridScale;


    private Texture2D _texture2D_GridScale;
    public Texture2D texture2D_GridScale {
        get {
            if (_texture2D_GridScale == null)
            {
                _texture2D_GridScale = CropScale.ScaleTexture(texture2D, 202, 147);
                _texture2D_GridScale.Compress(false);
            }
            return _texture2D_GridScale;
        }
    }

    public Sprite getSprite()
    {
        if (_sprite == null && texture2D != null)
        {
            _sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
        }
        return _sprite;
    }

    public Sprite getSprite_GridScale()
    {
        if (_sprite_GridScale == null && texture2D_GridScale != null)
        {
            _sprite_GridScale = Sprite.Create(texture2D_GridScale, new Rect(0, 0, texture2D_GridScale.width, texture2D_GridScale.height), new Vector2(0.5f, 0.5f));
        }
        return _sprite_GridScale;
    }

    public void Copy(NetImageData data)
    {
        this.url = data.url;
        this.texture2D = data.texture2D;
        this._sprite = data._sprite;
        this._texture2D_GridScale = data._texture2D_GridScale;
        this._sprite_GridScale = data._sprite_GridScale;
    }
}

public class NetImageRequestObj
{
    public NetImageData netImageData;
    public Image imageComponent;
    public RawImage rawImageComponent;
    /// <summary>
    /// 0原生大小,1GridScale大小 X202 Y147
    /// </summary>
    public int useScaleId = 0;
    /// <summary>
    /// 判断同一个Path 不为同一个path就不设置了 解决网络延迟和重复利用格子造成图片设置两次
    /// </summary>
    public Func<bool> judgePath;
    public NetImageProcess netImageProcess;
    public IESetImageRequestObj ieSetImgReqObj;

    public void SetComponentSprite()
    {
        if (this.imageComponent != null)
        {
            if (judgePath != null && judgePath() == false)
                return;
            this.imageComponent.sprite = netImageData.getSprite();
        }
        if (this.rawImageComponent != null)
        {
            if (judgePath != null && judgePath() == false)
                return;
            this.rawImageComponent.texture = netImageData.texture2D;
        }
    }

    public void SetComponentSprite_GridSprite()
    {
        if (this.imageComponent != null)
        {

            if (judgePath != null && judgePath() == false)
                return;
            this.imageComponent.sprite = netImageData.getSprite_GridScale();
        }
        if (this.rawImageComponent != null)
        {


            if (rawImageComponent.GetComponent<EveryImg>().mData == null)
            {
                Debug.Log("为空返回");
                return;
            }
            else if (rawImageComponent.GetComponent<EveryImg>().mData.toOtherType<ImgCellData>().imgPath != netImageData.url)
            {
                Debug.Log((rawImageComponent.GetComponent<EveryImg>().mData.toOtherType<ImgCellData>().imgPath == netImageData.url) + "返回");
                return;
            }

            //if (judgePath != null && judgePath() == false)
            //    return;
            this.rawImageComponent.texture = netImageData.texture2D_GridScale;
        }
    }

    public void Copy(NetImageRequestObj netImageRequestObj)
    {
        this.imageComponent = netImageRequestObj.imageComponent;
        this.rawImageComponent = netImageRequestObj.rawImageComponent;
        this.useScaleId = netImageRequestObj.useScaleId;
        this.judgePath = netImageRequestObj.judgePath;
        this.netImageData = new NetImageData();
        this.netImageData.Copy(netImageRequestObj.netImageData);
    }
}
