using UnityEngine;

/// <summary>
/// GameObjectの拡張クラス
/// </summary>
public static class GameObjectExtension 
{
  /// <summary>
  /// レイヤーを設定する
  /// </summary>
  public static void SetLayer(this GameObject gameObject, int layerNo)
  {
    if(gameObject == null)
    {
      return;
    }
        gameObject.layer = layerNo;
    }
}