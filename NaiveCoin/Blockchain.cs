using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NaiveCoin
{
    [Serializable]
    public class Blockchain
    {
        private List<Block> _blocks = new List<Block>();
        public ReadOnlyCollection<Block> Blocks { get { return _blocks.AsReadOnly(); } }
        public int Length => Blocks.Count;
        public int LastIndex => Blocks.Count - 1;
        public Block LastBlock => Blocks[LastIndex];

        public Blockchain()
        {
            Console.WriteLine("Genesis block created.");
            _blocks.Add(Block.GenerateGenesisBlock());
            Blockchain.SaveChain(this);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Blocks[0].ToString());
        }

        public Blockchain(List<Block> blocks)
        {
            _blocks = blocks ?? new List<Block>();
        }

        public bool Add(Block block)
        {
            if (!this.VerifyNewBlock(this.LastBlock, block)) return false;
            _blocks.Add(block);
            Blockchain.SaveChain(this);
            return true;
        }

        public bool VerifyNewBlock(Block prevBlock, Block newBlock)
        {
            if (newBlock.Index - 1 != prevBlock.Index) return false;
            if (prevBlock.Hash.VerifySha256(newBlock.PreviousHash)) return false;
            if (!newBlock.Hash.IsEqual(Block.HashBlock(newBlock))) return false;
            return true;
        }

        public bool ReplaceChain(Blockchain chain)
        {
            if(Blockchain.ChainValidation(chain) && chain.Length > this.Length)
            {
                Console.WriteLine("Replacing blockchain...");
                _blocks = chain.Blocks.ToList();
                return true;
            }
            return false;
        }

        public static bool ChainValidation(Blockchain chain)
        {
            if (chain.Blocks[0] != Block.GenerateGenesisBlock()) return false;
            List<Block> temporaryBlockchain = new List<Block>() { chain.Blocks[0] };
            for (int i = 1; i < chain.Length; i++)
            {
                if(chain.VerifyNewBlock(temporaryBlockchain[i-1],chain.Blocks[i]))
                {
                    temporaryBlockchain.Add(chain.Blocks[i]);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private static void SaveChain(Blockchain block)
        {
            using (Stream chainStream = File.Create(Constants.FileIO.BlockchainOutput))
            {
                BinaryFormatter chainSerializer = new BinaryFormatter();
                chainSerializer.Serialize(chainStream, block);
            }
        }

        public static bool LoadChain(out Blockchain block)
        {
            try
            {
                if (!File.Exists(Constants.FileIO.BlockchainOutput))
                {
                    block = new Blockchain();
                    return true;
                }
                using (Stream chainStream = File.OpenRead(Constants.FileIO.BlockchainOutput))
                {
                    BinaryFormatter chainDeserializer = new BinaryFormatter();
                    block = (Blockchain)chainDeserializer.Deserialize(chainStream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                block = null;
                return false;
            }
        }
    }
}
