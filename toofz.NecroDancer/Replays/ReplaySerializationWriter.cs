using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using toofz.NecroDancer.Saves;

namespace toofz.NecroDancer.Replays
{
    sealed class ReplaySerializationWriter : StreamWriter
    {
        static readonly XmlSerializer SaveDataSerializer = new XmlSerializer(typeof(SaveData));

        public ReplaySerializationWriter(Stream stream)
            : base(stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            NewLine = "\\n";
        }

        public void Serialize(ReplayData replay)
        {
            if (replay == null)
                throw new ArgumentNullException(nameof(replay));

            Write(replay.Header);
            Write(replay.Levels);
            WriteLine();
            Flush();
            if (replay.SaveData != null)
            {
                SaveDataSerializer.Serialize(this, replay.SaveData);
            }
        }

        public override void Write(object value)
        {
            throw new NotSupportedException();
        }

        public override void WriteLine(object value)
        {
            throw new NotSupportedException();
        }

        public override void Write(bool value)
        {
            Write(value ? 1 : 0);
        }

        void Write(Header header)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            WriteLine(header.Version);
            WriteLine(header.StartZone);
            WriteLine(header.StartCoins);
            WriteLine(header.HasBroadsword);
            WriteLine(header.IsHardcore);
            WriteLine(header.IsDaily);
            WriteLine(header.IsDancePadMode);
            WriteLine(header.IsSeeded);
            WriteLine(header.Duration);
            WriteLine(header.LevelCount);
        }

        void Write(IEnumerable<LevelData> levels)
        {
            if (levels == null)
                throw new ArgumentNullException(nameof(levels));

            foreach (var level in levels)
            {
                if (level == null)
                    throw new ArgumentNullException(nameof(level));

                Write(level);
            }
        }

        void Write(LevelData level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            WriteLine(level.Seed);
            WriteLine(level.Players.Count);
            WriteLine(level.Unknown0);
            WriteLine(level.Unknown1);
            WriteLine(level.TotalBeats);
            Write(level.Players);
            WriteLine(level.RandomMoves.Count);
            WriteLine(level.RandomMoves);
            WriteLine(level.ItemRolls.Count);
            WriteLine(level.ItemRolls);
        }

        void Write(IEnumerable<Player> players)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));

            foreach (var player in players)
            {
                if (player == null)
                    throw new ArgumentNullException(nameof(player));

                Write(player);
            }
        }

        void Write(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            WriteProperty(player.Character);
            WriteProperty(player.Moves.Count);
            WriteLine(player.Moves);
            WriteProperty(player.WrongMoveBeats.Count);
            WriteLine(player.WrongMoveBeats);
        }

        void Write(Version version)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            WriteLine(version.Major);
            WriteLine(version.Minor);
            WriteLine(version.Build);
        }

        void Write(TimeSpan timeSpan)
        {
            Write(timeSpan.TotalMilliseconds);
        }

        void Write(ICollection<Move> moves)
        {
            if (moves == null)
                throw new ArgumentNullException(nameof(moves));

            foreach (var move in moves)
            {
                Write("{0}:{1},", move.Beat, move.Id);
            }
        }

        void Write(IEnumerable<int> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (var value in values)
            {
                Write(value + ",");
            }
        }

        void WriteLine(TimeSpan value)
        {
            Write(value);
            WriteLine();
        }

        void WriteLine(ICollection<Move> moves)
        {
            if (moves == null)
                throw new ArgumentNullException(nameof(moves));

            Write(moves);
            WriteLine();
        }

        void WriteLine(IEnumerable<int> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Write(values);
            WriteLine();
        }

        void WriteProperty(int value)
        {
            Write(value);
            Write('|');
        }
    }
}
