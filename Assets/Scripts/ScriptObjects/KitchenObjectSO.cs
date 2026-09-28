using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
//ScriptableObject 是 Unity 提供的一个数据配置存储基类，它是一个可以用来保存大量数据的数据容器，我们可以将它保存为自定义的数据资源文件
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string ObjectName;
    public float price;
    public int number;
}
