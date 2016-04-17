using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeAddress.walletprint
{
    public class CoinDef
    {
        public string Name;
        public byte Version;

        public bool isWIFstupid;

        public CoinDef(string n, byte v)
        {
            Name = n; Version = v; isWIFstupid = false;
        }

        public CoinDef(string n, byte v, bool StupidWIFmode)
        {
            Name = n;
            Version = v;
            isWIFstupid = StupidWIFmode;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}{2}]", Name, Version, isWIFstupid ? "!" : "");
        }
    }

    // List of all stock-known Coins. Note that this is NOT done as an Enum because it's important to not be prescriptive. The system should cope with "unknown coins".
    public static class CoinLibrary
    {
        public static List<CoinDef> AllCoins()
        {
            List<CoinDef> coins = new List<CoinDef>();

            coins.Add(new CoinDef("Dogecoin", 30)); // full round-trip tested OK using Android wallet
            coins.Add(new CoinDef("Litecoin", 48)); // full round-trip tested OK, sending with Android wallet and receiving with Electrum-LTC privkey import
            coins.Add(new CoinDef("Bitcoin", 0)); // full round-trip, test Sent from Electrum

            coins.Add(new CoinDef("WorldCoin", 73)); // tested ok!

            coins.Add(new CoinDef("DigiByte", 30, true)); // Tests OK once "WIF-stupid" support was added! :)

            coins.Add(new CoinDef("UCoin", 68)); // tested OK with use of QT desktop wallet. Android wallet is super-unusual / custom
            coins.Add(new CoinDef("Dash (ex-Darkcoin)", 76)); // Tested OK using Dash Core qt wallet


            return coins;
        }
    }
}
