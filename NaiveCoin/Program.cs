using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NaiveCoin
{
    public static class EncryptionExtensions
    {
        public static string GetSha256(this string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sha256Bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < sha256Bytes.Length; i++)
                {
                    sb.Append(sha256Bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static bool VerifySha256(this string str, string sha256)
        {
            string check256 = str.GetSha256();
            StringComparer sc = StringComparer.Ordinal;
            return sc.Compare(sha256, check256) == 0;
        }

        public static bool VerifySha256(this string str, string sha256, out string hashed)
        {
            string check256 = str.GetSha256();
            StringComparer sc = StringComparer.Ordinal;
            hashed = check256;
            return sc.Compare(sha256, check256) == 0;
        }
    }

    public class Block
    {
        public UInt64 Index { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string Data { get; private set; }
        public string Hash { get; set; }
        public string PreviousHash { get; private set; }

        public Block(UInt64 _index, DateTime _timeStamp, string _data,  string _previousHash)
        {
            Index = _index;
            TimeStamp = _timeStamp;
            Data = _data;
            PreviousHash = _previousHash;
            Hash = Block.HashBlock(this);
        }

        public static string HashBlock(Block block)
        {
            string hash = string.Format("{0}{1}{2}{3}", block.Index, block.TimeStamp.ToString(), block.Data, block.PreviousHash).GetSha256();
            return hash;
        }

        public static Block GenerteGenesisBlock()
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

    class Program
    {
        static void Main(string[] args)
        {
            List<Block> Blockchain = new List<Block>();
            Console.WriteLine("Creating Genesis Block...");
            Blockchain.Add(Block.GenerteGenesisBlock());
            Console.WriteLine("Genesis block created.");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Blockchain[0].ToString());

            int choice = -1;
            while(choice != 0)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("0: Exit");
                Console.WriteLine("1: Add New Block");
                Console.WriteLine("2: Blockchain Info");
                Console.WriteLine("3: View Block");
                while(!int.TryParse(Console.ReadLine(), out choice))
                {

                }
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        Console.WriteLine(Environment.NewLine + "Enter Data to include: ");
                        Blockchain.Add(new Block(UInt64.Parse(Blockchain.Count.ToString()), DateTime.UtcNow, Console.ReadLine(), Blockchain[Blockchain.Count - 1].Hash));
                        Console.WriteLine("Block added.");
                        Console.WriteLine(Environment.NewLine + Blockchain[Blockchain.Count - 1].ToString());
                        break;
                    case 2:
                        Console.WriteLine(Environment.NewLine + "Blocks: " + Blockchain.Count);
                        Console.WriteLine("Last block added: " + Blockchain[Blockchain.Count - 1].TimeStamp.ToLongDateString() + Blockchain[Blockchain.Count - 1].TimeStamp.ToLongTimeString());
                        break;
                    case 3:
                        int blockIndex = 0;
                        while(blockIndex != -1)
                        {
                            Console.WriteLine(Environment.NewLine + "Enter block index (-1 to quit): ");
                            while (!int.TryParse(Console.ReadLine(), out blockIndex)) { }
                            if (blockIndex >= 0 && blockIndex < Blockchain.Count)
                            {
                                Console.WriteLine(Environment.NewLine + Blockchain[blockIndex].ToString());
                            }
                            else
                            {
                                Console.WriteLine("Out of range.");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
