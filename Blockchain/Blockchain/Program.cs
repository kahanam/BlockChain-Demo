using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {

           Blockchain aaronCoin = new Blockchain();
           aaronCoin.addBlock(new Block(1, "11/11/2018", "100 Coins", new byte[0]));
           aaronCoin.addBlock(new Block(2, "11/12/2018", "150 Coins", new byte[0]));
           aaronCoin.addBlock(new Block(3, "11/13/2018", "200 Coins", new byte[0]));

            Console.WriteLine("AaronCoin Blockchain\n");

            //aaronCoin.chain[1].data = "101 Coins";
            //aaronCoin.chain[1].hash = aaronCoin.chain[1].calculateHash();

            foreach (Block b in aaronCoin.chain)
            {
                Console.WriteLine("Index: " + b.index);
                Console.WriteLine("Timestamp: " + b.timestamp);
                Console.WriteLine("Block Data: " + b.data);
                Console.WriteLine("Block Hash: " + BitConverter.ToString(b.hash).Replace("-", ""));
                Console.WriteLine("Previous Block Hash: " + BitConverter.ToString(b.previousHash).Replace("-", "") + "\n");
            }

            Console.WriteLine("Is Blockchain valid? " + aaronCoin.isChainValid());
            Console.ReadLine();
        }
    }


    class Block
    {
        HashAlgorithm alg = new SHA256Managed();

        public int index;
        public string timestamp;
        public string data;
        public byte[] hash;
        public byte[] previousHash;

        public Block(int index, string timestamp, string data, byte[] previousHash)
        {
            this.index = index;
            this.timestamp = timestamp;
            this.data = data;
            this.previousHash = previousHash;
            this.hash = calculateHash();
        }

        public byte[] calculateHash()
        {
            byte[] timeByte = Encoding.Default.GetBytes(this.timestamp);
            byte[] dataByte = Encoding.Default.GetBytes(this.data);

            byte[] hash = new byte[timeByte.Length + dataByte.Length + this.previousHash.Length];

            for (int i = 0; i < timeByte.Length; i++)
            {
                hash[i] = timeByte[i];
            }

            for (int i = 0; i < dataByte.Length; i++)
            {
                hash[i + timeByte.Length] = dataByte[i];
            }

            for (int i = 0; i < previousHash.Length; i++)
            {
                hash[i + timeByte.Length + dataByte.Length] = previousHash[i];
            }
            
            return alg.ComputeHash(hash);
        }
 
    }

    class Blockchain
    {
        public List<Block> chain;

        public Blockchain()
        {
            this.chain = new List<Block> { createGenesisBlock() };
        }

        private Block createGenesisBlock()
        {
            return new Block(0, "01/03/2009", "The Times 03/Jan/2009 Chancellor on brink of second bailout for banks", new byte[0]);
        }

        private Block getLatestBlock()
        {
            return this.chain[this.chain.Count - 1];
        }
        
        public void addBlock(Block newBlock)
        {
            newBlock.previousHash = this.getLatestBlock().hash;
            newBlock.hash = newBlock.calculateHash();
            this.chain.Add(newBlock);
        }

        public bool isChainValid()
        {
            for(int i = 1; i<this.chain.Count; i++)
            {
                Block currentBlock = this.chain[i];
                Block previousBlock = this.chain[i - 1];

                //Compare the two hashes for verification
                if (currentBlock.hash.Length == currentBlock.calculateHash().Length)
                {
                    for (int ii = 0; ii < currentBlock.hash.Length; ii++)
                    {
                        //return false if a value doesn't match
                        if (currentBlock.hash[ii] != currentBlock.calculateHash()[ii])
                        {
                            return false;
                        }

                    }
                }
                else
                {
                    return false;
                }

                //Compare the two hashes for verification
                if (currentBlock.previousHash.Length == previousBlock.hash.Length)
                {
                    for (int ii = 0; ii < currentBlock.previousHash.Length; ii++)
                    {
                        //return false if a value doesn't match
                        if (currentBlock.previousHash[ii] != previousBlock.hash[ii])
                        {
                            return false;
                        }

                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
