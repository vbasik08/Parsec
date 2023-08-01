﻿using Parsec.Extensions;
using Parsec.Readers;
using Parsec.Shaiya.Common;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.WLD;

/// <summary>
/// A rectangular area of the world in which music is played
/// </summary>
public sealed class MusicZone : IBinary
{
    /// <summary>
    /// The rectangular area of the music zone
    /// </summary>
    public BoundingBox BoundingBox { get; set; }

    /// <summary>
    /// TODO: Check that "DistanceToCenter" fits this field
    /// </summary>
    public float DistanceToCenter { get; set; }

    /// <summary>
    /// Id of the wav file (from the linked name list of files)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Usually 0L
    /// </summary>
    public int Unknown { get; set; }

    public MusicZone(SBinaryReader binaryReader)
    {
        BoundingBox = new BoundingBox(binaryReader);
        DistanceToCenter = binaryReader.Read<float>();
        Id = binaryReader.Read<int>();
        Unknown = binaryReader.Read<int>();
    }

    public IEnumerable<byte> GetBytes(params object[] options)
    {
        var buffer = new List<byte>();
        buffer.AddRange(BoundingBox.GetBytes());
        buffer.AddRange(DistanceToCenter.GetBytes());
        buffer.AddRange(Id.GetBytes());
        buffer.AddRange(Unknown.GetBytes());
        return buffer;
    }
}
