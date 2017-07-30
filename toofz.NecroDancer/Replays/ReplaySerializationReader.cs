using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using toofz.NecroDancer.Saves;
using toofz.Xml;

namespace toofz.NecroDancer.Replays
{
    sealed class ReplaySerializationReader : StreamReader
    {
        const string RemoteSignature = "%*#%*";
        static readonly XmlSerializer SaveDataSerializer = new XmlSerializer(typeof(SaveData));

        static int ConvertToInt32(string line) => Convert.ToInt32(line, CultureInfo.InvariantCulture);

        public ReplaySerializationReader(Stream stream) : base(stream) { }

        public override string ReadLine()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int ch = Read();

                if (ch == -1)
                    break;

                if (ch == '\\' && Peek() == 'n')
                {
                    Read();

                    return sb.ToString();
                }

                sb.Append((char)ch);
            }

            if (sb.Length > 0)
                return sb.ToString();

            return null;
        }

        public ReplayData Deserialize()
        {
            var replay = new ReplayData();

            replay.Header = ReadHeader();
            ReadLevels(replay);
            replay.SaveData = ReadSaveData();

            return replay;
        }

        #region Header

        Header ReadHeader()
        {
            var header = new Header();

            var signature = ReadLineAsRemoteSignature();
            header.KilledBy = signature.KilledBy;
            header.Version = signature.Version;

            if (header.Version <= 84)
            {
                header.StartZone = ReadLineAsInt32();
                header.StartCoins = ReadLineAsInt32();
                header.HasBroadsword = ReadLineAsBoolean();
                header.IsHardcore = ReadLineAsBoolean();
                header.IsDaily = ReadLineAsBoolean();
                header.IsDancePadMode = ReadLineAsBoolean();
                header.IsSeeded = ReadLineAsBoolean();
            }
            else
            {
                header.Run = ReadLineAsInt32();
                header.Unknown0 = ReadLineAsInt32();
                header.Unknown1 = ReadLineAsInt32();
                header.Unknown2 = ReadLineAsInt32();
            }

            header.Duration = ReadLineAsDuration();
            header.LevelCount = ReadLineAsInt32();

            return header;
        }

        RemoteInfo ReadLineAsRemoteSignature()
        {
            var signature = new RemoteInfo();

            var line = ReadLine();

            if (line != null)
            {
                var index = line.IndexOf(RemoteSignature, StringComparison.OrdinalIgnoreCase);

                if (index > 1)
                {
                    signature.KilledBy = line.Substring(0, index);
                }

                var match = Regex.Match(line, @"(\d+)$");
                if (match.Success)
                {
                    signature.Version = ConvertToInt32(match.Value);
                }
            }

            return signature;
        }

        #endregion

        #region Levels

        void ReadLevels(ReplayData replay)
        {
            if (replay == null)
                throw new ArgumentNullException(nameof(replay));

            for (int i = 0; i < replay.Header.LevelCount; i++)
            {
                var zone = GetZone(i);
                var level = GetLevel(i);
                var levelData = ReadLevel(zone, level);
                replay.Levels.Add(levelData);
            }

            ReadLine();
        }

        LevelData ReadLevel(int zone, int level)
        {
            var data = new LevelData();

            data.Zone = zone;
            data.Level = level;
            data.Seed = ReadLineAsInt32();
            var playerCount = ReadLineAsInt32();
            data.Unknown0 = ReadLineAsInt32();
            data.Unknown1 = ReadLineAsInt32();
            data.TotalBeats = ReadLineAsInt32();
            if (playerCount > 0)
            {
                data.Players.AddRange(ReadPlayers(playerCount));
            }

            var randomMoveCount = ReadLineAsInt32();
            var randomMoves = ParseInt32List(ReadLine(), randomMoveCount);
            data.RandomMoves.AddRange(randomMoves);

            var itemRollsCount = ReadLineAsInt32();
            var itemRolls = ParseInt32List(ReadLine(), itemRollsCount);
            data.ItemRolls.AddRange(itemRolls);

            return data;
        }

        static int GetZone(int index)
        {
            if (index < 12)
            {
                return (index / 4) + 1;
            }
            else
            {
                return 4;
            }
        }

        static int GetLevel(int index)
        {
            if (index < 12)
            {
                return (index % 4) + 1;
            }
            else
            {
                return index - 12 + 1;
            }
        }

        #region Players

        IEnumerable<Player> ReadPlayers(int count)
        {
            if (count <= 0)
                throw new ArgumentException();

            var players = new List<Player>(count);

            for (int i = 0; i < count; i++)
            {
                players.Add(ReadPlayer());
            }

            return players;
        }

        Player ReadPlayer()
        {
            var player = new Player();

            var movesLine = ReadLine();
            if (movesLine != null)
            {
                var tokens = movesLine.Split('|');
                if (tokens.Length == 3)
                {
                    player.Character = ConvertToInt32(tokens[0]);

                    var moveCount = ConvertToInt32(tokens[1]);
                    if (moveCount > 0)
                    {
                        var moves = ParseMoves(tokens[2], moveCount);
                        player.Moves.AddRange(moves);
                    }
                }
            }

            var wrongMoveBeatsLine = ReadLine();
            if (wrongMoveBeatsLine != null)
            {
                var wrongMoveBeats = ParseInt32List(wrongMoveBeatsLine);
                player.WrongMoveBeats.AddRange(wrongMoveBeats);
            }

            return player;
        }

        ICollection<Move> ParseMoves(string value, int moveCount)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (moveCount <= 0)
                throw new ArgumentException();

            var moves = new List<Move>(moveCount);

            var pairs = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pairs.Length; i++)
            {
                var tokens = pairs[i].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                {
                    var move = new Move();

                    move.Beat = ConvertToInt32(tokens[0]);
                    move.Id = ConvertToInt32(tokens[1]);

                    moves.Add(move);
                }
            }

            return moves;
        }

        #endregion

        #endregion

        #region SaveData

        SaveData ReadSaveData()
        {
            if (EndOfStream)
            {
                return null;
            }

            var xpr = new XmlPreprocessingReader(BaseStream);

            return SaveDataSerializer.Deserialize(xpr) as SaveData;
        }

        #endregion

        #region Methods

        int[] ParseInt32List(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var tokens = value.Split('|');
            if (tokens.Length == 2)
            {
                var count = ConvertToInt32(tokens[0]);
                return ParseInt32List(tokens[1], count);
            }
            else
            {
                return new int[0];
            }
        }

        static int[] ParseInt32List(string value, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (from i in value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    select ConvertToInt32(i)).ToArray();
        }

        int ReadLineAsInt32()
        {
            var line = ReadLine();

            if (line == null)
            {
                return 0;
            }

            return ConvertToInt32(line);
        }

        bool ReadLineAsBoolean()
        {
            var line = ReadLine();

            switch (line)
            {
                case "1": return true;
                default: return false;
            }
        }

        TimeSpan ReadLineAsDuration()
        {
            var ms = ReadLineAsInt32();

            return TimeSpan.FromMilliseconds(ms);
        }

        #endregion

        class RemoteInfo
        {
            public string KilledBy { get; set; }
            public int Version { get; set; }
        }
    }
}
