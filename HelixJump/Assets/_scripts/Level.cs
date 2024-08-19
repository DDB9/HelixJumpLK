using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Platform
{
    [Range(1f, 11f)]
    public int SliceCount = 11;

    [Range(0f, 11f)]
    public int KillPartCount = 1;
}

[CreateAssetMenu(fileName = "New Level")]
public class Level : ScriptableObject
{
    public Color LevelBGColor = Color.white;
    public Color LevelPlatformColor = Color.white;
    public List<Platform> Platforms = new List<Platform>();
}
