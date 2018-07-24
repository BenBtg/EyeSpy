using EyeSpy.Service.AzureStorage.Services;
using EyeSpy.Service.Common.Abstractions;
using System;

namespace EyeSpy.Service.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ITrustedPersonsStorage trustedPersonsStorage = new AzureTrustedPersonsStorage("eyespystoragehack2018", "X1X+1gYkLzwNm8iatE098OxourmdlhJyf/zsazfsVLShK+60nxJ6+3yVgChtJ/IZ3Rkq5Da5hHKc8fZA9Qqoew==");
            var x = 0;
        }
    }
}
