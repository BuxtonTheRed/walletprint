# walletprint
Contains two projects - both in C#, both fully buildable with the free Community version of Visual Studio 2015, both licenced under the GNU GPLv3:

# Printer
Bulk-generate crypto currency paper wallets with total control over the print-templating.

It is based on Casascius's Bitcoin Address Utility project (https://github.com/casascius/Bitcoin-Address-Utility) and would not have been possible without having his work to build on.

# Loader
Use Block.IO's API to bulk-send cryptocoins (BTC/LTC/DOGE) to lists of addresses - intended for bulk loading of the wallet addresses created by the Printer app.

# Notes
These are two entirely separate projects and Solutions and please don't combine them. The airgap is there as a basic reassurance that "the thing which generates your private keys" is NOT "the thing that uses the internet at all". The Printer app can (and probably should) be run with no internet connection.

When the Printer generates a PDF of paper wallets, it also generates a matching log file containing the addresses to go with them (but *not* the private keys). Import that log in the Loader file and it will extract the addresses - or you can copy-paste a list of addresses from anywhere else.



Please note this is my first experience in putting code in to github and using it fully, so the odds are quite high of things being broken / messy / chaotic for a while. Constructive advice is welcome.

Reddit discussion for these projects is at https://www.reddit.com/r/walletprint

User documentation will be in the Wiki at https://www.reddit.com/r/walletprint/wiki/index
