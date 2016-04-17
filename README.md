# walletprint
Contains two projects:
*Printer*
Windows C# app to bulk-generate crypto currency paper wallets with total control over the print-templating.

*Loader*
Windows C# app which uses Block.IO's API to bulk-send cryptocoins (BTC/LTC/DOGE) to lists of addresses - intended for bulk loading of the wallet addresses created by the Printer app.

**Note:** These are two entirely separate projects and Solutions and please don't combine them. The airgap is there as a basic reassurance that "the thing which generates your private keys" is NOT "the thing that uses the internet at all". The Printer app can (and probably should) be run with no internet connection.

Please note this is my first experience in putting code in to github and using it fully, so the odds are quite high of things being broken / messy / chaotic for a while. Constructive advice is welcome.
