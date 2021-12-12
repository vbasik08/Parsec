﻿using System;
using System.Collections.Generic;
using Parsec.Readers;
using Parsec.Shaiya.Common;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.Svmap
{
    public class NamedArea : IBinary
    {
        public CubicArea Area { get; set; }
        public int NameIdentifier1 { get; set; }
        public int NameIdentifier2 { get; set; }

        public NamedArea(ShaiyaBinaryReader binaryReader)
        {
            Area = new CubicArea(binaryReader);
            NameIdentifier1 = binaryReader.Read<int>();
            NameIdentifier2 = binaryReader.Read<int>();
        }

        public byte[] GetBytes()
        {
            var buffer = new List<byte>();
            buffer.AddRange(Area.GetBytes());
            buffer.AddRange(BitConverter.GetBytes(NameIdentifier1));
            buffer.AddRange(BitConverter.GetBytes(NameIdentifier2));
            return buffer.ToArray();
        }
    }
}
