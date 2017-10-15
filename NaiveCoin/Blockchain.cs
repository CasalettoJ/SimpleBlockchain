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

        public Blockchain()
        {
            Console.WriteLine("Genesis block created.");
            _blocks.Add(Block.GenerateGenesisBlock());
            Blockchain.SaveChain(this);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Blocks[0].ToString());
        }

        public void Add(Block block)
        {
            _blocks.Add(block);
            Blockchain.SaveChain(this);
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
