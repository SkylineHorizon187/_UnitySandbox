using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Tags : MonoBehaviour {

    public List<string> MyTags;


    public void AddTag(string tag) {
        MyTags.Add(tag);
    }

    public void RemoveTag(string tag) {
        MyTags.Remove(tag);
    }

    public bool Contains(string tag) {
        return MyTags.Contains(tag);
    }

}
