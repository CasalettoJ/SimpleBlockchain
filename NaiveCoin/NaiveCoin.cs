using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveCoin
{
    public class NaiveCoin
    {
        private Blockchain _blockchain = null;

        public void Run()
        {
            if (!Initialize()) return;
            RunMenu();
        }

        private bool Initialize()
        {
            Console.WriteLine("Loading Blockchain..");
            _blockchain = null;
            return Blockchain.LoadChain(out _blockchain);
        }

        private void RunMenu()
        {
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("0: Exit");
                Console.WriteLine("1: Add New Block");
                Console.WriteLine("2: Blockchain Info");
                Console.WriteLine("3: View Block");
                while (!int.TryParse(Console.ReadLine(), out choice))
                {

                }
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        Console.WriteLine(Environment.NewLine + "Enter Data to include: ");
                        _blockchain.Add(new Block(UInt64.Parse(_blockchain.Blocks.Count.ToString()), DateTime.UtcNow, Console.ReadLine(), _blockchain.Blocks[_blockchain.Blocks.Count - 1].Hash));
                        Console.WriteLine("Block added.");
                        Console.WriteLine(Environment.NewLine + _blockchain.Blocks[_blockchain.Blocks.Count - 1].ToString());
                        break;
                    case 2:
                        Console.WriteLine(Environment.NewLine + "Blocks: " + _blockchain.Blocks.Count);
                        Console.WriteLine("Last block added: " + _blockchain.Blocks[_blockchain.Blocks.Count - 1].TimeStamp.ToLongDateString() + _blockchain.Blocks[_blockchain.Blocks.Count - 1].TimeStamp.ToLongTimeString());
                        break;
                    case 3:
                        int blockIndex = 0;
                        while (blockIndex != -1)
                        {
                            Console.WriteLine(Environment.NewLine + "Enter block index (-1 to quit): ");
                            while (!int.TryParse(Console.ReadLine(), out blockIndex)) { }
                            if (blockIndex >= 0 && blockIndex < _blockchain.Blocks.Count)
                            {
                                Console.WriteLine(Environment.NewLine + _blockchain.Blocks[blockIndex].ToString());
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
        }

    }
}
