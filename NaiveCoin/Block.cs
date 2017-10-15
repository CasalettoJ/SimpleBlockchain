using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveCoin
{
    [Serializable]
    public class Block
    {
        public UInt64 Index { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string Data { get; private set; }
        public string Hash { get; set; }
        public string PreviousHash { get; private set; }

        public Block(UInt64 _index, DateTime _timeStamp, string _data, string _previousHash)
        {
            Index = _index;
            TimeStamp = _timeStamp;
            Data = _data;
            PreviousHash = _previousHash;
            Hash = Block.HashBlock(this);
        }

        public static string HashBlock(Block block)
        {
            return string.Format("{0}{1}{2}{3}", block.Index, block.TimeStamp.ToString(), block.Data, block.PreviousHash).GetSha256();
        }

        public static Block GenerateGenesisBlock()
        {
            return new Block(0, DateTime.UtcNow, "This is the genesis block.", "0");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Block Index: " + this.Index);
            sb.Append(Environment.NewLine);
            sb.Append("Created: " + this.TimeStamp.ToLongDateString() + this.TimeStamp.ToLongTimeString());
            sb.Append(Environment.NewLine);
            sb.Append("Data: " + this.Data);
            sb.Append(Environment.NewLine);
            sb.Append("Previous Hash: " + this.PreviousHash);
            sb.Append(Environment.NewLine);
            sb.Append("Hash: " + this.Hash);
            return sb.ToString();
        }
    }
}
