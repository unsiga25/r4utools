﻿using Fluent.IO;
using Lamar;
using Montage.RebirthForYou.Tools.CLI.API;
using Montage.RebirthForYou.Tools.CLI.Entities;
using Montage.RebirthForYou.Tools.CLI.Utilities;
using Octokit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Montage.RebirthForYou.Tools.CLI.Impls.Exporters.Database.Cockatrice
{
    public class CockatriceExporter : IDatabaseExporter
    {
        private ILogger Log = Serilog.Log.ForContext<CockatriceExporter>();
        public string[] Alias => new string[] { "cockatrice", "cc3s" };

        public async Task Export(CardDatabaseContext database, IDatabaseExportInfo info)
        {
            Log.Information("Starting...");
            var query = CreateQuery(database.R4UCards, info);
            var resultFile = Path.CreateDirectory(info.Destination).Combine("cockatrice_card_db.xml");
            var serializer = new XmlSerializer(typeof(CockatriceCardDatabase));
            var cardSet = await CockatriceCardDatabase.CreateFromDatabase(query);
            resultFile.Open(s => serializer.Serialize(s, cardSet),
                                    System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write,
                                    System.IO.FileShare.ReadWrite
                                    );
            Log.Information($"Done: {resultFile.FullPath}");
        }

        private IAsyncEnumerable<R4UCard> CreateQuery(IAsyncEnumerable<R4UCard> query, IDatabaseExportInfo info)
        {
            var releaseIDLimitations = info.ReleaseIDs.ToArray();
            var result = query;
            if (releaseIDLimitations.Length > 0)
                result = result.Where(card => releaseIDLimitations.Contains(card.ReleaseID));
            return result.Where(card => card.Images.Count > 0);
        }
    }

    [XmlRootAttribute(  "cockatrice_carddatabase",
                        IsNullable = false
                        )]
    public class CockatriceCardDatabase
    {
        [XmlAttribute("version")]
        public string Version;

        [XmlArray("sets")]
        [XmlArrayItem("set")]
        public CockatriceSet[] Sets;

        [XmlArray("cards")]
        [XmlArrayItem("card")]
        public CockatriceCard[] Cards;

        internal static async Task<CockatriceCardDatabase> CreateFromDatabase(IAsyncEnumerable<R4UCard> query)
        {
            var result = new CockatriceCardDatabase();
            var tempSetList = new Dictionary<string,CockatriceSet>();
            var tempCardList = new List<CockatriceCard>();
            await foreach (var card in query)
            {
                try
                {
                    var newCckCard = new CockatriceCard();
                    newCckCard.Name = card.Name.AsNonEmptyString();
                    newCckCard.Set = GetOrCreateSet(tempSetList, card).AsSetRelationship();
                    newCckCard.Set.PicURL = card.Images[^1].ToString();
                    newCckCard.Props = new CockatriceCardCardProperties();
                    newCckCard.Props.Code = card.Serial;
                    newCckCard.Props.Colors = TranslateToColorPropString(card.Color);
                    newCckCard.Props.LevelCost = $"{card.Level}/{card.Cost}";
                    newCckCard.Props.PowerSoul = $"{card.Power ?? 0}/{card.Soul ?? 0}";
                    newCckCard.Props.Type = $"{card.Traits.Select(TranslateTrait).Prepend(card.Type.ToString()).ConcatAsString(" - ")}";
                    newCckCard.Props.MainType = card.Type.ToString();
                    newCckCard.Props.Triggers = card.Triggers?.Select(t => t.ToString()).ConcatAsString(" - ") ?? "";
                    newCckCard.Text = FormatText(card);
                    tempCardList.Add(newCckCard);
                } catch (Exception e)
                {
                    Log.Warning("Could not add [{card}] Reason: {message}", card.Serial, e.Message);
                }
            }
            result.Sets = tempSetList.Values.ToArray();
            result.Cards = tempCardList.ToArray();
            result.Version = "4";
            return result;
        }

        private static string FormatText(R4UCard card)
        {
            var result = "";
            result += card.Effect.ConcatAsString("\n");
            if (!String.IsNullOrWhiteSpace(card.Flavor))
                result += $"\n\n{card.Flavor}";
            return result;
        }

        private static string TranslateToColorPropString(CardColor color)
        {
            return color.ToString().Substring(0, 1);
        }

        private static string TranslateTrait(MultiLanguageString mlString)
        {
            return mlString?.EN?.Replace(" ", "");
        }

        private static CockatriceSet GetOrCreateSet(Dictionary<string, CockatriceSet> tempSetList, R4UCard card)
        {
            var rid = card.ReleaseID;
            if (tempSetList.TryGetValue(rid, out var existingSet))
                return existingSet;
            else
            {
                var result = new CockatriceSet(rid);
                tempSetList[rid] = result;
                return result;
            }
        }
    }

    public class CockatriceSet
    {
        [XmlElement("name")]
        public string Name;
        [XmlElement("longname")]
        public string LongName;

        public CockatriceSet()
        {
        }

        public CockatriceSet(string releaseID)
        {
            this.Name = "WS" + releaseID;
            this.LongName = "[To Be Replaced With an Actual Value]"; //TODO
        }

        internal CockatriceCardSetRelationship AsSetRelationship()
        {
            return new CockatriceCardSetRelationship(Name);
        }
    }

    public class CockatriceCard
    {
        [XmlElement("name")]
        public string Name;
        [XmlElement("set")]
        public CockatriceCardSetRelationship Set;
        [XmlElement("prop")]
        public CockatriceCardCardProperties Props;
        [XmlElement("text")]
        public string Text;
    }

    public class CockatriceCardCardProperties
    {
        [XmlElement("code")]
        public string Code;
        [XmlElement("colors")]
        public string Colors;
        [XmlElement("manacost")]
        public string LevelCost;
        [XmlElement("type")]
        public string Type;
        [XmlElement("maintype")]
        public string MainType;
        [XmlElement("pt")]
        public string PowerSoul;
        [XmlElement("triggers")]
        public string Triggers;
    }

    public class CockatriceCardSetRelationship
    {
        [XmlAttribute("picURL")]
        public string PicURL;
        [XmlAttribute("picURLHq")]
        public string PicURLHQ;
        [XmlAttribute("picURLSt")]
        public string PicURLST;
        [XmlText]
        public string SetName;

        public CockatriceCardSetRelationship()
        {
        }

        public CockatriceCardSetRelationship(string name)
        {
            this.SetName = name;
        }
    }
}
