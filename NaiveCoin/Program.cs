﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NaiveCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            NaiveCoin coin = new NaiveCoin();
            coin.Run();
        }
    }
}
