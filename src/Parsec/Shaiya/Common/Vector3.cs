﻿using Newtonsoft.Json;
using Parsec.Serialization;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.Common;

/// <summary>
/// Represents a vector in a 3-dimensional space
/// </summary>
public struct Vector3 : ISerializable
{
    /// <summary>
    /// 1st (first) element of the vector
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// 2nd (second) element of the vector
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// 3rd (third) element of the vector
    /// </summary>
    public float Z { get; set; }

    [JsonConstructor]
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Read(SBinaryReader binaryReader)
    {
        X = binaryReader.ReadSingle();
        Y = binaryReader.ReadSingle();
        Z = binaryReader.ReadSingle();
    }

    public void Write(SBinaryWriter binaryWriter)
    {
        binaryWriter.Write(X);
        binaryWriter.Write(Y);
        binaryWriter.Write(Z);
    }

    [JsonIgnore]
    public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

    public static Vector3 operator +(Vector3 vec1, Vector3 vec2) => new(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z);

    public static Vector3 operator -(Vector3 vec1, Vector3 vec2) => new(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z);
}
