﻿using Parsec.Extensions;
using Parsec.Readers;
using Parsec.Shaiya.Common;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.WLD;

/// <summary>
/// Coordinates to place a 3D object in the field. Used by 'MANI' entities only.
/// </summary>
public sealed class ManiCoordinate : IBinary
{
    /// <summary>
    /// Unknown field
    /// </summary>
    public int Unknown { get; set; }

    /// <summary>
    /// Id of a 3D Model
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// World position where to place the model
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// Rotation vector
    /// </summary>
    public Vector3 Rotation { get; set; }

    /// <summary>
    /// Scaling vector - almost always (0, 1, 0)
    /// </summary>
    public Vector3 Scale { get; set; }

    public ManiCoordinate(SBinaryReader binaryReader)
    {
        Unknown = binaryReader.Read<int>();
        Id = binaryReader.Read<int>();
        Position = new Vector3(binaryReader);
        Rotation = new Vector3(binaryReader);
        Scale = new Vector3(binaryReader);
    }

    public IEnumerable<byte> GetBytes(params object[] options)
    {
        var buffer = new List<byte>();
        buffer.AddRange(Unknown.GetBytes());
        buffer.AddRange(Id.GetBytes());
        buffer.AddRange(Position.GetBytes());
        buffer.AddRange(Rotation.GetBytes());
        buffer.AddRange(Scale.GetBytes());
        return buffer;
    }
}
