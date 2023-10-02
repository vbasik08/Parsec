﻿using System.Globalization;
using System.Text;
using CsvHelper;
using Newtonsoft.Json;
using Parsec.Common;
using Parsec.Extensions;

namespace Parsec.Shaiya.Item;

public sealed class Item : SData.SData, ICsv
{
    [JsonIgnore]
    public Dictionary<(byte type, byte typeId), IItemDefinition> ItemIndex = new();

    public int MaxItemType { get; set; }

    public List<ItemType> ItemTypes { get; } = new();

    public override void Read()
    {
        MaxItemType = _binaryReader.Read<int>();
        for (int i = 0; i < MaxItemType; i++)
        {
            var itemType = new ItemType(_binaryReader, i + 1, Episode, ItemIndex);
            ItemTypes.Add(itemType);
        }
    }

    public override IEnumerable<byte> GetBytes(Episode episode = Episode.Unknown)
    {
        var buffer = new List<byte>();
        buffer.AddRange(MaxItemType.GetBytes());

        for (int i = 1; i <= MaxItemType; i++)
        {
            var type = ItemTypes.SingleOrDefault(t => t.Id == i);

            // When type isn't part of the item, its MaxTypeId = 0 must be written to the file anyways
            if (type == null)
            {
                buffer.AddRange(0.GetBytes());
                continue;
            }

            buffer.AddRange(type.GetBytes(episode, Encoding));
        }

        return buffer;
    }

    /// <summary>
    /// Reads the Item.SData format from a csv file
    /// </summary>
    /// <param name="csvPath">csv file path</param>
    /// <param name="episode">The Item.SData format</param>
    /// <param name="encoding">Item.SData encoding</param>
    /// <returns><see cref="Item"/> instance</returns>
    public static Item ReadFromCsv(string csvPath, Episode episode, Encoding encoding = null)
    {
        encoding ??= Encoding.ASCII;

        // Create Item.SData instance
        var item = new Item { Episode = episode, Encoding = encoding };
        var itemDefinitions = new List<IItemDefinition>();

        // Read all item definitions from csv file
        switch (episode)
        {
            case Episode.EP4:
            case Episode.EP5:
            case Episode.Unknown:
            default:
                {
                    // Read item definitions from csv
                    using var reader = new StreamReader(csvPath, encoding);
                    using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    var records = csvReader.GetRecords<ItemDefinitionEp5>().ToList();

                    // Cast item definitions to IItemDefinition since the FileIndex is generic for every format
                    itemDefinitions = records.Cast<IItemDefinition>().ToList();
                    break;
                }
            case Episode.EP6:
            case Episode.EP7:
                {
                    // Read item definitions from csv
                    using var reader = new StreamReader(csvPath, encoding);
                    using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    var records = csvReader.GetRecords<ItemDefinitionEp6>().ToList();

                    // Cast item definitions to IItemDefinition since the FileIndex is generic for every format
                    itemDefinitions = records.Cast<IItemDefinition>().ToList();
                    break;
                }
            case Episode.EP8:
                throw new Exception("Episode 8 must use the DBItemData class.");
        }

        // Get max type from items
        item.MaxItemType = itemDefinitions.Max(x => x.Type);

        // Add all items to item index
        var itemIndex = itemDefinitions.ToDictionary(itemDef => (itemDef.Type, itemDef.TypeId));
        item.ItemIndex = itemIndex;

        // Create item types
        for (int i = 1; i <= item.MaxItemType; i++)
        {
            // Get items for this type
            var items = item.ItemIndex.Values.Where(x => x.Type == i).ToList();

            int maxTypeId = items.Count == 0 ? 0 : items.Max(x => x.TypeId);

            var type = new ItemType(i, maxTypeId, items);
            item.ItemTypes.Add(type);
        }

        return item;
    }

    /// <inheritdoc />
    public void WriteCsv(string outputPath, Encoding encoding = null)
    {
        encoding ??= Encoding.ASCII;

        switch (Episode)
        {
            case Episode.Unknown:
            case Episode.EP4:
            case Episode.EP5:
            default:
                {
                    var items = ItemIndex.Values.ToList().Cast<ItemDefinitionEp5>().ToList();
                    using var writer = new StreamWriter(outputPath, false, encoding);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csvWriter.WriteRecords(items);
                    break;
                }
            case Episode.EP6:
            case Episode.EP7:
                {
                    var items = ItemIndex.Values.ToList().Cast<ItemDefinitionEp6>().ToList();
                    using var writer = new StreamWriter(outputPath, false, encoding);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csvWriter.WriteRecords(items);
                    break;
                }
            case Episode.EP8:
                throw new Exception("Episode 8 must use the DBItemData class.");
        }
    }
}
