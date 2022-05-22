using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBased
{
    public class ResourceManager : MonoBehaviour
    {
        public static TResult CreatePrefabInstance<TEnum, TResult>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class
        {
            var type = typeof(TEnum).Name;
            var name = resource.ToString();
                
            var prefab = Resources.Load(type + "/" + name, typeof(GameObject)) as GameObject;
            var go = GameObject.Instantiate(prefab);
            var component = go.GetComponent(typeof(TResult)) as TResult;
            return component;
        }
        
        public static TResult CreatePrefabInstance<TEnum, TResult>(TEnum resource, Vector3 position)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class
        {
            var type = typeof(TEnum).Name;
            var name = resource.ToString();
            
            var prefab = Resources.Load(type + "/" + name, typeof(GameObject)) as GameObject;
            var go = GameObject.Instantiate(prefab, position, 
                new Quaternion(0f, 0f, 0f, 0f));
            var component = go.GetComponent(typeof(TResult)) as TResult;
            return component;
        }
    }
}

